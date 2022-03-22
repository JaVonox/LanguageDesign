using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using Parsing;
using Calculating;
using Nodes;

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
                        if (t.type == NodeContentType.End || iterate == -1)
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

                    //TODO should not parse each set? this is temporary.

                    //TEMPORARY CONVERT ALL TO NODE SET
                    //REPLACE WITH ABSTRACT SYNTAX TREE

                    foreach (List<Token> tSet in commands) //Parse 
                    {
                        List<Node> nodeSet = new List<Node>();

                        foreach(Token t in tSet) //TODO FIX THIS TO USE SYNTAX TREE  
                        {
                            nodeSet.Add(new Node(t));
                        }

                        Queue<Node> output = Parsing.Shunting.ShuntingYardAlgorithm(nodeSet);

                        foreach(Node outNode in output)
                        {
                            Console.Write(outNode.type + ":" + outNode.contents + " ");
                        }
                        Console.WriteLine("");

                        Node resultToken = Calculating.Equations.ProcessQueue(ref output);
                        Console.WriteLine(resultToken.type + ":" + resultToken.contents); //Return string form of operation result
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured: " + ex);
            }
        }
    }
}
