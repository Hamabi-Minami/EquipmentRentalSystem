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
using Microsoft.EntityFrameworkCore;

namespace EquipmentRentalSystem.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _customers;
        private readonly GenericityService _genericityService;

        // make pagenations
        public int PageSize = 10;
        public int _currentPage = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(pagedCustomers));
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(HasPreviousPage));
                    OnPropertyChanged(nameof(HasNextPage));
                }
            }
        }

        public List<Customer> pagedCustomers => _customers.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();
        public int TotalPages => (int)Math.Ceiling((double)_customers.Count / PageSize);
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

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
                    OnPropertyChanged(nameof(pagedCustomers));
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(HasPreviousPage));
                    OnPropertyChanged(nameof(HasNextPage));
                }
            }
        }


        public void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
            }
        }

        public void NextPage()
        {
            if (_currentPage < TotalPages)
            {
                _currentPage++;
            }
        }

        public async Task<Boolean> CheckExist(int id, string phone, string email)
        {
            var existingCustomer = await _genericityService._context.Customers
                                     .FirstOrDefaultAsync(c => c.ID == id || c.Phone == phone || c.Email == email);
            if (existingCustomer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task LoadCustomersAsync()
        {
            var customers = await _genericityService.GetObjects<Customer>();
            Customers = new ObservableCollection<Customer>(customers);
        }
        public async Task Search(Dictionary<string, string> filters)
        {
            var result = await _genericityService.Search<Customer>(filters);
            Customers = new ObservableCollection<Customer>(result);
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
