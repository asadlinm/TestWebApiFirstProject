using AutoMapper;
using JwtAuthWebAPiProject.Abstractions;
using JwtAuthWebAPiProject.CustomAttributes;
using JwtAuthWebAPiProject.DTOs;
using JwtAuthWebAPiProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtAuthWebAPiProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
        {
            var users = await _employeeRepository.GetAllAsync();
            //create employee output model to view
            var employeeOutputModel = new EmployeeResponse();
            var employeeOutputModels = users.Select(u => _mapper.Map(u, employeeOutputModel)).ToList();
            _mapper.Map(users, employeeOutputModels);
            return Ok(employeeOutputModels);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var user = await _employeeRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(null);
            }
            var response = new EmployeeResponse();
            var result = _mapper.Map(user, response);

            return Ok(result);
        }

        [PermissonCheck("CreateEmployee")]
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployee(CreateEmployeeRequest createEmployeeInputModel)
        {
            if (createEmployeeInputModel == null)
            {
                return BadRequest();
            }
            var employee = new Employee();
            _mapper.Map(createEmployeeInputModel, employee);
            var newEmployee = await _employeeRepository.CreateAsync(employee);
            //create employee output model to view
            var response = new EmployeeResponse();
            _mapper.Map(newEmployee, response);
            return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.Id }, response);
        }

        [PermissonCheck("UpdateEmployee")]
        [HttpPut]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(CreateEmployeeRequest updateEmployeeInputModel)
        {
            if (updateEmployeeInputModel == null)
            {
                return NotFound($"Employee with given data not found");
            }
            var employee = new Employee();
            _mapper.Map(updateEmployeeInputModel, employee);
            await _employeeRepository.UpdateAsync(employee);
            var updatedEmployee = await _employeeRepository.GetByIdAsync(employee.Id);
            //create employee output model to view
            var response = new EmployeeResponse();
            _mapper.Map(updatedEmployee, response);
            return response;
        }

        [PermissonCheck("DeleteEmployee")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EmployeeResponse>> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee with {id} not found");
            }
            var deletedEmployee = await _employeeRepository.DeleteAsync(id);
            //create employee output model to view
            var response = new EmployeeResponse();
            _mapper.Map(deletedEmployee, response);
            return Ok(response);
        }
    }
}
