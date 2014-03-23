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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Components.Common;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class PrivilegePresenter : ModulePresenter<IPrivilegeView, PrivilegeModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// Returns the privilege we want to view the detail of (based on the URL). 
		/// </summary>
		private Constants.Privileges Privilege
		{
			get
			{
				var privilege = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["privilege"])) privilege = (Request.Params["privilege"]);
				if (privilege != Null.NullString)
				{
					switch (privilege.ToLower())
					{
						case "createpost" :
							return Constants.Privileges.CreatePost;
						case "flag" :
							return Constants.Privileges.Flag;
						case "voteup" :
							return Constants.Privileges.VoteUp;
						case "commenteverywhere" :
							return Constants.Privileges.CommentEverywhere;
						case "votedown" :
							return Constants.Privileges.VoteDown;
						case "retagquestion" :
							return Constants.Privileges.RetagQuestion;
						case "editquestionsandanswers" :
							return Constants.Privileges.EditQuestionsAndAnswers;
						case "createtagsynonym" :
							return Constants.Privileges.CreateTagSynonym;
						case "closequestion" :
							return Constants.Privileges.CloseQuestion;
						case "approvetagedits" :
							return Constants.Privileges.ApproveTagEdits;
						case "moderatortools" :
							return Constants.Privileges.ModeratorTools;
						case "protectquestions" :
							return Constants.Privileges.ProtectQuestions;
						case "trusted" :
							return Constants.Privileges.Trusted;
						case "removenewuser" :
							return Constants.Privileges.RemoveNewUser;
						default:
							return Constants.Privileges.ViewAll;
					}
				}
					return Constants.Privileges.ViewAll;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> PrivilegeCollection
		{
			get
			{
				return QaSettings.GetPrivilegeCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private UserScoreInfo UserScore
		{
			get
			{
				if (ModuleContext.PortalSettings.UserId > 0)
				{
					var usersScore = Controller.GetUserScore(ModuleContext.PortalSettings.UserId, ModuleContext.PortalId);
					if (usersScore != null)
					{
						return usersScore;
					}
				}
				var objUserScore = new UserScoreInfo
				{
					Message = "",
					PortalId = ModuleContext.PortalId,
					UserId = ModuleContext.PortalSettings.UserId,
					Score = 0
				};
				return objUserScore;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public PrivilegePresenter(IPrivilegeView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public PrivilegePresenter(IPrivilegeView view, IDnnqaController controller)
			: base(view)
		{
			if (view == null)
			{
				throw new ArgumentException(@"View is nothing.", "view");
			}

			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
			View.Load += ViewLoad;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		protected void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				View.Model.Privileges = (from t in PrivilegeCollection orderby t.Value ascending where t.Value > 0 select t).ToList();
				View.Model.SelectedPrivilege = PrivilegeCollection.Single(s => s.Key == Privilege.ToString());
				View.Model.CurrentUserScore = UserScore.Score;

				// now that we have the user score, let's determine the next privilege they can achieve
				var colPrivs = (from t in PrivilegeCollection where t.Value > View.Model.CurrentUserScore orderby t.Value ascending select t).ToList();
				if (colPrivs.Count > 0)
				{
					var objPriv = colPrivs.Take(1).ToList();
					foreach (var priv in objPriv)
					{
						View.Model.NextAchievablePrivilege = priv;
					}
				}
				
				View.ItemDataBound += ItemDataBound;
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
		protected void ItemDataBound(object sender, PrivilegeListEventArgs<QaSettingInfo, int, HyperLink, Literal> e)
		{
			// there is no reason to display ViewAll, it's bound to other controls in the ascx and we don't want to display a link to it in this list.
			if (e.Privilege.Key != Constants.Privileges.ViewAll.ToString())
			{
				e.PrivHyperLink.NavigateUrl = Links.ViewPrivilege(ModuleContext, e.Privilege.Key.ToLower());
				e.PrivHyperLink.Text = Localization.GetString(e.Privilege.Name, Constants.SharedResourceFileName);

				if (e.Privilege.Key == Privilege.ToString())
				{
					e.PrivHyperLink.CssClass = "privSelected";
				}

				e.PercentCompleteLiteral.Text = Utils.CalucalatePercentForDisplay(e.CurrentUserScore, e.Privilege.Value);
			}
		}

		#endregion

	}
}