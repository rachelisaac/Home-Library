using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    internal interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(int id);
        T AddItem(T item);
        T UpdateItem(T item);
        T DeleteItem(int id);


    }
}
