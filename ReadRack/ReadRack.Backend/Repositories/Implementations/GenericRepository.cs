using Microsoft.EntityFrameworkCore;
using ReadRack.Backend.Data;
using ReadRack.Backend.Helpers;
using ReadRack.Backend.Repositories.Interfaces;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Responses;
using System;

namespace ReadRack.Backend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(DataContext context)
        {
           _context = context;
           _entity= _context.Set<T>();            
        }
        public virtual async Task<ActionResponse<T>> AddAsync(T Entity)
        {
            _context.Add(_entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                    Result = Entity
                };
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null)
                {
                    if (ex.InnerException!.Message.Contains("duplicate"))
                    {
                        return DbUpdateExceptionActionResponse();
                    }
                }
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = ex.Message
                };
            }
            catch (Exception exception) 
            {
                return ExceptionActionResponse(exception);
            }
        }

        
        public virtual async Task<ActionResponse<T>> DeleteAsync(int Id)
        {
            var row = await _entity.FindAsync(Id);
            if(row==null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess=false,
                    Message= "Record not found",
                };
            }
            try
            {
                _entity.Remove(row);
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                };
            }
            catch 
            {

                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Cannot be deleted because it has related records."
                };
            }
        }

        public virtual async Task<ActionResponse<T>> GetAsync(int Id)
        {
          var row= await _entity.FindAsync(Id);
            if(row==null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Record not found"
                };
            }
            return new ActionResponse<T>
            {
                WasSuccess = true,
                Result = row,
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true,
                Result =await _entity.ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO paginationDTO)
        {
            var queryable = _entity.AsQueryable();
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess =true,
                Result=await queryable.Paginate(paginationDTO).ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO paginationDTO)
        {
            var queryable = _entity.AsQueryable();
            int recordsNumber=await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WasSuccess=true,
                Result=recordsNumber,
            };
        }

        public Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO paginationDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResponse<T>> UpdateAsync(T Entity)
        {
            throw new NotImplementedException();
        }
        private ActionResponse<T> DbUpdateExceptionActionResponse( )
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "The record you are trying to create already exists."
            };
        }

        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = exception.Message,
            };
        }

    }
}
