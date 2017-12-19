/*global Vue:false */
/*exports appSettings */
let appSettings = (function () {
	'use strict';

	// FRAGILE: not the Vue.js way:
	const loadLeadsUrl = $('#loadLeadsUrl')[0].href;
	const getNextLeadUrl = $('#getNextLeadUrl')[0].href;

	return {
		loadLeadsUrl,
		getNextLeadUrl
	};
}());
