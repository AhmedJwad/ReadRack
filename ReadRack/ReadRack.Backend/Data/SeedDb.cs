
using ReadRack.Shared.Entites;

namespace ReadRack.Backend.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

       public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCollegesAsync();
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
