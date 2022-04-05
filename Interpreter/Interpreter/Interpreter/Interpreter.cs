using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using Parsing;
using Nodes;
using SyntaxTree;
using TypeDef;
using Keywords;

namespace Interpreter
{ 
    //KNOWN ISSUES
    //FINAL LINE DOESNT REQUIRE ; 
    //NOT HAVING ; AT LINE END PRODUCES STRANGE RESULTS
    //NEED A WAY TO DO LIKE INT A = 2;
    //string variables currently accept assignment like foobar = 1, creating a variable with value "1". This may need fixing.
    //Input cannot be a statement because it returns a value

    //Whole program must become a single AST
    //therefore trees should be able to have more than 2 nodes
    //Do error messages between different types
    class Interpreter
    {
        private List<List<Token>> commands = new List<List<Token>>(); //Commands -> set of tokens
        public static LoadedVariables globalVars = new LoadedVariables(); //Global variables
        public Interpreter(string input)
        {
            int curLine = -1;
            try
            {
                List<Token> tokens = new List<Token>(); //Token set
                TokenHandler.CreateTokens(input, ref tokens); //Creates tokens for entire script

                if (tokens.Count > 0)
                {
                    //Sorting tokens into command sets
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

                    foreach (List<Token> tSet in commands) //Parse and compute
                    {
                        curLine = commands.IndexOf(tSet) + 1;
                        List<Node> nodeSet = new List<Node>(); //Convert tokens for this command into nodes
                        foreach(Token t in tSet)
                        {
                            nodeSet.Add(new Node(t));
                            //Console.WriteLine(t.type + ":" + t.contents);
                        }

                        Tree syntaxTree = CreateCommandTree(nodeSet); //Create tree using statements and expression data

                        Node? resultSyn = syntaxTree.CalculateTreeResult(); //Calculate from tree
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error occured on line " + curLine + " - " + ex.Message);
            }
        }

        private Tree CreateCommandTree(List<Node> nodeSet)
        {
            Queue<Node> output = new Queue<Node>();
            if (nodeSet[0].type == NodeContentType.Keyword) //Check for keyword
            {
                //TODO this currently only processes the entire set, change this maybe?
                Node firstItem = nodeSet[0];
                nodeSet.RemoveAt(0);

                (List<Node> n, int count) containingExpression = Keywords.Keywords.SubsetStatement(firstItem.contents.ReturnValue(), nodeSet); //Returns only the nodes within the parameters
                output = Parsing.Shunting.ShuntingYardAlgorithm(containingExpression.n); //Convert to postfix
                output.Enqueue(firstItem); //Apply statement token
                nodeSet.RemoveRange(0, containingExpression.count);
            }
            else
            {
                output = Parsing.Shunting.ShuntingYardAlgorithm(nodeSet); //Convert to postfix
                output.Enqueue(new Node(NodeContentType.End, ";")); //Apply end token
                nodeSet.Clear();
            }

            if(nodeSet.Count > 0)
            {
                throw new Exception("Code recognised outside of statement"); //Maybe TODO? this checks for code outside an expression or statement.
            }

            Tree syntaxTree = SyntaxTree.SyntaxTreeGenerator.GenerateTree(output); //Produce tree

            return syntaxTree;
        }
    }
}
