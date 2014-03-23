<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Badge.ascx.cs" Inherits="DotNetNuke.DNNQA.Badge" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaBadge" id="qaBadge">
	<h2 class="dnnFormSectionHead"><%= Localization.GetString("Badge", LocalResourceFile)%></h2>
	<div class="qaBadgeDetails dnnClear">
		<div><asp:Literal ID="litAwarded" runat="server" /></div>
		<div>
			<asp:Literal ID="litBadge" runat="server" />
		</div>
		<div><asp:Literal ID="litDescription" runat="server" /></div>           
	</div>
	<div>
		<span class="qaUserScore"><asp:Literal ID="litMultiplier" runat="server" /></span>
		<%= Localization.GetString("UsersEarned", LocalResourceFile)%>
	</div>
	<div class="qaBadgeUsers">
		<asp:Repeater ID="rptUsers" runat="server">
			
		</asp:Repeater>
	</div>
</div>