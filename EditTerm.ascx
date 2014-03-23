<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditTerm.ascx.cs" Inherits="DotNetNuke.DNNQA.EditTerm" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm qaEditTag" id="qaEditTag">
	<h2 class="dnnFormSectionHead"><%= Localization.GetString("Title", LocalResourceFile) %> // <span><asp:Literal ID="litName" runat="server" /></span></h2>
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblDescription" runat="server" controlname="txtDescription" suffix=":" />
			<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Height="200" MaxLength="2500" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="dnnlblEditSummary" runat="server" controlname="txtEditSummary" suffix=":" />
			<asp:TextBox ID="txtEditSummary" runat="server" CssClass="dnnFormRequired" MaxLength="100" TabIndex="0" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton ID="cmdSave" runat="server" resourcekey="cmdSave" CssClass="dnnPrimaryAction" OnClick="CmdSaveClick" /></li>
		<li><asp:HyperLink ID="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="dnnSecondaryAction" /></li>
	</ul>
</div>