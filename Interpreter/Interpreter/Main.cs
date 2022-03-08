using System;
using Interpreter;

class MainFile
{
    public static Interpreter.Interpreter interpreter;
    static void Main(string[] args)
    {
        //REMEMBER TO MENTION AND AND OR ARE & / |
        //3*(24/2-3*4+2*6) RETURNS INVALID RESULT. IT SHOULD RETURN 36. This is most likely due to invalid queueing.
        string input = Console.ReadLine();
        interpreter = new Interpreter.Interpreter(input);
    }
}

