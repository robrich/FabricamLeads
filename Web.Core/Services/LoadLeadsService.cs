using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fabricam.Web.Core.DataAccess;
using Fabricam.Web.Core.Repositories;

namespace Fabricam.Web.Core.Services
{
    // TODO: consider moving all the "load leads" processing into a separate microservice

    public interface ILoadLeadsService
    {
        Task<int> GetMoreLeads(int Count);
    }

    public class LoadLeadsService : ILoadLeadsService
    {
        private readonly IRandomLeadRepository randomLeadRepository;
        private readonly ILeadRepository leadRepository;

        public LoadLeadsService(IRandomLeadRepository RandomLeadRepository, ILeadRepository LeadRepository) {
            this.randomLeadRepository = RandomLeadRepository ?? throw new ArgumentNullException(nameof(RandomLeadRepository));
            this.leadRepository = LeadRepository ?? throw new ArgumentNullException(nameof(LeadRepository));
        }

        public async Task<int> GetMoreLeads(int Count)
        {
            List<Lead> leads = await this.randomLeadRepository.GetLeadsAsync(Count);
            int count = await this.leadRepository.InsertLeadsAsync(leads);
            return count;
        }

    }
}
