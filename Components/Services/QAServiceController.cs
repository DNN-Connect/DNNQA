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
using System.Web.Mvc;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Social.Notifications;
using DotNetNuke.Web.Services;

namespace DotNetNuke.DNNQA.Components.Services
{

    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
    //[SupportedModules("Social Events")]
    [DnnAuthorize]
    [ValidateAntiForgeryToken]
    public class SocialEventServiceController : DnnController, IServiceRouteMapper
    {

        #region Explicit Interface Methods

        public void RegisterRoutes(IMapRoute mapRouteManager)
        {

            mapRouteManager.MapRoute("DNNQA", "{controller}.ashx/{action}", new[] { "DotNetNuke.DNNQA.Components.Services" });
        }

        #endregion

        #region Private Methods

        private static string GetFilteredValue(PortalSecurity objSecurity, string value)
        {
            return objSecurity.InputFilter(
                value,
                PortalSecurity.FilterFlag.NoScripting | PortalSecurity.FilterFlag.NoAngleBrackets
                | PortalSecurity.FilterFlag.NoMarkup);
        }

        //private void ParseKey(string key)
        //{
        //    var keys = key.Split(Convert.ToChar(":"));
        //    // 0 is content type string, to ensure unique key
        //    _eventId = int.Parse(keys[1]);
        //    _groupId = int.Parse(keys[2]);
        //    _tabId = int.Parse(keys[3]);
        //}

        #endregion

        #region Public Methods

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateAttendStatus(int portalId, int tabId, int groupId, int eventId, int status)
        {
            //var controller = new SocialEventsController();
            //var @event = controller.GetEvent(
            //    eventId, UserInfo.UserID, UserInfo.Profile.PreferredTimeZone);
            //if (@event == null) return Json(new { Result = "success" });

            //var objGuest = new EventGuestInfo
            //{
            //    EventId = @event.EventId,
            //    UserId = UserInfo.UserID,
            //    Email = UserInfo.Email,
            //    InvitedOnDate = @event.CreatedOnDate,
            //    RepliedOnDate = DateTime.Now,
            //    RSVPStatus = status
            //};
            //controller.UpdateGuestStatus(objGuest);

            //var url = DotNetNuke.Common.Globals.NavigateURL(tabId, "", "eventid=" + eventId);
            //if (groupId > Null.NullInteger) url = DotNetNuke.Common.Globals.NavigateURL(tabId, "", "eventid=" + eventId, "groupid=" + groupId);

            //var cntJournal = new Journal();
            //cntJournal.AddSocialEventAttendToJournal(objGuest, @event.Name, @event.GroupId, tabId, @event.PortalId, objGuest.UserId, url);

            //// Notification Integration
            //var notificationType = NotificationsController.Instance.GetNotificationType(Constants.NotificationEventInviteTypeName);
            //var notificationKey = string.Format("{0}:{1}:{2}:{3}", Constants.ContentTypeName, @event.EventId, @event.GroupId, PortalSettings.ActiveTab.TabID);
            //var objNotify = NotificationsController.Instance.GetNotificationByContext(notificationType.NotificationTypeId, notificationKey).SingleOrDefault();

            //if (objNotify != null)
            //{
            //    NotificationsController.Instance.DeleteNotificationRecipient(objNotify.NotificationID, UserInfo.UserID);
            //}

            var response = new { Value = eventId, Result = "success" };

            return Json(response);
        }

        #endregion

    }
}