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
                switch (opNode.contents.ReturnDeepValue())
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
                        if(lNode.type == NodeContentType.Identifier && Interpreter.Interpreter.globalVars.VarContains(lNode.contents.ReturnShallowValue()))
                        {
                            Item.SetContent(lNode.contents, rNode.contents);
                        }
                        else
                        {
                            throw new Exception("Unknown variable '" + lNode.contents.ReturnShallowValue() + "'");
                        }
                        return lNode; //Return the lNode with updated data
                    default:
                        throw new Exception("Unimplemented Operation");
                }
            }
            else
            {
                throw new Exception("Syntax error");
            }

            return new Node(GetTyping(producedItem), producedItem);
        }

        public static NodeContentType GetTyping(Item input)
        {
            return Node.contentRef[input.GetType()];
        }
    }
}