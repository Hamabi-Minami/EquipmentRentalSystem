using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquipmentRentalSystem.Data;
using EquipmentRentalSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace EquipmentRentalSystem.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var product = await _context.Categories.FindAsync(categoryId);
            if (product != null)
            {
                _context.Categories.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
