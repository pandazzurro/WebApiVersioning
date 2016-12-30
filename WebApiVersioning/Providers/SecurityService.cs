using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebApi.Binder.Providers
{
    public class SecurityService : IDisposable
    {
        private const string AdminUsername = "admin";
        private const string UserUsername = "user";
        private const string AdminPassword = "admin";
        private const string UserPassword = "user";
        private const string AdminEmail = "admin@m.com";
        private const string UserEmail= "user@m.com";
        
        public ClaimsPrincipal CreatePrincipalForUsername(string username)
        {
            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            
            switch (username)
            {
                case AdminUsername:
                    claims.Add(new Claim(ClaimTypes.Email, AdminEmail));
                    break;
                case UserUsername:
                    claims.Add(new Claim(ClaimTypes.Email, AdminEmail));
                    break;
                default:
                    Log.Warning($"Creazione principal per {username} fallita: utente non esiste");
                    return null;
            }

            Log.Debug($"Creato cliamsPrincipal per user {username}");
            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        public UserValidationResult ValidateUser(string username,string password)
        {
            switch (username)
            {
                case AdminUsername:
                    return password == AdminPassword ? UserValidationResult.Valid:UserValidationResult.InvalidCredentials;
                case UserUsername:
                    return password == UserPassword ? UserValidationResult.Valid : UserValidationResult.InvalidCredentials;
                default:
                    return UserValidationResult.UserNotExtists;
            }
        }

        public void Dispose()
        {
        }
    }

    public enum UserValidationResult
    {
        Valid,
        UserNotExtists,
        InvalidCredentials
    }
}