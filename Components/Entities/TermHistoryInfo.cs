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

namespace DotNetNuke.DNNQA.Components.Entities
{

    /// <summary>
    /// This is our Info class that represents columns in our data store that are associated with _Term_History table (This extends the core API for versioning).
    /// </summary>
    /// <remarks>Please notice that term history functions somewhat differently than post history, simply because terms are handled outside of this module (and @ the application level).</remarks>
    public class TermHistoryInfo
    {

        public int TermId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Portals are integrated here because they are not at the taxonomy level (for tags, that is, there vocabulary scope is application wide) and we display user intormation (which is always portal scope).</remarks>
        public int PortalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>250 char limit</remarks>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>2500 char limit</remarks>
        public string Description { get; set; }

        /// <summary>
        /// Notes are a summary explaining why a term description was edited. Thus, notes are stored with the 'backup' description & name in a single row in the history table. Because a term can be edited outside of this module (or portal), this seems the most appropriate way of handling it. 
        /// </summary>
        /// <remarks>1000 char limit</remarks>
        public string Notes { get; set; }

        public int Revision { get; set; }

        public int RevisedByUserId { get; set; }

        public DateTime RevisedOnDate { get; set; }

    }
}