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
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.Web.UI.WebControls;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class TagHistoryPresenter : ModulePresenter<ITagHistoryView, TagHistoryModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// The tag we want to search for (based on a parameter in the URL). 
		/// </summary>
		private string Tag
		{
			get
			{
				var tag = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["term"])) tag = (Request.Params["term"]);
				var objSecurity = new PortalSecurity();

				return objSecurity.InputFilter(tag, PortalSecurity.FilterFlag.NoSQL);
			}
		}

		/// <summary>
		/// TODO: Tie this to a module setting.
		/// </summary>
		private int VocabularyId
		{
			get { return 1; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public TagHistoryPresenter(ITagHistoryView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public TagHistoryPresenter(ITagHistoryView view, IDnnqaController controller)
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
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				var urlTerm =
					(from t in Util.GetTermController().GetTermsByVocabulary(VocabularyId)
					 where t.Name.ToLower() == Tag.ToLower()
					 select t).Single();

				if (urlTerm != null)
				{
					View.Model.SelectedTerm =
					(from t in Controller.GetTermsByContentType(ModuleContext.PortalId, ModuleContext.ModuleId, VocabularyId)
					 where t.Name.ToLower() == Tag.ToLower()
					 select t).SingleOrDefault();

					View.Model.SelectedCoreTerm = urlTerm;
					View.Model.TermHistory = Controller.GetTermHistory(ModuleContext.PortalId, urlTerm.TermId);
					View.ItemDataBound += ItemDataBound;
					View.Model.CurrentUserID = ModuleContext.PortalSettings.UserId;
					View.Model.PageTitle = Localization.GetString("HistoryMetaTitle", LocalResourceFile).Replace("[0]", View.Model.SelectedTerm.Name); ;
					View.Model.PageDescription = View.Model.SelectedTerm.Description;

					View.Refresh();
				}
				else
				{
					Response.Redirect(Globals.AccessDeniedURL("AccessDenied"), false);
				}
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
		protected void ItemDataBound(object sender, TagHistoryListEventArgs<Term, TermHistoryInfo, Literal, Literal, Literal, DnnBinaryImage> e)
		{
			UserInfo objUser;
			e.HeaderLiteral.Text = @"<h2 id='qaTermHistoryPanel-" + e.TermHistory.Revision + @"' class='dnnFormSectionHead'><a href="""">" + Utils.CalculateDateForDisplay(e.TermHistory.RevisedOnDate) + @" <span> " + @"</span></a></h2>";
			if (e.TermHistory.Description.Trim().Length < 1)
			{
				e.DescriptionLiteral.Text = @"<div class='dnnFormMessage dnnFormWarning'>" + Localization.GetString("NoHistory", LocalResourceFile) + @"</div>";				
			}
			else
			{
				// add link for reject?


				e.DescriptionLiteral.Text = e.TermHistory.Description;
			}

			if (e.TermHistory.Revision > 0)
			{
				objUser = UserController.GetUserById(ModuleContext.PortalId, e.TermHistory.RevisedByUserId);
			}
			else
			{
				objUser = UserController.GetUserById(ModuleContext.PortalId, e.SelectedTerm.CreatedByUserID);
			}

			if (objUser != null)
			{
				e.UpdatedLiteral.Text = @"<a href=" + Globals.UserProfileURL(objUser.UserID) + @">" + objUser.DisplayName + @" </a>".Trim();               
				e.UserImage.AlternateText = objUser.DisplayName;
				e.UserImage.ToolTip = objUser.DisplayName;
				e.UserImage.ImageUrl = objUser.Profile.PhotoURL;
			}
		}

		#endregion

	}
}