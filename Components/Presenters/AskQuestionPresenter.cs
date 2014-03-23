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

using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.Common;
using System;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class AskQuestionPresenter : ModulePresenter<IAskQuestionView, AskQuestionModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// The tag we want to associate the question with automatically (based on a parameter in the URL). 
		/// </summary>
		private string Tag
		{
			get
			{
				var tag = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["term"])) tag = (Request.Params["term"]);
				var objSecurity = new PortalSecurity();

				return objSecurity.InputFilter(tag, PortalSecurity.FilterFlag.NoSQL);
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

		/// <summary>
		/// TODO: Tie this to a module setting.
		/// </summary>
		private int VocabularyId
		{
			get { return 1; }
		}

		#endregion
		
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public AskQuestionPresenter(IAskQuestionView view) : this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public AskQuestionPresenter(IAskQuestionView view, IDnnqaController controller) : base(view)
		{
			if (view == null) {
				throw new ArgumentException(@"View is nothing.", "view");
			}

			if (controller == null) {
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
		private void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				var returnUrl = HttpContext.Request.RawUrl;
				var redirect = false;

				if (ModuleContext.PortalSettings.UserId < 1)
				{		
					if (returnUrl.IndexOf("?returnurl=") != -1)
					{
						returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?returnurl="));
					}
					returnUrl = HttpUtility.UrlEncode(returnUrl);
					returnUrl = Globals.LoginURL(returnUrl, true);
					redirect = true;
				}

				if (Tag.Trim().Length > 0)
				{
					View.Model.SelectedTags = Tag + @", ";
				}

				var objRemoveNewUser = PrivilegeCollection.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString());
				if (UserScore.Score >= objRemoveNewUser.Value || ModuleContext.IsEditable)
				{
					View.SaveEnabled(true);
				}
				else
				{
					var objLastPost = Controller.GetUsersLastPost(ModuleContext.PortalSettings.UserId, ModuleContext.PortalId);
					if (objLastPost != null)
					{
						View.SaveEnabled(objLastPost.CreatedDate <= DateTime.Now.AddMinutes(-1));
					}
					else
					{
						View.SaveEnabled(true);
					}

				}

				View.Save += Save;
				View.Refresh();

				if (redirect)
				{
					Response.Redirect(returnUrl, true);
				}
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
		protected void Save(object sender, AskQuestionEventArgs<PostInfo, bool, string> e)
		{
			try
			{
				var control = (System.Web.UI.UserControl) sender;

				if (e.Tags.Trim() == "")
				{
					var minMsg = Localization.GetString("SelectMinTag", LocalResourceFile);
					//var minCount = 1;
					//minMsg = minMsg.Replace("{0}", minCount.ToString());

					UI.Skins.Skin.AddModuleMessage(control, minMsg, ModuleMessage.ModuleMessageType.RedError);
					return;
				}

				if (ModuleContext.Settings.ContainsKey(Constants.SettingMaxQuestionTags))
				{
					var enteredTerms = e.Tags.Trim().Split(',').ToList();

					if (enteredTerms.Count > Convert.ToInt32(ModuleContext.Settings[Constants.SettingMaxQuestionTags]))
					{
						var maxMsg = Localization.GetString("SelectMoreTags", LocalResourceFile);
						maxMsg = maxMsg.Replace("{0}", (ModuleContext.Settings[Constants.SettingMaxQuestionTags]).ToString());

						UI.Skins.Skin.AddModuleMessage(control, maxMsg, ModuleMessage.ModuleMessageType.RedError);
						return;
					}
				}

				// TODO: validate selected tags are not in the disallow collection (module setting, loop through current collection and look for disallowed tags if found show message below)
				//UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("InvalidTag", SharedResourceFile), ModuleMessage.ModuleMessageType.RedError);

				if (ModuleContext.Settings.ContainsKey(Constants.SettingMinBodyChars))
				{
					if (e.Post.Content.Length > Convert.ToInt32(ModuleContext.Settings[Constants.SettingMaxQuestionTags]))
					{
						UI.Skins.Skin.AddModuleMessage(control, Localization.GetString("InvalidBody", LocalResourceFile),
										ModuleMessage.ModuleMessageType.RedError);
						return;
					}
				}

				var userEnteredTerms = e.Tags.Split(',')
					.Where(s => s != string.Empty)
					.Select(p => p.Trim())
					.Distinct().ToList();

				foreach (var s in userEnteredTerms)
				{
					if (!Utils.ContainsSpecialCharacter(s)) continue;
					var msg = Localization.GetString("UnAllowedCharacters", LocalResourceFile);
					msg = msg.Replace("{0}", Constants.DisallowedCharacters);
					UI.Skins.Skin.AddModuleMessage(control, msg, ModuleMessage.ModuleMessageType.RedError);
					return;
				}

				var terms = new List<Term>();
				userEnteredTerms.ForEach(t => terms.Add(Terms.CreateAndReturnTerm(t, VocabularyId)));

				var colOpThresholds = QaSettings.GetOpThresholdCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
				var objTermApprove = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TermSynonymApproveCount.ToString());
				var portalSynonyms = Controller.GetTermSynonyms(ModuleContext.PortalId);
				var postTerms = new List<Term>();

				foreach (var term in terms)
				{
					var matchedSynonym = (from t in portalSynonyms where t.RelatedTermId == term.TermId && t.Score >= objTermApprove.Value select t).SingleOrDefault();
					if (matchedSynonym != null)
					{
						var masterTerm = Terms.GetTermById(matchedSynonym.MasterTermId, VocabularyId);
						// we have to make sure the masterTerm is not already in the list of terms
						if (!terms.Contains(masterTerm))
						{
							postTerms.Add(masterTerm);
							// update replaced count (for synonym)
							Controller.TermSynonymReplaced(matchedSynonym.RelatedTermId, ModuleContext.PortalId);
						}
						//else
						//{
						//    // show it was removed?				
						//}	
					}
					else
					{
						postTerms.Add(term);
					}
				}

				e.Post.Terms.AddRange(postTerms);
				
				e.Post.PortalId = ModuleContext.PortalId;
				e.Post.ModuleID = ModuleContext.ModuleId;
				e.Post.Body = Utils.ProcessSavePostBody(e.Post.Body);

				// remove line below if you want to enable post approval (changes in ascx too)
				e.Post.Approved = true;

				if (e.Post.Approved)
				{
					e.Post.ApprovedDate = DateTime.Now;
				}

				e.Post.ParentId = 0;
				e.Post.TabID = ModuleContext.TabId;
				e.Post.CreatedUserId = ModuleContext.PortalSettings.UserId;
				e.Post.CreatedDate = DateTime.Now;
				e.Post.LastModifiedUserId = ModuleContext.PortalSettings.UserId;
				e.Post.LastModifiedDate = DateTime.Now;

				// make sure title/body are passing through security filters
				var objSecurity = new PortalSecurity();
				e.Post.Title = objSecurity.InputFilter(e.Post.Title, PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoAngleBrackets | PortalSecurity.FilterFlag.NoMarkup);
				e.Post.Body = objSecurity.InputFilter(e.Post.Body, PortalSecurity.FilterFlag.NoScripting);

				var objPost = Controller.AddPost(e.Post, ModuleContext.TabId);
				Controller.UpdatePost(objPost, ModuleContext.TabId);

				// subscribe user
				if (e.Notify)
				{
					var objSub = new SubscriptionInfo
										{
											CreatedOnDate = DateTime.Now,
											PortalId = ModuleContext.PortalId,
											PostId = objPost.PostId,
											SubscriptionType = (int) Constants.SubscriptionType.InstantPost,
											UserId = ModuleContext.PortalSettings.UserId
										};

					Controller.AddSubscription(objSub);
				}

				var colUserScoring = QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
				var objScoreLog = new UserScoreLogInfo
										{
											UserId = ModuleContext.PortalSettings.UserId,
											PortalId = ModuleContext.PortalId,
											UserScoringActionId = (int) Constants.UserScoringActions.AskedQuestion,
											Score =
												colUserScoring.Single(
													s => s.Key == Constants.UserScoringActions.AskedQuestion.ToString()).Value,
											CreatedOnDate = DateTime.Now
										};

				Controller.AddScoringLog(objScoreLog, PrivilegeCollection);

				// if we ever allow moderation/approval to be enabled, this needs to respect that.
				var cntJournal = new Journal();
				var questionUrl = Links.ViewQuestion(objPost.PostId, objPost.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings);

				cntJournal.AddQuestionToJournal(objPost, objPost.Title, ModuleContext.PortalId, ModuleContext.PortalSettings.UserId, questionUrl);
				Response.Redirect(questionUrl, false);
			}
			catch (Exception exception)
			{
				ProcessModuleLoadException(exception);
			}
		}

		#endregion
	
	}
}