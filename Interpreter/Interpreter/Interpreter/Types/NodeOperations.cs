using System;
using System.Collections.Generic;
using System.Text;
using Nodes;

namespace NodeOperations
{
    public static class OperatorDefinitions //Methods for token creation
    {
        public static Node? IntegerMethods(Node lNode, Node rNode, Node opNode)
        {
            switch (rNode.type)
            {
                case NodeContentType.Integer:
                    {
                        if (opNode.contents == "+") { return new Node(NodeContentType.Integer, (Convert.ToInt32(lNode.contents) + Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == "-") { return new Node(NodeContentType.Integer, (Convert.ToInt32(lNode.contents) - Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == "*") { return new Node(NodeContentType.Integer, (Convert.ToInt32(lNode.contents) * Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents == "<") { return new Node(NodeContentType.Boolean, (Convert.ToInt32(lNode.contents) < Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">") { return new Node(NodeContentType.Boolean, (Convert.ToInt32(lNode.contents) > Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToInt32(lNode.contents) <= Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToInt32(lNode.contents) >= Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == "==") { return new Node(NodeContentType.Boolean, (Convert.ToInt32(lNode.contents) == Convert.ToInt32(rNode.contents)).ToString()); }
                        else if (opNode.contents == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToInt32(lNode.contents) != Convert.ToInt32(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                case NodeContentType.Decimal:
                    {
                        if (opNode.contents == "+") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) + Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "-") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) - Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "*") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) * Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents == "<") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) < Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) > Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) <= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) >= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "==") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                case NodeContentType.String:
                    {
                        return StringMethods(lNode, rNode, opNode);
                    }
                default:
                    throw new Exception("Invalid operation attempted");
            }
        }

        public static Node? DecimalMethods(Node lNode, Node rNode, Node opNode)
        {
            switch (rNode.type)
            {
                case NodeContentType.Integer:
                    {
                        if (opNode.contents == "+") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) + Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "-") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) - Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "*") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) * Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents == "<") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) < Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) > Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) <= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) >= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "==") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) != Convert.ToDecimal(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                case NodeContentType.Decimal:
                    {
                        if (opNode.contents == "+") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) + Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "-") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) - Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "*") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) * Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents == "<") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) < Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) > Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) <= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) >= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "==") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) != Convert.ToDecimal(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                case NodeContentType.String:
                    {
                        return StringMethods(lNode, rNode, opNode);
                    }
                default:
                    throw new Exception("Invalid operation attempted");
            }

        }

        public static Node? BooleanMethods(Node lNode, Node rNode, Node opNode)
        {
            if (opNode.contents == "!") //Special operation
            {
                if (lNode != null && rNode != null) { throw new Exception("Invalid parameters in not clause"); }
                else if (lNode != null)
                {
                    return NotOperator(lNode, opNode);
                }
                else if (rNode != null)
                {
                    return NotOperator(rNode, opNode);
                }
                else
                {
                    throw new Exception("Invalid operation attempted");
                }
            }
            else
            {
                switch (rNode.type)
                {
                    case NodeContentType.Boolean:
                        {
                            if (opNode.contents == "&&") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) && Convert.ToBoolean(rNode.contents)).ToString()); }
                            else if (opNode.contents == "||") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) || Convert.ToBoolean(rNode.contents)).ToString()); }
                            else if (opNode.contents == "==") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) == Convert.ToBoolean(rNode.contents)).ToString()); }
                            else if (opNode.contents == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) != Convert.ToBoolean(rNode.contents)).ToString()); }
                            else { throw new Exception("Invalid operation attempted"); }
                        }
                    default:
                        throw new Exception("Invalid operation attempted");
                }
            }

        }

        public static Node? NotOperator(Node rNode, Node opNode) //This is a special command reserved for reversing a single bool
        {
            switch (rNode.type)
            {
                case NodeContentType.Boolean:
                    {
                        if (opNode.contents == "!") { return new Node(NodeContentType.Boolean, (!Convert.ToBoolean(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                default:
                    throw new Exception("Invalid operation attempted");
            }
        }

        public static Node? StringMethods(Node lNode, Node rNode, Node opNode)
        {
            if (opNode.contents == "+")
            {
                return new Node(NodeContentType.String, lNode.contents + rNode.contents);
            }
            else
            {
                switch (rNode.type)
                {
                    case NodeContentType.String:
                        {
                            if (opNode.contents == "==") { return new Node(NodeContentType.Boolean, (lNode.contents == rNode.contents).ToString()); }
                            if (opNode.contents == "!=") { return new Node(NodeContentType.Boolean, (lNode.contents != rNode.contents).ToString()); }
                            else { throw new Exception("Invalid operation attempted"); }
                        }
                    default:
                        throw new Exception("Invalid operation attempted");
                }

            }
        }
    }
}