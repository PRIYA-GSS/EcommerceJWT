using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using Interfaces.IRepository;
using Models.Constants;
using Models.DTOs;
namespace DataAccess.Repository
{
    public class BaseRepository<T>:IBaseRepository<T> where T:class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IList<T>> GetAllAsync()
        {
            return  _context.Set<T>().ToList();
        }
        public async Task<T> GetByIdAsync(int id)
        {

            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            throw new Exception(ErrorConstants.InValid);
            return entity;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {

            _context.Set<T>().Update(entity);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) throw new Exception(ErrorConstants.InValid);

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
