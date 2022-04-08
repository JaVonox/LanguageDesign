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

    //Its possible to have more than one keyword or = in a statement. remove this.
    class Interpreter
    {
        private List<List<Token>> commands = new List<List<Token>>(); //Commands -> set of tokens
        public static LoadedVariables globalVars = new LoadedVariables(); //Global variables
        public Interpreter(string input)
        {
            //try
            //{
                List<Token> tokens = new List<Token>(); //Token set
                TokenHandler.CreateTokens(input, ref tokens); //Creates tokens for entire script

                if (tokens.Count > 0) //If there is a set of tokens, begin building the tree
                {
                    Tree fullTree = null;
                    List<Node> nodeSet = new List<Node>(); //Node representation of tokens

                    foreach (Token x in tokens) //Parse and compute
                    {
                        if (x.type == NodeContentType.End)
                        {
                            Tree syntaxTree = CreateCommandTree(nodeSet); //Create tree using statements and expression data
                            nodeSet.Clear(); //Clears the node set to allow for reallocation

                            if (fullTree == null) //If tree is not yet set
                            {
                                fullTree = syntaxTree;
                            }
                            else //If tree exists
                            {
                                fullTree.InsertAtNextCommand(syntaxTree); //Add command to tree
                            }
                        }
                        else
                        {
                            nodeSet.Add(new Node(x)); //Add node to set
                        }
                    }
                    Node? resultSyn = fullTree.CalculateTreeResult(); //Calculate from tree


                }
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("An error occured - " + ex);
            //}
        }

        private Tree CreateCommandTree(List<Node> nodeSet)
        {
            Queue<Node> output = new Queue<Node>();
            if (nodeSet[0].type == NodeContentType.Keyword) //Check for keyword
            {
                Node firstItem = nodeSet[0];
                nodeSet.RemoveAt(0);

                (List<Node> n, int count) containingExpression = Keywords.Keywords.SubsetStatement(firstItem.contents.ReturnShallowValue(), nodeSet); //Returns only the nodes within the parameters
                output = Parsing.Shunting.ShuntingYardAlgorithm(containingExpression.n); //Convert to postfix
                output.Enqueue(firstItem); //Apply statement token
                nodeSet.RemoveRange(0, containingExpression.count);
            }
            else
            {
                output = Parsing.Shunting.ShuntingYardAlgorithm(nodeSet); //Convert to postfix
                nodeSet.Clear();
            }

            output.Enqueue(new Node(NodeContentType.End, ";")); //Apply end token

            if (nodeSet.Count > 0)
            {
                throw new Exception("Code recognised outside of statement"); //Maybe TODO? this checks for code outside an expression or statement.
            }

            Tree syntaxTree = SyntaxTree.SyntaxTreeGenerator.GenerateTree(output); //Produce tree

            return syntaxTree;
        }
    }
}
