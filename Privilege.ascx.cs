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
	[PresenterBinding(typeof(PrivilegePresenter))]
	public partial class Privilege : ModuleView<PrivilegeModel>, IPrivilegeView
	{

		#region Public Events

		public event EventHandler<PrivilegeListEventArgs<QaSettingInfo, int, HyperLink, Literal>> ItemDataBound;

		#endregion

		#region Constructor

		public Privilege()
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
			Utils.SetPrivilegePageMeta((CDefault)Page, ModuleContext, Localization.GetString(Model.SelectedPrivilege.Name, Constants.SharedResourceFileName), Localization.GetString(Model.SelectedPrivilege.Description, Constants.SharedResourceFileName));
		}

		/// <summary>
		/// Handles formatting of individual items in the questions repeater.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RptPrivilegesItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var link = (HyperLink)e.Item.FindControl("hlPrivilege");
				var litCompletePercent = (Literal)e.Item.FindControl("litCompletePercent");
				var objPrivilege = (QaSettingInfo)e.Item.DataItem;

				ItemDataBound(this, new PrivilegeListEventArgs<QaSettingInfo, int, HyperLink, Literal>(objPrivilege, Model.CurrentUserScore, link, litCompletePercent));
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

			litTitle.Text = Localization.GetString(Model.SelectedPrivilege.Name, Constants.SharedResourceFileName);
			litDescription.Text = Localization.GetString(Model.SelectedPrivilege.Description, Constants.SharedResourceFileName);

			if (Model.SelectedPrivilege.Key == Constants.Privileges.ViewAll.ToString())
			{
				litScoreTitle.Text = Localization.GetString("YourScore", LocalResourceFile);
				litScoreValue.Text = Model.CurrentUserScore.ToString();
				litProgress.Text =
					Utils.CalculatePointsTillNextPriv(Model.CurrentUserScore, Model.NextAchievablePrivilege.Value) +
					Localization.GetString("UntilNextPriv", LocalResourceFile) + Localization.GetString(Model.NextAchievablePrivilege.Name, Constants.SharedResourceFileName);
			}
			else
			{
				litScoreTitle.Text = Localization.GetString("ScoreRequired", LocalResourceFile);
				litScoreValue.Text = Model.SelectedPrivilege.Value.ToString();

				if (Model.CurrentUserScore >= Model.SelectedPrivilege.Value)
				{
					litProgress.Text = Localization.GetString("Complete", LocalResourceFile);
				}
				else
				{
					litProgress.Text = Utils.CalucalatePercentForDisplay(Model.CurrentUserScore, Model.SelectedPrivilege.Value) + Localization.GetString("NotComplete", LocalResourceFile);
				}
			}

			rptPrivileges.DataSource = Model.Privileges;
			rptPrivileges.DataBind();
		}

		#endregion

	}
}