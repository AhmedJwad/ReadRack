﻿using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ReadRack.Fronend.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var user = new ClaimsIdentity(authenticationType: "test");
            var admin = new ClaimsIdentity(new List<Claim>
            {
                new Claim("FirstName", "Ahmed"),
                new Claim("LastName", "Almershady"),
                new Claim(ClaimTypes.Name, "Ahmednet380@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            }, authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        }
    }
}
