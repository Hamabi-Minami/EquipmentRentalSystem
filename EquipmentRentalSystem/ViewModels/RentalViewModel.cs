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

namespace EquipmentRentalSystem.ViewModels
{
    public class RentalViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Rental> _rentals;
        private readonly GenericityService _genericityService;

        // make pagenations
        public int PageSize { get; set; } = 10;
        private int _currentPage = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PagedRentals));
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(HasPreviousPage));
                    OnPropertyChanged(nameof(HasNextPage));
                }
            }
        }

        public List<Rental> PagedRentals => _rentals.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        public int TotalPages => (int)Math.Ceiling((double)_rentals.Count / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public RentalViewModel(GenericityService genericityService)
        {
            _genericityService = genericityService;
            _rentals = new ObservableCollection<Rental>();
        }

        public ObservableCollection<Rental> Rentals
        {
            get => _rentals;
            set
            {
                if (_rentals != value)
                {
                    _rentals = value;
                    OnPropertyChanged();
                }
            }
        }

        public void PreviousPage()
        {
            if (HasPreviousPage)
            {
                CurrentPage--;
            }
        }

        public void NextPage()
        {
            if (HasNextPage)
            {
                CurrentPage++;
            }
        }

        public async Task<Boolean> CheckExist(int id)
        {
            try
            {
                var existingRental = await _genericityService._context.Rentals
                                             .FirstOrDefaultAsync(c => c.ID == id);
                return existingRental != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CheckExist Error: {ex.Message}");
                return false;
            }
        }

        public async Task Search(Dictionary<string, string> filters)
        {
            try
            {
                var result = await _genericityService.Search<Rental>(filters);
                Rentals = new ObservableCollection<Rental>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search Error: {ex.Message}");
            }
        }

        public async Task LoadRentalsAsync()
        {
            try
            {
                Console.WriteLine("LoadRentalsAsync: Start loading rentals.");

                var rentals = await _genericityService.GetObjects<Rental>(
                    e => e.Customer
                );
                Console.WriteLine("Loaded rentals and customers.");

                foreach (var rental in rentals)
                {
                    _genericityService._context.Entry(rental).Collection(r => r.RentalItems).Load();
                    foreach (var item in rental.RentalItems)
                    {
                        _genericityService._context.Entry(item).Reference(ri => ri.Equipment).Load();
                    }
                }
                Console.WriteLine("Loaded rental items and equipments.");

                Rentals = new ObservableCollection<Rental>(rentals);
                OnPropertyChanged(nameof(PagedRentals)); // Ensure the data binding is refreshed

                Console.WriteLine($"LoadRentalsAsync: Loaded {Rentals.Count} rentals.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LoadRentalsAsync Error: {ex.Message}");
                throw; // Rethrow the exception to ensure it can be caught further up the stack
            }
        }

        public async Task AddRentalAsync(Rental rental)
        {
            try
            {
                await _genericityService.AddItemAsync(rental);
                Rentals.Add(rental);
                OnPropertyChanged(nameof(PagedRentals)); // Ensure the data binding is refreshed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddRentalAsync Error: {ex.Message}");
            }
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            try
            {
                await _genericityService.UpdateItemAsync(rental);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UpdateRentalAsync Error: {ex.Message}");
            }
        }

        public async Task DeleteRentalAsync(Rental rental)
        {
            try
            {
                await _genericityService.DeleteItemAsync<Rental>(rental);
                Rentals.Remove(rental);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteRentalAsync Error: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
