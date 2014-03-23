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

using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;

namespace DotNetNuke.DNNQA {

	[PresenterBinding(typeof(DispatchPresenter))]
	public partial class Dispatch : ModuleView<DispatchModel>, IDispatchView, IActionable
	{

		#region Constructor

		public Dispatch()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Methods

		public void Refresh() {

			Utils.RegisterClientDependencies(Page);

			var ctlDirectory = TemplateSourceDirectory;

			var objControl = LoadControl(ctlDirectory + Model.ControlToLoad) as ModuleUserControlBase;
			if (objControl == null) return;

			phUserControl.Controls.Clear();
			objControl.ModuleContext.Configuration = ModuleContext.Configuration;
			objControl.ID = System.IO.Path.GetFileNameWithoutExtension(ctlDirectory + Model.ControlToLoad);
			phUserControl.Controls.Add(objControl);

			if ((string)ViewState["CtlToLoad"] != Model.ControlToLoad) {
				ViewState["CtlToLoad"] = Model.ControlToLoad;
			}
		}

		#endregion
		
		#region IActionable Members

		/// <summary>
		/// 
		/// </summary>
		public ModuleActionCollection ModuleActions {
			get
			{
				ModuleActionCollection actions;


				if (!Model.InProfileMode)
				{
					actions = new ModuleActionCollection
							  {
								  {
									  ModuleContext.GetNextActionID(), Localization.GetString("ManagePrivileges.Action", LocalResourceFile),
									  ModuleActionType.ContentOptions, "", "",
									  ModuleContext.NavigateUrl(ModuleContext.TabId, "ManagePrivileges", false, "mid=" + ModuleContext.ModuleId), false, Security.SecurityAccessLevel.Admin, true, false
									  }, 
								  {
									  ModuleContext.GetNextActionID(), Localization.GetString("ManageScoring.Action", LocalResourceFile),
									  ModuleActionType.ContentOptions, "", "",
									  ModuleContext.NavigateUrl(ModuleContext.TabId, "ManageScoring", false, "mid=" + ModuleContext.ModuleId), false, Security.SecurityAccessLevel.Admin, true, false
									  }
							  };

	//                var objBadgeAction
	//= new ModuleAction(ModuleContext.GetNextActionID(), Localization.GetString("ManageBadges.Action", LocalResourceFile),
	//ModuleActionType.ContentOptions, "", "",
	//ModuleContext.NavigateUrl(ModuleContext.TabId, "ManageBadges", false, "mid=" + ModuleContext.ModuleId), "", false, Security.SecurityAccessLevel.Admin, true, false);

	//                actions.Add(objBadgeAction);
				}
				else
				{
				    return new ModuleActionCollection();
				}

				return actions;
			}
		}

		#endregion

	}
}