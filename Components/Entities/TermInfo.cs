﻿//
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

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content.Taxonomy;

namespace DotNetNuke.DNNQA.Components.Entities {

    /// <summary>
    /// This is our Info class that represents columns in our data store that are associated with the vw_DNNQA_Terms view (This extends the core API).
    /// </summary>
    /// <remarks>This could go into the core if everything (and the view) were based on Content Type.</remarks>
    public class TermInfo : Term
    {

        #region Public Properties

        /// <summary>
        /// The total number of times the term was used. 
        /// </summary>
        /// <remarks>This is a count generated in the stored procedure/view.</remarks>
        public int TotalTermUsage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int MonthTermUsage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WeekTermUsage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DayTermUsage { get; set; }

        #region IHydratable Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dr"></param>
        public override void Fill(System.Data.IDataReader dr)
        {
            //Call the base classes fill method to populate base class proeprties
            base.Fill(dr);
            base.FillInternal(dr);

            //TermId = Null.SetNullInteger(dr["TermId"]);
            //Name = Null.SetNullString(dr["Name"]);
            //Description = Null.SetNullString(dr["Description"]);

            TotalTermUsage = Null.SetNullInteger(dr["TotalTermUsage"]);
            MonthTermUsage = Null.SetNullInteger(dr["MonthTermUsage"]);
            WeekTermUsage = Null.SetNullInteger(dr["WeekTermUsage"]);
            DayTermUsage = Null.SetNullInteger(dr["DayTermUsage"]);
        }

        #endregion

        #endregion

    }
}