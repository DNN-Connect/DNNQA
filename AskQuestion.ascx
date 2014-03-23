<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="AskQuestion.ascx.cs" Inherits="DotNetNuke.DNNQA.AskQuestion" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" Assembly="DotNetNuke.Modules.DNNQA" Namespace="DotNetNuke.DNNQA.Controls" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaAskQuestion" id="qaAskQuestion">
	<h2 class="dnnFormSectionHead"><%= Localization.GetString("litAskHeader", LocalResourceFile) %></h2>
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblTitle" runat="server" controlname="txtTitle" suffix=":" />
			<asp:TextBox ID="txtTitle" runat="server" CssClass="dnnFormRequired qaplaceholder qaAskQuestion" MaxLength="150" TabIndex="0" />
			<asp:RequiredFieldValidator ID="valTitle" runat="server" resourcekey="valTitle.ErrorMessage" Display="Dynamic" ControlToValidate="txtTitle" CssClass="dnnFormMessage dnnFormError" />
			<div class="dnnClear"></div>
			<label></label>
			<div id="divSearchResults" class="qaSimilarPosts">
				<p class="dnnClear"><%= Localization.GetString("Similar", LocalResourceFile) %></p>
				<ul id="questions" class="dnnClear"></ul>
			</div>
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblQuestion" runat="server" controlname="teContent" suffix=":" />
			<div class="dnnLeft">
				<dnn:texteditor id="teContent" runat="server" height="350px" width="500" ></dnn:texteditor>
				<asp:Label ID="valQuestion" runat="server" resourcekey="valQuestion.ErrorMessage" Display="Dynamic" CssClass="dnnFormMessage dnnFormValidationSummary" Visible="false" />
			</div>			
		</div>
		<div class="dnnFormItem" id="divApproved" runat="server" visible="false">
			<dnn:label id="dnnlblApproved" runat="server" controlname="chkApproved" suffix=":" />
			<asp:CheckBox ID="chkApproved" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblNotify" runat="server" controlname="chkNotify" suffix=":" />
			<asp:CheckBox ID="chkNotify" runat="server" />
		</div>
		<asp:Panel class="dnnFormItem" id="pnlTags" runat="server">
			<dnn:label id="dnnlblTags" runat="server" controlname="txtTags" suffix=":" />
			<asp:TextBox ID="txtTags" runat="server" CssClass="dnnFormRequired" />	
		</asp:Panel>
		<asp:Panel class="" ID="pnlHowToAsk" runat="server" style="display: none;">
			<h4><%= Localization.GetString("HowToAskTitle", LocalResourceFile) %></h4>
			<p class="dnnClear"><%= Localization.GetString("HowToAsk", LocalResourceFile) %></p>
		</asp:Panel>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton ID="cmdSave" runat="server" resourcekey="cmdSave" CssClass="dnnPrimaryAction" OnClick="CmdSaveClick" /></li>
		<li><asp:HyperLink ID="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="dnnSecondaryAction" /></li>
	</ul>
</div>
<dnnweb:DnnCodeBlock ID="cbEditPost" runat="server">
	<script language="javascript" type="text/javascript">
		/*globals jQuery, window, Sys */
		var mydata;
		(function ($, Sys) {
			function setupAskQuestion() {
				var testEvent;
				var testUI;

				$("#divSearchResults").hide();
				$("#<%= txtTitle.ClientID  %>").blur(function () { searchQuestions($("#<%= txtTitle.ClientID  %>").val()); });

				function split(val) {
					return val.split(/,\s*/);
				}

				function extractLast(term) {
					return split(term).pop();
				}

				var myTextArea = $('#<%= txtTags.ClientID  %>').tagify( {delimiters: [9, 13, 44, 59, 188], addTagPrompt: '<%= Localization.GetString("AddTags", LocalResourceFile) %>' } ); // tab, return, comma, semicolon
				myTextArea.tagify('inputField').autocomplete({
					source: function (request, response) {
								$.ajax({
									type:"POST",
									url:'<%= ResolveUrl("~/DesktopModules/DNNQA/QA.asmx/SearchTags")%>',
									data:"{'searchTerm' : '" + extractLast(request.term) + "'}",
									contentType: "application/json",
									dataType: "json",
									success: function(data){
										var suggestions=[];
										mydata=data;
										$.each($.parseJSON(data.d),function(i,val){
											suggestions.push(val);
										});
										response(suggestions);
									}
								});
							},			
					minLength: 2,
					close: function(event, ui) { myTextArea.tagify('add'); myTextArea.tagify('serialize'); }
				});

				function searchQuestions(searchPhrase) {
					if(searchPhrase!="" && searchPhrase!="<%= Localization.GetString("overlayTitle", LocalResourceFile) %>"){
						var moduleId = <%= ModuleContext.ModuleId %>;
						$.ajax({
							type: "POST",
							url: '<%= ResolveUrl("~/DesktopModules/DNNQA/Qa.asmx/SearchQuestionTitle") %>',
							data: "{ 'moduleId' : '" + moduleId + "' , 'searchPhrase' : '" + searchPhrase + "' }",
							contentType: "application/json",
							dataType: "json",
							success: function (data) {
								$("#questions").empty();
								if(data.d.length > 0 )
								{
									$("#questionsTemplate").tmpl(data.d).appendTo("#questions");
									// run the toggle effect
									var options = {};
									$("#divSearchResults").show('blind', options, 500);
								}
								else{
									$("#divSearchResults").hide();
								}
							},
							error: function (xhr, status, error) {
								if(error!="")alert(error);
							}
						});
					}
					
				};
			};

			$(document).ready(function () {
				setupAskQuestion();
				Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
					setupAskQuestion();
				});
			});

		} (jQuery, window.Sys));
	</script>
	<script id="questionsTemplate" type="text/html">
		<li>
			<span class="answercount">
				${QuestionVotes}
			</span>
			<span class="summary">
				<a href="${QuestionUrl}" target="_blank">${Title}</a>&nbsp;&nbsp;(${TotalAnswers})
			</span>
		</li>
	</script>
</dnnweb:DnnCodeBlock>