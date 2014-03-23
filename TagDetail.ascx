<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="TagDetail.ascx.cs" Inherits="DotNetNuke.DNNQA.TagDetail" %>
<%@ Register TagPrefix="dqa" Assembly="DotNetNuke.Modules.DNNQA" Namespace="DotNetNuke.DNNQA.Controls" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx"%>
<%@ Import Namespace="DotNetNuke.Services.Localization" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dqa" TagName="HeaderNav" Src="~/DesktopModules/DNNQA/Controls/HeaderNav.ascx" %>
<dqa:HeaderNav ID="dgqHeaderNav" runat="server" />
<div class="dnnForm dnnqa-answers">
	<div class="qaTagInfoTagDetails">
		<h2 class="dnnFormSectionHead"><%= Localization.GetString("Title", LocalResourceFile) %> //
		<span><%= Localization.GetString("About", LocalResourceFile) %>&nbsp;<%= Model.SelectedTerm.Name %></span></h2>
	</div>
	<div class="qatlRight">
		<ul class="qaSortActions dnnClear">
			<li><asp:HyperLink ID="hlAbout" runat="server" resourcekey="hlAbout" /></li>
			<li><asp:HyperLink ID="hlSynonym" runat="server" resourcekey="hlSynonym" /></li>
		</ul>
	</div>
	<div class="tagDescription" id="divDetail" runat="server">
		<p><%= Model.SelectedTerm.Description %></p>
		<ul class="qaNavActions dnnClear">
				<li><asp:HyperLink ID="hlEdit" runat="server" resourcekey="hlEdit" CssClass="dnnSecondaryAction" /></li>
				<li><asp:HyperLink ID="hlHistory" runat="server" resourcekey="hlHistory" CssClass="dnnSecondaryAction" /></li>
		</ul>
	</div>
	<div runat="server" id="divSynonym">
		<p><%= Localization.GetString("SynonymDescription", LocalResourceFile) %></p>
		<asp:Panel ID="pnlActiveSynonyms" runat="server" class="divActiveSynonyms">
			<p>
				<label for="<%= tagSynonyms.ClientID %>"><%= Localization.GetString("Remapped", LocalResourceFile)%></label>
				<dqa:Tags ID="tagSynonyms" runat="server" />
			</p>
		</asp:Panel>
		<asp:Panel ID="pnlNoSynonyms" runat="server" CssClass="dnnFormMessage dnnFormWarning" Visible="false">
			<p><%= Localization.GetString("NoSynonyms", LocalResourceFile) %></p>
		</asp:Panel>
		<asp:Panel ID="pnlSuggestedSynonyms" runat="server">
			<h3><%= Localization.GetString("SuggestedSynonyms", LocalResourceFile) %></h3>
			<asp:Repeater ID="rptSuggestedSynonyms" runat="server" OnItemDataBound="RptSuggestedSynonymsItemDataBound" OnItemCommand="RptSuggestedSynonymsItemCommand">
				<ItemTemplate>
					<table>
						<tbody>
							<tr>
								<td class="qaqVote">
									<dqa:Voting ID="qavTermSynonym" runat="server" VotingMode="Synonym" />
								</td>
								<td class="qaqBody">
									<dqa:Tags ID="qatTermSynonym" runat="server" />
									<asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.gif" CommandName="Delete" />
								</td>
							</tr>
						</tbody>
					</table>
				</ItemTemplate>
			</asp:Repeater>
		</asp:Panel>
		<asp:Panel ID="pnlAddSynonym" runat="server">
			<label for="<%= txtTags.ClientID %>"><%= Localization.GetString("SuggestASynonym", LocalResourceFile) %></label>
			<asp:TextBox ID="txtTags" runat="server" />				
			<asp:LinkButton ID="cmdSuggest" runat="server" resourcekey="cmdSuggest" OnClick="CmdAddSynonymClick" CssClass="dnnPrimaryAction" />
		</asp:Panel>
	</div>
</div>
<dnnweb:DnnCodeBlock ID="dcbTaglist" runat="server" >
	<script language="javascript" type="text/javascript">
		/*globals jQuery, window, Sys */
		(function ($, Sys) {
			function setupDnnTagDetail() {
				function split(val) {
					return val.split(/,\s*/);
				}

				function extractLast(term) {
					return split(term).pop();
				}

				$("#<%= txtTags.ClientID  %>")
				// don't navigate away from the field on tab when selecting an item
					.bind("keydown", function (event) {
						if (event.keyCode === $.ui.keyCode.TAB &&
								$(this).data("autocomplete").menu.active) {
							event.preventDefault();
						}
					})

					.autocomplete({
						source: function (request, response) {
							$.getJSON('<%= ResolveUrl("~/DesktopModules/DNNQA/Tags.ashx")%>', {
								term: extractLast(request.term)
							}, response);
						},
						minLength: 2,
						autoFocus: false,
						delay: 0,
						select: function (event, ui) {
							var terms = split(this.value);
							// remove the current input
							terms.pop();
							// add the selected item
							terms.push(ui.item.value);
							// add placeholder to get the comma-and-space at the end
							terms.push("");
							this.value = terms.join(", ");
							return false;
						}
					});
			}

			$(document).ready(function () {
				setupDnnTagDetail();
				Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
					setupDnnTagDetail();
				});
			});

		} (jQuery, window.Sys));
	</script>  
</dnnweb:DnnCodeBlock>