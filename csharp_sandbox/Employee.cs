using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpSandbox
{
    public class Employee : Person
    {
        private int _employeeId;
        private string _jobTitle;
        private string _company;
        private int _salary;
        private Employee _manager;

        public Employee() { }

        public Employee(int employeeId, string jobTitle, string company, int salary, Employee manager, int id, int age, string firstName, string lastName, DateOnly birthday, string job) : base(id, age, firstName, lastName, birthday, job)
        {
            _employeeId = employeeId;
            _jobTitle = jobTitle;
            _company = company;
            _salary = salary;
            _manager = manager;
        }

        public Employee(int employeeId, string jobTitle, string company, int salary)
        {
            _employeeId = employeeId;
            _jobTitle = jobTitle;
            _company = company;
            _salary = salary;
        }

        public Employee(int employeeId, string jobTitle, string company, int salary, Employee manager)
        {
            _employeeId = employeeId;
            _jobTitle = jobTitle;
            _company = company;
            _salary = salary;
            _manager = manager;
        }

        public int EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string JobTitle
        {
            get { return _jobTitle; }
            set { _jobTitle = value; }
        }

        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public int Salary
        {
            get { return _salary; }
            set { _salary = value; }
        }

        public Employee Manager 
        { 
            get { return _manager; }
            set { _manager = value; }
        }

        public bool AskForARaise()
        {
            bool raiseSuccess = false;

            if(this.Manager != null)
            {
                Random rand = new Random();
                double pitchStrength = rand.NextDouble();
                Console.WriteLine(pitchStrength);

                if (pitchStrength >= .90)
                {
                    _salary = (int)(this.Salary * 1.15);
                    Console.WriteLine($"nice, you got the raise! your salary is now {this.Salary}");
                    return raiseSuccess = true;
                }
                else
                {
                    Console.WriteLine("sorry, no raise this time, maybe next quarter");
                    return raiseSuccess;
                }
            }
            else
            {
                Console.WriteLine($"{this.FirstName}, you ARE the boss, go n get you some customers if you want more profit");

                return raiseSuccess;
            }
        }

        public bool poachEmployee(Employee employee)
        {
            bool poachSuccess = false;

            if (this.Manager == null)
            {
                Random rand = new Random();
                double poachStrength = rand.NextDouble();
                
                if(poachStrength >= .9)
                {
                    employee.Company = this.Company;
                    employee.Salary = (int)(employee.Salary * 1.25);
                    employee.Manager = this;

                    Console.WriteLine(employee.Company);
                    Console.WriteLine(employee.Salary);
                    Console.WriteLine(employee.Manager.FirstName);

                    Console.WriteLine($"sign on bonus was successful! you gained {employee.FirstName} as an employee {this.FirstName}!");

                    Console.WriteLine($"{employee.FirstName} now works for you at ${employee.Salary} a year");
                    return poachSuccess = true;
                }
                else
                {
                    Console.WriteLine($"{employee.FirstName} says not interested, take it somewhere else!");
                    return poachSuccess;
                }
            }
            return poachSuccess;
        }
    }

}
