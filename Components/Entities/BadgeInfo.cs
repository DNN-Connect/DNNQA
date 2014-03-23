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

using System.Linq;
using DotNetNuke.DNNQA.Components.Common;

namespace DotNetNuke.DNNQA.Components.Entities {

    /// <summary>
    /// This is our Info class that represents columns in our data store that are associated with the DNNQA_Badge table.
    /// </summary>
    public class BadgeInfo 
    {

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>PK</remarks>
        public int BadgeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>100 nvarchar</remarks>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>FK</remarks>
        public int PortalId { get; set; }

        public int TierId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>255 nvarchar</remarks>
        public string Icon { get; set; }

        public int RepPoints { get; set; }

        public bool Active { get; set; }

        /// <summary>
        /// The number of times the badge has been awarded.
        /// </summary>
        /// <remarks> Never subtracted from, even if users are deleted</remarks>
        public int Awarded { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Should match a user scoring action enumerator (if being used).</remarks>
        public int TriggerActionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>100 nvarchar</remarks>
        public string TriggerActions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>100 nvarchar</remarks>
        public string TriggerSproc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>nvarchar(MAX)</remarks>
        public string TriggerSql { get; set; }

        /// <summary>
        /// The number of times the action must occur.
        /// </summary>
        public int TriggerCount { get; set; }

        public int TriggerTimeCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>1 nvarchar</remarks>
        public string TriggerTimeUnit { get; set; }

        #region ReadOnly

        /// <summary>
        /// This is used to return a localized badge name from the shared resources file.
        /// </summary>
        public string NameLocalizedKey
        {
            get { return "Badge_" + Key; }
        }

        /// <summary>
        /// This is used to return a localized badge description from the shared resources file.
        /// </summary>
        public string DescriptionLocalizedKey
        {
            get { return "Badge_" + Key + "_Desc"; }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public BadgeTierInfo TierDetails
        {
            get
            {
                var colTiers = Utils.GetBadgeTiers();
                var objTier = colTiers.Single(s => s.Id == TierId);

                return objTier;
            }
        }

        public string TierLocalizedName
        {
            get { return TierDetails.Name; }
        }


        // experimental (not meant to be stored as columns in datastore)
        public string LocalizedName { get; set; }

        public string LocalizedDesc { get; set; }
        
        #endregion

    }
}