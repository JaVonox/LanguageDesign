using System;
using Interpreter;

class MainFile
{
    public static Interpreter.Interpreter interpreter;
    static void Main(string[] args)
    {
        string input = Console.ReadLine();
        interpreter = new Interpreter.Interpreter(input);
    }
}

