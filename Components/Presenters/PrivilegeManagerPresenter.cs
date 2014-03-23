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
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
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
	public class PrivilegeManagerPresenter : ModulePresenter<IPrivilegeManagerView, PrivilegeManagerModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public PrivilegeManagerPresenter(IPrivilegeManagerView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public PrivilegeManagerPresenter(IPrivilegeManagerView view, IDnnqaController controller)
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
				View.Model.UserPrivileges = QaSettings.GetPrivilegeCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId).ToList();
				View.OnPrivilegeSave += OnPrivilegeSave;

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
		protected void OnPrivilegeSave(object sender, PrivilegeManagerSaveEventArgs<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string> e)
		{
			var objSetting = new SettingInfo
								 {
									 PortalId = ModuleContext.PortalId,
									 TypeId = (int) Constants.SettingTypes.PrivilegeLevelScore,
									 Key = Constants.Privileges.ViewAll.ToString(),
									 Value = e.ViewAll
			};

			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.CreatePost.ToString();
			objSetting.Value = e.CreatePost;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.RemoveNewUser.ToString();
			objSetting.Value = e.RemoveNewUser;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.Flag.ToString();
			objSetting.Value = e.Flag;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.VoteUp.ToString();
			objSetting.Value = e.VoteUp;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.CommentEverywhere.ToString();
			objSetting.Value = e.CommentEverywhere;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.VoteDown.ToString();
			objSetting.Value = e.VoteDown;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.RetagQuestion.ToString();
			objSetting.Value = e.RetagQuestion;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.EditQuestionsAndAnswers.ToString();
			objSetting.Value = e.EditQuestionsAndAnswers;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.CreateTagSynonym.ToString();
			objSetting.Value = e.CreateTagSynonym;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.CloseQuestion.ToString();
			objSetting.Value = e.CloseQuestion;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.ApproveTagEdits.ToString();
			objSetting.Value = e.ApproveTagEdits;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.ModeratorTools.ToString();
			objSetting.Value = e.ModeratorTools;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.ProtectQuestions.ToString();
			objSetting.Value = e.ProtectQuestions;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.Privileges.Trusted.ToString();
			objSetting.Value = e.Trusted;
			Controller.UpdateQaPortalSetting(objSetting);

			// Clear settings cache (for this collection)
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + ModuleContext.PortalId);

			// Redirect User
			Response.Redirect(Links.Home(ModuleContext.TabId), false);
		}

		#endregion

	}
}