using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EquipmentRentalSystem.Models;
using EquipmentRentalSystem.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace EquipmentRentalSystem.ViewModels
{
    public class CategoryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Category> _categories;
        private readonly GenericityService _genericityService;
        
        // make pagenations
        public int pageSize = 10;
        public int currentPage = 1;
        public List<Category> pagedCategories => _categories.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        public int totalPages => (int)Math.Ceiling((double)_categories.Count / pageSize);

        public CategoryViewModel(GenericityService genericityService)
        {
            _genericityService = genericityService;
            _categories = new ObservableCollection<Category>();
        }

        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged();
                }
            }
        }

        public void PreviousPage()
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
        }

        public void NextPage()
        {
            if (currentPage < totalPages)
            {
                currentPage++;
            }
        }

        public async Task LoadCategoriesAsync()
        {
            var categories = await _genericityService.GetObjects<Category>();
            Categories = new ObservableCollection<Category>(categories);
        }

        public async Task Search(Dictionary<string, string> filters)
        {
            var result = await _genericityService.Search<Category>(filters);
            Categories = new ObservableCollection<Category>(result);
        }

        public async Task<Boolean> CheckExist(int id, string name)
        {
            var existingCategory = await _genericityService._context.Categories
                                     .FirstOrDefaultAsync(c => c.ID == id || c.Name == name);
            if (existingCategory != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _genericityService.AddItemAsync(category);
            Categories.Add(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _genericityService.UpdateItemAsync(category);
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await _genericityService.DeleteItemAsync<Category>(category);
            Categories.Remove(category);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
