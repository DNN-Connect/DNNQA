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

using System.Linq;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Services.Journal;

namespace DotNetNuke.DNNQA.Components.Integration
{
    public class Journal
    {

        #region Internal Methods

        /// <summary>
        /// Informs the core journal that the user has voted for a post (positive or negative).
        /// </summary>
        /// <param name="objPost"></param>
        /// <param name="title"></param>
        /// <param name="voteId"></param>
        /// <param name="summary"></param>
        /// <param name="portalId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        /// <remarks>Do not send flagged posts to this method (they are technically 'votes'), keep those out of the journal.</remarks>
        internal void AddVoteToJournal(PostInfo objPost, int voteId, string title, string summary, int portalId, int journalUserId, string url)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalVoteTypeName + "_" + string.Format("{0}:{1}", objPost.ModuleID, voteId);
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
                     {
                         PortalId = portalId,
                         ProfileId = journalUserId,
                         UserId = journalUserId,
                         ContentItemId = objPost.ContentItemId,
                         Title = title,
                         ItemData = new ItemData {Url = url},
                         Summary = summary,
                         Body = null,
                         JournalTypeId = GetVoteJournalTypeID(portalId),
                         ObjectKey = objectKey,
                         SecuritySet = "E,"
                     };

            JournalController.Instance.SaveJournalItem(ji, objPost.TabID);
        }

        /// <summary>
        /// Informs the core journal that we have to delete an item (vote). 
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="moduleId"></param>
        /// <param name="portalId"></param>
        internal void RemoveVoteFromJournal(int voteId, int moduleId, int portalId)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalVoteTypeName + "_" + string.Format("{0}:{1}", moduleId, voteId);
            JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
        }

        /// <summary>
        /// Informs the core journal that the user has provided an answer.
        /// </summary>
        /// <param name="objPost"></param>
        /// <param name="questionTitle"></param>
        /// <param name="portalId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        internal void AddAnswerToJournal(PostInfo objPost, string questionTitle, int portalId, int journalUserId, string url)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalAnswerTypeName + "_" + string.Format("{0}:{1}", objPost.ParentId, objPost.PostId);
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
            {
                PortalId = portalId,
                ProfileId = journalUserId,
                UserId = journalUserId,
                ContentItemId = objPost.ContentItemId,
                Title = questionTitle,
                ItemData = new ItemData { Url = url },
                Summary = "", //objPost.Body,
                Body = null,
                JournalTypeId = GetAnswerJournalTypeID(portalId),
                ObjectKey = objectKey,
                SecuritySet = "E,"
            };

            JournalController.Instance.SaveJournalItem(ji, objPost.TabID);
        }

        /// <summary>
        /// Informs the core journal that the user has asked a question.
        /// </summary>
        /// <param name="objPost"></param>
        /// <param name="questionTitle"></param>
        /// <param name="portalId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        internal void AddQuestionToJournal(PostInfo objPost, string questionTitle, int portalId, int journalUserId, string url)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalQuestionTypeName + "_" + string.Format("{0}:{1}", objPost.ModuleID, objPost.PostId);
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
            {
                PortalId = portalId,
                ProfileId = journalUserId,
                UserId = journalUserId,
                ContentItemId = objPost.ContentItemId,
                Title = questionTitle,
                ItemData = new ItemData { Url = url },
                Summary = "", // objPost.Body,
                Body = null,
                JournalTypeId = GetQuestionJournalTypeID(portalId),
                ObjectKey = objectKey,
                SecuritySet = "E,"
            };

            JournalController.Instance.SaveJournalItem(ji, objPost.TabID);
        }

        /// <summary>
        /// Informs the core journal that the user has earned a new privilege.
        /// </summary>
        /// <param name="objPrivilege"></param>
        /// <param name="title"></param>
        /// <param name="summary"></param>
        /// <param name="portalId"></param>
        /// <param name="tabId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        internal void AddPrivilegeToJournal(QaSettingInfo objPrivilege, string title, string summary, int portalId, int tabId, int journalUserId, string url)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalPrivilegeTypeName + "_" + objPrivilege.Key;
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
            {
                PortalId = portalId,
                ProfileId = journalUserId,
                UserId = journalUserId,
                Title = title,
                ItemData = new ItemData { Url = url },
                Summary = "", // summary,
                Body = null,
                JournalTypeId = GetPrivilegeJournalTypeID(portalId),
                ObjectKey = objectKey,
                SecuritySet = "E,"
            };

            JournalController.Instance.SaveJournalItem(ji, tabId);
        }

        /// <summary>
        /// Informs the core journal that the user has commented on a post.
        /// </summary>
        /// <param name="objEntry"></param>
        /// <param name="objComment"></param>
        /// <param name="title"></param>
        /// <param name="portalId"></param>
        /// <param name="journalUserId"></param>
        /// <param name="url"></param>
        internal void AddCommentToJournal(PostInfo objEntry, Entities.CommentInfo objComment, string title, int portalId, int journalUserId, string url)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalCommentTypeName + "_" + string.Format("{0}:{1}", objEntry.PostId, objComment.CommentId);
            var ji = JournalController.Instance.GetJournalItemByKey(portalId, objectKey);

            if ((ji != null))
            {
                JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
            }

            ji = new JournalItem
            {
                PortalId = portalId,
                ProfileId = journalUserId,
                UserId = journalUserId,
                ContentItemId = objEntry.ContentItemId,
                Title = title,
                ItemData = new ItemData { Url = url },
                Summary = "", // objComment.Comment,
                Body = null,
                JournalTypeId = GetCommentJournalTypeID(portalId),
                ObjectKey = objectKey,
                SecuritySet = "E,"
            };

            JournalController.Instance.SaveJournalItem(ji, objEntry.TabID);
        }

        /// <summary>
        /// Deletes a journal item associated with the specific comment.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="commentId"></param>
        /// <param name="portalId"></param>
        internal void RemoveCommentFromJournal(int postId, int commentId, int portalId)
        {
            var objectKey = Constants.ContentTypeName + "_" + Constants.JournalCommentTypeName + "_" + string.Format("{0}:{1}", postId, commentId);
            JournalController.Instance.DeleteJournalItemByKey(portalId, objectKey);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a journal type associated with voting (using one of the core built in journal types).
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetVoteJournalTypeID(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalVoteTypeName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 17;
            }

            return journalTypeId;
        }

        /// <summary>
        /// Returns a journal type associated with commenting (using one of the core built in journal types)
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetCommentJournalTypeID(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalCommentTypeName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 18;
            }

            return journalTypeId;
        }

        /// <summary>
        /// Returns a journal type associated with providing an answer (using one of the core built in journal types).
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetAnswerJournalTypeID(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalAnswerTypeName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 20;
            }

            return journalTypeId;
        }

        /// <summary>
        /// Returns a journal type associated with asking a question (using one of the core built in journal types).
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetQuestionJournalTypeID(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalQuestionTypeName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 19;
            }

            return journalTypeId;
        }

        /// <summary>
        /// Returns a journal type associated with a privilege (using one of the core built in journal types).
        /// </summary>
        /// <param name="portalId"></param>
        /// <returns></returns>
        private static int GetPrivilegeJournalTypeID(int portalId)
        {
            var colJournalTypes = (from t in JournalController.Instance.GetJournalTypes(portalId) where t.JournalType == Constants.JournalPrivilegeTypeName select t);
            int journalTypeId;

            if (colJournalTypes.Count() > 0)
            {
                var journalType = colJournalTypes.Single();
                journalTypeId = journalType.JournalTypeId;
            }
            else
            {
                journalTypeId = 30;
            }

            return journalTypeId;
        }

        #endregion

    }
}