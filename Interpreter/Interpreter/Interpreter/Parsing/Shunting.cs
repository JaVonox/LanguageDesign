using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using Nodes;

namespace Parsing
{ 

    static class Shunting
    {
        private static Dictionary<string, int> opToPriority = new Dictionary<string, int>()
        {
            {"(",-1},
            {"!",1 },
            {"/",2},
            {"*",2 },
            {"+",3 },
            {"-",3 },
            {"<",4 },
            {">",4 },
            {"<=",4 },
            {">=",4 },
            {"==",5 },
            {"!=",5 },
            {"&&",6 },
            {"||",7 },
        };
        public static Queue<Node> ShuntingYardAlgorithm(List<Node> tokenSequence)
        {
            Queue<Node> tQueue = new Queue<Node>(); //Node queue
            Stack<Node> opStack = new Stack<Node>(); //operationStack

            foreach(Node t in tokenSequence) //Iterate through existing sequence of tokens
            {
                if(t.type != NodeContentType.Operation && t.type != NodeContentType.Bracket)
                {
                    tQueue.Enqueue(t);
                }
                else
                {
                    if (t.type == NodeContentType.Operation && (opToPriority.ContainsKey(t.contents))) //Check for operations
                    {
                        if (opStack.Count > 0 && opToPriority[opStack.Peek().contents] <= opToPriority[t.contents]) //If lower priority than the current top item
                        {
                            while (opStack.Count > 0 && opStack.Peek().type != NodeContentType.Bracket && opToPriority[opStack.Peek().contents] <= opToPriority[t.contents])
                            {
                                tQueue.Enqueue(opStack.Pop());
                            }
                        }
                        opStack.Push(t);
                    }
                    else if(t.type == NodeContentType.Bracket) //Check for brackets
                    {
                        if (t.contents == "(")
                        {
                            opStack.Push(t);
                        }
                        else if (t.contents == ")")
                        {
                            while (opStack.Count > 0)
                            {
                                if (opStack.Peek().type == NodeContentType.Bracket && opStack.Peek().contents == "(") //Iterate through until finding a left bracket
                                {
                                    opStack.Pop();
                                    break;
                                }
                                else
                                {
                                    tQueue.Enqueue(opStack.Pop());
                                }
                            }
                        }
                    }

                }
            }

            while(opStack.Count > 0) //Reverse the op stack
            {
                tQueue.Enqueue(opStack.Pop());
            }

            return tQueue;
        }
    }
}
