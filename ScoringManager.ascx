<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ScoringManager.ascx.cs" Inherits="DotNetNuke.DNNQA.ScoringManager" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="qaScoringManager dnnForm dnnClear" id="qaScoringManager">
	<h2 id="dnnSitePanel-qaScoring" class="dnnFormSectionHead"><%= Localization.GetString("ScoringValues", LocalResourceFile) %></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblApprovedPostEdit" runat="server" controlname="dntbApprovedPostEdit" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbApprovedPostEdit" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblApproveTagEdit" runat="server" controlname="dntbApproveTagEdit" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbApproveTagEdit" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblAskedFlaggedQ" runat="server" controlname="dntbAskedFlaggedQ" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbAskedFlaggedQ" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblAskedQ" runat="server" controlname="dntbAskedQ" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbAskedQ" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblAskedQVotedDown" runat="server" controlname="dntbAskedQVotedDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbAskedQVotedDown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblAskedQVotedUp" runat="server" controlname="dntbAskedQVotedUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbAskedQVotedUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblCreatedSynonym" runat="server" controlname="dntbCreatedSynonym" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbCreatedSynonym" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblCommented" runat="server" controlname="dntbCommented" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbCommented" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblEditedPost" runat="server" controlname="dntbEditedPost" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbEditedPost" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblEditedTag" runat="server" controlname="dntbEditedTag" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbEditedTag" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblEditedTagVotedDown" runat="server" controlname="dntbEditedTagVotedDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbEditedTagVotedDown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblEditedTagVotedUp" runat="server" controlname="dntbEditedTagVotedUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbEditedTagVotedUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblFirstLoggedInView" runat="server" controlname="dntbFirstLoggedInView" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbFirstLoggedInView" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblProvidedA" runat="server" controlname="dntbProvidedA" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbProvidedA" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblProvidedAcceptedA" runat="server" controlname="dntbProvidedAcceptedA" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbProvidedAcceptedA" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblProvidedAVotedDown" runat="server" controlname="dntbProvidedAVotedDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbProvidedAVotedDown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblProvidedAVotedUp" runat="server" controlname="dntbProvidedAVotedUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbProvidedAVotedUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblProvidedFlaggedA" runat="server" controlname="dntbProvidedFlaggedA" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbProvidedFlaggedA" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedADown" runat="server" controlname="dntbVotedADown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedADown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedQDown" runat="server" controlname="dntbVotedQDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedQDown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedSDown" runat="server" controlname="dntbVotedSDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedSDown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedTDown" runat="server" controlname="dntbVotedTDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedTDown" runat="server" MinValue="-100" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedAUp" runat="server" controlname="dntbVotedAUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedAUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedQUp" runat="server" controlname="dntbVotedQUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedQUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedSUp" runat="server" controlname="dntbVotedSUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedSUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVotedTUp" runat="server" controlname="dntbVotedTUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVotedTUp" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblAcceptedQAnswer" runat="server" controlname="dntbAcceptdQAnswer" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbAcceptdQAnswer" runat="server" MinValue="0" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" CssClass="dnnPrimaryAction" OnClick="OnSubmitClick" /></li>
		<li><asp:HyperLink ID="hlCancel" resourcekey="cmdCancel" runat="server" CssClass="dnnSecondaryAction" /></li>
	</ul>
</div>