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
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Controls;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Components.Common;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Web.UI.WebControls;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class QuestionPresenter : ModulePresenter<IQuestionView, QuestionModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		private int PageSize
		{
			get
			{
				var pageSize = Constants.DefaultPageSize;
				if (ModuleContext.Settings.ContainsKey(Constants.SettingAnswerPageSize))
				{
					pageSize = Convert.ToInt32(ModuleContext.Settings[Constants.SettingAnswerPageSize]);
				}

				return pageSize;
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
		/// Checks the querystring for PostID.
		/// </summary>
		public int QuestionID
		{
			get
			{
				var questionID = Null.NullInteger;
				if (!String.IsNullOrEmpty(Request.Params["id"])) questionID = Int32.Parse(Request.Params["id"]);
				return questionID;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private string Sort
		{
			get
			{
				var sort = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["sort"]))
				{
					sort = (Request.Params["sort"]);
				}
				return sort;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> ThresholdCollection
		{
			get
			{
				return QaSettings.GetOpThresholdCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private int TotalRecords { get; set; }

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

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public QuestionPresenter(IQuestionView view)
			: this(view, new DnnqaController(new SqlDataProvider())){
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public QuestionPresenter(IQuestionView view, IDnnqaController controller)
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
		private void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				if (!IsPostBack)
				{
					Controller.IncreaseViewCount(QuestionID, ModuleContext.PortalId);
					IsPostBack = true;
				}

				if (QuestionID < 1)
				{
					Response.Redirect(Globals.AccessDeniedURL("AccessDenied"), false);
					return;
				}
				var question = Controller.GetQuestion(QuestionID, ModuleContext.PortalId);

				if (question == null)
				{
					Response.Redirect(Globals.AccessDeniedURL(Localization.GetString("InvalidPost", LocalResourceFile)), false);
					return;
				}

				// NOTE: I may want to revisit this, to add link to specific postid.
				if (question.ParentId != 0)
				{
					question = Controller.GetQuestion(question.ParentId, ModuleContext.PortalId);
				}

				var questionVotes = Controller.GetPostVotes(QuestionID);

				View.Model.Privileges = PrivilegeCollection.ToList();
				View.Model.Question = question;
				View.Model.QuestionVotes = questionVotes;
				View.ItemDataBound += ItemDataBound;

				var allUserSubs = Controller.GetUserSubscriptions(ModuleContext.PortalId, ModuleContext.PortalSettings.UserId);
				var colSubs = (from t in allUserSubs where t.PostId == QuestionID select t).SingleOrDefault();
				View.SubscribeButtonMode(colSubs != null);

				View.Model.NewPost = new PostInfo();		

				var returnUrl = HttpContext.Request.RawUrl;
				if (ModuleContext.PortalSettings.UserId < 1)
				{
					if (returnUrl.IndexOf("?returnurl=") != -1)
					{
						returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?returnurl="));
					}
					returnUrl = HttpUtility.UrlEncode(returnUrl);
					returnUrl = Globals.LoginURL(returnUrl, true);
				}

				var objRemoveNewUser = PrivilegeCollection.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString());
				if (question.CreatedUserId == ModuleContext.PortalSettings.UserId)
				{
					// we would only leave false if removing ability for question authors to answer their own questions (would need to update SaveEnabled method in questions.ascx.cs)
					if ((UserScore.Score >= objRemoveNewUser.Value) || ModuleContext.IsEditable)
					{
						View.SaveEnabled(true);
					}
					else
					{
						View.SaveEnabled(false);
					}
				}
				else
				{			
					if ((UserScore.Score >= objRemoveNewUser.Value) || ModuleContext.IsEditable)
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
				}

				View.Model.ColAnswers = BindAnswers(0);

				if (View.Model.ColAnswers.Count() >= PageSize)
				{
					View.ShowAnswerArea(false);
				}
				else
				{
					var colUserAnswers = (from t in View.Model.ColAnswers where t.CreatedUserId == ModuleContext.PortalSettings.UserId select t);
					View.ShowAnswerArea(colUserAnswers.Count() <= 0);
				}

				View.Model.LoginUrl = returnUrl;
				View.Model.SortBy = Sort;

				var objScore = Controller.GetUserScore(question.CreatedUserId, ModuleContext.PortalId) ??
							   new UserScoreInfo {Score = 0};

				View.Model.QuestionAuthorScore = objScore.Score;

				View.Model.QuestionEditedUserScore = question.CreatedUserId == question.LastModifiedUserId ? View.Model.QuestionAuthorScore : Controller.GetUserScore(question.LastModifiedUserId, ModuleContext.PortalId).Score;

				if (ModuleContext.Settings.ContainsKey(Constants.SettingsFacebookAppId))
				{
					if (Convert.ToString(ModuleContext.Settings[Constants.SettingsFacebookAppId]).Length > 0)
					{
						View.Model.FacebookAppId = Convert.ToString(ModuleContext.Settings[Constants.SettingsFacebookAppId]);
					}
				}

				View.Model.EnablePlusOne = ModuleContext.Settings.ContainsKey(Constants.SettingsEnablePlusOne) && Convert.ToBoolean(ModuleContext.Settings[Constants.SettingsEnablePlusOne]);
				View.Model.EnableTwitter = ModuleContext.Settings.ContainsKey(Constants.SettingsEnableTwitter) && Convert.ToBoolean(ModuleContext.Settings[Constants.SettingsEnableTwitter]);
				View.Model.EnableLinkedIn = ModuleContext.Settings.ContainsKey(Constants.SettingsEnableLinkedIn) && Convert.ToBoolean(ModuleContext.Settings[Constants.SettingsEnableLinkedIn]);

				View.Save += Save;
				View.Subscribe += Subscribe;
				View.FlagPost += FlagPost;
				View.DeletePost += DeletePost;
				View.AcceptAnswer += AcceptAnswer;
				View.PagerChanged += PagerChanged;
				View.QuestionDataBound += QuestionDataBound;
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
		protected void QuestionDataBound(object sender, QuestionEventArgs<Literal, Voting, Literal, DnnBinaryImage, Literal, DnnBinaryImage, Literal, Controls.Tags, HtmlGenericControl, Literal, Literal, Literal, Literal, Literal, Comments, Literal, Literal> e)
		{
			var control = (Control)sender;

			e.VotingControl.CurrentPostID = QuestionID;
			e.VotingControl.QuestionID = QuestionID;
			e.VotingControl.ModContext = ModuleContext;
			e.VotingControl.TermId = -1;
			e.VotingControl.RelatedTermId = -1;

			e.TagsControl.ModContext = ModuleContext;
			e.TagsControl.ModuleTab = ModuleContext.PortalSettings.ActiveTab;
			e.TagsControl.DataSource = View.Model.Question.QaTerms(VocabularyId);
			e.TagsControl.DataBind();

			e.TitleLiteral.Text = View.Model.Question.Title.Trim();
			e.BodyLiteral.Text = Utils.ProcessDisplayPostBody(View.Model.Question.Body);

			var objUser = DotNetNuke.Entities.Users.UserController.GetUserById(ModuleContext.PortalId, View.Model.Question.CreatedUserId);
			e.AuthorImage.ImageUrl = control.ResolveUrl("~/profilepic.ashx?userid=" + objUser.UserID + "&w=" + 50 + "&h=" + 50);
			e.AuthorImage.AlternateText = objUser.DisplayName + Localization.GetString("PhotoTitle");
			e.AuthorImage.ToolTip = objUser.DisplayName + Localization.GetString("PhotoTitle");

			e.AskedLiteral.Text = Localization.GetString("asked", LocalResourceFile) + Utils.CalculateDateForDisplay(View.Model.Question.CreatedDate) + @" " + @"<br /><a href=" + Globals.UserProfileURL(objUser.UserID) + @">" + objUser.DisplayName + @"</a><span title='" + Localization.GetString("ReputationScore", Constants.SharedResourceFileName) + @"'><strong>" + View.Model.QuestionAuthorScore + @"</strong></span>";

			// because we imediately update a question's last modified date (due to content item integration), we need to allow for a small variance between the two dates
			if (View.Model.Question.LastModifiedDate >= View.Model.Question.CreatedDate.AddMinutes(1))
			{
				var objEditUser = DotNetNuke.Entities.Users.UserController.GetUserById(ModuleContext.PortalId, View.Model.Question.LastModifiedUserId);
				e.EditorImage.ImageUrl = control.ResolveUrl("~/profilepic.ashx?userid=" + objEditUser.UserID + "&w=" + 50 + "&h=" + 50);
				e.EditorImage.AlternateText = objEditUser.DisplayName + Localization.GetString("PhotoTitle");
				e.EditorImage.ToolTip = objEditUser.DisplayName + Localization.GetString("PhotoTitle");

				e.EditedLiteral.Text = Localization.GetString("edited", LocalResourceFile) + Utils.CalculateDateForDisplay(View.Model.Question.LastModifiedDate) + @" " + @"<br /><a href=" + Globals.UserProfileURL(objEditUser.UserID) + @">" + objEditUser.DisplayName + @"</a><span title='" + Localization.GetString("ReputationScore", Constants.SharedResourceFileName) + @"'><strong>" + View.Model.QuestionEditedUserScore + @"</strong></span>";
			}
			else
			{
				e.EditorImage.Visible = false;
			}

			if (View.Model.Question.Protected == false)
			{
				if ((View.Model.Question.CreatedUserId == ModuleContext.PortalSettings.UserId))
				{
					var objRemoveNewUser = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString());
					if (Utils.HasPrivilege(objRemoveNewUser, UserScore.Score, ModuleContext.IsEditable))
					{
						// the user is the question author 
						e.EditLiteral.Text = @"<li><a href='" + Links.EditPost(ModuleContext, QuestionID) + @"' id='editQuestion' title='" + Localization.GetString("editTitle", LocalResourceFile) + @"'>" + Localization.GetString("edit", LocalResourceFile) + @"</a></li>";
					}
				}
				else
				{
					var objEditQa = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.EditQuestionsAndAnswers.ToString());

					if (Utils.HasPrivilege(objEditQa, UserScore.Score, ModuleContext.IsEditable))
					{
						e.EditLiteral.Text = @"<li><a href='" + Links.EditPost(ModuleContext, QuestionID) + @"' id='editQuestion' title='" + Localization.GetString("editTitle", LocalResourceFile) + @"'>" + Localization.GetString("edit", LocalResourceFile) + @"</a></li>";
					}
					else
					{
						// see if the user has the retag option
						var objRetag = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.RetagQuestion.ToString());

						if (Utils.HasPrivilege(objRetag, UserScore.Score, ModuleContext.IsEditable))
						{
							e.EditLiteral.Text = @"<li><a href='" + Links.EditPost(ModuleContext, QuestionID) + @"' id='editQuestion' title='" + Localization.GetString("editTitle", LocalResourceFile) + @"'>" + Localization.GetString("edit", LocalResourceFile) + @"</a></li>";
						}
					}
				}

				if (ModuleContext.IsEditable)
				{
					e.DeleteLiteral.Text = @"<li><a href='#' id='deleteQuestion' title='" + Localization.GetString("deleteTitle", LocalResourceFile) + @"'>" + Localization.GetString("delete", LocalResourceFile) + @"</a></li>";
				}

				if (ModuleContext.PortalSettings.UserId > 0)
				{
					e.UnorderedList.Visible = true;
				}

				var objFlag = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.Flag.ToString());
				if (Utils.HasPrivilege(objFlag, UserScore.Score, ModuleContext.IsEditable))
				{
					var flaggedVotes = (from t in View.Model.QuestionVotes where t.VoteTypeId == (int)Constants.VoteType.FlagPost select t).ToList();
					var objUserVote = (from t in View.Model.QuestionVotes where ((t.CreatedByUserId == ModuleContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.FlagPost)) select t).SingleOrDefault();

					if (objUserVote != null)
					{
						e.FlagLiteral.Text = @"<li><a href='#' postid='" + View.Model.Question.PostId + @"' title='" + Localization.GetString("flagTitle", LocalResourceFile) + @"' class='undoFlag'>" + Localization.GetString("flag", LocalResourceFile) + @" (" + flaggedVotes.Count() + @")</a></li>";
					}
					else
					{
						e.FlagLiteral.Text = @"<li><a href='#' id='flagQuestion' title='" + Localization.GetString("flagTitle", LocalResourceFile) + @"'>" + Localization.GetString("flag", LocalResourceFile) + @" (" + flaggedVotes.Count() + @")</a></li>";
					}
				}

				var objProtect = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.ProtectQuestions.ToString());
				if (objProtect.Value > 0)
				{
					if (Utils.HasPrivilege(objProtect, UserScore.Score, ModuleContext.IsEditable))
					{
						// TODO: Remove display:none;
						e.ProtectLiteral.Text = @"<li style='display:none;'><a href='#' id='protectQuestion' title='" + Localization.GetString("protectTitle", LocalResourceFile) + @"'>" + Localization.GetString("protect", LocalResourceFile) + @"</a></li>";
					}
				}

				var objClose = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.CloseQuestion.ToString());
				if (objClose.Value > 0)
				{
					if (Utils.HasPrivilege(objClose, UserScore.Score, ModuleContext.IsEditable))
					{
						// TODO: Remove display:none;
						var closeVotes = (from t in View.Model.QuestionVotes where t.VoteTypeId == (int)Constants.VoteType.CloseQuestion select t).ToList();
						e.CloseLiteral.Text = @"<li style='display:none;'><a href='#' id='closeQuestion' title='" + Localization.GetString("closeTitle", LocalResourceFile) + @"'>" + Localization.GetString("close", LocalResourceFile) + @" (" + closeVotes.Count() + @")</a></li>";
					}
				}

			}
			//else
			//{
			//    // the way this is setup now, once a question is protected it cannot be undone
			//    var objProtect = Model.Privileges.Single(s => s.Key == Constants.Privileges.ProtectQuestions.ToString());

			//    // add below: OR user is moderator
			//    if (Model.CurrentUserScore >= objProtect.Value || Entities.Users.UserController.GetCurrentUserInfo().IsSuperUser)
			//    {
			//        litProtect.Text = @"<li><a href='#' id='protectQuestion'>" + Localization.GetString("protect", LocalResourceFile) + @"</a></li>";
			//    }	
			//}

			e.CommentControl.ModContext = ModuleContext;
			e.CommentControl.ObjPost = View.Model.Question;
			e.CommentControl.CurrentUserScore = UserScore.Score;
			e.CommentControl.Privileges = View.Model.Privileges;
			e.CommentControl.Question = View.Model.Question;
			e.CommentControl.DataBind();

			e.ACountLiteral.Text = View.Model.Question.TotalAnswers == 1
						? View.Model.Question.TotalAnswers + @" " + Localization.GetString("Answer", LocalResourceFile)
						: View.Model.Question.TotalAnswers + @" " + Localization.GetString("Answers", LocalResourceFile);

			var facebookContent = "";
			var googleContent = "";
			var twitterContent = "";
			var linkedInContent = "";

			if (ModuleContext.Settings.ContainsKey(Constants.SettingsFacebookAppId))
			{
				if (Convert.ToString(ModuleContext.Settings[Constants.SettingsFacebookAppId]).Length > 0)
				{
					facebookContent = "<li><div class=\"fb-like\" data-send=\"false\" data-width=\"46\" data-show-faces=\"false\" data-layout=\"button_count\"></div></li>";
				} 
			}

			if (ModuleContext.Settings.ContainsKey(Constants.SettingsEnablePlusOne))
			{
				if (Convert.ToBoolean(ModuleContext.Settings[Constants.SettingsEnablePlusOne]))
				{
					googleContent = "<li><g:plusone annotation=\"none\" size=\"medium\"></g:plusone></li>";
				}
			}

			if (ModuleContext.Settings.ContainsKey(Constants.SettingsEnableTwitter))
			{
			   if (Convert.ToBoolean(ModuleContext.Settings[Constants.SettingsEnableTwitter]))
				{
					twitterContent = "<li><a href=\"https://twitter.com/share\" data-lang=\"en\" data-count=\"none\" class=\"twitter-share-button\" data-size=\"small\"" + "\"></a></li>"; // target=\"_blank\" ; counturl=\"  url=\"" + View.Model.CurrentPage
				}
			}

			if (ModuleContext.Settings.ContainsKey(Constants.SettingsEnableLinkedIn))
			{
				if (Convert.ToBoolean(ModuleContext.Settings[Constants.SettingsEnableLinkedIn]))
				{
					linkedInContent = "<li><script type=\"IN/Share\"></script></li>";
				}
			}

			var socialContent = facebookContent + googleContent + twitterContent + linkedInContent;

			//var facebookLike = "<a class=\"addthis_button_facebook_like\" fb:like:layout=\"button_count\"></a>";
			//var googlePlusOne = "<a class=\"addthis_button_google_plusone\" g:plusone:size=\"medium\"></a>";
			//var twitter = "<a class=\"addthis_button_tweet\"></a>";
			//var linkedIn = "";

			//var socialContent = "<div class=\"addthis_toolbox addthis_default_style\">" + twitter + facebookLike + googlePlusOne + linkedIn + "</div>";

			e.SocialSharingLiteral.Text = socialContent;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ItemDataBound(object sender, AnswersEventArgs<Literal, PostInfo, Literal, Voting, Image, HtmlGenericControl, Literal, Literal, LinkButton, Literal, DnnBinaryImage, Comments, Literal, DnnBinaryImage> e)
		{
			var control = (Control)sender;

			e.BodyLiteral.Text = Utils.ProcessDisplayPostBody(e.ObjPost.Body);

			var objUser = DotNetNuke.Entities.Users.UserController.GetUserById(ModuleContext.PortalId, e.ObjPost.CreatedUserId);
			var authorScore = Controller.GetUserScore(e.ObjPost.CreatedUserId, ModuleContext.PortalId);

			e.UserImage.ImageUrl = control.ResolveUrl("~/profilepic.ashx?userid=" + objUser.UserID + "&w=" + 50 + "&h=" + 50);
			e.UserImage.AlternateText = objUser.DisplayName;
			e.UserImage.ToolTip = objUser.DisplayName;

			if (e.ObjPost.CreatedDate != e.ObjPost.LastModifiedDate)
			{
				var objEditUser = DotNetNuke.Entities.Users.UserController.GetUserById(ModuleContext.PortalId, e.ObjPost.LastModifiedUserId);
				var editedUserScore = Controller.GetUserScore(e.ObjPost.LastModifiedUserId, ModuleContext.PortalId);

				e.EditUserImage.ImageUrl = control.ResolveUrl("~/profilepic.ashx?userid=" + objEditUser.UserID + "&w=" + 50 + "&h=" + 50);
				e.EditUserImage.AlternateText = objEditUser.DisplayName + Localization.GetString("PhotoTitle");
				e.EditUserImage.ToolTip = objEditUser.DisplayName + Localization.GetString("PhotoTitle");

				e.EditedLiteral.Text = Localization.GetString("edited", LocalResourceFile) + Utils.CalculateDateForDisplay(e.ObjPost.LastModifiedDate) + @" " + @"<br /><a href=" + Globals.UserProfileURL(objEditUser.UserID) + @">" + objEditUser.DisplayName + @"</a><span title='" + Localization.GetString("ReputationScore", Constants.SharedResourceFileName) + @"'><strong>" + editedUserScore.Score + @"</strong></span>";
			}
			else
			{
				e.EditUserImage.Visible = false;
			}

			e.DateLiteral.Text = Localization.GetString("answered", LocalResourceFile) +
								 Utils.CalculateDateForDisplay(e.ObjPost.CreatedDate) + @" " + @" <a href=" +
								 Globals.UserProfileURL(e.ObjPost.CreatedUserId) + @">" + objUser.DisplayName +
								 @" </a><span title='" +
								 Localization.GetString("ReputationScore", Constants.SharedResourceFileName) + @"'><strong>" +
								 authorScore.Score + @"</strong></span>";

			e.Voting.CurrentPostID = e.ObjPost.PostId;
			e.Voting.QuestionID = e.ObjPost.ParentId;
			e.Voting.ModContext = ModuleContext;
			e.Voting.TermId = -1;
			e.Voting.RelatedTermId = -1;

			//Comments
			e.AnswerComment.ID = "CommentArea_" + e.ObjPost.PostId;
			e.AnswerComment.ModContext = ModuleContext;
			e.AnswerComment.ObjPost = e.ObjPost;
			e.AnswerComment.CurrentUserScore = UserScore.Score;
			e.AnswerComment.Privileges = View.Model.Privileges;
			e.AnswerComment.Question = View.Model.Question;
			e.AnswerComment.DataBind();

			var colPostVotes = (from t in Controller.GetPostVotes(e.ObjPost.PostId) where t.VoteTypeId == (int)Constants.VoteType.FlagPost select t).ToList();

			if (e.ObjPost.PostId == View.Model.Question.AnswerId)
			{
				e.AcceptedImage.Visible = true;
				e.AcceptedImage.ImageUrl = control.ResolveUrl("~/DesktopModules/DNNQA/images/accepted_large.png");
				e.AcceptedImage.ToolTip = Localization.GetString("AcceptedAnswer", LocalResourceFile);
				e.AcceptedImage.AlternateText = Localization.GetString("AcceptedAnswer", LocalResourceFile);
				e.AnswerAcceptButton.CommandArgument = e.ObjPost.PostId.ToString();
				e.AnswerAcceptButton.Visible = false;
			}
			else
			{
				// it's not the accepted answer
				e.AcceptedImage.Visible = false;
				e.AnswerAcceptButton.Visible = false;

				e.AnswerAcceptButton.CommandArgument = e.ObjPost.PostId.ToString();
				// if user is question author, they should see accept button
				if ((View.Model.Question.CreatedUserId == ModuleContext.PortalSettings.UserId) && (e.ObjPost.CreatedUserId != ModuleContext.PortalSettings.UserId))
				{
					e.AnswerAcceptButton.Visible = true;
				}
			}

			if (View.Model.Question.Protected == false)
			{
				if (e.ObjPost.CreatedUserId == ModuleContext.PortalSettings.UserId)
				{
					var objRemoveNewUser = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString());
					if (Utils.HasPrivilege(objRemoveNewUser, UserScore.Score, ModuleContext.IsEditable))
					{
						// make sure the user has the privilege to edit their own post
						e.AnswersMenu.Visible = true;
						e.AnswerEditLiteral.Text = @"<li><a href='" + Links.EditPost(ModuleContext, e.ObjPost.PostId) + @"' class='editAnswer' postid='" + e.ObjPost.PostId + @"' title='" + Localization.GetString("editATitle", LocalResourceFile) + @"'>" + Localization.GetString("edit", LocalResourceFile) + @"</a></li>";
					}
				}
				else
				{
					var objEditQa = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.EditQuestionsAndAnswers.ToString());

					if (Utils.HasPrivilege(objEditQa, UserScore.Score, ModuleContext.IsEditable))
					{
						// the user is allowed to edit any question and answer
						e.AnswerEditLiteral.Text = @"<li><a href='" + Links.EditPost(ModuleContext, e.ObjPost.PostId) + @"' class='editAnswer' postid='" + e.ObjPost.PostId + @"' title='" + Localization.GetString("editATitle", LocalResourceFile) + @"'>" + Localization.GetString("edit", LocalResourceFile) + @"</a></li>";
					}
				}

				// show delete only to moderators
				if (ModuleContext.IsEditable)
				{
					e.AnswerDelete.Text = @"<li><a href='#' postid='" + e.ObjPost.PostId + @"' title='" + Localization.GetString("deleteTitle", LocalResourceFile) + @"' class='deleteAnswer'>" + Localization.GetString("delete", LocalResourceFile) + @"</a></li>";
				}

				if (ModuleContext.PortalSettings.UserId > 0)
				{
					e.AnswersMenu.Visible = true;
				}

				var objFlag = View.Model.Privileges.Single(s => s.Key == Constants.Privileges.Flag.ToString());
				if (Utils.HasPrivilege(objFlag, UserScore.Score, ModuleContext.IsEditable))
				{
					var objUserVote = (from t in Controller.GetPostVotes(e.ObjPost.PostId) where ((t.CreatedByUserId == ModuleContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.FlagPost)) select t).SingleOrDefault();

					if (objUserVote != null)
					{
						e.AnswerFlagLiteral.Text = @"<li><a href='#' postid='" + e.ObjPost.PostId + @"' title='" + Localization.GetString("flagTitle", LocalResourceFile) + @"' class='undoFlag'>" + Localization.GetString("flag", LocalResourceFile) + @" (" + colPostVotes.Count() + @")</a></li>";
					}
					else
					{
						e.AnswerFlagLiteral.Text = @"<li><a href='#' postid='" + e.ObjPost.PostId + @"' title='" + Localization.GetString("flagTitle", LocalResourceFile) + @"' class='flagAnswer'>" + Localization.GetString("flag", LocalResourceFile) + @" (" + colPostVotes.Count() + @")</a></li>";
					}	

					//e.AnswerFlagLiteral.Text = @"<li><a href='#' postid='" + e.ObjPost.PostId + @"' title='" + Localization.GetString("flagTitle", LocalResourceFile) + @"' class='flagAnswer'>" + Localization.GetString("flag", LocalResourceFile) + @" (" + colPostVotes.Count() + @")</a></li>";

				}

			}
			//else
			//{
			//    // the question is protected
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Save(object sender, AddAnswerEventArgs<PostInfo> e) {
			try 
			{
				e.NewPost.PortalId = ModuleContext.PortalId;
				e.NewPost.ModuleID = ModuleContext.ModuleId;
				e.NewPost.Body = Utils.ProcessSavePostBody(e.NewPost.Body);
				// We will always have a parent here that is not 0. 
				e.NewPost.ParentId = QuestionID;
				e.NewPost.Approved = true;
				e.NewPost.ApprovedDate = DateTime.Now;

				e.NewPost.ContentItemId = View.Model.Question.ContentItemId;
				e.NewPost.TabID = ModuleContext.TabId;
				e.NewPost.CreatedUserId = ModuleContext.PortalSettings.UserId;
				e.NewPost.CreatedDate = DateTime.Now;

				// make sure title/body are passing through security filters
				var objSecurity = new PortalSecurity();
				e.NewPost.Body = objSecurity.InputFilter(e.NewPost.Body, PortalSecurity.FilterFlag.NoScripting);

				e.NewPost = Controller.AddPost(e.NewPost, ModuleContext.TabId);

				var objScoreLog = new UserScoreLogInfo
				{
					UserId = ModuleContext.PortalSettings.UserId,
					PortalId = ModuleContext.PortalId,
					UserScoringActionId = (int)Constants.UserScoringActions.ProvidedAnswer,
					KeyId = e.NewPost.PostId,
					Score =
						UserScoringCollection.Single(
							s => s.Key == Constants.UserScoringActions.ProvidedAnswer.ToString()).Value,
					CreatedOnDate = DateTime.Now
				};

				Controller.AddScoringLog(objScoreLog, PrivilegeCollection);

				// if we ever allow moderation/approval to be enabled, this needs to respect that.
				var cntJournal = new Journal();
				var questionUrl =Links.ViewQuestion(QuestionID, View.Model.Question.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings);
				cntJournal.AddAnswerToJournal(e.NewPost, View.Model.Question.Title, ModuleContext.PortalId, ModuleContext.PortalSettings.UserId, questionUrl);
				Response.Redirect(questionUrl, true);
			} 
		catch (Exception exception) {
				ProcessModuleLoadException(exception);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Subscribe(object sender, EventArgs e)
		{
			var objButton = (LinkButton)sender;

			switch (objButton.CommandName)
			{
				case "unsubscribe":
					Controller.DeleteUserPostSubscription(ModuleContext.PortalSettings.UserId, QuestionID);
					View.SubscribeButtonMode(false);
					break;
				default :
					var objSubscription = new SubscriptionInfo
											  {
												  CreatedOnDate = DateTime.Now,
												  PortalId = ModuleContext.PortalId,
												  UserId = ModuleContext.PortalSettings.UserId,
												  PostId = QuestionID,
												  SubscriptionType = (int) Constants.SubscriptionType.InstantPost,
												  TermId = -1,
												  Name = View.Model.Question.Title
											  };

					Controller.AddSubscription(objSubscription);
					View.SubscribeButtonMode(true);
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>If we are changing the accepted answer, for now we are ok with multiple authors getting credit for an accepted answer BUT we only want to allow a single user to be rewarded 1x per answer. </remarks>
		protected void AcceptAnswer(object sender, AcceptAnswerEventArgs<int, int, int> e)
		{
			var objPost = Controller.GetPost(e.PostId, ModuleContext.PortalId);

			if (objPost != null)
			{
				Controller.AcceptAnswer(e.PostId, e.UserId, DateTime.Now, ModuleContext.ModuleId);

				var colUserScoring = QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
				if (View.Model.Question.AnswerId > 0)
				{
					// these prevent anyone for getting points 2x for the same answer (although multiple answer authors could be credited over the liftime of a question since an accepted answer can change). 
					Controller.DeleteUserScoreLog(objPost.CreatedUserId, ModuleContext.PortalId,
																	  (int)Constants.UserScoringActions.ProvidedAcceptedAnswer, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.ProvidedAcceptedAnswer.ToString()).Value,
																	  e.PostId);
					Controller.DeleteUserScoreLog(ModuleContext.PortalSettings.UserId, ModuleContext.PortalId,
																	  (int)Constants.UserScoringActions.AcceptedQuestionAnswer, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.AcceptedQuestionAnswer.ToString()).Value,
																	  e.PostId);
				}

				var objAScoreLog = new UserScoreLogInfo
				{
					UserId = objPost.CreatedUserId,
					PortalId = ModuleContext.PortalId,
					UserScoringActionId = (int)Constants.UserScoringActions.ProvidedAcceptedAnswer,
					KeyId = e.PostId,
					Score =
						colUserScoring.Single(
							s => s.Key == Constants.UserScoringActions.ProvidedAcceptedAnswer.ToString()).Value,
					CreatedOnDate = DateTime.Now
				};

				Controller.AddScoringLog(objAScoreLog, PrivilegeCollection);

				var objScoreLog = new UserScoreLogInfo
				{
					UserId = ModuleContext.PortalSettings.UserId,
					PortalId = ModuleContext.PortalId,
					UserScoringActionId = (int)Constants.UserScoringActions.AcceptedQuestionAnswer,
					KeyId = e.PostId,
					Score =
						colUserScoring.Single(
							s => s.Key == Constants.UserScoringActions.AcceptedQuestionAnswer.ToString()).Value,
					CreatedOnDate = DateTime.Now
				};

				Controller.AddScoringLog(objScoreLog, PrivilegeCollection);
			}
			
			//TODO: CP - Journal Integration once accepted answer is in build.'
			Response.Redirect(Links.ViewQuestion(QuestionID, View.Model.Question.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void FlagPost(object sender, FlagPostEventArgs<ModerationLogInfo> e)
		{
			e.ModerationLog.PortalId = ModuleContext.PortalId;
			e.ModerationLog.CreatedByUserId = ModuleContext.PortalSettings.UserId;
			e.ModerationLog.CreatedOnDate = DateTime.Now;

			var objVote = new VoteInfo
							  {
								  PostId = e.ModerationLog.PostId,
								  VoteTypeId = (int) Constants.VoteType.FlagPost,
								  KeyID = QuestionID,
								  CreatedByUserId = ModuleContext.PortalSettings.UserId,
								  PortalId = ModuleContext.PortalId,
								  CreatedOnDate = e.ModerationLog.CreatedOnDate
							  };
			e.ModerationLog.VoteId = Controller.AddVote(objVote, ModuleContext.ModuleId);

			//Controller.AddModLog(e.ModerationLog);

			if (e.ModerationLog.VoteId > 0)
			{
				FlagThresholdCheck(e.ModerationLog.PostId, QuestionID, View.Model.Question.ContentItemId);
			}
			
			View.Model.ColAnswers = BindAnswers(View.Model.CurrentPage);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DeletePost(object sender, DeletePostEventArgs<ModerationLogInfo> e)
		{
			try
			{
				//e.ModerationLog.PortalId = ModuleContext.PortalId;
				//e.ModerationLog.CreatedByUserId = ModuleContext.PortalSettings.UserId;
				//e.ModerationLog.CreatedOnDate = DateTime.Now;
				//Controller.AddModLog(e.ModerationLog);

				//perform post soft delete
				var objEntry = Controller.GetPost(e.ModerationLog.PostId, ModuleContext.PortalId);
				if (objEntry != null)
				{
					Controller.DeletePost(e.ModerationLog.PostId, objEntry.ParentId, ModuleContext.PortalId, objEntry.ContentItemId, true, ModuleContext.ModuleId);
					Response.Redirect(
						e.ModerationLog.PostId == View.Model.Question.PostId
							? Links.Home(ModuleContext.TabId)
							: Links.ViewQuestion(View.Model.Question.PostId, ModuleContext.TabId, ModuleContext.PortalSettings),
						false);
					//System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
				}
			}
			catch (Exception exception)
			{
				ProcessModuleLoadException(exception);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void PagerChanged(object sender, PagerChangedEventArgs<LinkButton, string> e)
		{
			View.Model.CurrentPage += Convert.ToInt32(e.LinkButton.CommandArgument);
			View.Model.ColAnswers = BindAnswers(View.Model.CurrentPage);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Based on sorting, we return the collection of answers to display to the end user (we also set any paging buttons here too).
		/// </summary>
		/// <param name="currentPage"></param>
		/// <returns></returns>
		public List<PostInfo> BindAnswers(int currentPage)
		{
			var objSort = new SortInfo { Column = "votes", Direction = Constants.SortDirection.Descending };

			if (Sort != Null.NullString)
			{
				switch (Sort.ToLower())
				{
					case "oldest":
						objSort.Column = "oldest";
						objSort.Direction = Constants.SortDirection.Ascending;
						break;
					case "active":
						objSort.Column = "active";
						break;
					//case "votes":
					//    objSort.Column = "votes";
					//    break;
					default:
						objSort.Column = "votes";
						break;
				}
			}

			var colAnswers = Controller.GetAnswers(QuestionID, ModuleContext.PortalId);
			TotalRecords = colAnswers.Count();

			var answers = Sorting.GetAnswerCollection(PageSize, currentPage, objSort, colAnswers).ToList();        
			var totalPages = Convert.ToDouble((double)TotalRecords / PageSize);

			if ((totalPages > 1) && (totalPages > currentPage + 1))
			{
				View.ShowNextButton(true);
			}
			else
			{
				View.ShowNextButton(false);
			}

			if ((totalPages > 1) && (currentPage > 0))
			{
				View.ShowBackButton(true);
			}
			else
			{
				View.ShowBackButton(false);
			}

			return answers;
		}

		/// <summary>
		/// This will check if the flagged vote put us over the threshold to complete the flagging process (soft delete on the post, assign reputation points, clearing cache, etc.)
		/// </summary>
		/// <param name="currentPostId"></param>
		/// <param name="questionId"></param>
		/// <param name="contentItemId"></param>
		/// <remarks>Always delete the post last, in case we move to hard deletes. We need to know the deleted post info to log scoring actions.</remarks>
		private void FlagThresholdCheck(int currentPostId, int questionId, int contentItemId)
		{
			var votes = (from t in Controller.GetPostVotes(currentPostId) where (t.VoteTypeId == (int)Constants.VoteType.FlagPost) select t).ToList();
			var objFlagCompleteThreshold = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.PostFlagCompleteCount.ToString());

			if ((votes.Count) >= objFlagCompleteThreshold.Value)
			{
				var objPost = Controller.GetPost(currentPostId, ModuleContext.PortalId);

				if (currentPostId != questionId)
				{
					var objScoreLogApprove = new UserScoreLogInfo
					{
						UserId = objPost.CreatedUserId,
						PortalId = ModuleContext.PortalId,
						UserScoringActionId = (int)Constants.UserScoringActions.ProvidedFlaggedAnswer,
						Score =
							UserScoringCollection.Single(
								s => s.Key == Constants.UserScoringActions.ProvidedFlaggedAnswer.ToString()).Value,
						KeyId = currentPostId,
						CreatedOnDate = DateTime.Now
					};

					Controller.AddScoringLog(objScoreLogApprove, PrivilegeCollection);
					Controller.DeletePost(currentPostId, questionId, ModuleContext.PortalId, contentItemId, true, ModuleContext.ModuleId);
				}
				else
				{
					var objScoreLogApprove = new UserScoreLogInfo
					{
						UserId = objPost.CreatedUserId,
						PortalId = ModuleContext.PortalId,
						UserScoringActionId = (int)Constants.UserScoringActions.AskedFlaggedQuestion,
						Score =
							UserScoringCollection.Single(
								s => s.Key == Constants.UserScoringActions.AskedFlaggedQuestion.ToString()).Value,
						KeyId = questionId,
						CreatedOnDate = DateTime.Now
					};

					Controller.AddScoringLog(objScoreLogApprove, PrivilegeCollection);
					Controller.DeletePost(currentPostId, 0, ModuleContext.PortalId, contentItemId, true, ModuleContext.ModuleId);
				}
			}
		}

		#endregion

	}
}