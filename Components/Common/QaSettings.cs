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
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;

namespace DotNetNuke.DNNQA.Components.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class QaSettings
    {

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colSettings"></param>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public static List<QaSettingInfo> GetOpThresholdCollection(List<SettingInfo> colSettings, int portalId)
        {
            // determine if operational threshold settings exist for this portal
            var colOpThreshSettings = (from t in colSettings where t.TypeId == (int)Constants.SettingTypes.OperationalThresholds select t);
            var colOpThresholds = new List<QaSettingInfo>();

            switch (colOpThreshSettings.Count())
            {
                case 0:
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.PostChangeVoteWindowMinutes.ToString(), Constants.DefaultOpPostChangeVoteWindowMinutes));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.PostFlagCompleteCount.ToString(), Constants.DefaultOpPostFlagCompleteCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.PostFlagWindowHours.ToString(), Constants.DefaultOpPostFlagWindowHours));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.QuestionCloseCompleteVoteCount.ToString(), Constants.DefaultOpQuestionCloseCompleteVoteCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.QuestionCloseWindowDays.ToString(), Constants.DefaultOpQuestionCloseWindowDays));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.QuestionFlagHomeRemoveCount.ToString(), Constants.DefaultOpQuestionFlagHomeRemoveCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.TagCloseWindowDays.ToString(), Constants.DefaultOpTagCloseWindowDays));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.TagFlagCompleteCount.ToString(), Constants.DefaultOpTagFlagCompleteCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.TagFlagWindowHours.ToString(), Constants.DefaultOpTagFlagWindowHours));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.TermSynonymApproveCount.ToString(), Constants.DefaultOpTermSynonymApproveCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.TermSynonymRejectCount.ToString(), Constants.DefaultOpTermSynonymRejectCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.TermSynonymMaxCount.ToString(), Constants.DefaultOpTermSynonymMaxCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserCloseVoteCount.ToString(), Constants.DefaultOpUserCloseVoteCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserFlagPostModerateCount.ToString(), Constants.DefaultOpUserFlagPostModerateCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserFlagPostSpamCount.ToString(), Constants.DefaultOpUserFlagPostSpamCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserTermSynonymCreateMinAnswerCount.ToString(), Constants.DefaultOpUserTermSynonymCreateMinAnswerCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserTermSynonymVoteMinAnswerScoreCount.ToString(), Constants.DefaultOpUserTermSynonymVoteMinAnswerScoreCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserUpVoteAnswerCount.ToString(), Constants.DefaultOpUserUpVoteAnswerCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.UserUpVoteQuestionCount.ToString(), Constants.DefaultOpUserUpVoteQuestionCount));
                    colOpThresholds.Add(BuildOpThreshold(Constants.OpThresholds.QuestionHomeMinScore.ToString(), Constants.DefaultOpHomeQuestionMinScore));
                    break;
                case 19:
                    var objNewSetting = new SettingInfo
                    {
                        PortalId = portalId,
                        TypeId = (int)Constants.SettingTypes.OperationalThresholds,
                        Key = Constants.OpThresholds.QuestionHomeMinScore.ToString(),
                        Value = Constants.DefaultOpHomeQuestionMinScore.ToString()
                    };

                    var cntQa = new DnnqaController();
                    cntQa.UpdateQaPortalSetting(objNewSetting);

                    DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + portalId);

                    colOpThreshSettings = (from t in colSettings where t.TypeId == (int)Constants.SettingTypes.OperationalThresholds select t);
                    colOpThresholds.AddRange(colOpThreshSettings.Select(objSetting => BuildOpThreshold(objSetting.Key, Convert.ToInt32(objSetting.Value))));
                    break;
                default :
                    colOpThresholds.AddRange(colOpThreshSettings.Select(objSetting => BuildOpThreshold(objSetting.Key, Convert.ToInt32(objSetting.Value))));
                    break;
            }

            return colOpThresholds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colSettings"></param>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public static List<QaSettingInfo> GetPrivilegeCollection(List<SettingInfo> colSettings, int portalId)
        {
            // determine if privilege settings exist for this portal
            var colPrivSettings = (from t in colSettings where t.TypeId == (int)Constants.SettingTypes.PrivilegeLevelScore select t);
            var colPrivileges = new List<QaSettingInfo>();

            if (colPrivSettings.Count() > 0)
            {
                colPrivileges.AddRange(colPrivSettings.Select(objSetting => BuildPrivilege(objSetting.Key, Convert.ToInt32(objSetting.Value))));
            }
            else
            {
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.ViewAll.ToString(), 0));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.CreatePost.ToString(), Constants.DefaultPrivCreatePost));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.RemoveNewUser.ToString(), Constants.DefaultPrivRemoveNewUser));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.Flag.ToString(), Constants.DefaultPrivFlag));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.VoteUp.ToString(), Constants.DefaultPrivVoteUp));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.CommentEverywhere.ToString(), Constants.DefaultPrivCommentEverywhere));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.VoteDown.ToString(), Constants.DefaultPrivVoteDown));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.RetagQuestion.ToString(), Constants.DefaultPrivRetagQuestion));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.EditQuestionsAndAnswers.ToString(), Constants.DefaultPrivEditQuestionsAndAnswers));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.CreateTagSynonym.ToString(), Constants.DefaultPrivCreateTagSynonym));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.CloseQuestion.ToString(), Constants.DefaultPrivCloseQuestion));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.ApproveTagEdits.ToString(), Constants.DefaultPrivApproveTagEdits));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.ModeratorTools.ToString(), Constants.DefaultPrivModeratorTools));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.ProtectQuestions.ToString(), Constants.DefaultPrivProtectQuestions));
                colPrivileges.Add(BuildPrivilege(Constants.Privileges.Trusted.ToString(), Constants.DefaultPrivTrusted));
            }

            return colPrivileges;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colSettings"></param>
        /// <param name="portalId"></param>
        /// <returns></returns>
        public static List<QaSettingInfo> GetUserScoringCollection(List<SettingInfo> colSettings, int portalId)
        {
            // determine if user scoring action settings exist for this portal
            var colUserScoringSettings = (from t in colSettings where t.TypeId == (int)Constants.SettingTypes.UserScoringActionValue select t);
            var x = colUserScoringSettings.Count();

            var colScoringActions = new List<QaSettingInfo>();

            switch (colUserScoringSettings.Count())
            {
                case 0:
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.AdminEntered.ToString(), Constants.DefaultScoreAdminEntered)); // static 0
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ApprovedPostEdit.ToString(), Constants.DefaultScoreApprovedPostEdit));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ApprovedTagEdit.ToString(), Constants.DefaultScoreApprovedTagEdit));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.AskedFlaggedQuestion.ToString(), Constants.DefaultScoreAskedFlaggedQuestion));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.AskedQuestion.ToString(), Constants.DefaultScoreAskedQuestion));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.AskedQuestionVotedDown.ToString(), Constants.DefaultScoreAskedQuestionVotedDown));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.AskedQuestionVotedUp.ToString(), Constants.DefaultScoreAskedQuestionVotedUp));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.BountyPaid.ToString(), Constants.DefaultScoreBountyPaid)); // static 0
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.BountyReceived.ToString(), Constants.DefaultScoreBountyReceived)); // static 0
                    //createtag
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.CreatedTagSynonym.ToString(), Constants.DefaultScoreCreatedTagSynonym));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.Commented.ToString(), Constants.DefaultScoreCommented));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.EditedPost.ToString(), Constants.DefaultScoreEditedPost));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.EditedTag.ToString(), Constants.DefaultScoreEditedTag));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.EditedTagVotedDown.ToString(), Constants.DefaultScoreEditedTagVotedDown));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.EditedTagVotedUp.ToString(), Constants.DefaultScoreEditedTagVotedUp));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.FirstLoggedInView.ToString(), Constants.DefaultScoreFirstLoggedInView));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ProvidedAcceptedAnswer.ToString(), Constants.DefaultScoreProvidedAcceptedAnswer));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ProvidedAnswer.ToString(), Constants.DefaultScoreProvidedAnswer));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ProvidedAnswerVotedDown.ToString(), Constants.DefaultScoreProvidedAnswerVotedDown));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ProvidedAnswerVotedUp.ToString(), Constants.DefaultScoreProvidedAnswerVotedUp));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.ProvidedFlaggedAnswer.ToString(), Constants.DefaultScoreProvidedFlaggedAnswer));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedDownAnswer.ToString(), Constants.DefaultScoreVotedDownAnswer));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedDownQuestion.ToString(), Constants.DefaultScoreVotedDownQuestion));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedSynonymDown.ToString(), Constants.DefaultScoreVotedSynonymDown));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedSynonymUp.ToString(), Constants.DefaultScoreVotedSynonymUp));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedTagDown.ToString(), Constants.DefaultScoreVotedTagDown));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedTagUp.ToString(), Constants.DefaultScoreVotedTagUp));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedUpAnswer.ToString(), Constants.DefaultScoreVotedUpAnswer));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.VotedUpQuestion.ToString(), Constants.DefaultScoreVotedUpQuestion));
                    colScoringActions.Add(BuildUserScore(Constants.UserScoringActions.AcceptedQuestionAnswer.ToString(), Constants.DefaultScoreAcceptedAnswer)); // new
                    break;
                case 29: 
                    var objNewSetting = new SettingInfo
                    {
                        PortalId = portalId,
                        TypeId = (int)Constants.SettingTypes.UserScoringActionValue,
                        Key = Constants.UserScoringActions.AcceptedQuestionAnswer.ToString(),
                        Value = Constants.DefaultScoreAcceptedAnswer.ToString()
                    };

                    var cntQa = new DnnqaController();
                    cntQa.UpdateQaPortalSetting(objNewSetting);

                    DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + portalId);

                    colUserScoringSettings = (from t in colSettings where t.TypeId == (int)Constants.SettingTypes.UserScoringActionValue select t);
                    colScoringActions.AddRange(colUserScoringSettings.Select(objSetting => BuildUserScore(objSetting.Key, Convert.ToInt32(objSetting.Value))));
                    break;
                default:
                    colScoringActions.AddRange(colUserScoringSettings.Select(objSetting => BuildUserScore(objSetting.Key, Convert.ToInt32(objSetting.Value))));
                    break;
            }

            return colScoringActions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colSettings"></param>
        /// <param name="portalId"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<EmailSettingInfo> GetEmailCollection(List<SettingInfo> colSettings, int portalId)
        {
            // determine if user scoring action settings exist for this portal
            var colEmailSettings = (from t in colSettings where t.TypeId == (int)Constants.SettingTypes.Email select t);
            var colEmail = new List<EmailSettingInfo>();

            if (colEmailSettings.Count() > 0)
            {
                colEmail.AddRange(colEmailSettings.Select(objSetting => BuildEmail(objSetting.Key, objSetting.Value)));
            }
            else
            {
                colEmail.Add(BuildEmail(Constants.EmailSettings.FromAddress.ToString(), @"email@change.me"));
                colEmail.Add(BuildEmail(Constants.EmailSettings.CommentTemplate.ToString(), @"<html><body><h3><a href='[TITLELINK]'>[TITLE]</a></h3><div style='padding:0 15px'>[COMMENT]<p>by [AUTHOR]</p></div><div><p>If you no longer wish to receive emails, go here to <a href='[SUBSCRIBELINK]'>unsubscribe</a>.</p></div></body></html>"));
                colEmail.Add(BuildEmail(Constants.EmailSettings.SingleQuestionTemplate.ToString(), @"<html><body><h3><a href='[TITLELINK]'>[TITLE]</a></h3><div style='padding:0 15px'>[BODY]<p>by [AUTHOR]</p><p>Tagged: [TERMS]</p></div><div><p>If you no longer wish to receive emails, go here to <a href='[SUBSCRIBELINK]'>unsubscribe</a>.</p></div></body></html>"));
                colEmail.Add(BuildEmail(Constants.EmailSettings.AnswerTemplate.ToString(), @"<html><body><h3><a href='[TITLELINK]'>[TITLE]</a></h3><div style='padding:0 15px'>[BODY]<p>by [AUTHOR]</p></div><div><p>If you no longer wish to receive emails, go here to <a href='[SUBSCRIBELINK]'>unsubscribe</a>.</p></div></body></html>"));
                colEmail.Add(BuildEmail(Constants.EmailSettings.SummaryTemplate.ToString(), @"<html><body><h3><a href='[TITLELINK]'>[TITLE]</a></h3><div style='padding:0 15px'>[BODY]<p>by [AUTHOR]</p><p>Tagged: [TERMS]</p></div><div><p>If you no longer wish to receive emails, go here to <a href='[SUBSCRIBELINK]'>unsubscribe</a>.</p></div></body></html>"));
            }

            return colEmail;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static QaSettingInfo BuildPrivilege(string key, int value)
        {
            var objPrivilege = new QaSettingInfo
            {
                Key = key,
                Name = @"Priv_" + key,
                Description = @"Priv_" + key + @"_Desc",
                Value = value
            };

            return objPrivilege;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static QaSettingInfo BuildUserScore(string key, int value)
        {
            var objUserScore = new QaSettingInfo
            {
                Key = key,
                Name = @"Score_" + key,
                Description = @"Score_" + key + @"_Desc",
                Value = value
            };

            return objUserScore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static QaSettingInfo BuildOpThreshold(string key, int value)
        {
            var objThreshold = new QaSettingInfo
            {
                Key = key,
                Name = @"OpThresh_" + key,
                Description = @"OpThresh_" + key + @"_Desc",
                Value = value
            };

            return objThreshold;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static EmailSettingInfo BuildEmail(string key, string value)
        {
            var objEmail = new EmailSettingInfo
            {
                Key = key,
                Name = @"Email_" + key,
                Description = @"Email_" + key + @"_Desc",
                Value = value
            };

            return objEmail;
        }

        #endregion

    }
}