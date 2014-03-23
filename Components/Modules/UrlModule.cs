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
using System.Web;
using System.Text.RegularExpressions;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Controllers;

namespace DotNetNuke.DNNQA.Components.Modules
{

    public class UrlModule : IHttpModule
    {

        #region IHttpModule Members

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void OnBeginRequest(Object source, EventArgs e)
        {
            try
            {
                var app = (HttpApplication)source;
                var context = app.Context;
                PortalSettings portalSettings;
                PortalAliasController pac = new PortalAliasController();

                PortalInfo portalInfo;
                //var requestedPath = app.Request.Url.AbsoluteUri;
                PortalAliasInfo objPortalAlias;
          
                 var url = context.Request.RawUrl.Replace("?" + context.Request.QueryString, "").ToLower();

                if (Utils.UseFriendlyUrls && (url.EndsWith(".aspx") || url.EndsWith("/")))
                {
                    var myAlias = DotNetNuke.Common.Globals.GetDomainName(app.Request, true);

                    do
                    {
                        objPortalAlias = PortalAliasController.GetPortalAliasInfo(myAlias);

                        if (objPortalAlias != null)
                        {
                            break;
                        }

                        var slashIndex = myAlias.LastIndexOf('/');
                        myAlias = slashIndex > 1 ? myAlias.Substring(0, slashIndex) : "";
                    } while (myAlias.Length > 0);

                    if (objPortalAlias == null)
                    {
                        //portalSettings = new PortalSettings(Host.HostPortalID);
                        //var portalAlias = app.Request.Url.Host;
                        //objPortalAlias = PortalAliasController.GetPortalAliasInfo(portalAlias);
                        return;
                        //if (Host.HostPortalID > Null.NullInteger)
                        //{
                        //    var portalAliasInfo = new PortalAliasInfo
                        //                              {
                        //                                  PortalID = Host.HostPortalID,
                        //                                  HTTPAlias = portalAlias
                        //                              };
                        //    pac.AddPortalAlias(portalAliasInfo);

                        //objPortalAlias = PortalAliasController.GetPortalAliasInfo(portalAlias);
                        //}
                    }
                    else
                    {
                        portalInfo = new PortalController().GetPortal(objPortalAlias.PortalID);
                        portalSettings = new PortalSettings(portalInfo.PortalID);
                    }

                    if (portalSettings != null)
                    {
                        portalSettings.PortalAlias = objPortalAlias;

                        Regex questionRegEx;
                        Regex tagRegEx;

                        questionRegEx = new Regex("/" + Utils.GetQuestionUrlName(portalSettings).ToLower() + "/([0-9]+)/(.+)(\\.aspx$|\\.aspx?.+)", RegexOptions.IgnoreCase);
                        tagRegEx = new Regex("/" + Utils.GetTagUrlName(portalSettings).ToLower() + "/(.+)(\\.aspx$|\\.aspx?.+)", RegexOptions.IgnoreCase);

                        if ((questionRegEx.IsMatch(url.ToLower()) && !questionRegEx.Match(url.ToLower()).Groups[1].Value.Contains("/")) || (tagRegEx.IsMatch(url.ToLower()) && !tagRegEx.Match(url.ToLower()).Groups[1].Value.Contains("/")))
                        {
                            string questionTitle;
                            string tagName;
                            

                            // JS 1/25/12: This check is for removing the .aspx from the question and tags.
                            //             There appears to be conflicts between IIS7.5 installations that need to be address
                            //if (HttpRuntime.UsingIntegratedPipeline)
                            //{
                            //    questionRegEx = new Regex("/Question/([0-9]+)/(.+)", RegexOptions.IgnoreCase);
                            //    tagRegEx = new Regex("/Tag/(.+)", RegexOptions.IgnoreCase);
                            //}
                            //else
                            //{
                            //    questionRegEx = new Regex("/Question/([0-9]+)/(.+)\\.aspx$", RegexOptions.IgnoreCase);
                            //    tagRegEx = new Regex("/Tag/(.+)\\.aspx$", RegexOptions.IgnoreCase);
                            //}

                            var dnnqa = new DnnqaController();
                            var tInfo = Utils.GetTabFromUrl(portalSettings);
                            
                            if (tInfo != null && Utils.IsModuleOnTab(tInfo.TabID))
                            {
                                

                                var match = questionRegEx.Match(url);
                                String relativePath;
                                if (match.Success)
                                {
                                    var questionId = Int32.Parse(match.Groups[1].Value);
                                    questionTitle = match.Groups[2].Value;
                                    if (tInfo != null)
                                    {

                                        QuestionInfo qInfo = dnnqa.GetQuestion(questionId, portalSettings.PortalId);

                                        if (qInfo != null)
                                        {
                                            if (Utils.CreateFriendlySlug(qInfo.Title).ToLower() == questionTitle.ToLower())
                                            {
                                                relativePath = Links.ViewQuestion(questionId, tInfo.TabID, portalSettings).Replace("http://", "").Replace("https://", "").Replace(objPortalAlias.HTTPAlias.Contains("/") ? objPortalAlias.HTTPAlias.Substring(0, objPortalAlias.HTTPAlias.IndexOf("/")) : objPortalAlias.HTTPAlias, "");

                                                context.RewritePath(relativePath);
                                                return;
                                            }
                                            context.Response.Status = "301 Moved Permanently";
                                            context.Response.RedirectLocation = Links.ViewQuestion(questionId, qInfo.Title, tInfo, portalSettings);
                                        }
                                    }
                                }

                                match = tagRegEx.Match(url);
                                if (match.Success)
                                {
                                    tagName = match.Groups[1].Value;
                                    tagName = tagName.Replace("-", " ");
                                    relativePath = Links.ViewTaggedQuestions(tagName, tInfo.TabID, portalSettings).Replace("http://", "").Replace("https://", "").Replace(objPortalAlias.HTTPAlias.Contains("/") ? objPortalAlias.HTTPAlias.Substring(0, objPortalAlias.HTTPAlias.IndexOf("/")) : objPortalAlias.HTTPAlias, "");

                                    context.RewritePath(relativePath);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }
        }

    }
}