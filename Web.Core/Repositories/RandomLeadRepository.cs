using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Fabricam.Web.Core.DataAccess;
using Fabricam.Shared;
using Fabricam.Web.Core.Models;

namespace Fabricam.Web.Core.Repositories
{
    /// <summary>
    /// Get random lead data from https://randomuser.me/
    /// </summary>
    public interface IRandomLeadRepository
    {
        Task<List<Lead>> GetLeadsAsync(int Count);
    }

    /// <summary>
    /// Get random lead data from https://randomuser.me/
    /// </summary>
    public class RandomLeadRepository : IRandomLeadRepository {
        private const string url = "https://randomuser.me/api/?exc=login,picture,id&results="; // TODO: move to config?

        public RandomLeadRepository()
        {
        }

        public async Task<List<Lead>> GetLeadsAsync(int Count)
        {
            HttpClient client = new HttpClient(); // TODO: get this from DI if we need interesting logging or proxying
            HttpResponseMessage res = await client.GetAsync(url+Count);
            RandomUserData result = await res.ReadJsonResponseAsync<RandomUserData>();

            // map-reduce to leads
            // TODO: this business logic doesn't belong here
            // FRAGILE: ASSUME: all leads will validate correctly
            List<Lead> leads = (
                from r in result?.results ?? new List<RandomUserResult>()
                select new Lead {
                    LeadStatus = LeadStatus.Created,
                    Title = r.name?.title,
                    FirstName = r.name?.first,
                    LastName = r.name?.last,
                    Gender = r.gender,
                    Address = r.location?.street,
                    City = r.location?.city,
                    State = r.location?.state,
                    PostalCode = r.location?.postcode,
                    Country = r.nat,
                    Email = r.email,
                    Phone = r.phone,
                    Cell = r.cell,
                }
            ).ToList();

            return leads;
        }

    }
}
