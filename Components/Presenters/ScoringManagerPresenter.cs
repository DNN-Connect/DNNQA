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
	public class ScoringManagerPresenter : ModulePresenter<IScoringManagerView, ScoringManagerModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public ScoringManagerPresenter(IScoringManagerView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public ScoringManagerPresenter(IScoringManagerView view, IDnnqaController controller)
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
				View.Model.UserScoringActions = QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId).ToList();
				View.OnScoringSave += OnScoringSave;

				View.Refresh();
			}
			catch (Exception exc)
			{
				ProcessModuleLoadException(exc);
			}
		}

		protected void OnScoringSave(object sender, ScoringManagerSaveEventArgs<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string> e)
		{
			var objSetting = new SettingInfo
			{
				PortalId = ModuleContext.PortalId,
				TypeId = (int)Constants.SettingTypes.UserScoringActionValue,
				Key = Constants.UserScoringActions.AdminEntered.ToString(),
				Value = "0"
			};
			Controller.UpdateQaPortalSetting(objSetting);

			// these get added/updated but cannot be altered from UI (these are ad-hoc and value changes per use OR only logged for tracking).
			objSetting.Key = Constants.UserScoringActions.BountyPaid.ToString();
			objSetting.Value = "0";
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.BountyReceived.ToString();
			objSetting.Value = "0";
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.OfferedBounty.ToString();
			objSetting.Value = "0";
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ApprovedPostEdit.ToString();
			objSetting.Value = e.ApprovedPostEdit;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ApprovedTagEdit.ToString();
			objSetting.Value = e.ApprovedTagEdit;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.AskedFlaggedQuestion.ToString();
			objSetting.Value = e.AskedFlaggedQuestion;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.AskedQuestion.ToString();
			objSetting.Value = e.AskedQuestion;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.AskedQuestionVotedDown.ToString();
			objSetting.Value = e.AskedQuestionVotedDown;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.AskedQuestionVotedUp.ToString();
			objSetting.Value = e.AskedQuestionVotedUp;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.CreatedTagSynonym.ToString();
			objSetting.Value = e.CreatedTagSynonym;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.Commented.ToString();
			objSetting.Value = e.Commented;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.EditedPost.ToString();
			objSetting.Value = e.EditedPost;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.EditedTag.ToString();
			objSetting.Value = e.EditedTag;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.EditedTagVotedDown.ToString();
			objSetting.Value = e.EditedTagVotedDown;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.EditedTagVotedUp.ToString();
			objSetting.Value = e.EditedTagVotedUp;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.FirstLoggedInView.ToString();
			objSetting.Value = e.FirstLoggedInView;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ProvidedAnswer.ToString();
			objSetting.Value = e.ProvidedAnswer;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ProvidedAcceptedAnswer.ToString();
			objSetting.Value = e.ProvidedAcceptedAnswer;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ProvidedAnswerVotedDown.ToString();
			objSetting.Value = e.ProvidedAnswerVotedDown;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ProvidedAnswerVotedUp.ToString();
			objSetting.Value = e.ProvidedAnswerVotedUp;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.ProvidedFlaggedAnswer.ToString();
			objSetting.Value = e.ProvidedFlaggedAnswer;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedDownAnswer.ToString();
			objSetting.Value = e.VotedDownAnswer;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedDownQuestion.ToString();
			objSetting.Value = e.VotedDownQuestion;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedSynonymDown.ToString();
			objSetting.Value = e.VotedSynonymDown;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedSynonymUp.ToString();
			objSetting.Value = e.VotedSynonymUp;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedTagDown.ToString();
			objSetting.Value = e.VotedTagDown;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedTagUp.ToString();
			objSetting.Value = e.VotedTagUp;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedUpAnswer.ToString();
			objSetting.Value = e.VotedUpAnswer;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.VotedUpQuestion.ToString();
			objSetting.Value = e.VotedUpQuestion;
			Controller.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.UserScoringActions.AcceptedQuestionAnswer.ToString();
			objSetting.Value = e.AcceptedQuestionAnswer;
			Controller.UpdateQaPortalSetting(objSetting);

			// Clear settings cache (for this collection) 
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + ModuleContext.PortalId);

			Response.Redirect(Links.Home(ModuleContext.TabId), false);
		}

		#endregion

	}
}