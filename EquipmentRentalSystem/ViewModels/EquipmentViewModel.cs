using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EquipmentRentalSystem.Models;
using EquipmentRentalSystem.Services;
using EquipmentRentalSystem.Data;
using System.Collections.ObjectModel;

namespace EquipmentRentalSystem.ViewModels
{
    public class EquipmentViewModel
    {
        private ObservableCollection<Equipment> _equipments;
        private readonly GenericityService _genericityService;

        // make pagenations
        public int pageSize = 10;
        public int currentPage = 1;
        public List<Equipment> pagedEquipments => _equipments.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        public int totalPages => (int)Math.Ceiling((double)_equipments.Count / pageSize);

        public EquipmentViewModel(GenericityService genericityService, AppDbContext context)
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

        public async Task LoadEquipmentsAsync()
        {
            //var equipments = await _genericityService.GetObjects<Equipment>();
            //Equipments = new ObservableCollection<Equipment>(equipments);

            var equipments = await _genericityService.GetObjects<Equipment>(e => e.Category);
            Equipments = new ObservableCollection<Equipment>(equipments);
        }

        public async Task AddEquipmentAsync(Equipment equipment)
        {
            await _genericityService.AddItemAsync(equipment);
            Equipments.Add(equipment);
        }

        public async Task UpdateEquipmentAsync(Equipment equipment)
        {
            await _genericityService.UpdateItemAsync(equipment);
        }

        public async Task DeleteEquipmentAsync(Equipment equipment)
        {
            await _genericityService.DeleteItemAsync<Equipment>(equipment);
            Equipments.Remove(equipment);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
