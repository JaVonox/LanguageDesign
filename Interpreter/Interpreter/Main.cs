using System;
using Interpreter;
using System.IO;
using System.Reflection;
class MainFile
{
    public static Interpreter.Interpreter interpreter;
    static void Main(string[] args)
    {
        string dirPath = System.AppDomain.CurrentDomain.BaseDirectory + "/CodeFile.txt";
        string input = String.Join(" ",System.IO.File.ReadAllLines(dirPath)); //Read all lines

        interpreter = new Interpreter.Interpreter(input);
    }
}

