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
using System.Web.UI.HtmlControls;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Framework;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Entities;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(BrowsePresenter))]
	public partial class Browse : ModuleView<BrowseModel>, IBrowseView
	{

		#region Public Events

		public event EventHandler<HomeQuestionsEventArgs<HyperLink, QuestionInfo, Literal, Literal, Literal, Literal, Literal, Panel, Controls.Tags, Image>> ItemDataBound;
		public event EventHandler<HomeTagsEventArgs<TermInfo, Controls.Tags>> TagItemDataBound;
		public event EventHandler<PagerChangedEventArgs<LinkButton, string>> PagerChanged;
		public event EventHandler<HomeUserEventArgs<HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, Literal, Literal, Literal, Literal>> DashboardDataBound;

		#endregion

		#region Constructor

		public Browse()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Utils.SetBrowsePageMeta((CDefault)Page, ModuleContext, Model.PageTitle, Model.PageDescription, Model.PageLink, Model.PrevPageLink, Model.NextPageLink);
		}

		/// <summary>
		/// Handles formatting of individual items in the questions repeater.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptQuestionsItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			var link = (HyperLink)e.Item.FindControl("hlTitle");
			var question = (QuestionInfo)e.Item.DataItem;
			var litDate = (Literal)e.Item.FindControl("litDate");
			var litViews = (Literal)e.Item.FindControl("litViews");
			var pnlAnswers = (Panel)e.Item.FindControl("pnlAnswers");
			var litAnswers = (Literal)e.Item.FindControl("litAnswers");
			var litAnswersText = (Literal)e.Item.FindControl("litAnswersText");
			var litVotes = (Literal)e.Item.FindControl("litVotes");
			var dqaTags = (Controls.Tags)e.Item.FindControl("dqaTag");
			var imgAccepted = (Image)e.Item.FindControl("imgAccepted");

			ItemDataBound(this, new HomeQuestionsEventArgs<HyperLink, QuestionInfo, Literal, Literal, Literal, Literal, Literal, Panel, Controls.Tags, Image>(link, question, litDate, litViews, litAnswers, litVotes, litAnswersText, pnlAnswers, dqaTags, imgAccepted));
		}

		/// <summary>
		/// Handles formatting of individual items in the tags repeater.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptTagsItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			var tagControl = (Controls.Tags)e.Item.FindControl("dqaSingleTag");
			var term = (TermInfo)e.Item.DataItem;

			TagItemDataBound(this, new HomeTagsEventArgs<TermInfo, Controls.Tags>(term, tagControl));
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

			rptQuestions.DataSource = Model.ColQuestions;
			rptQuestions.DataBind();

			cmdMore.CommandArgument = (Model.CurrentPage + 1).ToString();
			cmdBack.CommandArgument = (Model.CurrentPage - 1).ToString();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh() {
			dgqHeaderNav.ModContext = ModuleContext;
			litTitle.Text = Model.HeaderTitle;

			if (Model.SelectedTerm != null)
			{
				hlDetail.NavigateUrl = Model.TagDetailUrl;
				hlEdit.NavigateUrl = Links.EditTag(ModuleContext, ModuleContext.TabId, Model.SelectedTerm.Name);
				pnlTag.Visible = true;
			}

			rptQuestions.DataSource = Model.ColQuestions;
			rptQuestions.DataBind();

			pnlNoRecords.Visible = (Model.ColQuestions.Count <= 0);

			switch (Model.SortBy.ToLower())
			{
				case "newest":
					hlNewest.CssClass = "qaSortSel";
					break;
				case "votes":
					hlVotes.CssClass = "qaSortSel";
					break;
				default: // active
					hlActive.CssClass = "qaSortSel";
					break;
			}

			if (Model.AppliedFilters.Count > 0)
			{
				if (Model.AppliedKeyword != null)
				{
					hlNewest.NavigateUrl = Links.KeywordSearchSorted(ModuleContext, Model.AppliedKeyword, "newest");
					hlVotes.NavigateUrl = Links.KeywordSearchSorted(ModuleContext, Model.AppliedKeyword, "votes");
					hlActive.NavigateUrl = Links.KeywordSearch(ModuleContext, Model.AppliedKeyword);
				}
				else if (Model.SelectedTerm != null)
				{
					hlNewest.NavigateUrl = Links.ViewTaggedQuestionsSorted(ModuleContext, Model.SelectedTerm.Name, "newest");
					hlVotes.NavigateUrl = Links.ViewTaggedQuestionsSorted(ModuleContext, Model.SelectedTerm.Name, "votes");
					hlActive.NavigateUrl = Links.ViewTaggedQuestions(Model.SelectedTerm.Name, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings);
				}
				else if (Model.AppliedUser > 0)
				{
					// check for answers or questions
					if (Model.AppliedType == "questions")
					{
						hlNewest.NavigateUrl = Links.ViewUserQuestionsSorted(ModuleContext, Model.AppliedUser, "newest");
						hlVotes.NavigateUrl = Links.ViewUserQuestionsSorted(ModuleContext, Model.AppliedUser, "votes");
						hlActive.NavigateUrl = Links.ViewUserQuestions(ModuleContext, Model.AppliedUser);
					}
					else
					{
						hlNewest.NavigateUrl = Links.ViewUserAnswersSorted(ModuleContext, Model.AppliedUser, "newest");
						hlVotes.NavigateUrl = Links.ViewUserAnswersSorted(ModuleContext, Model.AppliedUser, "votes");
						hlActive.NavigateUrl = Links.ViewUserAnswers(ModuleContext, Model.AppliedUser);
					}
				}
				else
				{
					// no special filters
					hlNewest.NavigateUrl = Links.ViewQuestionsSorted(ModuleContext, "newest", Model.ApplyUnanswered, Model.CurrentPage);
					hlVotes.NavigateUrl = Links.ViewQuestionsSorted(ModuleContext, "votes", Model.ApplyUnanswered, Model.CurrentPage);
					hlActive.NavigateUrl = Model.ApplyUnanswered ? Links.ViewUnansweredQuestions(ModuleContext, Model.CurrentPage, "") : Links.ViewQuestions(ModuleContext);
				}
			}

			rptTags.DataSource = Model.RelatedTags;
			rptTags.DataBind();

			hlMyAnswers.NavigateUrl = Links.ViewUserAnswers(ModuleContext, ModuleContext.PortalSettings.UserId);
			hlMyQuestions.NavigateUrl = Links.ViewUserQuestions(ModuleContext, ModuleContext.PortalSettings.UserId);
			hlMySubscriptions.NavigateUrl = Links.ViewUserSubscriptions(ModuleContext);
			hlPrivileges.NavigateUrl = Links.ViewPrivilege(ModuleContext, string.Empty);

			DashboardDataBound(this, new HomeUserEventArgs<HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, Literal, Literal, Literal, Literal>(headMyDashboard, ulMyDashboard, headFavoriteTags, ulFavoriteTags, litQuestionCount, litAnswerCount, litSubscriptionCount, litUserScore));

			if (Page.IsPostBack) return;
			cmdMore.CommandArgument = "1";
			cmdBack.CommandArgument = "0";
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

	}
}