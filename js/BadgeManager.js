
function BadgeManager($, ko, settings) {
	var opts = $.extend({}, BadgeManager.defaultSettings, settings);
	var portalId = opts.portalId;
	var baseUrl = opts.baseUrl;
	var labelNext = opts.labelNext;
	var labelPrevious = opts.labelPrevious;
	var labelFinish = opts.labelFinish;
	var addWizardDialogTitle = opts.addWizardDialogTitle;
	
	// variables based on settings
	var getBadgesUrl = baseUrl + "GetPortalBadges";
	var addBadgeUrl = baseUrl + "AddBadge";
	var deleteBadgeUrl = baseUrl + "DeleteBadge";

	// represents 1 row of data
	function Badge(item) {
		var self = this;

		self.Key = ko.observable(item.Key);
		self.BadgeId = ko.observable(item.BadgeId);
		self.LocalizedName = ko.observable(item.LocalizedName);
		self.LocalizedDesc = ko.observable(item.LocalizedDesc);
		self.PortalId = ko.observable(item.PortalId);
		self.Awarded = ko.observable(item.Awarded);
		self.TriggerAction = ko.observable(item.TriggerAction);
		self.TriggerSproc = ko.observable(item.TriggerSproc);
		self.TriggerCount = ko.observable(item.TriggerCount);
		self.TriggerDays = ko.observable(item.TriggerDays);
		self.TierId = ko.observable(item.TierId);
		self.TierLocalizedName = ko.observable(item.TierLocalizedName);

		// lookup data
		self.TierDetails = ko.observable(new BadgeTier(item.TierDetails));

		self.DisplayPopUp = ko.observable(false);

		// Actions
		self.AddBadge = function () {
			$.ajax({
				type: "POST",
				url: addBadgeUrl,
				data: '{ portalId: ' + portalId + '  }',
				contentType: "application/json",
				dataType: "json"
			}).done(function (data) {

				return data;
			}).fail(function (xhr, status, error) {
				alert(error);
			});
		};

		self.DeleteBadge = function () {
			$.ajax({
				type: "POST",
				url: deleteBadgeUrl,
				data: '{ portalId: ' + portalId + '  }',
				contentType: "application/json",
				dataType: "json"
			}).done(function (data) {

				return data;
			}).fail(function (xhr, status, error) {
				alert(error);
			});
		};
	};

	function BadgeTier(item) {
		var self = this;

		self.Id = ko.observable(item.Id);
		self.Key = ko.observable(item.Key);
		self.Name = ko.observable(item.Name);
		self.Description = ko.observable(item.Description);
		self.IconClass = ko.observable(item.IconClass);
		self.TitlePrefixKey = ko.observable(item.TitlePrefixKey);
	};

	// Define Main View Model
	function BadgeManagerViewModel(initialData) {
		var self = this;
		var initialBadges = [];

		ko.utils.arrayForEach(initialData.d, function(item) {
			initialBadges.push(new Badge(item));
		});

		self.Badges = ko.observableArray(initialBadges);

		var availTiers = [];

		self.portalTiers = function () {
			$.ajax({
				type: "POST",
				url: getBadgesUrl,
				data: '{ portalId: ' + portalId + '  }',
				contentType: "application/json",
				dataType: "json"
			}).done(function (tiers) {
				ko.utils.arrayForEach(tiers.d, function(item) {
				   availTiers.push(new BadgeTier(item));
				});
			});
		};

		self.AvailableTiers = ko.observableArray(availTiers);

		var activities = [];
		self.Activities = ko.observableArray(activities);

		self.hidePopUp = function (item, event) {
			var target = null;
			if (event.relatedTarget)
				target = event.relatedTarget;
			else if (event.toElement)
				target = event.toElement;

			if (target && target.tagName.toLowerCase() == "li") {
				item.DisplayPopUp(false);
			}
		};

		self.showPopUp = function (item) {
			//hide all other popups
			for (var i = 0; i < self.Badges().length; i++) {
				self.Badges()[i].DisplayPopUp(false);
			}
			//show popup
			item.DisplayPopUp(true);
		};
	};

	// we call this to initialize (from the .ascx file)
	this.init = function () {
		$.ajax({
			type: "POST",
			url: getBadgesUrl,
			data: '{ portalId: ' + portalId + '  }',
			contentType: "application/json",
			dataType: "json"
		}).done(function (badges) {
			var viewModel = new BadgeManagerViewModel(badges);
			ko.applyBindings(viewModel, $("#qaBadgeManager")[0]);
		});

		// handle other setup functions
		$('#wizard').smartWizard({
			labelNext: labelNext,
			labelPrevious: labelPrevious,
			labelFinish: labelFinish,
			onLeaveStep: leaveAStepCallback,
			onFinish: onFinishCallback
		});

		$('#divEditBadge').dialog({ autoOpen: false, minWidth: 710, title: addWizardDialogTitle });
		
		$('#AddBadge').click(function () {
			$('#divEditBadge').dialog('open');
				// prevent the default action, e.g., following a link
				return false;
		});

		function leaveAStepCallback(obj) {
			var step_num = obj.attr('rel');
			return validateSteps(step_num);
		}

		function onFinishCallback(obj) {
			var validStep = true;
			validStep = validateSteps(3);

			if (validStep == true) {


				//TODO: save data to server (use KO?)

				$('#divEditBadge').dialog('close');
			}

			return validStep;
		}
		
		function validateSteps(step) {
			var isStepValid = true;
			// validate step 1
			if (step == 1) {
				if (validateStep1() == false) {
					isStepValid = false;
					//		                    $('#wizard').smartWizard('showMessage', 'Please correct the errors in step ' + step + ' and click next.');
					$('#wizard').smartWizard('setError', { stepnum: step, iserror: true });
				} else {
					$('#wizard').smartWizard('setError', { stepnum: step, iserror: false });
				}
			}

			// validate step 3
			if (step == 3) {
				if (validateStep3() == false) {
					isStepValid = false;
					//		                    $('#wizard').smartWizard('showMessage', 'Please correct the errors in step ' + step + ' and click next.');
					$('#wizard').smartWizard('setError', { stepnum: step, iserror: true });
				} else {
					$('#wizard').smartWizard('setError', { stepnum: step, iserror: false });
				}
			}

			return isStepValid;
		}

		function validateStep1() {
			var isValid = true;

			// Validate Name
			var name = $('#txtName').val();
			if (!name && name.length <= 0) {
				isValid = false;

				return false;
			}

			// TODO: make sure name is not used as a key already

			// Validate Desc
			var description = $('#txtDescription').val();
			if (!description && description.length <= 0) {
				isValid = false;

				return false;
			}
			
			return isValid;
		}

		function validateStep3() {
			var isValid = true;

			// Validate Count
			var count = $('#txtCount').val();
			if (!count && count.length <= 0) {
				isValid = false;

				return false;
			}

			// Validate Days
			var days = $('#txtDays').val();
			if (!days && days.length <= 0) {
				isValid = false;

				return false;
			}

			return isValid;
		}
	};
}

// Settings passed in from .ascx file
BadgeManager.defaultSettings = {
	portalId: 0,
	baseUrl: "/DesktopModules/DNNQA/QA.asmx",
	labelNext: "next",
	labelPrevious: "previous",
	labelFinish: "finish",
	addWizardDialogTitle: "Badge Wizard"
};