﻿namespace ReadRack.Fronend.Services
{
    public interface ILoginService
    {
        Task LoginAsync(string token);
        Task LogoutAsync();

    }
}
