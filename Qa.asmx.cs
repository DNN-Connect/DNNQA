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
using System.Web.Services;
using System.Web.Script.Services;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Presenters;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using System.Linq;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.DNNQA
{

    /// <summary>
    /// A series of services to be used with the QA module, primarily for ajax integration in the module itself. 
    /// </summary>
    [WebService(Namespace = "http://www.dotnetnuke.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    [PresenterBinding(typeof(QaServicePresenter), ViewType = typeof(IQaServiceView))]
    public class Qa : WebServiceView, IQaServiceView
    {

        public event EventHandler<SearchQuestionTitleEventArgs> ListQuestionTitleCalled;

        protected void OnSearchQuestionTitleCalled(SearchQuestionTitleEventArgs args)
        {
            if (ListQuestionTitleCalled != null)
            {
                ListQuestionTitleCalled(this, args);
            }
        }

        /// <summary>
        /// This method is used to search existing question titles and return the matching results, if any exist. 
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="searchPhrase"></param>
        /// <returns>A collection of questions that have a similar title (and module) based on the searchPhrase passed in.</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<QuestionServiceInfo> SearchQuestionTitle(int moduleId, string searchPhrase)
        {
            var args = new SearchQuestionTitleEventArgs(moduleId, searchPhrase);
            OnSearchQuestionTitleCalled(args);
            return args.Result;
        }

        /// <summary>
        /// This method is used to get the last QuestionPostId in order to display divMessage box alerting user a new answer has been submitted.
        /// </summary>
        /// <param name="postId"></param>
        /// <returns>Returns the last postId in the answers collection.</returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int GetLastQuestionPostId(int postId)
        {
            var dnnqa = new Components.Controllers.DnnqaController();
            var ps = Common.Globals.GetPortalSettings();

            var answers = dnnqa.GetAnswers(postId, ps.PortalId);
            if (answers.Count > 0)
            {
                return (from p in answers
                        orderby p.CreatedDate descending
                        select p).FirstOrDefault().PostId;
            }
            return 0;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SearchTags(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) { return ""; }

            var terms = Util.GetTermController().GetTermsByVocabulary(1)
                .Where(t => t.Name.ToLower().Contains(searchTerm.ToLower()))
                .Where(t => t.Name.IndexOfAny(Constants.DisallowedCharacters.ToCharArray()) == -1)
                .Select(term => term.Name);
            return terms.ToJson();
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<BadgeInfo> GetPortalBadges(int portalId)
        //{
        //    var dnnqa = new Components.Controllers.DnnqaController();
        //    var colBadges = dnnqa.GetPortalBadges(portalId);
        //    var portalSettings = new PortalSettings(portalId);

        //    // we can created a new property in BadgeInfo (not stored as a column in data store)
        //    foreach (var objBadge in colBadges)
        //    {
        //        objBadge.LocalizedName = Localization.GetString(objBadge.NameLocalizedKey, Constants.SharedResourceFileName, portalSettings, portalSettings.DefaultLanguage);
        //        objBadge.LocalizedDesc = Localization.GetString(objBadge.DescriptionLocalizedKey, Constants.SharedResourceFileName, portalSettings, portalSettings.DefaultLanguage);
        //    }

        //    //NOTE: Consider caching result based on language? (not sure its worth the effort here, only ever utilized in badge manager view)
        //    return colBadges;
        //}

    }
}
