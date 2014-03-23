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
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Framework;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.UI.Utilities;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(TagListPresenter))]
	public partial class TagList : ModuleView<TagListModel>, ITagListView
	{

		#region Public Events

		public event EventHandler<TagListEventArgs<Literal, Literal, TermInfo, Controls.Tags, Literal>> ItemDataBound;
		public event EventHandler<PagerChangedEventArgs<LinkButton, string>> PagerChanged;
		public event EventHandler<TagSearchEventArgs<string>> TagFiltered;

		#endregion

		#region Constructor

		public TagList()
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

			ClientAPI.RegisterKeyCapture(txtFilter, cmdFilter, 13);
			Utils.SetTagsPageMeta((CDefault)Page, ModuleContext, Model.PageTitle, Model.PageDescription, Model.PageLink, Model.PrevPageLink, Model.NextPageLink);
		}

		/// <summary>
		/// Handles formatting of individual items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptTagsItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var litDescription = (Literal)e.Item.FindControl("litDescription");
				var litCount = (Literal)e.Item.FindControl("litCount");
				var term = (TermInfo)e.Item.DataItem;
				var dqaTags = (Controls.Tags)e.Item.FindControl("dqaTag");
				var litStats = (Literal) e.Item.FindControl("litStats");

				ItemDataBound(this, new TagListEventArgs<Literal, Literal, TermInfo, Controls.Tags, Literal>(litDescription, litCount, term, dqaTags, litStats));
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
			var filter = txtFilter.Text;
			PagerChanged(sender, new PagerChangedEventArgs<LinkButton, string>(linkButton, filter));

			rptTags.DataSource = Model.TopTags;
			rptTags.DataBind();

			cmdMore.CommandArgument = (Model.CurrentPage + 1).ToString();
			cmdBack.CommandArgument = (Model.CurrentPage - 1).ToString();

			if (filter.Trim() != "")
			{
				hlTagPopular.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, filter, "popular", Model.CurrentPage);
				hlTagName.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, filter, "name", Model.CurrentPage);
				hlTagLatest.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, filter, "newest", Model.CurrentPage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void CmdOnSearchClick(object sender, EventArgs e)
		{
			var filter = txtFilter.Text;
			TagFiltered(sender, new TagSearchEventArgs<string>(filter));

			rptTags.DataSource = Model.TopTags;
			rptTags.DataBind();

			hlTagPopular.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, filter, "popular", Model.CurrentPage);
			hlTagName.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, filter, "name", Model.CurrentPage);
			hlTagLatest.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, filter, "newest", Model.CurrentPage);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;
			rptTags.DataSource = Model.TopTags;
			rptTags.DataBind();

			pnlNoRecords.Visible = (Model.TopTags.Count <= 0);

			switch (Model.SortBy.ToLower())
			{
				case "name":
					hlTagName.CssClass = "qaSortSel";
					break;
				case "newest":
					hlTagLatest.CssClass = "qaSortSel";
					break;
				default:
					hlTagPopular.CssClass = "qaSortSel";
					break;
			}

			if (Model.Filter != Null.NullString)
			{
				hlTagPopular.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, Model.Filter, "popular", Model.CurrentPage);
				hlTagName.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, Model.Filter, "name", Model.CurrentPage);
				hlTagLatest.NavigateUrl = Links.ViewTagsSortedAndFiltered(ModuleContext, Model.Filter, "newest", Model.CurrentPage);
			}
			else
			{
				hlTagPopular.NavigateUrl = Links.ViewTagsSorted(ModuleContext, "popular", Model.CurrentPage);
				hlTagName.NavigateUrl = Links.ViewTagsSorted(ModuleContext, "name", Model.CurrentPage);
				hlTagLatest.NavigateUrl = Links.ViewTagsSorted(ModuleContext, "newest", Model.CurrentPage);
			}

			if (Page.IsPostBack) return;
			txtFilter.Text = Model.Filter;
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