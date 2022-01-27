using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        private readonly EmployeeRepository employeeRepository;
        public EmployeesController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpPost("{RegisterVM}")]
        public ActionResult Register(RegisterVM registerVM)
        {
            var result = employeeRepository.Register(registerVM);
            if (result != 0)
            {
                if (result == 2)
                {
                    return BadRequest("error, phone sudah terdaftar");
                }
                else if (result == 3)
                {
                    return BadRequest("error, email sudah terdaftar");
                }
                else if (result == 4)
                {
                    return BadRequest("error, phone dan email sudah terdaftar");
                }
                else
                {
                    return Ok("data berhasil ditambahkan");
                }
            }
            else
            {
                return BadRequest("error, data tidak ditambahkan");
            }
        }
        [Route("registerdata")]
        [HttpGet]
        public ActionResult<RegisterVM> GetRegisteredData()
        {
            var result = employeeRepository.GetRegisteredData();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("data tidak ditemukan");
            }
        }
    }
}
