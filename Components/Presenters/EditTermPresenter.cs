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

using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.Common;
using System;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class EditTermPresenter : ModulePresenter<IEditTermView, EditTermModel>
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
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> PrivilegeCollection
		{
			get
			{
				return QaSettings.GetPrivilegeCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private UserScoreInfo UserScore
		{
			get
			{
				if (ModuleContext.PortalSettings.UserId > 0)
				{
					var usersScore = Controller.GetUserScore(ModuleContext.PortalSettings.UserId, ModuleContext.PortalId);
					if (usersScore != null)
					{
						return usersScore;
					}
				}
				var objUserScore = new UserScoreInfo
				{
					Message = "",
					PortalId = ModuleContext.PortalId,
					UserId = ModuleContext.PortalSettings.UserId,
					Score = 0
				};
				return objUserScore;
			}
		}

		#endregion
		
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public EditTermPresenter(IEditTermView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public EditTermPresenter(IEditTermView view, IDnnqaController controller)
			: base(view)
		{
			if (view == null) {
				throw new ArgumentException(@"View is nothing.", "view");
			}

			if (controller == null) {
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
				var returnUrl = HttpContext.Request.RawUrl;
				var redirect = false;

				if (ModuleContext.PortalSettings.UserId < 1)
				{	
					if (returnUrl.IndexOf("?returnurl=") != -1)
					{
						returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?returnurl="));
					}				
					returnUrl = HttpUtility.UrlEncode(returnUrl);
					returnUrl = Globals.LoginURL(returnUrl, true);
					redirect = true;
				}

				var objTerm = (from t in Util.GetTermController().GetTermsByVocabulary(1) where t.Name.ToLower() == Tag.ToLower() select t).Single();

				if (objTerm != null)
				{
					View.Model.SelectedTerm = objTerm;
					View.Model.SelectedTermHistory = Controller.GetTermHistory(ModuleContext.PortalId, objTerm.TermId);

					var objRemoveNewUser = PrivilegeCollection.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString());
					if (UserScore.Score >= objRemoveNewUser.Value || ModuleContext.IsEditable)
					{
						View.SaveEnabled(true);
					}
					else
					{
						View.SaveEnabled(false);  
					}

					View.Save += Save;
					View.Refresh();
				}
				else
				{
					returnUrl = Globals.AccessDeniedURL("AccessDenied");
					redirect = true;
				}

				if (redirect)
				{
					Response.Redirect(returnUrl, false);
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
		protected void Save(object sender, EditTermEventArgs<TermHistoryInfo> e)
		{
			try
			{
				var notes = e.TermHistory.Notes;

				//TODO: allow tag name editing
				// we have our UI values temporarily stored in variables above, we can update the history item (it needs to be added PRIOR to taxonomy term update done via core API)
				// TODO: Change auto approval to off
				Controller.AddTermHistory(ModuleContext.PortalId, View.Model.SelectedTerm.TermId, notes, true, ModuleContext.ModuleId);

				var cntTerm = new DotNetNuke.Entities.Content.Taxonomy.TermController();
				var objTerm = View.Model.SelectedTerm;

				// make sure title/body are passing through security filters
				var objSecurity = new PortalSecurity();
				var description = objSecurity.InputFilter(e.TermHistory.Description, PortalSecurity.FilterFlag.NoScripting);

				objTerm.Description = description;
				cntTerm.UpdateTerm(objTerm);

				var colUserScoring = QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
				var objScoreLog = new UserScoreLogInfo
				{
					UserId = ModuleContext.PortalSettings.UserId,
					PortalId = ModuleContext.PortalId,
					UserScoringActionId = (int)Constants.UserScoringActions.EditedTag,
					Score =
						colUserScoring.Single(
							s => s.Key == Constants.UserScoringActions.EditedTag.ToString()).Value,
					CreatedOnDate = DateTime.Now
				};

				Controller.AddScoringLog(objScoreLog, PrivilegeCollection);

				Response.Redirect(Links.ViewTagDetail(ModuleContext, ModuleContext.TabId, View.Model.SelectedTerm.Name), false);

			}
			catch (Exception exception)
			{
				ProcessModuleLoadException(exception);
			}
		}

		#endregion
	
	}
}