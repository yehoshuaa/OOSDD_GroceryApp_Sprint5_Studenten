using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        private readonly ICategoryService _categories;

        [ObservableProperty] private ObservableCollection<Category> items = new();

        public CategoriesViewModel(ICategoryService categories)
        {
            _categories = categories;
            Load();
        }

        private void Load()
        {
            Items = new ObservableCollection<Category>(_categories.GetAll());
        }

        [RelayCommand]
        private async Task OpenCategoryAsync(Category? category)
        {
            if (category is null) return;
            // Navigeer naar detail (ProductCategoriesView) met categoryId
            await Shell.Current.GoToAsync($"productcategories?categoryId={category.Id}");
        }
    }
}
