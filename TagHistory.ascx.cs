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
using DotNetNuke.Common;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.Web.UI.WebControls;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(TagHistoryPresenter))]
	public partial class TagHistory : ModuleView<TagHistoryModel>, ITagHistoryView
	{

		#region Public Events

		public event EventHandler<TagHistoryListEventArgs<Term, TermHistoryInfo, Literal, Literal, Literal, DnnBinaryImage>> ItemDataBound;

		#endregion

		#region Constructor

		public TagHistory()
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

			Utils.SetTermHistoryPageMeta((CDefault)Page, Model.SelectedTerm, ModuleContext, Model.PageTitle, Model.PageDescription);
		}

		/// <summary>
		/// Handles formatting of individual items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptTermHistoryItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			var objTermHistory = (TermHistoryInfo)e.Item.DataItem;
			var litHeaderPanel = (Literal)e.Item.FindControl("litHeaderPanel");
			var litTermDescription = (Literal)e.Item.FindControl("litTermDescription");
			var litTermUpdated = (Literal)e.Item.FindControl("litTermUpdated");
			var dbiTermUser = (DnnBinaryImage)e.Item.FindControl("dbiTermUser");

			ItemDataBound(this, new TagHistoryListEventArgs<Term, TermHistoryInfo, Literal, Literal, Literal, DnnBinaryImage>(Model.SelectedCoreTerm, objTermHistory, litHeaderPanel, litTermDescription, litTermUpdated, dbiTermUser));
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;

			hlTitle.Text = Model.SelectedCoreTerm.Name;
			hlTitle.NavigateUrl = Links.ViewTagDetail(ModuleContext, ModuleContext.TabId, Model.SelectedCoreTerm.Name);

			UserInfo objUser;
			if (Model.SelectedCoreTerm.Description.Trim().Length < 1)
			{
				var strMessage = @"<div class='dnnFormMessage dnnFormWarning'>" +
								 Localization.GetString("NoHistory", LocalResourceFile) + @"</div>";

				if (ModuleContext.PortalSettings.UserId > 0)
				{
					strMessage = @"<div class='dnnFormMessage dnnFormWarning'><p>" +
								 Localization.GetString("NoHistory", LocalResourceFile) + @"</p><a class='dnnPrimaryAction' href='" + Links.EditTag(ModuleContext, ModuleContext.TabId, Model.SelectedCoreTerm.Name) + @"'>" +  Localization.GetString("Improve", LocalResourceFile) + @"</a></div>";
				}

				litDescription.Text = strMessage;
				objUser = UserController.GetUserById(ModuleContext.PortalId, Model.SelectedCoreTerm.CreatedByUserID);
			}
			else
			{
				litDescription.Text = Model.SelectedCoreTerm.Description;
				objUser = UserController.GetUserById(ModuleContext.PortalId, Model.SelectedCoreTerm.LastModifiedByUserID);
			}

			litUpdated.Text = @"<a href=" + Globals.UserProfileURL(objUser.UserID) + @">" + objUser.DisplayName + @" </a>".Trim();
			dbiUser.AlternateText = objUser.DisplayName;
			dbiUser.ToolTip = objUser.DisplayName;
			dbiUser.ImageUrl = objUser.Profile.PhotoURL;

			rptTermHistory.DataSource = Model.TermHistory;
			rptTermHistory.DataBind();
		}

		#endregion

	}
}