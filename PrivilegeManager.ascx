<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="PrivilegeManager.ascx.cs" Inherits="DotNetNuke.DNNQA.PrivilegeManager" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="qaPrivilegeManager dnnForm dnnClear" id="qaPrivilegeManager">
	<h2 id="dnnSitePanel-qaPrivileges" class="dnnFormSectionHead"><%= Localization.GetString("PrivilegeLevels", LocalResourceFile) %></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblCreatePost" runat="server" controlname="dntbCreatePost" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbCreatePost" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblNewUser" runat="server" controlname="dntbNewUser" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbNewUser" runat="server" MinValue="1" MaxValue="100" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblFlag" runat="server" controlname="dntbFlag" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbFlag" runat="server" MinValue="1" MaxValue="100000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVoteUp" runat="server" controlname="dntbVoteUp" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVoteUp" runat="server" MinValue="1" MaxValue="100000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblCommentEverywhere" runat="server" controlname="dntbCommentEverywhere" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbCommentEverywhere" runat="server" MinValue="1" MaxValue="100000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblVoteDown" runat="server" controlname="dntbVoteDown" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbVoteDown" runat="server" MinValue="2" MaxValue="100000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblRetag" runat="server" controlname="dntbRetag" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbRetag" runat="server" MinValue="0" MaxValue="100000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblEditQA" runat="server" controlname="dntbEditQA" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbEditQA" runat="server" MinValue="0" MaxValue="100000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblCreateTagSynonym" runat="server" controlname="dntbCreateTagSynonym" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbCreateTagSynonym" runat="server" MinValue="0" MaxValue="1000000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblCloseQ" runat="server" controlname="dntbCloseQ" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbCloseQ" runat="server" MinValue="0" MaxValue="1000000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblApproveTags" runat="server" controlname="dntbApproveTags" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbApproveTags" runat="server" MinValue="0" MaxValue="1000000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblModTools" runat="server" controlname="dntbModTools" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbModTools" runat="server" MinValue="0" MaxValue="1000000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblProtectQ" runat="server" controlname="dntbProtectQ" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbProtectQ" runat="server" MinValue="0" MaxValue="1000000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblTrusted" runat="server" controlname="dntbTrusted" suffix=":" />
			<dnnweb:DnnNumericTextBox ID="dntbTrusted" runat="server" MinValue="0" MaxValue="9000000" NumberFormat-DecimalDigits="0" CssClass="dnnSmall dnnFormRequired" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" CssClass="dnnPrimaryAction" OnClick="OnSubmitClick" /></li>
		<li><asp:HyperLink ID="hlCancel" resourcekey="cmdCancel" runat="server" CssClass="dnnSecondaryAction" /></li>
	</ul>
</div>