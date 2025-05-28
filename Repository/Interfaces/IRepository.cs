

namespace Repository.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(int id);
        T AddItem(T item);
        void UpdateItem(int id, T item);
        T DeleteItem(int id);

    }
}
