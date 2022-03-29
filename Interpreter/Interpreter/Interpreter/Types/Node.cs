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
        Identifier, //Variables
        Keyword, //Built in keywords
        String,
        Integer,
        Operation,
        Decimal,
        Bracket,
        Boolean,
    }

    public class Item
    {
        private TypeTemplate content { get; set; }

        public dynamic ReturnValue() //Return the type-adjusted contents of the item
        {
            return content.contents;
        }

        public Type GetType() //Return the value type
        {
            return content.GetType();
        }

        public Item(Node item)
        {
            Setter(item.type, item.contents);
        }
        public Item(NodeContentType type, object contents)
        {
            Setter(type,contents);
        }
        private void Setter(NodeContentType type, object contents)
        {
            switch (type)
            {
                case NodeContentType.Integer:
                    {
                        content = new TypeTemplate.Integer();
                        content.contents = contents;
                        break;
                    }
                case NodeContentType.Decimal:
                    {
                        content = new TypeTemplate.Decimal();
                        content.contents = contents;
                        break;
                    }
                case NodeContentType.String:
                    {
                        content = new TypeTemplate.String();
                        content.contents = contents;
                        break;
                    }
                default:
                    throw new Exception("Unimplemented type");
            }
        }
    }
    public abstract class TypeTemplate //Matching unit object
    {
        public TypeTemplate() { }
        private object _interiorContents;
        public abstract object contents //Abstract object - inherits type of the new typed item
        {
            get;
            set;
        }
        public sealed class Integer : TypeTemplate
        {
            public override object contents
            {
                get { return Convert.ToInt32(_interiorContents); }
                set { _interiorContents = value; }
            }

        }
        public sealed class Decimal : TypeTemplate
        {
            public override object contents
            {
                get { return Convert.ToDecimal(_interiorContents); }
                set { _interiorContents = value; }
            }
        }
        public sealed class String : TypeTemplate
        {
            public override object contents
            {
                get { return _interiorContents.ToString(); }
                set { _interiorContents = value; }
            }
        }
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
        public string contents;
        public Func<Node, Node, Node, Node?> itemMethod;
        public Node(Token parentToken)
        {
            contents = parentToken.contents;
            type = (NodeContentType)parentToken.type;
            IdentifierChecking();
            AppendOperations();
        }
        public Node(NodeContentType typ, string cont)
        {
            type = typ;
            contents = cont;
            IdentifierChecking();
            AppendOperations();
        }
        public Node(Node inheritNode)
        {
            type = inheritNode.type;
            contents = inheritNode.contents;
            IdentifierChecking();
            AppendOperations();
        }
        private void IdentifierChecking() //Check if the identifier item is a keyword, and reassign appropriately
        {
            if(type == NodeContentType.Identifier)
            {
                if(keywords.ContainsKey(contents))
                {
                    type = NodeContentType.Keyword;
                }
            }
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

