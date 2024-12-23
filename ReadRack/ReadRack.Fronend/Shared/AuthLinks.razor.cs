﻿using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using ReadRack.Fronend.Pages.Auth;

namespace ReadRack.Fronend.Shared
{
    public partial class AuthLinks
    {
        private string? photoUser;
        [Inject] public NavigationManager navigationManager { get; set; } = null!;

        [CascadingParameter]

        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            var claim = authenticationState.User.Claims.ToList();
            var photoClaim = claim.FirstOrDefault(X => X.Type == "Photo");
            if (photoClaim is not null)
            {
                var imageName = Path.GetFileName(photoClaim.Value);
                photoUser = $"https://localhost:7186/images/users/{imageName}";
            }

        }
        private void ShowModal()
        {
            Modal.Show<Login>();
        }
    }
}
