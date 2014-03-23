<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Question.ascx.cs" Inherits="DotNetNuke.DNNQA.Question" %>
<%@ Register TagPrefix="dqa" Assembly="DotNetNuke.Modules.DNNQA" Namespace="DotNetNuke.DNNQA.Controls" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnqaQuestion dnnForm">
	<h1><asp:Literal ID="litTitle" runat="server" /></h1>
	<div class="question">
		<table id="tblQuestion">
			<tbody>
				<tr>
					<td class="qaqVote"><dqa:Voting id="dqaQuestionVote" runat="server" VotingMode="Question" /></td>
					<td class="qaqBody">
						<asp:Literal ID="litBody" runat="server" />
						<div class="tags"><dqa:Tags ID="dqaTag" runat="server" /></div>
						<ul class="qaModActions" id="questionMenu" runat="server" visible="false">
							<asp:Literal ID="litEditQuestion" runat="server" />
							<asp:Literal ID="litFlagQuestion" runat="server" />
							<asp:Literal ID="litCloseQuestion" runat="server" />
							<asp:Literal ID="litProtect" runat="server" />
							<asp:Literal ID="litDeleteQuestion" runat="server" />
						</ul>
						<ul class="qaSocialActions">
							<asp:Literal ID="litSocialSharing" runat="server" />
						</ul>
					</td>
				</tr>
				<tr>
					<td class="qaqVote"></td>
					<td class="qaqFooter">
						<div class="qaqActivity">
							<div class="dnnLeft"><asp:Literal ID="litAsked" runat="server" /></div>
							<div class="dnnRight"><dnnweb:DnnBinaryImage ID="dbiUser" runat="server" Width="50" /></div>
						</div>
						<div class="qaqActivity">
							<div class="dnnLeft"><asp:Literal ID="litEdited" runat="server" /></div>
							<div class="dnnRight"><dnnweb:DnnBinaryImage ID="dbiEditUser" runat="server" Width="50" /></div>
						</div>
					</td>
				</tr>
				<tr>
					<td class="qaqVote"></td>
					<td class="qaqComments" id="qaQComments">
						<dqa:Comments ID="qaQuestionComment" runat="server" />
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<h2 class="dnnFormSectionHead"><asp:Literal ID="litAnswerCount" runat="server" /></h2>
	<div class="qatlRight">
		<ul class="qaSortActions dnnClear">
			<li><asp:HyperLink ID="hlOldest" runat="server" resourcekey="hlOldest" /></li>
			<li><asp:HyperLink ID="hlActive" runat="server" resourcekey="hlActive" /></li>
			<li><asp:HyperLink ID="hlVotes" runat="server" resourcekey="hlVotes" /></li>
		</ul>
	</div>
	<div class="answers">
		<asp:Repeater ID="rptAnswers" runat="server" OnItemDataBound="RptAnswersItemDataBound">
			<HeaderTemplate>
				<table id="tblAnswers">
					<tbody>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td class="qaqVote">
						<dqa:Voting id="dqaAnswerVote" runat="server" VotingMode="Answer" />
						<asp:Image ID="imgAccepted" runat="server" />
					</td>
					<td class="qaqBody">
						<asp:Literal ID="litBody" runat="server" />
						<div class="tags" />
						<ul class="qaModActions" id="ulAnswersMenu" runat="server" visible="false">
							<asp:Literal ID="litEditAnswer" runat="server" />
							<asp:Literal ID="litFlagAnswer" runat="server" />
							<li><asp:LinkButton ID="cmdAccept" runat="server" resourcekey="cmdAccept" OnClick="CmdAcceptClick" /></li>
							<asp:Literal ID="litDeleteAnswer" runat="server" />
						</ul>
					</td>
				</tr>
				<tr class="qaAnswerFooter">
					<td class="qaqVote"></td>
					<td class="qaqFooter">
						<div class="qaqActivity">
							<div class="dnnLeft"><asp:Literal ID="litDate" runat="server" /></div>
							<div class="dnnRight"><dnnweb:DnnBinaryImage ID="dbiAUser" runat="server" Width="50" /></div>
						</div>
						<div class="qaqActivity">
							<div class="dnnLeft"><asp:Literal ID="litAEdited" runat="server" /></div>
							<div class="dnnRight"><dnnweb:DnnBinaryImage ID="dbiAEditUser" runat="server" Width="50" /></div>
						</div>
					</td>
				</tr>
				<tr>
					<td class="qaqVote"></td>
					<td class="qaqComments">
						<dqa:Comments ID="qaAnswerComment" runat="server" />
					</td>
				</tr>
			</ItemTemplate>
			<FooterTemplate>
					</tbody>
				</table>
			</FooterTemplate>
		</asp:Repeater>
		<div class="qaPager">
			<ul class="qaPageActions dnnClear">
				<li><asp:LinkButton ID="cmdMore" runat="server" resourcekey="cmdMore" OnCommand="CmdPagingClick" CommandName="up" CssClass="dnnPrimaryAction" /></li>
				<li><asp:LinkButton ID="cmdBack" runat="server" resourcekey="cmdBack" OnCommand="CmdPagingClick" CommandName="down" CssClass="dnnPrimaryAction" /></li>
			</ul>
		</div>
	</div>
	<div class="qaLogin" id="divLogin" runat="server" visible="false">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("YourAnswer", LocalResourceFile) %></h2>
		<div class="dnnFormMessage dnnFormWarning">
			<%= Localization.GetString("AnswerLogin", LocalResourceFile) %><asp:HyperLink ID="hlLogin" runat="server" resourcekey="hlLogin" />
		</div>
	</div>
	<div id="divAddAnswer" runat="server">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("YourAnswer", LocalResourceFile) %></h2>
		<div class="dnnFormItem"><div class="dnnLeft"><dnn:texteditor id="teContent" runat="server" height="350px" width="550"></dnn:texteditor></div></div>
		<ul class="dnnActions">
			<li><asp:LinkButton ID="cmdSave" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdSave" OnClick="CmdSaveClick" /></li>
			<li><asp:LinkButton ID="cmdSubscribe" runat="server" CssClass="dnnSecondaryAction" OnClick="CmdSubscribeClick" /></li>
		</ul>
	</div>
	<div id="divQuestionFlag" class="qaDialog">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("FlagQPopupHeader", LocalResourceFile) %></h2>
		<div class="options">
			<asp:RadioButtonList ID="rblstFlagQuestion" runat="server" Width="200" />
			<asp:TextBox ID="txtFlagQuestionOther" runat="server" TextMode="MultiLine" Width="350" Height="100" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:LinkButton ID="cmdFlagQuestion" runat="server" resourcekey="cmdFlag" CssClass="dnnPrimaryAction" OnClick="CmdFlagQuestionClick" /></li>
		</ul>
		<div class="qaInform"><asp:Literal ID="litQInform" runat="server" /></div>
	</div>
	<div id="divQuestionDelete" class="qaDialog">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("DeleteQPopupHeader", LocalResourceFile) %></h2>
		<div class="options">
			<asp:RadioButtonList ID="rblstDeleteQuestion" runat="server" Width="200" />
			<asp:TextBox ID="txtDeleteQuestionOther" runat="server" TextMode="MultiLine" Width="350" Height="100" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:LinkButton ID="cmdDeleteQuestion" runat="server" resourcekey="cmdDelete" CssClass="dnnPrimaryAction" OnClick="CmdDeleteQuestionClick" /></li>
		</ul>
	</div>
	<div id="divAnswerFlag" class="qaDialog">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("FlagAPopupHeader", LocalResourceFile)%></h2>
		<div class="options">
			<asp:RadioButtonList ID="rblstAnswerFlag" runat="server" Width="200" />
			<asp:TextBox ID="txtbxAnswerFlag" runat="server" TextMode="MultiLine" Width="350" Height="100" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:LinkButton ID="cmdAnswerFlag" runat="server" resourcekey="cmdFlag" CssClass="dnnPrimaryAction" OnClick="CmdFlagAnswerClick" /></li>
		</ul>
		<div class="qaInform"><asp:Literal ID="litAInform" runat="server" /></div>
	</div>
	<div id="divAlreadyFlagged" class="qaDialog">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("AlreadyFlaggedPopupHeader", LocalResourceFile)%></h2>
		<div class="options dnnFormMessage dnnFormWarning">
			<p><%= Localization.GetString("AlreadyFlagged", LocalResourceFile) %></p>
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:LinkButton ID="cmdAlreadyFlagged" runat="server" resourcekey="cmdUndoFlag" CssClass="dnnPrimaryAction" OnClick="CmdUndoFlagClick" /></li>
		</ul>
		<div class="qaInform"></div>
	</div>
	<div id="divAnswerDelete" class="qaDialog">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("DeleteAPopupHeader", LocalResourceFile)%></h2>
		<div class="options">
			<asp:RadioButtonList ID="rblstAnswerDelete" runat="server" Width="200" />
			<asp:TextBox ID="txtAnswerDelete" runat="server" TextMode="MultiLine" Width="350" Height="100" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:LinkButton ID="cmdAnswerDelete" runat="server" resourcekey="cmdDelete" CssClass="dnnPrimaryAction" OnClick="CmdDeleteAnswerClick" /></li>
		</ul>
	</div>
</div>
<asp:TextBox ID="tempId" runat="server" Visible="true" style="display:none;" />
<dnnweb:DnnCodeBlock ID="dcbQuestions" runat="server" >
	<script language="javascript" type="text/javascript">
		/*globals jQuery, window, Sys */
		(function ($, Sys) {
		    function setupDnnAnswers() {
		        $('.qaCommentArea').dnnPanels();

		        var po = document.createElement('script');
		        po.type = 'text/javascript';
		        po.async = true;
		        po.src = 'https://apis.google.com/js/plusone.js';
		        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);

		        $('#divQuestionFlag').dialog({ autoOpen: false, minWidth: 450, title: '<%= Localization.GetString("FlagQDialogTitle", LocalResourceFile) %>' });
		        $('#divAnswerFlag').dialog({ autoOpen: false, minWidth: 450, title: '<%= Localization.GetString("FlagADialogTitle", LocalResourceFile) %>' });
		        $('#divQuestionDelete').dialog({ autoOpen: false, minWidth: 450, title: '<%= Localization.GetString("DeleteQDialogTitle", LocalResourceFile) %>' });
		        $('#divAnswerDelete').dialog({ autoOpen: false, minWidth: 450, title: '<%= Localization.GetString("DeleteADialogTitle", LocalResourceFile) %>' });
		        $('#divAlreadyFlagged').dialog({ autoOpen: false, minWidth: 450, title: '<%= Localization.GetString("AlreadyFlaggedDialogTitle", LocalResourceFile) %>' });
		        $('#flagQuestion').click(function () {
		            $('#divQuestionDelete').dialog('close');
		            $('#divAnswerFlag').dialog('close');
		            $('#divAnswerDelete').dialog('close');
		            $('#divAlreadyFlagged').dialog('close');
		            $('#divQuestionFlag').dialog('open');
		            // prevent the default action, e.g., following a link
		            return false;
		        });
		        $('.flagAnswer').click(function () {
		            var postid = jQuery(this).attr("postid");
		            $("#<%= tempId.ClientID %>").val(postid);
		            $('#divQuestionFlag').dialog('close');
		            $('#divQuestionDelete').dialog('close');
		            $('#divAnswerDelete').dialog('close');
		            $('#divAlreadyFlagged').dialog('close');
		            $('#divAnswerFlag').dialog('open');
		            // prevent the default action, e.g., following a link
		            return false;
		        });
		        $('.undoFlag').click(function () {
		            var postid = jQuery(this).attr("postid");
		            $("#<%= tempId.ClientID %>").val(postid);
		            $('#divQuestionFlag').dialog('close');
		            $('#divQuestionDelete').dialog('close');
		            $('#divAnswerDelete').dialog('close');
		            $('#divAnswerFlag').dialog('close');
		            $('#divAlreadyFlagged').dialog('open');
		            // prevent the default action, e.g., following a link
		            return false;
		        });
		        $('#deleteQuestion').click(function () {
		            $('#divQuestionFlag').dialog('close');
		            $('#divAnswerFlag').dialog('close');
		            $('#divAnswerDelete').dialog('close');
		            $('#divAlreadyFlagged').dialog('close');
		            $('#divAlreadyFlagged').dialog('close');
		            $('#divQuestionDelete').dialog('open');
		            // prevent the default action, e.g., following a link
		            return false;
		        });
		        $('.deleteAnswer').click(function () {
		            var postid = jQuery(this).attr("postid");
		            $("#<%= tempId.ClientID %>").val(postid);
		            $('#divQuestionDelete').dialog('close');
		            $('#divAnswerFlag').dialog('close');
		            $('#divQuestionFlag').dialog('close');
		            $('#divAlreadyFlagged').dialog('close');
		            $('#divAnswerDelete').dialog('open');
		            // prevent the default action, e.g., following a link
		            return false;
		        });

		        MonitorAnswers();
		    };

		    function MonitorAnswers() {
		        $.ajax({
		            type: "POST",
		            url: '<%= ResolveUrl("~/DesktopModules/DNNQA/Qa.asmx/GetLastQuestionPostId") %>',
		            data: "{ 'postId' : '" + $("#PostId").text() + "'}",
		            contentType: "application/json",
		            dataType: "json",
		            success: function (data) {
		                if (data.d > $("#lastAnswerId").text()) {
		                    var refresh = '<%= Localization.GetString("Refresh", LocalResourceFile) %>';
		                    $("#divMessage span").html(refresh);
		                }
		                else {
		                    setTimeout(function () { MonitorAnswers(); }, 10000);
		                }
		            },
		            error: function (xhr, status, error) {
		                if (error != "") alert(error);
		            }
		        });


		        !function (d, s, id) {
		            var js, fjs = d.getElementsByTagName(s)[0];
		            if (!d.getElementById(id)) {
		                js = d.createElement(s); js.id = id; js.src = "//platform.twitter.com/widgets.js";
		                fjs.parentNode.insertBefore(js, fjs);
		            }
		        } (document, "script", "twitter-wjs");

		        !function (d, s, id) {
		            var js, fjs = d.getElementsByTagName(s)[0];
		            if (d.getElementById(id)) return;
		            js = d.createElement(s); js.id = id;
		            js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
		            fjs.parentNode.insertBefore(js, fjs);
		        } (document, 'script', 'facebook-jssdk');

		    }

		    $(document).ready(function () {
		        setupDnnAnswers();
		        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
		            setupDnnAnswers();

		            var enableTwitter = '<%= Model.EnableTwitter %>';
		            if (enableTwitter = true) {
		                twttr.widgets.load();
		            }

		            var enableFacebook = '<%= Model.FacebookAppId %>';
		            if (enableFacebook.length > 0) {
		                FB.XFBML.parse();
		            }
		            
		            var enableLinkedIn = '<%= Model.EnableLinkedIn %>';
		            if (enableLinkedIn = true) {
		                IN.parse();
		            }

		        });
		    });

		} (jQuery, window.Sys));
	</script>  
</dnnweb:DnnCodeBlock>
<div style="display:none;" id="lastAnswerId"><%= LastAnswerPostId %></div>
<div style="display:none;" id="PostId"><%= Model.Question.PostId %></div>