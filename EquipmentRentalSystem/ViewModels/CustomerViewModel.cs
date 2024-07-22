using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EquipmentRentalSystem.Models;
using EquipmentRentalSystem.Data;
using EquipmentRentalSystem.Services;
using System.Collections.ObjectModel;

namespace EquipmentRentalSystem.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers;
        private readonly GenericityService _genericityService;

        // make pagenations
        public int pageSize = 10;
        public int currentPage = 1;
        public List<Customer> pagedCustomers => _customers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        public int totalPages => (int)Math.Ceiling((double)_customers.Count / pageSize);

        public CustomerViewModel(GenericityService genericityService)
        {
            _genericityService = genericityService;
            _customers = new ObservableCollection<Customer>();
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                if (_customers != value)
                {
                    _customers = value;
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

        public async Task LoadCustomersAsync()
        {
            var customers = await _genericityService.GetObjects<Customer>();
            Customers = new ObservableCollection<Customer>(customers);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _genericityService.AddItemAsync(customer);
            Customers.Add(customer);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            await _genericityService.UpdateItemAsync(customer);
        }

        public async Task DeleteCustomerAsync(Customer customer)
        {
            await _genericityService.DeleteItemAsync<Customer>(customer);
            Customers.Remove(customer);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
