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
using System.Data;
using DotNetNuke.Common.Utilities;
using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.DNNQA.Providers.Data.SqlDataProvider {

	/// <summary>
	/// SQL Server implementation (ie. concrete provider) of the abstract DataProvider class
	/// </summary>
	public class SqlDataProvider : IDataProvider {

		#region Private Members

		//private const string ProviderType = "data";
		private const string ModuleQualifier = "DNNQA_";
		private string _connectionString = String.Empty;
		private string _databaseOwner = String.Empty;
		private string _objectQualifier = String.Empty;

		#endregion

		#region Properties

		public string ConnectionString {
			get {
				return string.IsNullOrEmpty(_connectionString) ? DotNetNuke.Data.DataProvider.Instance().ConnectionString : _connectionString;
			}
			set { _connectionString = value; }
		}

		public string DatabaseOwner {
			get {
				return string.IsNullOrEmpty(_databaseOwner) ? DotNetNuke.Data.DataProvider.Instance().DatabaseOwner : _databaseOwner;
			}
			set { _databaseOwner = value; }
		}

		public string ObjectQualifier {
			get {
				return string.IsNullOrEmpty(_objectQualifier) ? DotNetNuke.Data.DataProvider.Instance().ObjectQualifier : _objectQualifier;
			}
			set { _objectQualifier = value; }
		}

		#endregion

		#region Private Methods

		private static object GetNull(object field) {
			return Null.GetNull(field, DBNull.Value);
		}

		private string GetFullyQualifiedName(string name) {
			return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
		}

		#endregion

		#region Public Methods

		//#region Badges

		//public int AddBadge(string key, int portalId, int tierId, int triggerActionId, string triggerActions, string triggerSproc, string triggerSql, int triggerCount, int triggerTimeCount, string triggerTimeUnit)
		//{
		//    return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Badge_Add"), key, portalId, tierId, GetNull(triggerActionId), GetNull(triggerActions), GetNull(triggerSproc), GetNull(triggerSql), triggerCount, triggerTimeCount, triggerTimeUnit));
		//}

		//public IDataReader GetPortalBadges(int portalId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Badge_GetPortal"), portalId);
		//}

		//public IDataReader GetUserBadges(int portalId, int userId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Badge_GetUser"), portalId, userId);
		//}

		//public void UpdateBadge(int badgeId, int portalId,  int tierId, int triggerActionId, string triggerActions, string triggerSproc, string triggerSql, int triggerCount, int triggerTimeCount, string triggerTimeUnit)
		//{
		//    SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Badge_Update"), badgeId, portalId, tierId, GetNull(triggerActionId), GetNull(triggerActions), GetNull(triggerSproc), GetNull(triggerSql), triggerCount, triggerTimeCount, triggerTimeUnit);
		//}

		//public void DeleteBadge(int badgeId, int portalId)
		//{
		//    SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Badge_Delete"), badgeId, portalId);
		//}

		//#endregion
		
		#region Comments

		public int AddComment(int userId, int postId, string comment, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Comment_Add"), userId, postId, comment, createdOnDate));
		}

		public IDataReader GetPostComments(int postId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Comment_GetPost"), postId);
		}

		public IDataReader GetCommentsByDate(DateTime startDate, DateTime endDate)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Comment_GetByDate"), startDate, endDate);
		}

		#endregion

		#region Email

		#region Content Items

		public IDataReader GetContentItemsByTypeAndCreated(int contentTypeId, DateTime startDate, DateTime endDate)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("ContentItem_GetByTypeAndCreated"), contentTypeId, startDate, endDate);
		}

		#endregion

		#endregion

		#region Posts

		public IDataReader GetHomeQuestions(int moduleId, int excludeCount, int minScore)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_GetHome"), moduleId, excludeCount, minScore);
		}

		#region User

		public IDataReader GetUserQuestions(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_GetUserAsked"), portalId, userId);
		}

		public IDataReader GetUserAnswers(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_GetUserAnswered"), portalId, userId);
		}

		public IDataReader SearchByUser(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_SearchByUser"), portalId, userId);
		}

		public IDataReader GetUsersLastPost(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_GetUsersLast"), portalId, userId);
		}

		#endregion

		public IDataReader KeywordSearch(int moduleId, string keyword)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_KeywordSearch"), moduleId, GetNull(keyword));
		}

		public IDataReader TermSearch(int moduleId, int pageSize, string term) {
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_TermSearch"), moduleId, pageSize, term);
		}

		public IDataReader SearchQuestionTitles(int moduleId, string searchPhrase)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_SearchTitle"), moduleId, searchPhrase);
		}

		public IDataReader GetAnswers(int postId, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_GetAnswers"), postId, portalId);
		}

		public IDataReader GetAnswersByDate(DateTime lastRunDate, DateTime currentRunDate)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_GetAnswersByDate"), lastRunDate, currentRunDate);
		}

		public void IncreaseViewCount(int postId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_UpdateViewCount"), postId, portalId);
		}

		public IDataReader GetPost(int postId, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_Get"), postId, GetNull(portalId));
		}

		public IDataReader GetQuestionByContentItem(int contentItemId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_GetByContentItem"), contentItemId);
		}

		public IDataReader GetSitemapQuestions(int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Question_GetSitemap"), portalId);
		}

		public int AddPost(string title, string body, int bounty, int parentId, int portalId, int contentItemId, bool approved, DateTime approvedDate, int createdUserId, DateTime createdDate) {
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Post_Add"), title, body, bounty, parentId, portalId, contentItemId, approved, GetNull(approvedDate), createdUserId, createdDate));
		}

// ReSharper disable InconsistentNaming
		public void UpdatePost(int postId, string title, string body, int bounty, int parentId, int portalId, int contentItemId, bool approved, DateTime approvedDate, bool deleted, int answerID, DateTime answerDate, bool closed, DateTime closedDate, bool Protected, DateTime protectedDate, int lastModifiedUserID, DateTime lastModifiedDate)
// ReSharper restore InconsistentNaming
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_Update"), postId, title, body, bounty, parentId, portalId, contentItemId, approved, GetNull(approvedDate), deleted, GetNull(answerID), GetNull(answerDate), closed, GetNull(closedDate), Protected, GetNull(protectedDate), GetNull(lastModifiedUserID), GetNull(lastModifiedDate));
		}

		public void DeletePost(int postId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_Delete"), postId, portalId);
		}

		public void SoftDeletePost(int postId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_SoftDelete"), postId, portalId);
		}

		public void AcceptAnswer(int postId, int userId, DateTime acceptedDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_AcceptAnswer"), postId, userId, acceptedDate);
		}

		#region Post History

		public int AddPostHistory(int postId, string notes, bool approved)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Post_History_Add"), postId, notes, approved));
		}

		public IDataReader GetPostHistory(int postId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_History_Get"), postId);
		}

		#endregion

		#endregion

		#region Schedule Item Settings

		public void UpdateScheduleItemSetting(int scheduleId, string key, string value)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("UpdateScheduleItemSetting"), scheduleId, key, value);
		}

		#endregion
 
		#region Settings

		public IDataReader GetQaPortalSettings(int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Setting_GetPortal"), portalId);
		}

		public void UpdateQaPortalSetting(string key, string value, int typeId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Setting_Update"), key, value, typeId, portalId);
		}

		#endregion

		#region Subscribers

		public IDataReader GetSubscribersByContentItem(int contentItemId, int subscriptionType, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscribers_GetByContentItem"), contentItemId, subscriptionType, portalId);
		}

		public IDataReader GetSubscribersByQuestion(int questionId, int subscriptionType, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscribers_GetByQuestion"), questionId, subscriptionType, portalId);
		}

		#endregion

		#region Subscriptions

		public int AddSubscription(int portalId, int userId, string emailAddress, int postId, int termId, int subscriptionType, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Subscription_Add"), portalId, userId, GetNull(emailAddress), GetNull(postId), GetNull(termId), subscriptionType, createdOnDate));
		}

		public IDataReader GetUserSubscriptions(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscription_GetUser"), portalId, userId);
		}

		public IDataReader GetQuestionSubscriptions(int portalId, int postId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscription_GetPost"), portalId, postId);
		}

		public IDataReader GetTermSubscriptions(int portalId, int termId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscription_GetTerm"), portalId, termId);
		}

		public void DeleteSubscription(int portalId, int subscriptionId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Subscription_Delete"), portalId, subscriptionId);
		}

		public void DeleteUserPostSubscription(int userId, int postId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Subscription_DeleteUserPost"), userId, postId);
		}

		#endregion

		#region Terms

		public IDataReader GetTermsByContentItem(int contentItemId, int vocabularyId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Term_GetByContentItem"), contentItemId, vocabularyId);
		}

		public IDataReader GetTermsByContentType(int portalId, int contentTypeId, int moduleId, int vocabularyId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Term_GetByContentType"), portalId, contentTypeId, moduleId, vocabularyId);
		}

		#region Term History

		public int AddTermHistory(int portalId, int termId, string notes, bool approved)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Term_History_Add"), portalId, termId, notes, approved));
		}

		public IDataReader GetTermHistory(int portalId, int termId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Term_History_Get"), portalId, termId);
		}

		#endregion
		
		#region Term Synonym

		public IDataReader GetTermSynonyms(int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Term_Synonym_Get"), portalId);
		}

		public int AddTermSynonym(int masterTermId, int relatedTermId, int portalId, int createdByUserId, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Term_Synonym_Add"), masterTermId, relatedTermId, portalId, createdByUserId, createdOnDate));
		}

		public void TermSynonymReplaced(int relatedTermId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Term_Synonym_Replaced"), relatedTermId, portalId);
		}

		public void DeleteTermSynonym(int masterTermId, int relatedTermId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Term_Synonym_Delete"), masterTermId, relatedTermId, portalId);
		}

		#endregion

		#endregion

		#region User Badges

		public IDataReader GetBadgeRecipients(int badgeId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("User_Badge_GetRecipients"), badgeId);
		}

		#endregion

		#region User Scoring

		public void AddScoringLog(int userId, int portalId, int userScoringActionId, int score, int keyId, string notes, DateTime createdOnDate)
		{
			SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("User_Score_Log_Add"), userId, portalId, userScoringActionId, score, GetNull(keyId), GetNull(notes), createdOnDate);
		}

		public IDataReader GetUserScore(int userId, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("User_Score_Get"), userId, portalId);
		}

		public IDataReader GetUserScoresByPortal(int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("User_Score_GetByPortal"), portalId);
		}

		public IDataReader GetUserScoreLog(int userId, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("User_Score_Log_Get"), userId, portalId);
		}

		public IDataReader GetUserScoreLogByKey(int keyId, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("User_Score_Log_GetByKey"), keyId, portalId);
		}
		
		public void DeleteUserScoreLog(int userId, int portalId, int userScoringActionId, int score, int keyId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("User_Score_Log_Delete"), userId, portalId, userScoringActionId, score, keyId);
		}

		public void SetUserScoreMessage(int userId, int portalId, string message)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("User_Score_SetMessage"), userId, portalId, message);
		}

		public void ClearUserScoreMessage(int userId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("User_Score_ClearMessage"), userId, portalId);
		}

		#endregion
		
		#region Votes

		public int AddVote(int postId, int termId, int voteTypeId, int portalId, int createdByUserId, DateTime createdOnDate) {
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Vote_Add"), postId, termId, voteTypeId, portalId, createdByUserId, createdOnDate));
		}

		public IDataReader GetPostVotes(int postId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Vote_GetPost"), postId);
		}

		public IDataReader GetTermSynonymVotes(int termId, int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Vote_GetTerm"), termId, portalId);
		}

		#endregion

		#endregion

	}
}