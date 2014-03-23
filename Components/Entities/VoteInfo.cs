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
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.DNNQA.Components.Entities {

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the DNNQA_Vote table. Keep in mind, voting can occur on posts and terms.
	/// </summary>
	public class VoteInfo:  IHydratable {

		#region Public Properties

		public int VoteId { get; set; }

		public int PostId { get; set; }

		public int TermId { get; set; }

		public int VoteTypeId { get; set; }

		public int PortalId { get; set; }

		public DateTime CreatedOnDate { get; set; }

		public int CreatedByUserId { get; set; }

		// this is pulled in retrieval sprocs, not stored as an actual column
		public int TotalRecords { get; set; }

		#endregion

		#region IHydratable Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public void Fill(System.Data.IDataReader dr) {
			VoteId = Null.SetNullInteger(dr["VoteId"]);
			PostId = Null.SetNullInteger(dr["PostId"]);
			TermId = Null.SetNullInteger(dr["TermId"]);
			VoteTypeId = Null.SetNullInteger(dr["VoteTypeId"]);
			PortalId = Null.SetNullInteger(dr["PortalId"]);
			CreatedOnDate = Null.SetNullDateTime(dr["CreatedOnDate"]);
			CreatedByUserId = Null.SetNullInteger(dr["CreatedByUserId"]);
			//TotalRecords = Null.SetNullInteger(dr["TotalRecords"]);
		}

		/// <summary>
		/// Gets and sets the Key ID
		/// </summary>
		/// <returns>An Integer</returns>
		public int KeyID
		{
			get { return VoteId; }
			set { VoteId = value; }
		}

		#endregion

	}
}