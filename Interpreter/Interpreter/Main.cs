using System;
using Interpreter;
using System.IO;
using System.Reflection;
class MainFile
{
    public static Interpreter.Interpreter interpreter;
    static void Main(string[] args)
    {
        Console.WriteLine(":c interpreter initialised. Running script from CodeFile.txt");
        Console.WriteLine("Loading script CodeFile.txt from directory: " + System.AppDomain.CurrentDomain.BaseDirectory + "CodeFile.txt");

        try
        {
            string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "/CodeFile.txt";
            string input = String.Join(" ", System.IO.File.ReadAllLines(dirPath)); //Read all lines

            interpreter = new Interpreter.Interpreter(input);
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occured while reading the code file. are you sure there is a file named CodeFile.txt in directory " + System.AppDomain.CurrentDomain.BaseDirectory + "?");
        }
    }
}

