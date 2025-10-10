using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using CommunityToolkit.Mvvm.Input;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(CategoryId), "categoryId")]
    public partial class ProductCategoriesViewModel : ObservableObject
    {
        private readonly IProductCategoryService _links;
        private readonly ICategoryService _categories;

        [ObservableProperty] private int categoryId;
        [ObservableProperty] private string categoryName = string.Empty;
        [ObservableProperty] private ObservableCollection<Product> products = new();

        public ProductCategoriesViewModel(IProductCategoryService links, ICategoryService categories)
        {
            _links = links;
            _categories = categories;
        }

        partial void OnCategoryIdChanged(int value)
        {
            var cat = _categories.Get(value);
            CategoryName = cat?.Name ?? $"Categorie {value}";
            Products = new ObservableCollection<Product>(_links.GetProductsByCategory(value));
        }

        // Optioneel: command om product te verwijderen/toe te voegen
        [RelayCommand]
        private void Remove(Product p)
        {
            _links.RemoveProductFromCategory(p.Id, CategoryId);
            Products.Remove(p);
        }
    }
}

