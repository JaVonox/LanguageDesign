using System;
using System.Collections.Generic;
using System.Text;
using Tokens;

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
        public static Queue<Token> ShuntingYardAlgorithm(List<Token> tokenSequence)
        {
            Queue<Token> tQueue = new Queue<Token>(); //Token queue
            Stack<Token> opStack = new Stack<Token>(); //stack of operands

            foreach(Token t in tokenSequence) //Iterate through existing sequence of tokens
            {
                if(t.type != TokenType.Operation && t.type != TokenType.Bracket)
                {
                    tQueue.Enqueue(t);
                }
                else
                {
                    if (t.type == TokenType.Operation && (opToPriority.ContainsKey(t.contents))) //Check for operations
                    {
                        if (opStack.Count > 0 && opToPriority[opStack.Peek().contents] <= opToPriority[t.contents]) //If lower priority than the current top item
                        {
                            while (opStack.Count > 0 && opStack.Peek().type != TokenType.Bracket && opToPriority[opStack.Peek().contents] <= opToPriority[t.contents])
                            {
                                tQueue.Enqueue(opStack.Pop());
                            }
                        }
                        opStack.Push(t);
                    }
                    else if(t.type == TokenType.Bracket) //Check for brackets
                    {
                        if (t.contents == "(")
                        {
                            opStack.Push(t);
                        }
                        else if (t.contents == ")")
                        {
                            while (opStack.Count > 0)
                            {
                                if (opStack.Peek().type == TokenType.Bracket && opStack.Peek().contents == "(") //Iterate through until finding a left bracket
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
