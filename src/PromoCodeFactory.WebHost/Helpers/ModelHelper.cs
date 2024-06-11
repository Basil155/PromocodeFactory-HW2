using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Helpers
{
    public static class ModelHelper
    {
        public static EmployeeResponse ConvertToModel(this Employee employee)
        {
            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        public static async Task<Employee> ConvertToDomain(this EmployeeRequest employeeRequest, IRepository<Role> roleRepository)
        {
            var employee = new Employee
            {
                Id = Guid.Empty,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Roles = [],
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount
            };

            foreach (var rId in employeeRequest.Roles)
            {
                employee.Roles.Add(await roleRepository.GetByIdAsync(rId));
            }

            return employee;
        }

        public static List<EmployeeShortResponse> ConvertToModelList(this IEnumerable<Employee> employees)
        {
            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        public static List<RoleResponse> ConvertToModelList(this IEnumerable<Role> roles)
        {
            var rolesModelList = roles.Select(x =>
                new RoleResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

            return rolesModelList;
        }
    }
}
