/*global leadService:false, mocha:false, chai:false */
(function () {
	'use strict';

	const expect = chai.expect;

	const realFetch = window.fetch;
	afterEach(() => window.fetch = realFetch);
	
	// these tests don't need appSettings
	const realAppSettings = window.appSettings;
	beforeEach(() => window.appSettings = {}); // just enough to not be null
	afterEach(() => window.appSettings = realAppSettings);
	

	describe('leadService/loadLeads', function () {

		it('should not catch', async function () {
			
			// arrange
			let message = 'loadLeads test';
			window.fetch = async function () {
				throw new Error(message);
			};
			let caught = undefined;

			// act
			try {
				await leadService.loadLeads();
			} catch (err) {
				caught = err;
			}

			// assert
			expect(caught.message).to.equal(message);
		});
		
		it('should fail on no .json()', async function () {
			
			// arrange
			// FRAGILE: is this browser specific?
			let message = 'Cannot read property \'json\' of null';
			window.fetch = async function () {
				return null;
			};
			let caught = undefined;

			// act
			try {
				await leadService.loadLeads();
			} catch (err) {
				caught = err;
			}

			// assert
			expect(caught.message).to.equal(message);
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should pass on success', async function () {
			
			// arrange
			setupFetchMock({
				success: true
			});
			let expected = 'success';

			// act
			let results = await leadService.loadLeads();

			// assert
			expect(results.message).to.include(expected);
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should return fail when unsuccessful results', async function () {
			
			// arrange
			setupFetchMock({
				success: false
			});
			let expected = 'fail';

			// act
			let results = await leadService.loadLeads();

			// assert
			expect(results.message).to.include(expected);
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should return fail when no results', async function () {
			
			// arrange
			setupFetchMock(null);
			let expected = 'fail';

			// act
			let results = await leadService.loadLeads();

			// assert
			expect(results.message).to.include(expected);
		});

	});

	describe('leadService/getNextLead', function () {
		
		it('should not catch', async function () {

			// arrange
			let message = 'getNextLead test';
			window.fetch = async function () {
				throw new Error(message);
			};
			let caught = undefined;

			// act
			try {
				await leadService.getNextLead();
			} catch (err) {
				caught = err;
			}

			// assert
			expect(caught.message).to.equal(message);
		});
		
		it('should fail on no .json()', async function () {
			
			// arrange
			// FRAGILE: is this browser specific?
			let message = 'Cannot read property \'json\' of null';
			window.fetch = async function () {
				return null;
			};
			let caught = undefined;

			// act
			try {
				await leadService.getNextLead();
			} catch (err) {
				caught = err;
			}

			// assert
			expect(caught.message).to.equal(message);
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should pass blank message on lead', async function () {
			
			// arrange
			let lead = {
				leadId: 1
			};
			setupFetchMock({
				success: true,
				lead
			});

			// act
			let results = await leadService.getNextLead();

			// assert
			expect(results.lead).to.equal(lead);
			expect(results.message).to.not.be.ok; // falsey
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should pass message on null lead', async function () {
			
			// arrange
			setupFetchMock({
				success: true,
				lead: null
			});

			// act
			let results = await leadService.getNextLead();

			// assert
			expect(results.lead).to.equal(null);
			expect(results.message).to.include('database');
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should return fail when unsuccessful results', async function () {
			
			// arrange
			setupFetchMock({
				success: false
			});
			let expected = 'fail';

			// act
			let results = await leadService.getNextLead();

			// assert
			expect(results.message).to.include(expected);
		});
			
		// FRAGILE: matching strings, not i18n safe
		it('should return fail when no results', async function () {
			
			// arrange
			setupFetchMock(null);
			let expected = 'fail';

			// act
			let results = await leadService.getNextLead();

			// assert
			expect(results.message).to.include(expected);
		});

	});

	function setupFetchMock(results) {
		window.fetch = async function () {
			return {
				json: async function () {
					return results;
				}
			};
		};
	}

}());
