using Oogi2.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Oogi2.AspNetCore3.Identity
{
    public class IdentityRole : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public IList<Claim> Claims { get; set; } = new List<Claim>();

        public override string PartitionKey => _partitionKey;
        public override string Entity => _entity;

        readonly string _partitionKey = string.Empty;
        readonly string _entity = string.Empty;

        public IdentityRole()
        {
        }

        public IdentityRole(string entity, string partitionKey = null)
        {
            _entity = entity;
            _partitionKey = partitionKey;
        }
    }
}