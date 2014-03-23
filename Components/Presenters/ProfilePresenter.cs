//
// Copyright (c) 2010
// by Will Morgenweck & Chris Paterra
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Users.Social;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Web.UI.WebControls;

namespace DotNetNuke.DNNQA.Components.Presenters {

	/// <summary>
	/// 
	/// </summary>
	public class ProfilePresenter : ModulePresenter<IProfileView, ProfileModel> {

		#region Private Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public int ProfileUserId
		{
			get
			{
				var profileUserId = Null.NullInteger;
				if (!string.IsNullOrEmpty(Request.Params["UserId"]))
				{
					profileUserId = Int32.Parse(Request.Params["UserId"]);
				}
				return profileUserId;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private UserScoreInfo ProfileUserScore
		{
			get
			{
				return Controller.GetUserScore(ProfileUserId, ModuleContext.PortalId);
			}
		}

		private UserInfo ProfileUser
		{
			get
			{
				var cntUsers = new UserController();
				return cntUsers.GetUser(ModuleContext.PortalId, ProfileUserId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> UserScoringCollection
		{
			get
			{
				return QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			}
		}

		/// <summary>
		/// TODO: Tie this to a module setting.
		/// </summary>
		private int VocabularyId
		{
			get { return 1; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public ProfilePresenter(IProfileView view)
			: this(view, new DnnqaController(new SqlDataProvider())) {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ProfilePresenter(IProfileView view, IDnnqaController controller)
			: base(view) {
			if (view == null) {
				throw new ArgumentException(@"View is nothing.", "view");
			}

			if (controller == null) {
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
		}

		#endregion
		
		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		//protected void ViewLoad(object sender, EventArgs eventArgs)
		protected override void OnLoad() 
		{
				base.OnLoad();
			try
			{
				var objSort = new SortInfo { Column = "active", Direction = Constants.SortDirection.Descending };

				var colUserQuestions = Controller.GetUserQuestions(ModuleContext.PortalId, ProfileUserId);
				var colQuestions = Sorting.GetKeywordSearchCollection(10, 0, objSort, colUserQuestions).ToList();
				View.Model.ColQuestions = colQuestions;
				
				var colUserAnswers = Controller.GetUserAnswers(ModuleContext.PortalId, ProfileUserId);
				var colAnswers = Sorting.GetKeywordSearchCollection(10, 0, objSort, colUserAnswers).ToList();
				View.Model.ColAnswers = colAnswers;

				var colCompetingFriends = new List<UserScoreInfo>();   
				var friendsRelationship = RelationshipController.Instance.GetFriendsRelationshipByPortal(ModuleContext.PortalId);
				var friends = ProfileUser.Social.UserRelationships.Where(ur => ur.RelationshipId == friendsRelationship.RelationshipId);
				View.Model.HasFriends = friends.Count() > 0;
				
				if (View.Model.HasFriends)
				{
					var colPortalScores = Controller.GetUserScoresByPortal(ModuleContext.PortalId);
					var objTopFriend = (from t in colPortalScores where (t.Score >= ProfileUserScore.Score) && (friends.Select(y => y.UserId).Contains(t.UserId) && (t.UserId != ProfileUserId)) orderby t.Score ascending select t).Take(1).SingleOrDefault();
					UserScoreInfo objBottomFriend;

					if (objTopFriend != null)
					{
						colCompetingFriends.Add(objTopFriend);
						objBottomFriend = (from t in colPortalScores where (t.Score <= ProfileUserScore.Score) && (friends.Select(y => y.UserId).Contains(t.UserId) && (t.UserId != ProfileUserId) && (t.UserId != objTopFriend.UserId)) orderby t.Score descending select t).Take(1).SingleOrDefault();
					}
					else
					{
						objBottomFriend = (from t in colPortalScores where (t.Score <= ProfileUserScore.Score) && (friends.Select(y => y.UserId).Contains(t.UserId) && (t.UserId != ProfileUserId)) orderby t.Score descending select t).Take(1).SingleOrDefault();
					}

					if (ProfileUserScore != null)
					{
						colCompetingFriends.Add(ProfileUserScore);
					}

					if (objBottomFriend != null)
					{
						colCompetingFriends.Add(objBottomFriend);
					}

					View.Model.CompetingFriends = colCompetingFriends;
				}

				var isFriend = (ModuleContext.PortalSettings.UserInfo != null) && (ModuleContext.PortalSettings.UserInfo.Social.Friend != null) || (ModuleContext.PortalSettings.UserId== ProfileUserId);

				View.Model.CanViewFriendsVs = isFriend;
				View.Model.CanViewMyRep = isFriend;
				View.Model.IsProfileUser = ModuleContext.PortalSettings.UserId == ProfileUserId;

				var col = Controller.GetUserScoreLog(ProfileUserId, ModuleContext.PortalId);
				View.Model.ProfileUserRep = Sorting.GetUserRepCollection(10, col).ToList();


				View.ItemDataBound += ItemDataBound;
				View.VsItemDataBound += VsItemDataBound;
				View.RepItemDataBound += RepItemDataBound;

				View.Refresh();
			}
			catch (Exception exc)
			{
				ProcessModuleLoadException(exc);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ItemDataBound(object sender, ProfileQuestionsEventArgs<QuestionInfo, Literal, HyperLink> e)
		{
			//var control = (Control)sender;
			e.AnswerCountLiteral.Text = e.QuestionInfo.TotalAnswers.ToString();

			var tc = new TabController();
			var objTab = tc.GetTab(e.QuestionInfo.TabID, ModuleContext.PortalSettings.PortalId, false);
			e.QuestionTitleLink.NavigateUrl = Links.ViewQuestion(e.QuestionInfo.ParentId, e.QuestionInfo.Title, objTab, ModuleContext.PortalSettings);
			e.QuestionTitleLink.Text = e.QuestionInfo.Title;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void VsItemDataBound(object sender, ProfileFriendsEventArgs<UserScoreInfo, HyperLink, DnnBinaryImage, Literal> e)
		{
			var control = (Control)sender;

			if (e.UserScoreInfo.UserId > 0)
			{
				var objUser = UserController.GetUserById(ModuleContext.PortalId, e.UserScoreInfo.UserId);

				if (objUser != null)
				{
					e.UserBinaryImage.ImageUrl = control.ResolveUrl("~/profilepic.ashx?userid=" + objUser.UserID + "&w=" + 50 + "&h=" + 50);
					e.UserHyperLink.NavigateUrl = DotNetNuke.Common.Globals.UserProfileURL(objUser.UserID);
					e.DetailsLiteral.Text = objUser.DisplayName + " has " + e.UserScoreInfo.Score + " reputation points";
				}
			}
		}

		protected void RepItemDataBound(object sender, ProfileReputationEventArgs<UserScoreLogInfo, Literal, HyperLink> e)
		{
			//var control = (Control)sender;
			e.AnswerCountLiteral.Text = e.UserScoreLogInfo.Score.ToString();

			var actionId = e.UserScoreLogInfo.UserScoringActionId;
			var usersAction = (Constants.UserScoringActions)actionId;
			var objScoringAction = UserScoringCollection.Single(s => s.Key == usersAction.ToString());

			//e.QuestionTitleLink.NavigateUrl = Links.ViewQuestion(e.UserScoreLogInfo.PostId, e.QuestionInfo.TabID, ModuleContext.PortalSettings);

			e.QuestionTitleLink.Text = Localization.GetString(objScoringAction.Name, Constants.SharedResourceFileName);

		}

		#endregion

	}
}