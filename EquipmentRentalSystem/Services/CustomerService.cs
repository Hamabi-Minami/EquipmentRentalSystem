using EquipmentRentalSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentRentalSystem.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Customer>> GetCategoriesAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        //public async Task AddItemAsync(Customer customer)
        //{
        //    _context.Categories.Add(customer);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task UpdateItemAsync(Customer customer)
        //{
        //    _context.Categories.Update(category);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteItemAsync(int categoryId)
        //{
        //    var product = await _context.Categories.FindAsync(categoryId);
        //    if (product != null)
        //    {
        //        _context.Categories.Remove(product);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
