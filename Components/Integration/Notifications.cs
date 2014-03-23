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
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Social.Notifications;

namespace DotNetNuke.DNNQA.Components.Integration
{
    public class Notifications
    {

        /// <summary>
        /// This method will create a new notification message in the data store.
        /// </summary>
        /// <param name="objEntity"></param>
        /// <param name="portalId"></param>
        /// <param name="tabId"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <remarks>The last part of this method is commented out but was setup to send to a role (based on a group). You can utilize this and/or also pass a list of users.</remarks>
        internal void ItemNotification(PostInfo objEntity, int portalId, int tabId, string subject, string body)
        {
            var notificationType = NotificationsController.Instance.GetNotificationType(Constants.NotificationQaFlag);

            var notificationKey = string.Format("{0}:{1}:{2}", Constants.ContentTypeName, objEntity.PostId, tabId);
            var objNotification = new Notification
            {
                NotificationTypeID = notificationType.NotificationTypeId,
                Subject = subject,
                Body = body,
                IncludeDismissAction = true,
                SenderUserID = objEntity.CreatedByUserID,
                Context = notificationKey
            };

            //// invite the members of the group
            //var colRoles = new List<RoleInfo>();
            //var objGroup = TestableRoleController.Instance.GetRole(portalId, r => r.RoleID == objEntity.GroupId);
            //colRoles.Add(objGroup);

            //NotificationsController.Instance.SendNotification(objNotification, portalId, colRoles, null);
        }

        #region Install Methods

        /// <summary>
        /// This will create a notification type used by the module and also handle the actions that must be associated with it.
        /// </summary>
        /// <remarks>This should only ever run once, during 1.0.0 install (via IUpgradeable). 
        /// </remarks>
        static internal void AddNotificationTypes()
        {
            var actions = new List<NotificationTypeAction>();
            var deskModuleId = DesktopModuleController.GetDesktopModuleByFriendlyName("DNNQA").DesktopModuleID;

            var objNotificationType = new NotificationType
            {
                Name = Constants.NotificationQaFlag,
                Description = "Q&A Notification",
                DesktopModuleId = deskModuleId
            };

            if (NotificationsController.Instance.GetNotificationType(objNotificationType.Name) != null) return;
            var objAction = new NotificationTypeAction
            {
                NameResourceKey = "AcceptInvite",
                DescriptionResourceKey = "AcceptInvite_Desc",
                APICall = "DesktopModules/DNNQA/API/SocialModuleService.ashx/ActionMethod",
                Order = 1
            };
            actions.Add(objAction);

            NotificationsController.Instance.CreateNotificationType(objNotificationType);
            NotificationsController.Instance.SetNotificationTypeActions(actions, objNotificationType.NotificationTypeId);
        }

        #endregion

    }
}