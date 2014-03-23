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
using System.Web;
using DotNetNuke.UI.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;

namespace DotNetNuke.DNNQA.Components.Common {

	/// <summary>
	/// A class to hold common functions used throughout the module. 
	/// </summary>
	public class Links {

		public static string Home(int tabId)
		{
			return DotNetNuke.Common.Globals.NavigateURL(tabId, "", "");
		}

		public static string AskQuestion(ModuleInstanceContext modContext)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Ask.ToString().ToLower());
		}

		public static string EditPost(ModuleInstanceContext modContext, int postId)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.EditPost.ToString().ToLower(), "id=" + postId);
		}

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="modContext"></param>
	    /// <param name="tabId"></param>
	    /// <param name="termName">The taxonomy term the user is attempting to edit.</param>
	    /// <returns></returns>
	    public static string EditTag(ModuleInstanceContext modContext, int tabId, string termName)
		{
			return modContext.NavigateUrl(tabId, "", false, "view=" + Constants.PageScope.EditTerm.ToString().ToLower(), "term=" + HttpUtility.UrlEncode(termName));
		}

		public static string KeywordSearch(ModuleInstanceContext modContext, string searchString)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "keyword=" + HttpUtility.UrlEncode(searchString));
		}

		public static string KeywordSearchSorted(ModuleInstanceContext modContext, string keyword, string sortBy)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "keyword=" + keyword, "sort=" + sortBy);
		}

		public static string ViewBadge(ModuleInstanceContext modContext, string name, int id)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Badge.ToString().ToLower(), "name=" + id, name);
		}

		public static string ViewBadges(ModuleInstanceContext modContext)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Badges.ToString().ToLower());
		}

		public static string ViewFilteredBadges(ModuleInstanceContext modContext, string filter)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Badges.ToString().ToLower(), "filter=" + filter);
		}

		public static string ViewQuestion(int questionId, int tabId, PortalSettings ps)
		{
			return DotNetNuke.Common.Globals.NavigateURL(tabId,ps,"", "view=" + Constants.PageScope.Question.ToString().ToLower(), "id=" + questionId);   
		}

		/// <summary>
		/// This is the friendly URL format for viewing a question (handles re-writting on/off).
		/// </summary>
		/// <param name="questionId"></param>
		/// <param name="questionTitle"></param>
		/// <param name="tab"></param>
		/// <param name="ps"></param>
		/// <returns></returns>
		public static string ViewQuestion(int questionId, string questionTitle, TabInfo tab, PortalSettings ps)
		{
			if (Utils.IsFriendlyUrlModuleInstalled && Utils.UseFriendlyUrls)
			{
				// JS 1/25/12: This check is for removing the .aspx from the question and tags.
				//             There appears to be conflicts between IIS7.5 installations that need to be address
				//if (HttpRuntime.UsingIntegratedPipeline)
				//{
				//    return DotNetNuke.Common.Globals.NavigateURL(tab.TabID).Replace(".aspx", "") + ("/Question/" + questionId + "/" + Utils.CreateFriendlySlug(questionTitle));
				//}

				if (ps.HomeTabId == tab.TabID && DotNetNuke.Common.Globals.NavigateURL(tab.TabID).EndsWith("/"))
				{
					return DotNetNuke.Common.Globals.NavigateURL(tab.TabID).Replace(".aspx", "") + tab.IndentedTabName.Replace(" ","") + ("/" + Utils.GetQuestionUrlName() + "/" + questionId + "/" + Utils.CreateFriendlySlug(questionTitle) + ".aspx");
				}
				return DotNetNuke.Common.Globals.NavigateURL(tab.TabID).Replace(".aspx", "") + ("/" + Utils.GetQuestionUrlName() + "/" + questionId + "/" + Utils.CreateFriendlySlug(questionTitle) + ".aspx");
				
				
			}
			return ViewQuestion(questionId, tab.TabID, ps);
		}

		/// <summary>
		/// This URL is used when ModuleContext is not present (could happen from service, can't be used for tasks).
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="questionId"></param>
		/// <returns></returns>
		public static string ViewQuestion(int tabId, int questionId)
		{
			return DotNetNuke.Common.Globals.NavigateURL(tabId, "", "view=" + Constants.PageScope.Question.ToString().ToLower(), "id=" + questionId);
		}

		public static string ViewQuestionSorted(ModuleInstanceContext modContext, int questionId, string sortBy)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Question.ToString().ToLower(), "id=" + questionId, "sort=" + sortBy);
		}

		public static string ViewQuestions(ModuleInstanceContext modContext)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions");
		}

		public static string ViewQuestionsPaged(ModuleInstanceContext modContext, int page)
		{
			return page > 1 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "page=" + page) : ViewQuestions(modContext);
		}

		public static string ViewQuestionsSorted(ModuleInstanceContext modContext, string sortBy, bool unanswered, int page)
		{
			if (unanswered)
			{
				return ViewUnansweredQuestions(modContext, page, sortBy);
			}
			return page > 1 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "sort=" + sortBy, "page=" + page) : modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "sort=" + sortBy);
		}

		public static string ViewUnansweredQuestions(ModuleInstanceContext modContext, int page, string sortBy)
		{
			if (page > 1)
			{
				return sortBy.Length > 0 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "sort=" + sortBy, "unanswered=true", "page=" + page) : modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "unanswered=true", "page=" + page);
			}
			return sortBy.Length > 0 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "sort=" + sortBy, "unanswered=true") : modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "unanswered=true");
		}

		public static string ViewPrivilege(ModuleInstanceContext modContext, string privilegeName)
		{
			return privilegeName != string.Empty ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Convert.ToString(Constants.PageScope.Privileges).ToLower(), "privilege=" + privilegeName) : modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Convert.ToString(Constants.PageScope.Privileges).ToLower(), "");
		}

		public static string ViewTags(ModuleInstanceContext modContext)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Tags, "");
		}

		public static string ViewTagsPaged(ModuleInstanceContext modContext, int page)
		{
			return page > 1 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Tags.ToString().ToLower(), "page=" + page) : ViewTags(modContext);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modContext"></param>
		/// <param name="sortBy"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public static string ViewTagsSorted(ModuleInstanceContext modContext, string sortBy, int page)
		{
			return page > 1 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Tags.ToString().ToLower(), "sort=" + sortBy, "page=" + page) : modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Tags.ToString().ToLower(), "sort=" + sortBy);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modContext"></param>
		/// <param name="filter"></param>
		/// <param name="sortBy"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		public static string ViewTagsSortedAndFiltered(ModuleInstanceContext modContext, string filter, string sortBy, int page)
		{
			return page > 1 ? modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Tags.ToString().ToLower(), "filter=" + HttpUtility.UrlEncode(filter), "sort=" + sortBy, "page=" + page) : modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Tags.ToString().ToLower(), "filter=" + HttpUtility.UrlEncode(filter), "sort=" + sortBy);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modContext"></param>
		/// <param name="termName">The taxonomy term the user wants to view the details of.</param>
		/// <returns></returns>
		public static string ViewTagDetail(ModuleInstanceContext modContext, int tabId, string termName)
		{
			return modContext.NavigateUrl(tabId, "", false, "view=" + Constants.PageScope.TermDetail.ToString().ToLower(), "term=" + HttpUtility.UrlEncode(termName));
		}

		public static string ViewTermSynonyms(ModuleInstanceContext modContext, string termName)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.TermSynonyms.ToString().ToLower(), "term=" + HttpUtility.UrlEncode(termName));
		}

		public static string ViewTaggedQuestions(string termName, int tabId, PortalSettings ps)
		{
			return DotNetNuke.Common.Globals.NavigateURL(tabId, ps,"", "view=" + Constants.PageScope.Browse.ToString().ToLower(), "tag=" + HttpUtility.UrlEncode(termName));
		}

		public static string ViewTaggedQuestions(string termName, TabInfo tab, PortalSettings ps)
		{
			if (Utils.IsFriendlyUrlModuleInstalled && Utils.UseFriendlyUrls)
			{
				// JS 1/25/12: This check is for removing the .aspx from the question and tags.
				//             There appears to be conflicts between IIS7.5 installations that need to be address
				//if (HttpRuntime.UsingIntegratedPipeline)
				//{
				//    return DotNetNuke.Common.Globals.NavigateURL(tab.TabID).Replace(".aspx", "") + ("/Tag/" + HttpUtility.UrlEncode(termName));
				//}

				termName = termName.Replace(" ", "-");
				if (ps.HomeTabId == tab.TabID && DotNetNuke.Common.Globals.NavigateURL(tab.TabID).EndsWith("/")){
					return DotNetNuke.Common.Globals.NavigateURL(tab.TabID) + tab.IndentedTabName.Replace(" ","") + ("/" + Utils.GetTagUrlName(ps) + "/" + HttpUtility.UrlEncode(termName) + ".aspx");
				}
				return DotNetNuke.Common.Globals.NavigateURL(tab.TabID).Replace(".aspx", "") + ("/" + Utils.GetTagUrlName(ps) + "/" + HttpUtility.UrlEncode(termName) + ".aspx");
			}
			return DotNetNuke.Common.Globals.NavigateURL(tab.TabID, ps, "", "view=" + Constants.PageScope.Browse.ToString().ToLower(), "tag=" + HttpUtility.UrlEncode(termName));
		}

		public static string ViewTaggedQuestionsSorted(ModuleInstanceContext modContext, string termName, string sortBy)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "tag=" + HttpUtility.UrlEncode(termName), "sort=" + sortBy);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modContext"></param>
		/// <param name="termName">The taxonomy term the user wants to view the.</param>
		/// <returns></returns>
		public static string ViewTagHistory(ModuleInstanceContext modContext, string termName)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.TermHistory.ToString().ToLower(), "term=" + HttpUtility.UrlEncode(termName));
		}

		public static string ViewUserAnswers(ModuleInstanceContext modContext, int currentUserID)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=answers", "user=" + currentUserID);
		}

		public static string ViewUserAnswersSorted(ModuleInstanceContext modContext, int currentUserID, string sortBy)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=answers", "user=" + currentUserID, "sort=" + sortBy);
		}

		public static string ViewUserQuestions(ModuleInstanceContext modContext, int currentUserID)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "user=" + currentUserID);
		}

		public static string ViewUserQuestionsSorted(ModuleInstanceContext modContext, int currentUserID, string sortBy)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Browse.ToString().ToLower(), "type=questions", "user=" + currentUserID, "sort=" + sortBy);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tabId"></param>
		/// <param name="ps"></param>
		/// <returns></returns>
		/// <remarks>This is meant to be used when no context is available, like a scheduled task.</remarks>
		public static string ViewUserSubscriptions(int tabId, PortalSettings ps)
		{
			return DotNetNuke.Common.Globals.NavigateURL(tabId, ps, "view=" + Constants.PageScope.Subscriptions.ToString().ToLower());
		}

		public static string ViewUserSubscriptions(ModuleInstanceContext modContext)
		{
			return modContext.NavigateUrl(modContext.TabId, "", false, "view=" + Constants.PageScope.Subscriptions.ToString().ToLower(), "");
		}

	}
}