/*global Vue:false, leadService:false */
(function () {
	'use strict';

	let app = new Vue({
		el: '#app',
		data: {
			message: null,
			lead: null
		},
		methods: {
			loadLeads: function () {
				var that = this;
				that.message = 'Loading ...';
				leadService.loadLeads()
				.then(res => {
					that.message = res.message;
				}).catch(err => {
					that.message = 'error loading leads';
					console.log(err);
				});
			},
			getNextLead: function () {
				var that = this;
				that.message = 'Loading ...';
				leadService.getNextLead()
				.then(res => {
					that.lead = res.lead;
					that.message = res.message;
				}).catch(err => {
					that.lead = null;
					that.message = 'error loading leads';
					console.log(err);
				});
			}
		}
	});

}());
