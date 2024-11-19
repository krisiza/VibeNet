using Microsoft.EntityFrameworkCore;
using VibeNet.Infrastucture.Data;
using VibeNet.Infrastucture.Repository.Contracts;

namespace VibeNet.Infrastucture.Repository
{
    public class BaseRepository<TType, TId> : IRepository<TType, TId>
        where TType : class
    {
        private readonly VibeNetDbContext context;
        private readonly DbSet<TType> dbSet;

        public BaseRepository(VibeNetDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<TType>();
        }

        public TType? GetById(TId id)
            =>  this.dbSet.Find(id);

        public async Task<TType?> GetByIdAsync(TId id)
            => await this.dbSet.FindAsync(id);

        public IEnumerable<TType> GetAll()
            => dbSet.ToArray();

        public async Task<IEnumerable<TType>> GetAllAsync()
            => await dbSet.ToArrayAsync();

        public IQueryable<TType> GetAllAttached()
            => dbSet.AsQueryable();

        public void Add(TType item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await dbSet.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public bool Delete(TId id)
        {
            TType? entity = GetById(id);

            if(entity == null) return false;

            dbSet.Remove(entity);
            context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            TType? entity = await GetByIdAsync(id);

            if (entity == null) return false;

            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public bool DeleteEntity(TType entity)
        {
            if (entity == null) return false;

            dbSet.Remove(entity);
            context.SaveChanges();
            return true;
        }

        public async Task<bool> DeleteEntityAsync(TType entity)
        {         
            if (entity == null) return false;

            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEntityAsyncNotSaving(TType entity)
        {
            if (entity == null) return false;

            dbSet.Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public bool Update(TType item)
        {
            try
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                dbSet.Attach(item);
                context.Entry(item).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
