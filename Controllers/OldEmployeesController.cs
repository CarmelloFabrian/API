using API.Context;
using API.Models;
using API.Repository;
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
    public class OldEmployeesController : ControllerBase
    {
        private readonly OldEmployeeRepository employeeRepository;
        public OldEmployeesController(OldEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var result = employeeRepository.Get();
            if(result.Count()>0)
            {
                return Ok(employeeRepository.Get());
            }
            else
            {
                return NotFound("Data tidak ditemukan");
            }
        }
        [HttpGet("{NIK}")]
        public IActionResult GetByNIK(string NIK)
        {
            var result = employeeRepository.Get(NIK);
            if (result is null)
            {
                return NotFound("Data tidak ditemukan");
            }
            else
            {
                return Ok(employeeRepository.Get(NIK));
            }
        }
        [HttpPost]
        public ActionResult Post(Employee employee)
        {
            var insert = employeeRepository.Insert(employee);
            if (insert != 0)
            {
                if (insert == 2)
                {
                    return BadRequest("error, phone sudah terdaftar");
                }
                else if (insert == 3)
                {
                    return BadRequest("error, email sudah terdaftar");
                }
                else if (insert == 4)
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
        [HttpDelete]
        public ActionResult Delete(string NIK)
        {
            var result = employeeRepository.Get(NIK);
            if (result is null)
            {
                return BadRequest("data tidak ditemukan");
            }
            else
            {
                employeeRepository.Delete(NIK);
                return Ok("data berhasil dihapus");
            }
        }
        [HttpPut]
        public ActionResult Put(Employee employee)
        {
            var update = employeeRepository.Update(employee);
            if (update > 0)
            {
                return Ok("data berhasil diubah");
            }
            var get = employeeRepository.Get(employee.NIK);
            if (get == null)
            {
                return NotFound("Data tidak ditemukan");
            }
            else
            {
                return BadRequest("error, data tidak tepat");
            }
        }
    }
}
