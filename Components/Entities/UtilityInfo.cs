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
using DotNetNuke.DNNQA.Components.Common;

namespace DotNetNuke.DNNQA.Components.Entities
{

    public class BadgeTierInfo
    {

        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public string TitlePrefixKey { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class EmailSettingInfo
    {

        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string SettingId { get; set; }

    }

    /// <summary>
    /// Used while searching to construct an object necessary for updating the UI.
    /// </summary>
    public class FilterInfo
    {

        #region Public Properties

        public string FilterName { get; set; }

        public Constants.SearchFilterType FilterType { get; set; }

        public string RemoveFilterLink { get; set; }

        #endregion

    }

    /// <summary>
    /// This generic entity is used when working with custom module settings (not stored at module or tabmodule level). This entity is basically the SettingInfo entity populated with localized values for text (in addition to values stored in the database) and also default values are set (if necessary). This could have extended the SettingInfo class but it didn't seem worthwhile.
    /// </summary>
    public class QaSettingInfo
    {

        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string SettingId { get; set; }
    }

    /// <summary>
    /// Used for sorting data grids and list views. This allows us to remove dependency on the Telerik pagers (built in or independent) to present a better UI. 
    /// </summary>
    /// <remarks>Currently, this is only used in the search interface.</remarks>
    public class SortInfo
    {

        #region Public Properties

        public string Column { get; set; }

        public Constants.SortDirection Direction { get; set; }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public class SubscriberInfo
    {

        #region Public Properties

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public int PortalId { get; set; }

        #endregion

    }

}