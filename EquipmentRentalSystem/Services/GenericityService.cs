using EquipmentRentalSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentRentalSystem.Services
{
    public class GenericityService
    {
        private readonly AppDbContext _context;

        public GenericityService(AppDbContext context)
        {
            this._context = context;
        }

        //public async Task<List<T>> GetObjects<T>() where T : class
        //{
        //    return await _context.GetDbSet<T>().ToListAsync();
        //}

        public async Task<List<T>> GetObjects<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task AddItemAsync<T>(T item) where T : class
        {
            _context.GetDbSet<T>().Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync<T>(T item) where T : class
        {
            _context.GetDbSet<T>().Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync<T>(T item) where T : class
        {
            var id = typeof(T).GetProperty("Id").GetValue(item);

            var obj = await _context.GetDbSet<T>().FindAsync(id);
            if (item != null)
            {
                _context.GetDbSet<T>().Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
