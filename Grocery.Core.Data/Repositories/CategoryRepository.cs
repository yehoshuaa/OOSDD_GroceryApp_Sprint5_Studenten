using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;

namespace Grocery.Core.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Dictionary<int, Category> _store = new();
        private int _nextId = 1;

        // Eén constructor die seed verwerkt óf default seed toevoegt
        public CategoryRepository(IEnumerable<Category>? seed = null)
        {
            if (seed != null && seed.Any())
            {
                foreach (var c in seed)
                {
                    _store[c.Id] = c;
                    _nextId = Math.Max(_nextId, c.Id + 1);
                }
            }
            else
            {
                // eenvoudige default seed zodat de UI iets toont
                var defaults = new[]
                {
                    new Category(1, "Groente"),
                    new Category(2, "Fruit"),
                    new Category(3, "Zuivel"),
                    new Category(4, "Brood")
                };
                foreach (var c in defaults)
                {
                    _store[c.Id] = c;
                    _nextId = Math.Max(_nextId, c.Id + 1);
                }
            }
        }

        public List<Category> GetAll() => _store.Values.OrderBy(c => c.Name).ToList();

        public Category? Get(int id) => _store.TryGetValue(id, out var c) ? c : null;

        public Category Add(Category category)
        {
            if (category.Id == 0) category = new Category(_nextId++, category.Name);
            _store[category.Id] = category;
            return category;
        }

        public Category? Delete(Category category)
        {
            return _store.Remove(category.Id, out var removed) ? removed : null;
        }

        public Category Update(Category category)
        {
            _store[category.Id] = category;
            return category;
        }
    }
}