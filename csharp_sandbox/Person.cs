using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpSandbox
{
    public class Person
    {
        private int _id;
        private int _age;
        private string _firstName;
        private string _lastName;
        private DateOnly _birthday;
        private string _job;
           
        public Person() { }

        public Person(int id, int age, string firstName, string lastName, DateOnly birthday, string job) 
        {
            _id = id;
            _age = age;
            _firstName = firstName;
            _lastName = lastName;
            _birthday = birthday;
            _job = job;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set{ _lastName = value; }
        }

        public DateOnly Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public string Job
        {
            get { return _job; }
            set { _job = value; }
        }
    }
}
