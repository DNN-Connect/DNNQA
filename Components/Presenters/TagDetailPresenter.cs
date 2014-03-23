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
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class TagDetailPresenter : ModulePresenter<ITagDetailView, TagDetailModel>
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
		private string ControlView
		{
			get
			{
				var controlView = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["view"]))
				{
					controlView = (Request.Params["view"]);
				}
				return controlView;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> ThresholdCollection
		{
			get
			{
				return QaSettings.GetOpThresholdCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
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
		public TagDetailPresenter(ITagDetailView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public TagDetailPresenter(ITagDetailView view, IDnnqaController controller)
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
		/// <remarks>There is a potential problem here. If a user attempts to view a term that is not used in the module (need to investigate).</remarks>
		protected void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				var objTermApprove =
					ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymApproveCount.ToString());
				var objTermReject = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymRejectCount.ToString());
				var objMaxSynonym = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymMaxCount.ToString());
				var synonymCount = 0;

				var urlTerm =
					(from t in Controller.GetTermsByContentType(ModuleContext.PortalId, ModuleContext.ModuleId, VocabularyId)
					 where t.Name.ToLower() == Tag.ToLower()
					 select t).SingleOrDefault();

				if (urlTerm != null)
				{
					var portalSynonyms = Controller.GetTermSynonyms(ModuleContext.PortalId);
					var activeSynonyms =
						(from t in portalSynonyms
						 where t.MasterTermId == urlTerm.TermId && t.Score >= objTermApprove.Value
						 select t)
							.ToList();
					View.Model.SuggestedTermSynonyms =
						(from t in portalSynonyms
						 where ((t.MasterTermId == urlTerm.TermId) && ((t.Score < objTermApprove.Value) && (t.Score > -objTermReject.Value))) select t).ToList();
					View.Model.SelectedTerm = urlTerm;
					var colTerms = new List<TermInfo>();

					foreach (var objSynonym in activeSynonyms)
					{
						var synonym = objSynonym;        

						var termTerm = (from t in Controller.GetTermsByContentType(ModuleContext.PortalId, ModuleContext.ModuleId, VocabularyId)
										where t.TermId == synonym.RelatedTermId
										select t).SingleOrDefault();

						if (termTerm == null)
						{
							// the term is not currently used within this module, go to core taxonomy store
							var objTerm = (from t in Util.GetTermController().GetTermsByVocabulary(1)
										   where t.TermId == synonym.RelatedTermId
										   select t).SingleOrDefault();

							var objTermInfo = new TermInfo
												  {
													  DayTermUsage = 0,
													  TotalTermUsage = 0,
													  MonthTermUsage = 0,
													  WeekTermUsage = 0,
													  TermId = objTerm.TermId,
													  Name = objTerm.Name,
													  Description = objTerm.Description
												  };

							colTerms.Add(objTermInfo);
						}
						else
						{
							// in taxonomy
							colTerms.Add(termTerm);
						}
					}

					View.Model.ActiveTermSynonyms = colTerms;
					// we need to make sure we don't go above the max synonym count;
					synonymCount = View.Model.ActiveTermSynonyms.Count + View.Model.SuggestedTermSynonyms.Count;
				}
				else
				{
					// check to make sure this is a term in taxonomy (maybe it just wasn't associated with the content type)
					var objTerm = (from t in Util.GetTermController().GetTermsByVocabulary(1)
								   where t.Name.ToLower() == Tag.ToLower()
								   select t).SingleOrDefault();
					if (objTerm != null)
					{
						var objTermInfo = new TermInfo();

						View.Model.ActiveTermSynonyms = new List<TermInfo>();
						View.Model.SuggestedTermSynonyms = new List<TermSynonymInfo>();
						objTermInfo.DayTermUsage = 0;
						objTermInfo.TotalTermUsage = 0;
						objTermInfo.MonthTermUsage = 0;
						objTermInfo.WeekTermUsage = 0;
						objTermInfo.TermId = objTerm.TermId;
						objTermInfo.Name = objTerm.Name;
						objTermInfo.Description = objTerm.Description;
						View.Model.SelectedTerm = objTermInfo;
					}
					else
					{
					Response.Redirect(Globals.AccessDeniedURL("AccessDenied"), false);
					}
				}

				View.Model.SelectedView = ControlView;
				View.ItemDataBound += ItemDataBound;
				View.AddSynonym += AddSynonym;
				View.DeleteSynonym += DeleteSynonym;

				View.ShowActiveSynonyms(View.Model.ActiveTermSynonyms.Count > 0);

				if (synonymCount < objMaxSynonym.Value)
				{
					// check priv.
					var objSuggestPriv = PrivilegeCollection.Single(s => s.Key == Constants.Privileges.CreateTagSynonym.ToString());

					if (objSuggestPriv != null)
					{
				//        var objMinSynonymCreateAnswer =
				//colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserTermSynonymCreateMinAnswerCount.ToString());

						if ((UserScore.Score >= objSuggestPriv.Value || ModuleContext.IsEditable))
						{
							View.ShowAddSynonym(true);
							View.ShowSuggestedSynonyms(true);
						}
						else
						{
							View.ShowAddSynonym(false);
							View.ShowSuggestedSynonyms(View.Model.SuggestedTermSynonyms.Count > 0);
						}
					}  
				}
				else
				{
					View.ShowAddSynonym(false);
					View.ShowSuggestedSynonyms(View.Model.SuggestedTermSynonyms.Count > 0);
				}
			
				switch (View.Model.SelectedView.ToLower())
				{
					case "termsynonyms":
						View.Model.PageTitle = Localization.GetString("SynonymMetaTitle", LocalResourceFile).Replace("[0]", View.Model.SelectedTerm.Name);
						View.Model.PageDescription = Localization.GetString("SynonymMetaDescription", LocalResourceFile).Replace("[0]", View.Model.SelectedTerm.Name);
						break;
					default:
						View.Model.PageTitle  = Localization.GetString("DetailMetaTitle", LocalResourceFile).Replace("[0]", View.Model.SelectedTerm.Name); ;
						View.Model.PageDescription = View.Model.SelectedTerm.Description;
						break;
				}

				// this should only factor in privileges/moderation + any op thresholds

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
		protected void ItemDataBound(object sender, TermSynonymListEventArgs<Controls.Voting, Controls.Tags, TermSynonymInfo, ImageButton> e)
		{
			// because a term may not be assigned to a question (and therefore not available via content type), we need to get the core term (we know it exists at core level @)
			var objTermInfo = new TermInfo();

			var termTerm = (from t in Controller.GetTermsByContentType(ModuleContext.PortalId, ModuleContext.ModuleId, VocabularyId)
							 where t.TermId == e.TermSynonym.RelatedTermId
							 select t).SingleOrDefault();

			if (termTerm == null)
			{
				var objTerm = (from t in Util.GetTermController().GetTermsByVocabulary(1)
							   where t.TermId == e.TermSynonym.RelatedTermId
							   select t).SingleOrDefault();
				objTermInfo.DayTermUsage = 0;
				objTermInfo.TotalTermUsage = 0;
				objTermInfo.MonthTermUsage = 0;
				objTermInfo.WeekTermUsage = 0;
				objTermInfo.TermId = objTerm.TermId;
				objTermInfo.Name = objTerm.Name;
				objTermInfo.Description = objTerm.Description;
			}
			else
			{
				objTermInfo.DayTermUsage = termTerm.DayTermUsage;
				objTermInfo.TotalTermUsage = termTerm.TotalTermUsage;
				objTermInfo.MonthTermUsage = termTerm.MonthTermUsage;
				objTermInfo.WeekTermUsage = termTerm.WeekTermUsage;
				objTermInfo.TermId = termTerm.TermId;
				objTermInfo.Name = termTerm.Name;
				objTermInfo.Description = termTerm.Description;
			}

			var colTerms = new List<TermInfo> { objTermInfo };

			if (colTerms.Count > 0)
			{
				e.Tags.ModContext = ModuleContext;
                e.Tags.ModuleTab = ModuleContext.PortalSettings.ActiveTab;
				e.Tags.DataSource = colTerms;
				e.Tags.DataBind();
			}

			e.Voting.QuestionID = -1;
			e.Voting.CurrentPostID = -1;
			e.Voting.TermId = View.Model.SelectedTerm.TermId;
			e.Voting.RelatedTermId = e.TermSynonym.RelatedTermId;
			e.Voting.ModContext = ModuleContext;

			e.ImageButton.CommandArgument = e.TermSynonym.RelatedTermId.ToString();
			e.ImageButton.ID = "imgDelete_" + e.TermSynonym.RelatedTermId;
			e.ImageButton.AlternateText = Localization.GetString("DeleteSynonym", LocalResourceFile);
			e.ImageButton.ToolTip = Localization.GetString("DeleteSynonym", LocalResourceFile);
			// this should only show to moderators
			e.ImageButton.Visible = ModuleContext.IsEditable;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void AddSynonym(object sender, AddTermSynonymEventArgs<string> e)
		{
			var objTerm = Terms.CreateAndReturnTerm(e.TermName, VocabularyId);
			var portalSynonyms = Controller.GetTermSynonyms(ModuleContext.PortalId);
			var colMaster = (from t in portalSynonyms where t.MasterTermId == objTerm.TermId select t).ToList();
			var colRelated = (from t in portalSynonyms where t.RelatedTermId == objTerm.TermId select t).ToList();

			View.Model.ErrorMessage = "";

			if (colMaster.Count > 0)
			{
				View.Model.ErrorMessage = "AlreadyMaster";
				return;
			}
			if (colRelated.Count > 0)
			{
				var colSameTerm =
					(from t in portalSynonyms
					 where ((t.MasterTermId == View.Model.SelectedTerm.TermId) && (t.RelatedTermId == objTerm.TermId))
					 select t).ToList();

				View.Model.ErrorMessage = colSameTerm.Count > 0 ? "SameRelationSuggested" : "AlreadyRelated";
				return;
			}

			var objSynonym = new TermSynonymInfo
								 {
									 MasterTermId = View.Model.SelectedTerm.TermId,
									 RelatedTermId = objTerm.TermId,
									 PortalId = ModuleContext.PortalId,
									 CreatedByUserId = ModuleContext.PortalSettings.UserId,
									 CreatedOnDate = DateTime.Now
								 };
			Controller.AddTermSynonym(objSynonym);

			var updatedPortalSynonyms = Controller.GetTermSynonyms(ModuleContext.PortalId);
			var objTermApprove =
				ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymApproveCount.ToString());
			var objTermReject = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymRejectCount.ToString());

			View.Model.SuggestedTermSynonyms =
						(from t in updatedPortalSynonyms
						 where ((t.MasterTermId == View.Model.SelectedTerm.TermId) && ((t.Score < objTermApprove.Value) && (t.Score > -objTermReject.Value)))
						 select t).ToList();

			View.ShowSuggestedSynonyms(true);
			View.ShowActiveSynonyms(View.Model.ActiveTermSynonyms.Count > 0);
			View.ShowAddSynonym(true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DeleteSynonym(object sender, DeleteSuggestedSynonymEventArgs<ImageButton> e)
		{
			var relatedTermId = Convert.ToInt32(e.ImageButton.CommandArgument);

			if (relatedTermId > 0)
			{
				var scoringCollection = Controller.GetUserScoreLogByKey(relatedTermId, ModuleContext.PortalId);

				if (scoringCollection.Count > 0)
				{
					// Need to remove all logged items associated w/ this term synonym, also reset the user's score
					foreach (var objScoreLog in scoringCollection)
					{
						Controller.DeleteUserScoreLog(objScoreLog.UserId, ModuleContext.PortalId, objScoreLog.UserScoringActionId, objScoreLog.Score, relatedTermId);
					}                   
				}

				Controller.DeleteTermSynonym(View.Model.SelectedTerm.TermId, relatedTermId, ModuleContext.PortalId);

				var portalSynonyms = Controller.GetTermSynonyms(ModuleContext.PortalId);
				var objTermApprove =
					ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymApproveCount.ToString());
				var objTermReject = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.TermSynonymRejectCount.ToString());

				View.Model.SuggestedTermSynonyms =
							(from t in portalSynonyms
							 where ((t.MasterTermId == View.Model.SelectedTerm.TermId) && ((t.Score < objTermApprove.Value) && (t.Score > -objTermReject.Value)))
							 select t).ToList();

				View.ShowAddSynonym(true);
				View.ShowSuggestedSynonyms(true);
				//View.ShowActiveSynonyms(View.Model.ActiveTermSynonyms.Count > 0);
			}	
		}

		#endregion

	}
}