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
using DotNetNuke.Services.Localization;
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
	/// This is the initial view of the module. It will only display the latest questions (assuming the question has not been down voted beyond the x threshold) and does not permit paging. This is simply a 'gateway' to other parts of the module (as well as a way to see the latest activity). 
	/// </summary>
	[PresenterBinding(typeof(HomePresenter))]
	public partial class Home : ModuleView<HomeModel>, IHomeView
	{

		#region Public Events

		public event EventHandler<HomeQuestionsEventArgs<HyperLink, QuestionInfo, Literal, Literal, Literal, Literal, Literal, Panel, Controls.Tags, Image>> ItemDataBound;
		public event EventHandler<HomeTagsEventArgs<TermInfo, Controls.Tags>> TagItemDataBound;
		public event EventHandler<HomeUserEventArgs<HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, Literal, Literal, Literal, Literal>> DashboardDataBound;

		#endregion

		#region Constructor

		public Home()
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
			Utils.SetHomePageMeta((CDefault)Page, ModuleContext);
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
			var pnlAnswers = (Panel) e.Item.FindControl("pnlAnswers");
			var litAnswers = (Literal)e.Item.FindControl("litAnswers");
			var litAnswersText = (Literal) e.Item.FindControl("litAnswersText");
			var litVotes = (Literal)e.Item.FindControl("litVotes");
			var dqaTags = (Controls.Tags)e.Item.FindControl("dqaTag");
			var imgAccepted = (Image) e.Item.FindControl("imgAccepted");

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

		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;
			hlAllTags.NavigateUrl = Links.ViewTags(ModuleContext);
			rptQuestions.DataSource = Model.LatestQuestions;
			rptQuestions.DataBind();
			rptTags.DataSource = Model.LatestTerms;
			rptTags.DataBind();
			pnlNoRecords.Visible = (Model.LatestQuestions.Count <= 0);

			hlMyAnswers.NavigateUrl = Links.ViewUserAnswers(ModuleContext, ModuleContext.PortalSettings.UserId);
			hlMyQuestions.NavigateUrl = Links.ViewUserQuestions(ModuleContext, ModuleContext.PortalSettings.UserId);
			hlMySubscriptions.NavigateUrl = Links.ViewUserSubscriptions(ModuleContext);
			hlPrivileges.NavigateUrl = Links.ViewPrivilege(ModuleContext, string.Empty);

			DashboardDataBound(this, new HomeUserEventArgs<HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, Literal, Literal, Literal, Literal>(headMyDashboard, ulMyDashboard, headFavoriteTags, ulFavoriteTags, litQuestionCount, litAnswerCount, litSubscriptionCount, litUserScore));
		}

		#endregion

	}
}