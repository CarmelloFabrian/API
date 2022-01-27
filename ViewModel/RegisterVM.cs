using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class RegisterVM
    {
        //emp
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        //acc
         public string Password { get; set; }
        //edu
        public string Degree { get; set; }
        public string GPA { get; set; }
        public int UniversityId { get; set; }
    }
    public enum Gender
    {
        Male,
        Female
    }
}
