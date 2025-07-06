namespace Service.Interfaces
{
    public interface IService<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);

        Task<T> AddItem(T item);
        Task DeleteItem(int id);
        Task Update(int id, T item);
    }
}
