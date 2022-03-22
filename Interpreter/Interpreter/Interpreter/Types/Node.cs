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
    public class Node
    {
        public NodeContentType type;
        public string contents;
        public Func<Node, Node, Node, Node?> itemMethod;
        public Node(Token parentToken)
        {
            type = (NodeContentType)parentToken.type;
            contents = parentToken.contents;
            AppendOperations();
        }
        public Node(NodeContentType typ, string cont)
        {
            type = typ;
            contents = cont;
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
        public Node myNode;
        public Node leftNode;
        public Node rightNode;

        public Tree(Node self, Node left, Node right)
        {
            myNode = self;
            leftNode = left;
            rightNode = right;
        }
    }
}

