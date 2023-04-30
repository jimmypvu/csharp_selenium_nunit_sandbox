using CsharpSandbox;
//globally scoped Main block via top level statement. CANNOT MIX TLS with specify/main in properties
Console.WriteLine("i am in the 2nd program class");

Program1 p = new Program1();
p.getData();
p.setData();

Program1 p1instance2 = new Program1();
p1instance2.setData();

//at runtime if you didn't declare a Main block, csharp will automatically generate a main block for you

//it's different in java, java you can create many classes each with their own psvm block and run each class individually, but in csharp that's not the case, there can ONLY be ONE main block in a csharp program

//here this got scoped globally as main because we used top level statement, so csharp at compile time will run this block and ignore the other main block in the program, global main scope takes priority over other main blocks in your code (but again you should only have ONE main block in your program)

//you can change the properties of a class to tell csharp not to compile this at runtime