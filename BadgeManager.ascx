<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="BadgeManager.ascx.cs" Inherits="DotNetNuke.DNNQA.BadgeManager" %>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="qaBadgeManager dnnForm" id="qaBadgeManager">
	<div class="qabmList">
		<h2 class="dnnFormSectionHead"><%=LocalizeString("BadgeManagement")%></h2>
		<div class="qaBadgeList">
			<table  class="dnnGrid">
				<tbody data-bind="foreach: Badges">
					<tr class="dnnGridItem">
						<td class="qaAwardedCell"></td>
						<td class="qaBadgeCell">
							<a href="#" class="qaBadge">
								<span data-bind="class: TierDetails().IconClass"></span>
								<b data-bind="text: LocalizedName"></b>
							</a>
							<span class="qaMultiplier">
								<label><%=LocalizeString("Multiplier")%></label>
								<label data-bind="text: Awarded"></label>
							</span>
						</td>
						<td class="qaDescriptionCell" data-bind="text: LocalizedDesc"></td>           
					</tr>
				</tbody>
			</table>
		</div>
		<ul class="wizardActions">
			<li><asp:HyperLink ID="hlCancel" resourcekey="cmdCancel" runat="server" CssClass="dnnSecondaryAction" /></li>
			<li><a href="#" class="dnnPrimaryAction ComposeBadge" id="AddBadge"><%=LocalizeString("AddBadge")%></a></li>
		</ul>
	</div>
</div>
<div id="divEditBadge" class="qaDialogWrapper" style="display:none;">
	<div id="wizard" class="swMain">
		<ul>
			<li>
				<a href="#step-1">
					<label class="stepNumber">1</label>
					<span class="stepDesc"><%= Localization.GetString("AboutStepDesc", LocalResourceFile) %></span>
				</a>
			</li>
			<li>
				<a href="#step-2">
					<label class="stepNumber">2</label>
					<span class="stepDesc"><%= Localization.GetString("AssociateStepDesc", LocalResourceFile) %></span>
				</a>
			</li>
			<li>
				<a href="#step-3">
					<label class="stepNumber">3</label>
					<span class="stepDesc"><%= Localization.GetString("GoalsStepDesc", LocalResourceFile) %></span>                   
				</a>
			</li>
		</ul>
		<div id="step-1" class="dnnForm">
			<h2 class="StepTitle"><strong><%= Localization.GetString("CreateBadge", LocalResourceFile) %></strong><%= Localization.GetString("AboutStepTitle", LocalResourceFile) %></h2>
			<div class="dnnFormItem dnnFormHelp dnnClear">
				<p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p>
			</div>
			<div class="dnnFormItem">
				<label for="txtName"><%=LocalizeString("dnnlblName")%></label>
				<input id="txtName" class="dnnFormRequired" type="text" />
			</div>
			<div class="dnnFormItem">
				<label for="txtDescription"><%=LocalizeString("dnnlblDescription")%></label>
				<textarea id="txtDescription" class="dnnFormRequired" rows="5" cols="10"></textarea>
			</div>
			<div class="dnnFormItem">
				<label for="ddlTier"><%=LocalizeString("dnnlblTier")%></label>
				<select id="ddlTier">
					<option value="0">none</option>
					<option value="1">bronze</option>
					<option value="2">silver</option>
					<option value="3">gold</option>
				</select>
				<div class="dnnFormItem">
					<label for="txtRepPts"><%=LocalizeString("dnnlblRepPts")%></label>
					<input id="txtRepPts" class="dnnSmall" type="text" disabled="disabled" />
				</div>
			</div>
		</div>
		<div id="step-2" class="dnnForm">
			<h2 class="StepTitle"><strong><%= Localization.GetString("CreateBadge", LocalResourceFile) %></strong><%= Localization.GetString("AssociateStepTitle", LocalResourceFile) %></h2>
			<div class="dnnFormItem">
				<label for="simple"><%=LocalizeString("dnnlblMode")%></label>
				<input type="radio" name="groupMode" value="0" class="dnnFormRadioButtons" checked="checked" id="simple" /><%= Localization.GetString("Simple", LocalResourceFile) %><br />
				<input type="radio" name="groupMode" value="1" class="dnnFormRadioButtons" id="advanced" disabled="disabled" /><%= Localization.GetString("Advanced", LocalResourceFile) %><br />
			</div>
			<div class="dnnFormItem">
				<label for="ddlActivity"><%=LocalizeString("dnnlblActivity")%></label>
				<select data-bind="options: AvailableTiers" id="ddlActivity"></select>
			</div>
		</div>
		<div id="step-3" class="dnnForm">
			<h2 class="StepTitle"><strong><%= Localization.GetString("CreateBadge", LocalResourceFile) %></strong><%= Localization.GetString("GoalsStepTitle", LocalResourceFile) %></h2>
			<div class="dnnFormItem dnnFormHelp dnnClear">
				<p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p>
			</div>
			<div class="dnnFormItem">
				<label for="txtCount"><%=LocalizeString("dnnlblCount")%></label>
				<input id="txtCount" class="dnnFormRequired dnnSmall" type="text" />
			</div>
			<div class="dnnFormItem">
				<label for="txtDays"><%=LocalizeString("dnnlblDays")%></label>
				<input id="txtDays" class="dnnFormRequired dnnSmall" type="text" />
			</div>
		</div>
	</div>
</div>
<dnnweb:DnnCodeBlock ID="dcbQuestions" runat="server" >
	<script language="javascript" type="text/javascript">
		jQuery(document).ready(function ($) {
			var bm = new BadgeManager($, ko, {
				portalId: '<% = ModuleContext.PortalId %>',
				baseUrl: '<%= ResolveUrl("~/DesktopModules/DNNQA/Qa.asmx/") %>',
				labelNext: '<%= Localization.GetString("cmdNext", LocalResourceFile) %>',
				labelPrevious: '<%= Localization.GetString("cmdPrev", LocalResourceFile) %>',
				labelFinish: '<%= Localization.GetString("cmdFinish", LocalResourceFile) %>',
				addWizardDialogTitle: '<%= Localization.GetString("DialogTitle", LocalResourceFile) %>' 
			});

			bm.init();
		});
	</script>
</dnnweb:DnnCodeBlock>