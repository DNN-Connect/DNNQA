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

	[PresenterBinding(typeof(AskQuestionPresenter))]
	public partial class AskQuestion : ModuleView<AskQuestionModel>, IAskQuestionView
	{

		#region Controls

// ReSharper disable InconsistentNaming
		protected UI.UserControls.TextEditor teContent;
// ReSharper restore InconsistentNaming

		#endregion

		#region Public Events

		public event EventHandler<AskQuestionEventArgs<PostInfo, bool, string>> Save;

		#endregion

		#region Constructor

		public AskQuestion()
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

			ClientResourceManager.RegisterScript(Page, "http://ajax.microsoft.com/ajax/jquery.templates/beta1/jquery.tmpl.js");
			ClientResourceManager.RegisterScript(Page, TemplateSourceDirectory + "/js/jquery.tagify.js");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void CmdSaveClick(object sender, EventArgs e)
		{
			var newPost = new PostInfo
			{
				Title = txtTitle.Text,
				Body = teContent.Text.Trim(),
				Approved = chkApproved.Checked
			};

			Save(this, new AskQuestionEventArgs<PostInfo, bool, string>(newPost, chkNotify.Checked, txtTags.Text.Trim()));
		}

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
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;
			cmdCancel.NavigateUrl = ModuleContext.NavigateUrl(ModuleContext.TabId, "", false, "");
			if (Page.IsPostBack) return;
			txtTags.Text = Model.SelectedTags;
			txtTitle.Attributes.Add("defaultvalue", Localization.GetString("overlayTitle", LocalResourceFile));
			//txtTags.Attributes.Add("defaultvalue", Localization.GetString("AddTags", LocalResourceFile)); // this always looses to tagify
		}

		#endregion

	}
}