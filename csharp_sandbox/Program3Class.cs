namespace CsharpSandbox
{
    internal class Program3Class
    {
        static void Main(string[] args)
        {
            //running as is will throw a "program has more than one entry point defined.
            //compile with /main to specify the type that contains the entry point" error
            //so need to edit project properties and set startup object to specify which Main is the entry point (but you should really only have ONE entry point in a csharp project)

            Console.WriteLine("i am in the 3rd program / class");

            Program1 p = new Program1();
            p.getData();
        }
    }
}
