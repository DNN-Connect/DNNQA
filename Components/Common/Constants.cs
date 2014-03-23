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

namespace DotNetNuke.DNNQA.Components.Common {

	public class Constants
	{

		#region Module Settings
	
		// General
		internal const string SettingNameFormat = "DNNQA_NameFormat";
		internal const string SettingEnableRss = "DNNQA_EnableRss";
		
		// Ask Question
		internal const string SettingMinTitleChars = "DNNQA_MinimumTitleCharacters";
		internal const string SettingMinBodyChars = "DNNQA_MinimumBodyCharacters";
		internal const string SettingMaxQuestionTags = "DNNQA_MaximumQuestionTags";
		internal const string SettingAutoApprove = "DNNQA_AutoApprove";

		// UI
		internal const string SettingHomePageSize = "DNNQA_HomePageSize";
		internal const string SettingHomeMaxTags = "DNNQA_HomeMaxTags";
		internal const string SettingHomeTagTimeFrame = "DNNQA_HomeTagTimeFrame";
		internal const string SettingBrowseQPageSize = "DNNQA_BrowseQPageSize";
		internal const string SettingAnswerPageSize = "DNNQA_AnswerPageSize";
		internal const string SettingMaxTagsTags = "DNNQA_MaxTagsTags";

		internal const string SettingsFacebookAppId = "DNNQA_FacebookAppId";
		internal const string SettingsEnablePlusOne = "DNNQA_EnablePlusOne";
		internal const string SettingsEnableTwitter = "DNNQA_EnableTwitter";
		internal const string SettingsEnableLinkedIn = "DNNQA_EnableLinkedIn";

		internal const string DefaultNameFormat = "DisplayName";
		internal const bool DefaultEnableRss = false;
		internal const bool DefaultAutoApprove = true;

		// UI
		internal const int DefaultPageSize = 20;
		internal const int DefaultHomeMaxTags = 20;
		internal const string DefaultHomeTagTimeFrame = "DailyUsage";

		#endregion

		#region Custom Settings

		public const int DefaultOpQuestionCloseCompleteVoteCount = 5;
		public const int DefaultOpQuestionCloseWindowDays = 4;
		public const int DefaultOpQuestionFlagHomeRemoveCount = 3;
		public const int DefaultOpPostChangeVoteWindowMinutes = 5;
		public const int DefaultOpPostFlagCompleteCount = 6;
		public const int DefaultOpPostFlagWindowHours = 48;
		public const int DefaultOpTagCloseWindowDays = 4;
		public const int DefaultOpTagFlagWindowHours = 48;
		public const int DefaultOpTagFlagCompleteCount = 6;
		public const int DefaultOpTermSynonymApproveCount = 4;
		public const int DefaultOpTermSynonymRejectCount = 2;
		public const int DefaultOpTermSynonymMaxCount = 10;
		public const int DefaultOpUserCloseVoteCount = 24;
		public const int DefaultOpUserFlagPostModerateCount = 10;
		public const int DefaultOpUserFlagPostSpamCount = 5;
		public const int DefaultOpUserTermSynonymCreateMinAnswerCount = 5;
		public const int DefaultOpUserTermSynonymVoteMinAnswerScoreCount = 5;
		public const int DefaultOpUserUpVoteAnswerCount = 30;
		public const int DefaultOpUserUpVoteQuestionCount = 40;
		public const int DefaultOpHomeQuestionMinScore = 0;

		public const int DefaultPrivView = 0;
		public const int DefaultPrivCreatePost = 1;
		public const int DefaultPrivRemoveNewUser = 5;
		public const int DefaultPrivFlag = 10;
		public const int DefaultPrivVoteUp = 15;
		public const int DefaultPrivCommentEverywhere = 50;
		public const int DefaultPrivVoteDown = 125;
		public const int DefaultPrivRetagQuestion = 500;
		public const int DefaultPrivEditQuestionsAndAnswers = 2000;
		public const int DefaultPrivCreateTagSynonym = 2500;
		public const int DefaultPrivCloseQuestion = 3000;
		public const int DefaultPrivApproveTagEdits = 5000;
		public const int DefaultPrivModeratorTools = 10000;
		public const int DefaultPrivProtectQuestions = 15000;
		public const int DefaultPrivTrusted = 20000;

		//these first 3 items are always custom, don't give a default value besides 0
		public const int DefaultScoreAdminEntered = 0;
		public const int DefaultScoreBountyPaid = 0;
		public const int DefaultScoreBountyReceived = 0;
		public const int DefaultScoreFirstLoggedInView = 2; // unlike other actions, this can only happen once per user
		public const int DefaultScoreAskedQuestion = 1; 
		public const int DefaultScoreProvidedAnswer = 1; 
		public const int DefaultScoreProvidedAcceptedAnswer = 5;
		public const int DefaultScoreAskedQuestionVotedUp = 5;
		public const int DefaultScoreVotedUpQuestion = 0;
		public const int DefaultScoreProvidedAnswerVotedUp = 10;
		public const int DefaultScoreVotedUpAnswer = 0;
		public const int DefaultScoreAskedFlaggedQuestion = -2;
		public const int DefaultScoreProvidedFlaggedAnswer = -2;
		public const int DefaultScoreAskedQuestionVotedDown = -2;
		public const int DefaultScoreVotedDownQuestion = 0;
		public const int DefaultScoreProvidedAnswerVotedDown = -2;
		public const int DefaultScoreVotedDownAnswer = -1;
		public const int DefaultScoreCreatedTagSynonym = 2;
		public const int DefaultScoreVotedSynonymUp = 1;
		public const int DefaultScoreVotedSynonymDown = 0;
		public const int DefaultScoreEditedTag = 0;
		public const int DefaultScoreEditedTagVotedUp = 0;
		public const int DefaultScoreVotedTagUp = 1;
		public const int DefaultScoreEditedTagVotedDown = 0;
		public const int DefaultScoreVotedTagDown = 0;
		public const int DefaultScoreApprovedTagEdit = 0;
		public const int DefaultScoreEditedPost = 0;
		public const int DefaultScoreApprovedPostEdit = 0;
		public const int DefaultScoreCommented = 0;
		public const int DefaultScoreAcceptedAnswer = 1;

		#endregion

		#region Cache Keys

		/// <summary>
		/// The prefix to be applied to all cached objects in this module (to help ensure the name is unique). 
		/// </summary>
		internal const string ModuleCacheKey = "DNNQA_";

		internal const string QaSettingsCacheKey = "qaSettings_";

		internal const string HomeQuestionsCacheKey = "qaHomeQuestions_";

		internal const string HomeTermsCacheKey = "qaHomeTerms_";

		internal const string ModuleTermsCacheKey = "qaModuleTerms_";

		internal const string ContentTermsCacheKey = "qaContentTerms_";

		internal const string UserScoreCacheKey = "qaUserScore_";

		internal const string IsFriendlyUrlModuleInstalled = "IsFriendlyUrlModuleInstalled";

		internal const string ModuleQuestionsCacheKey = "qaModuleQuestions_";

		internal const string TermSynonymsCacheKey = "qaTermSynonyms_";

		#endregion

		#region Misc.

		/// <summary>
		/// The name of the content type stored in the ContentTypes table of the core.
		/// </summary>
		public const string ContentTypeName = "DNN_DNNQA_Question";
		public const string JournalVoteTypeName = "vote";
		public const string JournalCommentTypeName = "comment";
		public const string JournalQuestionTypeName = "question";
		public const string JournalAnswerTypeName = "answer";

	    public const string NotificationQaFlag = "DNN_DNNQA_Flag";

		// This may be abstracted at some point.
		public const string JournalPrivilegeTypeName = "privilege";

		/// <summary>
		/// The relative path to the shared resource file for this module.
		/// </summary>
		public const string SharedResourceFileName = "~/DesktopModules/DNNQA/App_LocalResources/SharedResources.resx";

		/// <summary>
		/// Allows an easy way to enable/disable in the module itself without having to alter the entire install (although it does require a compile to change/deployl). 
		/// </summary>
		public const bool EnableCaching = true;

		/// <summary>
		/// A recommended limit for a meta page title for SEO purposes.
		/// </summary>
		public const int SeoTitleLimit = 64; 

		/// <summary>
		/// A recommended limit for a meta page description for SEO purposes.
		/// </summary>
		public const int SeoDescriptionLimit = 150; 

		/// <summary>
		/// A recommended limit for meta page keywords for SEO purposes.
		/// </summary>
		public const int SeoKeywordsLimit = 15; 

		public const string DisallowedCharacters = "%?*&;:'\\";
		
		#endregion
		
		#region Enumerators

		/// <summary>
		/// Badge Tiers are a way of adding some prestige to badges. The higher the tier, the more prestigious (gold = 3; this is the top). 
		/// </summary>
		/// <remarks>Stored as an enum so it can be localized, as well as limited (at least for now).</remarks>
		public enum BadgeTiers
		{
			Bronze = 1,
			Silver = 2,
			Gold = 3
		}

		/// <summary>
		/// 
		/// </summary>
		public enum EmailSettings
		{
			CommentTemplate = 0,
			FromAddress = 1,
			SingleQuestionTemplate = 2,
			SummaryTemplate = 3,
			AnswerTemplate = 4
		}

		/// <summary>
		/// There are several actions related to moderation that are stored in the _Moderation_Log table and these are the types associated with them. For example, a comment deletion is logged here. It is also worth mentioning that any moderation action performed by the system (due to reaching an op threshold not logged elsewhere) is also logged (ie. SysClearFlag - removes expired flags).
		/// </summary>
		/// <remarks>The 'delete' & 'flag' (items that start w/ that name) actions are only available to end users via the Answers.ascx screen.</remarks>
		public enum ModerationLogType
		{
			CommentHardDelete = 0,
			DeleteLowQuality = 1,
			DeleteSpam = 2,
			DeleteOutOfContext = 3,
			DeleteOther = 4,
			FlagLowQuality = 5,
			FlagSpam = 6,
			FlagOutOfContext = 7,
			FlagOther = 8,
			PostHardDelete = 9,
			PostRolledBack = 10,
			/// <summary>
			/// This occurs when the OpThreshold PostFlagWindowHours is not met.
			/// </summary>
			SysClearFlags = 11,
			/// <summary>
			/// This occurs when the OpThreshold QuestionCloseCompleteVoteCount is not met within the threshold QuestionCloseWindowDays.
			/// </summary>
			SysClearQuestionClose = 12,
			/// <summary>
			/// This occurs when the Op Threshold PostFlagCompleteCount is reached within the threshold of PostFlagWindowHours.
			/// </summary>
			SysFlagLimitReached = 13,
			/// <summary>
			/// The OpThreshold QuestionFlagHomeRemoveCount was reached within the PostFlagWindowHours threshold.
			/// </summary>
			SysQuestionRemovedFromHome = 14,
			TermRolledBack = 15,
			/// <summary>
			/// This occurs when the OpThreshold TermSynonymApproveCount is met, prior to threshold TermSynonymRejectCount being met.
			/// </summary>
			SysTermSynonymApproved = 16,
			/// <summary>
			/// This occurs when the OpThreshold TermSynonymRejectCount is met. 
			/// </summary>
			SysTermSynonymRejected = 17,
			RemoveFlag = 18
		}

		/// <summary>
		/// Operational thresholds are settings that represent points in which business rules take effect. 
		/// </summary>
		/// <remarks>Flagging an answer, pushing it above the PostFlagCompleteCount within the PostFlagWindowHours, will close the post (which runs ProvidedFlaggedAnswer scoring action against the poster) and performs a 'soft delete' on the individual post.</remarks>
		/// <remarks>Anytime a scoring action is added, it needs to be added to the Settings.ascx UI, QaSettings classes for population and to the ShareResources.resx file.</remarks>
		public enum OpThresholds
		{
			QuestionCloseCompleteVoteCount,
			QuestionCloseWindowDays,
			QuestionFlagHomeRemoveCount,
			PostChangeVoteWindowMinutes,
			PostFlagCompleteCount,
			PostFlagWindowHours,         
			TagCloseWindowDays,
			TagFlagWindowHours,
			TagFlagCompleteCount,
			TermSynonymApproveCount,
			TermSynonymRejectCount,
			TermSynonymMaxCount,
			UserCloseVoteCount,
			UserFlagPostModerateCount,
			UserFlagPostSpamCount,
			UserTermSynonymCreateMinAnswerCount, // this is combined w/ rep level on a specific term
			UserTermSynonymVoteMinAnswerScoreCount, // this is based on specific term (upvotes - downvotes)
			UserUpVoteAnswerCount,
			UserUpVoteQuestionCount,     
			QuestionHomeMinScore,
		}

		/// <summary>
		/// These are the various user interfaces loaded into the dispatch control. 
		/// </summary>
		public enum PageScope {
			TermSynonyms = -1,
			Home = 0,
			Question = 1,
			Ask = 2,
			Browse = 3,
			Tags = 4,
			TermDetail = 5,
			Subscriptions = 6,
			TermHistory = 7,
			EditTerm = 8,
			PostHistory = 9,
			Privileges = 10,
			EditPost = 11,
			Badges = 12,
			Badge = 13,
			Users = 14
		}

		/// <summary>
		/// These are the privilege levels a user can achieve. In a sense, these are different permission levels that can be achieved through user scoring actions. The value of each action is stored in the module's _Setting table and is portal specific. User's who are listed as moderator's (via the module's permissions), are considered to be at the highest 'Trusted' level. Privileges must be defined at the module level and based on  on a user's 'reuptation' level (either portal wide or module specific). 
		/// </summary>
		/// <remarks>The end result of "Trusted" is like achieving a level of access equivalent to that of a long time (and active) community member (specific to this module).</remarks>
		/// <remarks>Anytime a scoring action is added, it needs to be added to the Settings.ascx UI, QaSettings classes for population and to the ShareResources.resx file.</remarks>
		public enum Privileges
		{
			ViewAll = -1,
			CreatePost = 0,
			Flag = 1,
			VoteUp = 2,
			CommentEverywhere = 3,
			VoteDown = 4,
			RetagQuestion = 5,
			//CreateTag = 6, // not sure we are going to use this
			EditQuestionsAndAnswers = 7,
			CreateTagSynonym = 8,
			CloseQuestion = 9,
			ApproveTagEdits = 10, // not sure phase I
			ModeratorTools = 11, // not sure phase I (include: rollback post)
			ProtectQuestions = 12,
			Trusted = 13, // not sure phase I
			RemoveNewUser = 14
		}

		/// <summary>
		/// 
		/// </summary>
		public enum ScheduleItemSettings
		{
			InstantLastRunDate,
			DailyLastRunDate
		}

		/// <summary>
		/// These are the parameter types that can be applied to a search. 
		/// </summary>
		public enum SearchFilterType
		{
			User = 0,
			Type = 1,
			Tag = 2,
			Content = 3,
			Unanswered = 4
		}

		/// <summary>
		/// Used to simplify retrieval of custom settings (so they can be stored as one collection and cached, then queried against via linq). 
		/// </summary>
		public enum SettingTypes
		{
			PrivilegeLevelScore = 0,
			UserScoringActionValue = 1,
			OperationalThresholds = 2,
			Email = 3
		}

		/// <summary>
		/// The direction a column is being sorted by (only 1 of 3 possible states).
		/// </summary>
		public enum SortDirection
		{
			Ascending = 1,
			Descending = 2
		}

		/// <summary>
		/// These are the subscription types stored per each subscription. We use an enum here to avoid the need for a lookup table.
		/// </summary>
		public enum SubscriptionType
		{
			InstantPost = 0,
			InstantTerm = 1,
			DailyTerm = 2
		}

		/// <summary>
		/// 
		/// </summary>
		public enum TagMode
		{
			ShowNoUsage = 0,
			ShowDailyUsage = 1,
			ShowWeeklyUsage = 2,
			ShowMonthlyUsage = 3,
			ShowTotalUsage = 4
		}

		/// <summary>
		/// These are the various actions a user can perform to alter his/her score (in a postive or negative manner). These actions, and their values, are stored in the module's _Setting table and are portal specific. 
		/// </summary>
		/// <remarks>Anytime a scoring action is added, it needs to be added to the Settings.ascx UI and code, QaSettings classes for population and in the ShareResources.resx file (as Score_NAME, Score_NAME_Desc, NAME.</remarks>
		public enum UserScoringActions
		{
			AdminEntered = 0,
			ApprovedPostEdit = 1,
			ApprovedTagEdit = 2,
			AskedFlaggedQuestion = 3,
			AskedQuestion = 4,
			AskedQuestionVotedDown = 5,
			AskedQuestionVotedUp = 6,
			BountyPaid = 7,
			BountyReceived = 8,
			//CreatedTag = 9,, // we are not using this
			CreatedTagSynonym = 10,
			Commented = 11,
			EditedPost = 12,
			EditedTag = 13,
			EditedTagVotedDown = 14,
			EditedTagVotedUp = 15,
			FirstLoggedInView = 16,
			ProvidedAcceptedAnswer = 18,
			ProvidedAnswer = 19,
			ProvidedAnswerVotedDown = 20,
			ProvidedAnswerVotedUp = 21,
			ProvidedFlaggedAnswer = 22,
			VotedDownAnswer = 23,
			VotedDownQuestion = 24,
			VotedSynonymDown = 25,
			VotedSynonymUp = 26,				
			VotedTagDown = 27,
			VotedTagUp = 28,
			VotedUpAnswer = 29,
			VotedUpQuestion = 30,
			AcceptedQuestionAnswer = 31,
			OfferedBounty = 32

			// CreatedAcceptedTagSynonym, SociallySharedQuestion
		}

		/// <summary>
		/// Used to control how the voting control operates. 
		/// </summary>
		public enum VoteMode
		{
			Question = 0,
			Answer = 1,
			Term = 2,
			Synonym = 3
		}

		/// <summary>
		/// These are the vote types stored per each vote. We use an enum here to avoid the need for a lookup table. 
		/// </summary>
		/// <remarks>These values should never be changed!</remarks>
		public enum VoteType
		{
			VoteSynonymDown = -2,
			VoteDownPost = -1,
			FlagPost = 0,
			VoteUpPost = 1,
			VoteSynonymUp = 2,
			CloseQuestion = 10
		}

		#endregion
	
	}
}