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
            var result = accountRepository.ForgotPassword(registerVM);
            if (result != 0)
            {
                if (result == 2)
                {
                    return BadRequest("akun tidak ditemukan");
                }
                else
                {
                    return Ok("otp telah dikirimkan");
                }
            }
            else
            {
                return BadRequest("error, otp gagal");
            }
        }
        [Route("changepassword")]
        [HttpGet]

        public ActionResult ChangePassword(RegisterVM registerVM)
        {
            var result = accountRepository.ChangePassword(registerVM);
            if (result != 0)
            {
                if (result == 2)
                {
                    return BadRequest("akun tidak ditemukan");
                }
                else if (result == 3)
                {
                    return BadRequest("otp salah");
                }
                else if (result == 4)
                {
                    return BadRequest("otp expired");
                }
                else if (result == 5)
                {
                    return BadRequest("otp sudah digunakan");
                }
                else if (result == 6)
                {
                    return BadRequest("password tidak cocok");
                }
                else
                {
                    return Ok("ganti password berhasil");
                }
            }
            else
            {
                return BadRequest("error, otp gagal");
            }
        }
    }
}
