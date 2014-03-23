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

#pragma warning disable 1587
	/// This File contains all the classes we need for our custom events for UI's (views) 
#pragma warning restore 1587

	#region Public Methods 

	public class AcceptAnswerEventArgs<TPostId, TUserId, TParentId> : EventArgs
	{

		public AcceptAnswerEventArgs(TPostId postId, TUserId userId, TParentId parentId)
		{
			PostId = postId;
			UserId = userId;
			ParentId = parentId;
		}

		public TPostId PostId { get; set; }
		public TUserId UserId { get; set; }
		public TParentId ParentId { get; set; }

	}

	public class AddAnswerEventArgs<T> : EventArgs
	{

		public AddAnswerEventArgs(T newPost)
		{
			NewPost = newPost;
		}

		public T NewPost { get; set; }

	}

	public class AddTermSynonymEventArgs<TTermName> : EventArgs
	{

		public AddTermSynonymEventArgs(TTermName termName)
		{
			TermName = termName;
		}

		public TTermName TermName { get; set; }

	}

	public class AskQuestionEventArgs<TPost, TNotify, TTags> : EventArgs
	{

		public AskQuestionEventArgs(TPost post, TNotify notify, TTags tags)
		{
			Post = post;
			Notify = notify;
			Tags = tags;
		}

		public TPost Post { get; set; }
		public TNotify Notify { get; set; }
		public TTags Tags { get; set; }

	}

	public class DeletePostEventArgs<T> : EventArgs
	{

		public DeletePostEventArgs(T moderationLog)
		{
			ModerationLog = moderationLog;
		}

		public T ModerationLog { get; set; }

	}

	public class DeleteSuggestedSynonymEventArgs<TImageButton> : EventArgs
	{

		public DeleteSuggestedSynonymEventArgs(TImageButton imageButton)
		{
			ImageButton = imageButton;
		}

		public TImageButton ImageButton { get; set; }

	}

	public class EditPostEventArgs<TPost, TTags> : EventArgs
	{

		public EditPostEventArgs(TPost post, TTags tags)
		{
			Post = post;
			Tags = tags;
		}

		public TPost Post { get; set; }
		public TTags Tags { get; set; }

	}

	public class EditTermEventArgs<TTermHistory> : EventArgs
	{

		public EditTermEventArgs(TTermHistory termHistory)
		{
			TermHistory = termHistory;
		}

		public TTermHistory TermHistory { get; set; }

	}

	public class FlagPostEventArgs<T> : EventArgs
	{

		public FlagPostEventArgs(T moderationLog)
		{
			ModerationLog = moderationLog;
		}

		public T ModerationLog { get; set; }

	}

	public class PagerChangedEventArgs<TLinkButton, TFilter>: EventArgs
	{
		
		public PagerChangedEventArgs(TLinkButton linkButton, TFilter filter)
		{
			LinkButton = linkButton;
			Filter = filter;
		}

		public TLinkButton LinkButton { get; set; }
		public TFilter Filter { get; set; }

	}

	public class PrivilegeManagerSaveEventArgs<TViewAll, TCreatePost, TRemoveNewUser, TFlag, TVoteUp, TCommentEverywhere, TVoteDown, TRetagQuestion, TEditQuestionsAndAnswers, TCreateTagSynonym, TCloseQuestion, TApproveTagEdits, TModeratorTools, TProtectQuestions, TTrusted> : EventArgs
	{

		public PrivilegeManagerSaveEventArgs(TViewAll viewAll, TCreatePost createPost, TRemoveNewUser removeNewUser, TFlag flag, TVoteUp voteUp, TCommentEverywhere commentEverywhere, TVoteDown voteDown, TRetagQuestion retagQuestion, TEditQuestionsAndAnswers editQuestionsAndAnswers, TCreateTagSynonym createTagSynonym, TCloseQuestion closeQuestion, TApproveTagEdits approveTagEdits, TModeratorTools moderatorTools, TProtectQuestions protectQuestions, TTrusted trusted)
		{
			ViewAll = viewAll;
			CreatePost = createPost;
			RemoveNewUser = removeNewUser;
			Flag = flag;
			VoteUp = voteUp;
			CommentEverywhere = commentEverywhere;
			VoteDown = voteDown;
			RetagQuestion = retagQuestion;
			EditQuestionsAndAnswers = editQuestionsAndAnswers;
			CreateTagSynonym = createTagSynonym;
			CloseQuestion = closeQuestion;
			ApproveTagEdits = approveTagEdits;
			ModeratorTools = moderatorTools;
			ProtectQuestions = protectQuestions;
			Trusted = trusted;
		}

		public TViewAll ViewAll { get; set; }
		public TCreatePost CreatePost { get; set; }
		public TRemoveNewUser RemoveNewUser { get; set; }
		public TFlag Flag { get; set; }
		public TVoteUp VoteUp { get; set; }
		public TCommentEverywhere CommentEverywhere { get; set; }
		public TVoteDown VoteDown { get; set; }
		public TRetagQuestion RetagQuestion { get; set; }
		public TEditQuestionsAndAnswers EditQuestionsAndAnswers { get; set; }
		public TCreateTagSynonym CreateTagSynonym { get; set; }
		public TCloseQuestion CloseQuestion { get; set; }
		public TApproveTagEdits ApproveTagEdits { get; set; }
		public TModeratorTools ModeratorTools { get; set; }
		public TProtectQuestions ProtectQuestions { get; set; }
		public TTrusted Trusted { get; set; }

	}

	public class QuestionEventArgs<TTitleLiteral, TVotingControl, TBodyLiteral, TAuthorImage, TAskedLiteral, TEditorImage, TEditedLiteral, TTagsControl, TUnorderedList, TEditLiteral, TFlagLiteral, TCloseLiteral, TProtectLiteral, TDeleteLiteral, TCommentControl, TACountLiteral, TSocialSharingLiteral> : EventArgs
	{
	
		public QuestionEventArgs(TTitleLiteral titleLiteral, TVotingControl votingControl, TBodyLiteral bodyLiteral, TAuthorImage authorImage, TAskedLiteral askedLiteral, TEditorImage editorImage, TEditedLiteral editedLiteral, TTagsControl tagsControl, TUnorderedList unorderedList, TEditLiteral editLiteral, TFlagLiteral flagLiteral, TCloseLiteral closeLiteral, TProtectLiteral protectLiteral, TDeleteLiteral deleteLiteral, TCommentControl commentControl, TACountLiteral aCountLiteral, TSocialSharingLiteral socialSharingLiteral)
		{
			TitleLiteral = titleLiteral;
			VotingControl = votingControl;
			BodyLiteral = bodyLiteral;
			AuthorImage = authorImage;
			AskedLiteral = askedLiteral;
			EditorImage = editorImage;
			EditedLiteral = editedLiteral;
			TagsControl = tagsControl;
			UnorderedList = unorderedList;
			EditLiteral = editLiteral;
			FlagLiteral = flagLiteral;
			CloseLiteral = closeLiteral;
			ProtectLiteral = protectLiteral;
			DeleteLiteral = deleteLiteral;
			CommentControl = commentControl;
			ACountLiteral = aCountLiteral;
			SocialSharingLiteral = socialSharingLiteral;
		}

		public TTitleLiteral TitleLiteral { get; set; }
		public TVotingControl VotingControl { get; set; }
		public TBodyLiteral BodyLiteral { get; set; }
		public TAuthorImage AuthorImage { get; set; }
		public TAskedLiteral AskedLiteral { get; set; }
		public TEditorImage EditorImage { get; set; }
		public TEditedLiteral EditedLiteral { get; set; }
		public TTagsControl TagsControl { get; set; }
		public TUnorderedList UnorderedList { get; set; }
		public TEditLiteral EditLiteral { get; set; }
		public TFlagLiteral FlagLiteral { get; set; }
		public TCloseLiteral CloseLiteral { get; set; }
		public TProtectLiteral ProtectLiteral { get; set; }
		public TDeleteLiteral DeleteLiteral { get; set; }
		public TCommentControl CommentControl { get; set; }
		public TACountLiteral ACountLiteral { get; set; }
		public TSocialSharingLiteral SocialSharingLiteral { get; set; }

	}

	public class ScoringManagerSaveEventArgs<TApprovedPostEdit, TApprovedTagEdit, TAskedFlaggedQuestion, TAskedQuestion, TAskedQuestionVotedDown, TAskedQuestionVotedUp, TCreatedTagSynonym, TCommented, TEditedPost, TEditedTag, TEditedTagVotedDown, TEditedTagVotedUp, TFirstLoggedInView, TProvidedAnswer, TProvidedAcceptedAnswer, TProvidedAnswerVotedDown, TProvidedAnswerVotedUp, TProvidedFlaggedAnswer, TVotedDownAnswer, TVotedDownQuestion, TVotedSynonymDown, TVotedSynonymUp, TVotedTagDown, TVotedTagUp, TVotedUpAnswer, TVotedUpQuestion, TAcceptedQuestionAnswer> : EventArgs
	{

		public ScoringManagerSaveEventArgs(TApprovedPostEdit approvedPostEdit, TApprovedTagEdit approvedTagEdit, TAskedFlaggedQuestion askedFlaggedQuestion, TAskedQuestion askedQuestion, TAskedQuestionVotedDown askedQuestionVotedDown, TAskedQuestionVotedUp askedQuestionVotedUp, TCreatedTagSynonym createdTagSynonym, TCommented commented, TEditedPost editedPost, TEditedTag editedTag, TEditedTagVotedDown editedTagVotedDown, TEditedTagVotedUp editedTagVotedUp, TFirstLoggedInView firstLoggedInView, TProvidedAnswer providedAnswer, TProvidedAcceptedAnswer providedAcceptedAnswer, TProvidedAnswerVotedDown providedAnswerVotedDown, TProvidedAnswerVotedUp providedAnswerVotedUp, TProvidedFlaggedAnswer providedFlaggedAnswer, TVotedDownAnswer votedDownAnswer, TVotedDownQuestion votedDownQuestion, TVotedSynonymDown votedSynonymDown, TVotedSynonymUp votedSynonymUp, TVotedTagDown votedTagDown, TVotedTagUp votedTagUp, TVotedUpAnswer votedUpAnswer, TVotedUpQuestion votedUpQuestion, TAcceptedQuestionAnswer acceptedQuestionAnswer)
		{
			ApprovedPostEdit = approvedPostEdit;
			ApprovedTagEdit = approvedTagEdit;
			AskedFlaggedQuestion = askedFlaggedQuestion;
			AskedQuestion = askedQuestion;
			AskedQuestionVotedDown = askedQuestionVotedDown;
			AskedQuestionVotedUp = askedQuestionVotedUp;
			CreatedTagSynonym = createdTagSynonym;
			Commented = commented;
			EditedPost = editedPost;
			EditedTag = editedTag;
			EditedTagVotedDown = editedTagVotedDown;
			EditedTagVotedUp = editedTagVotedUp;
			FirstLoggedInView = firstLoggedInView;
			ProvidedAnswer = providedAnswer;
			ProvidedAcceptedAnswer = providedAcceptedAnswer;
			ProvidedAnswerVotedDown = providedAnswerVotedDown;
			ProvidedAnswerVotedUp = providedAnswerVotedUp;
			ProvidedFlaggedAnswer = providedFlaggedAnswer;
			VotedDownAnswer = votedDownAnswer;
			VotedDownQuestion = votedDownQuestion;
			VotedSynonymDown = votedSynonymDown;
			VotedSynonymUp = votedSynonymUp;
			VotedTagDown = votedTagDown;
			VotedTagUp = votedTagUp;
			VotedUpAnswer = votedUpAnswer;
			VotedUpQuestion = votedUpQuestion;
			AcceptedQuestionAnswer = acceptedQuestionAnswer;
		}

		public TApprovedPostEdit ApprovedPostEdit { get; set; }
		public TApprovedTagEdit ApprovedTagEdit { get; set; }
		public TAskedFlaggedQuestion AskedFlaggedQuestion { get; set; }
		public TAskedQuestion AskedQuestion { get; set; }
		public TAskedQuestionVotedDown AskedQuestionVotedDown { get; set; }
		public TAskedQuestionVotedUp AskedQuestionVotedUp { get; set; }
		public TCreatedTagSynonym CreatedTagSynonym { get; set; }
		public TCommented Commented { get; set; }
		public TEditedPost EditedPost { get; set; }
		public TEditedTag EditedTag { get; set; }
		public TEditedTagVotedDown EditedTagVotedDown { get; set; }
		public TEditedTagVotedUp EditedTagVotedUp { get; set; }
		public TFirstLoggedInView FirstLoggedInView { get; set; }
		public TProvidedAnswer ProvidedAnswer { get; set; }
		public TProvidedAcceptedAnswer ProvidedAcceptedAnswer { get; set; }
		public TProvidedAnswerVotedDown ProvidedAnswerVotedDown { get; set; }
		public TProvidedAnswerVotedUp ProvidedAnswerVotedUp { get; set; }
		public TProvidedFlaggedAnswer ProvidedFlaggedAnswer { get; set; }
		public TVotedDownAnswer VotedDownAnswer { get; set; }
		public TVotedDownQuestion VotedDownQuestion { get; set; }
		public TVotedSynonymDown VotedSynonymDown { get; set; }
		public TVotedSynonymUp VotedSynonymUp { get; set; }
		public TVotedTagDown VotedTagDown { get; set; }
		public TVotedTagUp VotedTagUp { get; set; }
		public TVotedUpAnswer VotedUpAnswer { get; set; }
		public TVotedUpQuestion VotedUpQuestion { get; set; }
		public TAcceptedQuestionAnswer AcceptedQuestionAnswer { get; set; }

	}

	/// <summary>
	/// Event handling for the search go button shown in HeaderNav.ascx.
	/// </summary>
	/// <typeparam name="TSearchString"></typeparam>
	public class SearchEventArgs<TSearchString> : EventArgs
	{

		public SearchEventArgs(TSearchString searchString)
		{
			SearchString = searchString;
		}

		public TSearchString SearchString { get; set; }

	}

	public class TagSearchEventArgs<TFilter> : EventArgs
	{

		public TagSearchEventArgs(TFilter filter)
		{
			Filter = filter;
		}

		public TFilter Filter { get; set; }

	}

	#endregion

}