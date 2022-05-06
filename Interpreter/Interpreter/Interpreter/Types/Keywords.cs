using System;
using System.Collections.Generic;
using System.Text;
using Nodes;
using TypeDef;
using Interpreter;
using System.Linq;
namespace Keywords //File for keywords and built in functions/statements
{
    public static class Keywords
    {
        public static Dictionary<string,(Action<VariantNode[]> act, string delimStart, string delimEnd, string? secDelimStart, string? secDelimEnd)> keywords = new Dictionary<string, (Action<VariantNode[]> act, string delimStart, string delimEnd, string? secDelimStart, string? secDelimEnd)>() //Keywords and their functions
        {
            {"print",(PrintStatement,"(",")",null,null)}, //print(expression)
            {"input",(InputStatement,"(",")",null,null)}, //input(variable to assign to)
            {"while",(WhileStatement,"(",")","{","}")}, //while(condition){commands}
            {"if",(IfStatement,"(",")","{","}")}, //if(condition){commands}
            {"func",(FuncStatement,"(",")","{","}")}, //func(name){commands}
            {"call",(CallFuncStatement,"(",")",null,null)}, //call(funcname)
            {"int",(CreateInt," "," ",null,null)}, //int var
            {"float",(CreateDecimal," "," ",null,null)}, //float var
            {"string",(CreateString," "," ",null,null)}, //string var
            {"bool",(CreateBool," "," ",null,null)}, //bool var
        };
        public static (List<VariantNode> out1, int out1c, Tree out2) SubsetStatement(string statementName, List<VariantNode> items) //Returns the nodes within the statement parameters. item input is all nodes after the statement, unsorted.
        {
            if (items.Count == 0) { throw new Exception("Unexpected keyword '" + statementName + "'"); }
            //Assumes the statement has been removed but the delimiters havent
            switch(statementName)
            {
                case "print":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "input":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "while":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "if":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "func":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "call":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "int":
                    {
                        return (new List<VariantNode>() { items[0] },1, null); //Gets next item in the command
                    }
                case "float":
                    {
                        return (new List<VariantNode>() { items[0] }, 1, null);  //Gets next item in the command
                    }
                case "string":
                    {
                        return (new List<VariantNode>() { items[0] }, 1, null);  //Gets next item in the command
                    }
                case "bool":
                    {
                        return (new List<VariantNode>() { items[0] }, 1, null);  //Gets next item in the command
                    }
                default:
                    throw new Exception("Unknown Statement '" + statementName + "'");
            }
        }

        private static (List<VariantNode> out1, int out1c, Tree out2) SimpleStatementDelim(string statementName, List<VariantNode> items) //Returns all between delimiting characters, accounting for others
        {
            (List<VariantNode> out1, int out1c, Tree out2) outputs = (new List<VariantNode>(){ },0,null);
            int maxIter = 0;
            if(keywords[statementName].secDelimStart == null) { maxIter = 1; }
            else { maxIter = 2; }

            for (int i = 0; i < maxIter; i++)
            {
                string delimStart = "";
                string delimEnd = "";

                if(i == 0)
                {
                    delimStart = keywords[statementName].delimStart;
                    delimEnd = keywords[statementName].delimEnd;
                }
                else
                {
                    delimStart = keywords[statementName].secDelimStart;
                    delimEnd = keywords[statementName].secDelimEnd;
                }
                List<VariantNode> outputSet = new List<VariantNode>();
                int counter = 0; //Counts the number of items in the set including delimiters
                bool isFinish = false; //Checks if the statement has both delimiting characters

                int setCounter = -1; //Adds when reaching the delimiting character, if reaching the end or end character and at 0, it selects the set
                for(int c = outputs.out1c;c<items.Count;c++) //Iterates through the set, starting at the initialised value. 
                {
                    VariantNode x = items[c];
                    counter++;

                    if (x._item.GetType() == typeof(Node) && ((Node)(x._item)).contents.ReturnShallowValue().ToString() == delimEnd && setCounter >= 0)
                    {
                        if (setCounter == 0)
                        {
                            isFinish = true;
                            break;
                        }

                        setCounter--;
                    }

                    if (setCounter >= 0)
                    {
                        outputSet.Add(x); //Append to the output if within the range
                    }

                    if (x._item.GetType() == typeof(Node) && ((Node)(x._item)).contents.ReturnShallowValue().ToString() == delimStart)
                    {
                        setCounter++;
                    }
                }

                if (setCounter != 0 || !isFinish)
                {
                    throw new Exception("No matching " + delimEnd + " for " + delimStart);
                }
                else
                {
                    if (i == 0) { outputs.out1 = outputSet; outputs.out1c = counter; }
                    else { outputs.out2 = outputSet[0].ToTree();}
                }
            }

            return outputs;
        }
        public static void RouteStatement(string statementName, VariantNode[] args)
        {
            VariantNode[] passedArgs = args.Where(x => x != null).ToArray(); //Get array of non-null values

            if (keywords.ContainsKey(statementName))
            {
                if (passedArgs.Count() == 0) { throw new Exception("Unexpected keyword '" + statementName + "'"); }
                keywords[statementName].act.Invoke(passedArgs);
            }
            else
            {
                throw new Exception("Unknown function " + statementName);
            }
        }

        private static void PrintStatement(VariantNode[] nodeInput) //Print the appropriate value to console
        {
            if(nodeInput.Count() > 1 || ((Node)(nodeInput[0]._item)).contents.GetType() == typeof(TypeTemplate.Identifier) && !Interpreter.Interpreter.globalVars.VarContains(((Node)(nodeInput[0]._item)).contents.ReturnShallowValue().ToString()))
            {
                throw new Exception("Invalid parameter(s) in print function");
            }
            Console.WriteLine(((Node)(nodeInput[0]._item)).contents.ReturnDeepValue()); 
        }
        private static void WhileStatement(VariantNode[] nodeInput) //Repeat the tree contents of node 0 until node 1s condition is met
        {
            if (nodeInput.Count() != 2)
            {
                throw new Exception("Invalid parameter(s) in while function");
            }

            while (true)
            {
                if (IsCondition(nodeInput[1]))
                {
                    ((Tree)(nodeInput[0]._item)).CalculateTreeResult();
                }
                else
                {
                    break;
                }
            }
        }

        private static void IfStatement(VariantNode[] nodeInput) //Run contents of node 0 if node 1 is correct
        {
            if (nodeInput.Count() != 2)
            {
                throw new Exception("Invalid parameter(s) in if function");
            }

            if (IsCondition(nodeInput[1]))
            {
                ((Tree)(nodeInput[0]._item)).CalculateTreeResult();
            }
        }

        private static void FuncStatement(VariantNode[] nodeInput) //Store tree of node 0 in identifier of node 1
        {
            if (nodeInput.Count() != 2)
            {
                throw new Exception("Invalid parameter(s) in function declaration");
            }

            if(nodeInput[1]._item.GetType() != typeof(Node))
            {
                throw new Exception("Invalid name for function");
            }
            if(nodeInput[0]._item.GetType() != typeof(Tree))
            {
                throw new Exception("Invalid function contents");
            }

            string name = ((Node)(nodeInput[1]._item)).contents.ReturnShallowValue();
            if (((Node)(nodeInput[1]._item)).type != NodeContentType.Identifier) { throw new Exception("Invalid function name: " + name); }

            if (Interpreter.Interpreter.globalVars.FuncContains(name))
            {
                throw new Exception("Redeclaration of function " + name);
            }
            else
            {
                Interpreter.Interpreter.globalVars.FuncAddNewItem(name, (Tree)(nodeInput[0]._item));
            }
        }
        private static void CallFuncStatement(VariantNode[] nodeInput) //Run the func specified in nodeInput[0]
        {
            if (nodeInput.Count() != 1)
            {
                throw new Exception("Invalid parameter(s) in function call function ");
            }
            else if(((Node)(nodeInput[0]._item)).contents.GetType() != typeof(TypeTemplate.Identifier))
            {
                throw new Exception("Function calls must use an identifier");
            }
            else if(((Node)(nodeInput[0]._item)).contents.GetType() == typeof(TypeTemplate.Identifier) && !Interpreter.Interpreter.globalVars.FuncContains(((Node)(nodeInput[0]._item)).contents.ReturnShallowValue().ToString()))
            {
                throw new Exception("Unknown function: " + ((Node)(nodeInput[0]._item)).contents.ReturnShallowValue().ToString());
            }

            string name = ((Node)(nodeInput[0]._item)).contents.ReturnShallowValue().ToString();

            Interpreter.Interpreter.globalVars.FuncGetTree(name).CalculateTreeResult(); //Run function
        }

        private static bool IsCondition(VariantNode conditional)
        {
            if (conditional._item.GetType() == typeof(Tree)) //Tree type
            {
                Node? condTreeResult = ((Tree)(conditional._item)).CalculateTreeResult();

                if (condTreeResult == null || condTreeResult.contents.ReturnDeepVarType() != typeof(TypeDef.TypeTemplate.Boolean))
                {
                    throw new Exception("Invalid condition in function");
                }
                if (!Convert.ToBoolean(condTreeResult.contents.ReturnDeepValue())) { return false; }
                else { return true; }
            }
            else //Node type
            {
                if (conditional == null || ((Node)(conditional._item)).contents.ReturnDeepVarType() != typeof(TypeDef.TypeTemplate.Boolean))
                {
                    throw new Exception("Invalid condition in function");
                }
                if (!Convert.ToBoolean(((Node)(conditional._item)).contents.ReturnDeepValue())) { return false; }
                else { return true; }
            }
        }
        private static void InputStatement(VariantNode[] nodeInput) //Ask for input and then assign to variable
        {
            if (nodeInput.Count() > 1 || ((Node)(nodeInput[0]._item)).contents.GetType() != typeof(TypeTemplate.Identifier) || !Interpreter.Interpreter.globalVars.VarContains(((Node)(nodeInput[0]._item)).contents.ReturnShallowValue().ToString()))
            {
                throw new Exception("Invalid parameter(s) in input function");
            }

            string input = Console.ReadLine();

            if (((Node)(nodeInput[0]._item)).type == NodeContentType.Identifier && Interpreter.Interpreter.globalVars.VarContains(((Node)(nodeInput[0]._item)).contents.ReturnShallowValue()))
            {
                if (Item.Assignable(((Node)(nodeInput[0]._item)).contents, input))
                {
                    Interpreter.Interpreter.globalVars.VarUpdateItem(((Node)(nodeInput[0]._item)).contents.ReturnShallowValue(), input);//Set value into variable
                }
            }
            else
            {
                throw new Exception("Unknown variable '" + ((Node)(nodeInput[0]._item)).contents.ReturnShallowValue() + "'");
            }

        }

        //Variable creators
        private static void CreateInt(VariantNode[] nodeInput){CreateNewVar((Node)(nodeInput[0]._item), NodeContentType.Integer);}
        private static void CreateDecimal(VariantNode[] nodeInput) { CreateNewVar((Node)(nodeInput[0]._item), NodeContentType.Decimal); }
        private static void CreateString(VariantNode[] nodeInput) { CreateNewVar((Node)(nodeInput[0]._item), NodeContentType.String); }
        private static void CreateBool(VariantNode[] nodeInput) { CreateNewVar((Node)(nodeInput[0]._item), NodeContentType.Boolean); }

        private static void CreateNewVar(Node newItem, NodeContentType type)
        {
            if (newItem.type != NodeContentType.Identifier) { throw new Exception("Invalid variable name: " + newItem.contents.ReturnShallowValue()); }
            if (Interpreter.Interpreter.globalVars.VarContains(newItem.contents.ReturnShallowValue()))
            {
                throw new Exception("Redeclaration of variable " + newItem.contents.ReturnShallowValue());
            }
            else
            {
                Interpreter.Interpreter.globalVars.VarAddNewItem(newItem.contents.ReturnShallowValue(), new Item(type, null));
            }
        }
    }
}
