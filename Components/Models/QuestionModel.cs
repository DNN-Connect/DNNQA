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

using System.Collections.Generic;
using DotNetNuke.DNNQA.Components.Entities;

namespace DotNetNuke.DNNQA.Components.Models
{

	public class QuestionModel 
	{

		public QuestionInfo Question { get; set; }
		public List<PostInfo> ColAnswers { get; set; }
		public PostInfo NewPost { get; set; }
		public List<QaSettingInfo> Privileges { get; set; }
		public List<VoteInfo> QuestionVotes { get; set; }
		public string SortBy { get; set; }
		public string LoginUrl { get; set; }
		public int CurrentPage { get; set; }
		public int QuestionAuthorScore { get; set; }
		public int QuestionEditedUserScore { get; set; }
		public string PageLink { get; set; }
		public string PrevPageLink { get; set; }
		public string NextPageLink { get; set; }
		public string FacebookAppId { get; set; }
		public bool EnablePlusOne { get; set; }
		public bool EnableTwitter { get; set; }
		public bool EnableLinkedIn { get; set; }

	}
}