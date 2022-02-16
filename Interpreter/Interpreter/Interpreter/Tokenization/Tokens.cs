using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Tokens
{
    public enum TokenType
    {
        End,
        Identifier,
        String,
        Integer,
        Operation,
        Decimal,
        Bracket,
    }
    public class Token
    {
        public TokenType? type;
        public string contents = "";
    }

    public class Conditions //Stores the conditions to apply to each token
    {
        public char? startDelim;
        public char?[] endDelim;
        public Func<string,bool> comparitor;
        public TokenType inType;
        public int charLimit;
        public Conditions(TokenType type, char? sDelim, char?[] eDelim, Func<string,bool> comparitive, int lim)
        {
            startDelim = sDelim;
            endDelim = eDelim;
            inType = type;
            comparitor = comparitive;
            charLimit = lim;
        }

        public bool RunConditions(char input, string tokenSet) //Run condition during data appending. This is used for delimiter operations
        {
            if (comparitor.Invoke(tokenSet))
            {
                return true;
            }

            return false;
        }
        public List<string> PollAllConditions(string dataSet) //Check that all contents fit the conditions. 0 is false, 1 is true, 2 is some valid items
        {

            if (startDelim != null || endDelim != null)
            {
                return null;
            }

            char[] chars = dataSet.ToCharArray();
            string appendedSet = "";
            List<string> validSet = new List<string>();

            int stringIter = -1;

            if(inType == TokenType.Operation)
            {
                if(TokenHandler.IsInteger(dataSet) || TokenHandler.IsDecimal(dataSet))
                {
                    return null;
                }
            }

            for (int i = 0; i < chars.Length; i++)
            {

                appendedSet += chars[i];
                if (comparitor.Invoke(appendedSet))
                {
                    if((validSet.Count - 1) < stringIter || stringIter == -1)
                    {
                        validSet.Add("");
                        stringIter = validSet.Count - 1;
                    }


                    if (charLimit == -1)
                    {
                        validSet[stringIter] = appendedSet;
                    }
                    else
                    {
                        if (appendedSet.Length == charLimit)
                        {
                            validSet[validSet.Count -1] = appendedSet;
                            stringIter = validSet.Count;
                            appendedSet = "";
                        }
                    }

                }
                else
                {
                    appendedSet = "";

                    if ((validSet.Count - 1) <= stringIter) //set to next index in validset
                    {
                        stringIter = validSet.Count;
                    }
                }
            }

            if(validSet.Count == 0) //No valid items
            {
                return null;
            }
            else //Multiple or single applicable
            {
                return validSet;
            }
        }
    }


    public static class TokenHandler
    {
        //Sets conditions for each tokentype
        public static Conditions[] condRefs =
        {
        new Conditions(TokenType.String,'"',new char?[]{'"'},new Func<string,bool>(IsStringChar),-1),
        new Conditions(TokenType.Identifier,null,null,new Func<string, bool>(IsIdentifier),-1),
        new Conditions(TokenType.Integer,null,null,new Func<string, bool>(IsInteger),-1),
        new Conditions(TokenType.Operation,null,null,new Func<string, bool>(IsOperator),1),
        new Conditions(TokenType.Decimal,null,null,new Func<string, bool>(IsDecimal),-1),
        new Conditions(TokenType.Bracket,null,null,new Func<string, bool>(IsBrackets),1),
        };

        public static void CreateTokens(string data, ref List<Token> tokens)
        {
            List<char?> endChars = new List<char?>() { ' ', '\t', '\n', ';' };
            try
            {
                bool tokenIsActive = false; //Stores if there is currently a token being appended to
                int curToken = -1;
                List<char?> delimChar = endChars;

                if(data.Length == 0)
                {
                    throw new Exception("No Data Input");
                }    

                for (int i = 0; i < data.Length; i++)
                {

                    if(IsEndStatement(data[i],ref delimChar)) //Check for ; character to signify end of statement
                    {
                        if(tokenIsActive) //If there is a token that needs verifying
                        {
                            ValidateToken(ref tokens, ref curToken, ref tokenIsActive, ref delimChar, ref endChars);
                        }

                        Token endToken = new Token();
                        endToken.type = TokenType.End;
                        endToken.contents = ";";
                        tokens.Add(endToken);
                        delimChar = endChars; //Reset delimiters

                        if(i+1 >= data.Length) //If end of file
                        {
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }

                    if(tokenIsActive == false)
                    {
                        if(!delimChar.Contains(data[i])) //skipped characters when there is no active token
                        {
                            curToken = tokens.Count; //get ID of new token
                            tokens.Add(new Token());

                            if (condRefs.Any(x => x.startDelim == data[i])) //If item matches start delimiter
                            {
                                Conditions tmpCond = condRefs.First(x => x.startDelim == data[i]);
                                tokens[curToken].type = tmpCond.inType;
                                if (tmpCond.endDelim != null) //If there is a valid end delimiter condition
                                {
                                    delimChar = tmpCond.endDelim.ToList();
                                }
                            }
                            else
                            {
                                delimChar = endChars;
                                tokens[curToken].contents += data[i];
                            }

                            tokenIsActive = true;
                        }
                    }
                    else
                    {
                        if (condRefs.Any(x => x.startDelim == data[i]) && tokens[curToken].type == null) //If item matches start delimiter
                        {
                            ValidateToken(ref tokens, ref curToken, ref tokenIsActive, ref delimChar, ref endChars);
                            curToken = tokens.Count; //get ID of new token
                            tokens.Add(new Token());

                            Conditions tmpCond = condRefs.First(x => x.startDelim == data[i]);
                            tokens[curToken].type = tmpCond.inType;
                            if (tmpCond.endDelim != null) //If there is a valid end delimiter condition
                            {
                                delimChar = tmpCond.endDelim.ToList();
                            }
                            tokenIsActive = true;
                        }
                        else
                        { 
                            if (!delimChar.Contains(data[i])) //If not an end char
                            {

                                TokenType? tokenTypeExists = tokens[curToken].type;
                                if (tokenTypeExists != null)
                                {
                                    if (condRefs.First(x => x.inType == tokenTypeExists).RunConditions(data[i], tokens[curToken].contents)) //Run the conditions for the selected token type
                                    {
                                        tokens[curToken].contents += data[i];
                                    }
                                    else //If the conditions for this predetermined item are not met, throw an exception
                                    {
                                        throw new Exception("Predetermined Type Violation");
                                    }
                                }
                                else
                                {
                                    tokens[curToken].contents += data[i];
                                }
                            }
                            else //when reaching a startchar or endchar
                            {
                                ValidateToken(ref tokens, ref curToken, ref tokenIsActive, ref delimChar, ref endChars);
                            }
                        }
                    }
                }

                if(tokenIsActive == true) //If there is still an active token
                {
                    ValidateToken(ref tokens, ref curToken, ref tokenIsActive, ref delimChar, ref endChars);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception while producing tokens - " + ex);
            }
        }

        private static void ValidateToken(ref List<Token> tokens, ref int curToken, ref bool tokenIsActive, ref List<char?> delimChar, ref List<char?> endChars) //Find valid type for token
        {
            tokenIsActive = false;
            delimChar = endChars; //reset delimiters

            if (tokens[curToken].type == null)
            {
                string contents = tokens[curToken].contents;
                List<(TokenType type, List<string> valid)> tokenOuts = condRefs.Where(x => x.PollAllConditions(contents) != null).Select(x=>(x.inType,x.PollAllConditions(contents).ToList())).ToList();

                if(tokenOuts.Count() == 0 || (tokenOuts.Count() > 1 && !tokenOuts.Any(x=> condRefs.First(y=>y.inType == x.type).charLimit != -1))) //If no matches or substrings with no potential dividers
                {
                    throw new Exception("No matching token type");
                }
                else if (tokenOuts.Count() > 1 || tokenOuts.Sum(x=>x.valid.Count()) > 1) //If multiple possible tokens
                {
                    TokenType[] set = tokenOuts.Select(x => condRefs.First(y => y.inType == x.type).inType).ToArray();

                    if (!tokenOuts.Any(x => condRefs.First(y => y.inType == x.type).charLimit == 1)) //If no divider types
                    {
                        throw new Exception("Ambiguity Error Between " + tokenOuts.Count() + " tokens");
                    }
                    else
                    {
                        ReorderTokens(tokenOuts, contents, ref tokens, ref curToken); //Split into different values
                    }
                }
                else
                {
                    tokens[curToken].type = tokenOuts[0].type;
                }
            }
        }

        private static void ReorderTokens(List<(TokenType type, List<string> valid)> tokenOuts, string targetString, ref List<Token> tokens, ref int curToken)
        {
            //Remove existing token and split into component parts

            tokens.RemoveAt(curToken);
            curToken--;

            char[] strSet = targetString.ToCharArray();

            string curString = "";
            foreach (char x in strSet)
            {
                curString += x;
                (int typeIndex ,int strIndex) entryToRemove = (-1,-1);
                foreach ((TokenType type, List<string> valid) components in tokenOuts)
                {
                    foreach(string str in components.valid)
                    {
                        if(curString == str)
                        {
                            Token tmpToken = new Token();
                            tmpToken.contents = str;
                            tmpToken.type = components.type;
                            curString = "";
                            entryToRemove = (tokenOuts.IndexOf(components), components.valid.IndexOf(str));

                            tokens.Add(tmpToken);
                            curToken++;
                        }
                    }
                }

                if(entryToRemove.typeIndex != -1)
                {
                    tokenOuts[entryToRemove.typeIndex].valid.RemoveAt(entryToRemove.strIndex); //Remove filled entry from set
                }
            }

        }

        //Conditionals
        public static bool IsStringChar(string stringSet) //Always true, as no characters cannot be contained in a string. This will never flag if the delimiter conditions are not met
        {
            return true;
        }

        public static bool IsIdentifier(string stringSet) //Checks for identifier pattern matches
        {
            if(char.IsDigit(stringSet[0])) { return false; } //First character cannot be a number
            return (stringSet.All(x=> char.IsLetterOrDigit(x) || x == '_'));
        }

        public static bool IsOperator(string stringSet)
        {
            return (stringSet == "+" || stringSet == "-" || stringSet == "*" || stringSet == "/");
        }
        public static bool IsInteger(string stringSet)
        {
            //Allow - at start, but not anywhere afterwards
            if(IsDecimal(stringSet)) { return false; }
            char[] numofminus = stringSet.Where(x => x == '-').ToArray();
            return (stringSet.StartsWith(new string(numofminus)) && stringSet.Length > numofminus.Count() && stringSet.Where(x => x != '-').ToArray().All(y => char.IsDigit(y))); //Check that all minuses are at the start of the string, and that the string does not contain just minuses
        }
        public static bool IsDecimal(string stringSet)
        {
            if(stringSet.Count(x=>x == '.') != 1) { return false; }
            else
            {
                string[] componentNums = stringSet.Split('.');

                if(IsInteger(componentNums[0]))
                {
                    return (componentNums[1].ToArray().All(y => char.IsDigit(y)));
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool IsBrackets(string stringSet)
        {
            return (stringSet == "(" || stringSet == ")");
        }
        public static bool IsEndStatement(char newChar, ref List<char?> delimChars) //Check for ; if ; is not expected
        {
            if (newChar == ';')
            {
                if (delimChars.Contains(';'))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
