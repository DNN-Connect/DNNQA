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
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Security;
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
	public class PostHistoryPresenter : ModulePresenter<IPostHistoryView, PostHistoryModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// Checks the querystring for PostID. If not found, the interface needs to be in 'add question' mode. 
		/// </summary>
		public int PostId
		{
			get
			{
				var postId = Null.NullInteger;
				if (!String.IsNullOrEmpty(Request.Params["postid"]))
				{
					postId = Int32.Parse(Request.Params["postid"]);
				}
				return postId;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public PostHistoryPresenter(IPostHistoryView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public PostHistoryPresenter(IPostHistoryView view, IDnnqaController controller)
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

				if (PostId != Null.NullInteger)
				{
					View.Model.SelectedPost = Controller.GetPost(PostId, ModuleContext.PortalId);
					View.Model.PostHistory = Controller.GetPostHistory(PostId);
					View.ItemDataBound += ItemDataBound;

					View.Refresh();
				}
				else
				{
					Response.Redirect(Globals.AccessDeniedURL("AccessDenied"), false);
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
		protected void ItemDataBound(object sender, PostHistoryListEventArgs<PostInfo, PostHistoryInfo, Literal, Literal> e)
		{
			e.HeaderLiteral.Text = @"<h2 id='qaTermHistoryPanel-" + e.PostHistory.Revision + @"' class='dnnFormSectionHead'><a href="""">" + Utils.CalculateDateForDisplay(e.PostHistory.RevisedOnDate) + @" <span> " + @"</span></a></h2>";
			e.DescriptionLiteral.Text = e.PostHistory.Body;
		}

		#endregion

	}
}