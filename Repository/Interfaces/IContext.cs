
using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    //ממשק המייצג את הנתונים 
    public interface IContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<Category> Categories { get; set; }

        void Save();
    }

}