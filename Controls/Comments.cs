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
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using System;
using DotNetNuke.DNNQA.Components.Common;
using DotNetNuke.DNNQA.Components.Controllers;
using DotNetNuke.DNNQA.Components.Entities;
using DotNetNuke.DNNQA.Components.Integration;
using DotNetNuke.DNNQA.Providers.Data.SqlDataProvider;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;
using System.Text.RegularExpressions;

namespace DotNetNuke.DNNQA.Controls
{

	/// <summary>
	/// The comments control contains a comment input form and a list view of comments.
	/// </summary>
	[DefaultEvent("Submit"), DefaultProperty("Width"), Themeable(false)]
	[ToolboxData("<{0}:Comments runat=\"server\"> </{0}:Comments>")]
	public class Comments : CompositeControl 
	{
	
		#region Private Members

		protected IDnnqaController Controller { get; private set; }

		private LinkButton _cmdAddComment;
		private TextBox _txtComment;
		private static readonly object EventSubmitKey = new object();

		/// <summary>
		/// This provides a full path to the shared resource file for localization. 
		/// </summary>
		private string SharedResourceFile
		{
			get { return ResolveUrl("~/DesktopModules/DNNQA/App_LocalResources/SharedResources.resx"); }
		}

		#endregion

		#region Public Properties

		[Browsable(false)]
		public ModuleInstanceContext ModContext { get; set; }

		public PostInfo ObjPost { get; set; }

		public int CurrentUserScore { get; set; }

		public List<QaSettingInfo> Privileges { get; set; }

		public QuestionInfo Question { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public Comments()
			: this(new DnnqaController(new SqlDataProvider()))
		{

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="controller"></param>
		public Comments(IDnnqaController controller)
		{
			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		protected override void RecreateChildControls()
		{
			EnsureChildControls();
		}

		/// <summary>
		/// Adds the necessary control(s) to the controls collection.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			Controls.Clear();

			_txtComment = new TextBox {TextMode = TextBoxMode.MultiLine, ID = String.Concat(ID, "_CommentTxt")};

			_cmdAddComment = new LinkButton {ID = String.Concat(ID, "_SubmitBtn")};
			_cmdAddComment.Click += CmdAddCommentClick;
			_cmdAddComment.Text = Localization.GetString("AddComment.Text", SharedResourceFile);
			_cmdAddComment.CssClass = "dnnPrimaryAction";

			Controls.Add(_txtComment);
			Controls.Add(_cmdAddComment);
		}

		/// <summary>
		/// Creates the user interface.
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(HtmlTextWriter writer)
		{
			// <div> -- entire area
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			// <ul>
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaComments");
			writer.RenderBeginTag(HtmlTextWriterTag.Ul);
			// get data
			var postComments = Controller.GetPostComments(ObjPost.PostId);

			if (postComments != null)
			{
				foreach (var comment in postComments)
				{
					// <li>
					writer.RenderBeginTag(HtmlTextWriterTag.Li);

					var matches = new Regex("(?<![\">])((http|https|ftp)\\://.+?)(?=\\s|$)").Matches(comment.Comment);

					foreach(Match m in matches){
						comment.Comment = comment.Comment.Replace(m.Value, "<a rel=\"nofollow\" href=\"" + m.Value + "\">" + m.Value + "</a>");
					}

					writer.Write("<p>" + comment.Comment + " - ");
					
					// <a />
					var objUser = Entities.Users.UserController.GetUserById(ModContext.PortalId, comment.UserId);
					writer.AddAttribute(HtmlTextWriterAttribute.Href, Common.Globals.UserProfileURL(objUser.UserID));
					writer.RenderBeginTag(HtmlTextWriterTag.A);
					writer.Write(objUser.DisplayName);
					writer.RenderEndTag();

					// comment date
					writer.Write(" " + Utils.CalculateDateForDisplay(comment.CreatedOnDate));
					writer.Write("</p>");

					// </li>
					writer.RenderEndTag();
				}
			}

			// </ul>
			writer.RenderEndTag();

			var objCommentE = Privileges.Single(s => s.Key == Constants.Privileges.CommentEverywhere.ToString());
			var objRemoveUser = Privileges.Single(s => s.Key == Constants.Privileges.RemoveNewUser.ToString());

			if (CurrentUserScore >= objCommentE.Value || ModContext.IsEditable || ((ObjPost.CreatedUserId == ModContext.PortalSettings.UserId) && (CurrentUserScore >= objRemoveUser.Value)) || ((Question.CreatedUserId == ModContext.PortalSettings.UserId) && (CurrentUserScore >= objRemoveUser.Value)))
			{
				// <div> - comment expand/collapse and fieldset container
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "qaCommentArea");
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				// <h2>
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "dnnFormSectionHead");
				writer.RenderBeginTag(HtmlTextWriterTag.H2);
				// <a />
				writer.AddAttribute(HtmlTextWriterAttribute.Id, "comment-postid-" + ObjPost.PostId);
				writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write(Localization.GetString("CommentHeader.Text", SharedResourceFile));
				writer.RenderEndTag();
				// </h2>
				writer.RenderEndTag();

				// <fieldset>
				writer.RenderBeginTag(HtmlTextWriterTag.Fieldset);

				// <div> - dnnFormItem
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "dnnFormItem");
				writer.RenderBeginTag(HtmlTextWriterTag.Div);
				// <input />
				_txtComment.RenderControl(writer);
				// </div> - dnnFormItem
				writer.RenderEndTag();

				// add comment link button
				_cmdAddComment.RenderControl(writer);

				// </fieldset>
				writer.RenderEndTag();
				// </div> - comment expand/collapse and fieldset container
				writer.RenderEndTag();
			}
			// </div> -- entire area
			writer.RenderEndTag();
		}

		[Category("Action"),
Description("Raised when the user clicks the add comment button.")]
		public event EventHandler AddComment
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
		/// The method that raises the Submit event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnAddComment(EventArgs e)
		{
			var submitHandler = (EventHandler)Events[EventSubmitKey];
			if (submitHandler != null)
			{
				submitHandler(this, e);
			}

			SaveComment();
		}

		/// <summary>
		/// Handles the Click event of the Button and raises the AddComment event.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		private void CmdAddCommentClick(object source, EventArgs e)
		{
			OnAddComment(EventArgs.Empty);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Will save the comment to the data store, as long as it has some text value.
		/// </summary>
		private void SaveComment()
		{
			if (_txtComment.Text.Trim().Length > 1)
			{
				var comment = Utils.ProcessSavePostBody(_txtComment.Text);
				comment = comment.Replace("\n", "<br />");

				var objComment = new CommentInfo
									 {
										 Comment = comment,
										 CreatedOnDate = DateTime.Now,
										 PostId = ObjPost.PostId,
										 UserId = ModContext.PortalSettings.UserId
									 };

				objComment.CommentId = Controller.AddComment(objComment);

				var questionUrl = Links.ViewQuestion(Question.PostId, Question.Title, ModContext.PortalSettings.ActiveTab, ModContext.PortalSettings);
				var cntJournal = new Journal();
				cntJournal.AddCommentToJournal(Question, objComment, Question.Title, ModContext.PortalId, ModContext.PortalSettings.UserId, questionUrl);
			}

			// we should consider an else to give end user feedback

			_txtComment.Text = "";
		}

		#endregion

	}
}