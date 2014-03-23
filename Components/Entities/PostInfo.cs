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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace DotNetNuke.DNNQA.Components.Entities {

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the DNNQA_Post table.
	/// </summary>
	public class PostInfo : ContentItem {

		#region Public Properties

		/// <summary>
		/// The primary key identifier. All Questions/Answers are posts. 
		/// </summary>
		public int PostId { get; set; }

		/// <summary>
		/// This is the equivalent of a subject. 
		/// </summary>
		/// <remarks>150 chars.</remarks>
		public string Title { get; set; }

		/// <summary>
		/// The question or answer itself. 
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// The number of score points the question asker is willing to part with for an accepted answer. 
		/// </summary>
		/// <remarks>Not available in UI phase I.</remarks>
		public int Bounty { get; set; }

		/// <summary>
		/// ParentID of 0 signifies the item is a question (ie. first post). 
		/// </summary>
		public int ParentId { get; set; }

		/// <summary>
		/// Main queries are based on moduleID (this is done via conten item relation).
		/// </summary>
		public int PortalId { get; set; }

		/// <summary>
		/// Each new post starts at 0, this tells us how many times each post has been viewed since being added and approved.
		/// </summary>
		public int ViewCount { get; set; }

		/// <summary>
		/// Score is caluculated per post, based on VoteType's of VoteUp/VoteDown. Updated each time a vote is added by a user. 
		/// </summary>
		/// <remarks>This is meant to be updated alone, via voting (not as part of entire postinfo object)</remarks>
		public int Score { get; set; }

		/// <summary>
		/// If the post is approved for public viewing.
		/// </summary>
		public bool Approved { get; set; }

		/// <summary>
		/// The date the post was approved for public viewing. 
		/// </summary>
		public DateTime ApprovedDate { get; set; }

		/// <summary>
		/// True if a soft delete was performed on the post record, false otherwise. 
		/// </summary>
		public bool Deleted { get; set; }

		/// <summary>
		/// The post set as an answer for a question thread.
		/// </summary>
		public int AnswerId { get; set; }

		/// <summary>
		/// The date a question was set as answered.
		/// </summary>
		public DateTime AnswerDate { get; set; }

		/// <summary>
		/// Determines if a question is closed from further answers.
		/// </summary>
		/// <remarks>Not sure if voting should be turned off or not too.</remarks>
		public bool Closed { get; set; }

		/// <summary>
		/// The date the question was closed.
		/// </summary>
		public DateTime ClosedDate { get; set; }

		/// <summary>
		/// Determines if a question is protected from future edits.
		/// </summary>
		public bool Protected { get; set; }

		/// <summary>
		/// The date the question was protected.
		/// </summary>
		public DateTime ProtectedDate { get; set; }

		/// <summary>
		/// The user who created the post. For anonymous support, this would be -1.
		/// </summary>
		/// <remarks>Initial go of module does not support anonymous posting (or voting).</remarks>
		public int CreatedUserId { get; set; }

		/// <summary>
		/// The date the post was created.
		/// </summary>
		public DateTime CreatedDate { get; set; }

		/// <summary>
		/// The user who last modified the post.
		/// </summary>
		public int LastModifiedUserId { get; set; }

		/// <summary>
		/// The date the post was last modified. 
		/// </summary>
		public DateTime LastModifiedDate { get; set; }

		// this is pulled in retrieval sprocs, not stored as an actual column
		public int TotalRecords { get; set; }

		//Read Only Props
		internal string PostCreatedDisplayName {
			get {
				return CreatedUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, CreatedUserId).DisplayName : Null.NullString;
			}
		}

		internal string PostLastModifiedDisplayName
		{
			get
			{
				return LastModifiedUserId != 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, LastModifiedUserId).DisplayName : Null.NullString;
			}
		}
		#endregion

		#region IHydratable Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public override void Fill(System.Data.IDataReader dr) {
			//Call the base classes fill method to populate base class proeprties
			base.FillInternal(dr);

			PostId = Null.SetNullInteger(dr["PostId"]);
			Title = Null.SetNullString(dr["Title"]);
			Body = Null.SetNullString(dr["Body"]);
			Bounty = Null.SetNullInteger(dr["Bounty"]);
			ParentId = Null.SetNullInteger(dr["ParentId"]);
			PortalId = Null.SetNullInteger(dr["PortalId"]);
			ViewCount = Null.SetNullInteger(dr["ViewCount"]);
			Score = Null.SetNullInteger(dr["Score"]);
			Approved = Null.SetNullBoolean(dr["Approved"]);
			ApprovedDate = Null.SetNullDateTime(dr["ApprovedDate"]);
			Deleted = Null.SetNullBoolean(dr["Deleted"]);
			AnswerId = Null.SetNullInteger(dr["AnswerId"]);
			AnswerDate = Null.SetNullDateTime(dr["AnswerDate"]);
			Closed = Null.SetNullBoolean(dr["Closed"]);
			ClosedDate = Null.SetNullDateTime(dr["ClosedDate"]);
			Protected = Null.SetNullBoolean(dr["Protected"]);
			ProtectedDate = Null.SetNullDateTime(dr["ProtectedDate"]);
			CreatedUserId = Null.SetNullInteger(dr["CreatedUserId"]);
			CreatedDate = Null.SetNullDateTime(dr["CreatedDate"]);
			LastModifiedUserId = Null.SetNullInteger(dr["LastModifiedUserId"]);
			LastModifiedDate = Null.SetNullDateTime(dr["LastModifiedDate"]);
			TotalRecords = Null.SetNullInteger(dr["TotalRecords"]);
		}

		/// <summary>
		/// Gets and sets the Key ID
		/// </summary>
		/// <returns>An Integer</returns>
		public override int KeyID {
			get { return PostId; }
			set { PostId = value; }
		}

		#endregion

	}
}