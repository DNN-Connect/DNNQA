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

using System.Data;
using System;

namespace DotNetNuke.DNNQA.Providers.Data {

	/// <summary>
	/// An abstract class for the data access layer (Thus, this is our abstract data provider).
	/// </summary>
	public interface IDataProvider {

		#region Abstract methods

		//#region Badges

		//int AddBadge(string key, int portalId, int tierId, int triggerActionId, string triggerActions, string triggerSproc, string triggerSql, int triggerCount, int triggerTimeCount, string triggerTimeUnit);

		//IDataReader GetPortalBadges(int portalId);

		//IDataReader GetUserBadges(int portalId, int userId);

		//void UpdateBadge(int badgeId, int portalId, int tierId, int triggerActionId, string triggerActions, string triggerSproc, string triggerSql, int triggerCount, int triggerTimeCount, string triggerTimeUnit);

		//void DeleteBadge(int badgeId, int portalId);

		//#endregion
									   
		#region Comments

		int AddComment(int userId, int postId, string comment, DateTime createdOnDate);

		IDataReader GetPostComments(int postId);

		IDataReader GetCommentsByDate(DateTime startDate, DateTime endDate);

		#endregion

		#region Email

		#region Content Items

		IDataReader GetContentItemsByTypeAndCreated(int contentTypeId, DateTime startDate, DateTime endDate);

		#endregion

		#endregion
		
		#region Posts

		IDataReader GetHomeQuestions(int moduleId, int excludeCount, int minScore);

		#region User

		IDataReader GetUserQuestions(int portalId, int userId);

		IDataReader GetUserAnswers(int portalId, int userId);

		IDataReader SearchByUser(int portalId, int userId);

		IDataReader GetUsersLastPost(int portalId, int userId);

		#endregion

		IDataReader KeywordSearch(int moduleId, string keyword);

		IDataReader TermSearch(int moduleId, int pageSize, string term);

		IDataReader SearchQuestionTitles(int moduleId, string searchPhrase);

		IDataReader GetAnswers(int postId, int portalId);

		IDataReader GetAnswersByDate(DateTime lastRunDate, DateTime currentRunDate);

		void IncreaseViewCount(int postId, int portalId);

		IDataReader GetPost(int postId, int portalId);

		IDataReader GetQuestionByContentItem(int contentItemId);

		IDataReader GetSitemapQuestions(int portalId);

		int AddPost(string title, string body, int bounty, int parentId, int portalId, int contentItemId, bool approved, DateTime approvedDate, int createdUserID, DateTime createdDate);

// ReSharper disable InconsistentNaming
		void UpdatePost(int postId, string title, string body, int bounty, int parentID, int portalId, int contentItemId, bool approved, DateTime approvedDate, bool deleted, int answerId, DateTime answerDate, bool closed, DateTime closedDate, bool Protected, DateTime protectedDate, int lastModifiedUserID, DateTime lastModifiedDate);
// ReSharper restore InconsistentNaming

		void DeletePost(int postId, int portalId);

		void SoftDeletePost(int postId, int portalId);

		void AcceptAnswer(int postId, int userId, DateTime acceptedDate);

		#region Post History

		int AddPostHistory(int postId, string notes, bool approved);

		IDataReader GetPostHistory(int postId);

		#endregion

		#endregion

		#region Schedule Item Settings

		void UpdateScheduleItemSetting(int scheduleId, string key, string value);

		#endregion

		#region Settings

		IDataReader GetQaPortalSettings(int portalId);

		void UpdateQaPortalSetting(string key, string value, int typeId, int portalId);

		#endregion

		#region Subscribers

		IDataReader GetSubscribersByContentItem(int contentItemId, int subscriptionType, int portalId);

		IDataReader GetSubscribersByQuestion(int questionId, int subscriptionType, int portalId);

		#endregion

		#region Subscriptions

		int AddSubscription(int portalId, int userId, string emailAddress, int postId, int termId, int subscriptionType, DateTime createdOnDate);

		IDataReader GetUserSubscriptions(int portalId, int userId);

		IDataReader GetQuestionSubscriptions(int portalId, int postId);

		IDataReader GetTermSubscriptions(int portalId, int termId);

		void DeleteSubscription(int portalId, int subscriptionId);

		void DeleteUserPostSubscription(int userId, int postId);

		#endregion

		#region Terms

		IDataReader GetTermsByContentItem(int contentItemId, int vocabularyId);

		IDataReader GetTermsByContentType(int portalId, int contentTypeId, int moduleId, int vocabularyId);

		#region Term History

		int AddTermHistory(int portalId, int termId, string notes, bool approved);

		IDataReader GetTermHistory(int portalId, int termId);

		#endregion

		#region Term Synonym

		int AddTermSynonym(int masterTermId, int relatedTermId, int portalId, int createdByUserId, DateTime createdOnDate);

		IDataReader GetTermSynonyms(int portalId);

		void TermSynonymReplaced(int relatedTermId, int portalId);

		void DeleteTermSynonym(int masterTermId, int relatedTermId, int portalId);

		#endregion

		#endregion

		#region User Badges

		IDataReader GetBadgeRecipients(int badgeId);

		#endregion
		
		#region User Scoring

		void AddScoringLog(int userId, int portalId, int userScoringActionId, int score, int keyId, string notes, DateTime createdOnDate);

		IDataReader GetUserScore(int userId, int portalId);

		IDataReader GetUserScoresByPortal(int portalId);

		IDataReader GetUserScoreLog(int userId, int portalId);

		IDataReader GetUserScoreLogByKey(int keyId, int portalId);

		void DeleteUserScoreLog(int userId, int portalId, int userScoringActionId, int score, int keyId);

		void SetUserScoreMessage(int userId, int portalId, string message);

		void ClearUserScoreMessage(int userId, int portalId);

		#endregion
		
		#region Votes

		int AddVote(int postId, int termId, int voteTypeId, int portalId, int createdByUserId, DateTime createdOnDate);

		IDataReader GetPostVotes(int postId);

		IDataReader GetTermSynonymVotes(int termId, int portalId);

		#endregion

		#endregion

	}
}