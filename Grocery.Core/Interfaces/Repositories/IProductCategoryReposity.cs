using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        List<ProductCategory> GetAll();
        List<Product> GetProductsByCategory(int categoryId);
        List<Category> GetCategoriesByProduct(int productId);

        ProductCategory Add(ProductCategory link);
        bool Remove(ProductCategory link);
        bool Exists(int productId, int categoryId);
    }
}

