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
    //NOT HAVING ; AT LINE END PRODUCES STRANGE RESULTS - IF THERE IS A ; IN A WHILE STATEMENT IT BREAKS
    //string variables currently accept assignment like foobar = 1, creating a variable with value "1". This may need fixing.

    //Its possible to have more than one keyword or = in a statement. remove this.
    class Interpreter
    {
        public static LoadedVariables globalVars = new LoadedVariables(); //Global variables
        public Interpreter(string input)
        {
            int line = 0;
            try
            {
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
                            if (nodeSet.Count > 0) //Skip empty statements
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
                        }
                        else
                        {
                            nodeSet.Add(new Node(x)); //Add node to set
                        }
                    }

                    if(nodeSet.Count > 0)
                    {
                        throw new Exception("Syntax error - is there a missing ; or \" ?");
                    }

                    line = 1;
                    Node? resultSyn = fullTree.CalculateTreeResult(); //Calculate from tree


                }
            }
            catch(Exception ex)
            {
                if (line == 0)
                {
                    Console.WriteLine("An error occured while building the program - " + ex.Message);
                }
                else
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
        }

        private Tree CreateCommandTree(List<Node> nodeSet)
        {
            Queue<Node> output = new Queue<Node>();
            Queue<Node> output2 = new Queue<Node>(){ };
            Tree? branch = null;

            if (nodeSet[0].type == NodeContentType.Keyword) //Check for keyword
            {
                Node firstItem = nodeSet[0];
                nodeSet.RemoveAt(0);

                (List<Node> out1, int out1c, List<Node>? out2, int? out2c) containingExpression = Keywords.Keywords.SubsetStatement(firstItem.contents.ReturnShallowValue(), nodeSet); //Returns only the nodes within the parameters
                output = Parsing.Shunting.ShuntingYardAlgorithm(containingExpression.out1); //Convert to postfix
                output.Enqueue(firstItem); //Apply statement token
                nodeSet.RemoveRange(0, containingExpression.out1c);

                if(containingExpression.out2 != null)
                {
                    branch = CreateCommandTree(containingExpression.out2);
                }
            }
            else
            {
                output = Parsing.Shunting.ShuntingYardAlgorithm(nodeSet); //Convert to postfix
                nodeSet.Clear();

            }

            output.Enqueue(new Node(NodeContentType.End, ";")); //Apply end token

            if (nodeSet.Count > 0 && branch == null)
            {
                throw new Exception("Syntax error - functions must encapsulate all code in a statement"); //Maybe TODO? this checks for code outside an expression or statement.
            }

            Tree syntaxTree = SyntaxTree.SyntaxTreeGenerator.GenerateTree(output); //Produce tree

            if(branch != null)
            {
                ((Tree)(syntaxTree.nodes[1]._item)).nodes[0] = new VariantNode(branch); //Adds the branch to the item at index 0 with the keyword
            }

            return syntaxTree;
        }
    }
}
