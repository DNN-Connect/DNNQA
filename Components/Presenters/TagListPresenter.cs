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
using System.Web.UI.WebControls;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class TagListPresenter : ModulePresenter<ITagListView, TagListModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		private string Filter
		{
			get
			{
				var filter = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["filter"]))
				{
					filter = (Request.Params["filter"]);
				}
				return filter;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private int Page
		{
			get
			{
				var page = 0;
				if (!String.IsNullOrEmpty(Request.Params["page"])) page = Convert.ToInt32(Request.Params["page"]);

				return page;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private int PageSize
		{
			get
			{
				var pageSize = Constants.DefaultPageSize;
				if (ModuleContext.Settings.ContainsKey(Constants.SettingMaxTagsTags))
				{
					pageSize = Convert.ToInt32(ModuleContext.Settings[Constants.SettingMaxTagsTags]);
				}

				return pageSize;
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
		private int TotalRecords { get; set; }

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
		public TagListPresenter(ITagListView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public TagListPresenter(ITagListView view, IDnnqaController controller)
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
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		protected void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				View.Model.SortBy = Sort;
				View.Model.Filter = Filter;
				View.Model.TopTags = BindTags(View.Model.CurrentPage, View.Model.Filter);
				View.Model.PageTitle = Localization.GetString("TagListMetaTitle", LocalResourceFile);
				View.Model.PageDescription = Localization.GetString("TagListMetaDescription", LocalResourceFile);
				View.Model.PageLink = Links.ViewTags(ModuleContext);
				View.ItemDataBound += ItemDataBound;
				View.PagerChanged += PagerChanged;
				View.TagFiltered += TagFiltered;

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
		protected void ItemDataBound(object sender, TagListEventArgs<Literal, Literal, TermInfo, Controls.Tags, Literal> e)
		{
			var colTerms = new List<TermInfo> {e.Term};

			e.Tags.ModContext = ModuleContext;
            e.Tags.ModuleTab = ModuleContext.PortalSettings.ActiveTab;
			e.Tags.DataSource = colTerms;
			e.Tags.DataBind();

			e.CountLiteral.Text = e.Term.TotalTermUsage + Localization.GetString("QuestionCount", LocalResourceFile);
			e.DescriptionLiteral.Text = Utils.TruncateString(e.Term.Description, 80, true);

			var statsText = e.Term.DayTermUsage + " " + Localization.GetString("AskedToday", LocalResourceFile) + ", " + e.Term.WeekTermUsage + " " +
							Localization.GetString("ThisWeek", LocalResourceFile);
			e.StatsLiteral.Text = statsText;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void PagerChanged(object sender, PagerChangedEventArgs<LinkButton, string> e)
		{
			View.Model.CurrentPage += Convert.ToInt32(e.LinkButton.CommandArgument);
			View.Model.Filter = e.Filter;
			View.Model.TopTags = BindTags(View.Model.CurrentPage, e.Filter);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TagFiltered(object sender, TagSearchEventArgs<string> e)
		{
			View.Model.CurrentPage = 0;
			View.Model.Filter = e.Filter;
			View.Model.TopTags = BindTags(View.Model.CurrentPage, e.Filter);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Returns a collection of tags, sorted and showing a specific 'page'. Also handles display of paging related buttons (like previous/next). 
		/// </summary>
		/// <param name="currentPage"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public List<TermInfo> BindTags(int currentPage, string filter)
		{
			var topTags = Controller.GetTermsByContentType(ModuleContext.PortalId, ModuleContext.ModuleId, VocabularyId);

			// apply filter
			if (filter != Null.NullString)
			{
				topTags = (from t in topTags where t.Name.Contains(filter) select t).ToList();
			}

			var objSort = new SortInfo { Column = "SortTotalUsage", Direction = Constants.SortDirection.Descending };

			if (Sort != Null.NullString)
			{
				switch (Sort.ToLower())
				{
					case "name":
						objSort.Column = "name";
						objSort.Direction = Constants.SortDirection.Ascending;
						//var nameSorted = (from t in View.Model.TopTags orderby t.Name descending select t);
						break;
					case "newest":
						objSort.Column = "newest";
						//var newestSorted = (from t in View.Model.TopTags orderby t. descending select t);
						break;
					default:
						objSort.Column = "popular";
						//var usageSorted = (from t in View.Model.TopTags orderby t.TotalTermUsage descending select t);
						break;
				}
			}

			TotalRecords = topTags.Count();
			var totalPages = Convert.ToDouble((double)TotalRecords/PageSize);

			if ((totalPages > 1) && (totalPages > currentPage + 1))
			{
				View.ShowNextButton(true);
				View.Model.NextPageLink = Links.ViewTagsPaged(ModuleContext, View.Model.CurrentPage + 1);
			}
			else
			{
				View.ShowNextButton(false);
			}

			if ((totalPages > 1) && (currentPage > 0))
			{
				View.ShowBackButton(true);
				View.Model.PrevPageLink = Links.ViewTagsPaged(ModuleContext, View.Model.CurrentPage - 1);
			}
			else
			{
				View.ShowBackButton(false);
			}

			return Sorting.GetTermCollection(PageSize, currentPage, objSort, topTags).ToList();
		}

		#endregion

	}
}