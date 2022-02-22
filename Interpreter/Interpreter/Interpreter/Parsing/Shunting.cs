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
            {"/",1},
            {"*",1 },
            {"+",2 },
            {"-",2 },
        };
        public static Queue<Token> ShuntingYardAlgorithm(List<Token> tokenSequence)
        {
            Queue<Token> tQueue = new Queue<Token>(); //Token queue
            Stack<Token> opStack = new Stack<Token>(); //stack of operands

            bool isBracket = false; //Is searching for brackets
            foreach(Token t in tokenSequence) //Iterate through existing sequence of tokens
            {
                if(t.type != TokenType.Operation && t.type != TokenType.Bracket)
                {
                    tQueue.Enqueue(t);
                }
                else
                {
                    if(opStack.Count == 0 || t.type == TokenType.Bracket) //If there is no items in the queue, add to the queue
                    {
                        if (t.type == TokenType.Bracket)
                        {
                            if(t.contents == "(")
                            {
                                isBracket = true; //Start searching for right bracket
                                opStack.Push(t);
                            }
                            else if(t.contents == ")")
                            {
                                if(isBracket) //If searching for right bracket
                                {
                                    isBracket = false;
                                    while (opStack.Count > 0)
                                    {
                                        Token tToken = opStack.Pop();
                                        if(tToken.type == TokenType.Bracket && tToken.contents == "(")
                                        {
                                            break;
                                        }
                                        tQueue.Enqueue(tToken);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Missing bracket in command");
                                }
                            }
                        }
                        else
                        {
                            opStack.Push(t);
                        }
                    }
                    else
                    {
                        if(opStack.Peek().type == TokenType.Bracket)
                        {
                            opStack.Push(t);
                        }
                        else if(opToPriority[opStack.Peek().contents] <= opToPriority[t.contents]) //If lower priority than the current top item
                        {
                            tQueue.Enqueue(opStack.Pop());
                            opStack.Push(t);
                        }
                        else
                        {
                            opStack.Push(t);
                        }
                    }
                }
            }

            while(opStack.Count > 0)
            {
                tQueue.Enqueue(opStack.Pop());
            }

            return tQueue;
        }
    }
}
