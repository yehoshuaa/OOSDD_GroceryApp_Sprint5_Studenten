using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _links;

        public ProductCategoryService(IProductCategoryRepository links)
        {
            _links = links;
        }

        public List<Product> GetProductsByCategory(int categoryId) => _links.GetProductsByCategory(categoryId);
        public List<Category> GetCategoriesByProduct(int productId) => _links.GetCategoriesByProduct(productId);

        public void AddProductToCategory(int productId, int categoryId)
        {
            if (_links.Exists(productId, categoryId)) return; // idempotent
            _links.Add(new ProductCategory(productId, categoryId));
        }

        public void RemoveProductFromCategory(int productId, int categoryId)
        {
            _links.Remove(new ProductCategory(productId, categoryId));
        }

        public bool IsLinked(int productId, int categoryId) => _links.Exists(productId, categoryId);
    }
}

