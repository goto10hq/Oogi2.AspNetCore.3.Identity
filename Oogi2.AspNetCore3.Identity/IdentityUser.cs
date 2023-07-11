using Microsoft.AspNetCore.Identity;
using Oogi2.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Oogi2.AspNetCore3.Identity
{
    /// <summary>
    /// Represents a user in the identity system for the <see cref="Stores.DocumentDbUserStore{TUser, TRole}"/> with the role type defaulted to <see cref="IdentityRole"/>
    /// </summary>
    public class IdentityUser : IdentityUser<IdentityRole>
    {
    }

    /// <summary>
    /// Represents a user in the identity system for the <see cref="Stores.DocumentDbUserStore{TUser, TRole}"/>
    /// </summary>
    public class IdentityUser<TRole> : BaseEntity
    {        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NormalizedUserName { get; set; }
        public string NormalizedEmail { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool IsTwoFactorAuthEnabled { get; set; }
        public IList<UserLoginInfo> Logins { get; set; } = new List<UserLoginInfo>();
        public IList<TRole> Roles { get; set; } = new List<TRole>();
        public IList<Claim> Claims { get; set; } = new List<Claim>();
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEndDate { get; set; }
        public int AccessFailedCount { get; set; }

        public override string PartitionKey => _partitionKey;
        public override string Entity => _entity;

        readonly string _partitionKey = string.Empty;
        readonly string _entity = string.Empty;

        public IdentityUser()
        {
        }

        public IdentityUser(string entity, string partitionKey = null)
        {
            _entity = entity;
            _partitionKey = partitionKey;
        }
    }
}