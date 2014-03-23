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
using System.Linq;
using System.Web.UI.HtmlControls;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Controls;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Mvp;
using DotNetNuke.Web.UI.WebControls;
using WebFormsMvp;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Entities;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// This is the question detail view. It displays the original question and then a collection of answers (can be paged). It also provides an area for the user to provide an answer to the question as well as provides the user interface for post 'moderation' type tasks to be performed.
	/// </summary>
	[PresenterBinding(typeof(QuestionPresenter))]
	public partial class Question : ModuleView<QuestionModel>, IQuestionView
	{

		#region Members

// ReSharper disable InconsistentNaming
		protected UI.UserControls.TextEditor teContent;
// ReSharper restore InconsistentNaming

		/// <summary>
		/// 
		/// </summary>
		public int LastAnswerPostId
		{
			get
			{
				if (Model.ColAnswers.Count > 0)
				{
					return (from p in Model.ColAnswers
							orderby p.CreatedDate descending
							select p).FirstOrDefault().PostId;
				}
				return 0;
			}
		}

		#endregion

		#region Public Events

		public event EventHandler<QuestionEventArgs<Literal, Voting, Literal, DnnBinaryImage, Literal, DnnBinaryImage, Literal, Controls.Tags, HtmlGenericControl, Literal, Literal, Literal, Literal, Literal, Comments, Literal, Literal>> QuestionDataBound;
		public event EventHandler<AddAnswerEventArgs<PostInfo>> Save;
		public event EventHandler Subscribe;
		public event EventHandler<FlagPostEventArgs<ModerationLogInfo>> FlagPost;
		public event EventHandler<DeletePostEventArgs<ModerationLogInfo>> DeletePost;
		public event EventHandler<AcceptAnswerEventArgs<int, int, int>> AcceptAnswer;
		public event EventHandler<AnswersEventArgs<Literal, PostInfo, Literal, Voting, Image, HtmlGenericControl, Literal, Literal, LinkButton, Literal, DnnBinaryImage, Comments, Literal, DnnBinaryImage>> ItemDataBound;
		public event EventHandler<PagerChangedEventArgs<LinkButton, string>> PagerChanged;

		#endregion

		#region Constructor

		public Question()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);
			Utils.SetQuestionPageMeta((CDefault)Page, Model.Question, ModuleContext);

			//ClientResourceManager.RegisterScript(Page, "http://s7.addthis.com/js/250/addthis_widget.js#domready"); //pubid=

			if (Model.EnableLinkedIn)
			{
				ClientResourceManager.RegisterScript(Page, "https://platform.linkedin.com/in.js");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdSaveClick(object sender, EventArgs e)
		{
			var content = teContent.Text.Trim();

			// TODO: integrate module setting here for min char count.
			if (content.Length > 2)
			{
				Model.NewPost = new PostInfo { Body = teContent.Text };
				Save(this, new AddAnswerEventArgs<PostInfo>(Model.NewPost));
			}
			else
			{
				UI.Skins.Skin.AddModuleMessage(this, "Invalid body", ModuleMessage.ModuleMessageType.RedError);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdSubscribeClick(object sender, EventArgs e)
		{
			Subscribe(sender, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdAcceptClick(object sender, EventArgs e)
		{
			var button = (LinkButton) sender;
			var postId = Convert.ToInt32(button.CommandArgument);
			AcceptAnswer(this, new AcceptAnswerEventArgs<int, int, int>(postId, ModuleContext.PortalSettings.UserId, Model.Question.PostId));

			//// force rebind
			//rptAnswers.DataSource = Model.ColAnswers;
			//rptAnswers.DataBind();

			//cmdMore.CommandArgument = "1";
			//cmdBack.CommandArgument = "0";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdFlagQuestionClick(object sender, EventArgs e)
		{
			// verify the user can still flag a question (operation threshold, what they have done today, etc.)
			var objModeration = new ModerationLogInfo
									{
										ModLogTypeId = Convert.ToInt32(rblstFlagQuestion.SelectedValue),
										PostId = Model.Question.PostId
									};

			if (objModeration.ModLogTypeId == (int)Constants.ModerationLogType.FlagOther)
			{
				objModeration.Notes = txtFlagQuestionOther.Text;
			}

			FlagPost(this, new FlagPostEventArgs<ModerationLogInfo>(objModeration));

			Response.Redirect(Links.ViewQuestion(Model.Question.PostId, Model.Question.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings), true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdDeleteQuestionClick(object sender, EventArgs e)
		{
			// verify the user can still flag a question (operation threshold, what they have done today, etc.)
			var objModeration = new ModerationLogInfo
									{
										ModLogTypeId = Convert.ToInt32(rblstDeleteQuestion.SelectedValue),
										PostId = Model.Question.PostId
									};

			if (objModeration.ModLogTypeId == (int)Constants.ModerationLogType.DeleteOther)
			{
				objModeration.Notes = txtDeleteQuestionOther.Text;
			}

			DeletePost(this, new DeletePostEventArgs<ModerationLogInfo>(objModeration));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdFlagAnswerClick(object sender, EventArgs e)
		{
			// verify the user can still flag a question (operation threshold, what they have done today, etc.)
			if (tempId.Text.Trim() != string.Empty)
			{
				var objModeration = new ModerationLogInfo
										{
											ModLogTypeId = Convert.ToInt32(rblstAnswerFlag.SelectedValue),
											PostId = Convert.ToInt32(tempId.Text)
										};

				if (objModeration.ModLogTypeId == (int)Constants.ModerationLogType.FlagOther)
				{
					objModeration.Notes = txtFlagQuestionOther.Text;
				}

				FlagPost(this, new FlagPostEventArgs<ModerationLogInfo>(objModeration));

				rptAnswers.DataSource = Model.ColAnswers;
				rptAnswers.DataBind();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdUndoFlagClick(object sender, EventArgs e)
		{
			if (tempId.Text.Trim() != string.Empty)
			{
				var objModeration = new ModerationLogInfo
										{
											ModLogTypeId = (int) Constants.ModerationLogType.RemoveFlag,
											PostId = Convert.ToInt32(tempId.Text)
										};

				FlagPost(this, new FlagPostEventArgs<ModerationLogInfo>(objModeration));

				if (Model.Question.PostId == objModeration.PostId)
				{
					// undoing a flagged question
					Response.Redirect(Links.ViewQuestion(Model.Question.PostId, Model.Question.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings), true);
				}
				else
				{
					// undoing a flagged answer
					rptAnswers.DataSource = Model.ColAnswers;
					rptAnswers.DataBind();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdDeleteAnswerClick(object sender, EventArgs e)
		{
			// verify the user can still flag a question (operation threshold, what they have done today, etc.)
			if (tempId.Text.Trim() != string.Empty)
			{
				var objModeration = new ModerationLogInfo
										{
											ModLogTypeId = Convert.ToInt32(rblstAnswerDelete.SelectedValue),
											PostId = Convert.ToInt32(tempId.Text)
										};

				if (objModeration.ModLogTypeId == (int)Constants.ModerationLogType.DeleteOther)
				{
					objModeration.Notes = txtDeleteQuestionOther.Text;
				}

				DeletePost(this, new DeletePostEventArgs<ModerationLogInfo>(objModeration));
			}
		}

		/// <summary>
		/// Handles formatting of individual items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptAnswersItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item ||  e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var body = (Literal)e.Item.FindControl("litBody");
				var question = (PostInfo)e.Item.DataItem;
				var date = (Literal)e.Item.FindControl("litDate");
				var answervoting = (Voting)e.Item.FindControl("dqaAnswerVote");
				var imgAccepted = (Image) e.Item.FindControl("imgAccepted");
				
				var ulAnswersMenu = (HtmlGenericControl)e.Item.FindControl("ulAnswersMenu");
				var litEditAnswer = (Literal)e.Item.FindControl("litEditAnswer");
				var litFlagAnswer = (Literal)e.Item.FindControl("litFlagAnswer");
				var cmdAccept = (LinkButton)e.Item.FindControl("cmdAccept");
				var litDeleteAnswer = (Literal)e.Item.FindControl("litDeleteAnswer");
				var dbiAUser = (DnnBinaryImage) e.Item.FindControl("dbiAUser");
				var edited = (Literal)e.Item.FindControl("litAEdited");
				var dbiAEditUser = (DnnBinaryImage)e.Item.FindControl("dbiAEditUser");
				var qaAnswerComment = (Comments)e.Item.FindControl("qaAnswerComment");

				ItemDataBound(this, new AnswersEventArgs<Literal, PostInfo, Literal, Voting, Image, HtmlGenericControl, Literal, Literal, LinkButton, Literal, DnnBinaryImage, Comments, Literal, DnnBinaryImage>(body, question, date, answervoting, imgAccepted, ulAnswersMenu, litEditAnswer, litFlagAnswer, cmdAccept, litDeleteAnswer, dbiAUser, qaAnswerComment, edited, dbiAEditUser));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdPagingClick(object sender, EventArgs e)
		{
			var linkButton = (LinkButton)sender;
			PagerChanged(sender, new PagerChangedEventArgs<LinkButton, string>(linkButton, ""));

			rptAnswers.DataSource = Model.ColAnswers;
			rptAnswers.DataBind();

			cmdMore.CommandArgument = (Model.CurrentPage + 1).ToString();
			cmdBack.CommandArgument = (Model.CurrentPage - 1).ToString();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;
			hlLogin.NavigateUrl = Model.LoginUrl;
			BindListItems();

			QuestionDataBound(this, new QuestionEventArgs<Literal, Voting, Literal, DnnBinaryImage, Literal, DnnBinaryImage, Literal, Controls.Tags, HtmlGenericControl, Literal, Literal, Literal, Literal, Literal, Comments, Literal, Literal>(litTitle, dqaQuestionVote, litBody, dbiUser, litAsked, dbiEditUser, litEdited, dqaTag, questionMenu, litEditQuestion, litFlagQuestion, litCloseQuestion, litProtect, litDeleteQuestion, qaQuestionComment, litAnswerCount, litSocialSharing));

			//var obj = Model.Privileges.Single(s => s.Key == Constants.Privileges.Flag.ToString());

			// tie to user
			litQInform.Text = @"<span>" + 10 + @"</span> " +  Localization.GetString("RemainingFlags", LocalResourceFile);
			litAInform.Text = @"<span>" + 10 + @"</span> " +  Localization.GetString("RemainingFlags", LocalResourceFile);

			rptAnswers.DataSource = Model.ColAnswers;
			rptAnswers.DataBind();

			divLogin.Visible = ModuleContext.PortalSettings.UserId < 1;
			switch (Model.SortBy.ToLower())
			{
				case "oldest":
					hlOldest.CssClass = "qaSortSel";
					break;
				case "active":
					hlActive.CssClass = "qaSortSel";
					break;
				//case "votes":
				//    hlVotes.CssClass = "qaSortSel";
				//    break;
				default: // votes
					hlVotes.CssClass = "qaSortSel";
					break;
			}

			hlOldest.NavigateUrl = Links.ViewQuestionSorted(ModuleContext, Model.Question.PostId, "oldest");
			hlActive.NavigateUrl = Links.ViewQuestionSorted(ModuleContext, Model.Question.PostId, "active");
			hlVotes.NavigateUrl = Links.ViewQuestion(Model.Question.PostId, Model.Question.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings);

			if (Page.IsPostBack) return;
			cmdMore.CommandArgument = "1";
			cmdBack.CommandArgument = "0";
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enabled"></param>
		public void SaveEnabled(bool enabled)
		{
			if (!enabled)
			{
				//if (Model.Question.CreatedUserId == ModuleContext.PortalSettings.UserId)
				//{
				//    UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("AnswerOwnQuestion", LocalResourceFile), ModuleMessage.ModuleMessageType.YellowWarning);
				//}
				//else
				//{
					UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("NoRemoveNewUser", LocalResourceFile), ModuleMessage.ModuleMessageType.YellowWarning);
				//}
				cmdSave.Enabled = false;
			}
		}

		/// <summary>
		/// Determines the mode the 'subscribe' button will be setup for (this is based on the user being subscribed to the question)
		/// </summary>
		/// <param name="subscribed"></param>
		public void SubscribeButtonMode(bool subscribed)
		{
			if (subscribed)
			{
				cmdSubscribe.Text = Localization.GetString("cmdUnsubscribe", LocalResourceFile);
				cmdSubscribe.CommandName = "unsubscribe";
			}
			else
			{
				cmdSubscribe.Text = Localization.GetString("cmdSubscribe", LocalResourceFile);
				cmdSubscribe.CommandName = "subscribe";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowAnswerArea(bool show)
		{
			divAddAnswer.Visible = ModuleContext.PortalSettings.UserId > 0;

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowBackButton(bool show)
		{
			cmdBack.Visible = show;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowNextButton(bool show)
		{
			cmdMore.Visible = show;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// This localizes text that is bound to radio button lists. 
		/// </summary>
		private void BindListItems()
		{
			rblstFlagQuestion.Items.Clear();
			rblstDeleteQuestion.Items.Clear();
			rblstAnswerFlag.Items.Clear();
			rblstAnswerDelete.Items.Clear();

			var liLow = new ListItem
						{
							Text = Localization.GetString("LowQuality", LocalResourceFile),
							Value = ((int)Constants.ModerationLogType.FlagLowQuality).ToString()
						};

			rblstFlagQuestion.Items.Insert(0, liLow);
			rblstAnswerFlag.Items.Insert(0, liLow);

			liLow.Value = ((int) Constants.ModerationLogType.DeleteLowQuality).ToString();

			rblstDeleteQuestion.Items.Insert(0, liLow);
			rblstAnswerDelete.Items.Insert(0, liLow);
			
			var liSpam = new ListItem
						{
							Text = Localization.GetString("Spam", LocalResourceFile),
							Value = ((int)Constants.ModerationLogType.FlagSpam).ToString()
						};

			rblstFlagQuestion.Items.Insert(1, liSpam);
			rblstAnswerFlag.Items.Insert(1, liSpam);

			liSpam.Value = ((int)Constants.ModerationLogType.FlagOutOfContext).ToString();

			rblstDeleteQuestion.Items.Insert(1, liSpam);
			rblstAnswerDelete.Items.Insert(1, liSpam);

			var liBelong = new ListItem
							   {
								   Text = Localization.GetString("DoesntBelong", LocalResourceFile),
								   Value = ((int)Constants.ModerationLogType.DeleteOutOfContext).ToString()
							   };

			rblstFlagQuestion.Items.Insert(2, liBelong);		
			rblstAnswerFlag.Items.Insert(2, liBelong);

			liBelong.Value = ((int)Constants.ModerationLogType.DeleteSpam).ToString();

			rblstDeleteQuestion.Items.Insert(2, liBelong);
			rblstAnswerDelete.Items.Insert(2, liBelong);

			var liOther = new ListItem
							  {
								  Text = Localization.GetString("Other", LocalResourceFile),
								  Value = ((int)Constants.ModerationLogType.FlagOther).ToString()
							  };

			rblstFlagQuestion.Items.Insert(3, liOther);
			rblstAnswerFlag.Items.Insert(3, liOther);

			liOther.Value = ((int)Constants.ModerationLogType.DeleteOther).ToString();

			rblstDeleteQuestion.Items.Insert(3, liOther);
			rblstAnswerDelete.Items.Insert(3, liOther);

			rblstFlagQuestion.SelectedIndex = 0;
			rblstAnswerFlag.SelectedIndex = 0;
			rblstDeleteQuestion.SelectedIndex = 0;
			rblstAnswerDelete.SelectedIndex = 0;
		}

		#endregion
	
	}
}