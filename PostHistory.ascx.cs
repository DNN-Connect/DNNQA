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
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(PostHistoryPresenter))]
	public partial class PostHistory : ModuleView<PostHistoryModel>, IPostHistoryView
	{

		#region Public Events

		public event EventHandler<PostHistoryListEventArgs<PostInfo, PostHistoryInfo, Literal, Literal>> ItemDataBound;

		#endregion

		#region Constructor

		public PostHistory()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles formatting of individual items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptTermHistoryItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			var objPostHistory = (PostHistoryInfo)e.Item.DataItem;
			var litHeaderPanel = (Literal)e.Item.FindControl("litHeaderPanel");
			var litDescription = (Literal)e.Item.FindControl("litDescription");

			ItemDataBound(this, new PostHistoryListEventArgs<PostInfo, PostHistoryInfo, Literal, Literal>(Model.SelectedPost, objPostHistory, litHeaderPanel, litDescription));
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;

			litTitle.Text = Model.SelectedPost.Title;

			rptTermHistory.DataSource = Model.PostHistory;
			rptTermHistory.DataBind();
		}

		#endregion

	}
}