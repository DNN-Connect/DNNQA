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

namespace DotNetNuke.DNNQA.Components.Entities
{

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Every record must be associated with at least one of the following: PostId, TermId, VoteId (should also set TermId or PostId when this is used).</remarks>
    public class ModerationLogInfo
    {

        public int ModLogId { get; set; }
        /// <summary>
        /// This is based on an enumerator contained in the module's Constants class.
        /// </summary>
        public int ModLogTypeId { get; set; }
        public int PortalId { get; set; }
        public int PostId { get; set; }
        public int TermId { get; set; }
        public int VoteId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>2000 char.</remarks>
        public string Notes { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedOnDate { get; set; }

    }
}