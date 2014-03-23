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
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Host;

namespace DotNetNuke.DNNQA.Components.Controllers {

	public class DnnqaController : IDnnqaController {

		private readonly IDataProvider _dataProvider;

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public DnnqaController() {
			_dataProvider = ComponentModel.ComponentFactory.GetComponent<IDataProvider>();
			if (_dataProvider != null) return;

			// get the provider configuration based on the type
			var defaultprovider = Data.DataProvider.Instance().DefaultProviderName;
			const string dataProviderNamespace = "DotNetNuke.DNNQA.Providers.Data";

			if (defaultprovider == "SqlDataProvider") {
				_dataProvider = new SqlDataProvider();
			} else {
				var providerType = dataProviderNamespace + "." + defaultprovider;
				_dataProvider = (IDataProvider)Framework.Reflection.CreateObject(providerType, providerType, true);
			}

			ComponentModel.ComponentFactory.RegisterComponentInstance<IDataProvider>(_dataProvider);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataProvider"></param>
		public DnnqaController(IDataProvider dataProvider) {
			//DotNetNuke.Common.Requires.NotNull("dataProvider", dataProvider);
			_dataProvider = dataProvider;
		}

		#endregion

		#region Public Methods

		//#region Badges

		//public int AddBadge(BadgeInfo objBadge)
		//{
		//    return _dataProvider.AddBadge(objBadge.Key, objBadge.PortalId, objBadge.TierId, objBadge.TriggerActionId, objBadge.TriggerActions, objBadge.TriggerSproc, objBadge.TriggerSql, objBadge.TriggerCount, objBadge.TriggerTimeCount, objBadge.TriggerTimeUnit);
		//}

		//public List<BadgeInfo> GetPortalBadges(int portalId)
		//{
		//    //TODO: Cache ME
		//    return CBO.FillCollection<BadgeInfo>(_dataProvider.GetPortalBadges(portalId));
		//}

		//public List<BadgeInfo> GetUserBadges(int portalId, int userId)
		//{
		//    return CBO.FillCollection<BadgeInfo>(_dataProvider.GetUserBadges(portalId, userId));
		//}

		//public void UpdateBadge(BadgeInfo objBadge)
		//{
		//    _dataProvider.UpdateBadge(objBadge.BadgeId, objBadge.PortalId, objBadge.TierId, objBadge.TriggerActionId, objBadge.TriggerActions, objBadge.TriggerSproc, objBadge.TriggerSql, objBadge.TriggerCount, objBadge.TriggerTimeCount, objBadge.TriggerTimeUnit);
		//}

		//public void DeleteBadge(int badgeId, int portalId)
		//{
		//    _dataProvider.DeleteBadge(badgeId, portalId);
		//}

		//#endregion
		
		#region Comments

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objComment"></param>
		/// <returns></returns>
		public int AddComment(CommentInfo objComment)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("UserId", "", UserId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("PostId", "", PostId);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("createdOnDate", "", objComment.CreatedOnDate.ToString());

			return _dataProvider.AddComment(objComment.UserId, objComment.PostId, objComment.Comment, objComment.CreatedOnDate);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public List<CommentInfo> GetPostComments(int postId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", postId);

			// TODO: Implement caching here
			return CBO.FillCollection<CommentInfo>(_dataProvider.GetPostComments(postId));
		}

		public List<CommentInfo> GetCommentsByDate(DateTime startDate, DateTime endDate)
		{
			//DotNetNuke.Common.Requires.PropertyNotNull("startDate", "", startDate);
			//DotNetNuke.Common.Requires.PropertyNotNull("endDate", "", endDate);

			return CBO.FillCollection<CommentInfo>(_dataProvider.GetCommentsByDate(startDate, endDate));
		}

		#endregion

		#region Email

		#region Content Items

		/// <summary>
		/// Returns a list of content items for a specific portal created between 2 dates using a specific content type. 
		/// </summary>
		/// <param name="contentTypeId"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <returns></returns>
		public List<ContentItem> GetContentItemsByTypeAndCreated(int contentTypeId, DateTime startDate, DateTime endDate)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalID", "", objPost.PortalID);
			return CBO.FillCollection<ContentItem>(_dataProvider.GetContentItemsByTypeAndCreated(contentTypeId, startDate, endDate));
		}

		#endregion

		#endregion
		
		#region Posts

		/// <summary>
		/// Returns a collection of the latest x active questions. This is a special view, shown only in the home control which has several restrictions: must not be flagged (the question) = or beyond the exclude count, must be within last 200 questions. These restrictions ensure fresh and relevant content in the initial view. 
		/// </summary>
		/// <param name="moduleId">The module associated with the questions.</param>
		/// <param name="pageSize">The number of records to return.</param>
		/// <param name="excludeCount"></param>
		/// <param name="minScore"></param>
		/// <returns>A colleciton of the latest active questions. Activity, in this case, is based on the question or the last answer provided (and has nothing to do with voting).</returns>
		/// <remarks>This method needs to cache at the module level and get updated every time a post occurs (question or reply). This is not user specific and we don't have to include pagesize/exclude count as part of cache key (as long as we resset this on settings update).</remarks>
		public List<QuestionInfo> GetHomeQuestions(int moduleId, int pageSize, int excludeCount, int minScore) {
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("pageSize", "", pageSize);
			//DotNetNuke.Common.Requires.PropertyNotNegative("excludeCount", "", excludeCount);

			var colHomeQuestions = (List<QuestionInfo>)DataCache.GetCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + moduleId);

			if (colHomeQuestions == null)
			{
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

				colHomeQuestions = CBO.FillCollection<QuestionInfo>(_dataProvider.GetHomeQuestions(moduleId, excludeCount, minScore));
				// handle sorting before storing in cache.
				var objSort = new SortInfo { Column = "active", Direction = Constants.SortDirection.Descending };
				colHomeQuestions = Sorting.GetKeywordSearchCollection(pageSize, 0, objSort, colHomeQuestions).ToList();

// ReSharper disable RedundantLogicalConditionalExpressionOperand
				if ((timeOut > 0) && Constants.EnableCaching && (colHomeQuestions.Count > 0))
// ReSharper restore RedundantLogicalConditionalExpressionOperand
				{
					DataCache.SetCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + moduleId, colHomeQuestions, TimeSpan.FromMinutes(timeOut));
				}
			}
			return colHomeQuestions;
		}

		#region User

		public List<QuestionInfo> GetUserQuestions(int portalId, int userId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);

			// TODO: Implement caching here
			return CBO.FillCollection<QuestionInfo>(_dataProvider.GetUserQuestions(portalId, userId));
		}

		public List<QuestionInfo> GetUserAnswers(int portalId, int userId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);

			// TODO: Implement caching here
			return CBO.FillCollection<QuestionInfo>(_dataProvider.GetUserAnswers(portalId, userId));
		}

		public List<QuestionInfo> SearchByUser(int portalId, int userId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);

			// TODO: Implement caching here
			return CBO.FillCollection<QuestionInfo>(_dataProvider.SearchByUser(portalId, userId));
		}

		public PostInfo GetUsersLastPost(int userId, int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);

			return CBO.FillObject<PostInfo>(_dataProvider.GetUsersLastPost(userId, portalId));

			//TODO: Should we cache this? (being user specific, probably not)
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public List<QuestionInfo> KeywordSearch(int moduleId, string keyword)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);

			var colModuleQuestions = (List<QuestionInfo>)DataCache.GetCache(Constants.ModuleCacheKey + Constants.ModuleQuestionsCacheKey + moduleId);

			if (keyword == string.Empty)
			{
				if (colModuleQuestions == null)
				{
					var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

					colModuleQuestions = CBO.FillCollection<QuestionInfo>(_dataProvider.KeywordSearch(moduleId, keyword));

					// ReSharper disable RedundantLogicalConditionalExpressionOperand
					if ((timeOut > 0) && Constants.EnableCaching && (colModuleQuestions.Count > 0))
					// ReSharper restore RedundantLogicalConditionalExpressionOperand
					{
						DataCache.SetCache(Constants.ModuleCacheKey + Constants.ModuleQuestionsCacheKey + moduleId, colModuleQuestions, TimeSpan.FromMinutes(timeOut));
					}
				}
			}
			else
			{
				colModuleQuestions = CBO.FillCollection<QuestionInfo>(_dataProvider.KeywordSearch(moduleId, keyword));
			}
		  
			return colModuleQuestions;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="pageSize"></param>
		/// <param name="term"></param>
		/// <returns></returns>
		public List<QuestionInfo> TermSearch(int moduleId, int pageSize, string term) {
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("pageSize", "", pageSize);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("term", "", term);

			// TODO: Implement caching here
			return CBO.FillCollection<QuestionInfo>(_dataProvider.TermSearch(moduleId, pageSize, term));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="searchPhrase"></param>
		/// <returns></returns>
		public List<QuestionServiceInfo> SearchQuestionTitles(int moduleId, string searchPhrase)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("searchPhrase", "", searchPhrase);

			// TODO: Implement caching here
			return CBO.FillCollection<QuestionServiceInfo>(_dataProvider.SearchQuestionTitles(moduleId, searchPhrase));
		}

		/// <summary>
		/// Returns a single row of question data. 
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="portalId"></param>
		/// <returns>A QuestionInfo object.</returns>
		public QuestionInfo GetQuestion(int postId, int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", objPost.PostID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", objPost.PortalID);

			// TODO: Implement caching here (Not sure that is needed, since we are using for url's now we should consider it; but how to clear that way?)
			return (QuestionInfo)CBO.FillObject(_dataProvider.GetPost(postId, portalId), typeof(QuestionInfo));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentItemId"></param>
		/// <returns></returns>
		public QuestionInfo GetQuestionByContentItem(int contentItemId)
		{
			return (QuestionInfo)CBO.FillObject(_dataProvider.GetQuestionByContentItem(contentItemId), typeof(QuestionInfo));
		}

		/// <summary>
		/// This returns a list of questions for a single portal (Approved and not 'soft' deleted). This is necessary for the Sitemap provider.
		/// </summary>
		/// <param name="portalId"></param>
		/// <returns></returns>
		public List<QuestionInfo> GetSitemapQuestions(int portalId)
		{
			return CBO.FillCollection<QuestionInfo>(_dataProvider.GetSitemapQuestions(portalId));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="portalId"></param>
		public void IncreaseViewCount(int postId, int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", objPost.PostID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", objPost.PortalID);

			_dataProvider.IncreaseViewCount(postId, portalId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="postID"></param>
		/// <param name="portalID"></param>
		/// <returns></returns>
		public List<PostInfo> GetAnswers(int postID, int portalID) {
			//DotNetNuke.Common.Requires.PropertyNotNegative("postID", "", postID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("postID", "", portalID);

			// TODO: Implement caching here
			return CBO.FillCollection<PostInfo>(_dataProvider.GetAnswers(postID, portalID));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lastRunDate"></param>
		/// <param name="currentRunDate"></param>
		/// <returns></returns>
		public List<PostInfo> GetAnswersByDate(DateTime lastRunDate, DateTime currentRunDate)
		{
			return CBO.FillCollection<PostInfo>(_dataProvider.GetAnswersByDate(lastRunDate, currentRunDate));
		}

		/// <summary>
		/// Returns a single row of post data. Should only be necessary for editing a specific post. 
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="portalId"></param>
		/// <returns>A PostInfo object.</returns>
		public PostInfo GetPost(int postId, int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", objPost.PostID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", objPost.PortalID);

			return (PostInfo)CBO.FillObject(_dataProvider.GetPost(postId, portalId), typeof(PostInfo));
		}

		/// <summary>
		/// Adds a post to the data store and returns the PostInfo object (updated with PostID and ContentItemId) that we just added to the data store.
		/// </summary>
		/// <param name="objPost"></param>
		/// <param name="tabId"></param>
		/// <returns>The PostInfo object we added to the data store, along w/ it's newly created ContentItemId populated. Since update is called immediately after this (during question creation), no need to clear term cache here.</returns>
		public PostInfo AddPost(PostInfo objPost, int tabId) {
			//// title
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("body", "", objPost.Body);
			//DotNetNuke.Common.Requires.PropertyNotNegative("parentID", "", objPost.ParentID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalID", "", objPost.PortalID);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("approved", "", objPost.Approved.ToString());
			//// approvedDate
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("createdByUserID", "", objPost.CreatedUserID.ToString());
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("createdOnDate", "", objPost.CreatedDate.ToString());

			objPost.PostId = _dataProvider.AddPost(objPost.Title, objPost.Body, objPost.Bounty, objPost.ParentId, objPost.PortalId, objPost.ContentItemId, objPost.Approved, objPost.ApprovedDate, objPost.CreatedUserId, objPost.CreatedDate);

			if (objPost.ContentItemId < 1)
			{
				objPost.ContentItemId = CompleteQuestionCreation(objPost, tabId);
			}
			
			// handle cache clearing
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + objPost.ModuleID);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleQuestionsCacheKey + objPost.ModuleID);

			return objPost;
		}

		/// <summary>
		/// Updates an existing post in the data store.
		/// </summary>
		/// <param name="objPost"></param>
		/// <param name="tabId"></param>
		public void UpdatePost(PostInfo objPost, int tabId) {
			//DotNetNuke.Common.Requires.PropertyNotNegative("postID", "", objPost.PostID);
			//// title
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("body", "", objPost.Body);
			//DotNetNuke.Common.Requires.PropertyNotNegative("parentID", "", objPost.ParentID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalID", "", objPost.PortalID);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("approved", "", objPost.Approved.ToString());
			//// approvedDate
			//// answerID
			//// answerDate
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("closed", "", objPost.Closed.ToString());
			//// closedDate
			//// lastModifiedUserID
			//// lastModifiedDate
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("lastActivityDate", "", objPost.LastActivityDate.ToString());

			_dataProvider.UpdatePost(objPost.PostId, objPost.Title, objPost.Body, objPost.Bounty, objPost.ParentId, objPost.PortalId, objPost.ContentItemId, objPost.Approved, objPost.ApprovedDate, objPost.Deleted, objPost.AnswerId, objPost.AnswerDate, objPost.Closed, objPost.ClosedDate, objPost.Protected, objPost.ProtectedDate, objPost.LastModifiedUserId, objPost.LastModifiedDate);

			CompleteQuestionUpdate(objPost, tabId);
		}

		/// <summary>
		/// Deletes a post from the data store (and any children if applicable). Cascade deletes take care of votes too.
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="parentId"></param>
		/// <param name="portalId"></param>
		/// <param name="contentItemId"></param>
		/// <param name="softDelete"></param>
		/// <param name="moduleId"></param>
		/// <remarks>If a parent is deleted (ie. the actual question, thus the first post) it should delete all answers.
		/// It is also important to understand that deleting a post deletes any child posts (the idea is we only allow actual delete of an entire thread since we don't have nested replies like a forum).</remarks>
		public void DeletePost(int postId, int parentId, int portalId, int contentItemId, bool softDelete, int moduleId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", postId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("parentId", "", parentId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("contentItemId", "", contentItemId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);

			if (softDelete)
			{
				_dataProvider.SoftDeletePost(postId, portalId);
			}
			else
			{
				_dataProvider.DeletePost(postId, portalId);
			}

			if (parentId == 0)
			{
				// we always delete the content item if it was the question.
				CompleteQuestionDelete(contentItemId);
			}

			// handle cache clearing 
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + moduleId);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleQuestionsCacheKey + moduleId);
			// only update terms if the post was a question (no parent)
			if (parentId == 0)
			{
				DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeTermsCacheKey + moduleId);
				DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleTermsCacheKey + moduleId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="userId"></param>
		/// <param name="acceptedDate"></param>
		/// <param name="moduleId"></param>
		public void AcceptAnswer(int postId, int userId, DateTime acceptedDate, int moduleId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", postId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);

			_dataProvider.AcceptAnswer(postId, userId, acceptedDate);

			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + moduleId);
		}

		#region History

		/// <summary>
		/// 
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="notes"></param>
		/// <param name="approved"></param>
		/// <returns></returns>
		public int AddPostHistory(int postId, string notes, bool approved)
		{
			return _dataProvider.AddPostHistory(postId, notes, approved);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public List<PostHistoryInfo> GetPostHistory(int postId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", postId);
			return CBO.FillCollection<PostHistoryInfo>(_dataProvider.GetPostHistory(postId));
		}

		#endregion

		#endregion

		#region Schedule Item Settings

		public void UpdateScheduleItemSetting(int scheduleId, string key, string value)
		{
			_dataProvider.UpdateScheduleItemSetting(scheduleId, key, value);
		}

		#endregion

		#region Settings

		public List<SettingInfo> GetQaPortalSettings(int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			var colSettings = (List<SettingInfo>)DataCache.GetCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + portalId);

			if (colSettings == null)
			{
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

				colSettings = CBO.FillCollection<SettingInfo>(_dataProvider.GetQaPortalSettings(portalId));

				if (colSettings != null)
				{
					if (timeOut > 0 & Constants.EnableCaching & colSettings.Count > 1)
					{
						DataCache.SetCache(Constants.ModuleCacheKey + Constants.QaSettingsCacheKey + portalId, colSettings, TimeSpan.FromMinutes(timeOut));
					}
				}
			}
			return colSettings;
		}

		public void UpdateQaPortalSetting(SettingInfo objSetting)
		{
			_dataProvider.UpdateQaPortalSetting(objSetting.Key, objSetting.Value, objSetting.TypeId, objSetting.PortalId);
		}

		#endregion

		#region Subscribers

		public List<SubscriberInfo> GetSubscribersByContentItem(int contentItemId, int subscriptionType, int portalId)
		{
			return CBO.FillCollection<SubscriberInfo>(_dataProvider.GetSubscribersByContentItem(contentItemId, subscriptionType, portalId));
		}

		public List<SubscriberInfo> GetSubscribersByQuestion(int questionId, int subscriptionType, int portalId)
		{
			return CBO.FillCollection<SubscriberInfo>(_dataProvider.GetSubscribersByQuestion(questionId, subscriptionType, portalId));
		}

		#endregion

		#region Subscriptions

		public int AddSubscription(SubscriptionInfo objSub)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", objSub.PortalId);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("createdOnDate", "", objSub.CreatedOnDate.ToString());

			return _dataProvider.AddSubscription(objSub.PortalId, objSub.UserId, objSub.EmailAddress, objSub.PostId, objSub.TermId, objSub.SubscriptionType, objSub.CreatedOnDate);
		}

		public List<SubscriptionInfo> GetQuestionSubscribers(int portalId, int questionId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("questionId", "", questionId);
			return CBO.FillCollection<SubscriptionInfo>(_dataProvider.GetQuestionSubscriptions(portalId, questionId));
		}

		public List<SubscriptionInfo> GetTermSubscribers(int portalId, int termId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("termId", "", termId);
			return CBO.FillCollection<SubscriptionInfo>(_dataProvider.GetTermSubscriptions(portalId, termId));
		}

		public List<SubscriptionInfo> GetUserSubscriptions(int portalId, int userId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);

			// TODO: Implement caching here (because of usage in HeaderNav, this is A MUST!!!)
			return CBO.FillCollection<SubscriptionInfo>(_dataProvider.GetUserSubscriptions(portalId, userId));
		}

		public void DeleteSubscription(int portalId, int subscriptionId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalID", "", portalID);
			//DotNetNuke.Common.Requires.PropertyNotNegative("subscriptionId", "", subscriptionId);

			_dataProvider.DeleteSubscription(portalId, subscriptionId);
		}

		public void DeleteUserPostSubscription(int userId, int postId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("userId", "", userId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", postId);
			_dataProvider.DeleteUserPostSubscription(userId, postId);
		}

		#endregion

		#region Terms

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contentItemId"></param>
		/// <param name="vocabularyId"></param>
		/// <returns></returns>
		public List<TermInfo> GetTermsByContentItem(int contentItemId, int vocabularyId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("contentItemId", "", contentItemId);
			var colTerms = (List<TermInfo>)DataCache.GetCache(Constants.ModuleCacheKey + Constants.ContentTermsCacheKey + contentItemId);

			if (colTerms == null)
			{
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

				colTerms = CBO.FillCollection<TermInfo>(_dataProvider.GetTermsByContentItem(contentItemId, vocabularyId));

				if (timeOut > 0 & Constants.EnableCaching & colTerms != null)
				{
					DataCache.SetCache(Constants.ModuleCacheKey + Constants.ContentTermsCacheKey + contentItemId, colTerms, TimeSpan.FromMinutes(timeOut));
				}
			}
			return colTerms;
		}

		/// <summary>
		/// Returns a collection of terms (tags) associated with the DNN_Forge content type (ie. all tags used by the forge module content) for a single portal. 
		/// </summary>
		/// <param name="portalId"></param>
		/// <param name="moduleId"></param>
		/// <param name="vocabularyId"></param>
		/// <returns></returns>
		public List<TermInfo> GetTermsByContentType(int portalId, int moduleId, int vocabularyId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("moduleId", "", moduleId);
			var colTerms = (List<TermInfo>)DataCache.GetCache(Constants.ModuleCacheKey + Constants.ModuleTermsCacheKey + moduleId);

			if (colTerms == null)
			{
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

				colTerms = CBO.FillCollection<TermInfo>(_dataProvider.GetTermsByContentType(portalId, Content.GetContentTypeID(), moduleId, vocabularyId));

				if (timeOut > 0 & Constants.EnableCaching & colTerms != null)
				{
					DataCache.SetCache(Constants.ModuleCacheKey + Constants.ModuleTermsCacheKey + moduleId, colTerms, TimeSpan.FromMinutes(timeOut));
				}
			}
			return colTerms;
		}

		#region History

		public int AddTermHistory(int portalId, int termId, string notes, bool approved, int moduleId)
		{
			// we clear cache here since its a core method we are using to save (individual question/content items not handled)
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + moduleId);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeTermsCacheKey + moduleId);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleTermsCacheKey + moduleId);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleQuestionsCacheKey + moduleId);

			return _dataProvider.AddTermHistory(portalId, termId, notes, approved);
		}

		public List<TermHistoryInfo> GetTermHistory(int portalId, int termId)
		{
		   return CBO.FillCollection<TermHistoryInfo>(_dataProvider.GetTermHistory(portalId, termId));
		}

		#endregion

		#region Term Synonym

		public TermSynonymInfo AddTermSynonym(TermSynonymInfo objSynonym)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("MasterTermId", "", MasterTermId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("RelatedTermId", "", RelatedTermId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("CreatedByUserId", "", CreatedByUserId);

			objSynonym.TermSynonymId = _dataProvider.AddTermSynonym(objSynonym.MasterTermId, objSynonym.RelatedTermId, objSynonym.PortalId, objSynonym.CreatedByUserId, objSynonym.CreatedOnDate);
			objSynonym.Score = 0;
			objSynonym.ChangedCount = 0;

			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.TermSynonymsCacheKey + objSynonym.PortalId);

			return objSynonym;
		}

		public List<TermSynonymInfo> GetTermSynonyms(int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);

			var colTerms = (List<TermSynonymInfo>)DataCache.GetCache(Constants.ModuleCacheKey + Constants.TermSynonymsCacheKey + portalId);

			if (colTerms == null)
			{
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

				colTerms = CBO.FillCollection<TermSynonymInfo>(_dataProvider.GetTermSynonyms(portalId));

				if (timeOut > 0 & Constants.EnableCaching & colTerms != null)
				{
					DataCache.SetCache(Constants.ModuleCacheKey + Constants.TermSynonymsCacheKey + portalId, colTerms, TimeSpan.FromMinutes(timeOut));
				}
			}
			return colTerms;
		}

		public void TermSynonymReplaced(int relatedTermId, int portalId)
		{
			_dataProvider.TermSynonymReplaced(relatedTermId, portalId);
		}

		public void DeleteTermSynonym(int masterTermId, int relatedTermId, int portalId)
		{
			_dataProvider.DeleteTermSynonym(masterTermId, relatedTermId, portalId);
		}

		#endregion
		
		#endregion

		#region User Badges

		public List<UserBadgeInfo> GetBadgeRecipients(int badgeId)
		{
			return CBO.FillCollection<UserBadgeInfo>(_dataProvider.GetBadgeRecipients(badgeId));
		}

		#endregion
		
		#region User Scoring

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objScoreLog"></param>
		/// <param name="sitePrivileges"></param>
		public void AddScoringLog(UserScoreLogInfo objScoreLog, IEnumerable<QaSettingInfo> sitePrivileges)
		{
			if (objScoreLog.Score > 0)
			{
				var currentScore = GetUserScore(objScoreLog.UserId, objScoreLog.PortalId);
				var oldTotal = 0;

				if (currentScore != null)
				{
				   oldTotal = currentScore.Score; 
				}

				var newScore = oldTotal + objScoreLog.Score;

				_dataProvider.AddScoringLog(objScoreLog.UserId, objScoreLog.PortalId, objScoreLog.UserScoringActionId, objScoreLog.Score, objScoreLog.KeyId, objScoreLog.Notes, objScoreLog.CreatedOnDate);

				// determine if the user gained a privilege with the last action
				var colPrivs = (from t in sitePrivileges where (t.Value > oldTotal) && (newScore >= t.Value) orderby t.Value ascending select t).ToList();

				if (colPrivs.Count() > 0)
				{
					foreach (var privilege in colPrivs)
					{
						// this should only be one (we sort ascending so the one w/ the highest score is recorded last in case of multiples)
						// we need to set something in the db so we know to inform the user on their next view.
						_dataProvider.SetUserScoreMessage(objScoreLog.UserId, objScoreLog.PortalId, privilege.Key);
					}
				}
			}
			else
			{
				_dataProvider.AddScoringLog(objScoreLog.UserId, objScoreLog.PortalId, objScoreLog.UserScoringActionId, objScoreLog.Score, objScoreLog.KeyId, objScoreLog.Notes, objScoreLog.CreatedOnDate);
			}
		   
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.UserScoreCacheKey + objScoreLog.UserId + @"_" + objScoreLog.PortalId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="portalId"></param>
		/// <returns></returns>
		public UserScoreInfo GetUserScore(int userId, int portalId)
		{
			UserScoreInfo objUserScore;
			if (DataCache.GetCache(Constants.ModuleCacheKey + Constants.UserScoreCacheKey + userId + @"_" + portalId) != null)
			{
				objUserScore = (UserScoreInfo)DataCache.GetCache(Constants.ModuleCacheKey + Constants.UserScoreCacheKey + userId + @"_" + portalId);
			}
			else
			{
				var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);
				objUserScore = CBO.FillObject<UserScoreInfo>(_dataProvider.GetUserScore(userId, portalId));

				if (timeOut > 0 & Constants.EnableCaching)
				{
					DataCache.SetCache(Constants.ModuleCacheKey + Constants.UserScoreCacheKey + userId + @"_" + portalId, objUserScore, TimeSpan.FromMinutes(timeOut));
				}
			}

			return objUserScore;
		}

		public List<UserScoreInfo> GetUserScoresByPortal(int portalId)
		{
			return CBO.FillCollection<UserScoreInfo>(_dataProvider.GetUserScoresByPortal(portalId));
		}

		public List<UserScoreLogInfo> GetUserScoreLog(int userId, int portalId)
		{
			return CBO.FillCollection<UserScoreLogInfo>(_dataProvider.GetUserScoreLog(userId, portalId));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyId"></param>
		/// <param name="portalId"></param>
		/// <returns></returns>
		public List<UserScoreLogInfo> GetUserScoreLogByKey(int keyId, int portalId)
		{
			return CBO.FillCollection<UserScoreLogInfo>(_dataProvider.GetUserScoreLogByKey(keyId, portalId));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="portalId"></param>
		/// <param name="userScoringActionId"></param>
		/// <param name="score"></param>
		/// <param name="keyId"></param>
		public void DeleteUserScoreLog(int userId, int portalId, int userScoringActionId, int score, int keyId)
		{
			_dataProvider.DeleteUserScoreLog(userId, portalId, userScoringActionId, score, keyId);

			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.UserScoreCacheKey + userId + @"_" + portalId);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="portalId"></param>
		public void ClearUserScoreMessage(int userId, int portalId)
		{
			_dataProvider.ClearUserScoreMessage(userId, portalId);

			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.UserScoreCacheKey + userId + @"_" + portalId);
		}

		#endregion

		#region Votes

		/// <summary>
		/// Adding a vote will automatically update a posts score column too, in addition to logging the vote. Also, last activity date for post as well as parent post (if applicable) are updated.
		/// </summary>
		/// <param name="objVote"></param>
		/// <param name="moduleId"></param>
		/// <remarks>Logic in stored procedure based on enumerator values for VoteTypeID. Also, this is used for flagging a post too.</remarks>
		/// <returns>The primary key value of the vote just added (ie. VoteID).</returns>
		public int AddVote(VoteInfo objVote, int moduleId) {
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("voteTypeID", "", objVote.VoteTypeID);
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("createdByUserID", "", objVote.CreatedByUserID.ToString());
			//DotNetNuke.Common.Requires.PropertyNotNullOrEmpty("createdOnDate", "", objVote.CreatedOnDate.ToString());
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", objVote.PortalId);

			var voteId = _dataProvider.AddVote(objVote.PostId, objVote.TermId, objVote.VoteTypeId, objVote.PortalId, objVote.CreatedByUserId, objVote.CreatedOnDate);

			// handle cache clearing stuff
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + moduleId);

			return voteId;
		}

		/// <summary>
		/// Used to retrieve all votes (this is up/down/flagged) for each post.
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public List<VoteInfo> GetPostVotes(int postId) {
			//DotNetNuke.Common.Requires.PropertyNotNegative("postId", "", postId);

			// TODO: Implement caching here
			return CBO.FillCollection<VoteInfo>(_dataProvider.GetPostVotes(postId));
		}

		public List<VoteInfo> GetTermSynonymVotes(int termId, int portalId)
		{
			//DotNetNuke.Common.Requires.PropertyNotNegative("termId", "", termId);
			//DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);

			// TODO: Implement caching here
			return CBO.FillCollection<VoteInfo>(_dataProvider.GetTermSynonymVotes(termId, portalId));
		}

		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// This completes the things necessary for creating a content item in the data store. 
		/// </summary>
		/// <param name="objPost">The PostInfo entity we just created in the data store.</param>
		/// <param name="tabId">The page we will associate with our content item.</param>
		/// <returns>The ContentItemId primary key created in the Core ContentItems table.</returns>
		private static int CompleteQuestionCreation(PostInfo objPost, int tabId) {
			var cntTaxonomy = new Content();
			var objContentItem = cntTaxonomy.CreateContentItem(objPost, tabId);

			return objContentItem.ContentItemId;
		}

		/// <summary>
		/// Handles any content item/taxonomy updates, then deals with cache clearing. 
		/// </summary>
		/// <param name="objPost"></param>
		/// <param name="tabId"></param>
		private static void CompleteQuestionUpdate(PostInfo objPost, int tabId) {
			var cntTaxonomy = new Content();
			cntTaxonomy.UpdateContentItem(objPost, tabId);

			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeQuestionsCacheKey + objPost.ModuleID);

			if (objPost.ParentId >= 1) return;
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.HomeTermsCacheKey + objPost.ModuleID);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleTermsCacheKey + objPost.ModuleID);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ModuleQuestionsCacheKey + objPost.ModuleID);
			DataCache.RemoveCache(Constants.ModuleCacheKey + Constants.ContentTermsCacheKey + objPost.ContentItemId);

		}

		/// <summary>
		/// Cleanup any taxonomy related items.
		/// </summary>
		/// <param name="contentItemID"></param>
		private static void CompleteQuestionDelete(int contentItemID)
		{
			var cntTaxonomy = new Content();
			cntTaxonomy.DeleteContentItem(contentItemID);
		}

		#endregion

	}
}