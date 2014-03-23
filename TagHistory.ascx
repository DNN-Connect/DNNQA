<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TagHistory.ascx.cs" Inherits="DotNetNuke.DNNQA.TagHistory" %>
<%@ Import Namespace="DotNetNuke.DNNQA.Components.Common" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaTagHistory" id="qaTagHistory">
	<h2><%= Localization.GetString("History", LocalResourceFile) %> // <asp:HyperLink ID="hlTitle" runat="server" /></h2>
	<h2 class="dnnFormSectionHead" id="qaTermHistoryPanel-LatestTermRevision"><a href="" class="dnnSectionExpanded"><%= Utils.CalculateDateForDisplay(Model.SelectedTerm.LastModifiedOnDate) %></a></h2>
	<fieldset>
        <div class="dnnLeft">
            <dnnweb:DnnBinaryImage ID="dbiUser" runat="server" Width="40" /><br />
            <asp:Literal ID="litUpdated" runat="server" />
        </div>
        <div class="dnnRight"><asp:Literal ID="litDescription" runat="server" /></div>
	</fieldset>
	<asp:Repeater ID="rptTermHistory" runat="server" OnItemDataBound="RptTermHistoryItemDataBound" EnableViewState="false" >
		<ItemTemplate>
			<asp:Literal ID="litHeaderPanel" runat="server" />
			<fieldset>
                <div class="dnnLeft">
                    <dnnweb:DnnBinaryImage ID="dbiTermUser" runat="server" Width="40" /><br />
                    <asp:Literal ID="litTermUpdated" runat="server" />
                </div>
                <div class="dnnRight"><asp:Literal ID="litTermDescription" runat="server" /></div>                
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