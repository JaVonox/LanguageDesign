using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using NodeOperations;
using TypeDef;
using Interpreter; //For loading global variables
using System.Linq;

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
        public static Dictionary<string, Item> variables = new Dictionary<string, Item>() {};
        public bool Contains(string name)
        {
            return variables.ContainsKey(name);
        }

        public bool Contains(Item item)
        {
            return variables.Values.Contains(item);
        }

        public Item GetItem(string name)
        {
            return variables[name];
        }
        public void UpdateItem(string name, object contents) //Need to add type checking TODO
        {
            variables[name] = new Item(Node.contentRef[variables[name].GetType()], contents);
        }
        public Item AddNewItem(string name, Item contents) //Add new variable and return the newly created variable ref
        {
            if (Node.IsKeyword(name)) { throw new Exception("Invalid variable name"); }
            Item newItem = new Item(Node.contentRef[contents.GetType()], contents.ReturnValue());
            variables.Add(name, newItem);
            return variables[name];
        }
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

        public static List<string> keywords = new List<string>() //Keywords
        {
            "if",
            "int",
            "print",
        };

        public NodeContentType type;
        public Item contents;
        public Node(Token parentToken)
        {
            if(!GetVariable(parentToken.contents,(NodeContentType)parentToken.type))
            {
                contents = new Item(parentToken);
                type = (NodeContentType)parentToken.type;
                CheckKeyword();
            }
        }
        public Node(NodeContentType typ, Item newItem)
        {
            contents = newItem;
            type = typ;
        }
        public Node(NodeContentType typ, string cont)
        {
            if (!GetVariable(cont.ToString(),typ))
            {
                type = typ;
                contents = new Item(typ, cont);
                CheckKeyword();
            }
        }
        public Node(Node inheritNode)
        {
            type = inheritNode.type;
            contents = inheritNode.contents;
        }
        public void CheckKeyword()
        {
            if(type == NodeContentType.Identifier && IsKeyword(contents.ReturnValue()))
            {
                type = NodeContentType.Keyword;
            }
        }

        public static bool IsKeyword(string item)
        {
            return keywords.Contains(item);
        }
        public bool GetVariable(string name, NodeContentType typing)
        {
            if(typing == NodeContentType.Identifier) //Check if variable
            {
                if(Interpreter.Interpreter.globalVars.Contains(name) && !keywords.Contains(name))
                {
                    contents = Interpreter.Interpreter.globalVars.GetItem(name);
                    type = contentRef[contents.GetType()];
                    return true;
                }
            }
            return false;
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
                else if(leftValue.type == NodeContentType.Keyword)
                {
                    return OperatorInteractions.Interact(rightValue, null, leftValue); //Process single argument keywords
                }
                else
                {
                    throw new Exception("END NODE HAD MULTIPLE VALUES");
                    return null;
                }
            }
            else if(leftValue != null || rightValue != null) //Operator or keyword
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

