using System;
using System.Collections.Generic;
using System.Text;
using Nodes;

namespace SyntaxTree
{
    public static class SyntaxTreeGenerator
    {

        public static Tree GenerateTree(Queue<Node> nodeSet) //Turn a postfix node set into a tree
        {
            Stack<VariantNode> storedNodes = new Stack<VariantNode>();

            while(nodeSet.Count > 0)
            {
                storedNodes.Push(new VariantNode(nodeSet.Dequeue())); //Remove node and push into stored nodes

                if (storedNodes.Peek()._item.GetType() == typeof(Node)) //If node type 
                {
                    if (((Node)storedNodes.Peek()._item).type == NodeContentType.Operation) //If it is an operation, form a tree
                    {
                        if (((Node)storedNodes.Peek()._item).contents.GetStringContents() != "!")
                        {
                            storedNodes.Push(new VariantNode(MakeTreeStack(ref storedNodes))); //Push new tree item
                        }
                        else
                        {
                            storedNodes.Push(new VariantNode(MakeNot(ref storedNodes))); //Make not operation
                        }
                    }
                    else if(((Node)storedNodes.Peek()._item).type == NodeContentType.End) //If it is an end node, form a tree
                    {
                        storedNodes.Push(new VariantNode(MakeTreeStack(ref storedNodes))); //Push new tree item
                    }
                }
                else if (storedNodes.Peek()._item.GetType() == typeof(Tree)) //If Tree type 
                {
                    storedNodes.Push(new VariantNode(new Tree((Node)storedNodes.Pop()._item, storedNodes.Pop(), storedNodes.Pop()))); //Push new tree item
                }
            }

            Tree newTree = storedNodes.Pop().ToTree();
            return newTree;
        }

        private static Tree MakeTreeStack(ref Stack<VariantNode> nodeStack)
        {
            Node opNode = (Node)nodeStack.Pop()._item;
            VariantNode? right = null;
            VariantNode? left = null;

            if (nodeStack.Count > 0) //Check for an empty stack
            {
                right = nodeStack.Pop();
            }

            if (nodeStack.Count > 0) //Check for an empty stack
            {
                left = nodeStack.Pop();
            }

            return new Tree(opNode, left, right);
        }

        private static Tree MakeNot(ref Stack<VariantNode> nodeStack)
        {
            Node opNode = (Node)nodeStack.Pop()._item;
            VariantNode? left = null;

            if (nodeStack.Count > 0) //Check for an empty stack
            {
                left = nodeStack.Pop();
            }

            return new Tree(opNode, left, null);
        }
    }
}
