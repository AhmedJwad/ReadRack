using ReadRack.Shared.DTOs;
using ReadRack.Shared.Responses;

namespace ReadRack.Backend.UnitsOfWork.Interfaces
{
    public interface IGenericUnitOfWork<T> where T : class
    {
        Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO pagination);
        Task<ActionResponse<IEnumerable<T>>> GetAsync();
        Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
        Task<ActionResponse<T>> AddAsync(T Model);
        Task<ActionResponse<T>> UpdateAsync(T Model);
        Task<ActionResponse<T>> DeleteAsync(int Id);
        Task<ActionResponse<T>> GetAsync(int Id);
    }
}
