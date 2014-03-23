<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="HeaderNav.ascx.cs" Inherits="DotNetNuke.DNNQA.Controls.HeaderNav" %>
<div class="qaNav dnnClear">
	<div class="dnnClear">
		<ul class="qaNavItems dnnLeft">
			<li><asp:HyperLink ID="hlHome" runat="server" /></li>
			<li><asp:HyperLink ID="hlQuestions" runat="server" /></li>
			<li><asp:HyperLink ID="hlTags" runat="server" /></li>
			<li><asp:HyperLink ID="hlBadges" runat="server" style="display:none;" /></li>
			<li><asp:HyperLink ID="hlUnanswered" runat="server" /></li>       
		</ul>	 
		<div class="qaSearch dnnRight">
			<asp:TextBox ID="txtSearch" runat="server" CssClass="qaplaceholder" defaultvalue="Search Questions" MaxLength="100" />
		    <asp:ImageButton runat="server" ID="imgSearch" CssClass="searchIcon" Width="12" Height="12" />
		</div>
		<div class="qaAskQuestionLink dnnRight">
			<asp:HyperLink ID="hlAskQuestion" runat="server" />
		</div>
	</div>
	<div id="divMessage" style="display:none;">
		<span><asp:Literal ID="litMessage" runat="server" /></span>
		<a href="#" class="close-notify">X</a>
	</div>
</div>
<script language="javascript" type="text/javascript">
	/*globals jQuery, window, Sys */
	(function ($, Sys) {
		function checkDivMessage(fadeMessage) {
			if ($("#divMessage span").html() != "") {
				$("#divMessage").slideDown();
				if (fadeMessage) {
					setTimeout(function () {
						$("#divMessage").fadeOut(function () {
							$("#divMessage span").html("");
							checkDivMessage();
						});
					}, 8000);
				}
			} else {
				//JS: Remove else statement if you don't want automatic message checking.
				setTimeout(function () { checkDivMessage(); }, 1000);
			}
		}

		function setupQaHeaderNav(pb) {
			$('.qaHeaderTooltip').qaTooltip({
				tooltipSelector: '.dashMenu',
				suppressClickSelector: 'a.dashboard',
				useParentActiveClass: true,
				enableTouch: true
			});


			if (!pb) {
				$("#divMessage").prependTo("#Body");
			} else {
				$(".DNNModuleContent #divMessage").remove();
			}

			checkDivMessage();

			$("#divMessage a.close-notify").click(function () {
				$("#divMessage").fadeOut("slow");
				return false;
			});
		}

		$(document).ready(function () {
			setupQaHeaderNav();
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
				setupQaHeaderNav(true);
			});
		});

	} (jQuery, window.Sys));
</script>