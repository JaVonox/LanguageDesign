using System;
using System.Collections.Generic;
using System.Text;
using Nodes;
using TypeDef;

namespace NodeOperations
{
    public static class OperatorInteractions
    {
        public static Node? Interact(Node lNode, Node rNode, Node opNode)
        {
            Item producedItem;

            if (opNode.type == NodeContentType.Operation)
            {
                switch (opNode.contents.ReturnValue())
                {
                    case "+":
                        producedItem = lNode.contents + rNode.contents;
                        break;
                    case "-":
                        producedItem = lNode.contents - rNode.contents;
                        break;
                    case "*":
                        producedItem = lNode.contents * rNode.contents;
                        break;
                    case "/":
                        producedItem = lNode.contents / rNode.contents;
                        break;
                    case "<":
                        producedItem = Item.LessThan(lNode.contents, rNode.contents);
                        break;
                    case ">":
                        producedItem = Item.GreaterThan(lNode.contents, rNode.contents);
                        break;
                    case "==":
                        producedItem = Item.EqualTo(lNode.contents, rNode.contents);
                        break;
                    case "!=":
                        producedItem = Item.NotEqualTo(lNode.contents, rNode.contents);
                        break;
                    case "<=":
                        producedItem = Item.LessThanEqualTo(lNode.contents, rNode.contents);
                        break;
                    case ">=":
                        producedItem = Item.GreaterThanEqualTo(lNode.contents, rNode.contents);
                        break;
                    case "&&":
                        producedItem = Item.And(lNode.contents, rNode.contents);
                        break;
                    case "||":
                        producedItem = Item.Or(lNode.contents, rNode.contents);
                        break;
                    case "!":
                        producedItem = Item.Not(lNode.contents);
                        break;
                    case "=":
                        if (lNode.type == NodeContentType.Identifier) //If undefined variable - contents stores name
                        {
                            if (Interpreter.Interpreter.globalVars.Contains(lNode.contents.ReturnValue().ToString())) { throw new Exception("Variable redefinition"); }
                            else
                            {
                                lNode.contents = Interpreter.Interpreter.globalVars.AddNewItem(lNode.contents.ReturnValue(), rNode.contents); //Make new variable with value equal to that of rNode
                            }
                        }
                        else if(Interpreter.Interpreter.globalVars.Contains(lNode.contents))
                        {
                            Item.SetContent(lNode.contents, rNode.contents);
                        }
                        else
                        {
                            throw new Exception("Could not assign to non-variable item");
                        }
                        return lNode; //Return the lNode with updated data
                    default:
                        throw new Exception("Unimplemented Operation");
                }
            }
            else if (opNode.type == NodeContentType.Keyword)
            {
                switch (opNode.contents.ReturnValue())
                {
                    case "print":
                        {
                            Console.WriteLine(lNode.contents.ReturnValue()); //print out the value of the left node
                            return null;
                        }
                    default:
                        throw new Exception("Unrecognised keyword");
                }
            }
            else
            {
                throw new Exception("Invalid operation used on item");
            }

            return new Node(GetTyping(producedItem), producedItem);
        }

        public static NodeContentType GetTyping(Item input)
        {
            return Node.contentRef[input.GetType()];
        }
    }
}