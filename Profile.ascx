<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Profile.ascx.cs" Inherits="DotNetNuke.DNNQA.Profile" %>
<%@ Register TagPrefix="dqa" Assembly="DotNetNuke.Modules.DNNQA" Namespace="DotNetNuke.DNNQA.Controls" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<div class="dnnForm qaProfile dnnClear" id="qaProfile">
	<ul class="dnnAdminTabNav dnnClear">
		<li><a href="#qaMyQuestions"><%=LocalizeString("MyQuestions") %></a></li>
		<li><a href="#qaMyAnswers"><%=LocalizeString("MyAnswers") %></a></li>
		<li id="liRep" runat="server"><a href="#<%= pnlMyReputation.ClientID %>"><%=LocalizeString("MyReputation") %></a></li>
		<li id="liVs" runat="server"><a href="#<%= pnlFriendsVs.ClientID %>"><%=LocalizeString("Vs")%></a></li>
	</ul>
	<div class="qaInnerTab" id="qaMyQuestions">
		<asp:Panel ID="pnlNoQuestions" runat="server" CssClass="dnnFormMessage dnnFormInfo">
			<asp:Literal ID="litNoQuestions" runat="server" />
		</asp:Panel>
		<asp:Repeater ID="rptQuestions" runat="server" EnableViewState="false" OnItemDataBound="RptQuestionsItemDataBound">
			<HeaderTemplate><div class="qaPostsList" id="qaMyQ"><ul id="questions" class="dnnClear"></HeaderTemplate>
			<ItemTemplate>
				<li><span class="answercount"><asp:Literal ID="litAnswers" runat="server" /></span><span class="summary"><asp:HyperLink ID="hlTitle" runat="server" /></span></li>
			</ItemTemplate>
			<FooterTemplate></ul></div></FooterTemplate>
		</asp:Repeater>		
	</div>
	<div class="qaInnerTab" id="qaMyAnswers">
		<asp:Panel ID="pnlNoAnswers" runat="server" CssClass="dnnFormMessage dnnFormInfo">
			<asp:Literal ID="litNoAnswers" runat="server" />
		</asp:Panel>
		<asp:Repeater ID="rptAnswers" runat="server" EnableViewState="false" OnItemDataBound="RptQuestionsItemDataBound">
			<HeaderTemplate><div class="qaPostsList" id="qaMyA"><ul id="questions" class="dnnClear"></HeaderTemplate>
			<ItemTemplate>
				<li><span class="answercount"><asp:Literal ID="litAnswers" runat="server" /></span><span class="summary"><asp:HyperLink ID="hlTitle" runat="server" /></span></li>
			</ItemTemplate>
			<FooterTemplate></ul></div></FooterTemplate>
		</asp:Repeater>
	</div>
	<asp:Panel class="qaInnerTab" id="pnlMyReputation" runat="server">
		<asp:Panel ID="pnlNoRep" runat="server" CssClass="dnnFormMessage dnnFormInfo">
			<asp:Literal ID="litNoRep" runat="server" />
		</asp:Panel>
		<asp:Repeater ID="rptReputation" runat="server" EnableViewState="false" OnItemDataBound="RptReputationItemDataBound">
			<HeaderTemplate><div class="qaPostsList" id="qaMyR"><ul id="questions" class="dnnClear"></HeaderTemplate>
			<ItemTemplate>
				<li><span class="answercount"><asp:Literal ID="litPoints" runat="server" /></span><span class="summary"><asp:HyperLink ID="hlTitle" runat="server" /></span></li>
			</ItemTemplate>
			<FooterTemplate></ul></div></FooterTemplate>
		</asp:Repeater>
	</asp:Panel>
	<asp:Panel class="qaInnerTab" id="pnlFriendsVs" runat="server">
		<asp:Panel ID="pnlNoFriends" runat="server" CssClass="dnnFormMessage dnnFormInfo">
			<asp:Literal ID="litNoFriends" runat="server" />
		</asp:Panel>	
		<asp:Repeater ID="rptVs" runat="server" EnableViewState="false" OnItemDataBound="RptFriendsItemDataBound">
			<ItemTemplate>
				<div class="vsRow dnnClear">
					<div class="dnnLeft">
						<asp:HyperLink ID="hlUser" runat="server">
							<dnnweb:DnnBinaryImage ID="dbiUser" runat="server" Width="40" />
						</asp:HyperLink>
					</div>
					<div class="dnnRight">
						<asp:Literal ID="litDetails" runat="server" />
					</div>
				</div>
			</ItemTemplate>
			<SeparatorTemplate>
				<hr />
			</SeparatorTemplate>
		</asp:Repeater>
	</asp:Panel>
</div>
<script language="javascript" type="text/javascript">
	/*globals jQuery, window, Sys */
	(function ($, Sys) {
		function setupQaProfile() {
			$('#qaProfile').dnnTabs();
			$('.qaTooltip').qaTooltip();
		}

		$(document).ready(function () {
			setupQaProfile();
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
				setupQaProfile();
			});
		});

	} (jQuery, window.Sys));
</script>  