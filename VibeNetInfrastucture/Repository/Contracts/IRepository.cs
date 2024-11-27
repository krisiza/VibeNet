namespace VibeNet.Infrastucture.Repository.Contracts
{
    public interface IRepository<TType, TId>
    {
        TType? GetById(TId id);

        Task<TType?> GetByIdAsync(TId id);

        IEnumerable<TType> GetAll();

        Task<IEnumerable<TType>> GetAllAsync();

        IQueryable<TType> GetAllAttached();

        void Add(TType item);

        Task AddAsync(TType item);

        bool Delete(TId id);

        Task<bool> DeleteAsync(TId id);

        bool DeleteEntity(TType entity);

        Task<bool> DeleteEntityAsync(TType entity);

        Task<bool> DeleteEntityRangeAsync(IEnumerable<TType> entities);

        bool Update(TType item);

        Task<bool> UpdateAsync(TType item);
    }
}
