using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IProductCategoryService
    {
        List<Product> GetProductsByCategory(int categoryId);
        List<Category> GetCategoriesByProduct(int productId);
        void AddProductToCategory(int productId, int categoryId);
        void RemoveProductFromCategory(int productId, int categoryId);
        bool IsLinked(int productId, int categoryId);
    }
}
