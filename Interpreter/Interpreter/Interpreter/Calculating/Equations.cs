using System;
using System.Collections.Generic;
using System.Text;
using Parsing;
using Tokens;
using Nodes;
using NodeOperations;
namespace Calculating
{
    class Equations
    {
        public static Node ProcessQueue(Queue<Node> nodeQueue) //Processes the result of a postfix/prefix queue
        {
            try
            {
                Stack<Node> nodeStack = new Stack<Node>();
                while (nodeQueue.Count > 0)
                {
                    nodeStack.Push(nodeQueue.Dequeue()); //Get next item from queue
                    if (nodeStack.Peek().type == NodeContentType.Operation)
                    {
                        if (nodeStack.Peek().contents != "!") //For most operators
                        {
                            Node operation = nodeStack.Pop();
                            Node rightItem = nodeStack.Pop();
                            Node leftItem = nodeStack.Pop();

                            Node tmpNode = leftItem.itemMethod(leftItem, rightItem, operation);
                            if (tmpNode == null) { throw new InvalidOperationException(); }
                            else
                            {
                                nodeStack.Push(tmpNode);
                            }
                        }
                        else //For the not operator
                        {
                            Node operation = nodeStack.Pop();
                            Node rightItem = nodeStack.Pop();

                            Node tmpToken = OperatorDefinitions.NotOperator(rightItem, operation);
                            if (tmpToken == null) { throw new InvalidOperationException(); }
                            else
                            {
                                nodeStack.Push(tmpToken);
                            }
                        }
                    }
                }

                return nodeStack.Pop();
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
