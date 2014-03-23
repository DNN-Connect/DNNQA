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
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using System;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.UI.Modules;

namespace DotNetNuke.DNNQA.Controls
{

	/// <summary>
	/// 
	/// </summary>
	[DefaultProperty("QuestionID"), Themeable(false)]
	[ToolboxData("<{0}:Voting runat=server> </{0}:Voting>")]
	public class Voting : CompositeControl
	{

		#region Private Members

		protected IDnnqaController Controller { get; private set; }
		public event EventHandler VoteClick;

		private LinkButton _voteUp;
		private LinkButton _voteDown;

		private static readonly object EventSubmitKey = new object();
	
		private IEnumerable<QaSettingInfo> ThresholdCollection
		{
			get
			{
				return QaSettings.GetOpThresholdCollection(Controller.GetQaPortalSettings(ModContext.PortalId), ModContext.PortalId);
			}
		}

		private IEnumerable<QaSettingInfo> UserScoringCollection
		{
			get
			{
				return QaSettings.GetUserScoringCollection(Controller.GetQaPortalSettings(ModContext.PortalId), ModContext.PortalId);
			}
		}

		private IEnumerable<QaSettingInfo> PrivilegeCollection
		{
			get
			{
				return QaSettings.GetPrivilegeCollection(Controller.GetQaPortalSettings(ModContext.PortalId), ModContext.PortalId);
			}
		}

		private UserScoreInfo UserScore
		{
			get
			{
				if (ModContext.PortalSettings.UserId > 0)
				{
					var usersScore = Controller.GetUserScore(ModContext.PortalSettings.UserId, ModContext.PortalId);
					if (usersScore != null)
					{
						return usersScore;
					}
				}
				var objUserScore = new UserScoreInfo
				{
					Message = "",
					PortalId = ModContext.PortalId,
					UserId = ModContext.PortalSettings.UserId,
					Score = 0
				};
				return objUserScore;
			}
		}

		private QuestionInfo Question
		{
			get { return Controller.GetQuestion(QuestionID, ModContext.PortalId); }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public Voting()
			: this(new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="controller"></param>
		public Voting(IDnnqaController controller)
		{
			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The QuestionID is used to update LastActivityDate in the posts table for the question/answer being voted on.
		/// </summary>
		/// <remarks>This only needs populated for question/answer up/down voting.</remarks>
		[Browsable(false)]
		public int QuestionID {
			get { return ViewState["QuestionID"] != null ? (int)ViewState["QuestionID"] : 0; }
			set { ViewState["QuestionID"] = value; }
		}

		/// <summary>
		/// The post we are associating the implemented  control with.
		/// </summary>
		[Browsable(false)]
		public int CurrentPostID
		{
			get
			{
				var currentPostId = 0;
				if (ViewState["CurrentPostID"] != null)
				{
					currentPostId = Convert.ToInt32(ViewState["CurrentPostID"]);
				}
				return currentPostId;
			}
			set { ViewState["CurrentPostID"] = value; }
		}

		/// <summary>
		/// This represents the termid (in vote mode = term) or the MasterTermId in vote mode = Synonym. 
		/// </summary>
		[Browsable(false)]
		public int TermId
		{
			get { return ViewState["TermId"] != null ? (int)ViewState["TermId"] : 0; }
			set { ViewState["TermId"] = value; }
		}

		/// <summary>
		/// This would be the related termid and is only necessary in view mode = synonym.
		/// </summary>
		[Browsable(false)]
		public int RelatedTermId
		{
			get { return ViewState["RelatedTermId"] != null ? (int)ViewState["RelatedTermId"] : 0; }
			set { ViewState["RelatedTermId"] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Bindable(true), Browsable(true), Category("Behavior"), DefaultValue(""),
		 Description("The voting mode determines what text to display to the user (is it question or answer specific?)."), Localizable(false)]
		public Constants.VoteMode VotingMode
		{
			get
			{
				var voteMode = Constants.VoteMode.Answer;
				if (ViewState["VotingMode"] != null)
				{
					voteMode = (Constants.VoteMode)(ViewState["VotingMode"]);
				}
				return voteMode;
			}
			set { ViewState["VotingMode"] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public ModuleInstanceContext ModContext { get; set; }

		/// <summary>
		/// This provides a full path to the shared resource file for localization. 
		/// </summary>
		[Browsable(false)]
		private string SharedResourceFile
		{
			get { return ResolveUrl(Constants.SharedResourceFileName); }
		}

		#endregion
		
		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		protected override void RecreateChildControls() {
			EnsureChildControls();
		}

		/// <summary>
		/// Adds the necessary control(s) to the controls collection.
		/// </summary>
		protected override void CreateChildControls() 
		{
			base.CreateChildControls();
			Controls.Clear();

			_voteUp = new LinkButton
						{
							ID = String.Concat(ID, "_VoteUp"),
							CausesValidation = false,
							CommandArgument = "1"
						};

			_voteDown = new LinkButton
						{
							ID = String.Concat(ID, "_VoteDown"),
							CausesValidation = false,
							CommandArgument = "-1"
						};

			_voteUp.Command += VoteCommand;
			_voteDown.Command += VoteCommand;

			Controls.Add(_voteUp);
			Controls.Add(_voteDown);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender(e);

			// setup control props
			EnsureChildControls();
		}

		/// <summary>
		/// Creates the user interface.
		/// </summary>
		/// <param name="writer"></param>
		/// <remarks></remarks>
		protected override void Render(HtmlTextWriter writer)
		{
			RenderControlUi(writer);
		}

		/// <summary>
		/// The Vote event.
		/// </summary>
		/// <remarks>This is normally done behind the scenes by .net, implemented here for performance reasons.</remarks>
		[Category("Action"),
		Description("Raised when the user clicks either of the vote buttons.")]
		public event EventHandler Vote {
			add {
				Events.AddHandler(EventSubmitKey, value);
			}
			remove {
				Events.RemoveHandler(EventSubmitKey, value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		protected void VoteCommand(object source, CommandEventArgs e) {
			if (VoteClick != null) {
				VoteClick(this, e);
			}

			var voteValue = Convert.ToInt32(e.CommandArgument);
			SaveVote(voteValue);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <returns></returns>
		private void RenderControlUi(HtmlTextWriter writer)
		{
			SetVoteOptions();

			// <div>
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaVotingArea");
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);

			// <div>
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaVoteUpLink");
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			_voteUp.RenderControl(writer);
			// </div>
			writer.RenderEndTag();

			var voteValue = 0;

			switch (VotingMode)
			{
				case Constants.VoteMode.Synonym :
					var portalSynonyms = Controller.GetTermSynonyms(ModContext.PortalId);
						var objSynonym = (from t in portalSynonyms where t.RelatedTermId == RelatedTermId select t).SingleOrDefault();
						if (objSynonym != null)
						{
							voteValue = objSynonym.Score;
						}
					break;
				//case Constants.VoteMode.Term :

				//    break;
				default:
					if (ModContext != null)
					{
						var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);

						if (objPost != null)
						{
							voteValue = objPost.Score;
						}  
					}
					break;
			}

			// <div>
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaVoteCount");
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			// <label>
			writer.AddAttribute(HtmlTextWriterAttribute.Name, "lblVoteValue");
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(voteValue);
			// </label>
			writer.RenderEndTag();
			// </div>
			writer.RenderEndTag();

			// <div>
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaVoteDownLink");
			writer.RenderBeginTag(HtmlTextWriterTag.Li);
			_voteDown.RenderControl(writer);
			// </div>
			writer.RenderEndTag();

			// </div>
			writer.RenderEndTag();

			//return writer;
		}

		/// <summary>
		/// 
		/// </summary>
		private void SetVoteOptions()
		{
			if (ModContext != null)
			{
				_voteUp.Text = Services.Localization.Localization.GetString("VoteUp", SharedResourceFile);
				_voteDown.Text = Services.Localization.Localization.GetString("VoteDown", SharedResourceFile);

				VoteInfo objUserVote;
				switch (VotingMode)
				{
					case Constants.VoteMode.Synonym:
						var col = Controller.GetTermSynonymVotes(RelatedTermId, ModContext.PortalId);
						objUserVote = (from t in col where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && ((t.VoteTypeId == (int)Constants.VoteType.VoteSynonymUp) || (t.VoteTypeId == (int)Constants.VoteType.VoteSynonymDown))) select t).SingleOrDefault();
						break;
					//case Constants.VoteMode.Term:
					//    objUserVote = (from t in Controller.GetPostVotes(CurrentPostID) where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.FlagPost)) select t).SingleOrDefault();
						//break;
					default: // question/answer
						var colPostVotes = Controller.GetPostVotes(CurrentPostID);
						objUserVote = (from t in colPostVotes where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && ((t.VoteTypeId == (int)Constants.VoteType.VoteUpPost) || (t.VoteTypeId == (int)Constants.VoteType.VoteDownPost))) select t).SingleOrDefault();
						break;
				}

				if (objUserVote != null)
				{
					switch (objUserVote.VoteTypeId)
					{
						case (int)Constants.VoteType.VoteDownPost:
							_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteUpTitle" + VotingMode, SharedResourceFile);
							_voteUp.Enabled = false;
							_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteDownSelectedTitle" + VotingMode, SharedResourceFile);
							_voteDown.CssClass = "selected";
							break;
						case (int)Constants.VoteType.VoteSynonymDown:
							_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteUpTitle" + VotingMode, SharedResourceFile);
							_voteUp.Enabled = false;
							_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteDownSelectedTitle" + VotingMode, SharedResourceFile);
							_voteDown.CssClass = "selected";
							break;
						case (int)Constants.VoteType.VoteUpPost:
							_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteDownTitle" + VotingMode, SharedResourceFile);
							_voteDown.Enabled = false;
							_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteUpSelectedTitle" + VotingMode, SharedResourceFile);
							_voteUp.CssClass = "selected";
							break;
						case (int)Constants.VoteType.VoteSynonymUp:
							_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteDownTitle" + VotingMode, SharedResourceFile);
							_voteDown.Enabled = false;
							_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteUpSelectedTitle" + VotingMode, SharedResourceFile);
							_voteUp.CssClass = "selected";
							break;
					}
				}
				else
				{
					_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteUpTitle" + VotingMode, SharedResourceFile);
					_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteDownTitle" + VotingMode, SharedResourceFile);

					var privVoteUp = PrivilegeCollection.Single(s => s.Key == Constants.Privileges.VoteUp.ToString());
					var privVoteDown = PrivilegeCollection.Single(s => s.Key == Constants.Privileges.VoteDown.ToString());

					switch (VotingMode)
					{
						case Constants.VoteMode.Synonym:
							var colPortalSynonyms = Controller.GetTermSynonyms(ModContext.PortalId);
							var objSynonym = (from t in colPortalSynonyms where ((t.RelatedTermId == RelatedTermId) && (t.PortalId == ModContext.PortalId) && (t.MasterTermId == TermId)) select t).SingleOrDefault();

							if (objSynonym.CreatedByUserId == ModContext.PortalSettings.UserId)
							{
								_voteUp.Enabled = false;
								_voteDown.Enabled = false;
								_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteOwnSynonym", SharedResourceFile);
								_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteOwnSynonym", SharedResourceFile);
							}
							else
							{
								_voteUp.Enabled = ((UserScore.Score >= privVoteUp.Value) || (ModContext.IsEditable));
								_voteDown.Enabled = ((UserScore.Score >= privVoteDown.Value) || (ModContext.IsEditable));
							}
							break;
						//case Constants.VoteMode.Term:
						//    // currently not supported
						//        _voteUp.Enabled = ((UserScore >= privVoteUp.Value) || (ModContext.IsEditable));
						//        _voteDown.Enabled = ((UserScore >= privVoteDown.Value) || (ModContext.IsEditable));
						//    break;
						default : // question/answer
							var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);

							if (objPost != null)
							{
								if (objPost.CreatedUserId == ModContext.PortalSettings.UserId)
								{
									_voteUp.Enabled = false;
									_voteDown.Enabled = false;
									_voteUp.ToolTip = Services.Localization.Localization.GetString("VoteOwnPost", SharedResourceFile);
									_voteDown.ToolTip = Services.Localization.Localization.GetString("VoteOwnPost", SharedResourceFile);
								}
								else
								{
									if ((UserScore.Score >= privVoteUp.Value) || (ModContext.IsEditable))
									{
										_voteUp.Enabled = true;
									}
									else
									{
										_voteUp.Enabled = false;
										_voteUp.ToolTip = Services.Localization.Localization.GetString("NoUpVotePrivs", SharedResourceFile);

									}
									if ((UserScore.Score >= privVoteDown.Value) || (ModContext.IsEditable))
									{
										_voteDown.Enabled = true;
									}
									else
									{
										_voteDown.Enabled = false;
										_voteDown.ToolTip = Services.Localization.Localization.GetString("NoDownVotePrivs", SharedResourceFile);
									}
								}
							}

							break;
					}
				}				
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="voteValue"></param>
		private void SaveVote(int voteValue)
		{
			var objVote = new VoteInfo { CreatedByUserId = ModContext.PortalSettings.UserId, CreatedOnDate = DateTime.Now, TermId = -1, PostId = -1, PortalId = ModContext.PortalId };

			switch (VotingMode)
			{
				case Constants.VoteMode.Synonym:
					var colTermVotes = Controller.GetTermSynonymVotes(RelatedTermId, ModContext.PortalId);

					switch (voteValue)
					{
						case -1:
							var voteSDown = UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedSynonymDown.ToString()).Value;

							objVote.VoteTypeId = (int)Constants.VoteType.VoteSynonymDown;
							objVote.TermId = RelatedTermId;
	
							var objUserVoteSd = (from t in colTermVotes where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.VoteSynonymDown)) select t).SingleOrDefault();

							if (objUserVoteSd != null)
							{
								Controller.DeleteUserScoreLog(ModContext.PortalSettings.UserId, ModContext.PortalId, (int)Constants.UserScoringActions.VotedSynonymDown, voteSDown, RelatedTermId);
							}
							else
							{
								var objScoreLog = new UserScoreLogInfo
								{
									UserId = ModContext.PortalSettings.UserId,
									PortalId = ModContext.PortalId,
									UserScoringActionId = (int)Constants.UserScoringActions.VotedSynonymDown,
									Score = voteSDown,
									KeyId = RelatedTermId,
									CreatedOnDate = DateTime.Now
								};

								Controller.AddScoringLog(objScoreLog, PrivilegeCollection);
								VotingThresholdCheck(Constants.OpThresholds.TermSynonymRejectCount, Constants.VoteType.VoteSynonymDown);
							}
							break;
						case 1:
							var voteSUp = UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedSynonymUp.ToString()).Value;

							objVote.VoteTypeId = (int)Constants.VoteType.VoteSynonymUp;
							objVote.TermId = RelatedTermId;

							var objUserVoteSu = (from t in colTermVotes where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.VoteSynonymUp)) select t).SingleOrDefault();
							
							if (objUserVoteSu != null)
							{
								Controller.DeleteUserScoreLog(ModContext.PortalSettings.UserId, ModContext.PortalId, (int)Constants.UserScoringActions.VotedSynonymUp, voteSUp, RelatedTermId);
							}
							else
							{
								var objScoreLog = new UserScoreLogInfo
								{
									UserId = ModContext.PortalSettings.UserId,
									PortalId = ModContext.PortalId,
									UserScoringActionId = (int)Constants.UserScoringActions.VotedSynonymUp,
									Score = voteSUp,
									KeyId = RelatedTermId,
									CreatedOnDate = DateTime.Now
								};

								Controller.AddScoringLog(objScoreLog, PrivilegeCollection);
								VotingThresholdCheck(Constants.OpThresholds.TermSynonymApproveCount, Constants.VoteType.VoteSynonymUp);
							}
							break;
					}
					break;
				//case Constants.VoteMode.Term:
				//    //objVote.VoteTypeId = (int)Constants.VoteType.FlagPost;
				//    break;
				default: // post=question/answer
					var colPostVotes = Controller.GetPostVotes(CurrentPostID);

					switch (voteValue)
					{
						case -1:
							objVote.VoteTypeId = (int)Constants.VoteType.VoteDownPost;
							objVote.PostId = CurrentPostID;

							var objUserPostVoteD = (from t in colPostVotes where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.VoteDownPost)) select t).SingleOrDefault();

							if (objUserPostVoteD != null)
							{
								if (VotingMode == Constants.VoteMode.Question)
								{
									Controller.DeleteUserScoreLog(ModContext.PortalSettings.UserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.VotedDownQuestion, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedDownQuestion.ToString()).Value,
																  CurrentPostID);

									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);
									Controller.DeleteUserScoreLog(objPost.CreatedUserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.AskedQuestionVotedDown, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.AskedQuestionVotedDown.ToString()).Value,
																  CurrentPostID);
								}
								else
								{
									Controller.DeleteUserScoreLog(ModContext.PortalSettings.UserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.VotedDownAnswer, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedDownAnswer.ToString()).Value,
																  CurrentPostID);

									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);
									Controller.DeleteUserScoreLog(objPost.CreatedUserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.ProvidedAnswerVotedDown, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.ProvidedAnswerVotedDown.ToString()).Value,
																  CurrentPostID);
								}
							}
							else
							{					
								var objScoreLogD = new UserScoreLogInfo
								{
									UserId = ModContext.PortalSettings.UserId,
									PortalId = ModContext.PortalId,
									KeyId = CurrentPostID,
									CreatedOnDate = DateTime.Now
								};

								if (VotingMode == Constants.VoteMode.Question)
								{
									objScoreLogD.UserScoringActionId = (int)Constants.UserScoringActions.VotedDownQuestion;
									objScoreLogD.Score = UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedDownQuestion.ToString()).Value;

									Controller.AddScoringLog(objScoreLogD, PrivilegeCollection);

									// handle reputation credit for question up-vote
									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);

									var objUserScoringLog = new UserScoreLogInfo
									{
										UserId = objPost.CreatedUserId,
										PortalId = ModContext.PortalId,
										KeyId = CurrentPostID,
										CreatedOnDate = DateTime.Now,
										UserScoringActionId = (int)Constants.UserScoringActions.AskedQuestionVotedDown,
										Score =
											UserScoringCollection.Single(
												s =>
												s.Key == Constants.UserScoringActions.AskedQuestionVotedDown.ToString()).
											Value
									};
									Controller.AddScoringLog(objUserScoringLog, PrivilegeCollection);

									//VotingThresholdCheck(Constants.OpThresholds.UserDownVoteQuestionCount, Constants.VoteType.VoteDownPost);
								}
								else
								{
									objScoreLogD.UserScoringActionId = (int)Constants.UserScoringActions.VotedDownAnswer;
									objScoreLogD.Score = UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedDownAnswer.ToString()).Value;

									Controller.AddScoringLog(objScoreLogD, PrivilegeCollection);

									// handle reputation credit for answer up-vote
									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);

									var objUserScoringLog = new UserScoreLogInfo
									{
										UserId = objPost.CreatedUserId,
										PortalId = ModContext.PortalId,
										KeyId = CurrentPostID,
										CreatedOnDate = DateTime.Now,
										UserScoringActionId = (int)Constants.UserScoringActions.ProvidedAnswerVotedDown,
										Score =
											UserScoringCollection.Single(
												s =>
												s.Key == Constants.UserScoringActions.ProvidedAnswerVotedDown.ToString()).
											Value
									};
									Controller.AddScoringLog(objUserScoringLog, PrivilegeCollection);

									//VotingThresholdCheck(Constants.OpThresholds.UserDownVoteAnswerCount, Constants.VoteType.VoteDownPost);
								}
							}

							break;
						case 1:
							objVote.VoteTypeId = (int)Constants.VoteType.VoteUpPost;
							objVote.PostId = CurrentPostID;

							var objUserPostVoteU = (from t in colPostVotes where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.VoteUpPost)) select t).SingleOrDefault();

							if (objUserPostVoteU != null)
							{
								if (VotingMode == Constants.VoteMode.Question)
								{
									Controller.DeleteUserScoreLog(ModContext.PortalSettings.UserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.VotedUpQuestion, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedUpQuestion.ToString()).Value,
																  CurrentPostID);

									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);
									Controller.DeleteUserScoreLog(objPost.CreatedUserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.AskedQuestionVotedUp, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.AskedQuestionVotedUp.ToString()).Value,
																  CurrentPostID);
								}
								else
								{
									Controller.DeleteUserScoreLog(ModContext.PortalSettings.UserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.VotedUpAnswer, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedUpAnswer.ToString()).Value,
																  CurrentPostID);

									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);
									Controller.DeleteUserScoreLog(objPost.CreatedUserId, ModContext.PortalId,
																  (int)Constants.UserScoringActions.ProvidedAnswerVotedUp, UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.ProvidedAnswerVotedUp.ToString()).Value,
																  CurrentPostID);
								}
							}
							else
							{
								var objScoreLogU = new UserScoreLogInfo
								{
									UserId = ModContext.PortalSettings.UserId,
									PortalId = ModContext.PortalId,
									KeyId = CurrentPostID,
									CreatedOnDate = DateTime.Now
								};

								if (VotingMode == Constants.VoteMode.Question)
								{
									objScoreLogU.UserScoringActionId = (int)Constants.UserScoringActions.VotedUpQuestion;
									objScoreLogU.Score = UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedUpQuestion.ToString()).Value;

									Controller.AddScoringLog(objScoreLogU, PrivilegeCollection);

									// handle reputation credit for question up-vote
									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);

									var objUserScoringLog = new UserScoreLogInfo
																{
																	UserId = objPost.CreatedUserId,
																	PortalId = ModContext.PortalId,
																	KeyId = CurrentPostID,
																	CreatedOnDate = DateTime.Now,
																	UserScoringActionId = (int) Constants.UserScoringActions.AskedQuestionVotedUp,
																	Score =
																		UserScoringCollection.Single(
																			s =>
																			s.Key == Constants.UserScoringActions.AskedQuestionVotedUp.ToString()).
																		Value
																};
									Controller.AddScoringLog(objUserScoringLog, PrivilegeCollection);

									VotingThresholdCheck(Constants.OpThresholds.UserUpVoteQuestionCount, Constants.VoteType.VoteUpPost);
								}
								else
								{
									objScoreLogU.UserScoringActionId = (int)Constants.UserScoringActions.VotedUpAnswer;
									objScoreLogU.Score = UserScoringCollection.Single(s => s.Key == Constants.UserScoringActions.VotedUpAnswer.ToString()).Value;

									Controller.AddScoringLog(objScoreLogU, PrivilegeCollection);

									// handle reputation credit for answer up-vote
									var objPost = Controller.GetPost(CurrentPostID, ModContext.PortalId);

									var objUserScoringLog = new UserScoreLogInfo
									{
										UserId = objPost.CreatedUserId,
										PortalId = ModContext.PortalId,
										KeyId = CurrentPostID,
										CreatedOnDate = DateTime.Now,
										UserScoringActionId = (int)Constants.UserScoringActions.ProvidedAnswerVotedUp,
										Score =
											UserScoringCollection.Single(
												s =>
												s.Key == Constants.UserScoringActions.ProvidedAnswerVotedUp.ToString()).
											Value
									};
									Controller.AddScoringLog(objUserScoringLog, PrivilegeCollection);


									VotingThresholdCheck(Constants.OpThresholds.UserUpVoteAnswerCount, Constants.VoteType.VoteUpPost);
								}								
							}
							break;
					}
					break;
			}

			VoteInfo objExistingVote;
			switch (VotingMode)
			{
				case Constants.VoteMode.Synonym:
					var col = Controller.GetTermSynonymVotes(RelatedTermId, ModContext.PortalId);
					objExistingVote = (from t in col where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && ((t.VoteTypeId == (int)Constants.VoteType.VoteSynonymUp) || (t.VoteTypeId == (int)Constants.VoteType.VoteSynonymDown))) select t).SingleOrDefault();
					break;
				//case Constants.VoteMode.Term:
				//    objUserVote = (from t in Controller.GetPostVotes(CurrentPostID) where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && (t.VoteTypeId == (int)Constants.VoteType.FlagPost)) select t).SingleOrDefault();
				//break;
				default: // question/answer
					var colPostVotes = Controller.GetPostVotes(CurrentPostID);
					objExistingVote = (from t in colPostVotes where ((t.CreatedByUserId == ModContext.PortalSettings.UserId) && ((t.VoteTypeId == (int)Constants.VoteType.VoteUpPost) || (t.VoteTypeId == (int)Constants.VoteType.VoteDownPost))) select t).SingleOrDefault();
					break;
			}

			objVote.VoteId = Controller.AddVote(objVote, ModContext.ModuleId);

			if (objVote.VoteTypeId == Convert.ToInt32(Constants.VoteType.FlagPost)) return;
			if (objExistingVote != null)
			{
				var cntJournal = new Journal();
				cntJournal.RemoveVoteFromJournal(objExistingVote.VoteId, ModContext.ModuleId, ModContext.PortalId);
			}
			else
			{
				var cntJournal = new Journal();
				string summary;
				var title = Question.Title;
				var questionUrl = Links.ViewQuestion(Question.PostId, Question.Title, ModContext.PortalSettings.ActiveTab, ModContext.PortalSettings);

				switch (objVote.VoteTypeId)
				{
					case (int)Constants.VoteType.VoteDownPost:
						summary = Services.Localization.Localization.GetString(objVote.PostId == Question.PostId ? "VoteDownTitleQuestion" : "VoteDownTitleAnswer", SharedResourceFile);
						cntJournal.AddVoteToJournal(Question, objVote.VoteId, title, summary, ModContext.PortalId, ModContext.PortalSettings.UserId, questionUrl);
						break;
					case (int)Constants.VoteType.VoteSynonymDown:

						break;
					case (int)Constants.VoteType.VoteUpPost:
						summary = Services.Localization.Localization.GetString(objVote.PostId == Question.PostId ? "VoteUpTitleQuestion" : "VoteUpTitleAnswer", SharedResourceFile);
						cntJournal.AddVoteToJournal(Question, objVote.VoteId, title, summary, ModContext.PortalId, ModContext.PortalSettings.UserId, questionUrl);
						break;
					case (int)Constants.VoteType.VoteSynonymUp:

						break;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="threshold"></param>
		/// <param name="voteType"></param>
		private void VotingThresholdCheck(Constants.OpThresholds threshold, Constants.VoteType voteType)
		{
			var objThreshold = ThresholdCollection.Single(s => s.Key == threshold.ToString());
			var votes = new List<VoteInfo>();

			switch (voteType)
			{
				case Constants.VoteType.VoteDownPost:
					votes = (from t in Controller.GetPostVotes(CurrentPostID) where (t.VoteTypeId == (int)Constants.VoteType.VoteDownPost) select t).ToList();
					break;
				case Constants.VoteType.VoteUpPost:
					votes = (from t in Controller.GetPostVotes(CurrentPostID) where (t.VoteTypeId == (int)Constants.VoteType.VoteUpPost) select t).ToList();
					break;
				case Constants.VoteType.VoteSynonymUp:
					votes = (from t in Controller.GetTermSynonymVotes(RelatedTermId, ModContext.PortalId) where (t.VoteTypeId == (int)Constants.VoteType.VoteSynonymUp) select t).ToList();
					break;
				case Constants.VoteType.VoteSynonymDown:
					votes = (from t in Controller.GetTermSynonymVotes(RelatedTermId, ModContext.PortalId) where (t.VoteTypeId == (int)Constants.VoteType.VoteSynonymDown) select t).ToList();
					break;
					// term cases possible in future
			} 
			
			if ((votes.Count + 1) >= objThreshold.Value)
			{
				// we broke the threshold, log it
				switch (threshold)
				{
					case Constants.OpThresholds.TermSynonymApproveCount :
						// give the author any credit due for suggesting a sysnonym that was approved
						// we need to get the userId to given them some rep
						var colPortalSynonyms = Controller.GetTermSynonyms(ModContext.PortalId);
						var objSynonym = (from t in colPortalSynonyms where ((t.RelatedTermId == RelatedTermId) && (t.PortalId == ModContext.PortalId) && (t.MasterTermId == TermId)) select t).SingleOrDefault();

						var objScoreLogApprove = new UserScoreLogInfo
						{
							UserId = objSynonym.CreatedByUserId,
							PortalId = ModContext.PortalId,
							UserScoringActionId = (int)Constants.UserScoringActions.CreatedTagSynonym,
							Score =
								UserScoringCollection.Single(
									s => s.Key == Constants.UserScoringActions.CreatedTagSynonym.ToString()).Value,
							KeyId = RelatedTermId,
							CreatedOnDate = DateTime.Now
						};

						Controller.AddScoringLog(objScoreLogApprove, PrivilegeCollection);

						// TODO: handle remapping of tags (keep count)? (or do we only update the new posts added)

						break;
					case Constants.OpThresholds.TermSynonymRejectCount :
						// TODO: handle closing off term synonym (so it cannot be suggested again, any scoring actions)

						break;
					case Constants.OpThresholds.UserUpVoteQuestionCount :
						// a question can be voted up infinite times, take no action
						break;
					case Constants.OpThresholds.UserUpVoteAnswerCount :
						// an answer can be voted up infinite times, take no action
						break;
					//case Constants.OpThresholds.UserDownVoteQuestionCount :

					//    break;
					//case Constants.OpThresholds.UserDownVoteAnswerCount :

					//    break;
				}
			}
		}

		#endregion

	}
}