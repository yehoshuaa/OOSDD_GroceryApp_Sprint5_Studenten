using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category? Get(int id);
        Category Add(Category category);
        Category? Delete(Category category);
        Category Update(Category category);
    }
}

