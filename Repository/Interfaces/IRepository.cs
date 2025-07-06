namespace Repository.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        IQueryable<T> Query(); 
        Task<T> GetById(int id);
        Task<T> AddItem(T item);
        Task UpdateItem(int id, T item);
        Task<T> DeleteItem(int id);
    }
}
