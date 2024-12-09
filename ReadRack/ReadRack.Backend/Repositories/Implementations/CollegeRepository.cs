using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadRack.Backend.Data;
using ReadRack.Backend.Helpers;
using ReadRack.Backend.Repositories.Interfaces;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Entites;
using ReadRack.Shared.Responses;
using System;

namespace ReadRack.Backend.Repositories.Implementations
{
    public class CollegeRepository :GenericRepository<College>, ICollegesRepository
    {
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;

        public CollegeRepository(DataContext context , IFileStorage fileStorage):base(context)
        {
           _context = context;
           _fileStorage = fileStorage;
        }
        public async Task<ActionResponse<College>> AddFullAsync(College college)
        {
            try
            {
                if (!string.IsNullOrEmpty(college.Photo))
                {
                    var photoUser = Convert.FromBase64String(college.Photo);
                    college.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", "colleges");
                };

                _context.Add(college);
                await _context.SaveChangesAsync();
                return new ActionResponse<College>
                {
                    WasSuccess = true,
                    Result = college
                };
            }
            catch (DbUpdateException)
            {
                return new ActionResponse<College>
                {
                    WasSuccess = false,
                    Message = "A college with the same name already exists."
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<College>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };
            }
        }

        [HttpPut]

        public async Task<ActionResponse<College>> PutAsync(College college)
        {
            try
            {
                var currentcollege = await _context.colleges.FirstOrDefaultAsync(x => x.Id == college.Id);
                if (currentcollege == null)
                {
                    return new ActionResponse<College>
                    {
                        WasSuccess = false,
                        Message = "college does not exist"
                    };
                }
                if (!string.IsNullOrEmpty(college.Photo))
                {
                    var photoUser = Convert.FromBase64String(college.Photo);
                    college.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", "colleges");
                }
                currentcollege.Name = college.Name;

                currentcollege.Photo = !string.IsNullOrEmpty(college.Photo) && college.Photo != currentcollege.Photo ? college.Photo :
                    currentcollege.Photo;

                _context.Update(currentcollege);
                await _context.SaveChangesAsync();
                return new ActionResponse<College>
                {
                    WasSuccess = true,
                    Result = college
                };

            }
            catch (Exception ex)
            {

                return new ActionResponse<College>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public override async Task<ActionResponse<College>> GetAsync(int id)
        {
            var college =await _context.colleges.Include(x=>x.Departments!)
                .FirstOrDefaultAsync(x=>x.Id == id);

            if(college==null)
            {
                return new ActionResponse<College>
                {
                    WasSuccess=false,
                    Message="college not exist"
                };
            }
            return new ActionResponse<College>
            {
                WasSuccess=true,
                Result=college,
            };
        }

        public override async Task<ActionResponse<IEnumerable<College>>> GetAsync()
        {
            var college = await _context.colleges.OrderBy(x => x.Name).ToListAsync();
            return new ActionResponse<IEnumerable<College>>
            {
                WasSuccess = true,
                Result = college,
            };
        }
        

        public override async Task<ActionResponse<IEnumerable<College>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.colleges.AsQueryable();

            if(!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable=queryable.Where(x=>x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return new ActionResponse<IEnumerable<College>>
            {
                WasSuccess = true,
                Result = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync()
            };
        }

        public async Task<IEnumerable<College>> GetComboAsync()
        {
            return await _context.colleges.OrderBy(x=>x.Name).ToListAsync();
        }

        public override async Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO pagination)
        {
            var queryable = _context.colleges.AsQueryable();
            if(!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable=queryable.Where(x=>x.Name.ToLower().Contains($"{pagination.Filter.ToLower()}"));
            }
            int recordsNumber=await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = recordsNumber,
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.colleges.AsQueryable();
            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains($"{pagination.Filter.ToLower()}"));
            }
            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count / pagination.RecordsNumber);
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = totalPages,
            };
        }
    }
}
