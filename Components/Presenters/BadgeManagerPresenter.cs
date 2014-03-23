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

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class BadgeManagerPresenter : ModulePresenter<IBadgeManagerView, BadgeManagerModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> UserScoringCollection
		{
			get
			{
				return QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			}
		}

		private List<BadgeInfo> UserBadges
		{
			get { return Controller.GetUserBadges(ModuleContext.PortalId, ModuleContext.PortalSettings.UserId); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public BadgeManagerPresenter(IBadgeManagerView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public BadgeManagerPresenter(IBadgeManagerView view, IDnnqaController controller)
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
		protected void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				//var colBadges = Controller.GetPortalBadges(ModuleContext.PortalId);
				//var objBadge = colBadges.Single(s => s.BadgeId == Id);

				//if (objBadge != null)
				//{
				//    View.Model.Badge = objBadge;
				//}
				View.Model.PortalBadges = Controller.GetPortalBadges(ModuleContext.PortalId);

				View.Model.UserScoringActions = UserScoringCollection.ToList();
				View.ItemDataBound += ItemDataBound;
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
		protected void ItemDataBound(object sender, BadgeManagerListEventArgs<BadgeInfo, Literal, Literal, Literal, Literal> e)
		{
			//e.EditLiteral.Text = "<span class=\"earnedBadge\" title=\"" + Localization.GetString("EarnedBadge", Constants.SharedResourceFileName) + "\" >&nbsp;</span>";

			e.BadgeLiteral.Text = "<a href=\"" + Links.ViewBadge(ModuleContext, Localization.GetString(e.Badge.NameLocalizedKey, Constants.SharedResourceFileName), e.Badge.BadgeId) + "\" title=\"" + Localization.GetString(e.Badge.TierDetails.TitlePrefixKey, Constants.SharedResourceFileName) + Localization.GetString(e.Badge.DescriptionLocalizedKey, Constants.SharedResourceFileName) + "\" class=\"qaBadge\"><span class=\"" + e.Badge.TierDetails.IconClass + "\"></span>" + Localization.GetString(e.Badge.NameLocalizedKey, Constants.SharedResourceFileName) + "</a>";
			e.MultiplierLiteral.Text = " x " + e.Badge.Awarded;
			e.DescriptionLiteral.Text = Localization.GetString(e.Badge.DescriptionLocalizedKey, Constants.SharedResourceFileName);
		}

		#endregion

	}
}