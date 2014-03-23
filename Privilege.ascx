<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Privilege.ascx.cs" Inherits="DotNetNuke.DNNQA.Privilege" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="qaPrivilege dnnForm">
	<div class="qaContent dnnLeft">
		<h2 class="dnnFormSectionHead"><asp:Literal ID="litTitle" runat="server" /></h2>
		<asp:Literal ID="litDescription" runat="server" />
	</div>
	<div class="qaSideList dnnRight">
		<h3><asp:Literal ID="litScoreTitle" runat="server" /></h3>
		<p class="qaUserPrivilegeScore">
			<span class="qaUserScore"><asp:Literal ID="litScoreValue" runat="server" /></span>
		</p>
		<p class="qaUserPrivilegeScore">
			<span class="qaCompletionStatus"><asp:Literal ID="litProgress" runat="server" /></span>
		</p>
		<h3><%= Localization.GetString("Privileges", LocalResourceFile) %></h3>
		<ul class="plActions dnnClear">
			<asp:Repeater ID="rptPrivileges" runat="server" OnItemDataBound="RptPrivilegesItemDataBound" EnableViewState="false">
				<ItemTemplate>
					<li><asp:HyperLink ID="hlPrivilege" runat="server" /><span class="percentRight"><asp:Literal ID="litCompletePercent" runat="server" /></span></li>
				</ItemTemplate>
			</asp:Repeater>
		</ul>
	</div>
</div>