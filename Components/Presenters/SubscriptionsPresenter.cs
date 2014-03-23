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
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Web.UI.WebControls;
using Telerik.Web.UI;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class SubscriptionsPresenter : ModulePresenter<ISubscriptionsView, SubscriptionsModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// A collection of all subscriptions for the current user
		/// </summary>
		private IEnumerable<SubscriptionInfo> UserSubscriptions
		{
			get
			{
				return Controller.GetUserSubscriptions(ModuleContext.PortalId, ModuleContext.PortalSettings.UserId);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public SubscriptionsPresenter(ISubscriptionsView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public SubscriptionsPresenter(ISubscriptionsView view, IDnnqaController controller)
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
				View.QuestionGridNeedDataSource += QuestionGridNeedDataSource;
				View.TermGridNeedDataSource += TermGridNeedDataSource;
				View.GridsItemCommand += GridsItemCommand;
				View.GridsItemDataBound += GridsItemDataBound;
				View.Model.CurrentUserID = ModuleContext.PortalSettings.UserId;

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
		protected void QuestionGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
		{
			var colSubs = (from t in UserSubscriptions where t.PostId > 0 select t);


			var objGrid = (RadGrid)sender;

			objGrid.DataSource = colSubs;
			objGrid.VirtualItemCount = colSubs.Count();
			//objGrid.MasterTableView.ShowHeader = colMembers.Count > 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TermGridNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
		{
			var colSubs = (from t in UserSubscriptions where t.TermId > 0 select t);
			var objGrid = (RadGrid)sender;

			objGrid.DataSource = colSubs;
			objGrid.VirtualItemCount = colSubs.Count();
			//objGrid.MasterTableView.ShowHeader = colMembers.Count > 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GridsItemDataBound(object sender, GridItemEventArgs e)
		{
			if (e.Item.ItemType == GridItemType.AlternatingItem | e.Item.ItemType == GridItemType.Item)
			{
				//DnnGridDataItem item;
				//item = e.Item;
				//LinkButton LinkButton1 = default(LinkButton);
				//LinkButton1 = item("LinkColumn").FindControl("LinkButton1");
				//LinkButton1.CommandArgument = e.Item.ItemIndex.ToString();
			}

			//if (!(e.Item is GridDataItem)) return;
			//var userKeyID = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"];
			//var displayName = (string)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["DisplayName"];
			//var memberType = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["MemberType"];
			//var dataItem = (GridDataItem)e.Item;

			//var hlName = ((HyperLink)(dataItem)["DisplayName"].Controls[0]);
			//hlName.Text = displayName;
			//hlName.NavigateUrl = Globals.UserProfileURL(userKeyID);

			//var litTeamRole = ((Literal)(dataItem).FindControl("litMemberType"));
			//string memberTypeText;

			//switch (memberType)
			//{
			//    case 0:
			//        memberTypeText = Services.Localization.Localization.GetString(Constants.MemberType.Owner.ToString(), LocalResourceFile);
			//        break;
			//    case 1:
			//        memberTypeText = Services.Localization.Localization.GetString(Constants.MemberType.Coordinator.ToString(), LocalResourceFile);
			//        break;
			//    case 2:
			//        memberTypeText = Services.Localization.Localization.GetString(Constants.MemberType.Officer.ToString(), LocalResourceFile);
			//        break;
			//    default:
			//        memberTypeText = Services.Localization.Localization.GetString(Constants.MemberType.Contributor.ToString(), LocalResourceFile);
			//        break;
			//}

			//litTeamRole.Text = memberTypeText;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GridsItemCommand(object sender, GridCommandEventArgs e)
		{
			var objGrid = (RadGrid)sender;
			if (!(e.Item is GridDataItem)) return;
			var subscriptionId = (int)e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SubscriptionId"];

			switch (e.CommandName)
			{
				case "DeleteTerm":
					Controller.DeleteSubscription(ModuleContext.PortalId, subscriptionId);
					break;
				case "DeleteQuestion":
					Controller.DeleteSubscription(ModuleContext.PortalId, subscriptionId);
					break;
			}

			objGrid.Rebind();
		}

		#endregion

	}
}