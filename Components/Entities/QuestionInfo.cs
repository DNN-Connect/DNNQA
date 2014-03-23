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
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Controllers;

namespace DotNetNuke.DNNQA.Components.Entities {

	public class QuestionInfo : PostInfo {

		public int UpVotes { get; set; }

		public int DownVotes { get; set; }

		public int TotalAnswers { get; set; }

		/// <summary>
		/// This is the last active user in a question thread.
		/// </summary>
		public int LastApprovedUserId { get; set; }

		/// <summary>
		/// This is the last active date in a question thread.
		/// </summary>
		public DateTime LastApprovedDate { get; set; }

		//Read Only Props
		public string CreatedByDisplayName {
			get {
				return CreatedByUserID > 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, CreatedByUserID).DisplayName : "Anonymous";
			}
		}

		public string LastApprovedDisplayName
		{
			get
			{
				return LastApprovedUserId > 0 ? DotNetNuke.Entities.Users.UserController.GetUserById(PortalId, LastApprovedUserId).DisplayName : "Anonymous";
			}
		}

		public int QuestionVotes {
			get { return (UpVotes - DownVotes); }
			}

		public List<TermInfo> QaTerms(int vocabularyId){
				var controller = new DnnqaController();
				return controller.GetTermsByContentItem(ContentItemId, vocabularyId);
		}

		#region IHydratable Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public override void Fill(System.Data.IDataReader dr) {
			//Call the base classes fill method to populate base class proeprties
			base.Fill(dr);
			base.FillInternal(dr);

			UpVotes = Null.SetNullInteger(dr["UpVotes"]);
			DownVotes = Null.SetNullInteger(dr["DownVotes"]);
			TotalAnswers = Null.SetNullInteger(dr["TotalAnswers"]);   
			LastApprovedUserId = Null.SetNullInteger(dr["LastApprovedUserId"]);
			LastApprovedDate = Null.SetNullDateTime(dr["LastApprovedDate"]);
		}

		#endregion

	}

}