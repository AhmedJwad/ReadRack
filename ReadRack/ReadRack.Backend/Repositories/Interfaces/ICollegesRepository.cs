using ReadRack.Shared.DTOs;
using ReadRack.Shared.Entites;
using ReadRack.Shared.Responses;

namespace ReadRack.Backend.Repositories.Interfaces
{
    public interface ICollegesRepository
    {
        Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO pagination);
        Task<ActionResponse<College>> GetAsync(int id);
        Task<ActionResponse<IEnumerable<College>>> GetAsync();
        Task<ActionResponse<IEnumerable<College>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination);
        Task<IEnumerable<College>> GetComboAsync();
        Task<ActionResponse<College>> AddFullAsync(College college);
        Task<ActionResponse<College>> PutAsync(College college);
    }
}
