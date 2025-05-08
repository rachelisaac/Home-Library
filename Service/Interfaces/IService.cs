using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
