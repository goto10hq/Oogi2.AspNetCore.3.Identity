namespace Oogi2.AspNetCore3.Identity.Tests.Entities
{    
    public class TestIdentityUser : IdentityUser<TestIdentityRole>
    {
        public TestIdentityUser() : base("oogi2/user", "oogi2")
        {
        }        
    }
}