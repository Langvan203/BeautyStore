using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using my_cosmetic_store.Models;
using my_cosmetic_store.Utility;
using System.Linq.Expressions;

namespace my_cosmetic_store.Repository
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : class
    {
        protected DbSet<T> Model { get; set; }
        protected DatabaseContext Context { get; set; }
        protected ApiOptions ApiConfig { get; set; }
        public BaseRepository(ApiOptions apiConfig, DatabaseContext databaseContext)
        {
            Context = databaseContext;
            Model = databaseContext.Set<T>();
            ApiConfig = apiConfig;
        }
        public IQueryable<T> FindAll()
        {
            return Model.AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Model.Where(expression).AsNoTracking();
        }
        public async Task<int> CountByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await Model.CountAsync(expression);
        }
        public T Create(T entity)
        {
            try
            {
                var model = Model.Add(entity);
                return model.Entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public T UpdateByEntity(T entity)
        {
            try
            {
                Model.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool DeleteByEntity(T entity)
        {
            try
            {
                Model.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveChange()
        {
            Context.SaveChanges();
        }
        public async Task SaveChangeAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
