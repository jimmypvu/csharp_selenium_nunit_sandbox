using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpSandbox
{
    internal class OfficeProgram
    {

        static void Main(string[] args)
        {
            Person john = new Person(142124, 25, "john", "doe", new DateOnly(1980, 6, 15), "ceo");

            Employee misterJohn = new Employee(9918, "ceo", "company corp.", 50000);
            misterJohn.Id = john.Id;
            misterJohn.Age = john.Age;
            misterJohn.FirstName = john.FirstName;
            misterJohn.LastName = john.LastName;
            misterJohn.Birthday = john.Birthday;
            misterJohn.Job = john.Job;

            misterJohn.AskForARaise();
            misterJohn.AskForARaise();
            Console.WriteLine("dammit ok! lemme put some recruiting ads up");

            Employee misterWhite = new Employee(14125, "owner", "megacorp", 100000, null, 1231241, 30, "barry", "white", new DateOnly(1992, 10, 15), "owner operator");

            Employee michael = new Employee(14125, "sales manager", "megacorp", 100000, misterWhite, 1231241, 30, "michael", "doe", new DateOnly(1992, 10, 15), "sales");
            Console.WriteLine(michael.Age);
            Console.WriteLine(michael.FirstName);
            Console.WriteLine(michael.Company);
            Console.WriteLine(michael.Job);
            Console.WriteLine(michael.Birthday);
            Console.WriteLine(michael.Salary);
            Console.WriteLine(michael.Manager.FirstName);
            Console.WriteLine(michael.Manager.JobTitle);

            int askCount = 0;

            if (!misterJohn.poachEmployee(michael))
            {
                bool raiseSuccess = false;

                do
                {
                    raiseSuccess = michael.AskForARaise();
                    askCount++;

                } while (!raiseSuccess);

                Console.WriteLine(askCount);
            }
            else
            {
                Console.WriteLine("nice, got me a new sales team!");
            };


            if(askCount <= 3)
            {
                askCount = 0;
                bool raiseSuccess = false;
                do
                {
                    raiseSuccess = michael.AskForARaise();
                    askCount++;
                }while(!raiseSuccess);
            }

            Console.WriteLine(askCount);

        }
    }
}
