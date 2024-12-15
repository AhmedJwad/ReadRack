﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadRack.Backend.Data;
using ReadRack.Backend.Helpers;
using ReadRack.Backend.Repositories.Interfaces;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Entites;
using ReadRack.Shared.Responses;

namespace ReadRack.Backend.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UsersRepository(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager
            ,SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
           _roleManager = roleManager;
           _signInManager = signInManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
           return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExist=await _roleManager.RoleExistsAsync(roleName);
            if(!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName,
                });
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<ActionResponse<IEnumerable<User>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Users.Include(x => x.college).AsQueryable();
            if(!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FirstName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase)
                    || x.LastName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase));
            }
            return new ActionResponse<IEnumerable<User>>
            {
                WasSuccess = true,
                Result = await queryable.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).Paginate(pagination)
                .ToListAsync()
            };
        }

        public async Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO pagination)
        {
            var queryable = _context.Users.Include(x => x.college).AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FirstName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase)
                    || x.LastName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase));
            }
            int recodeNumber=await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recodeNumber,
            };
        }

        public async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Users.Include(x => x.college).AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FirstName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase)
                    || x.LastName.Contains(pagination.Filter, StringComparison.CurrentCultureIgnoreCase));
            }
            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = (int)totalPages,
            };
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _context.Users.Include(x => x.college).FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            var user = await _context.Users.Include(x => x.college).FirstOrDefaultAsync(x => x.Id ==userId.ToString());
            return user!;
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
           return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<SignInResult> LoginAsync(LoginDTO model)
        {
            var user=await _userManager.FindByEmailAsync(model.Email);
            
            if(user==null)
            {
                return SignInResult.Failed;
            }
            return await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
