using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EquipmentRentalSystem.Models;
using EquipmentRentalSystem.Services;

namespace EquipmentRentalSystem.ViewModels
{
    public class EquipmentViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Equipment> _equipments;
        private readonly GenericityService _genericityService;

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
                    OnPropertyChanged(nameof(PagedEquipments));
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(HasPreviousPage));
                    OnPropertyChanged(nameof(HasNextPage));
                }
            }
        }

        public List<Equipment> PagedEquipments => _equipments.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        public int TotalPages => (int)Math.Ceiling((double)_equipments.Count / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public EquipmentViewModel(GenericityService genericityService)
        {
            _genericityService = genericityService;
            _equipments = new ObservableCollection<Equipment>();
        }

        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set
            {
                if (_equipments != value)
                {
                    _equipments = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PagedEquipments));
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(HasPreviousPage));
                    OnPropertyChanged(nameof(HasNextPage));
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

        public async Task LoadEquipmentsAsync()
        {
            var equipments = await _genericityService.GetObjects<Equipment>(e => e.Category);
            Equipments = new ObservableCollection<Equipment>(equipments);
        }

        public async Task AddEquipmentAsync(Equipment equipment)
        {
            await _genericityService.AddItemAsync(equipment);
            Equipments.Add(equipment);
            OnPropertyChanged(nameof(PagedEquipments));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(HasPreviousPage));
            OnPropertyChanged(nameof(HasNextPage));
        }

        public async Task UpdateEquipmentAsync(Equipment equipment)
        {
            await _genericityService.UpdateItemAsync(equipment);
            OnPropertyChanged(nameof(PagedEquipments));
        }

        public async Task DeleteEquipmentAsync(Equipment equipment)
        {
            await _genericityService.DeleteItemAsync(equipment);
            Equipments.Remove(equipment);
            OnPropertyChanged(nameof(PagedEquipments));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(HasPreviousPage));
            OnPropertyChanged(nameof(HasNextPage));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
