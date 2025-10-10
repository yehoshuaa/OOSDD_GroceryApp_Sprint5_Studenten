using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category? Get(int id);
        Category Add(string name);
        Category Update(Category category);
        Category? Delete(int id);
    }
}

