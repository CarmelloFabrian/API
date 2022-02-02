using API.Base;
using API.Context;
using API.Models;
using API.Repository.Data;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accountRepository;
        //public AccountsController(AccountRepository accountRepository) : base(accountRepository)
        public IConfiguration _configuration;
        public MyContext context;
        public AccountsController(AccountRepository accountRepository, IConfiguration configuration, MyContext context) : base(accountRepository)
        {
            this.accountRepository = accountRepository;
            this._configuration = configuration;
            this.context = context;
        }
        [Route("login")]
        [HttpGet]

        //public ActionResult Login(RegisterVM registerVM)
        public ActionResult<RegisterVM> Login(RegisterVM registerVM)
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
                    //return Ok("login berhasil");
                    var getUserData = context.Employees.Where(e => e.Email == registerVM.Email || e.Phone == registerVM.Phone).FirstOrDefault();//email, role
                    var account = context.Accounts.Where(a => a.NIK == getUserData.NIK).FirstOrDefault();
                    var role = context.AccountRoles.Where(r => r.AccountId == account.NIK).FirstOrDefault();

                    var claims = new List<Claim>
                    {
                        new Claim("Email", getUserData.Email),//payload
                        new Claim("Roles", role.Role.Name)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//header
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                        );

                    var idtoken = new JwtSecurityTokenHandler().WriteToken(token);//gen token
                    claims.Add(new Claim("TokenSecurity", idtoken.ToString()));

                    return StatusCode(200, new { status = HttpStatusCode.OK, idtoken, message = "login berhasil" });
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
