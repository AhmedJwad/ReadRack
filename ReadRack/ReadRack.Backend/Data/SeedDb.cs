
using Microsoft.AspNetCore.Http.HttpResults;
using ReadRack.Backend.UnitsOfWork.Interfaces;
using ReadRack.Shared.Entites;
using ReadRack.Shared.Enums;

namespace ReadRack.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public SeedDb(DataContext context , IUsersUnitOfWork usersUnitOfWork)
        {
            _context = context;
           _usersUnitOfWork = usersUnitOfWork;
        }

       public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCollegesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Ahmed", "Almershady", "Ahmednet380@gmail.com", "07804468010", "Iraq , babil", UserType.Admin);

        }
        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }
        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user=await _usersUnitOfWork.GetUserAsync(email);
            if(user==null)
            {
                user = new User
                {
                    FirstName=firstName,
                    LastName=lastName,
                    Email=email,
                    UserName=email,
                    PhoneNumber=phone,
                    Address=address,
                    UserType=userType,
                    CountryCode="964",
                    college=_context.colleges.FirstOrDefault(),

                };
                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());   
            }
            return user;
        }

        

        private async Task CheckCollegesAsync()
        {
            if (!_context.colleges.Any())
            {
                _context.colleges.Add(new College
                {
                    Name = "Engineering",
                    Departments =
                  [
                      new Department()
                        {
                            Name = "Electricity",

                        },
                       new Department()
                        {
                            Name = "Mechanical",

                        },
                        new Department()
                        {
                            Name = "civil",

                        },

                    ]
                });
                _context.colleges.Add(new College
                {
                    Name = "Oman",
                    Departments =
                    [
                        new Department()
                        {
                            Name = "Ad DakhiliyahNursing",

                        },

                    ]
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
