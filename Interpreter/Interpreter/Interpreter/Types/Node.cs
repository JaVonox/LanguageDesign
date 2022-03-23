using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using NodeOperations;

namespace Nodes
{
    public enum NodeContentType
    {
        End,
        Identifier,
        String,
        Integer,
        Operation,
        Decimal,
        Bracket,
        Boolean,
    }

    public class VariantNode
    {
        public object _item; //As either tree or node

        public VariantNode(Node inherit)
        {
            _item = inherit;
        }

        public VariantNode(Tree inherit)
        {
            _item = inherit;
        }
        public Tree ToTree() //Return tree form of item
        {
            return (Tree)_item;
        }
    }
    public class Node
    {
        public NodeContentType type;
        public string contents;
        public Func<Node, Node, Node, Node?> itemMethod;
        public Node(Token parentToken)
        {
            contents = parentToken.contents;
            type = (NodeContentType)parentToken.type;
            AppendOperations();
        }
        public Node(NodeContentType typ, string cont)
        {
            type = typ;
            contents = cont;
            AppendOperations();
        }
        public Node(Node inheritNode)
        {
            type = inheritNode.type;
            contents = inheritNode.contents;
            AppendOperations();
        }
        public void AppendOperations() //Append the token methods as appropriate
        {
            switch (type)
            {
                case NodeContentType.Integer:
                    {
                        itemMethod = OperatorDefinitions.IntegerMethods;
                        break;
                    }
                case NodeContentType.Decimal:
                    {
                        itemMethod = OperatorDefinitions.DecimalMethods;
                        break;
                    }
                case NodeContentType.String:
                    {
                        itemMethod = OperatorDefinitions.StringMethods;
                        break;
                    }
                case NodeContentType.Boolean:
                    {
                        itemMethod = OperatorDefinitions.BooleanMethods;
                        break;
                    }
                default:
                    break;
            }
        }
    }

    public class Tree
    {
        public Node myNode; //Operator 
        public VariantNode? leftNode;
        public VariantNode? rightNode;

        public Tree(Node self, VariantNode? left, VariantNode? right)
        {
            myNode = self;
            leftNode = left;
            rightNode = right;
        }
        public string PrintTreeContents()
        {
            string corrString = "(";
            corrString += myNode.contents;

            if (leftNode != null)
            {
                if (leftNode._item.GetType() == typeof(Tree))
                {
                    corrString += ((Tree)leftNode._item).PrintTreeContents();
                }
                else
                {
                    corrString += ((Node)leftNode._item).contents;
                }
            }
            else
            {
                corrString += "<NULL>";
            }

            if (rightNode != null)
            {
                if (rightNode._item.GetType() == typeof(Tree))
                {
                    corrString += ((Tree)rightNode._item).PrintTreeContents();
                }
                else
                {
                    corrString += ((Node)rightNode._item).contents;
                }
            }
            else
            {
                corrString += "<NULL>";
            }

            corrString += ")";

            return corrString;
        }
        public Node? CalculateTreeResult()
        {
            Node? leftValue = null;
            Node? rightValue = null;

            if (leftNode != null)
            {
                if (leftNode._item.GetType() == typeof(Node))
                {
                    leftValue = new Node((Node)(leftNode._item)); //Get node value
                }
                else
                {
                    leftValue = new Node(((Tree)(leftNode._item)).CalculateTreeResult()); //Get tree result from this item
                }
            }

            if (rightNode != null)
            {
                if (rightNode._item.GetType() == typeof(Node))
                {
                    rightValue = new Node((Node)(rightNode._item)); //Get node value
                }
                else
                {
                    rightValue = new Node(((Tree)(rightNode._item)).CalculateTreeResult()); //Get tree result from this item
                }
            }

            if (myNode.type == NodeContentType.End)
            {
                if(leftValue != null && rightValue == null)
                {
                    return leftValue;
                }
                else if(leftValue == null && rightValue != null)
                {
                    return rightValue;
                }
                else if(leftValue == null && rightValue == null)
                {
                    return null;
                }
                else
                {
                    throw new Exception("END NODE HAD MULTIPLE VALUES");
                    return null;
                }
            }
            else if(leftValue != null || rightValue != null)
            {
                return leftValue.itemMethod(leftValue, rightValue, myNode); //Use operator to calculate result of the query
            }
            else
            {
                throw new Exception("Calculation Error");
                return null;
            }
        }
    }
}

