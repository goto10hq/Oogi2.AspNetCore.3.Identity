using Microsoft.AspNetCore.Identity;

namespace Oogi2.AspNetCore3.Identity
{
    public class LookupNormalizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.Normalize().ToLowerInvariant();
        }

        public string NormalizeEmail(string email)
        {
            return email.Normalize().ToLowerInvariant();
        }

        public string NormalizeName(string name)
        {
            return name.Normalize().ToLowerInvariant();
        }
    }
}