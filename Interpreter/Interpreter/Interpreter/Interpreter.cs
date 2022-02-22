using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using Parsing;

namespace Interpreter
{ 
    class Interpreter
    {
        private List<List<Token>> commands = new List<List<Token>>(); //Commands -> set of tokens
        public Interpreter(string input)
        {
            try
            {
                List<Token> tokens = new List<Token>();
                TokenHandler.CreateTokens(input, ref tokens);

                if (tokens.Count > 0)
                {
                    int iterate = -1; //Index for sorting tokens into command sets
                    foreach (Token t in tokens)
                    {
                        if (t.type == TokenType.End || iterate == -1)
                        {
                            iterate++;
                            commands.Add(new List<Token>() { });

                            if(iterate == 0)
                            {
                                commands[iterate].Add(t);
                            }
                        }
                        else
                        {
                            commands[iterate].Add(t);
                        }
                    }

                    if (commands[iterate].Count == 0)
                    {
                        commands.RemoveAt(iterate);
                    }

                    //TODO should not parse each set. this is temporary.

                    foreach(List<Token> tSet in commands) //Parse 
                    {
                        Queue<Token> output = Parsing.Shunting.ShuntingYardAlgorithm(tSet);

                        foreach(Token outT in output)
                        {
                            Console.Write(outT.type + ":" + outT.contents + " ");
                        }
                        Console.WriteLine("");
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured during tokenization : " + ex);
            }
        }
    }
}
