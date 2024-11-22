using ReadRack.Backend.Repositories.Interfaces;
using ReadRack.Backend.UnitsOfWork.Interfaces;
using ReadRack.Shared.DTOs;
using ReadRack.Shared.Responses;

namespace ReadRack.Backend.UnitsOfWork.Implementations
{
    public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericUnitOfWork(IGenericRepository<T> repository)
        {
           _repository = repository;
        }
        public virtual async Task<ActionResponse<T>> AddAsync(T Model)
         =>await _repository.AddAsync(Model);

        public virtual async Task<ActionResponse<T>> DeleteAsync(int Id)
        => await _repository.DeleteAsync(Id);

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        => await _repository.GetAsync();

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        => await  _repository.GetAsync(pagination);

        public virtual async Task<ActionResponse<T>> GetAsync(int Id)
        => await _repository.GetAsync(Id);

        public virtual async Task<ActionResponse<int>> GetRecordsNumberAsync(PaginationDTO pagination)
        => await _repository.GetRecordsNumberAsync(pagination);

        public virtual async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        =>await _repository.GetTotalPagesAsync(pagination);

        public virtual async Task<ActionResponse<T>> UpdateAsync(T Model)
        => await _repository.UpdateAsync(Model);
    }
}
