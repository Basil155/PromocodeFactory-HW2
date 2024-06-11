using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Helpers;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Services
{
    public class EmployeeService(IRepository<Role> roleRepository, IRepository<Employee> employeeRepository)
    {
        public async Task<List<EmployeeShortResponse>> GetAllAsync()
        {
            var employees = await employeeRepository.GetAllAsync();

            return employees.ConvertToModelList();
        }

        public async Task<EmployeeResponse> GetByIdAsync(Guid id)
        {
            var employee = await employeeRepository.GetByIdAsync(id);

            return employee?.ConvertToModel();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            return await employeeRepository.DeleteByIdAsync(id);
        }

        public async Task<bool> UpdateByIdAsync(Guid id, EmployeeRequest employeeRequest)
        {
            var employee = await employeeRequest.ConvertToDomain(roleRepository);
            employee.Id = id;

            return await employeeRepository.UpdateByIdAsync(id, employee);
        }

        public async Task<Guid> CreateAsync(EmployeeRequest employeeRequest)
        {
            var employee = await employeeRequest.ConvertToDomain(roleRepository);

            return await employeeRepository.CreateAsync(employee);
        }
    }
}
