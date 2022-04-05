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
        public static Dictionary<string,(Action<Node[]> act, string delimStart, string delimEnd)> keywords = new Dictionary<string, (Action<Node[]> act, string delimStart, string delimEnd)>() //Keywords and their functions
        {
            {"print",(PrintStatement,"(",")")}, //print(expression)
            {"int",(CreateInt," "," ")}, //int var
            {"float",(CreateDecimal," "," ")}, //float var
            {"string",(CreateString," "," ")}, //string var
            {"bool",(CreateBool," "," ")}, //bool var
        };
        public static (List<Node> outItems, int count) SubsetStatement(string statementName, List<Node> items) //Returns the nodes within the statement parameters. item input is all nodes after the statement, unsorted.
        {
            //Assumes the statement has been removed but the delimiters havent
            switch(statementName)
            {
                case "print":
                    {
                        return SimpleStatementDelim(statementName, items);
                    }
                case "int":
                    {
                        return (new List<Node>() { items[0] }, 1); //Gets next item in the command
                    }
                case "float":
                    {
                        return (new List<Node>() { items[0] }, 1); //Gets next item in the command
                    }
                case "string":
                    {
                        return (new List<Node>() { items[0] }, 1); //Gets next item in the command
                    }
                case "bool":
                    {
                        return (new List<Node>() { items[0] }, 1); //Gets next item in the command
                    }
                default:
                    throw new Exception("Unknown Statement '" + statementName + "'");
            }
        }

        private static (List<Node> outItems, int count) SimpleStatementDelim(string statementName, List<Node> items) //Returns all between delimiting characters, accounting for others
        {
            List<Node> outputSet = new List<Node>();
            int counter = 0; //Counts the number of items in the set including delimiters
            bool isFinish = false; //Checks if the statement has both delimiting characters

            int setCounter = -1; //Adds when reaching the delimiting character, if reaching the end or end character and at 0, it selects the set
            foreach (Node x in items)
            {
                counter++;
                if (x.contents.ReturnValue().ToString() == keywords[statementName].delimStart)
                {
                    setCounter++;
                }
                else if (x.contents.ReturnValue().ToString() == keywords[statementName].delimEnd)
                {
                    if(setCounter == 0)
                    {
                        isFinish = true;
                        break;
                    }

                    setCounter--;
                }

                if(setCounter >= 0)
                {
                    outputSet.Add(x); //Append to the output if within the range
                }
            }

            if (setCounter != 0 || !isFinish)
            {
                throw new Exception("No matching " + keywords[statementName].delimEnd + " for " + keywords[statementName].delimStart);
            }
            else
            {
                return (outputSet,counter);
            }
        }
        public static void RouteStatement(string statementName, Node[] args)
        {
            Node[] passedArgs = args.Where(x => x != null).ToArray(); //Get array of non-null values

            if (keywords.ContainsKey(statementName))
            {
                keywords[statementName].act.Invoke(passedArgs);
            }
            else
            {
                throw new Exception("Unknown function " + statementName);
            }
        }

        private static void PrintStatement(Node[] nodeInput) //Print the appropriate value to console
        {
            Console.WriteLine(nodeInput[0].contents.ReturnValue()); 
        }

        //Variable creators
        private static void CreateInt(Node[] nodeInput){CreateNewVar(nodeInput[0], NodeContentType.Integer);}
        private static void CreateDecimal(Node[] nodeInput) { CreateNewVar(nodeInput[0], NodeContentType.Decimal); }
        private static void CreateString(Node[] nodeInput) { CreateNewVar(nodeInput[0], NodeContentType.String); }
        private static void CreateBool(Node[] nodeInput) { CreateNewVar(nodeInput[0], NodeContentType.Boolean); }

        private static void CreateNewVar(Node newItem, NodeContentType type)
        {
            if (newItem.type != NodeContentType.Identifier) { throw new Exception("Invalid variable declaration or redeclaration of variable " + newItem.contents.ReturnValue()); }
            if (Interpreter.Interpreter.globalVars.Contains(newItem.contents.ReturnValue()))
            {
                throw new Exception("Redeclaration of variable " + newItem.contents.ReturnValue());
            }
            else
            {
                Interpreter.Interpreter.globalVars.AddNewItem(newItem.contents.ReturnValue(), new Item(type, null));
            }
        }
    }
}
