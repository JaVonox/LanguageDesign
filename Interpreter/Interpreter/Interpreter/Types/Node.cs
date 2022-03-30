using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using NodeOperations;
using TypeDef;

namespace Nodes
{
    public enum NodeContentType
    {
        End,
        Identifier, //Variables
        Keyword, //Built in keywords
        String,
        Integer,
        Operation,
        Decimal,
        Bracket,
        Boolean,
    }
    public class LoadedVariables //Variables
    {
        public static Dictionary<string, Node> variables = new Dictionary<string, Node>() { };
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
        public static Dictionary<string, object> keywords = new Dictionary<string, object>() //Keywords and their referenced things
        {
            {"if",null},
        };

        public NodeContentType type;
        public Item contents;
        public Node(Token parentToken)
        {
            contents = new Item(parentToken);
            type = (NodeContentType)parentToken.type;
        }
        public Node(NodeContentType typ, Item newItem)
        {
            contents = newItem;
            type = typ;
        }
        public Node(NodeContentType typ, string cont)
        {
            type = typ;
            contents = new Item(typ,cont);
        }
        public Node(Node inheritNode)
        {
            type = inheritNode.type;
            contents = inheritNode.contents;
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
            corrString += myNode.contents.ReturnValue();

            if (leftNode != null)
            {
                if (leftNode._item.GetType() == typeof(Tree))
                {
                    corrString += ((Tree)leftNode._item).PrintTreeContents();
                }
                else
                {
                    corrString += ((Node)leftNode._item).contents.ReturnValue();
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
                    corrString += ((Node)rightNode._item).contents.ReturnValue();
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
                return OperatorInteractions.Interact(leftValue, rightValue, myNode); //Use operator to calculate result of the query
            }
            else
            {
                throw new Exception("Calculation Error");
                return null;
            }
        }
    }
}

