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
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        [Route("login")]
        [HttpGet]

        public ActionResult Login(RegisterVM registerVM)
        {
            var result = accountRepository.Login(registerVM);
            if (result != 0)
            {
                if (result == 2)
                {
                    return BadRequest("akun tidak ditemukan");
                }
                else if (result == 3)
                {
                    return BadRequest("password salah");
                }
                else
                {
                    return Ok("login berhasil");
                }
            }
            else
            {
                return BadRequest("error, login gagal");
            }
        }
        [Route("forgotpassword")]
        [HttpGet]

        public ActionResult ForgotPassword(RegisterVM registerVM)
        {
            var result = accountRepository.Login(registerVM);
            if (result != 0)
            {
                if (result == 2)
                {
                    return BadRequest("akun tidak ditemukan");
                }
                else if (result == 3)
                {
                    return BadRequest("password salah");
                }
                else
                {
                    return Ok("login berhasil");
                }
            }
            else
            {
                return BadRequest("error, login gagal");
            }
        }
    }
}
