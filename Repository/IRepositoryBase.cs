using System.Linq.Expressions;

namespace my_cosmetic_store.Repository
{
    interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T Create(T entity);
        T UpdateByEntity(T entity);
        bool DeleteByEntity(T entity);
        IQueryable<T> GetTopItemsByCondition(Expression<Func<T, bool>> expression, int number);
        IQueryable<T> GetTopItems(int number);

        void AddRangeAsync(IEnumerable<T> items);

        void UpdateRange(IEnumerable<T> items);

    }
}
