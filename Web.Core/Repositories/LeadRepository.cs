using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fabricam.Web.Core.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Fabricam.Web.Core.Repositories
{
    public interface ILeadRepository
    {
        Task<Lead> GetNextLeadAsync(int CurrentUserId);
        Task<int> InsertLeadsAsync(List<Lead> Leads);
    }

    public class LeadRepository : ILeadRepository {
        private readonly FabricamDbContext context;

        public LeadRepository(FabricamDbContext Context) {
            this.context = Context ?? throw new ArgumentNullException(nameof(Context));
        }

        public async Task<Lead> GetNextLeadAsync(int CheckedOutBy)
        {
            // FRAGILE: named parameters aren't available in EF Core right now, so must name them @p0, @p1, etc
            // See also: https://dotnetthoughts.net/how-to-execute-storedprocedure-in-ef-core/
            return await this.context.Lead.FromSql("dbo.LeadGetNext @p0", CheckedOutBy).FirstOrDefaultAsync();
        }

        public async Task<int> InsertLeadsAsync(List<Lead> Leads)
        {
            this.context.Lead.AddRange(Leads);
            int rows = await this.context.SaveChangesAsync();
            return rows;
        }

    }
}
