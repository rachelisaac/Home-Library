

namespace Repository.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        IQueryable<T> Query();
        T GetById(int id);
        T AddItem(T item);
        void UpdateItem(int id, T item);
        T DeleteItem(int id);
    }
}
