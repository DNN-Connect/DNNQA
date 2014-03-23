<%@ Control Language="C#" AutoEventWireup="false" Inherits="DotNetNuke.DNNQA.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<div class="dnnForm dnnQASettings dnnClear" id="dnnQASettings">
	<asp:Panel id="pnlNormalSettings" runat="server">
	   <div class="dnnFormExpandContent"><a href=""><%=Localization.GetString("ExpandAll", Localization.SharedResourceFile)%></a></div>
		<h2 id="dnnSitePanel-qaGeneralSettings" class="dnnFormSectionHead"><a href="" class="dnnSectionExpanded"><%= Localization.GetString("EmailSettings", LocalResourceFile) %></a></h2>
		<fieldset>
			<div class="dnnFormItem">
				<dnn:label ID="dnnlblFromEmail" runat="server" ControlName="txtbxFromEmail" Suffix=":" />
				<asp:TextBox ID="txtbxFromEmail" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:label ID="dnnlblQuestionEmailTemplate" runat="server" ControlName="txtbxQuestionEmailTemplate" Suffix=":" />
				<asp:TextBox ID="txtbxQuestionEmailTemplate" runat="server" TextMode="MultiLine" />
			</div>
			<div class="dnnFormItem">
				<dnn:label ID="dnnlblAnswerEmailTemplate" runat="server" ControlName="txtAnswerEmailTemplate" Suffix=":" />
				<asp:TextBox ID="txtAnswerEmailTemplate" runat="server" TextMode="MultiLine" />
			</div>
			<div class="dnnFormItem">
				<dnn:label ID="dnnlblCommentEmailTemplate" runat="server" ControlName="txtbxCommentEmailTemplate" Suffix=":" />
				<asp:TextBox ID="txtbxCommentEmailTemplate" runat="server" TextMode="MultiLine" />
			</div>
			<div class="dnnFormItem" style="display: none;">
				<dnn:label ID="dnnlblSummaryEmailTemplate" runat="server" ControlName="txtbxSummaryEmailTemplate" Suffix=":" />
				<asp:TextBox ID="txtbxSummaryEmailTemplate" runat="server" TextMode="MultiLine" />
			</div>
		</fieldset>
		<h2 id="dnnSitePanel-qaAskQuestionSettings" class="dnnFormSectionHead"><a href=""><%= Localization.GetString("AskQuestionSettings", LocalResourceFile) %></a></h2>
		<fieldset>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblMinTitleChars" runat="server" controlname="dntxtbxMinTitleChars" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntxtbxMinTitleChars" runat="server" MinValue="3" MaxValue="50" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem" style="display:none;">
				<dnn:label id="dnnlblMinBodyChars" runat="server" controlname="dntxtbxMinBodyChars" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntxtbxMinBodyChars" runat="server" MinValue="3" MaxValue="50" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblMaxTags" runat="server" controlname="dntxtbxMaxTags" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntxtbxMaxTags" runat="server" MinValue="1" MaxValue="5" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem" style="display:none;">
				<dnn:label id="dnnlblVocabRoot" runat="server" controlname="dntxtbxDailyFlags" suffix=":" />
				<asp:DropDownList ID="ddlVocabRoot" runat="server" />
			</div>
			<div class="dnnFormItem" style="display:none;">
				<dnn:label id="dnnlblAutoApprove" runat="server" controlname="chkAutoApprove" suffix=":" />
				<asp:CheckBox ID="chkAutoApprove" runat="server" />
			</div>
		</fieldset>
		<h2 id="dnnSitePanel-qaUISettings" class="dnnFormSectionHead"><a href=""><%= Localization.GetString("UISettings", LocalResourceFile) %></a></h2>
		<fieldset>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblHomePageSize" runat="server" controlname="dntHomePageSize" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntHomePageSize" runat="server" MinValue="5" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblHomeTagSize" runat="server" controlname="dntHomeTagSize" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntHomeTagSize" runat="server" MinValue="5" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblHomeTagUsageType" runat="server" controlname="ddlHomeTagUsageType" suffix=":" />
				<asp:DropDownList ID="ddlHomeTagUsageType" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblBrowseQPageSize" runat="server" controlname="dntBrowseQPageSize" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntBrowseQPageSize" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblAnswerPageSize" runat="server" controlname="dnntAnswerPageSize" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dnntAnswerPageSize" runat="server" MinValue="1" MaxValue="50" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblMaxTagsTags" runat="server" controlname="dnntMaxTagsTags" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dnntMaxTagsTags" runat="server" MinValue="1" MaxValue="50" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem" style="display:none;">
				<dnn:label id="dnnlblNameFormat" runat="server" controlname="ddlNameFormat" suffix=":" />
				<asp:DropDownList ID="ddlNameFormat" runat="server" />
			</div>
			<div class="dnnFormItem" style="display:none;">
				<dnn:label id="dnnlblTagHistory" runat="server" controlname="dntxtbxTagHistory" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntxtbxTagHistory" runat="server" MinValue="0" MaxValue="50" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblFacebookAppId" runat="server" controlname="txtbxFacebookAppId" suffix=":" />
				<asp:TextBox ID="txtbxFacebookAppId" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblEnablePlusOne" runat="server" controlname="chkEnablePlusOne" suffix=":" />
				<asp:CheckBox ID="chkEnablePlusOne" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblEnableTwitter" runat="server" controlname="chkEnableTwitter" suffix=":" />
				<asp:CheckBox ID="chkEnableTwitter" runat="server" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblEnableLinkedIn" runat="server" controlname="chkEnableLinkedIn" suffix=":" />
				<asp:CheckBox ID="chkEnableLinkedIn" runat="server" />
			</div>
			<div class="dnnFormItem" style="display: none;">
				<dnn:label id="dnnlblEnableRSS" runat="server" controlname="chkEnableRSS" suffix=":" />
				<asp:CheckBox ID="chkEnableRSS" runat="server" />
			</div>
		</fieldset>
		<h2 id="dnnSitePanel-qaThresholdSettings" class="dnnFormSectionHead"><a href=""><%= Localization.GetString("ThresholdSettings", LocalResourceFile) %></a></h2>
		<fieldset>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblQCCVC" runat="server" controlname="dntbQCCVC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbQCCVC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblQCWD" runat="server" controlname="dntbQCWD" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbQCWD" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblQFHRC" runat="server" controlname="dntbQFHRC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbQFHRC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblPCVWM" runat="server" controlname="dntbPCVWM" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbPCVWM" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblPFCC" runat="server" controlname="dntbPFCC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbPFCC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem" style="display:none;">
				<dnn:label id="dnnlblPFWH" runat="server" controlname="dntbPFWH" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbPFWH" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblTCWD" runat="server" controlname="dntbTCWD" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbTCWD" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblTFWH" runat="server" controlname="dntbTFWH" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbTFWH" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblTFCC" runat="server" controlname="dntbTFCC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbTFCC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblTSAC" runat="server" controlname="dntbTSAC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbTSAC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblTSRC" runat="server" controlname="dntbTSRC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbTSRC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblTSMC" runat="server" controlname="dntbTSMC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbTSMC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblUCVC" runat="server" controlname="dntbUCVC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbUCVC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblUFPMC" runat="server" controlname="dntbUFPMC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbUFPMC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblFPSC" runat="server" controlname="dntbFPSC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbFPSC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div id="Div1" class="dnnFormItem" visible="false" runat="server">
				<dnn:label id="dnnlblUTSCMAC" runat="server" controlname="dntbUTSCMAC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbUTSCMAC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div id="Div2" class="dnnFormItem" visible="false" runat="server">
				<dnn:label id="dnnlblUTSVMASC" runat="server" controlname="dntbUTSVMASC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbUTSVMASC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblUUVAC" runat="server" controlname="dntbUUVAC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbUUVAC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblUUVQC" runat="server" controlname="dntbUUVQC" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbUUVQC" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
			<div class="dnnFormItem">
				<dnn:label id="dnnlblQHMS" runat="server" controlname="dntbQHMS" suffix=":" />
				<dnnweb:DnnNumericTextBox ID="dntbQHMS" runat="server" MinValue="-10" MaxValue="50" NumberFormat-DecimalDigits="0" CssClass="dnnFormRequired" />
			</div>
		</fieldset>
	</asp:Panel>
	<asp:Panel id="pnlProfileSettings" runat="server">
		
	</asp:Panel>
</div>
<script language="javascript" type="text/javascript">
	/*globals jQuery, window, Sys */
	(function ($, Sys) {
		function setupQaSettings() {
			$('#qaNormalSettings .dnnFormExpandContent a').dnnExpandAll({ expandText: '<%=Localization.GetString("ExpandAll", Localization.SharedResourceFile)%>', collapseText: '<%=Localization.GetString("CollapseAll", Localization.SharedResourceFile)%>', targetArea: '#qaNormalSettings' });
		}

		$(document).ready(function () {
			setupQaSettings();
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
				setupQaSettings();
			});
		});

	} (jQuery, window.Sys));
</script>   