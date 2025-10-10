using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product Add(Product item)
        {
            // (Buiten scope UC14 – laat NotImplemented of implementeer later)
            throw new NotImplementedException();
        }

        public Product? Delete(Product item)
        {
            // (Buiten scope UC14)
            throw new NotImplementedException();
        }

        public Product? Get(int id)
        {
            // Gebruik repository i.p.v. NotImplemented
            return _productRepository.Get(id);
        }

        public Product? Update(Product item)
        {
            return _productRepository.Update(item);
        }

        /// <summary>
        /// UC14 – Prijs toevoegen/aanpassen voor bestaand product.
        /// </summary>
        public Product AddPrice(int productId, decimal newPrice)
        {
            var product = _productRepository.Get(productId);
            if (product is null)
                throw new InvalidOperationException("Product niet gevonden.");

            product.UpdatePrice(newPrice);
            _productRepository.Update(product);
            return product;
        }
    }
}
