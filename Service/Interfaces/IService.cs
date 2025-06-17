namespace Service.Interfaces
{
    public interface IService<T>
    {
        List<T> GetAll();
        T GetById(int id);

        T AddItem(T item);
        void DeleteItem(int id);
        void Update(int id, T item);
    }
}