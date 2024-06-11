using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.WebHost.Services;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController(EmployeeService employeeService) : ControllerBase
    {
        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<EmployeeShortResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<EmployeeShortResponse>>> GetEmployeesAsync()
        {
            return await employeeService.GetAllAsync();
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Объект не найден</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employeeModel = await employeeService.GetByIdAsync(id);

            if (employeeModel == null)
                return NotFound();

            return employeeModel;
        }

        /// <summary>
        /// Изменить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Успешное выполнение</response>
        /// <response code="404">Объект не найден</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateEmployeeByIdAsync(Guid id, EmployeeRequest employeeRequest)
        {
            var success = await employeeService.UpdateByIdAsync(id, employeeRequest);

            if (success)
            {
                return NoContent();
            }

            return NotFound();
        }

        /// <summary>
        /// Добавить данные сотрудника
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Успешное выполнение</response>
        /// <response header="Location">Расположение объекта</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> CreateEmployeeAsync(EmployeeRequest employeeRequest)
        {
            var id = await employeeService.CreateAsync(employeeRequest);

            Response.Headers.Add("Location", $"api/v1/Employees/{id}");
            return NoContent();
        }

        /// <summary>
        /// Удалить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Успешное выполнение</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            var success = await employeeService.DeleteByIdAsync(id);

            return NoContent();
        }
    }
}