using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace SocialNetwork.Helpers.Auth
{
    public class AuthOptions
    {
        public const String Issuer = "ToyotaAuthServer"; // издатель токена
        public const String Audince = "AuthClient"; // потребитель токена
        public const int Lifetime = 60; // время жизни токена - 1 час

        private const String key = "1mVKANeW4P6QdyAwNknGOGwie94vffEGJFXhLiqQfJibwSjkutS8tuWE9IYpcgN";   // ключ для шифрации

        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
    }
}
