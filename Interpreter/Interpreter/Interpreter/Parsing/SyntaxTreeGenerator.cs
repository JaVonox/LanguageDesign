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
                        Node operation = (Node)storedNodes.Pop()._item;
                        VariantNode? right = storedNodes.Pop();
                        VariantNode? left = storedNodes.Pop();

                        storedNodes.Push(new VariantNode(new Tree(operation,left,right))); //Push new tree item
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
    }
}
