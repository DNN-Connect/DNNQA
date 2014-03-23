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
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Entities.Tabs;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Installer;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Modules;
using System.Collections.Generic;
namespace DotNetNuke.DNNQA.Components.Common
{

    public class Utils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<BadgeTierInfo> GetBadgeTiers()
        {
            //TODO: Utilize caching
            var colTiers = new List<BadgeTierInfo>();

            var bronzeTier = new BadgeTierInfo
            {
                Id = (int) Constants.BadgeTiers.Bronze,
                Key = Constants.BadgeTiers.Bronze.ToString(),
                Name = @"Tier_" + Constants.BadgeTiers.Bronze,
                Description = @"Tier_" + Constants.BadgeTiers.Bronze + @"_Desc",
                IconClass = Constants.BadgeTiers.Bronze.ToString().ToLower(),
                TitlePrefixKey = "TierTitle_" + Constants.BadgeTiers.Bronze
            };
            colTiers.Add(bronzeTier);

            var silverTier = new BadgeTierInfo
            {
                Id = (int)Constants.BadgeTiers.Silver,
                Key = Constants.BadgeTiers.Silver.ToString(),
                Name = @"Tier_" + Constants.BadgeTiers.Silver,
                Description = @"Tier_" + Constants.BadgeTiers.Silver + @"_Desc",
                IconClass = Constants.BadgeTiers.Silver.ToString().ToLower(),
                TitlePrefixKey = "TierTitle_" + Constants.BadgeTiers.Silver
            };
            colTiers.Add(silverTier);

            var goldTier = new BadgeTierInfo
            {
                Id = (int)Constants.BadgeTiers.Gold,
                Key = Constants.BadgeTiers.Gold.ToString(),
                Name = @"Tier_" + Constants.BadgeTiers.Gold,
                Description = @"Tier_" + Constants.BadgeTiers.Gold + @"_Desc",
                IconClass = Constants.BadgeTiers.Gold.ToString().ToLower(),
                TitlePrefixKey = "TierTitle_" + Constants.BadgeTiers.Gold
            };
            colTiers.Add(goldTier);

            return colTiers;
        }

        /// <summary>
        /// Ensures registration of dnn.js, jQuery, jQuery UI, hoverIntent, and qaTooltip.
        /// </summary>
        /// <param name="dnnPage"></param>
        public static void RegisterClientDependencies(System.Web.UI.Page dnnPage)
        {
            ClientAPI.RegisterClientReference(dnnPage, ClientAPI.ClientNamespaceReferences.dnn);
            jQuery.RequestUIRegistration();
            ClientResourceManager.RegisterScript(dnnPage, "~/Resources/Shared/Scripts/jquery/jquery.hoverIntent.min.js");
            ClientResourceManager.RegisterScript(dnnPage, "~/DesktopModules/DNNQA/js/jquery.qatooltip.js");
            ClientResourceManager.RegisterScript(dnnPage, "~/DesktopModules/DNNQA/js/jquery.qaplaceholder.js");
        }

        /// <summary>
        /// This tells us if a user has the 'privilege' to perform an action.
        /// </summary>
        /// <param name="objPrivilege"></param>
        /// <param name="userScore"></param>
        /// <param name="editMode"></param>
        /// <returns></returns>
        public static bool HasPrivilege(QaSettingInfo objPrivilege, int userScore, bool editMode)
        {
            if (editMode)
            {
                return true;
            }
            if (userScore >= objPrivilege.Value)
            {
                return true;
            }
            return false;
        }

        #region Friendly Display

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public static string CalucalatePercentForDisplay(int numerator, int denominator)
        {
            if (denominator != 0)
            {
                if ((int)(((double)numerator / denominator) * 100) >= 100)
                {
                    return @"100 %";
                }
                return (int)(((double)numerator / denominator) * 100) + @" %";
            }
            return "0 %";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userScore"></param>
        /// <param name="privScore"></param>
        /// <returns></returns>
        public static int CalculatePointsTillNextPriv(int userScore, int privScore)
        {
            return privScore - userScore;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string CalculateDateForDisplay(DateTime date)
        {
            var utcDate = date.ToUniversalTime();
            var utcTimeDifference = DotNetNuke.Services.SystemDateTime.SystemDateTime.GetCurrentTimeUtc() - utcDate;

            if (utcTimeDifference.TotalSeconds < 60)
            {
                return (int)utcTimeDifference.TotalSeconds + Localization.GetString("secondsago", Constants.SharedResourceFileName);
            }
            if (utcTimeDifference.TotalMinutes < 60)
            {
                if (utcTimeDifference.TotalMinutes < 2)
                {
                    return (int)utcTimeDifference.TotalMinutes + Localization.GetString("minuteago", Constants.SharedResourceFileName);
                }
                return (int)utcTimeDifference.TotalMinutes + Localization.GetString("minutesago", Constants.SharedResourceFileName);
            }
            if (utcTimeDifference.TotalHours < 24)
            {
                if (utcTimeDifference.TotalHours < 2)
                {
                    return (int)utcTimeDifference.TotalHours + Localization.GetString("hourago", Constants.SharedResourceFileName);
                }
                return (int)utcTimeDifference.TotalHours + Localization.GetString("hoursago", Constants.SharedResourceFileName);
            }

            if (utcTimeDifference.TotalDays < 7)
            {
                if (utcTimeDifference.TotalDays < 2)
                {
                    return (int)utcTimeDifference.TotalDays + Localization.GetString("dayago", Constants.SharedResourceFileName);
                }
                return (int)utcTimeDifference.TotalDays + Localization.GetString("daysago", Constants.SharedResourceFileName);
            }

            if (utcTimeDifference.TotalDays < 30)
            {
                if (utcTimeDifference.TotalDays < 14)
                {
                    return (int)utcTimeDifference.TotalDays / 7 + Localization.GetString("weekago", Constants.SharedResourceFileName);
                }
                return (int)utcTimeDifference.TotalDays / 7 + Localization.GetString("weeksago", Constants.SharedResourceFileName);
            }

            if (utcTimeDifference.TotalDays < 180)
            {
                if (utcTimeDifference.TotalDays < 60)
                {
                    return (int)utcTimeDifference.TotalDays / 30 + Localization.GetString("monthago", Constants.SharedResourceFileName);
                }
                return (int)utcTimeDifference.TotalDays / 30 + Localization.GetString("monthsago", Constants.SharedResourceFileName);
            }

            //if (utcTimeDifference.TotalDays < 60)
            //{
            //    return 1 + Localization.GetString("monthago", Constants.SharedResourceFileName);
            //}

            // anything else (this is the only time we have to personalize it to the user)
            return date.ToShortDateString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string CalculateCountForDisplay(int count)
        {
            var friendlyCount = count.ToString();

            if (count < 999)
            {
                return friendlyCount;
            }
            if (count < 9999)
            {
                return friendlyCount.Substring(0, 1) + "K";
            }
            if (count < 99999)
            {
                return friendlyCount.Substring(0, 2) + "K";
            }
            if (count < 999999)
            {
                return friendlyCount.Substring(0, 3) + "K";
            }
            if (count < 9999999)
            {
                return friendlyCount.Substring(0, 1) + "M";
            }
            if (count < 99999999)
            {
                return friendlyCount.Substring(0, 2) + "M";
            }
            return friendlyCount;
        }

        #endregion

        #region Post Content

        /// <summary>
        /// Processes a post's body content prior to submission to the data store. It performs all content manipulation including security checks and returns it for saving to the data store.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>This will likely be updated w/ more content manipulation prior to save.</returns>
        public static string ProcessSavePostBody(string content)
        {
            var cntSecurity = new PortalSecurity();
            var cleanContent = cntSecurity.InputFilter(content, PortalSecurity.FilterFlag.NoScripting);
            return (cleanContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ProcessDisplayPostBody(string content)
        {
            return HttpUtility.HtmlDecode(content);
        }

        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        /// <remarks></remarks>
        public static string TruncateString(string source, int length, bool showElipse)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
                if (showElipse)
                {
                    source += @"...";
                }
            }
            return source;
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            for (var i = 0; i < source.Length; i++)
            {
                var let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        #endregion

        #region SEO

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="modContext"></param>
        /// <param name="pageTitle"></param>
        /// <param name="description"></param>
        /// <param name="link"></param>
        /// <param name="prevLink"></param>
        /// <param name="nextLink"></param>
        /// <remarks>Need to wire in page to accept page param in URL first.</remarks>
        public static void SetBrowsePageMeta(CDefault defaultPage, ModuleInstanceContext modContext, string pageTitle, string description, string link, string prevLink, string nextLink)
        {    
            var title = TruncateString(pageTitle + " - " + modContext.PortalSettings.PortalName, Constants.SeoTitleLimit, false);
            var content = TruncateString(description, Constants.SeoDescriptionLimit, false);
            var keyWords = defaultPage.KeyWords;

            SetPageMetaAndOpenGraph(defaultPage, modContext, title, content, keyWords, link);
            SetPagingMeta(defaultPage, prevLink, nextLink);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="modContext"></param>
        /// <remarks>In this method we don't really want to override the dnn stuff, we simply want to add the OG stuff.</remarks>
        public static void SetHomePageMeta(CDefault defaultPage, ModuleInstanceContext modContext)
        {
            var link = Links.Home(modContext.TabId);
            var content = defaultPage.Description;
            var title = defaultPage.Title;
            var keyWords = defaultPage.KeyWords;

            SetPageMetaAndOpenGraph(defaultPage, modContext, title, content, keyWords, link);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="modContext"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static void SetPrivilegePageMeta(CDefault defaultPage, ModuleInstanceContext modContext, string title, string description)
        {
            var link = Links.ViewPrivilege(modContext, title);
            SetPageMetaAndOpenGraph(defaultPage, modContext, title, description, "", link);
        }

        public static void SetBadgesPageMeta(CDefault defaultPage, ModuleInstanceContext modContext, string title, string description, string link)
        {
            SetPageMetaAndOpenGraph(defaultPage, modContext, title, description, "", link);
        }

        /// <summary>
        ///  Sets the Page Meta information for the Question.ascx view. 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="objQuestion"></param>
        /// <param name="modContext"></param>
        /// <remarks>Need to wire in page to accept page param in URL.</remarks>
        public static void SetQuestionPageMeta(CDefault defaultPage, QuestionInfo objQuestion, ModuleInstanceContext modContext)
        {
            var title = TruncateString(objQuestion.Title + " - " + modContext.PortalSettings.PortalName, Constants.SeoTitleLimit, false);
            var content = TruncateString(objQuestion.Body, Constants.SeoDescriptionLimit, false);
            var link = Links.ViewQuestion(objQuestion.PostId, objQuestion.Title, modContext.PortalSettings.ActiveTab, modContext.PortalSettings);
            var keyWords = "";
            var keyCount = 1;
            var count = keyCount;

            foreach (var term in objQuestion.Terms.TakeWhile(term => count <= Constants.SeoKeywordsLimit))
            {
                keyWords += "," + term.Name;
                keyCount += 1;
            }

            SetPageMetaAndOpenGraph(defaultPage, modContext, title, content, keyWords, link);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="modContext"></param>
        /// <param name="pageTitle"></param>
        /// <param name="description"></param>
        /// <param name="link"></param>
        /// <param name="prevLink"></param>
        /// <param name="nextLink"></param>
        public static void SetTagsPageMeta(CDefault defaultPage, ModuleInstanceContext modContext, string pageTitle, string description, string link, string prevLink, string nextLink)
        {
            var title = TruncateString(pageTitle + " - " + modContext.PortalSettings.PortalName, Constants.SeoTitleLimit, false);
            var content = TruncateString(description, Constants.SeoDescriptionLimit, false);
            var keyWords = defaultPage.KeyWords;

            SetPageMetaAndOpenGraph(defaultPage, modContext, title, content, keyWords, link);
            SetPagingMeta(defaultPage, prevLink, nextLink);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="objTerm"></param>
        /// <param name="modContext"></param>
        /// <param name="pageTitle"></param>
        /// <param name="description"></param>
        /// <remarks>Only primary module should set meta data.</remarks>
        public static void SetTermHistoryPageMeta(CDefault defaultPage, TermInfo objTerm, ModuleInstanceContext modContext, string pageTitle, string description)
        {
            var title = TruncateString(pageTitle + " - " + modContext.PortalSettings.PortalName, Constants.SeoTitleLimit, false);
            var content = TruncateString(description, Constants.SeoDescriptionLimit, false);
            var link = Links.ViewTagDetail(modContext, modContext.TabId, objTerm.Name);
            var keyWords = objTerm.Name;

            SetPageMetaAndOpenGraph(defaultPage, modContext, title, content, keyWords, link);
        }

        /// <summary>
        /// Sets the Page Meta information for the TagDetail.ascx view. 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="objTerm"></param>
        /// <param name="modContext"></param>
        /// <param name="pageTitle"></param>
        /// <param name="description"></param>
        public static void SetTermPageMeta(CDefault defaultPage, TermInfo objTerm, ModuleInstanceContext modContext, string pageTitle, string description)
        {
            var title = TruncateString(pageTitle + " - " + modContext.PortalSettings.PortalName, Constants.SeoTitleLimit, false);
            var content = TruncateString(description, Constants.SeoDescriptionLimit, false);
            var link = Links.ViewTagDetail(modContext, modContext.TabId, objTerm.Name);
            var keyWords = objTerm.Name;

            SetPageMetaAndOpenGraph(defaultPage, modContext, title, content, keyWords, link);
        }

        #endregion

        #region URL

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagename"></param>
        /// <returns></returns>
        public static string CreateFriendlySlug(string pagename)
        {
            //Set the PageName
            const RegexOptions options = RegexOptions.IgnoreCase;
            pagename = pagename.Replace("'", string.Empty);
            //Handle international characters
            pagename = Regex.Replace(pagename, "Ă|Ā|À|Á|Â|Ã|Ä|Å", "A");
            pagename = Regex.Replace(pagename, "ă|ā|à|á|â|ã|ä|å|ą", "a");
            pagename = Regex.Replace(pagename, "Æ", "AE");
            pagename = Regex.Replace(pagename, "æ", "ae");
            pagename = Regex.Replace(pagename, "ß", "ss");
            pagename = Regex.Replace(pagename, "Ç|Ć|Ĉ|Ċ|Č", "C");
            pagename = Regex.Replace(pagename, "ć|ĉ|ċ|č|ç", "c");
            pagename = Regex.Replace(pagename, "Ď|Đ", "D");
            pagename = Regex.Replace(pagename, "ď|đ", "d");
            pagename = Regex.Replace(pagename, "Ē|Ĕ|Ė|Ę|Ě|É|Ę|È|É|Ê|Ë", "E");
            pagename = Regex.Replace(pagename, "ē|ĕ|ė|ę|ě|ê|ë|è|é", "e");
            pagename = Regex.Replace(pagename, "Ĝ|Ğ|Ġ|Ģ|Ģ", "G");
            pagename = Regex.Replace(pagename, "ĝ|ğ|ġ|ģ|ģ", "g");
            pagename = Regex.Replace(pagename, "Ĥ|Ħ", "H");
            pagename = Regex.Replace(pagename, "ĥ|ħ", "h");
            pagename = Regex.Replace(pagename, "Ì|Í|Î|Ï|Ĩ|Ī|Ĭ|Į|İ|İ", "I");
            pagename = Regex.Replace(pagename, "ì|í|î|ï|ĩ|ī|ĭ|į", "i");
            pagename = Regex.Replace(pagename, "Ĳ", "IJ");
            pagename = Regex.Replace(pagename, "Ĵ", "J");
            pagename = Regex.Replace(pagename, "ĵ", "j");
            pagename = Regex.Replace(pagename, "Ķ", "K");
            pagename = Regex.Replace(pagename, "ķ", "k");
            pagename = Regex.Replace(pagename, "Ñ|Ñ", "N");
            pagename = Regex.Replace(pagename, "ñ", "n");
            pagename = Regex.Replace(pagename, "Ò|Ó|Ô|Õ|Ö|Ø|Ő", "O");
            pagename = Regex.Replace(pagename, "ò|ó|ô|õ|ö|ø|ő", "o");
            pagename = Regex.Replace(pagename, "Œ", "OE");
            pagename = Regex.Replace(pagename, "œ", "oe");
            pagename = Regex.Replace(pagename, "Ŕ|Ř|Ŗ|Ŕ", "R");
            pagename = Regex.Replace(pagename, "ř|ŗ|ŕ", "r");
            pagename = Regex.Replace(pagename, "Š|Ş|Ŝ|Ś", "S");
            pagename = Regex.Replace(pagename, "š|ş|ŝ|ś", "s");
            pagename = Regex.Replace(pagename, "Ť|Ţ", "T");
            pagename = Regex.Replace(pagename, "ť|ţ", "t");
            pagename = Regex.Replace(pagename, "Ų|Ű|Ů|Ŭ|Ū|Ũ|Ù|Ú|Û|Ü", "U");
            pagename = Regex.Replace(pagename, "ų|ű|ů|ŭ|ū|ũ|ú|û|ü|ù", "u");
            pagename = Regex.Replace(pagename, "Ŵ", "W");
            pagename = Regex.Replace(pagename, "ŵ", "w");
            pagename = Regex.Replace(pagename, "Ÿ|Ŷ|Ý", "Y");
            pagename = Regex.Replace(pagename, "ŷ|ÿ|ý", "y");
            pagename = Regex.Replace(pagename, "Ž|Ż|Ź", "Z");
            pagename = Regex.Replace(pagename, "ž|ż|ź", "z");

            pagename = Regex.Replace(pagename, "[^a-z0-9_-ĂăĀāÀÁÂÃÄÅàáâãäåąæÆßÇĆćĈĉĊċČčçĎďĐđĒēĔĕĖėĘęĚěÉêëĘÈÉÊËèéĜĝĞğĠġĢģĢģĤĥĦħÌÍÎÏĨĩĪīĬĭĮįİÌíîïìĲĴĵĶķÑÑÒÓÔÕÖŐØòóôõőöøñŒœŔřŘŗŖŕŔšŠşŞŝŜśŚťŤţŢųŲűŰůŮŭŬūŪũŨÙÚÛÜÙúûüùŵŴŸŷŶÝÿýžŽżŻźŹ]+", "-", options);
            //For titles with ' - ', we replace --- with -
            pagename = pagename.Replace("---", "-");

            //Remove trailing dash if one exists.
            if ((pagename.EndsWith("-")))
            {
                pagename = pagename.Substring(0,pagename.Length-1);
            }

            return pagename;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portalSettings"></param>
        /// <returns></returns>
        public static TabInfo GetTabFromUrl(PortalSettings portalSettings)
        {
            var tc = new TabController();
            TabInfo tInfo = null;
            var url = HttpContext.Current.Request.Url.AbsoluteUri.Replace("?" + HttpContext.Current.Request.QueryString, "").Replace("http://", "").Replace("https://", "").Replace(portalSettings.PortalAlias.HTTPAlias,"").ToLower();

            try
            {
                int tabId;
                if (url.Contains("tabid"))
                {
                    tabId = Int32.Parse(Regex.Match(url, "tabid[=/](\\d+)", RegexOptions.IgnoreCase).Groups[1].Value);
                    tInfo = tc.GetTab(tabId, portalSettings.PortalId, false);
                }
                else
                {

                    string strippedUrl = url;
                    do
                    {
                        tabId = TabController.GetTabByTabPath(portalSettings.PortalId, strippedUrl.Replace("/", "//").Replace("-", ""), portalSettings.CultureCode);
                        if (tabId != -1)
                        {
                            tInfo = tc.GetTab(tabId, portalSettings.PortalId, false);
                            break;
                        }

                        var slashIndex = strippedUrl.LastIndexOf('/');
                        strippedUrl = slashIndex > 1 ? strippedUrl.Substring(0, slashIndex) : "";
                    } while (strippedUrl.Length > 0);

                    //if (url.Contains("/" + GetQuestionUrlName(portalSettings).ToLower() + "/"))
                    //{
                    //    var tabPath = url.Substring(0, url.IndexOf("/" + GetQuestionUrlName(portalSettings).ToLower() + "/"));
                    //    //TODO JS: The - replacement needs to be more generic.
                    //    tabPath = tabPath.Replace("/", "//").Replace("-","");
                    //    tabId = TabController.GetTabByTabPath(portalSettings.PortalId, tabPath, portalSettings.CultureCode);
                    //    tInfo = tc.GetTab(tabId, portalSettings.PortalId, false);
                    //}
                    //else
                    //{
                    //    if (url.Contains("/" + GetTagUrlName().ToLower() + "/"))
                    //    {
                    //        var tabPath = url.Substring(0, url.IndexOf("/" + GetTagUrlName().ToLower() + "/"));
                    //        tabPath = tabPath.Replace("/", "//").Replace("-", "");
                    //        tabId = TabController.GetTabByTabPath(portalSettings.PortalId, tabPath, portalSettings.CultureCode);
                    //        tInfo = tc.GetTab(tabId, portalSettings.PortalId, false);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }

            if (tInfo == null)
            {
                var lc = new DotNetNuke.Services.Log.EventLog.LogController();
                var logInfo = new DotNetNuke.Services.Log.EventLog.LogInfo
                                  {
                                      LogTypeKey = "ADMIN_ALERT",
                                      LogPortalID = portalSettings.PortalId
                                  };

                logInfo.AddProperty("DNNQA HTTP Module", "GetTabFromURL returned null. URL=" + url);
                lc.AddLog(logInfo);
            }

            return tInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tabid"></param>
        /// <returns></returns>
        public static bool IsModuleOnTab(int tabid)
        {
            var modules = new ModuleController().GetTabModules(tabid);
            foreach (KeyValuePair<int, ModuleInfo> kvp in modules)
            {
                if (kvp.Value.DesktopModule.FriendlyName == "DNNQA")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddFriendlyUrlHttpModule()
        {
            try
            {
                const string configString = "<configuration>" +
                                            "<nodes configfile=\"web.config\">" +
                                            "<node path=\"/configuration/system.web/httpModules/add[@name='RequestFilter']\" action=\"insertbefore\" key=\"name\" collision=\"ignore\">" +
                                            "<add name=\"DNNQAUrlRewrite\" type=\"DotNetNuke.DNNQA.Components.Modules.UrlModule, DotNetNuke.Modules.DNNQA\" />" +
                                            "</node>" +
                                            "<node path=\"/configuration/system.webServer/modules/add[@name='RequestFilter']\" action=\"insertbefore\" key=\"name\" collision=\"ignore\">" +
                                            "<add name=\"DNNQAUrlRewrite\" type=\"DotNetNuke.DNNQA.Components.Modules.UrlModule, DotNetNuke.Modules.DNNQA\" preCondition=\"managedHandler\" />" +
                                            "</node>" +
                                            "</nodes>" +
                                            "</configuration>";

                var targetConfig = new XmlDocument();
                targetConfig.Load(Path.Combine(HttpContext.Current.Server.MapPath("~"), "web.config"));

                if (targetConfig.SelectSingleNode("/configuration/system.webServer/modules/add[@name='DNNQAUrlRewrite']") == null)
                {
                    var merge = new XmlMerge(new StringReader(configString), "", "");
                    merge.UpdateConfig(targetConfig);
                    Config.Save(targetConfig, "web.config");
                }

            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool IsFriendlyUrlModuleInstalled
        {
            get
            {
                var isInstalled = DotNetNuke.Common.Utilities.DataCache.GetCache(Constants.ModuleCacheKey + Constants.IsFriendlyUrlModuleInstalled);
                var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);
                if (isInstalled == null)
                {
                    var targetConfig = new XmlDocument();
                    targetConfig.Load(Path.Combine(HttpContext.Current.Server.MapPath("~"), "web.config"));

                    isInstalled = targetConfig.SelectSingleNode("/configuration/system.webServer/modules/add[@name='DNNQAUrlRewrite']") != null;

                    if (timeOut > 0 & Constants.EnableCaching)
                    {
                        DotNetNuke.Common.Utilities.DataCache.SetCache(Constants.ModuleCacheKey + Constants.IsFriendlyUrlModuleInstalled, isInstalled, TimeSpan.FromMinutes(timeOut));
                    }
                }
                return Convert.ToBoolean(isInstalled);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool UseFriendlyUrls
        {
            get
            {
                if (!IsFriendlyUrlModuleInstalled)
                {
                    return false;
                }
                var providerConfig = Framework.Providers.ProviderConfiguration.GetProviderConfiguration("friendlyUrl");
                var objProvider = (Framework.Providers.Provider)providerConfig.Providers[providerConfig.DefaultProvider];
                if (objProvider.Name == "DNNFriendlyUrl" && objProvider.Attributes["urlFormat"].ToLower() == "humanfriendly")
                {
                    return true;
                }
                return objProvider.Name != "DNNFriendlyUrl";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string GetQuestionUrlName(PortalSettings ps)
        {            
            return Localization.GetString("QuestionUrlName", Constants.SharedResourceFileName,ps,ps.DefaultLanguage);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetQuestionUrlName()
        {         
                return Localization.GetString("QuestionUrlName", Constants.SharedResourceFileName);         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetTagUrlName()
        {
            return Localization.GetString("TagUrlName", Constants.SharedResourceFileName); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static string GetTagUrlName(PortalSettings ps)
        {
            return Localization.GetString("TagUrlName", Constants.SharedResourceFileName,ps,ps.DefaultLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ContainsSpecialCharacter(string input)
        {
            // Characters that cause no results.
            //      \ - Backslash
            //      ; - Semi-colon
            //      ' - single quote
            // Characters that cause dangerous page request validation errors.
            //      &?*%@

            return input.IndexOfAny(Constants.DisallowedCharacters.ToCharArray())!=-1;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is meant to update SEO related meta information in the DotNetNuke base page. It is passed different information based on which specific 'page' in the module is being viewed.
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="modContext"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="keyWords"></param>
        /// <param name="link"></param>
        private static void SetPageMetaAndOpenGraph(CDefault defaultPage, ModuleInstanceContext modContext, string title, string content, string keyWords, string link)
        {
            defaultPage.Title = title;

            var meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:title");
            meta.Attributes.Add("content", title);
            defaultPage.Header.Controls.Add(meta);

            content = StripTagsCharArray(HttpUtility.HtmlDecode(content));
            var description = TruncateString(content, Constants.SeoDescriptionLimit, false);

            if (description.Length > 0)
            {
                defaultPage.Description = description;

                meta = new HtmlMeta();
                meta.Attributes.Add("property", "og:description");
                meta.Attributes.Add("content", description);
                defaultPage.Header.Controls.Add(meta);
            }

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:type");
            meta.Attributes.Add("content", "article");
            defaultPage.Header.Controls.Add(meta);

            if (keyWords.Length > 0)
            {
                defaultPage.KeyWords = keyWords;

                meta = new HtmlMeta();
                meta.Attributes.Add("property", "article:tag");
                meta.Attributes.Add("content", keyWords);
                defaultPage.Header.Controls.Add(meta);
            }

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:url");
            meta.Attributes.Add("content", link);
            defaultPage.Header.Controls.Add(meta);

            meta = new HtmlMeta();
            meta.Attributes.Add("property", "og:site_name");
            meta.Attributes.Add("content", modContext.PortalSettings.PortalName);
            defaultPage.Header.Controls.Add(meta);

            if (modContext.PortalSettings.LogoFile.Trim().Length > 0)
            {
                meta = new HtmlMeta();
                meta.Attributes.Add("property", "og:image");
                meta.Attributes.Add("content", "http://" + modContext.PortalAlias.HTTPAlias + "/Portals/" + modContext.PortalId + "/" + modContext.PortalSettings.LogoFile);
                defaultPage.Header.Controls.Add(meta);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultPage"></param>
        /// <param name="prevLink"></param>
        /// <param name="nextLink"></param>
        private static void SetPagingMeta(CDefault defaultPage, string prevLink, string nextLink)
        {

            if (prevLink != null)
            {
                var linkPrev = new HyperLink();
                linkPrev.Attributes.Add("rel", "prev");
                linkPrev.NavigateUrl = prevLink;
                defaultPage.Header.Controls.Add(linkPrev);
            }

            if (nextLink != null)
            {
                var linkNext = new HyperLink();
                linkNext.Attributes.Add("rel", "next");
                linkNext.NavigateUrl = nextLink;
                defaultPage.Header.Controls.Add(linkNext);
            }
        }

        #endregion

    }
}