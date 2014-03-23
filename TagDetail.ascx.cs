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
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(TagDetailPresenter))]
	public partial class TagDetail : ModuleView<TagDetailModel>, ITagDetailView
	{

		#region Public Events

		public event EventHandler<TermSynonymListEventArgs<Controls.Voting, Controls.Tags, TermSynonymInfo, ImageButton>> ItemDataBound;
		public event EventHandler<AddTermSynonymEventArgs<string>> AddSynonym;
		public event EventHandler<DeleteSuggestedSynonymEventArgs<ImageButton>> DeleteSynonym;

		#endregion

		#region Constructor

		public TagDetail()
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

			Utils.SetTermPageMeta((CDefault)Page, Model.SelectedTerm, ModuleContext, Model.PageTitle, Model.PageDescription);
		}

		/// <summary>
		/// Handles formatting of individual items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptSuggestedSynonymsItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var votingControl = (Controls.Voting)e.Item.FindControl("qavTermSynonym");
				var tagControl = (Controls.Tags)e.Item.FindControl("qatTermSynonym");
				var termSynonym = (TermSynonymInfo)e.Item.DataItem;
				var imgDelete = (ImageButton) e.Item.FindControl("imgDelete");

				ItemDataBound(this, new TermSynonymListEventArgs<Controls.Voting, Controls.Tags, TermSynonymInfo, ImageButton>(votingControl, tagControl, termSynonym, imgDelete));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptSuggestedSynonymsItemCommand(object sender, RepeaterCommandEventArgs e)
		{

			if (e.CommandName == "Delete")
			{
				var imgDelete = (ImageButton)e.CommandSource;
				DeleteSynonym(this, new DeleteSuggestedSynonymEventArgs<ImageButton>(imgDelete));
				rptSuggestedSynonyms.DataSource = Model.SuggestedTermSynonyms;
				rptSuggestedSynonyms.DataBind();
				ShowAddSynonym(true);
			}
			//else
			//{
			//    // means user voted or subscribed
			//}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdAddSynonymClick(object sender, EventArgs e)
		{
			var termString = txtTags.Text.Trim();
			termString = termString.TrimEnd(',', ';');
			var userEnteredTerms = termString.Split(',').ToList();

			switch (userEnteredTerms.Count)
			{
				case 0:
					UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("OneTerm", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError);
					break;
				case 1:
					break;
				default:
					UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("MultipleTerms", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError);
					return;
			}

			// we know we have 1 term here
			AddSynonym(this, new AddTermSynonymEventArgs<string>(termString));

			if (Model.ErrorMessage.Length > 0)
			{
				UI.Skins.Skin.AddModuleMessage(this, Localization.GetString(Model.ErrorMessage, LocalResourceFile), ModuleMessage.ModuleMessageType.RedError);
				return;
			}
			txtTags.Text = "";
			rptSuggestedSynonyms.DataSource = Model.SuggestedTermSynonyms;
			rptSuggestedSynonyms.DataBind();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;
			hlEdit.NavigateUrl = Links.EditTag(ModuleContext, ModuleContext.TabId, Model.SelectedTerm.Name);
			hlHistory.NavigateUrl = Links.ViewTagHistory(ModuleContext, Model.SelectedTerm.Name);
			hlSynonym.NavigateUrl = Links.ViewTermSynonyms(ModuleContext, Model.SelectedTerm.Name);
			hlAbout.NavigateUrl = Links.ViewTagDetail(ModuleContext, ModuleContext.TabId, Model.SelectedTerm.Name);

			if (Model.ActiveTermSynonyms.Count > 0)
			{
				tagSynonyms.ModContext = ModuleContext;
				tagSynonyms.ModuleTab = ModuleContext.PortalSettings.ActiveTab;
				tagSynonyms.DataSource = Model.ActiveTermSynonyms;
				tagSynonyms.DataBind();
			}

			if (Model.SuggestedTermSynonyms.Count > 0)
			{
				rptSuggestedSynonyms.DataSource = Model.SuggestedTermSynonyms;
				rptSuggestedSynonyms.DataBind();
			}

			switch (Model.SelectedView.ToLower())
			{
				case "termsynonyms":
					hlSynonym.CssClass = "qaSortSel";
					divDetail.Visible = false;
					divSynonym.Visible = true;
					break;
				default:
					hlAbout.CssClass = "qaSortSel";
					divSynonym.Visible = false;
					divDetail.Visible = true;
					break;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowActiveSynonyms(bool show)
		{
			pnlActiveSynonyms.Visible = show;
			pnlNoSynonyms.Visible = !show;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowSuggestedSynonyms(bool show)
		{
			pnlSuggestedSynonyms.Visible = show;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowAddSynonym(bool show)
		{
			pnlAddSynonym.Visible = show;
		}

		#endregion

	}
}