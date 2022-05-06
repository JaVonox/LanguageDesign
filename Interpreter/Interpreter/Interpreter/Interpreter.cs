using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using Parsing;
using Nodes;
using SyntaxTree;
using TypeDef;
using Keywords;
using System.Linq;

namespace Interpreter
{ 
    //KNOWN ISSUES
    //INPUTS AFTER {} BEFORE ; ARE ACCEPTED BUT NOT RAN

    //Its possible to have more than one keyword or = in a statement. remove this. STILL PROBLEM
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
                    List<VariantNode> nodeSet = new List<VariantNode>(); //Node representation of tokens

                    List<VariantNode> iterSet = IdentifyScope(tokens); //turns the tokens into nodes and squashes scopes

                    foreach (VariantNode x in iterSet) //Parse and compute
                    {
                        if (x._item.GetType() == typeof(Node) && ((Node)x._item).type == NodeContentType.End)
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
                            nodeSet.Add(x); //Add node to set
                        }
                    }

                    if(nodeSet.Count > 0)
                    {
                        throw new Exception("Syntax error - is there a missing ; or \" ?");
                    }

                    line = 1;

                    if(fullTree == null)
                    {
                        throw new Exception("No code input detected");
                    }
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
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        //TODO add checking for non matching { and }
        //Add handling for no contents in {}
        private List<VariantNode> IdentifyScope(List<Token> tSet) //Turns tokens into node list and squashes scope contents into a tree
        {
            List<int> scopeStartPos = new List<int>();
            List<int> scopeEndPos = new List<int>();
            List<VariantNode> nodes = new List<VariantNode>();

            for(int i = 0; i < tSet.Count;i++)
            {
                Token x = tSet[i];

                if(x.type == NodeContentType.Bracket)
                {
                    if(x.contents.ToString() == "{")
                    {
                        scopeStartPos.Add(i);
                    }
                    else if(x.contents.ToString() == "}")
                    {
                        scopeEndPos.Add(i);
                    }
                }
            }

            foreach(Token x in tSet)
            {
                nodes.Add(new VariantNode(new Node(x)));
            }

            scopeStartPos.Reverse(); //Reverses the scope array so it starts at the final { - iterating backwards through this should allow each to find its paired }

            if(scopeStartPos.Count() > scopeEndPos.Count()) { throw new Exception("One or more missing }"); }
            else if(scopeStartPos.Count() < scopeEndPos.Count()) { throw new Exception("One or more missing {"); }

            foreach(int posMarker in scopeStartPos)
            {
                int nextPos = scopeEndPos.First(x => x > posMarker); //get the next element where there is a scope end
                scopeEndPos.Remove(nextPos); //remove from endPos

                List<VariantNode> contentNodes = nodes.GetRange(posMarker + 1, (nextPos - posMarker)-1); //Get all nodes between the two points
                Tree scopeTree = null;

                List<VariantNode> storedNodes = new List<VariantNode>(); //Nodes to be stored

                foreach (VariantNode c in contentNodes) //Turn each node into an individual part of the tree
                {
                    if (c._item.GetType() == typeof(Node) && ((Node)(c._item)).type == NodeContentType.End) //TODO this will probably break for nested scopes since itll try and convert a variant tree into just a node
                    {
                        if (storedNodes.Count > 0) //Skip empty statements
                        {
                            Tree syntaxTree = CreateCommandTree(storedNodes); //Create tree using statements and expression data
                            storedNodes.Clear(); //Clears the node set to allow for reallocation

                            if (scopeTree == null) //If tree is not yet set
                            {
                                scopeTree = syntaxTree;
                            }
                            else //If tree exists
                            {
                                scopeTree.InsertAtNextCommand(syntaxTree); //Add command to tree
                            }
                        }
                    }
                    else
                    {
                        storedNodes.Add(c); //Add node to set
                    }
                }

                if(scopeTree == null)
                {
                    throw new Exception("Empty scope parenthesis - { } must contain one or more statements");
                }

                int preCount = nodes.Count;
                nodes.RemoveRange(posMarker + 1, (nextPos - posMarker) - 1);
                nodes.Insert(posMarker + 1, new VariantNode(scopeTree));
                int change = nodes.Count - preCount;

                for(int i = 0; i< scopeEndPos.Count;i++)
                {
                    if (scopeEndPos[i] > nextPos)
                    {
                        scopeEndPos[i] += change; //Adjust for changes
                    }
                }
            }

            return nodes;
        }
        private Tree CreateCommandTree(List<VariantNode> nodeSet)
        {
            try
            {
                Queue<Node> output = new Queue<Node>();
                Queue<Node> output2 = new Queue<Node>() { };
                Tree? branch = null;

                if (nodeSet[0]._item.GetType() == typeof(Node) && ((Node)(nodeSet[0]._item)).type == NodeContentType.Keyword) //Check for keyword
                {
                    Node firstItem = ((Node)(nodeSet[0]._item));
                    nodeSet.RemoveAt(0);

                    (List<VariantNode> out1, int out1c, Tree out2) containingExpression = Keywords.Keywords.SubsetStatement(firstItem.contents.ReturnShallowValue(), nodeSet); //Returns only the nodes within the parameters
                    output = Parsing.Shunting.ShuntingYardAlgorithm(containingExpression.out1.Select(y => ((Node)y._item)).ToList()); //Convert to postfix
                    output.Enqueue(firstItem); //Apply statement token
                    nodeSet.RemoveRange(0, containingExpression.out1c);

                    if (containingExpression.out2 != null)
                    {
                        branch = containingExpression.out2;
                    }
                }
                else if (nodeSet[0]._item.GetType() == typeof(Node))
                {
                    if(nodeSet.Any(x=>x._item.GetType() != typeof(Node))) { 
                        throw new Exception("Syntax error while parsing scope. Are there any scope ({ or }) assignments outside of a function?");
                    }
                    output = Parsing.Shunting.ShuntingYardAlgorithm(nodeSet.Select(y => ((Node)y._item)).ToList()); //Convert to postfix
                    nodeSet.Clear();
                }
                else
                {
                    throw new Exception("Error occured while trying to create a command tree");
                }

                output.Enqueue(new Node(NodeContentType.End, ";")); //Apply end token

                if (nodeSet.Count > 0 && branch == null)
                {
                    throw new Exception("Syntax error - functions must encapsulate all code in a statement"); //Maybe TODO? this checks for code outside an expression or statement.
                }

                Tree syntaxTree = SyntaxTree.SyntaxTreeGenerator.GenerateTree(output); //Produce tree

                if (branch != null)
                {
                    ((Tree)(syntaxTree.nodes[1]._item)).nodes[0] = new VariantNode(branch); //Adds the branch to the item at index 0 with the keyword
                }

                return syntaxTree;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
