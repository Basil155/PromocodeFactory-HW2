using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> rolRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = rolRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<EmployeeShortResponse>>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return await Task.FromResult(employeesModelList);
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return await Task.FromResult(employeeModel);
        }

        /// <summary>
        /// Изменить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateEmployeeByIdAsync(Guid id, EmployeeRequest employeeRequest)
        {
            var employee = new Employee
            {
                Id = id,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Roles = [],
                AppliedPromocodesCount = employeeRequest.AppliedPromocodesCount
            };

            foreach (var rId in employeeRequest.Roles)
            {
                employee.Roles.Add(await _roleRepository.GetByIdAsync(rId));
            }

            var success = await _employeeRepository.UpdateByIdAsync(id, employee);
 
            return await Task.FromResult(NoContent());

        }

        /// <summary>
        /// Добавить данные сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateEmployeeAsync(EmployeeRequest employeeRequest)
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
                employee.Roles.Add(await _roleRepository.GetByIdAsync(rId));
            }

            var id = await _employeeRepository.CreateAsync(employee);

            Response.Headers.Add("Location", $"api/v1/Employees/{id}");
            return await Task.FromResult(NoContent());

        }

        /// <summary>
        /// Удалить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeByIdAsync(Guid id)
        {

            var success = await _employeeRepository.DeleteByIdAsync(id);

            return await Task.FromResult(NoContent());

        }
    }
}