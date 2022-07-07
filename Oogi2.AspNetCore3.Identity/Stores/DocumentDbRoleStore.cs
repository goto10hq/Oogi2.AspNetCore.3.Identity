using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;
using Oogi2.Queries;

namespace Oogi2.AspNetCore3.Identity.Stores
{
    /// <summary>
    /// Represents a DocumentDb-based persistence store for ASP.NET Core Identity roles
    /// </summary>
    /// <typeparam name="TRole">The type representing a role</typeparam>
    public class DocumentDbRoleStore<TRole> : StoreBase, IRoleClaimStore<TRole>
        where TRole : IdentityRole, new()
    {
        readonly Repository<TRole> _repository;
        readonly string _entity = null;        

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbRoleStore{TRole}"/>
        /// </summary>
        /// <param name="connection">The DocumentDb client to be used</param>
        public DocumentDbRoleStore(IConnection connection)
            : base(connection)
        {
            _repository = new Repository<TRole>(connection);
            _entity = new TRole().Entity;
        }

        public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult(role.Claims);
        }

        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            role.Claims.Add(claim);

            return Task.CompletedTask;
        }

        public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            role.Claims.Remove(claim);

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            // If no RoleId was specified, generate one
            if (role.Id == null)
            {
                role.Id = Guid.NewGuid().ToString();
            }

            var result = await _repository.CreateAsync(role).ConfigureAwait(false);

            return result != null ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var result = await _repository.ReplaceAsync(role).ConfigureAwait(false);

            return result == null ? IdentityResult.Failed() : IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var result = await _repository.DeleteAsync(role).ConfigureAwait(false);

            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            role.Name = roleName ?? throw new ArgumentNullException(nameof(roleName));

            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (role == null)
                throw new ArgumentNullException(nameof(role));

            role.NormalizedName = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));

            return Task.CompletedTask;
        }

        public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (roleId == null)
                throw new ArgumentNullException(nameof(roleId));

            var role = await _repository.GetFirstOrDefaultAsync(roleId).ConfigureAwait(false);

            return role;
        }

        public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (normalizedRoleName == null)
                throw new ArgumentNullException(nameof(normalizedRoleName));

            var dynamicQuery = new DynamicQuery
                (
                $"select top 1 * from c where c.normalizedName = @normalizedRoleName {EntityTypeConstraint}",
                new
                {
                    normalizedRoleName
                }
                );

            var role = await _repository.GetFirstOrDefaultAsync(dynamicQuery).ConfigureAwait(false);

            return role;
        }

        string EntityTypeConstraint
        {
            get
            {                
                if (_entity != null)
                {
                    var q = new DynamicQuery($" and c[\"entity\"] = @val ", new { val = _entity });

                    var sql = q.ToSqlQuery();

                    return sql;
                }

                return null;
            }
        }

        #region IDisposable Support

        public void Dispose()
        {
            // TODO: Workaround, gets disposed too early currently
            disposed = false;
        }

        #endregion IDisposable Support
    }
}