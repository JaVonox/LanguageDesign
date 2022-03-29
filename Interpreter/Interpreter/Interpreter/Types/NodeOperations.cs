using System;
using System.Collections.Generic;
using System.Text;
using Nodes;

namespace NodeOperations
{
    public static class OperatorDefinitions //Methods for token creation
    {

        //TODO
        //THIS SHOULD ALL BE DEPRICIATED. IT SHOULD BE REPLACED BY THE ITEMS OPERATIONS. FOR NOW MOST OF THE STUFF WONT WORK>
        public static Node? IntegerMethods(Node lNode, Node rNode, Node opNode)
        {
            switch (opNode.contents.ReturnValue())
            {
                case "+":
                    {
                        return new Node(NodeContentType.Integer, lNode.contents + rNode.contents); //TODO this may not always return integer. Gotta rework this
                    }
                case NodeContentType.Decimal:
                    {
                        if (opNode.contents.ReturnValue() == "+") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) + Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "-") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) - Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "*") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) * Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents.ReturnValue() == "<") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) < Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == ">") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) > Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) <= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) >= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "==") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
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
                        if (opNode.contents.ReturnValue() == "+") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) + Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "-") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) - Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "*") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) * Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents.ReturnValue() == "<") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) < Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == ">") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) > Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) <= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) >= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "==") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) != Convert.ToDecimal(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                case NodeContentType.Decimal:
                    {
                        if (opNode.contents.ReturnValue() == "+") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) + Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "-") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) - Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "*") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) * Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "/") { return new Node(NodeContentType.Decimal, (Convert.ToDecimal(lNode.contents) / Convert.ToDecimal(rNode.contents)).ToString()); }

                        else if (opNode.contents.ReturnValue() == "<") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) < Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == ">") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) > Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "<=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) <= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == ">=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) >= Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "==") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) == Convert.ToDecimal(rNode.contents)).ToString()); }
                        else if (opNode.contents.ReturnValue() == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToDecimal(lNode.contents) != Convert.ToDecimal(rNode.contents)).ToString()); }
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
            if (opNode.contents.ReturnValue() == "!") //Special operation
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
                            if (opNode.contents.ReturnValue() == "&&") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) && Convert.ToBoolean(rNode.contents)).ToString()); }
                            else if (opNode.contents.ReturnValue() == "||") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) || Convert.ToBoolean(rNode.contents)).ToString()); }
                            else if (opNode.contents.ReturnValue() == "==") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) == Convert.ToBoolean(rNode.contents)).ToString()); }
                            else if (opNode.contents.ReturnValue() == "!=") { return new Node(NodeContentType.Boolean, (Convert.ToBoolean(lNode.contents) != Convert.ToBoolean(rNode.contents)).ToString()); }
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
                        if (opNode.contents.ReturnValue() == "!") { return new Node(NodeContentType.Boolean, (!Convert.ToBoolean(rNode.contents)).ToString()); }
                        else { throw new Exception("Invalid operation attempted"); }
                    }
                default:
                    throw new Exception("Invalid operation attempted");
            }
        }

        public static Node? StringMethods(Node lNode, Node rNode, Node opNode)
        {
            if (opNode.contents.ReturnValue() == "+")
            {
                return new Node(NodeContentType.String, lNode.contents.GetStringContents() + rNode.contents.GetStringContents());
            }
            else
            {
                switch (rNode.type)
                {
                    case NodeContentType.String:
                        {
                            if (opNode.contents.ReturnValue() == "==") { return new Node(NodeContentType.Boolean, (lNode.contents == rNode.contents).ToString()); }
                            if (opNode.contents.ReturnValue() == "!=") { return new Node(NodeContentType.Boolean, (lNode.contents != rNode.contents).ToString()); }
                            else { throw new Exception("Invalid operation attempted"); }
                        }
                    default:
                        throw new Exception("Invalid operation attempted");
                }

            }
        }
    }
}