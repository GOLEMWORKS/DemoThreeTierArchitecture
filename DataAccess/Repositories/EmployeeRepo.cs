using Business.DTOs;
using Business.Entities;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class EmployeeRepo : IEmployee
    {
        private readonly AppDbContext appDbContext;
        public EmployeeRepo(AppDbContext appDbContext) 
        {
            this.appDbContext = appDbContext;
        }

        private async Task SaveChangesAsync() => await appDbContext.SaveChangesAsync();

        public async Task<ServiceResponse> AddAsync(Employee employee)
        {
            var check = await appDbContext.Employees
                .FirstOrDefaultAsync( _ => _.Name.ToLower() == employee.Name.ToLower());
            if (check != null)
                return new ServiceResponse(false, "User already exists! | Пользователь уже существует!");

            appDbContext.Employees.Add(employee);
            await SaveChangesAsync();
            return new ServiceResponse(true, "Added | Добавлен");
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var employee = await appDbContext.Employees.FindAsync(id);
            if (employee == null)
                return new ServiceResponse(false, "User not found | Пользователь не найден");

            appDbContext.Employees.Remove(employee);
            await SaveChangesAsync();
            return new ServiceResponse(true, "Deleted | Удален");
        }

        public async Task<List<Employee>> GetAsync() => await appDbContext.Employees.AsNoTracking().ToListAsync();

        public async Task<Employee> GetByIdAsync(int id) => await appDbContext.Employees.FindAsync(id);

        public async Task<ServiceResponse> UpdateAsync(Employee employee)
        {
            appDbContext.Update(employee);
            await SaveChangesAsync();
            return new ServiceResponse(true, "Updated");
        }
    }
}
