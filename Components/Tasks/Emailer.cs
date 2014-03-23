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
using System.Text;
using DotNetNuke.Common.Utilities;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Content;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Mail;

namespace DotNetNuke.DNNQA.Components.Tasks 
{

	/// <summary>
	/// 
	/// </summary>
	public class Emailer : DotNetNuke.Services.Scheduling.SchedulerClient
	{

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objScheduleHistoryItem"></param>
		public Emailer(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem)
			: this(objScheduleHistoryItem, new DnnqaController(new SqlDataProvider()))
		{
				
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="objScheduleHistoryItem"></param>
		/// <param name="controller"></param>
		public Emailer(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem, IDnnqaController controller)
		{
			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
			ScheduleHistoryItem = objScheduleHistoryItem;
		}

		#endregion

		protected IDnnqaController Controller { get; private set; }

		#region Public Method

		/// <summary>
		/// 
		/// </summary>
		public override void DoWork()
		{
			try
			{
				Progressing(); // OPTIONAL
			
				var currentRunDate = DateTime.Now;
				var lastInstantRun = DateTime.Now;
				var lastDailyRun = DateTime.MinValue;

				var colScheduleItemSettings = DotNetNuke.Services.Scheduling.SchedulingProvider.Instance().GetScheduleItemSettings(ScheduleHistoryItem.ScheduleID);

				if (colScheduleItemSettings.Count > 0)
				{
					if (colScheduleItemSettings.ContainsKey(Constants.ScheduleItemSettings.InstantLastRunDate.ToString()))
					{
						lastInstantRun = Convert.ToDateTime(colScheduleItemSettings[Constants.ScheduleItemSettings.InstantLastRunDate.ToString()]);
					}

					if (colScheduleItemSettings.ContainsKey(Constants.ScheduleItemSettings.DailyLastRunDate.ToString()))
					{
						lastDailyRun = Convert.ToDateTime(colScheduleItemSettings[Constants.ScheduleItemSettings.DailyLastRunDate.ToString()]);
					}

				}
				else
				{                
					DotNetNuke.Services.Scheduling.SchedulingProvider.Instance().AddScheduleItemSetting(ScheduleHistoryItem.ScheduleID, Constants.ScheduleItemSettings.InstantLastRunDate.ToString(), lastInstantRun.ToString());
					DotNetNuke.Services.Scheduling.SchedulingProvider.Instance().AddScheduleItemSetting(ScheduleHistoryItem.ScheduleID, Constants.ScheduleItemSettings.DailyLastRunDate.ToString(), lastDailyRun.ToString());
				}

				// handle instant notifications
				var strResults = GenerateNotifications(lastInstantRun, currentRunDate, Constants.SubscriptionType.InstantPost);
				strResults += GenerateNotifications(lastInstantRun, currentRunDate, Constants.SubscriptionType.InstantTerm);
				// update settings (we have to create a method since the core doesn't have one for us)
				Controller.UpdateScheduleItemSetting(ScheduleHistoryItem.ScheduleID, Constants.ScheduleItemSettings.InstantLastRunDate.ToString(), currentRunDate.ToString());

				// handle daily
				// first, see if we need to even attempt a daily (get daily run's last date, see if it has been 24 hours)
				if (lastDailyRun < currentRunDate.AddHours(-24))
				{
					strResults += GenerateNotifications(lastDailyRun, currentRunDate, Constants.SubscriptionType.DailyTerm);

					// update settings (we have to create a method since the core doesn't have one for us) - only update when we send the emails
					Controller.UpdateScheduleItemSetting(ScheduleHistoryItem.ScheduleID, Constants.ScheduleItemSettings.DailyLastRunDate.ToString(), currentRunDate.ToString());
				}

				ScheduleHistoryItem.Succeeded = true; // REQUIRED
				ScheduleHistoryItem.AddLogNote(strResults); // OPTIONAL
			}
			catch (Exception exc)
			{
				ScheduleHistoryItem.Succeeded = false; // REQUIRED
				ScheduleHistoryItem.AddLogNote("Q&A Emailer Task Failed. " + exc); // OPTIONAL
				//this.Errored(exc); // REQUIRED
				DotNetNuke.Services.Exceptions.Exceptions.LogException(exc); // OPTIONAL
				throw;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private string GenerateNotifications(DateTime lastRunDate, DateTime currentRunDate, Constants.SubscriptionType subType)
		{
			var strErrors = "";

			// based on subscription type, generate list of content additions to email
			switch (subType)
			{
				case Constants.SubscriptionType.DailyTerm:
					// daily term (all questions created between dates)

					// This email is going to be user specific, so we need to start w/ a list of subscribers (based on type daily term); each user can have multiple terms
					// -- select all questions where 

					// we retrieve a collection of questions, each question will have a contentItem associated with it (we only retrieve stuff not voted out)

					// we need to retrieve a collection of terms, based on content item collection from above
					strErrors = "Daily Term: Sent " + 0 + " emails <br />";
					break;
				case Constants.SubscriptionType.InstantTerm:
					var typeController = new ContentTypeController();
					var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == Constants.ContentTypeName select t);

					if (colContentTypes.Count() > 0)
					{
						var contentType = colContentTypes.Single();
						var colContentItems = Controller.GetContentItemsByTypeAndCreated(contentType.ContentTypeId, lastRunDate, currentRunDate);

						foreach (var item in colContentItems)
						{
							// we are using the question object because content item doesn't have a title association yet (for core integration)
							var objQuestion = Controller.GetQuestionByContentItem(item.ContentItemId);
							
							if (objQuestion != null)
							{
								// use the content item to build the email subject/body/content (sub question for content item now)
								var colEmail = QaSettings.GetEmailCollection(Controller.GetQaPortalSettings(objQuestion.PortalId), objQuestion.PortalId);
								var senderEmail = colEmail.Single(s => s.Key == Constants.EmailSettings.FromAddress.ToString()).Value;
								//var urlBase = colEmail.Single(s => s.Key == Constants.EmailSettings.PrimaryUrl.ToString()).Value;
								var questionTemplate =  colEmail.Single(s => s.Key == Constants.EmailSettings.SingleQuestionTemplate.ToString()).Value;
								var ps = new PortalSettings(objQuestion.PortalId);

								var titleLink = "http://" + ps.DefaultPortalAlias + "/tabid/" + objQuestion.TabID + "/view/question/id/" + objQuestion.PostId + "/" + DotNetNuke.Common.Globals.glbDefaultPage;
								var subscribeLink = "http://" + ps.DefaultPortalAlias + "/tabid/" + objQuestion.TabID + "/view/subscriptions/" + DotNetNuke.Common.Globals.glbDefaultPage;

								var terms = "";
								var i = 0;
								foreach (var t in objQuestion.Terms)
								{
									terms += t.Name;
									i += 1;

									if (objQuestion.Terms.Count != i)
									{
										terms += ", ";
									}
								}

								questionTemplate = questionTemplate.Replace("[AUTHOR]", objQuestion.CreatedByDisplayName);
								questionTemplate = questionTemplate.Replace("[TERMS]", terms);
								questionTemplate = questionTemplate.Replace("[TITLELINK]", titleLink);
								questionTemplate = questionTemplate.Replace("[TITLE]", objQuestion.Title);
								questionTemplate = questionTemplate.Replace("[BODY]", Utils.ProcessDisplayPostBody(objQuestion.Body));
								questionTemplate = questionTemplate.Replace("[SUBSCRIBELINK]", subscribeLink);

								var colSubscribers = Controller.GetSubscribersByContentItem(item.ContentItemId, (int)Constants.SubscriptionType.InstantTerm, objQuestion.PortalId);
								foreach (var subscriber in colSubscribers)
								{
									// send off the email one by one (same email to multiple subscribers)
									Mail.SendMail(senderEmail, subscriber.Email, "", "", MailPriority.Normal,
												  HtmlUtils.StripWhiteSpace(objQuestion.Title, true), MailFormat.Html, Encoding.UTF8, questionTemplate,
												  "", Host.SMTPServer, Host.SMTPAuthentication, Host.SMTPUsername,
												  Host.SMTPPassword, Host.EnableSMTPSSL);
								}

								strErrors = "Instant Term: Sent " + colSubscribers.Count + " emails - " + objQuestion.Title + "<br />";
							}
						}
					}
					else
					{
						strErrors = "Instant Term: No email to send <br />";
					}
				
					break;
				default:
					var colAnswers = Controller.GetAnswersByDate(lastRunDate, currentRunDate);

					if (colAnswers.Count() > 0)
					{
						// for each content item in the collection, get a list of subscribers and send the emails off one by one. 
						foreach (var item in colAnswers)
						{
							var objQuestion = Controller.GetQuestionByContentItem(item.ContentItemId);
							// use the post item to build the email subject/body/content (sub question for content item now)
							var colEmail = QaSettings.GetEmailCollection(Controller.GetQaPortalSettings(item.PortalId), item.PortalId);
							var senderEmail = colEmail.Single(s => s.Key == Constants.EmailSettings.FromAddress.ToString()).Value;
							//var urlBase = colEmail.Single(s => s.Key == Constants.EmailSettings.PrimaryUrl.ToString()).Value;
							var answerTemplate = colEmail.Single(s => s.Key == Constants.EmailSettings.AnswerTemplate.ToString()).Value;
							var ps = new PortalSettings(item.PortalId);

							var titleLink = "http://" + ps.DefaultPortalAlias + "/tabid/" + objQuestion.TabID + "/view/question/id/" + objQuestion.PostId + "/" + DotNetNuke.Common.Globals.glbDefaultPage;
							var subscribeLink = "http://" + ps.DefaultPortalAlias + "/tabid/" + objQuestion.TabID + "/view/subscriptions/" + DotNetNuke.Common.Globals.glbDefaultPage;

							answerTemplate = answerTemplate.Replace("[AUTHOR]", item.PostCreatedDisplayName);
							answerTemplate = answerTemplate.Replace("[TITLELINK]", titleLink);
							answerTemplate = answerTemplate.Replace("[TITLE]", objQuestion.Title);
							answerTemplate = answerTemplate.Replace("[BODY]", Utils.ProcessDisplayPostBody(item.Body));
							answerTemplate = answerTemplate.Replace("[SUBSCRIBELINK]", subscribeLink);

							var colSubscribers = Controller.GetSubscribersByQuestion(item.ParentId, (int)Constants.SubscriptionType.InstantPost, item.PortalId);
							foreach (var subscriber in colSubscribers)
							{
								// send off the email one by one (same email to multiple subscribers)
								Mail.SendMail(senderEmail, subscriber.Email, "", "", MailPriority.Normal,
											  HtmlUtils.StripWhiteSpace(objQuestion.Title, true), MailFormat.Html, Encoding.UTF8, answerTemplate,
											  "", Host.SMTPServer, Host.SMTPAuthentication, Host.SMTPUsername,
											  Host.SMTPPassword, Host.EnableSMTPSSL);
							}

							strErrors = "Instant Post: Sent " + colSubscribers.Count + " emails - " + objQuestion.Title + "<br />";
						}
					}
					else
					{
						strErrors = "Instant Post: No new answers to email <br />";
					}

					// now we move onto comments
					var colComments = Controller.GetCommentsByDate(lastRunDate, currentRunDate);

					if (colComments.Count() > 0)
					{
						foreach (var item in colComments)
						{
							var objPost = Controller.GetPost(item.PostId, -1);
							var ps = new PortalSettings(objPost.PortalId);
							var objQuestion = objPost.ParentId == 0 ? objPost : Controller.GetQuestionByContentItem(objPost.ContentItemId);
							var colEmail = QaSettings.GetEmailCollection(Controller.GetQaPortalSettings(objQuestion.PortalId), objPost.PortalId);
							var commentTemplate = colEmail.Single(s => s.Key == Constants.EmailSettings.CommentTemplate.ToString()).Value;
							var senderEmail = colEmail.Single(s => s.Key == Constants.EmailSettings.FromAddress.ToString()).Value;

							var titleLink = "http://" + ps.DefaultPortalAlias + "/tabid/" + objQuestion.TabID + "/view/question/id/" + objQuestion.PostId + "/" + DotNetNuke.Common.Globals.glbDefaultPage;
							var subscribeLink = "http://" + ps.DefaultPortalAlias + "/tabid/" + objQuestion.TabID + "/view/subscriptions/" + DotNetNuke.Common.Globals.glbDefaultPage;

							commentTemplate = commentTemplate.Replace("[AUTHOR]", DotNetNuke.Entities.Users.UserController.GetUserById(objQuestion.PortalId, item.UserId).DisplayName);
							commentTemplate = commentTemplate.Replace("[TITLELINK]", titleLink);
							commentTemplate = commentTemplate.Replace("[TITLE]", objQuestion.Title);
							commentTemplate = commentTemplate.Replace("[COMMENT]", Utils.ProcessDisplayPostBody(item.Comment));
							commentTemplate = commentTemplate.Replace("[SUBSCRIBELINK]", subscribeLink);

							var colSubscribers = Controller.GetSubscribersByQuestion(objQuestion.PostId, (int)Constants.SubscriptionType.InstantPost, objQuestion.PortalId);
							foreach (var subscriber in colSubscribers)
							{
								// send off the email one by one (same email to multiple subscribers)
								Mail.SendMail(senderEmail, subscriber.Email, "", "", MailPriority.Normal,
											  HtmlUtils.StripWhiteSpace(objQuestion.Title, true), MailFormat.Html, Encoding.UTF8, commentTemplate,
											  "", Host.SMTPServer, Host.SMTPAuthentication, Host.SMTPUsername,
											  Host.SMTPPassword, Host.EnableSMTPSSL);
							}
							// we also have to remember to check if we emailed notification of a comment around this question already (only within this method, to avoid to comments on question being emailed 2x). 
							// also avoid emailing someone of their own comment

						}
					}
					else
					{
						strErrors = "Instant Post: No new comments to email <br />";
					}

					break;
			}
			
			return strErrors;
		}

		#endregion
	
	}
}