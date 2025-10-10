using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        // Bestaande ProductRepository voor producten
        private readonly IProductRepository _products;
        private readonly ICategoryRepository _categories;

        // In-memory links
        private readonly HashSet<(int productId, int categoryId)> _links = new();

        public ProductCategoryRepository(IProductRepository products, ICategoryRepository categories)
        {
            _products = products;
            _categories = categories;
        }

        public List<ProductCategory> GetAll()
            => _links.Select(l => new ProductCategory(l.productId, l.categoryId)).ToList();

        public List<Product> GetProductsByCategory(int categoryId)
        {
            var ids = _links.Where(l => l.categoryId == categoryId).Select(l => l.productId).ToHashSet();
            return _products.GetAll().Where(p => ids.Contains(p.Id)).ToList();
        }

        public List<Category> GetCategoriesByProduct(int productId)
        {
            var ids = _links.Where(l => l.productId == productId).Select(l => l.categoryId).ToHashSet();
            return _categories.GetAll().Where(c => ids.Contains(c.Id)).ToList();
        }

        public ProductCategory Add(ProductCategory link)
        {
            _links.Add((link.ProductId, link.CategoryId));
            return link;
        }

        public bool Remove(ProductCategory link)
            => _links.Remove((link.ProductId, link.CategoryId));

        public bool Exists(int productId, int categoryId)
            => _links.Contains((productId, categoryId));
    }
}

