using System.Collections;

string[] a = { "hi", "bye", "shy", "guy" };
int[] b = { 1, 2, 3, 4 };

string[] a1 = new string[5];
a1[0] = "hello";
a1[1] = "darkness";
a1[2] = "my";
a1[3] = "old";
a1[4] = "friend";

for (int i = 0; i < a.Length; i++)
{
    Console.Write(a[i] + " ");

    string searchItem = "guy";

    if (a[i] == searchItem)
    {
        Console.WriteLine($"match found for \"{searchItem}\" at index {i}", searchItem);
        break;
    }
}

for (int i = 0; i < b.Length; i++)
{
    Console.Write(b[i] + " ");
}

foreach(var item in a1)
{
    Console.WriteLine(item);
}

ArrayList list = new ArrayList();
list.Add("i've");
list.Add("come");
list.Add("to");
list.Add("visit");
list.Add("you");
list.Add("again");

foreach (var item in list)
{
    Console.WriteLine(item);
}

Console.WriteLine("list contains \"you\": " + list.Contains("you"));

list.Sort();
Console.WriteLine("list after sorting: ");
foreach (var item in list)
{
    Console.WriteLine(item);
}