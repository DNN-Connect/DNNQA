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
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Presenters;

namespace DotNetNuke.DNNQA
{

	/// <summary>
	/// 
	/// </summary>
	[PresenterBinding(typeof(ScoringManagerPresenter))]
	public partial class ScoringManager : ModuleView<ScoringManagerModel>, IScoringManagerView
	{

		#region Public Events

		public event EventHandler<ScoringManagerSaveEventArgs<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>> OnScoringSave;

		#endregion

		#region Constructor

		public ScoringManager()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnSubmitClick(object sender, EventArgs e)
		{
			OnScoringSave(this, new ScoringManagerSaveEventArgs<string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string, string>(dntbApprovedPostEdit.Text, dntbApproveTagEdit.Text, dntbAskedFlaggedQ.Text, dntbAskedQ.Text, dntbAskedQVotedDown.Text, dntbAskedQVotedUp.Text, dntbCreatedSynonym.Text, dntbCommented.Text, dntbEditedPost.Text, dntbEditedTag.Text, dntbEditedTagVotedDown.Text, dntbEditedTagVotedUp.Text, dntbFirstLoggedInView.Text, dntbProvidedA.Text, dntbProvidedAcceptedA.Text, dntbProvidedAVotedDown.Text, dntbProvidedAVotedUp.Text, dntbProvidedFlaggedA.Text, dntbVotedADown.Text, dntbVotedQDown.Text, dntbVotedSDown.Text, dntbVotedSUp.Text, dntbVotedTDown.Text, dntbVotedTUp.Text, dntbVotedAUp.Text, dntbVotedQUp.Text, dntbAcceptdQAnswer.Text));
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		public void Refresh()
		{
			if (Page.IsPostBack) return;
			var colUserScoring = Model.UserScoringActions;
			dntbApprovedPostEdit.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ApprovedPostEdit.ToString()).Value.ToString();
			dntbApproveTagEdit.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ApprovedTagEdit.ToString()).Value.ToString();
			dntbAskedFlaggedQ.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.AskedFlaggedQuestion.ToString()).Value.ToString();
			dntbAskedQ.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.AskedQuestion.ToString()).Value.ToString();
			dntbAskedQVotedDown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.AskedQuestionVotedDown.ToString()).Value.ToString();
			dntbAskedQVotedUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.AskedQuestionVotedUp.ToString()).Value.ToString();
			dntbCreatedSynonym.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.CreatedTagSynonym.ToString()).Value.ToString();
			dntbCommented.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.Commented.ToString()).Value.ToString();
			dntbEditedPost.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.EditedPost.ToString()).Value.ToString();
			dntbEditedTag.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.EditedTag.ToString()).Value.ToString();
			dntbEditedTagVotedDown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.EditedTagVotedDown.ToString()).Value.ToString();
			dntbEditedTagVotedUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.EditedTagVotedUp.ToString()).Value.ToString();
			dntbFirstLoggedInView.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.FirstLoggedInView.ToString()).Value.ToString();
			dntbProvidedA.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ProvidedAnswer.ToString()).Value.ToString();
			dntbProvidedAcceptedA.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ProvidedAcceptedAnswer.ToString()).Value.ToString();
			dntbProvidedAVotedDown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ProvidedAnswerVotedDown.ToString()).Value.ToString();
			dntbProvidedAVotedUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ProvidedAnswerVotedUp.ToString()).Value.ToString();
			dntbProvidedFlaggedA.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.ProvidedFlaggedAnswer.ToString()).Value.ToString();
			dntbVotedADown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedDownAnswer.ToString()).Value.ToString();
			dntbVotedQDown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedDownQuestion.ToString()).Value.ToString();
			dntbVotedSDown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedSynonymDown.ToString()).Value.ToString();
			dntbVotedSUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedSynonymUp.ToString()).Value.ToString();
			dntbVotedTDown.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedTagDown.ToString()).Value.ToString();
			dntbVotedTUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedTagUp.ToString()).Value.ToString();
			dntbVotedAUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedUpAnswer.ToString()).Value.ToString();
			dntbVotedQUp.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.VotedUpQuestion.ToString()).Value.ToString();
			dntbAcceptdQAnswer.Text = colUserScoring.Single(s => s.Key == Constants.UserScoringActions.AcceptedQuestionAnswer.ToString()).Value.ToString();

			hlCancel.NavigateUrl = Links.Home(ModuleContext.TabId);
		}

		#endregion

	}
}