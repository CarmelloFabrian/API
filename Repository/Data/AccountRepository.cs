using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext context;
        public AccountRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        public int Login(RegisterVM registerVM)
        {
            var phone = (from g in context.Employees where g.Phone == registerVM.Phone select g).FirstOrDefault<Employee>();
            var email = (from g in context.Employees where g.Email == registerVM.Email select g).FirstOrDefault<Employee>();
            if (phone != null || email != null)
            {
            var employees = context.Employees;
            var accounts = context.Accounts;
            var result = (from emp in employees
                          join acc in accounts on emp.NIK equals acc.NIK
                          where emp.Phone == registerVM.Phone || emp.Email == registerVM.Email
                          select acc).FirstOrDefault();
                if (BCrypt.Net.BCrypt.Verify(registerVM.Password, result.Password))
                {
                    return 1;//login 
                }
                else
                {
                    return 3;//password salah
                }
            }
            else
            {
                return 2;//akun tidak ditemukan
            }
        }
        public int ForgotPassword(RegisterVM registerVM)
        {
            var email = context.Employees.Where(e => e.Email == registerVM.Email).FirstOrDefault();
            if (email != null)
            {
                var acc = context.Accounts.Where(e => e.NIK == email.NIK).FirstOrDefault();
                Random rand = new Random();
                var otp = rand.Next(0, 999999).ToString("D6");
                acc.OTP = int.Parse(otp);
                var token = DateTime.Now.AddMinutes(5);
                acc.ExpiredToken = token;
                acc.IsUsed = false;
                context.Entry(acc).State = EntityState.Modified;
                context.SaveChanges();

                var fromAdress = new MailAddress("garvisnoreply256@gmail.com", "Admin");
                var toAdress = new MailAddress(registerVM.Email, "User");
                var subject = $"OTP {token}";
                var body = otp;
                const string fromPassword = "garvis256";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAdress.Address ,fromPassword)
                };

                using (var message = new MailMessage(fromAdress, toAdress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

                return 1;//otp dikirim
            }
            else
            {
                return 2;//akun tidak ditemukan
            }
        }
        public int ChangePassword(RegisterVM registerVM)
        {
            var email = context.Employees.Where(e => e.Email == registerVM.Email).FirstOrDefault();
            if (email != null)
            {
                var acc = context.Accounts.Where(a => a.NIK == email.NIK).FirstOrDefault();
                if (acc.OTP == registerVM.OTP)
                {
                    if (DateTime.Now < acc.ExpiredToken)
                    {
                        if(acc.IsUsed == false)
                        {
                            if (registerVM.NewPassword == registerVM.ConfirmPassword)
                            {
                                acc.Password = BCrypt.Net.BCrypt.HashPassword(registerVM.ConfirmPassword);
                                acc.IsUsed = true;
                                context.Entry(acc).State = EntityState.Modified;
                                context.SaveChanges();
                                return 1;
                            }
                            else
                            {
                                return 6;//password tidak cocok
                            }
                        }
                        else
                        {
                            return 5;//otp sudah digunakan
                        }
                    }
                    else
                    {
                        return 4;//otp expired
                    }
                }
                else
                {
                    return 3;//error otp salah
                }
            }
            else
            {
                return 2;//akun tidak ditemukan
            }
        }
    }
}
