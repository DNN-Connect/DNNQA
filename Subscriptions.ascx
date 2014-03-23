<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="Subscriptions.ascx.cs" Inherits="DotNetNuke.DNNQA.Subscriptions" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaSubscriptions" id="qaSubscriptions">
	<h2 class="dnnFormSectionHead"><%= LocalizeString("Title") %></h2>
	<ul class="dnnAdminTabNav dnnClear">
		<li><a href="#qaTermSubs"><%=LocalizeString("TagSubs") %></a></li>
		<li><a href="#qaQuestionSubs"><%=LocalizeString("QuestionSubs") %></a></li>
	</ul>
	<div id="qaTermSubs" class="qaInnerTab">
		<h2 class="dnnFormSectionHead">Tag Subscriptions</h2>
		<dnnweb:DnnGrid ID="dgTermSubs" runat="server" AllowPaging="false" OnItemDataBound="DgItemDataBound" OnNeedDataSource="TermsSubGridNeedDataSource" OnItemCommand="DgItemCommand" AutoGenerateColumns="false" ShowHeader="false"  Skin="Simple" GridLines="None">
			<MasterTableView DataKeyNames="SubscriptionId,SubscriptionType">	
				<Columns>
					<dnnweb:DnnGridBoundColumn UniqueName="Name" HeaderText="Name" DataField="Name" />
					<dnnweb:DnnGridButtonColumn UniqueName="DeleteTerm" CommandName="DeleteTerm" ButtonType="LinkButton" Text="Unsubscribe" ButtonCssClass="dnnDelete" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
				</Columns>
				<NoRecordsTemplate>
					<div class="dnnFormMessage dnnFormWarning"><asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" /></div>
				</NoRecordsTemplate>
			</MasterTableView>
		</dnnweb:DnnGrid>
	</div>
	<div id="qaQuestionSubs" class="qaInnerTab">
		<h2 class="dnnFormSectionHead">Question Subscriptions</h2>
		<dnnweb:DnnGrid ID="dgQuestionSubs" runat="server" AllowPaging="false" OnItemDataBound="DgItemDataBound" OnNeedDataSource="QuestionSubsGridNeedDataSource" OnItemCommand="DgItemCommand" AutoGenerateColumns="false" ShowHeader="false"  Skin="Simple" GridLines="None">
			<MasterTableView DataKeyNames="SubscriptionId,SubscriptionType">
				<Columns>
					<dnnweb:DnnGridBoundColumn UniqueName="QTitle" HeaderText="QTitle" DataField="Title" />
					<dnnweb:DnnGridButtonColumn UniqueName="DeleteQuestion" CommandName="DeleteQuestion" ButtonType="LinkButton" Text="Unsubscribe" ButtonCssClass="dnnDelete" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
				</Columns>
				<NoRecordsTemplate>
					<div class="dnnFormMessage dnnFormWarning"><asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoQRecords" /></div>
				</NoRecordsTemplate>
			</MasterTableView>
		</dnnweb:DnnGrid>
	</div>
</div>
<dnnweb:DnnCodeBlock ID="dcbTaglist" runat="server" >
	<script language="javascript" type="text/javascript">
		/*globals jQuery, window, Sys */
		(function ($, Sys) {
			function setupDnnQuestions() {
			    $('#qaSubscriptions').dnnTabs();

			    $('.dnnDelete').dnnConfirm({
			        text: '<%= LocalizeString("DeleteMessage") %>',
			        yesText: '<%= Localization.GetString("Yes.Text", Localization.SharedResourceFile) %>',
			        noText: '<%= Localization.GetString("No.Text", Localization.SharedResourceFile) %>',
			        title: '<%= Localization.GetString("Confirm.Text", Localization.SharedResourceFile) %>'
			    });
			}

			$(document).ready(function () {
				setupDnnQuestions();
				Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
					setupDnnQuestions();
				});
			});

		} (jQuery, window.Sys));
	</script>  
</dnnweb:DnnCodeBlock>