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

namespace EquipmentRentalSystem.ViewModels
{
    public class RentalViewModel
    {
        private ObservableCollection<Rental> _rentals;
        private readonly GenericityService _genericityService;

        // make pagenations
        public int pageSize = 10;
        public int currentPage = 1;
        public List<Rental> pagedRentals => _rentals.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        public int totalPages => (int)Math.Ceiling((double)_rentals.Count / pageSize);

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

        public async Task LoadRentalsAsync()
        {
            var rentals = await _genericityService.GetObjects<Rental>();
            Rentals = new ObservableCollection<Rental>(rentals);
        }

        public async Task AddRentalAsync(Rental rental)
        {
            await _genericityService.AddItemAsync(rental);
            Rentals.Add(rental);
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            await _genericityService.UpdateItemAsync(rental);
        }

        public async Task DeleteRentalAsync(Rental rental)
        {
            await _genericityService.DeleteItemAsync<Rental>(rental);
            Rentals.Remove(rental);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
