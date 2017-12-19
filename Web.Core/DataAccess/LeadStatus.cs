namespace Fabricam.Web.Core.DataAccess
{
    /// <summary>
    /// Matches dbo.LeadStatus table
    /// </summary>
    public enum LeadStatus
    {
        Created = 1,
        CheckedOut = 2,
        Completed = 3,
        // TODO: clarify lead workflow with users
    }
}
