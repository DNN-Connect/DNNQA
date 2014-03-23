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
	[PresenterBinding(typeof(PrivilegeManagerPresenter))]
	public partial class PrivilegeManager : ModuleView<PrivilegeManagerModel>, IPrivilegeManagerView
	{

		#region Public Events

		public event EventHandler<PrivilegeManagerSaveEventArgs<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>> OnPrivilegeSave;

		#endregion

		#region Constructor

		public PrivilegeManager()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Event Handlers

		protected void OnSubmitClick(object sender, EventArgs e)
		{
			OnPrivilegeSave(this, new PrivilegeManagerSaveEventArgs<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>("0", dntbCreatePost.Text, dntbNewUser.Text, dntbFlag.Text, dntbVoteUp.Text, dntbCommentEverywhere.Text, dntbVoteDown.Text, dntbRetag.Text, dntbEditQA.Text, dntbCreateTagSynonym.Text, dntbCloseQ.Text, dntbApproveTags.Text, dntbModTools.Text, dntbProtectQ.Text, dntbTrusted.Text));
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{

			if (Page.IsPostBack) return;
			var colPrivileges = Model.UserPrivileges;
			dntbCreatePost.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.CreatePost.ToString()).Value.ToString();
			dntbNewUser.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString()).Value.ToString();
			dntbFlag.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.Flag.ToString()).Value.ToString();
			dntbVoteUp.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.VoteUp.ToString()).Value.ToString();
			dntbCommentEverywhere.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.CommentEverywhere.ToString()).Value.ToString();
			dntbVoteDown.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.VoteDown.ToString()).Value.ToString();
			dntbRetag.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.RetagQuestion.ToString()).Value.ToString();
			dntbEditQA.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.EditQuestionsAndAnswers.ToString()).Value.ToString();
			dntbCreateTagSynonym.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.CreateTagSynonym.ToString()).Value.ToString();
			dntbCloseQ.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.CloseQuestion.ToString()).Value.ToString();
			dntbApproveTags.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.ApproveTagEdits.ToString()).Value.ToString();
			dntbModTools.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.ModeratorTools.ToString()).Value.ToString();
			dntbProtectQ.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.ProtectQuestions.ToString()).Value.ToString();
			dntbTrusted.Text = colPrivileges.Single(s => s.Key == Constants.Privileges.Trusted.ToString()).Value.ToString();

			hlCancel.NavigateUrl = Links.Home(ModuleContext.TabId);
		}

		#endregion

	}
}