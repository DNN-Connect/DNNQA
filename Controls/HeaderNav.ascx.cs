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
using System.ComponentModel;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;
using DotNetNuke.UI.Utilities;

namespace DotNetNuke.DNNQA.Controls
{

	public partial class HeaderNav : System.Web.UI.UserControl
	{

		#region Private Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		private string ControlView
		{
			get
			{
				var controlView = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["view"]))
				{
					controlView = (Request.Params["view"]);
				}
				return controlView;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private string Keyword
		{
			get
			{
				var content = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["keyword"])) content = (Request.Params["keyword"]);
				var objSecurity = new PortalSecurity();

				return objSecurity.InputFilter(content, PortalSecurity.FilterFlag.NoSQL);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private string MyResourceFile
		{
			get { return TemplateSourceDirectory + ResourceFile; }
		}

		/// <summary>
		/// 
		/// </summary>
		private bool Unanswered
		{
			get
			{
				var response = false;
				if (!String.IsNullOrEmpty(Request.Params["unanswered"]))
				{
					var unanswered = (Request.Params["unanswered"]);
					if (unanswered == "true")
					{
						response = true;
					}
				}

				return response;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> PrivilegeCollection
		{
			get
			{
				return QaSettings.GetPrivilegeCollection(Controller.GetQaPortalSettings(ModContext.PortalId), ModContext.PortalId);
			}
		}

		/// <summary>
		/// Path to the resource file for localization.
		/// </summary>
		/// <remarks>It is necessary to manually set localized text in code in user controls, otherwise keys would need to be created in host control's resource file.</remarks>
		private const string ResourceFile = "/App_LocalResources/HeaderNav.ascx.resx";

		/// <summary>
		/// 
		/// </summary>
		private UserScoreInfo UserScore
		{
			get
			{
				if (ModContext.PortalSettings.UserId > 0)
				{
					var usersScore = Controller.GetUserScore(ModContext.PortalSettings.UserId, ModContext.PortalId);
					if (usersScore != null)
					{
						return usersScore;
					}
				}
				var objUserScore = new UserScoreInfo
				{
					Message = "",
					PortalId = ModContext.PortalId,
					UserId = ModContext.PortalSettings.UserId,
					Score = 0
				};
				return objUserScore;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> UserScoringCollection
		{
			get
			{
				return QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModContext.PortalId), ModContext.PortalId);
			}
		}

		#endregion

		#region Public Properties

		[Browsable(false)]
		public ModuleInstanceContext ModContext { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public HeaderNav()
			: this(new DnnqaController(new SqlDataProvider()))
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="controller"></param>
		public HeaderNav(IDnnqaController controller)
		{
			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			imgSearch.Click += OnSearchClick;

			AwardMessaging();
			
			if (Page.IsPostBack) return;
			BindNavigation();

			if (Keyword.Length > 0)
			{
				txtSearch.Text = Keyword;
			}
			else
			{
				txtSearch.Attributes.Add("defaultvalue", Localization.GetString("overlaySearch", MyResourceFile));
			}

			Framework.AJAX.RegisterPostBackControl(imgSearch);
			ClientAPI.RegisterKeyCapture(txtSearch, imgSearch, 13);		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnSearchClick(object sender, EventArgs e)
		{
			var search = txtSearch.Text.Trim();

			if (search.Length > 1)
			{
				Response.Redirect(Links.KeywordSearch(ModContext, search));
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// This will bind the text and navigation URL's for the links. When finished, it will call the DetermineVisibility method.
		/// </summary>
		private void BindNavigation()
		{
			hlHome.Text = Localization.GetString("hlHome", MyResourceFile);
			hlHome.NavigateUrl = Links.Home(ModContext.TabId);

			hlQuestions.Text = Localization.GetString("hlQuestions", MyResourceFile);
			hlQuestions.NavigateUrl = Links.ViewQuestions(ModContext);

			hlTags.Text = Localization.GetString("hlTags", MyResourceFile);
			hlTags.NavigateUrl = Links.ViewTags(ModContext);

			hlBadges.Text = Localization.GetString("hlBadges", MyResourceFile);
			hlBadges.NavigateUrl = Links.ViewBadges(ModContext);

			hlUnanswered.Text = Localization.GetString("hlUnanswered", MyResourceFile);
			hlUnanswered.NavigateUrl = Links.ViewUnansweredQuestions(ModContext, 1, "");

			hlAskQuestion.Text = Localization.GetString("hlAskQuestion", MyResourceFile);
			hlAskQuestion.NavigateUrl = Links.AskQuestion(ModContext);

			imgSearch.ImageUrl = ResolveUrl("~/DesktopModules/DNNQA/images/searchButton_bg.png");
			imgSearch.AlternateText = Localization.GetString("overlaySearch", MyResourceFile);
			imgSearch.ToolTip = Localization.GetString("overlaySearch", MyResourceFile);

			DetermineVisibility();

			if (ModContext.PortalSettings.UserId > 0)
			{
				if (UserScore.Score == 0)
				{
					var objScoreLog = new UserScoreLogInfo
										  {
											  UserId = ModContext.PortalSettings.UserId,
											  PortalId = ModContext.PortalId,
											  UserScoringActionId = (int) Constants.UserScoringActions.FirstLoggedInView,
											  Score = UserScoringCollection.Single(
							s => s.Key == Constants.UserScoringActions.ProvidedAnswer.ToString()).Value,
											  CreatedOnDate = DateTime.Now
											};
					Controller.AddScoringLog(objScoreLog, PrivilegeCollection);		    
				}
			}
		}

		/// <summary>
		/// Determines which links should be displayed to the end user.
		/// </summary>
		private void DetermineVisibility()
		{
			//if (ModContext.PortalSettings.UserId <= 0)
			//{
			//    hlMyDashboard.Visible = false;
			//}

			switch (ControlView.ToLower())
			{
				case "termsynonyms" :
					hlTags.CssClass = "qaNavSel";
					break;
				case "question":
					hlQuestions.CssClass = "qaNavSel";
					break;
				case "browse":
					if (Unanswered)
					{
						hlUnanswered.CssClass = "qaNavSel";
					}
					else
					{
						hlQuestions.CssClass = "qaNavSel";
					}
					
					if (Keyword.Length > 0)
					{
						txtSearch.Text = Keyword;
					}
					
					break;
				case "ask":
					//hlAskQuestion.CssClass = "qaNavSel";
					break;
				case "tags":
					hlTags.CssClass = "qaNavSel";
					break;
				case "termdetail":
					hlTags.CssClass = "qaNavSel";
					break;
				case "subscriptions":
					break;
				case "termhistory":
					hlTags.CssClass = "qaNavSel";
					break;
				case "editterm":
					hlTags.CssClass = "qaNavSel";
					break;
				case "posthistory":
					break;
				case "privileges":
					break;
				case "badges":
					hlBadges.CssClass = "qaNavSel";
					break;
				case "badge" :
					hlBadges.CssClass = "qaNavSel";
					break;
				default:
					hlHome.CssClass = "qaNavSel";
					break;
			}
		}

		/// <summary>
		/// This method checks to see if the currently logged in user has gained a privilege since the last time the page was loaded. 
		/// </summary>
		/// <remarks>This should be run on every page load.</remarks>
		private void AwardMessaging()
		{
			if (ModContext.PortalSettings.UserId > 0)
			{
				if (UserScore.Message.Length > 0)
				{
					int num;
					var message = "";
					var isNum = int.TryParse(UserScore.Message, out num);

					if (isNum)
					{
						var actionId = Convert.ToInt32(UserScore.Message);
						var usersAction = (Constants.UserScoringActions)actionId;
						var objScoringAction = UserScoringCollection.Single(s => s.Key == usersAction.ToString());

						if (objScoringAction.Value > 0)
						{						
							message = Localization.GetString("ReputationPoints", MyResourceFile);
							var actionName = Localization.GetString(usersAction.ToString(), Constants.SharedResourceFileName);

							message = message.Replace("{0}", objScoringAction.Value.ToString());
							message = message.Replace("{1}", actionName);
						}
					}
					else
					{
						message = Localization.GetString("NewPrivilege", MyResourceFile);
						// we store the key in the message table
						var objPriv = PrivilegeCollection.Single(s => s.Key == UserScore.Message);
						var privText = Localization.GetString(objPriv.Name, Constants.SharedResourceFileName);
						var privDesc = Localization.GetString(objPriv.Description, Constants.SharedResourceFileName);
						var privLink = Links.ViewPrivilege(ModContext, objPriv.Key);

						if (objPriv.Key != Constants.Privileges.CreatePost.ToString())
						{
							var cntJournal = new Journal();
							cntJournal.AddPrivilegeToJournal(objPriv, privText, privDesc, ModContext.PortalId, ModContext.TabId, ModContext.PortalSettings.UserId, privLink);
						}

						message = message.Replace("{0}", privText);
						message = message.Replace("{1}", privLink);
					}
								
					litMessage.Text = message;

					// clear the message from the db
					Controller.ClearUserScoreMessage(UserScore.UserId, ModContext.PortalId);
				}
			}
		}

		#endregion

	}
}