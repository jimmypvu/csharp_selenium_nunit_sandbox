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

            Console.WriteLine("john decides to get a raise: ");
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
                Console.WriteLine(michael.FirstName + $" decides to ask {michael.Manager.FirstName} for a raise at {michael.Company}");
                do
                {
                    raiseSuccess = michael.AskForARaise();
                    askCount++;

                    if (!raiseSuccess)
                    {
                        Console.WriteLine("michael waits a few weeks and asks again");
                    }
                    else
                    {
                        Console.WriteLine($"{michael.Manager.FirstName}: congrats on the good job {michael.FirstName}");
                        Console.WriteLine("number of asks it took: " + askCount);
                    }
                } while (!raiseSuccess && askCount < 7);
            }
            else
            {
                Console.WriteLine($"{misterJohn.FirstName} nice, I got me a new sales team!");

                Console.WriteLine($"{michael.FirstName}: cole world, c, c, cole world");
            };


            if(askCount <= 3)
            {
                askCount = 0;
                bool raiseSuccess = false;
                Console.WriteLine($"after a few solid quarters michael decides to ask {michael.Manager.FirstName} for a raise at {michael.Company}");
                do
                {
                    raiseSuccess = michael.AskForARaise();
                    askCount++;

                    if (!raiseSuccess) 
                    {
                        Console.WriteLine("michael waits a few weeks and asks again");
                    }
                    else
                    {
                        Console.WriteLine($"{michael.Manager.FirstName}: congrats on the good job {michael.FirstName}");
                        Console.WriteLine("number of asks it took: " + askCount);
                    }
                } while(!raiseSuccess && askCount < 5);
            }
            else
            {
                Console.WriteLine($"{michael.FirstName} says to himself forget it i'm just gonna become a bank robber");
            }
        }
    }
}
