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
        public readonly AppDbContext _context;

        public GenericityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetObjects<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();

                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetObjects<{typeof(T).Name}> Error: {ex.Message}");
                throw new Exception("nmsl??????????????????????"); // Rethrow the exception to ensure it can be caught further up the stack
            }
        }

        public async Task AddItemAsync<T>(T item) where T : class
        {
            _context.Set<T>().Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync<T>(T item) where T : class
        {
            _context.Set<T>().Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync<T>(T item) where T : class
        {
            _context.Set<T>().Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> Search<T>(Dictionary<string, string> filters) where T : class
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var filter in filters)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, filter.Key);
                var propertyType = property.Type;
                object convertedValue;

                try
                {
                    convertedValue = Convert.ChangeType(filter.Value, propertyType);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to convert '{filter.Value}' to type '{propertyType}'", ex);
                }

                var constant = Expression.Constant(convertedValue);
                var equals = Expression.Equal(property, constant);

                var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                query = query.Where(lambda);
            }

            return await query.ToListAsync();
        }
    }




}
