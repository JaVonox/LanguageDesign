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
                default:
                    throw new Exception("Unimplemented Operation");
            }

            return new Node(GetTyping(producedItem), producedItem);
        }

        public static Dictionary<Type, NodeContentType> contentRef = new Dictionary<Type, NodeContentType>() //Convert type to node content type
        {
            {typeof(TypeTemplate.Integer), NodeContentType.Integer},
            {typeof(TypeTemplate.Decimal), NodeContentType.Decimal },
            {typeof(TypeTemplate.String), NodeContentType.String },
            {typeof(TypeTemplate.Boolean), NodeContentType.Boolean },
            {typeof(TypeTemplate.Operation), NodeContentType.Operation },
            {typeof(TypeTemplate.End), NodeContentType.End },
            {typeof(TypeTemplate.Bracket), NodeContentType.Bracket },
        };

        public static NodeContentType GetTyping(Item input)
        {
            return contentRef[input.GetType()];
        }
    }
}