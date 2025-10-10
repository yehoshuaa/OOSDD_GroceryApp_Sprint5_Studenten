using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(CategoryId), "categoryId")]
    public partial class ProductCategoriesViewModel : ObservableObject
    {
        private readonly IProductCategoryService _links;
        private readonly ICategoryService _categories;
        private readonly IProductService _products;

        [ObservableProperty] private int categoryId;
        [ObservableProperty] private string categoryName = string.Empty;
        [ObservableProperty] private ObservableCollection<Product> assigned = new(); // in categorie
        [ObservableProperty] private ObservableCollection<Product> available = new(); // nog niet in categorie
        [ObservableProperty] private string searchText = string.Empty;

        public ProductCategoriesViewModel(IProductCategoryService links, ICategoryService categories, IProductService products)
        {
            _links = links;
            _categories = categories;
            _products = products;
        }

        partial void OnCategoryIdChanged(int value) => Refresh();

        private void Refresh()
        {
            var cat = _categories.Get(CategoryId);
            CategoryName = cat?.Name ?? $"Categorie {CategoryId}";

            var inCat = _links.GetProductsByCategory(CategoryId);
            Assigned = new ObservableCollection<Product>(inCat);

            var all = _products.GetAll();
            var set = inCat.Select(p => p.Id).ToHashSet();
            var notInCat = all.Where(p => !set.Contains(p.Id)).ToList();
            ApplySearch(notInCat);
        }

        private void ApplySearch(IEnumerable<Product> candidates)
        {
            var q = (SearchText ?? string.Empty).Trim();
            if (q.Length > 0)
                candidates = candidates.Where(p => p.Name.Contains(q, StringComparison.OrdinalIgnoreCase));

            Available = new ObservableCollection<Product>(candidates);
        }

        [RelayCommand] private void Search() => Refresh();

        [RelayCommand]
        private void AddToCategory(Product? product)
        {
            if (product is null) return;
            _links.AddProductToCategory(product.Id, CategoryId);
            // direct de UI-lijsten bijwerken
            Assigned.Add(product);
            Available.Remove(product);
        }

        [RelayCommand]
        private void RemoveFromCategory(Product? product)
        {
            if (product is null) return;
            _links.RemoveProductFromCategory(product.Id, CategoryId);
            Assigned.Remove(product);
            // mag terug naar available zodat je ‘m weer kunt toevoegen
            Available.Add(product);
        }
    }
}


