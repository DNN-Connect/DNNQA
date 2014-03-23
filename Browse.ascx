<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Browse.ascx.cs" Inherits="DotNetNuke.DNNQA.Browse" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" Assembly="DotNetNuke.Modules.DNNQA" Namespace="DotNetNuke.DNNQA.Controls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="qaBrowse dnnForm">
	<div class="qaContent dnnLeft">
		<h2 class="dnnFormSectionHead"><asp:Literal ID="litTitle" runat="server" /></h2>
		<div class="qatlRight">
			<ul class="qaSortActions dnnClear">
				<li><asp:HyperLink ID="hlNewest" runat="server" resourcekey="hlNewest" /></li>
				<li><asp:HyperLink ID="hlVotes" runat="server" resourcekey="hlVotes" /></li>
				<li><asp:HyperLink ID="hlActive" runat="server" resourcekey="hlActive" /></li>
			</ul>
		</div>
		<asp:Panel ID="pnlTag" runat="server" Visible="false">
			<div class="qaTagInfoTagDetails">
				<h2><%= Localization.GetString("Title", LocalResourceFile) %> // <span><%= Localization.GetString("About", LocalResourceFile) %>&nbsp;<%= Model.SelectedTerm.Name %></span></h2>
			</div>
			<div class="tagDescription">
				<p><%= Model.SelectedTerm.Description %></p>
				<ul class="qaNavActions dnnClear">
					<li><asp:HyperLink ID="hlDetail" runat="server" resourcekey="hlDetail" CssClass="dnnSecondaryAction" /></li>
					<li><asp:HyperLink ID="hlEdit" runat="server" resourcekey="hlEdit" CssClass="dnnSecondaryAction" /></li>
				</ul>
			</div>
		</asp:Panel>
		<asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="RptQuestionsItemDataBound" EnableViewState="false">
			<ItemTemplate>
				<div class="qaQuestion">
					<div class="qnotes dnnLeft">
						<h3><asp:HyperLink ID="hlTitle" runat="server" /><asp:Image ID="imgAccepted" runat="server" Visible="false" /></h3>
						<div class="started"><asp:Literal ID="litDate" runat="server" /></div>
						<div class="tags"><dqa:Tags ID="dqaTag" runat="server" /></div>
					</div>
					<div class="qstats dnnRight">
						<asp:Panel ID="pnlAnswers" runat="server" CssClass="answers">
							<asp:Literal ID="litAnswers" runat="server" />
							<asp:Literal ID="litAnswersText" runat="server" />
						</asp:Panel>
						<div class="votes">
							<span class="count"><asp:Literal ID="litVotes" runat="server" /></span>
							<span><%= Localization.GetString("Votes.Text", LocalResourceFile) %></span>
						</div>
						<div class="views">
							<span class="count"><asp:Literal ID="litViews" runat="server" /></span>
							<span><%= Localization.GetString("Views.Text", LocalResourceFile) %></span>
						</div>
					</div>
				</div>
			</ItemTemplate>
		</asp:Repeater>
		<asp:Panel ID="pnlNoRecords" runat="server" CssClass="dnnFormMessage dnnFormWarning">
			<%= Localization.GetString("NoRecords", LocalResourceFile) %>
		</asp:Panel>
		<div class="qaPager">
			<ul class="qaPageActions dnnClear">
				<li><asp:LinkButton ID="cmdMore" runat="server" resourcekey="cmdMore" OnClick="CmdPagingClick" CommandName="up" CssClass="dnnPrimaryAction" /></li>
				<li><asp:LinkButton ID="cmdBack" runat="server" resourcekey="cmdBack" OnClick="CmdPagingClick" CommandName="down" CssClass="dnnPrimaryAction" /></li>
			</ul>
		</div>
	</div>
	<div class="qaSideList dnnRight">
		<h3 id="headMyDashboard" runat="server"><%= Localization.GetString("MyDashboard", LocalResourceFile) %></h3>
		<ul class="plActions dnnClear myDashboardNumbers" id="ulMyDashboard" runat="server">
			<li><asp:HyperLink id="hlMyQuestions" runat="server" resourcekey="hlMyQuestions" /><span><asp:Literal ID="litQuestionCount" runat="server" /></span></li>
			<li><asp:HyperLink id="hlMyAnswers" runat="server" resourcekey="hlMyAnswers"  /><span><asp:Literal ID="litAnswerCount" runat="server" /></span></li>
			<li><asp:HyperLink id="hlMySubscriptions" runat="server" resourcekey="hlMySubscriptions" /><span><asp:Literal ID="litSubscriptionCount" runat="server" /></span></li>
			<li><asp:HyperLink ID="hlPrivileges" runat="server" resourcekey="hlPrivileges" /><span><asp:Literal ID="litUserScore" runat="server" /></span></li>
		</ul>
		<h3 id="headFavoriteTags" runat="server"><%= Localization.GetString("FavoriteTags", LocalResourceFile) %></h3>
		<ul class="qaRecentTags dnnClear" id="ulFavoriteTags" runat="server">
			<asp:Repeater ID="rptFavoriteTags" runat="server" EnableViewState="false">
				<ItemTemplate><li class="rtTag"><a>Tag Name<span class="deleteTag" /></a></li></ItemTemplate>
			</asp:Repeater>
		</ul>
		<h3><%= Localization.GetString("RelatedTags", LocalResourceFile) %></h3>
		<ul class="qaRecentTags dnnClear">
			<asp:Repeater ID="rptTags" runat="server" OnItemDataBound="RptTagsItemDataBound" EnableViewState="false">
				<ItemTemplate>
					<li class="rtTag"><dqa:Tags ID="dqaSingleTag" runat="server" EnableViewState="false" /></li>
				</ItemTemplate>
			</asp:Repeater>
			<li class="rtTag"><asp:HyperLink ID="hlAllTags" runat="server" resourcekey="hlAllTags" /></li>
		</ul>	
	</div>
</div>