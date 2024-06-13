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
        /// <response code="204">Нет данных</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EmployeeResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employeeModel = await employeeService.GetByIdAsync(id);

            if (employeeModel == null)
                return NoContent();

            return employeeModel;
        }

        /// <summary>
        /// Изменить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Объект не найден</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateEmployeeByIdAsync(Guid id, EmployeeRequest employeeRequest)
        {
            var success = await employeeService.UpdateByIdAsync(id, employeeRequest);

            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// Добавить данные сотрудника
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Успешное выполнение</response>
        /// <response header="Location">Расположение объекта</response>
        [HttpPost]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateEmployeeAsync(EmployeeRequest employeeRequest)
        {
            var id = await employeeService.CreateAsync(employeeRequest);

            return Created($"api/v1/Employees/{id}", null);
        }

        /// <summary>
        /// Удалить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            var success = await employeeService.DeleteByIdAsync(id);

            return Ok();
        }
    }
}