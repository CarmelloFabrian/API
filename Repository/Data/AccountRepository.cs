using API.Context;
using API.Models;
using API.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var email = (from g in context.Employees where g.Email == registerVM.Email select g).FirstOrDefault<Employee>();
            if (email != null)
            {
                return 1;//otp dikirim
            }
            else
            {
                return 2;//akun tidak ditemukan
            }
        }
    }
}
