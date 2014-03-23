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
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(BadgesPresenter))]
	public partial class Badges : ModuleView<BadgesModel>, IBadgesView
	{

		#region Public Events

		public event EventHandler<BadgesListEventArgs<BadgeInfo, Literal, Literal, Literal, Literal>> ItemDataBound;
		public event EventHandler<TierListEventArgs<BadgeTierInfo, Literal, Literal>> TierItemDataBound;

		#endregion

		#region Constructor

		public Badges()
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
			Utils.SetBadgesPageMeta((CDefault)Page, ModuleContext, Model.PageTitle, Model.PageDescription, Model.PageLink);
		}

		/// <summary>
		/// Handles formatting of individual items in the badges repeater.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptBadgesItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var objBadge = (BadgeInfo)e.Item.DataItem;
				var litAwarded = (Literal)e.Item.FindControl("litAwarded");
				var litBadge = (Literal)e.Item.FindControl("litBadge");
				var litMultiplier = (Literal)e.Item.FindControl("litMultiplier");
				var litDescription = (Literal)e.Item.FindControl("litDescription");

				ItemDataBound(this, new BadgesListEventArgs<BadgeInfo, Literal, Literal, Literal, Literal>(objBadge, litAwarded, litBadge, litDescription, litMultiplier));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptTiersItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var objTier = (BadgeTierInfo)e.Item.DataItem;
				var litBadgeTier = (Literal)e.Item.FindControl("litBadgeTier");
				var litTierDescription = (Literal)e.Item.FindControl("litTierDescription");

				TierItemDataBound(this, new TierListEventArgs<BadgeTierInfo, Literal, Literal>(objTier, litBadgeTier, litTierDescription));
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			dgqHeaderNav.ModContext = ModuleContext;

			rptTiers.DataSource = Model.BadgeTiers;
			rptTiers.DataBind();

			rptBadges.DataSource = Model.PortalBadges;
			rptBadges.DataBind();
		}

		#endregion

	}
}