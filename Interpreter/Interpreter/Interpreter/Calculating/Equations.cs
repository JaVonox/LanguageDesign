using System;
using System.Collections.Generic;
using System.Text;
using Parsing;
using Tokens;

namespace Calculating
{
    class Equations
    {
        public static Token ProcessQueue(ref Queue<Token> tokenQueue) //Processes the result of a postfix/prefix queue
        {
            try
            {
                Stack<Token> tokenStack = new Stack<Token>();
                while (tokenQueue.Count > 0)
                {
                    tokenStack.Push(tokenQueue.Dequeue()); //Get next item from queue
                    if (tokenStack.Peek().type == TokenType.Operation)
                    {
                        Token operation = tokenStack.Pop();
                        Token rightItem = tokenStack.Pop();
                        Token leftItem = tokenStack.Pop();

                        Token tmpToken = leftItem.tokenMethod(leftItem, rightItem, operation);
                        if (tmpToken == null) { throw new InvalidOperationException(); }
                        else
                        {
                            tokenStack.Push(leftItem.tokenMethod(leftItem, rightItem, operation));
                        }
                    }
                }

                return tokenStack.Pop();
            }
            catch(ArithmeticException)
            {
                throw new Exception("Invalid arithmetic operation");
            }
            catch(InvalidOperationException)
            {
                throw new Exception("Syntax error or operation exception");
            }
            catch(Exception ex)
            {
                throw new Exception("Misc exception: " + ex);
            }
        }
    }
}
