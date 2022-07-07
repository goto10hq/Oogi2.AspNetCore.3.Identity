using Oogi2.AspNetCore3.Identity.Stores;
using Oogi2.AspNetCore3.Identity.Tests.Fixtures;
using Xunit;
using Oogi2.AspNetCore3.Identity.Tests.Entities;
using Sushi2;

namespace Oogi2.AspNetCore3.Identity.Tests
{
    public abstract class StoreTestsBase : IClassFixture<DocumentDbFixture>
    {
        protected Repository<TestIdentityUser> _repoUsers;
        protected Repository<TestIdentityRole> _repoRoles;
        readonly Connection _connection;

        protected StoreTestsBase(DocumentDbFixture documentDbFixture)
        {
            _connection = documentDbFixture.Connection;
            _repoUsers = new Repository<TestIdentityUser>(documentDbFixture.Connection);
            _repoRoles = new Repository<TestIdentityRole>(documentDbFixture.Connection);
        }

        protected void CreateDocument(TestIdentityUser user)
        {
            AsyncTools.RunSync(() => _repoUsers.CreateAsync(user));
        }

        protected void CreateDocument(TestIdentityRole role)
        {
            AsyncTools.RunSync(() => _repoRoles.CreateAsync(role));
        }

        protected DocumentDbUserStore<TestIdentityUser, TestIdentityRole> CreateUserStore()
        {
            return new DocumentDbUserStore<TestIdentityUser, TestIdentityRole>(_connection);
        }

        protected DocumentDbRoleStore<TestIdentityRole> CreateRoleStore()
        {
            return new DocumentDbRoleStore<TestIdentityRole>(_connection);
        }
    }
}