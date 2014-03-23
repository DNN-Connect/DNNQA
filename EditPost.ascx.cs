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
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Components.Models;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA 
{
	[PresenterBinding(typeof(EditPostPresenter))]
	public partial class EditPost : ModuleView<EditPostModel>, IEditPostView
	{

		#region Private Members

// ReSharper disable InconsistentNaming
		protected UI.UserControls.TextEditor teContent;
// ReSharper restore InconsistentNaming

		#endregion

		#region Public Events

		public event EventHandler Delete;
		public event EventHandler<EditPostEventArgs<PostInfo, string>> Save;

		#endregion

		#region Constructor

		public EditPost()
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

			Framework.jQuery.RequestUIRegistration();

			// request script registration for custom/3rd party scripts
			ClientResourceManager.RegisterScript(Page, TemplateSourceDirectory + "/js/jquery.tagify.js");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdDeleteClick(object sender, EventArgs e)
		{
			Delete(sender, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdSaveClick(object sender, EventArgs e)
		{
			var objPost = new PostInfo();

			if (Model.SelectedPost.ParentId < 1)
			{
				objPost.Title = txtTitle.Text;
			}

			objPost.Body = teContent.Text.Trim();
			objPost.Approved = chkApproved.Checked;

			Save(this, new EditPostEventArgs<PostInfo, string>(objPost, txtTags.Text.Trim()));
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enabled"></param>
		public void SaveEnabled(bool enabled)
		{
			if (!enabled)
			{
				UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("NoRemoveNewUser", LocalResourceFile), ModuleMessage.ModuleMessageType.RedError);
				cmdSave.Enabled = false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowAuditControl(bool show)
		{
			ctlAudit.Visible = show;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowDeleteButton(bool show)
		{
			cmdDelete.Visible = show;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowTagEdits(bool show)
		{
			pnlTags.Visible = show;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="permit"></param>
		/// <param name="showSubject"></param>
		public void PermitPostEdit(bool permit, bool showSubject)
		{
			pnlQuestion.Visible = permit;
			pnlSubject.Visible = showSubject;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="show"></param>
		public void ShowQuestionSpecific(bool show)
		{
			pnlSubject.Visible = show;

			UI.Skins.Skin.AddModuleMessage(this,
										   show
											   ? Localization.GetString("EditQuestion", LocalResourceFile)
											   : Localization.GetString("EditAnswer", LocalResourceFile),
										   ModuleMessage.ModuleMessageType.YellowWarning);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			cmdCancel.NavigateUrl = Model.QuestionUrl;
	

			ctlAudit.CreatedDate = Model.SelectedPost.CreatedDate.ToString();
			ctlAudit.CreatedByUser = Model.SelectedPost.PostCreatedDisplayName;
			ctlAudit.LastModifiedDate = Model.SelectedPost.LastModifiedDate.ToString();
			ctlAudit.LastModifiedByUser = Model.SelectedPost.PostLastModifiedDisplayName;

			if (Page.IsPostBack) return;

			if (Model.SelectedPost.PostId > 0)
			{
				teContent.Text = Model.SelectedPost.Body;
				chkApproved.Checked = Model.SelectedPost.Approved;

				if (Model.SelectedPost.ParentId < 1)
				{
					txtTitle.Text = Model.SelectedPost.Title;

					if (Model.SelectedTags.Count > 0)
					{
						foreach (var t in Model.SelectedTags)
						{
							txtTags.Text = txtTags.Text + t.Name + @",";
						}	
					}
				}
			}
		}

		#endregion

	}
}