﻿using System;
using System.Collections.Generic;
using System.Text;
using Tokens;
using NodeOperations;
using TypeDef;
using Interpreter; //For loading global variables
using System.Linq;
using Keywords;

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
            Item newItem = new Item(Node.contentRef[contents.GetType()], contents.ReturnDeepValue());
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
            if(type == NodeContentType.Identifier && IsKeyword(contents.ReturnShallowValue()))
            {
                type = NodeContentType.Keyword;
            }
        }

        public static bool IsKeyword(string item)
        {
            return Keywords.Keywords.keywords.ContainsKey(item);
        }
        public bool GetVariable(string name, NodeContentType typing)
        {
            if(typing == NodeContentType.Identifier) //Check if variable
            {
                if(Interpreter.Interpreter.globalVars.Contains(name) && !Keywords.Keywords.keywords.ContainsKey(name))
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
        public List<VariantNode?> nodes = new List<VariantNode?>(); // 0 as left, 1 as right

        public Tree(Node self, VariantNode? left, VariantNode? right)
        {
            myNode = self;
            nodes.Add(left);
            nodes.Add(right);
        }

        public Tree(Node self, VariantNode?[] compNodes)
        {
            myNode = self;
            nodes = compNodes.ToList();
        }
        public void InsertAtNextCommand(Tree newCommandTree) //Inserts a new command after the end node
        {
            if(myNode.type == NodeContentType.End)
            {
                if(nodes.Count == 2) //If completed tree
                {
                    nodes.Add(new VariantNode(newCommandTree)); //Add tree as 3rd item to end node
                }
                else if(nodes.Count == 3)
                {
                    nodes[2].ToTree().InsertAtNextCommand(newCommandTree); //Add the node at next command
                }
                
            }
        }
        public string PrintTreeContents()
        {
            string corrString = "(";
            corrString += myNode.contents.ReturnShallowValue();


            foreach(VariantNode x in nodes)
            {
                if (x != null)
                {
                    if (x._item.GetType() == typeof(Tree))
                    {
                        corrString += ((Tree)x._item).PrintTreeContents();
                    }
                    else
                    {
                        corrString += ((Node)x._item).contents.ReturnShallowValue();
                    }
                }
                else
                {
                    corrString += "<NULL>";
                }
            }

            corrString += ")";

            return corrString;
        }
        public Node? CalculateTreeResult(ref int line)
        {
            Node? leftValue = null;
            Node? rightValue = null;

            if (nodes[0] != null)
            {
                if (nodes[0]._item.GetType() == typeof(Node))
                {
                    leftValue = new Node((Node)(nodes[0]._item)); //Get node value
                }
                else
                {
                    leftValue = new Node(((Tree)(nodes[0]._item)).CalculateTreeResult(ref line)); //Get tree result from this item
                }
            }

            if (nodes[1] != null)
            {
                if (nodes[1]._item.GetType() == typeof(Node))
                {
                    rightValue = new Node((Node)(nodes[1]._item)); //Get node value
                }
                else
                {
                    rightValue = new Node(((Tree)(nodes[1]._item)).CalculateTreeResult(ref line)); //Get tree result from this item
                }
            }

            if (myNode.type == NodeContentType.End)
            {
                line++;
                if(nodes.Count > 2) //If there are more commands to process
                {
                    return new Node(((Tree)(nodes[2]._item)).CalculateTreeResult(ref line)); //Process the next command in the set
                }
                else
                {
                    return myNode;
                }
            }
            else if(myNode.type == NodeContentType.Keyword)
            {
                Keywords.Keywords.RouteStatement(myNode.contents.ReturnShallowValue(), new Node[]{leftValue, rightValue});
                return myNode;
            }
            else if(leftValue != null || rightValue != null) //Operator
            {
                return OperatorInteractions.Interact(leftValue, rightValue, myNode); //Use operator to calculate result of the query
            }
            else
            {
                throw new Exception("Calculation Error");
            }
        }
    }
}

