using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
namespace Interpreter
{ 
    class Interpreter
    {
        private List<Token> tokens = new List<Token>();
        public Interpreter(string input)
        {
            TokenHandler.CreateTokens(input, ref tokens);

            foreach(Token t in tokens)
            {
                Console.WriteLine(t.type.ToString() + ":" + t.contents);
            }
        }
    }
}
