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

namespace DotNetNuke.DNNQA.Components.Common
{

	/// <summary>
	/// This is for the answer data list shown in Answers.ascx.
	/// </summary>
	/// <typeparam name="TBodyLiteral"></typeparam>
	/// <typeparam name="TPostInfo"></typeparam>
	/// <typeparam name="TDateLiteral"></typeparam>
	/// <typeparam name="TVoting"></typeparam>
	/// <typeparam name="TAcceptedImage"></typeparam>
	/// <typeparam name="TAnswersMenu"></typeparam>
	/// <typeparam name="TAnswerEditLiteral"></typeparam>
	/// <typeparam name="TAnswerFlagLiteral"></typeparam>
	/// <typeparam name="TAnswerAcceptButton"></typeparam>
	/// <typeparam name="TAnswerDeleteLiteral"></typeparam>
	/// <typeparam name="TUserImage"></typeparam>
	/// <typeparam name="TAnswerComment"></typeparam>
	/// <typeparam name="TEditedLiteral"></typeparam>
	/// <typeparam name="TEditUserImage"></typeparam>
	public class AnswersEventArgs<TBodyLiteral, TPostInfo, TDateLiteral, TVoting, TAcceptedImage, TAnswersMenu, TAnswerEditLiteral, TAnswerFlagLiteral, TAnswerAcceptButton, TAnswerDeleteLiteral, TUserImage, TAnswerComment, TEditedLiteral, TEditUserImage> : EventArgs
	{
		public AnswersEventArgs(TBodyLiteral bodyLiteral, TPostInfo item, TDateLiteral dateLiteral, TVoting voting, TAcceptedImage acceptedImage, TAnswersMenu answersMenu, TAnswerEditLiteral answerEditLiteral, TAnswerFlagLiteral answerFlagLiteral, TAnswerAcceptButton answerAcceptButton, TAnswerDeleteLiteral answerDelete, TUserImage userImage, TAnswerComment answerComment, TEditedLiteral editedLiteral, TEditUserImage editUserImage)
		{
			BodyLiteral = bodyLiteral;
			ObjPost = item;
			DateLiteral = dateLiteral;
			Voting = voting;
			AcceptedImage = acceptedImage;
			AnswersMenu = answersMenu;
			AnswerEditLiteral = answerEditLiteral;
			AnswerFlagLiteral = answerFlagLiteral;
			AnswerAcceptButton = answerAcceptButton;
			AnswerDelete = answerDelete;
			UserImage = userImage;
			EditedLiteral = editedLiteral;
			EditUserImage = editUserImage;
			AnswerComment = answerComment;
		}

		public TBodyLiteral BodyLiteral { get; set; }
		public TPostInfo ObjPost { get; set; }
		public TDateLiteral DateLiteral { get; set; }
		public TVoting Voting { get; set; }
		public TAcceptedImage AcceptedImage { get; set; }
		public TAnswersMenu AnswersMenu { get; set; }
		public TAnswerEditLiteral AnswerEditLiteral { get; set; }
		public TAnswerFlagLiteral AnswerFlagLiteral { get; set; }
		public TAnswerAcceptButton AnswerAcceptButton { get; set; }
		public TAnswerDeleteLiteral AnswerDelete { get; set; }
		public TUserImage UserImage { get; set; }
		public TEditedLiteral EditedLiteral { get; set; }
		public TEditUserImage EditUserImage { get; set; }
		public TAnswerComment AnswerComment { get; set; }

	}

	public class BadgesListEventArgs<TBadge, TAwardedLiteral, TBadgeLiteral, TDescriptionLiteral, TMultiplierLiteral> : EventArgs
	{

		public BadgesListEventArgs(TBadge badge, TAwardedLiteral awardedLiteral, TBadgeLiteral badgeLiteral, TDescriptionLiteral descriptionLiteral, TMultiplierLiteral multiplierLiteral)
		{
			Badge = badge;
			AwardedLiteral = awardedLiteral;
			BadgeLiteral = badgeLiteral;
			DescriptionLiteral = descriptionLiteral;
			MultiplierLiteral = multiplierLiteral;
		}

		public TBadge Badge { get; set; }
		public TAwardedLiteral AwardedLiteral { get; set; }
		public TBadgeLiteral BadgeLiteral { get; set; }
		public TDescriptionLiteral DescriptionLiteral { get; set; }
		public TMultiplierLiteral MultiplierLiteral { get; set; }

	}

	public class BadgeManagerListEventArgs<TBadge, TEditLiteral, TBadgeLiteral, TDescriptionLiteral, TMultiplierLiteral> : EventArgs
	{

		public BadgeManagerListEventArgs(TBadge badge, TEditLiteral editLiteral, TBadgeLiteral badgeLiteral, TDescriptionLiteral descriptionLiteral, TMultiplierLiteral multiplierLiteral)
		{
			Badge = badge;
			EditLiteral = editLiteral;
			BadgeLiteral = badgeLiteral;
			DescriptionLiteral = descriptionLiteral;
			MultiplierLiteral = multiplierLiteral;
		}

		public TBadge Badge { get; set; }
		public TEditLiteral EditLiteral { get; set; }
		public TBadgeLiteral BadgeLiteral { get; set; }
		public TDescriptionLiteral DescriptionLiteral { get; set; }
		public TMultiplierLiteral MultiplierLiteral { get; set; }

	}

	/// <summary>
	/// This is for the question data list shown in Questions.ascx.
	/// </summary>
	/// <typeparam name="THyperLink"></typeparam>
	/// <typeparam name="TQuestionInfo"></typeparam>
	/// <typeparam name="TDateLiteral"></typeparam>
	/// <typeparam name="TViewsLiteral"></typeparam>
	/// <typeparam name="TAnswersLiteral"></typeparam>
	/// <typeparam name="TVotesLiteral"></typeparam>
	/// <typeparam name="TTags"></typeparam>
	/// <typeparam name="TAnswersTextLiteral"></typeparam>
	/// <typeparam name="TAnswersPanel"></typeparam>
	/// <typeparam name="TAcceptedImage"></typeparam>
	public class HomeQuestionsEventArgs<THyperLink, TQuestionInfo, TDateLiteral, TViewsLiteral, TAnswersLiteral, TVotesLiteral, TAnswersTextLiteral, TAnswersPanel, TTags, TAcceptedImage> : EventArgs
	{

		public HomeQuestionsEventArgs(THyperLink link, TQuestionInfo item, TDateLiteral dateLiteral, TViewsLiteral viewsLiteral, TAnswersLiteral answersLiteral, TVotesLiteral votesLiteral, TAnswersTextLiteral answersTextLiteral, TAnswersPanel answersPanel, TTags tagControl, TAcceptedImage acceptedImage)
		{
			TitleLink = link;
			ObjQuestion = item;
			DateLiteral = dateLiteral;
			ViewsLiteral = viewsLiteral;
			AnswersLiteral = answersLiteral;	
			VotesLiteral = votesLiteral;
			AnswersTextLiteral = answersTextLiteral;
			AnswersPanel = answersPanel;
			Tags = tagControl;
			AcceptedImage = acceptedImage;
		}

		public THyperLink TitleLink { get; set; }
		public TQuestionInfo ObjQuestion { get; set; }
		public TDateLiteral DateLiteral { get; set; }
		public TViewsLiteral ViewsLiteral { get; set; }
		public TAnswersLiteral AnswersLiteral { get; set; }		
		public TVotesLiteral VotesLiteral { get; set; }
		public TAnswersTextLiteral AnswersTextLiteral { get; set; }
		public TAnswersPanel AnswersPanel { get; set; }
		public TTags Tags { get; set; }
		public TAcceptedImage AcceptedImage { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TTerm"></typeparam>
	/// <typeparam name="TTagControl"></typeparam>
	public class HomeTagsEventArgs<TTerm, TTagControl> : EventArgs
	{

		public HomeTagsEventArgs(TTerm term, TTagControl tagControl)
		{
			Term = term;
			TagControl = tagControl;
		}

		public TTerm Term { get; set; }
		public TTagControl TagControl { get; set; }

	}

	public class HomeUserEventArgs<TDashHeader, TDashList, TFavoriteTagsHead, TFavoriteTagsList, TQuestionLiteral, TAnswerLiteral, TSubscriptionLiteral, TScoreLiteral> : EventArgs
	{

		public HomeUserEventArgs(TDashHeader dashHeader, TDashList dashList, TFavoriteTagsHead favoriteTagsHead, TFavoriteTagsList favoriteTagsList, TQuestionLiteral questionLiteral, TAnswerLiteral answerLiteral, TSubscriptionLiteral subscriptionLiteral, TScoreLiteral scoreLiteral)
		{
			DashHeader = dashHeader;
			DashList = dashList;
			FavoriteTagsHead = favoriteTagsHead;
			FavoriteTagsList = favoriteTagsList;
			QuestionLiteral = questionLiteral;
			AnswerLiteral = answerLiteral;
			SubscriptionLiteral = subscriptionLiteral;
			ScoreLiteral = scoreLiteral;
		}

		public TDashHeader DashHeader { get; set; }
		public TDashList DashList { get; set; }
		public TFavoriteTagsHead FavoriteTagsHead { get; set; }
		public TFavoriteTagsList FavoriteTagsList { get; set; }
		public TQuestionLiteral QuestionLiteral { get; set; }
		public TAnswerLiteral AnswerLiteral { get; set; }
		public TSubscriptionLiteral SubscriptionLiteral { get; set; }
		public TScoreLiteral ScoreLiteral { get; set; }
	}

	/// <summary>
	/// This is for the question data list shown in SearchResults.ascx.
	/// </summary>
	/// <typeparam name="THyperLink"></typeparam>
	/// <typeparam name="TQuestionInfo"></typeparam>
	/// <typeparam name="TDateLiteral"></typeparam>
	/// <typeparam name="TViewsLiteral"></typeparam>
	/// <typeparam name="TAnswersLiteral"></typeparam>
	public class BrowseQuestionsEventArgs<THyperLink, TQuestionInfo, TDateLiteral, TViewsLiteral, TAnswersLiteral> : EventArgs {

		public BrowseQuestionsEventArgs(THyperLink link, TQuestionInfo item, TDateLiteral dateLiteral, TViewsLiteral viewsLiteral, TAnswersLiteral answersLiteral) {
			ViewsLiteral = viewsLiteral;
			AnswersLiteral = answersLiteral;
			TitleLink = link;
			ObjPost = item;
			DateLiteral = dateLiteral;
		}

		public TViewsLiteral ViewsLiteral { get; set; }
		public TAnswersLiteral AnswersLiteral { get; set; }
		public THyperLink TitleLink { get; set; }
		public TQuestionInfo ObjPost { get; set; }
		public TDateLiteral DateLiteral { get; set; }

	}



	public class ProfileFriendsEventArgs<TUserScoreInfo, TUserHyperLink, TUserBinaryImage, TDetailsLiteral> : EventArgs
	{

		public ProfileFriendsEventArgs(TUserScoreInfo userScoreInfo, TUserHyperLink userHyperLink, TUserBinaryImage userBinaryImage, TDetailsLiteral detailsLiteral)
		{
			UserScoreInfo = userScoreInfo;
			UserHyperLink = userHyperLink;
			UserBinaryImage = userBinaryImage;
			DetailsLiteral = detailsLiteral;
		}

		public TUserScoreInfo UserScoreInfo { get; set; }
		public TUserHyperLink UserHyperLink { get; set; }
		public TUserBinaryImage UserBinaryImage { get; set; }
		public TDetailsLiteral DetailsLiteral { get; set; }

	}

	public class ProfileQuestionsEventArgs<TQuestionInfo, TAnswerCountLiteral, TQuestionTitleLink> : EventArgs
	{

		public ProfileQuestionsEventArgs(TQuestionInfo questionInfo, TAnswerCountLiteral answerCountLiteral, TQuestionTitleLink questionTitleLink)
		{
			QuestionInfo = questionInfo;
			AnswerCountLiteral = answerCountLiteral;
			QuestionTitleLink = questionTitleLink;
		}

		public TQuestionInfo QuestionInfo { get; set; }
		public TAnswerCountLiteral AnswerCountLiteral { get; set; }
		public TQuestionTitleLink QuestionTitleLink { get; set; }

	}

	public class ProfileReputationEventArgs<TUserScoreLogInfo, TAnswerCountLiteral, TQuestionTitleLink> : EventArgs
	{

			public ProfileReputationEventArgs(TUserScoreLogInfo userScoreLogInfo, TAnswerCountLiteral answerCountLiteral, TQuestionTitleLink questionTitleLink)
		{
			UserScoreLogInfo = userScoreLogInfo;
			AnswerCountLiteral = answerCountLiteral;
			QuestionTitleLink = questionTitleLink;
		}

			public TUserScoreLogInfo UserScoreLogInfo { get; set; }
		public TAnswerCountLiteral AnswerCountLiteral { get; set; }
		public TQuestionTitleLink QuestionTitleLink { get; set; }

	}
	

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TDescriptionLiteral"></typeparam>
	/// <typeparam name="TCountLiteral"></typeparam>
	/// <typeparam name="TTermInfo"></typeparam>
	/// <typeparam name="TTags"></typeparam>
	/// <typeparam name="TStatsLiteral"></typeparam>
	public class TagListEventArgs<TDescriptionLiteral, TCountLiteral, TTermInfo, TTags, TStatsLiteral> : EventArgs
	{

		public TagListEventArgs(TDescriptionLiteral litDescription, TCountLiteral litCount, TTermInfo term, TTags tagControl, TStatsLiteral statsLiteral)
		{
			DescriptionLiteral = litDescription;
			CountLiteral = litCount;
			Term = term;
			Tags = tagControl;
			StatsLiteral = statsLiteral;
		}

		public TDescriptionLiteral DescriptionLiteral { get; set; }
		public TCountLiteral CountLiteral { get; set; }
		public TTermInfo Term { get; set; }
		public TTags Tags { get; set; }
		public TStatsLiteral StatsLiteral { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TTerm"></typeparam>
	/// <typeparam name="TTermHistory"></typeparam>
	/// <typeparam name="THeaderLiteral"></typeparam>
	/// <typeparam name="TDescriptionLiteral"></typeparam>
	/// <typeparam name="TUpdatedLiteral"></typeparam>
	/// <typeparam name="TUserImage"></typeparam>
	public class TagHistoryListEventArgs<TTerm, TTermHistory, THeaderLiteral, TDescriptionLiteral, TUpdatedLiteral, TUserImage> : EventArgs
	{

		public TagHistoryListEventArgs(TTerm selectedTerm, TTermHistory termHistory, THeaderLiteral headerLiteral, TDescriptionLiteral descriptionLiteral, TUpdatedLiteral updatedLiteral, TUserImage userImage)
		{
			SelectedTerm = selectedTerm;
			TermHistory = termHistory;
			HeaderLiteral = headerLiteral;
			DescriptionLiteral = descriptionLiteral;
			UpdatedLiteral = updatedLiteral;
			UserImage = userImage;
		}

		public TTerm SelectedTerm { get; set; }
		public TTermHistory TermHistory { get; set; }
		public THeaderLiteral HeaderLiteral { get; set; }
		public TDescriptionLiteral DescriptionLiteral { get; set; }
		public TUpdatedLiteral UpdatedLiteral { get; set; }
		public TUserImage UserImage { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TTier"></typeparam>
	/// <typeparam name="TBadgeLiteral"></typeparam>
	/// <typeparam name="TDescriptionLiteral"></typeparam>
	public class TierListEventArgs<TTier, TBadgeLiteral, TDescriptionLiteral> : EventArgs
	{

		public TierListEventArgs(TTier tier, TBadgeLiteral badgeLiteral, TDescriptionLiteral descriptionLiteral)
		{
			Tier = tier;
			BadgeLiteral = badgeLiteral;
			DescriptionLiteral = descriptionLiteral;
		}

		public TTier Tier { get; set; }
		public TBadgeLiteral BadgeLiteral { get; set; }
		public TDescriptionLiteral DescriptionLiteral { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TPost"></typeparam>
	/// <typeparam name="TPostHistory"></typeparam>
	/// <typeparam name="THeaderLiteral"></typeparam>
	/// <typeparam name="TDescriptionLiteral"></typeparam>
	public class PostHistoryListEventArgs<TPost, TPostHistory, THeaderLiteral, TDescriptionLiteral> : EventArgs
	{

		public PostHistoryListEventArgs(TPost selectedPost, TPostHistory postHistory, THeaderLiteral headerLiteral, TDescriptionLiteral descriptionLiteral)
		{
			SelectedPost = selectedPost;
			PostHistory = postHistory;
			HeaderLiteral = headerLiteral;
			DescriptionLiteral = descriptionLiteral;
		}

		public TPost SelectedPost { get; set; }
		public TPostHistory PostHistory { get; set; }
		public THeaderLiteral HeaderLiteral { get; set; }
		public TDescriptionLiteral DescriptionLiteral { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TPrivilege"></typeparam>
	/// <typeparam name="TCurrentUserScore"></typeparam>
	/// <typeparam name="TPrivHyperLink"></typeparam>
	/// <typeparam name="TPercentCompleteLiteral"></typeparam>
	public class PrivilegeListEventArgs<TPrivilege, TCurrentUserScore, TPrivHyperLink, TPercentCompleteLiteral> : EventArgs
	{

		public PrivilegeListEventArgs(TPrivilege privilege, TCurrentUserScore currentUserScore, TPrivHyperLink privHyperLink, TPercentCompleteLiteral percentCompleteLiteral)
		{
			Privilege = privilege;
			CurrentUserScore = currentUserScore;
			PrivHyperLink = privHyperLink;
			PercentCompleteLiteral = percentCompleteLiteral;
		}

		public TPrivilege Privilege { get; set; }
		public TCurrentUserScore CurrentUserScore { get; set; }
		public TPrivHyperLink PrivHyperLink { get; set; }
		public TPercentCompleteLiteral PercentCompleteLiteral { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TVoting"></typeparam>
	/// <typeparam name="TTags"></typeparam>
	/// <typeparam name="TTermSynonym"></typeparam>
	/// <typeparam name="TImageButton"></typeparam>
	public class TermSynonymListEventArgs<TVoting, TTags, TTermSynonym, TImageButton> : EventArgs
	{

		public TermSynonymListEventArgs(TVoting voting, TTags tags, TTermSynonym termSynonym, TImageButton imageButton)
		{
			Voting = voting;
			Tags = tags;
			TermSynonym = termSynonym;
			ImageButton = imageButton;
		}

		public TVoting Voting { get; set; }
		public TTags Tags { get; set; }
		public TTermSynonym TermSynonym { get; set; }
		public TImageButton ImageButton { get; set; }

	}

}