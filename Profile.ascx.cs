//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2012
// by DotNetNuke Corporation
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

using System;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.Web.UI.WebControls;
using WebFormsMvp;

namespace DotNetNuke.DNNQA {

	[PresenterBinding(typeof(ProfilePresenter))]
	public partial class Profile : ProfileModuleViewBase<ProfileModel>, IProfileView
	{

		#region Constructor

		public Profile()
		{
			AutoDataBind = false;
		}

		#endregion

		#region IProfileModule Members

		public override bool DisplayModule
		{
			get { return true; }
		}

		#endregion

		#region Public Events

		public event EventHandler<ProfileQuestionsEventArgs<QuestionInfo, Literal, HyperLink>> ItemDataBound;

		public event EventHandler<ProfileFriendsEventArgs<UserScoreInfo, HyperLink, DnnBinaryImage, Literal>> VsItemDataBound;

		public event EventHandler<ProfileReputationEventArgs<UserScoreLogInfo, Literal, HyperLink>> RepItemDataBound;

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Framework.jQuery.RequestUIRegistration();

		}

		/// <summary>
		/// Handles formatting of individual items in the questions repeater.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptQuestionsItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item) return;
			var question = (QuestionInfo)e.Item.DataItem;
			var litAnswers = (Literal)e.Item.FindControl("litAnswers");
			var hlTitle = (HyperLink)e.Item.FindControl("hlTitle");

			ItemDataBound(this, new ProfileQuestionsEventArgs<QuestionInfo, Literal, HyperLink>(question, litAnswers, hlTitle));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptFriendsItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item) return;
			var userScore = (UserScoreInfo)e.Item.DataItem ?? new UserScoreInfo();
			var hlUser = (HyperLink)e.Item.FindControl("hlUser");
			var dbiUser = (DnnBinaryImage) e.Item.FindControl("dbiUser");
			var litDetails = (Literal) e.Item.FindControl("litDetails");

			VsItemDataBound(this, new ProfileFriendsEventArgs<UserScoreInfo, HyperLink, DnnBinaryImage, Literal>(userScore, hlUser, dbiUser, litDetails));
		}

		protected void RptReputationItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.AlternatingItem && e.Item.ItemType != ListItemType.Item) return;
			var scoreLog = (UserScoreLogInfo)e.Item.DataItem;
			var litPoints = (Literal)e.Item.FindControl("litPoints");
			var hlTitle = (HyperLink)e.Item.FindControl("hlTitle");

			RepItemDataBound(this, new ProfileReputationEventArgs<UserScoreLogInfo, Literal, HyperLink>(scoreLog, litPoints, hlTitle));
		}

		#endregion
		
		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh() {
			if  (Model.ColQuestions.Count <= 0)
			{
				pnlNoQuestions.Visible = true;
				litNoQuestions.Text = Localization.GetString(Model.IsProfileUser ? "PersonalNoQuestions" : "NoQuestions", LocalResourceFile);
			}
			else
			{
				pnlNoQuestions.Visible = false;
				rptQuestions.DataSource = Model.ColQuestions;
				rptQuestions.DataBind();
			}

			if (Model.ColAnswers.Count <= 0)
			{
				pnlNoAnswers.Visible = true;
				litNoAnswers.Text = Localization.GetString(Model.IsProfileUser ? "PersonalNoAnswers" : "NoAnswers", LocalResourceFile);
			}
			else
			{
				pnlNoAnswers.Visible = false;
				rptAnswers.DataSource = Model.ColAnswers;
				rptAnswers.DataBind();
			}

			if (Model.CanViewMyRep)
			{
				pnlMyReputation.Visible = true;
				liRep.Visible = true;

				if (Model.ProfileUserRep.Count > 0)
				{
					pnlNoRep.Visible = false;
					rptReputation.DataSource = Model.ProfileUserRep;
					rptReputation.DataBind();
				}
				else
				{
					pnlNoRep.Visible = true;
					litNoRep.Text = Localization.GetString(Model.IsProfileUser ? "PersonalNoReputation" : "NoReputation", LocalResourceFile);
				}
			}
			else
			{
				pnlMyReputation.Visible = false;
				liRep.Visible = false;
			}

			if (Model.CanViewFriendsVs)
			{
				pnlFriendsVs.Visible = true;
				liVs.Visible = true;
				if (!Model.HasFriends)
				{
					pnlNoFriends.Visible = true;
					litNoFriends.Text = Localization.GetString("NoFriends", LocalResourceFile);
				}
				else
				{
					pnlNoFriends.Visible = false;
					rptVs.DataSource = Model.CompetingFriends;
					rptVs.DataBind();
				}
			}
			else
			{
				pnlFriendsVs.Visible = false;
				liVs.Visible = false;
			}
		}

		#endregion

	}
}