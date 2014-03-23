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

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Providers.Data;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;

namespace DotNetNuke.DNNQA.Controls
{

	/// <summary>
	/// 
	/// </summary>
	[DefaultProperty("Terms"), Themeable(false)]
	[ToolboxData("<{0}:Tags runat=server> </{0}:Tags>")]
	public class Tags : CompositeDataBoundControl
	{

		#region Private Members

		protected IDnnqaController Controller { get; private set; }
		public event EventHandler SubscribeClick;

		private LinkButton _cmdSubscribe;
		private Hashtable _htTags;

		private static readonly object EventSubmitKey = new object();

		/// <summary>
		/// A collection of terms to be rendered by the control.
		/// </summary>
		[Browsable(false)]
		private List<TermInfo> Terms { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public Tags()
			: this(new DnnqaController(new SqlDataProvider()))
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="controller"></param>
		public Tags(IDnnqaController controller)
		{
			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// This provides a full path to the shared resource file for localization. 
		/// </summary>
		internal string SharedResourceFile
		{
			get { return ResolveUrl("~/DesktopModules/DNNQA/App_LocalResources/SharedResources.resx"); }
		}

		[Browsable(false)]
		public ModuleInstanceContext ModContext { get; set; }

		public Constants.TagMode CountMode { get; set; }

		public TabInfo ModuleTab { get; set; }

		#endregion

		#region Event Handlers

		protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
		{
			Controls.Clear();

			int count = 0;
			_htTags = new Hashtable();

			if (dataSource != null)
			{
				var e = dataSource.GetEnumerator();

				if (dataSource is List<TermInfo>)
				{
					Terms = (List<TermInfo>)dataSource;

					foreach (var term in Terms)
					{
						_cmdSubscribe = new LinkButton
						{
							CausesValidation = false,
							CssClass = ""
						};

						_cmdSubscribe.Command += SubscribeCommand;

						var colUserSubs = Controller.GetUserSubscriptions(ModContext.PortalId, ModContext.PortalSettings.UserId);
						var term1 = term;
						var objSub = (from t in colUserSubs where t.TermId == term1.TermId select t).SingleOrDefault();
						if (objSub == null)
						{
							_cmdSubscribe.CommandName = "subscribe";
							_cmdSubscribe.Text = Localization.GetString("Subscribe.Text", SharedResourceFile);
							_cmdSubscribe.CommandArgument = term.TermId.ToString();
						}
						else
						{
							_cmdSubscribe.Text = Localization.GetString("Unsubscribe.Text", SharedResourceFile);
							_cmdSubscribe.CommandName = "unsubscribe";
							_cmdSubscribe.CommandArgument = objSub.SubscriptionId.ToString();
						}

						++count;

						if (!_htTags.ContainsKey(term.TermId))
						{
							_htTags.Add(term.TermId, _cmdSubscribe);
							Controls.Add(_cmdSubscribe);
						}
					}
				}

			}

			return count;
		}

		/// <summary>
		/// The Vote event.
		/// </summary>
		/// <remarks>This is normally done behind the scenes by .net, implemented here for performance reasons.</remarks>
		[Category("Action"),
		Description("Raised when the user clicks the subscribe button.")]
		public event EventHandler Subscribe
		{
			add
			{
				Events.AddHandler(EventSubmitKey, value);
			}
			remove
			{
				Events.RemoveHandler(EventSubmitKey, value);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		protected void SubscribeCommand(object source, CommandEventArgs e)
		{
			if (SubscribeClick != null)
			{
				SubscribeClick(this, e);
			}
			var control = (LinkButton)source;
			var action = e.CommandName;
			var id = Convert.ToInt32(e.CommandArgument);

			switch (action)
			{
				case "subscribe":
					SubscribeUser(id, false);
					control.Text = Localization.GetString("Unsubscribe.Text", SharedResourceFile);
					break;
				default:
					SubscribeUser(id, true);
					control.Text = Localization.GetString("Subscribe.Text", SharedResourceFile);
					break;
			}
		}

		/// <summary>
		/// This method renders the entire user interface for this control.
		/// </summary>
		/// <param name="writer"></param>
		/// <returns></returns>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (Terms != null)
			{
				foreach (var term in Terms)
				{
					var link = Links.ViewTaggedQuestions(term.Name, ModuleTab, ModContext.PortalSettings);
					var detaillink = Links.ViewTagDetail(ModContext, ModuleTab.TabID, term.Name);
					//var historylink = Links.ViewTagHistory(ModContext, term.Name);
					var improvelink = Links.EditTag(ModContext, ModuleTab.TabID, term.Name);

					// <div>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaTooltip");
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					// <a />
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tag");
					writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(term.Name);
					writer.RenderEndTag();
					// <div>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tag-menu dnnClear");
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none;");
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					// <div>
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					// <div>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tm-heading");
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					// <span>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tm-sub-info");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					writer.Write(Localization.GetString("Tag", SharedResourceFile));
					writer.Write(term.Name);
					// <span>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tm-sub-links");
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "float:right;");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					if (ModContext.PortalSettings.UserId > 0)
					{
						_cmdSubscribe = (LinkButton)_htTags[term.TermId];
						// <a />
						_cmdSubscribe.RenderControl(writer);
						//// we register this here so that the tooltip is updated after the event action is taken.
						//AJAX.RegisterPostBackControl(_cmdSubscribe);
					}
					// </span>
					writer.RenderEndTag();
					// </span>
					writer.RenderEndTag();
					// </div>
					writer.RenderEndTag();
					// <div />
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tm-description");
					writer.RenderBeginTag(HtmlTextWriterTag.Div);
					writer.Write(term.Description);
					writer.RenderEndTag();
					// </div>
					writer.RenderEndTag();
					// <span>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tm-links");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					//// <a />
					//writer.AddAttribute(HtmlTextWriterAttribute.Href, link);
					//writer.RenderBeginTag(HtmlTextWriterTag.A);
					//writer.Write(Localization.GetString("browse.Text", SharedResourceFile));
					//writer.RenderEndTag();

					// <a />
					writer.AddAttribute(HtmlTextWriterAttribute.Href, detaillink);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(Localization.GetString("about.Text", SharedResourceFile));
					writer.RenderEndTag();

					//// <a />
					//writer.AddAttribute(HtmlTextWriterAttribute.Href, historylink);
					//writer.RenderBeginTag(HtmlTextWriterTag.A);
					//writer.Write(Localization.GetString("history.Text", SharedResourceFile));
					//writer.RenderEndTag();

					// <span>
					writer.AddAttribute(HtmlTextWriterAttribute.Class, "tm-links");
					writer.AddAttribute(HtmlTextWriterAttribute.Style, "float:right;");
					writer.RenderBeginTag(HtmlTextWriterTag.Span);
					// <a />
					writer.AddAttribute(HtmlTextWriterAttribute.Href, improvelink);
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(Localization.GetString("improve.Text", SharedResourceFile));
					writer.RenderEndTag();
					// </span>
					writer.RenderEndTag();

					// </span>
					writer.RenderEndTag();
					// </div>
					writer.RenderEndTag();

					//if (ShowCount)
					//{
					//    // <span>
					//    writer.AddAttribute(HtmlTextWriterAttribute.Class, "percentRight");
					//    writer.RenderBeginTag(HtmlTextWriterTag.Span);
					//    writer.Write(term.DayTermUsage + Localization.GetString("questions", SharedResourceFile));
					//    // </span>
					//    writer.RenderEndTag();
					//}

					// </div>
					writer.RenderEndTag();

					if (CountMode != Constants.TagMode.ShowNoUsage)
					{
						// <span>
						writer.AddAttribute(HtmlTextWriterAttribute.Class, "percentRight");
						writer.RenderBeginTag(HtmlTextWriterTag.Span);

						switch(CountMode)
						{
							case Constants.TagMode.ShowDailyUsage :
								writer.Write(term.DayTermUsage + Localization.GetString("questions", SharedResourceFile));
								break;
							case Constants.TagMode.ShowWeeklyUsage :
								writer.Write(term.WeekTermUsage + Localization.GetString("questions", SharedResourceFile));
								break;
							case Constants.TagMode.ShowMonthlyUsage :
								writer.Write(term.MonthTermUsage + Localization.GetString("questions", SharedResourceFile));
								break;
							case Constants.TagMode.ShowTotalUsage :
								writer.Write(term.TotalTermUsage + Localization.GetString("questions", SharedResourceFile));
								break;
						}
						


						// </span>
						writer.RenderEndTag();
					}
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="remove"></param>
		private void SubscribeUser(int id, bool remove)
		{
			if (id <= 0 || ModContext.PortalSettings.UserId <= 0) return;
			if (remove)
			{
				Controller.DeleteSubscription(ModContext.PortalId, id);
			}
			else
			{
				var objSub = new SubscriptionInfo
								 {
									 PortalId = ModContext.PortalId,
									 UserId = ModContext.PortalSettings.UserId,
									 TermId = id,
									 CreatedOnDate = DateTime.Now,
									 SubscriptionType = (int) Constants.SubscriptionType.InstantTerm
								 };
				Controller.AddSubscription(objSub);
			}
		}

		#endregion

	}
}