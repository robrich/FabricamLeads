/*global appSettings:false */
var leadService = (function () {
	'use strict';
	
	return {
		loadLeads,
		getNextLead
	};
	
	async function loadLeads() {
		let res = await fetch(appSettings.loadLeadsUrl, {
			method: 'GET',
			credentials: 'same-origin',
			headers: new Headers({
				'Accept': 'application/json'
			})
		});
		let json = await res.json();
		
		let results = {};
		
		if (!json || !json.success) {
			results.lead = null;
			results.message = 'getting leads failed';
			return results;
		}
		
		results.message = 'loading leads success';
		return results;
	}

	async function getNextLead() {
		let res = await fetch(appSettings.getNextLeadUrl, {
			method: 'GET',
			credentials: 'same-origin',
			headers: new Headers({
				'Accept': 'application/json'
			})
		});
		let json = await res.json();
		
		let results = {};

		if (!json || !json.success) {
			results.lead = null;
			results.message = 'getting next lead failed';
			return results;
		}

		if (json.lead) {
			results.lead = json.lead;
			results.message = '';
		} else {
			results.lead = null;
			results.message = 'there are no more available leads in the database';
		}
		return results;
	}

}());
