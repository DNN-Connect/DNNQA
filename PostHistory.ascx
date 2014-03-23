<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PostHistory.ascx.cs" Inherits="DotNetNuke.DNNQA.PostHistory" %>
<%@ Import Namespace="DotNetNuke.DNNQA.Components.Common" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaTagHistory" id="qaTagHistory">
	<h1><%= Localization.GetString("History", LocalResourceFile) %> // <span><asp:Literal ID="litTitle" runat="server" /></span></h1>
	<h2 class="dnnFormSectionHead" id="qaTermHistoryPanel-LatestTermRevision"><a href="" class="dnnSectionExpanded"><%= Utils.CalculateDateForDisplay(Model.SelectedPost.LastModifiedOnDate)%> <span></span></a></h2>
	<fieldset>
		<p><%= Model.SelectedPost.Body %></p>
	</fieldset>
	<asp:Repeater ID="rptTermHistory" runat="server" OnItemDataBound="RptTermHistoryItemDataBound" EnableViewState="false" >
		<ItemTemplate>
			<asp:Literal ID="litHeaderPanel" runat="server" />
			<fieldset>
				<p><asp:Literal ID="litDescription" runat="server" /></p>
			</fieldset>
		</ItemTemplate>
	</asp:Repeater>
</div>
<dnnweb:DnnCodeBlock ID="dcbTaglist" runat="server" >
	<script language="javascript" type="text/javascript">
		/*globals jQuery, window, Sys */
		(function ($, Sys) {
			function setupQaTagHistory() {
				$('#qaTagHistory').dnnPanels();
			}

			$(document).ready(function () {
				setupQaTagHistory();
				Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
					setupQaTagHistory();
				});
			});

		} (jQuery, window.Sys));
	</script>  
</dnnweb:DnnCodeBlock>