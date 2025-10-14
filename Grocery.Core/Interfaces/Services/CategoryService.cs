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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public List<Category> GetAll() => _repo.GetAll();
        public Category? Get(int id) => _repo.Get(id);

        public Category Add(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Naam is verplicht.", nameof(name));

            return _repo.Add(new Category(0, name.Trim()));
        }

        public Category Update(Category category)
        {
            if (category.Id <= 0) throw new ArgumentException("Ongeldige id.", nameof(category));
            return _repo.Update(category);
        }

        public Category? Delete(int id)
        {
            var existing = _repo.Get(id);
            return existing is null ? null : _repo.Delete(existing);
        }
    }
}

