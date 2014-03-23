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
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Entities.Content;

namespace DotNetNuke.DNNQA.Components.Controllers {

	/// <summary>
	/// 
	/// </summary>
	public interface IDnnqaController
	{

		//#region Badges

		//int AddBadge(BadgeInfo objBadge);

		//List<BadgeInfo> GetPortalBadges(int portalId);

		//List<BadgeInfo> GetUserBadges(int portalId, int userId);

		//void UpdateBadge(BadgeInfo objBadge);

		//void DeleteBadge(int badgeId, int portalId);

		//#endregion
		
		#region Comments

		int AddComment(CommentInfo objComment);

		List<CommentInfo> GetPostComments(int postId);

		List<CommentInfo> GetCommentsByDate(DateTime startDate, DateTime endDate);

		#endregion

		#region Email

		#region Content Items

		List<ContentItem> GetContentItemsByTypeAndCreated(int contentTypeId, DateTime startDate, DateTime endDate);

		#endregion

		#endregion

		#region Posts

		List<QuestionInfo> GetHomeQuestions(int moduleId, int pageSize, int excludeCount, int minScore);

		#region User

		List<QuestionInfo> GetUserQuestions(int portalId, int userId);

		List<QuestionInfo> GetUserAnswers(int portalId, int userId);

		List<QuestionInfo> SearchByUser(int portalId, int userId);

		List<QuestionInfo> KeywordSearch(int moduleId, string keyword);

		PostInfo GetUsersLastPost(int userId, int portalId);

		#endregion
		
		List<QuestionInfo> TermSearch(int moduleId, int pageSize, string term);

		List<QuestionServiceInfo> SearchQuestionTitles(int moduleId, string searchPhrase);

		QuestionInfo GetQuestion(int postId, int portalId);

		QuestionInfo GetQuestionByContentItem(int contentItemId);

		void IncreaseViewCount(int postId, int portalId);

		List<PostInfo> GetAnswers(int postId, int portalId);

		List<PostInfo> GetAnswersByDate(DateTime lastRunDate, DateTime currentRunDate);

		PostInfo GetPost(int postID, int portalId);

		PostInfo AddPost(PostInfo objPost, int tabId);

		void UpdatePost(PostInfo objPost, int tabId);

		void DeletePost(int postId, int parentId, int portalId, int contentItemId, bool softDelete, int moduleId);

		void AcceptAnswer(int postId, int userId, DateTime acceptedDate, int moduleId);

		#region History

		int AddPostHistory(int postId, string notes, bool approved);

		List<PostHistoryInfo> GetPostHistory(int postId);

		#endregion

		#endregion

		#region Schedule Item Settings

		void UpdateScheduleItemSetting(int scheduleId, string key, string value);

		#endregion

		#region Settings

		List<SettingInfo> GetQaPortalSettings(int portalId);

		void UpdateQaPortalSetting(SettingInfo objSetting);

		#endregion

		#region Subscribers

		List<SubscriberInfo> GetSubscribersByContentItem(int contentItemId, int subscriptionType, int portalId);

		List<SubscriberInfo> GetSubscribersByQuestion(int questionId, int subscriptionType, int portalId);

		#endregion

		#region Subscriptions

		int AddSubscription(SubscriptionInfo objSubscription);

		List<SubscriptionInfo> GetQuestionSubscribers(int portalId, int questionId);

		List<SubscriptionInfo> GetTermSubscribers(int portalId, int termId);

		List<SubscriptionInfo> GetUserSubscriptions(int portalId, int userId);

		void DeleteSubscription(int portalId, int subscriptionId);

		void DeleteUserPostSubscription(int userId, int postId);

		#endregion

		#region Terms

		List<TermInfo> GetTermsByContentItem(int contentItemId, int vocabularyId);

		List<TermInfo> GetTermsByContentType(int portalId, int moduleId, int vocabularyId);

		#region Term History

		int AddTermHistory(int portalId, int termId, string notes, bool approved, int moduleId);

		List<TermHistoryInfo> GetTermHistory(int portalId, int termId);

		#endregion

		#region Term Synonym

		TermSynonymInfo AddTermSynonym(TermSynonymInfo objSynonym);

		List<TermSynonymInfo> GetTermSynonyms(int portalId);

		void TermSynonymReplaced(int relatedTermId, int portalId);

		void DeleteTermSynonym(int masterTermId, int relatedTermId, int portalId);

		#endregion
		
		#endregion

		#region User Badges

		List<UserBadgeInfo> GetBadgeRecipients(int badgeId);

		#endregion
		
		#region User Scoring

		void AddScoringLog(UserScoreLogInfo objScoreLog, IEnumerable<QaSettingInfo> sitePrivileges);

		UserScoreInfo GetUserScore(int userId, int portalId);

		List<UserScoreLogInfo> GetUserScoreLog(int userId, int portalId);

		List<UserScoreInfo> GetUserScoresByPortal(int portalId);

		List<UserScoreLogInfo> GetUserScoreLogByKey(int keyId, int portalId);

		void DeleteUserScoreLog(int userId, int portalId, int userScoringActionId, int score, int keyId);

		void ClearUserScoreMessage(int userId, int portalId);

		#endregion
		
		#region Votes

		int AddVote(VoteInfo objVote, int moduleId);

		List<VoteInfo> GetPostVotes(int postId);

		List<VoteInfo> GetTermSynonymVotes(int termId, int portalId);
		
		#endregion

	}
}