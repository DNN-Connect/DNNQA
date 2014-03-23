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
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(BadgeManagerPresenter))]
	public partial class BadgeManager : ModuleView<BadgeManagerModel>, IBadgeManagerView
	{

		#region Public Events

		public event EventHandler<BadgeManagerListEventArgs<BadgeInfo, Literal, Literal, Literal, Literal>> ItemDataBound;

		#endregion

		#region Constructor

		public BadgeManager()
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
			
			jQuery.RequestUIRegistration();

			ClientResourceManager.RegisterScript(Page, "~/Resources/Shared/scripts/knockout.js");
			ClientResourceManager.RegisterScript(Page, "~/DesktopModules/DNNQA/js/BadgeManager.js");

			ClientResourceManager.RegisterScript(Page, "~/DesktopModules/DNNQA/js/jquery.smartWizard-2.0.js");
			ClientResourceManager.RegisterStyleSheet(Page, "~/DesktopModules/DNNQA/smart_wizard.css");
		}


		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
		    hlCancel.NavigateUrl = Links.Home(ModuleContext.TabId);

		}

		#endregion

	}
}