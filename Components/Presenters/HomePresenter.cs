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
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Mvp;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Models;
using DotNetNuke.DNNQA.Components.Views;
using DotNetNuke.DNNQA.Components.Common;
using System.Web.UI.WebControls;
using DotNetNuke.DNNQA.Components.Entities;

namespace DotNetNuke.DNNQA.Components.Presenters
{

	/// <summary>
	/// 
	/// </summary>
	public class HomePresenter : ModulePresenter<IHomeView, HomeModel>
	{

		#region Members

		protected IDnnqaController Controller { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		private int QuestionPageSize
		{
			get
			{
				var pageSize = Constants.DefaultPageSize;
				if (ModuleContext.Settings.ContainsKey(Constants.SettingHomePageSize))
				{
					pageSize = Convert.ToInt32(ModuleContext.Settings[Constants.SettingHomePageSize]);
				}

				return pageSize;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private int TagPageSize
		{
			get
			{
				var pageSize = Constants.DefaultPageSize;
				if (ModuleContext.Settings.ContainsKey(Constants.SettingHomeMaxTags))
				{
					pageSize = Convert.ToInt32(ModuleContext.Settings[Constants.SettingHomeMaxTags]);
				}

				return pageSize;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private Constants.TagMode TagTimeFrame
		{
			get
			{
				if (ModuleContext.Settings.ContainsKey(Constants.SettingHomeTagTimeFrame))
				{
					switch (Convert.ToInt32(ModuleContext.Settings[Constants.SettingHomeTagTimeFrame]))
					{
						case (int)Constants.TagMode.ShowTotalUsage:
							return Constants.TagMode.ShowTotalUsage;
						case (int)Constants.TagMode.ShowMonthlyUsage:
							return Constants.TagMode.ShowMonthlyUsage;
						case (int)Constants.TagMode.ShowWeeklyUsage:
							return Constants.TagMode.ShowWeeklyUsage;
						//case (int)Constants.TagMode.ShowNoUsage:
						//    e.TagControl.CountMode = Constants.TagMode.ShowNoUsage;
						//    break;
						default:
							return Constants.TagMode.ShowDailyUsage;
					}
				}
				return Constants.TagMode.ShowDailyUsage;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable<QaSettingInfo> ThresholdCollection
		{
			get
			{
				return QaSettings.GetOpThresholdCollection(Controller.GetQaPortalSettings(ModuleContext.PortalId), ModuleContext.PortalId);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private UserScoreInfo UserScore
		{
			get
			{
				if (ModuleContext.PortalSettings.UserId > 0)
				{
					var usersScore = Controller.GetUserScore(ModuleContext.PortalSettings.UserId, ModuleContext.PortalId);
					if (usersScore != null)
					{
						return usersScore;
					}
				}
				var objUserScore = new UserScoreInfo
				{
					Message = "",
					PortalId = ModuleContext.PortalId,
					UserId = ModuleContext.PortalSettings.UserId,
					Score = 0
				};
				return objUserScore;
			}
		}

		/// <summary>
		/// TODO: Tie this to a module setting.
		/// </summary>
		private int VocabularyId
		{
			get { return 1; }
		}	

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		public HomePresenter(IHomeView view)
			: this(view, new DnnqaController(new SqlDataProvider()))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="view"></param>
		/// <param name="controller"></param>
		public HomePresenter(IHomeView view, IDnnqaController controller)
			: base(view)
		{
			if (view == null)
			{
				throw new ArgumentException(@"View is nothing.", "view");
			}

			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
			View.Load += ViewLoad;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		protected void ViewLoad(object sender, EventArgs eventArgs)
		{
			try
			{
				var objFlagHomeRemovalThreshold = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.PostFlagCompleteCount.ToString());
				var objMinHomeScore = ThresholdCollection.Single(s => s.Key == Constants.OpThresholds.QuestionHomeMinScore.ToString());

				var colModuleTags = Controller.GetTermsByContentType(ModuleContext.PortalId, ModuleContext.ModuleId, VocabularyId);

				IEnumerable<TermInfo> colTags;
				switch (TagTimeFrame)
				{
					case Constants.TagMode.ShowDailyUsage :
						colTags = (from t in colModuleTags where t.DayTermUsage > 0 select t);
						break;
					case Constants.TagMode.ShowWeeklyUsage :
						colTags = (from t in colModuleTags where t.WeekTermUsage > 0 select t);
						break;
					case Constants.TagMode.ShowMonthlyUsage :
						colTags = (from t in colModuleTags where t.MonthTermUsage > 0 select t);
						break;
					default :
						colTags = (from t in colModuleTags where t.TotalTermUsage > 0 select t);
						break;
				}

				View.Model.LatestQuestions = Controller.GetHomeQuestions(ModuleContext.ModuleId, QuestionPageSize, objFlagHomeRemovalThreshold.Value, objMinHomeScore.Value);
				View.Model.LatestTerms = Sorting.GetHomeTermCollection(TagPageSize, TagTimeFrame, colTags).ToList();
				View.ItemDataBound += ItemDataBound;
				View.TagItemDataBound += TagItemDataBound;
				View.DashboardDataBound += DashboardDataBound;

				View.Refresh();
			}
			catch (Exception exc)
			{
				ProcessModuleLoadException(exc);
			}
			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ItemDataBound(object sender, HomeQuestionsEventArgs<HyperLink, QuestionInfo, Literal, Literal, Literal, Literal, Literal, Panel, Controls.Tags, Image> e)
		{
			var control = (Control)sender;

			e.TitleLink.Text = e.ObjQuestion.Title;
			e.TitleLink.NavigateUrl = Links.ViewQuestion(e.ObjQuestion.PostId, e.ObjQuestion.Title, ModuleContext.PortalSettings.ActiveTab, ModuleContext.PortalSettings);

			if (e.ObjQuestion.TotalAnswers > 0)
			{
				e.AnswersPanel.CssClass = "answers multiple";
			}

			if (e.ObjQuestion.AnswerId > 0)
			{
				e.AnswersLiteral.Text = @"<span class='count accepted'>" + e.ObjQuestion.TotalAnswers + @"</span>";
				e.AnswersTextLiteral.Text = @"<span class='accepted'>" + Localization.GetString("Answers", LocalResourceFile) + @"</span>";
			}
			else
			{
				e.AnswersLiteral.Text = @"<span class='count'>" + e.ObjQuestion.TotalAnswers + @"</span>";
				e.AnswersTextLiteral.Text = @"<span>" + Localization.GetString("Answers", LocalResourceFile) + @"</span>";
			}
			
			e.ViewsLiteral.Text = e.ObjQuestion.ViewCount.ToString();
			e.VotesLiteral.Text = e.ObjQuestion.QuestionVotes.ToString();

			e.Tags.ModContext = ModuleContext;
			e.Tags.ModuleTab = ModuleContext.PortalSettings.ActiveTab;
			e.Tags.DataSource = e.ObjQuestion.QaTerms(VocabularyId);
			e.Tags.DataBind();

			var objUser = DotNetNuke.Entities.Users.UserController.GetUserById(ModuleContext.PortalId, e.ObjQuestion.LastApprovedUserId);
			e.DateLiteral.Text = Utils.CalculateDateForDisplay(e.ObjQuestion.LastApprovedDate) + @" by <a href=" + DotNetNuke.Common.Globals.UserProfileURL(e.ObjQuestion.LastApprovedUserId) + @">" + objUser.DisplayName+ @"</a>";

			e.AcceptedImage.Visible = e.ObjQuestion.AnswerId > 0;
			e.AcceptedImage.ImageUrl = control.ResolveUrl("~/DesktopModules/DNNQA/images/accepted.png");
			e.AcceptedImage.AlternateText = Localization.GetString("imgAccepted", Constants.SharedResourceFileName);
			e.AcceptedImage.ToolTip = Localization.GetString("AcceptedAnswer", Constants.SharedResourceFileName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void TagItemDataBound(object sender, HomeTagsEventArgs<TermInfo, Controls.Tags> e)
		{
			var colTerms = new List<TermInfo> {e.Term};

			e.TagControl.ModContext = ModuleContext;
			e.TagControl.ModuleTab = ModuleContext.PortalSettings.ActiveTab;
			e.TagControl.DataSource = colTerms;
			e.TagControl.CountMode = TagTimeFrame;			
			e.TagControl.DataBind();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void DashboardDataBound(object sender, HomeUserEventArgs<HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, HtmlGenericControl, Literal, Literal, Literal, Literal> e)
		{
			e.DashHeader.Visible = ModuleContext.PortalSettings.UserId > 0;
			e.DashList.Visible = ModuleContext.PortalSettings.UserId > 0;

			e.FavoriteTagsHead.Visible = false;
			e.FavoriteTagsList.Visible = false;

			var dashCount = Localization.GetString("dashCount", LocalResourceFile);
			var colAnswers = Controller.GetUserAnswers(ModuleContext.PortalId, ModuleContext.PortalSettings.UserId);
			e.AnswerLiteral.Text = dashCount.Replace("{0}", colAnswers.Count.ToString());

			var colQuestions = Controller.GetUserQuestions(ModuleContext.PortalId, ModuleContext.PortalSettings.UserId);
			e.QuestionLiteral.Text = dashCount.Replace("{0}", colQuestions.Count.ToString());

			var colSubs = Controller.GetUserSubscriptions(ModuleContext.PortalId, ModuleContext.PortalSettings.UserId);
			e.SubscriptionLiteral.Text = dashCount.Replace("{0}", colSubs.Count.ToString());

			e.ScoreLiteral.Text = Localization.GetString("userRep", LocalResourceFile).Replace("{0}", UserScore.Score.ToString());
		}

		#endregion

	}
}