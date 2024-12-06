using ReadRack.Backend.Repositories.Interfaces;
using ReadRack.Backend.UnitsOfWork.Interfaces;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Entites;
using ReadRack.Shared.Responses;

namespace ReadRack.Backend.UnitsOfWork.Implementations
{
    public class CollegesUnitOfWork :GenericUnitOfWork<College>, IcollegesUnitOfWork
    {
        private readonly ICollegesRepository _collegesRepository;

        public CollegesUnitOfWork(IGenericRepository<College> repository, ICollegesRepository  collegesRepository) : base(repository)
        {
           _collegesRepository = collegesRepository;
        }

        public async Task<ActionResponse<College>> AddFullAsync(College college)
        => await _collegesRepository.AddFullAsync(college);

        public override async Task<ActionResponse<College>> GetAsync(int id)
        => await _collegesRepository.GetAsync(id);

        public override async Task<ActionResponse<IEnumerable<College>>> GetAsync()
        => await _collegesRepository.GetAsync();

        public override async Task<ActionResponse<IEnumerable<College>>> GetAsync(PaginationDTO pagination)
        => await _collegesRepository.GetAsync(pagination);

        public async Task<IEnumerable<College>> GetComboAsync()
        =>await _collegesRepository.GetComboAsync();

        public override async Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO pagination)
         => await _collegesRepository.GetRecordsNumberAsync(pagination);

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        => await _collegesRepository.GetTotalPagesAsync(pagination);

        public async Task<ActionResponse<College>> PutAsync(College college)
        => await _collegesRepository.PutAsync(college);
    }
}
