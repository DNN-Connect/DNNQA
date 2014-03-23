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
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.DNNQA {

	/// <summary>
	/// The Settings class manages Module Settings
	/// </summary>
	public partial class Settings : ModuleSettingsBase {

		#region Base Method Implementations

		/// <summary>
		/// LoadSettings loads the settings from the Database and displays them
		/// </summary>
		public override void LoadSettings() {
			try {
				if (!Page.IsPostBack)
				{
					// should be able to get the current tab's parentid and see if it ='s the user profile url. 

					if ((ModuleContext.PortalSettings.ActiveTab.ParentId == ModuleContext.PortalSettings.UserTabId) || (ModuleContext.TabId == ModuleContext.PortalSettings.UserTabId))
					{
						// we are in profile mode
						Response.Write("WE are in profile mode");
						pnlNormalSettings.Visible = false;
						pnlProfileSettings.Visible = true;



					}
					else
					{
						pnlNormalSettings.Visible = true;
						pnlProfileSettings.Visible = false;

						PopulateDropDowns();

						// Ask Question
						dntxtbxMinTitleChars.Value = Settings.ContainsKey(Constants.SettingMinTitleChars) ? Convert.ToInt32(Settings[Constants.SettingMinTitleChars]) : 3;
						dntxtbxMinBodyChars.Value = Settings.ContainsKey(Constants.SettingMinBodyChars) ? Convert.ToInt32(Settings[Constants.SettingMinBodyChars]) : 5;
						dntxtbxMaxTags.Value = Settings.ContainsKey(Constants.SettingMaxQuestionTags) ? Convert.ToInt32(Settings[Constants.SettingMaxQuestionTags]) : 5;
						chkAutoApprove.Checked = !Settings.ContainsKey(Constants.SettingAutoApprove) || Convert.ToBoolean(Settings[Constants.SettingAutoApprove]);

						// UI
						ddlNameFormat.SelectedValue = Settings.ContainsKey(Constants.SettingNameFormat)
														  ? (string)Settings[Constants.SettingNameFormat]
														  : Constants.DefaultNameFormat;
						dntHomePageSize.Value = Settings.ContainsKey(Constants.SettingHomePageSize)
													? Convert.ToInt32(Settings[Constants.SettingHomePageSize])
													: Constants.DefaultPageSize;
						dntHomeTagSize.Value = Settings.ContainsKey(Constants.SettingHomeMaxTags) ? Convert.ToInt32(Settings[Constants.SettingHomeMaxTags]) : Constants.DefaultHomeMaxTags;
						ddlHomeTagUsageType.SelectedValue = Settings.ContainsKey(Constants.SettingHomeTagTimeFrame)
																? (string)Settings[Constants.SettingHomeTagTimeFrame]
																: Constants.DefaultHomeTagTimeFrame;
						dntBrowseQPageSize.Value = Settings.ContainsKey(Constants.SettingBrowseQPageSize)
													   ? Convert.ToInt32(Settings[Constants.SettingBrowseQPageSize])
													   : Constants.DefaultPageSize;
						dnntAnswerPageSize.Value = Settings.ContainsKey(Constants.SettingAnswerPageSize) ? Convert.ToInt32(Settings[Constants.SettingAnswerPageSize]) : Constants.DefaultPageSize;
						dnntMaxTagsTags.Value = Settings.ContainsKey(Constants.SettingMaxTagsTags) ? Convert.ToInt32(Settings[Constants.SettingMaxTagsTags]) : Constants.DefaultPageSize;

						if (Settings.ContainsKey(Constants.SettingsFacebookAppId))
						{
							txtbxFacebookAppId.Text = Convert.ToString(Settings[Constants.SettingsFacebookAppId]);
						}

						chkEnablePlusOne.Checked = Settings.ContainsKey(Constants.SettingsEnablePlusOne)
															 ? Convert.ToBoolean(Settings[Constants.SettingsEnablePlusOne])
															 : true;
						chkEnableTwitter.Checked = Settings.ContainsKey(Constants.SettingsEnableTwitter)
															 ? Convert.ToBoolean(Settings[Constants.SettingsEnableTwitter])
															 : true;
						chkEnableLinkedIn.Checked = Settings.ContainsKey(Constants.SettingsEnableLinkedIn)
															 ? Convert.ToBoolean(Settings[Constants.SettingsEnableLinkedIn])
															 : true;

						// Tag Revision History

						BuildQaPortalSettings();
					}				
				}
			} catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		/// <summary>
		/// UpdateSettings saves the modified settings to the Database
		/// </summary>
		public override void UpdateSettings() {
			try {

				if ((ModuleContext.PortalSettings.ActiveTab.ParentId == ModuleContext.PortalSettings.UserTabId) || (ModuleContext.TabId == ModuleContext.PortalSettings.UserTabId))
				{
					// we are in profile mode
					UpdateProfileSettings();
				}
				else
				{
					UpdateNormalSettings();
				}
			} catch (Exception exc) //Module failed to load
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

		#region Private Methods

		private void UpdateNormalSettings()
		{
			var objModule = new ModuleController();

			objModule.UpdateModuleSetting(ModuleId, Constants.SettingNameFormat, ddlNameFormat.SelectedValue);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingAutoApprove, chkAutoApprove.Checked.ToString());
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingHomePageSize, dntHomePageSize.Text);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingHomeMaxTags, dntHomeTagSize.Text);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingHomeTagTimeFrame, ddlHomeTagUsageType.SelectedValue);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingBrowseQPageSize, dntBrowseQPageSize.Text);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingAnswerPageSize, dnntAnswerPageSize.Text);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingMaxTagsTags, dnntMaxTagsTags.Text);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingsFacebookAppId, txtbxFacebookAppId.Text);
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingsEnablePlusOne, chkEnablePlusOne.Checked.ToString());
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingsEnableTwitter, chkEnableTwitter.Checked.ToString());
			objModule.UpdateModuleSetting(ModuleId, Constants.SettingsEnableLinkedIn, chkEnableLinkedIn.Checked.ToString());

			UpdateQaPortalSettings();
		}

		private void UpdateProfileSettings()
		{
			
		}

		/// <summary>
		/// Uses localization to bind list of options for user name display (either username or display name, both located in the Users table).
		/// </summary>
		private void PopulateDropDowns() {
			ddlVocabRoot.Items.Clear();

			ddlNameFormat.Items.Clear();
			ddlNameFormat.Items.Insert(0, new ListItem(Localization.GetString("DisplayName", LocalResourceFile), "DisplayName"));
			ddlNameFormat.Items.Insert(1, new ListItem(Localization.GetString("Username", LocalResourceFile), "Username"));

			ddlHomeTagUsageType.Items.Clear();
			ddlHomeTagUsageType.Items.Insert(0, new ListItem(Localization.GetString("DailyUsage", LocalResourceFile), ((int)Constants.TagMode.ShowDailyUsage).ToString()));
			ddlHomeTagUsageType.Items.Insert(1, new ListItem(Localization.GetString("WeeklyUsage", LocalResourceFile), ((int)Constants.TagMode.ShowWeeklyUsage).ToString()));
			ddlHomeTagUsageType.Items.Insert(2, new ListItem(Localization.GetString("MonthlyUsage", LocalResourceFile), ((int)Constants.TagMode.ShowMonthlyUsage).ToString()));
			ddlHomeTagUsageType.Items.Insert(3, new ListItem(Localization.GetString("TotalUsage", LocalResourceFile), ((int)Constants.TagMode.ShowTotalUsage).ToString()));
		}

		/// <summary>
		/// 
		/// </summary>
		private void BuildQaPortalSettings()
		{
			var cntQa = new DnnqaController();
			var colOpThresholds = QaSettings.GetOpThresholdCollection(cntQa.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			dntbQCCVC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.QuestionCloseCompleteVoteCount.ToString()).Value.ToString();
			dntbQCWD.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.QuestionCloseWindowDays.ToString()).Value.ToString();
			dntbQFHRC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.QuestionFlagHomeRemoveCount.ToString()).Value.ToString();
			dntbPCVWM.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.PostChangeVoteWindowMinutes.ToString()).Value.ToString();
			dntbPFCC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.PostFlagCompleteCount.ToString()).Value.ToString();
			dntbPFWH.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.PostFlagWindowHours.ToString()).Value.ToString();
			dntbTCWD.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TagCloseWindowDays.ToString()).Value.ToString();
			dntbTFWH.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TagFlagWindowHours.ToString()).Value.ToString();
			dntbTFCC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TagFlagCompleteCount.ToString()).Value.ToString();
			dntbTSAC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TermSynonymApproveCount.ToString()).Value.ToString();
			dntbTSRC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TermSynonymRejectCount.ToString()).Value.ToString();
			dntbTSMC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.TermSynonymMaxCount.ToString()).Value.ToString();
			dntbUCVC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserCloseVoteCount.ToString()).Value.ToString();
			dntbUFPMC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserFlagPostModerateCount.ToString()).Value.ToString();
			dntbFPSC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserFlagPostSpamCount.ToString()).Value.ToString();
			dntbUTSCMAC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserTermSynonymCreateMinAnswerCount.ToString()).Value.ToString();
			dntbUTSVMASC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserTermSynonymVoteMinAnswerScoreCount.ToString()).Value.ToString();
			dntbUUVAC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserUpVoteAnswerCount.ToString()).Value.ToString();
			dntbUUVQC.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.UserUpVoteQuestionCount.ToString()).Value.ToString();
			dntbQHMS.Text = colOpThresholds.Single(s => s.Key == Constants.OpThresholds.QuestionHomeMinScore.ToString()).Value.ToString();

			var colEmail = QaSettings.GetEmailCollection(cntQa.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);            
			var currentEmail = colEmail.Single(s => s.Key == Constants.EmailSettings.FromAddress.ToString()).Value;
			txtbxFromEmail.Text = currentEmail != @"email@change.me" ? currentEmail : ModuleContext.PortalSettings.Email;

			var commentTemplate = colEmail.Single(s => s.Key == Constants.EmailSettings.CommentTemplate.ToString()).Value;
			txtbxCommentEmailTemplate.Text = commentTemplate;

			var singleQuestion = colEmail.Single(s => s.Key == Constants.EmailSettings.SingleQuestionTemplate.ToString()).Value;
			txtbxQuestionEmailTemplate.Text = singleQuestion;

			var answerTemplate = colEmail.Single(s => s.Key == Constants.EmailSettings.AnswerTemplate.ToString()).Value;
			txtAnswerEmailTemplate.Text = answerTemplate;

			var summaryTemplate = colEmail.Single(s => s.Key == Constants.EmailSettings.SummaryTemplate.ToString()).Value;
			txtbxSummaryEmailTemplate.Text = summaryTemplate;
		}

		/// <summary>
		/// 
		/// </summary>
		private void UpdateQaPortalSettings()
		{
			var cntQa = new DnnqaController();
			var objSetting = new SettingInfo
								 {
									 PortalId = ModuleContext.PortalId,
									 TypeId = (int) Constants.SettingTypes.OperationalThresholds,
									 Key = Constants.OpThresholds.QuestionCloseCompleteVoteCount.ToString(),
									 Value = dntbQCCVC.Text
								 };

			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.QuestionCloseWindowDays.ToString();
			objSetting.Value = dntbQCWD.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.QuestionFlagHomeRemoveCount.ToString();
			objSetting.Value = dntbQFHRC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.PostChangeVoteWindowMinutes.ToString();
			objSetting.Value = dntbPCVWM.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.PostFlagCompleteCount.ToString();
			objSetting.Value = dntbPFCC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.PostFlagWindowHours.ToString();
			objSetting.Value = dntbPFWH.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.TagCloseWindowDays.ToString();
			objSetting.Value = dntbTCWD.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.TagFlagWindowHours.ToString();
			objSetting.Value = dntbTFWH.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.TagFlagCompleteCount.ToString();
			objSetting.Value = dntbTFCC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.TermSynonymApproveCount.ToString();
			objSetting.Value = dntbTSAC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.TermSynonymRejectCount.ToString();
			objSetting.Value = dntbTSRC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.TermSynonymMaxCount.ToString();
			objSetting.Value = dntbTSMC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserCloseVoteCount.ToString();
			objSetting.Value = dntbUCVC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserFlagPostModerateCount.ToString();
			objSetting.Value = dntbUFPMC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserFlagPostSpamCount.ToString();
			objSetting.Value = dntbFPSC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserTermSynonymCreateMinAnswerCount.ToString();
			objSetting.Value = dntbUTSCMAC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserTermSynonymVoteMinAnswerScoreCount.ToString();
			objSetting.Value = dntbUTSVMASC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserUpVoteAnswerCount.ToString();
			objSetting.Value = dntbUUVAC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.UserUpVoteQuestionCount.ToString();
			objSetting.Value = dntbUUVQC.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.OpThresholds.QuestionHomeMinScore.ToString();
			objSetting.Value = dntbQHMS.Text;
			cntQa.UpdateQaPortalSetting(objSetting);


			objSetting.TypeId = (int)Constants.SettingTypes.Email;

			objSetting.Key = Constants.EmailSettings.FromAddress.ToString();
			objSetting.Value = txtbxFromEmail.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.EmailSettings.CommentTemplate.ToString();
			objSetting.Value = txtbxCommentEmailTemplate.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.EmailSettings.SingleQuestionTemplate.ToString();
			objSetting.Value = txtbxQuestionEmailTemplate.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.EmailSettings.AnswerTemplate.ToString();
			objSetting.Value = txtAnswerEmailTemplate.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			objSetting.Key = Constants.EmailSettings.SummaryTemplate.ToString();
			objSetting.Value = txtbxSummaryEmailTemplate.Text;
			cntQa.UpdateQaPortalSetting(objSetting);

			// handle cache clearing
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + ModuleContext.PortalId);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + ModuleContext.ModuleId);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeTermsCacheKey + ModuleContext.ModuleId);
		}

		#endregion

	}
}