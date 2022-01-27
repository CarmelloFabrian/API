using API.Context;
using API.Models;
using API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository
{
    public class OldEmployeeRepository : IEmployeeRepository
    {
        private readonly MyContext context;
        public OldEmployeeRepository(MyContext context)
        {
            this.context = context;
        }


        public int Delete(string NIK)
        {
            var entity = context.Employees.Find(NIK);
            context.Remove(entity);
            var result = context.SaveChanges();
            return result;
        }

        public IEnumerable<Employee> Get()
        {
            return context.Employees.ToList();
        }

        public Employee Get(string NIK)
        {
            var employeeByNIK = context.Employees.FirstOrDefault(e => e.NIK == NIK);
            return employeeByNIK;
        }

        public int Insert(Employee employee)
        {
            string year = DateTime.Now.ToString("yyyy");
            int countRow = context.Employees.Count();
            string maxNik = context.Employees.Max(e => e.NIK);
            int newNik = Convert.ToInt32(maxNik);
            newNik += 1;
            string stringNik = Convert.ToString(newNik).Remove(0, 6);
            var phone = (from g in context.Employees where g.Phone == employee.Phone select g).FirstOrDefault<Employee>();
            var email = (from g in context.Employees where g.Email == employee.Email select g).FirstOrDefault<Employee>();
            if (email == null)
            {
                if (phone == null)
                {
                    if (countRow == 0)
                    {
                        employee.NIK = $"{year}00{countRow + 1}";
                    }
                    else
                    {
                        employee.NIK = $"{year}00{stringNik}";
                    }
                    context.Employees.Add(employee);
                    var result = context.SaveChanges();
                    return result;
                }
                else
                {
                    return 2;//phone sudah terdaftar
                }
            }
            else if(email != null && phone != null)
            {
                return 4;//email dan phone sudah terdaftar
            }
            else
            {
                return 3;//email sudah terdaftar
            }
        }

        public int Update(Employee employee)
        {
            var phone = (from g in context.Employees where g.Phone == employee.Phone select g).FirstOrDefault<Employee>();
            var email = (from g in context.Employees where g.Email == employee.Email select g).FirstOrDefault<Employee>();
            if (phone == null && email == null)
            {
                context.Entry(employee).State = EntityState.Modified;
            }
            var result = context.SaveChanges();
            return result;
        }
    }
    


}
