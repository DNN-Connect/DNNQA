<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Badges.ascx.cs" Inherits="DotNetNuke.DNNQA.Badges" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaBadges" id="qaBadges">
	<div class="qaContent dnnLeft">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("BadgesMetaTitle", LocalResourceFile)%></h2>
		<div class="qatlRight">
			<ul class="qaSortActions dnnClear">
				<li></li>
			</ul>
		</div>
		<div class="badgeDescription"><%= Localization.GetString("BadgesMetaDescription", LocalResourceFile)%></div>
		<div class="qaBadgeList">
			<asp:Repeater ID="rptBadges" runat="server" OnItemDataBound="RptBadgesItemDataBound" EnableViewState="false">
				<HeaderTemplate>
					<table class="dnnGrid">
						<tbody>
				</HeaderTemplate>
				<ItemTemplate>
					<tr class="dnnGridItem">
						<td class="qaAwardedCell"><asp:Literal ID="litAwarded" runat="server" /></td>
						<td class="qaBadgeCell">
							<asp:Literal ID="litBadge" runat="server" />
							<span class="qaMultiplier"><asp:Literal ID="litMultiplier" runat="server" /></span>
						</td>
						<td class="qaDescriptionCell"><asp:Literal ID="litDescription" runat="server" /></td>           
					</tr>
				</ItemTemplate>
				<AlternatingItemTemplate>
					<tr class="dnnGridAltItem">
						<td class="qaAwardedCell"><asp:Literal ID="litAwarded" runat="server" /></td>
						<td class="qaBadgeCell">
							<asp:Literal ID="litBadge" runat="server" />
							<span class="qaMultiplier"><asp:Literal ID="litMultiplier" runat="server" /></span>
						</td>
						<td class="qaDescriptionCell"><asp:Literal ID="litDescription" runat="server" /></td>           
					</tr>
				</AlternatingItemTemplate>
				<FooterTemplate>
						</tbody>
					</table>
				</FooterTemplate>
			</asp:Repeater>
		</div>
		<div class="dnnFormMessage dnnFormValidationSummary" style="display:none;">
			<p>No Peeking!</p>
		</div>
	</div>
	<div class="qaSideList dnnRight">
		<h3><%= Localization.GetString("Legend", LocalResourceFile) %></h3>
		<div id="qaBadgeTiers">
			<asp:Repeater ID="rptTiers" runat="server" OnItemDataBound="RptTiersItemDataBound" EnableViewState="false">
				<ItemTemplate>
					<div class="qaBadgeDiv">
						<asp:Literal ID="litBadgeTier" runat="server" />
					</div>
					<p><asp:Literal ID="litTierDescription" runat="server" /></p>
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</div>
</div>