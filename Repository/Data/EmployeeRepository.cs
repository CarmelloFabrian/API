using API.Context;
using API.Models;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext context;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            this.context = myContext;
        }
        public int Register(RegisterVM registerVM)
        {
            string year = DateTime.Now.ToString("yyyy");
            int countRow = context.Employees.Count();
            string maxNik = context.Employees.Max(e => e.NIK);
            int newNik = Convert.ToInt32(maxNik);
            newNik += 1;

            Employee emp = new Employee
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.BirthDate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Models.Gender)registerVM.Gender
            };
            var phone = (from g in context.Employees where g.Phone == registerVM.Phone select g).FirstOrDefault<Employee>();
            var email = (from g in context.Employees where g.Email == registerVM.Email select g).FirstOrDefault<Employee>();
            if (email == null)
            {
                if (phone == null)
                {
                    if (countRow == 0)
                    {
                        emp.NIK = $"{year}00{countRow + 1}";
                    }
                    else
                    {
                        string stringNik = Convert.ToString(newNik).Remove(0, 6);
                        emp.NIK = $"{year}00{stringNik}";
                    }
                    context.Employees.Add(emp);
                }
                else
                {
                    return 2;//phone sudah terdaftar
                }
            }
            else if (email != null && phone != null)
            {
                return 4;//email dan phone sudah terdaftar
            }
            else
            {
                return 3;//email sudah terdaftar
            }

            Account acc = new Account
            {
                NIK = emp.NIK,
                Password = registerVM.Password,
            };
            acc.Password = BCrypt.Net.BCrypt.HashPassword(acc.Password);
            context.Accounts.Add(acc);

            Education edu = new Education
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversityId = registerVM.UniversityId
            };
            context.Educations.Add(edu);
            context.SaveChanges();

            Profiling pro = new Profiling
            {
                NIK = acc.NIK,
                EducationId = edu.Id
            };
            context.Profilings.Add(pro);
            context.SaveChanges();

            AccountRole ar = new AccountRole();
            ar.AccountId = emp.NIK;
            ar.RoleId = 1;
            context.AccountRoles.Add(ar);
            context.SaveChanges();

            return 1;
        }
        public IEnumerable GetRegisteredData()
        {

            var employees = context.Employees;
            var accounts = context.Accounts;
            var profilings = context.Profilings;
            var educations = context.Educations;
            var universities = context.Universities;
            var accountrole = context.AccountRoles;
            var roles = context.Roles;


            var result = (from emp in employees
                          join acc in accounts on emp.NIK equals acc.NIK
                          join ar in accountrole on acc.NIK equals ar.AccountId
                          join r in roles on ar.RoleId equals r.Id
                          join pro in profilings on acc.NIK equals pro.NIK
                          join edu in educations on pro.EducationId equals edu.Id
                          join uni in universities on edu.UniversityId equals uni.Id

                          select new
                          {
                              FullName = emp.FirstName + " " + emp.LastName,
                              Phone = emp.Phone,
                              BirthDate = emp.BirthDate,
                              Salary = emp.Salary,
                              Email = emp.Email,
                              Degree = edu.Degree,
                              GPA = edu.GPA,
                              UnivName = uni.Name,
                              RoleName = r.Name
                          }).ToList();

            return result;
        }
    }
}
