using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using Parsing;
using Nodes;
using SyntaxTree;
using TypeDef;

namespace Interpreter
{ 
    class Interpreter
    {
        private List<List<Token>> commands = new List<List<Token>>(); //Commands -> set of tokens
        public Interpreter(string input)
        {
            try
            {
                Item a = new Item(NodeContentType.Integer, "1");

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

                    foreach (List<Token> tSet in commands) //Parse 
                    {
                        List<Node> nodeSet = new List<Node>(); //Convert tokens to nodes
                        foreach(Token t in tSet) 
                        {
                            nodeSet.Add(new Node(t));
                        }

                        Queue<Node> output = Parsing.Shunting.ShuntingYardAlgorithm(nodeSet); //Convert to postfix
                        output.Enqueue(new Node(NodeContentType.End, ";")); //Apply end token
                        Tree syntaxTree = SyntaxTree.SyntaxTreeGenerator.GenerateTree(output);

                        Console.WriteLine(syntaxTree.PrintTreeContents()); //Print contents (SHOWS WITHOUT TYPE - MAYBE FIX?)

                        Node? resultSyn = syntaxTree.CalculateTreeResult(); //Evaluate conditions of tree

                        if (resultSyn != null)
                        {
                            Console.WriteLine(resultSyn.type + ":" + resultSyn.contents.ReturnValue());
                        }
                        else
                        {
                            Console.WriteLine("<NULL>");
                        }
                        Console.WriteLine("\n");
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
