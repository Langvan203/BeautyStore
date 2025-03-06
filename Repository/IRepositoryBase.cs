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
    }
}
