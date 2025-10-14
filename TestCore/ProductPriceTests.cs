using System;
using System.Collections.Generic;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using Grocery.Core.Services;
using NUnit.Framework;

namespace TestCore
{
     public class ProductPriceTests
    {
        // Eenvoudige in-memory repository [tijdelijke opslag] voor testen
        private sealed class InMemoryProductRepository : IProductRepository
        {
            private readonly Dictionary<int, Product> _store = new();

            public InMemoryProductRepository(IEnumerable<Product>? seed = null)
            {
                if (seed != null)
                {
                    foreach (var p in seed)
                        _store[p.Id] = p;
                }
            }

            public List<Product> GetAll() => new List<Product>(_store.Values);

            public Product? Get(int id) => _store.TryGetValue(id, out var p) ? p : null;

            public Product Add(Product product)
            {
                _store[product.Id] = product;
                return product;
            }

            public Product? Delete(Product product)
            {
                return _store.Remove(product.Id, out var removed) ? removed : null;
            }

            public Product Update(Product product)
            {
                _store[product.Id] = product;
                return product;
            }
        }

        private InMemoryProductRepository _repo = null!;
        private ProductService _service = null!;

        [SetUp]
        public void Setup()
        {
            // Seed met 1 product zonder prijs
            var seeded = new List<Product>
            {
                // constructor-chain: id, name, stock  (shelfLife/price defaulten naar 0)
                new Product(1, "Milk", 10)
            };

            _repo = new InMemoryProductRepository(seeded);
            _service = new ProductService(_repo);
        }

        // FR14-1: Happy flow – prijs toevoegen
        [Test]
        public void AddPrice_SetsPrice_ForExistingProduct()
        {
            var updated = _service.AddPrice(1, 1.99m);

            Assert.That(updated, Is.Not.Null);
            Assert.That(updated.Price, Is.EqualTo(1.99m));

            var fromRepo = _repo.Get(1)!;
            Assert.That(fromRepo.Price, Is.EqualTo(1.99m));
        }

        // FR14-2: Eén actieve prijs (overschrijven)
        [Test]
        public void AddPrice_OverwritesExistingPrice_KeepsSingleActive()
        {
            // initieel een prijs via domain
            var p = _repo.Get(1)!;
            p.UpdatePrice(1.25m);
            _repo.Update(p);

            // nieuwe prijs toevoegen via service
            var updated = _service.AddPrice(1, 1.49m);

            Assert.That(updated.Price, Is.EqualTo(1.49m));
            Assert.That(_repo.Get(1)!.Price, Is.EqualTo(1.49m));
        }

        // NFR14-1: Negatief bedrag afgewezen
        [TestCase(-0.01)]
        [TestCase(-3.50)]
        public void AddPrice_Throws_OnNegative(decimal amount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _service.AddPrice(1, amount));
        }

        // NFR14-1: Meer dan 2 decimalen afgewezen
        [Test]
        public void AddPrice_Throws_OnMoreThanTwoDecimals()
        {
            Assert.Throws<ArgumentException>(() => _service.AddPrice(1, 1.999m));
        }

        // NFR14-3: Product niet gevonden
        [Test]
        public void AddPrice_Throws_WhenProductNotFound()
        {
            Assert.Throws<InvalidOperationException>(() => _service.AddPrice(42, 2.49m));
        }

        // Extra: constructor-chain blijft consistent
        [Test]
        public void ConstructorChain_AllowsSettingPriceLater()
        {
            var p = _repo.Get(1)!;
            Assert.That(p.Price, Is.EqualTo(0.00m));

            var updated = _service.AddPrice(1, 3.40m);
            Assert.That(updated.Price, Is.EqualTo(3.40m));
        }
    }
}
