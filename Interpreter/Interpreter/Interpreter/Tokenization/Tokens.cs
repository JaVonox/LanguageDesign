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
        Boolean,
    }
    public class Token
    {
        public TokenType? type;
        public string contents = "";
        public Func<Token,Token,Token,Token?> tokenMethod;

        public Token(TokenType typ, string cont)
        {
            type = typ;
            contents = cont;
            AppendTokenMethods(); //In this constructor, token methods are added immediately.
        }

        public Token() { }
        public void AppendTokenMethods() //Append the token methods as appropriate
        {
            switch(type)
            {
                case TokenType.Integer:
                    {
                        tokenMethod = TokenCombination.IntegerMethods;
                        break;
                    }
                case TokenType.Decimal:
                    {
                        tokenMethod = TokenCombination.DecimalMethods;
                        break;
                    }
                case TokenType.String:
                    {
                        tokenMethod = TokenCombination.StringMethods;
                        break;
                    }
                case TokenType.Boolean:
                    {
                        tokenMethod = TokenCombination.BooleanMethods;
                        break;
                    }
                default:
                    break;
            }
        }
    }

    public static class TokenCombination //Methods for token creation
    {
        public static Token? IntegerMethods(Token lToken, Token rToken, Token opToken)
        {
            switch(rToken.type)
            {
                case TokenType.Integer:
                    {
                        if(opToken.contents == "+") { return new Token(TokenType.Integer, (Convert.ToInt32(lToken.contents) + Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == "-") { return new Token(TokenType.Integer, (Convert.ToInt32(lToken.contents) - Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == "*") { return new Token(TokenType.Integer, (Convert.ToInt32(lToken.contents) * Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == "/") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) / Convert.ToDecimal(rToken.contents)).ToString()); }

                        else if (opToken.contents == "<") { return new Token(TokenType.Boolean, (Convert.ToInt32(lToken.contents) < Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">") { return new Token(TokenType.Boolean, (Convert.ToInt32(lToken.contents) > Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == "<=") { return new Token(TokenType.Boolean, (Convert.ToInt32(lToken.contents) <= Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">=") { return new Token(TokenType.Boolean, (Convert.ToInt32(lToken.contents) >= Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == "==") { return new Token(TokenType.Boolean, (Convert.ToInt32(lToken.contents) == Convert.ToInt32(rToken.contents)).ToString()); }
                        else if (opToken.contents == "!=") { return new Token(TokenType.Boolean, (Convert.ToInt32(lToken.contents) != Convert.ToInt32(rToken.contents)).ToString()); }
                        else { return null; }
                    }
                case TokenType.Decimal:
                    {
                        if (opToken.contents == "+") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) + Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "-") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) - Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "*") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) * Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "/") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) / Convert.ToDecimal(rToken.contents)).ToString()); }

                        else if (opToken.contents == "<") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) < Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) > Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "<=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) <= Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) >= Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "==") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) == Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "!=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) == Convert.ToDecimal(rToken.contents)).ToString()); }
                        else { return null; }
                    }
                case TokenType.String:
                    {
                        return StringMethods(lToken, rToken, opToken);
                    }
                default:
                    return null;
            }
        }

        public static Token? DecimalMethods(Token lToken, Token rToken, Token opToken)
        {
            switch (rToken.type)
            {
                case TokenType.Integer:
                    {
                        if (opToken.contents == "+") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) + Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "-") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) - Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "*") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) * Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "/") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) / Convert.ToDecimal(rToken.contents)).ToString()); }

                        else if (opToken.contents == "<") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) < Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) > Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "<=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) <= Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) >= Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "==") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) == Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "!=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) != Convert.ToDecimal(rToken.contents)).ToString()); }
                        else { return null; }
                    }
                case TokenType.Decimal:
                    {
                        if (opToken.contents == "+") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) + Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "-") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) - Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "*") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) * Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "/") { return new Token(TokenType.Decimal, (Convert.ToDecimal(lToken.contents) / Convert.ToDecimal(rToken.contents)).ToString()); }

                        else if (opToken.contents == "<") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) < Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) > Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "<=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) <= Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == ">=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) >= Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "==") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) == Convert.ToDecimal(rToken.contents)).ToString()); }
                        else if (opToken.contents == "!=") { return new Token(TokenType.Boolean, (Convert.ToDecimal(lToken.contents) != Convert.ToDecimal(rToken.contents)).ToString()); }
                        else { return null; }
                    }
                case TokenType.String:
                    {
                        return StringMethods(lToken, rToken, opToken);
                    }
                default:
                    return null;
            }

        }

        public static Token? BooleanMethods(Token lToken, Token rToken, Token opToken)
        {
            switch (rToken.type)
            {
                case TokenType.Boolean:
                    {
                        if (opToken.contents == "&&") { return new Token(TokenType.Boolean, (Convert.ToBoolean(lToken.contents) && Convert.ToBoolean(rToken.contents)).ToString()); }
                        else if (opToken.contents == "||") { return new Token(TokenType.Boolean, (Convert.ToBoolean(lToken.contents) || Convert.ToBoolean(rToken.contents)).ToString()); }
                        else if (opToken.contents == "==") { return new Token(TokenType.Boolean, (Convert.ToBoolean(lToken.contents) == Convert.ToBoolean(rToken.contents)).ToString()); }
                        else if (opToken.contents == "!=") { return new Token(TokenType.Boolean, (Convert.ToBoolean(lToken.contents) != Convert.ToBoolean(rToken.contents)).ToString()); }
                        else { return null; }
                    }
                default:
                    return null;
            }

        }

        public static Token? NotOperator(Token rToken, Token opToken) //This is a special command reserved for reversing a single bool
        {
            switch (rToken.type)
            {
                case TokenType.Boolean:
                    {
                        if (opToken.contents == "!") { return new Token(TokenType.Boolean, (!Convert.ToBoolean(rToken.contents)).ToString()); }
                        else { return null; }
                    }
                default:
                    return null;
            }
        }

        public static Token? StringMethods(Token lToken, Token rToken, Token opToken)
        {
            if (opToken.contents == "+")
            {
                return new Token(TokenType.String, lToken.contents + rToken.contents);
            }
            else
            {
                switch (rToken.type)
                {
                    case TokenType.String:
                        {
                            if (opToken.contents == "==") { return new Token(TokenType.Boolean, (lToken.contents == rToken.contents).ToString()); }
                            if (opToken.contents == "!=") { return new Token(TokenType.Boolean, (lToken.contents != rToken.contents).ToString()); }
                            else { return null; }
                        }
                    default:
                        return null;
                }

            }
        }
    }
    public class Conditions //Stores the conditions to apply to each token
    {
        public char? startDelim;
        public char?[] endDelim;
        public Func<string,bool>[] comparitors;
        public Func<string, bool> finalCheck;
        public int? maxLen;
        public TokenType inType;
        public Conditions(TokenType type, char? sDelim, char?[] eDelim, Func<string,bool>[] comparitive, Func<string,bool> fCheck, int? mLen)
        {
            startDelim = sDelim;
            endDelim = eDelim;
            inType = type;
            comparitors = comparitive;
            finalCheck = fCheck;
            maxLen = mLen;
        }
        public List<string> PollAllConditions(string dataSet) //Check that all contents fit the conditions
        {
            char[] chars = dataSet.ToCharArray();
            string appendedSet = "";
            List<string> validSet = new List<string>();

            int stringIter = 0;
            bool needNewIndex = true;

            for (int i = 0; i < chars.Length; i++)
            {
                appendedSet += chars[i];

                int conditionsMet = 0;
                foreach (Func<string, bool> condition in comparitors) //Loop through each condition
                {
                    if (condition.Invoke(appendedSet))
                    {
                        conditionsMet++;

                        if (conditionsMet == comparitors.Count()) //If this has met all the conditions
                        {
                            bool isStringStart = false;
                            if(inType == TokenType.String) //This only occurs if a string is connected to other values
                            {
                                if(appendedSet.Count(x => x == '\"') % 2 == 1)
                                {
                                    isStringStart = true;
                                }
                                else
                                {
                                    validSet[stringIter] = appendedSet;
                                }
                            } 

                            if (endDelim != null && (endDelim.Contains(chars[i]) && !isStringStart)) //If contains the endchar
                            {
                                appendedSet = "";
                                needNewIndex = true;
                            }
                            else
                            {
                                if (needNewIndex == true)
                                {
                                    validSet.Add("");
                                    stringIter = validSet.Count() - 1;
                                }

                                validSet[stringIter] = appendedSet;

                                if (maxLen != null && appendedSet.Length >= maxLen)
                                {
                                    appendedSet = "";
                                    needNewIndex = true;
                                }
                                else
                                {
                                    needNewIndex = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if(conditionsMet == 0) //If the string has not met any conditions, reset the string set
                        {
                            if(appendedSet.Count() > 1) //Allows for next character to be checked if this breaks a sequence
                            {
                                i--;
                            }

                            appendedSet = "";
                            needNewIndex = true;
                        }
                    }
                }
            }

            ApplyFinalConds(ref validSet);

            if (validSet.Count == 0) //No valid items
            {
                return null;
            }
            else //Multiple or single applicable
            {
                return validSet;
            }
        }

        public void ApplyFinalConds(ref List<string> validSet)
        {
            if (finalCheck != null) //Apply final checking on set
            {
                string[] tmpSet = validSet.ToArray();

                foreach (string x in tmpSet) //Apply final conditions to collated items
                {
                    if (!finalCheck.Invoke(x))
                    {
                        validSet.Remove(x);
                    }
                }
            }
        }
    }


    public static class TokenHandler
    {
        //Sets conditions for each tokentype
        public static Conditions[] condRefs =
        {
        new Conditions(TokenType.String,'"',new char?[]{'"'},new Func<string,bool>[]{IsStringChar},IsStringSet,null),
        new Conditions(TokenType.Identifier,null,null,new Func<string, bool>[]{IsIdentifier},IsValidIdentifier,null),
        new Conditions(TokenType.Decimal,null,null,new Func<string, bool>[]{ IsNumericOrDecimal, IsDecimal},IsntJustDot,null),
        new Conditions(TokenType.Integer,null,null,new Func<string, bool>[]{IsNumericOrDecimal},IsInteger,null), //By applying isInteger as a final conditional, we can remove all decimals.
        new Conditions(TokenType.Operation,null,null,new Func<string, bool>[]{IsOperatorChar},IsOperated,2),
        new Conditions(TokenType.Bracket,null,null,new Func<string, bool>[]{IsBrackets},null,1),
        new Conditions(TokenType.Boolean,null,null,new Func<string,bool>[]{IsBoolChars},IsBool,null),
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
                    bool newSet = false;

                    if (IsEndStatement(data[i], ref delimChar)) //Check for ; character to signify end of statement
                    {
                        if (tokenIsActive) //If there is a token that needs verifying
                        {
                            ValidateToken(ref tokens, ref curToken, ref tokenIsActive, ref delimChar, ref endChars);
                        }

                        Token endToken = new Token();
                        endToken.type = TokenType.End;
                        endToken.contents = ";";
                        tokens.Add(endToken);
                        delimChar = endChars; //Reset delimiters

                        if (i + 1 >= data.Length) //If end of file
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (tokenIsActive == false)
                        {
                            if (!delimChar.Contains(data[i])) //skipped characters when there is no active token
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
                                }
                                tokens[curToken].contents += data[i];
                                tokenIsActive = true;
                                newSet = true;
                            }
                        }

                        if ((!newSet || i == data.Length - 1) && tokenIsActive)
                        {
                            if (!newSet)
                            {
                                tokens[curToken].contents += data[i];
                            }

                            if (delimChar.Contains(data[i]) || i == data.Length - 1) //If found end character or reached end of text
                            {
                                ValidateToken(ref tokens, ref curToken, ref tokenIsActive, ref delimChar, ref endChars);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception while producing tokens - " + ex);
            }

            foreach(Token t in tokens)
            {
                if(t.type == TokenType.String)
                {
                    t.contents = t.contents.Replace("\"", "");
                }
                else
                {
                    t.contents = t.contents.Replace(" ", "");
                }

                t.AppendTokenMethods(); //Append the type specific token method
            }

        } 

        private static void ValidateToken(ref List<Token> tokens, ref int curToken, ref bool tokenIsActive, ref List<char?> delimChar, ref List<char?> endChars) //Find valid type for token
        {
            tokenIsActive = false;
            delimChar = endChars; //reset delimiters

            if (tokens[curToken].type == null)
            {
                string contents = tokens[curToken].contents;
                List<(TokenType type, List<string> valid)> tokenOuts =  condRefs.Select(x=> new { cond = x.PollAllConditions(contents), type = x.inType}).Where(y=> y.cond != null).Select(z => (z.type, z.cond.ToList())).ToList();

                if (tokenOuts.Count() == 0) //If no matches
                {
                    throw new Exception("No matching token type for supplied parameter");
                }
                else if (tokenOuts.Count() > 1 || tokenOuts.Sum(x => x.valid.Count()) > 1) //If multiple possible tokens
                {
                    bool sortFixed = ReorderTokens(tokenOuts, contents, ref tokens, ref curToken, ref delimChar); //Split into different values

                    if (!sortFixed)
                    {
                        throw new Exception("Substring '" + contents + "' Could not be sorted");
                    }
                }
                else
                {
                    int contCount = contents.Count();

                    if (delimChar.Count(x => x != null) > 0) //Count up non-delimiter characters
                    {
                        contCount = 0;
                        foreach (char x in contents)
                        {
                            if (!delimChar.Contains(x)) { contCount++; }
                        }
                    }

                    if (tokenOuts.Sum(x => x.valid.Sum(y => y.Count())) < contCount) //Check the parameters fill the entire content 
                    {
                        throw new Exception("No matching token type for supplied parameter");
                    }
                    else
                    {
                        tokens[curToken].type = tokenOuts[0].type;
                    }
                }
            }
            else
            {
                Token tmp = tokens[curToken];
                List<string> tokenConts = new List<string> { tmp.contents };
                condRefs.First(x => x.inType == tmp.type).ApplyFinalConds(ref tokenConts);
                if(tokenConts.Count == 0) { throw new Exception("No matching token type for supplied parameter"); }
                tokens[curToken].contents = tokenConts[0]; //Apply new contents
            }
        }

        private static bool ReorderTokens(List<(TokenType type, List<string> valid)> tokenOuts, string targetString, ref List<Token> tokens, ref int curToken, ref List<char?>delimChars)
        {
            //Remove existing token and split into component parts

            tokens.RemoveAt(curToken);
            curToken--;

            int removedChars = 0;
            string curString = "";

            List<Token> tokenSet = new List<Token>();
            foreach (char x in targetString)
            {
                curString += x;
                (int typeIndex ,int strIndex) entryToRemove = (-1,-1);
                foreach ((TokenType type, List<string> valid) components in tokenOuts)
                {
                    if(components.valid.Count >= 1)
                    {
                        string str = components.valid[0]; //Only check the first in the list for each token type. The items are appended in order therefore they will come in order.
                        if (curString == str)
                        {
                            Token tmpToken = new Token();
                            tmpToken.contents = str;
                            tmpToken.type = components.type;
                            curString = "";
                            entryToRemove = (tokenOuts.IndexOf(components), components.valid.IndexOf(str));
                            removedChars += str.Length;

                            if(tokenSet.Count > 0 && (tmpToken.type == TokenType.Integer || tmpToken.type == TokenType.Decimal)) //Combine negatives when put together
                            {
                                if(tokenSet[tokenSet.Count - 1].type == TokenType.Operation && tokenSet[tokenSet.Count - 1].contents == "-")
                                {
                                    bool isSkip = false; //If the previous value is a deductable then do not connect
                                    if(tokenSet.Count >= 2)
                                    {
                                        //If it is anything that can be minused
                                        if(tokenSet[tokenSet.Count-2].type == TokenType.Integer || tokenSet[tokenSet.Count - 2].type == TokenType.Decimal || tokenSet[tokenSet.Count - 2].type == TokenType.Identifier ||tokenSet[tokenSet.Count - 2].type == TokenType.Bracket)
                                        {
                                            isSkip = true;
                                        }
                                    }

                                    if (!isSkip)
                                    {
                                        tmpToken.contents = "-" + tmpToken.contents;
                                        tokenSet.RemoveAt(tokenSet.Count - 1);
                                    }
                                }
                            }

                            tokenSet.Add(tmpToken);
                            curToken = tokenSet.Count - 1;
                        }
                    }
                }

                if(entryToRemove.typeIndex != -1)
                {
                    tokenOuts[entryToRemove.typeIndex].valid.RemoveAt(entryToRemove.strIndex); //Remove filled entry from set
                }
            }

            tokens.AddRange(tokenSet);
            int charCount = targetString.Length;
            if (delimChars.Count(x=> x!=null) > 0)
            {
                charCount = 0;
                foreach (char x in targetString)
                {
                    if(!delimChars.Contains(x)) { charCount++; }
                }
            }
            return (removedChars >= charCount); //Return true if all characters have been sorted
        }

        //Conditionals
        public static bool IsStringChar(string stringSet) //Whenever a set starts with " 
        {
            return stringSet.StartsWith('\"');
        }
        public static bool IsBoolChars(string stringSet)
        {
            List<char> tfchars = new List<char>(){ 'T', 'F', 't', 'r', 'u', 'e', 'f', 'a', 'l', 's' };
            foreach(char a in stringSet)
            {
                if(!tfchars.Contains(a))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsBool(string stringSet) //Whenever a set starts with " 
        {
            return stringSet == "True" || stringSet == "False" || stringSet == "true" || stringSet == "false";
        }
        public static bool IsStringSet(string stringSet) //strings must end with "
        {
            return stringSet.EndsWith('\"');
        }
        public static bool IsNotKeyword(string stringSet)
        {
            return !IsBool(stringSet); //Append with other keyword checks
        }
        public static bool IsIdentifier(string stringSet) //Checks for identifier pattern matches
        {
            if(char.IsDigit(stringSet[0]) || IsStringChar(stringSet)) { return false; } //First character cannot be a number
            return (stringSet.All(x=> char.IsLetterOrDigit(x) || x == '_'));
        }

        private static bool IsOperatorChar(string stringSet)
        {
            List<string> opChars = new List<string>() { "+", "*", "/", "-", "=", "<", ">", "&", "|", "!" };
            return (opChars.Contains(stringSet) || IsOperated(stringSet));
        }
        private static bool IsOperated(string stringSet) //Checks for all stringsets with
        {
            if (stringSet.Length == 1)
            {
                return (stringSet == "+" || stringSet == "*" || stringSet == "/" || stringSet == "-"
                    || stringSet == "=" || stringSet == "<" || stringSet == ">" || stringSet=="!");
            }
            else
            {
                return (stringSet == "==" ||stringSet == "<=" || stringSet == ">=" || stringSet == "&&" || stringSet == "||" || stringSet == "!=");
            }
        }
        public static bool IsInteger(string stringSet)
        {
            if(stringSet.Count(y=> y == '.') == 0)
            {
                return IsNumeric(stringSet);
            }
            else
            {
                return false;
            }
        }
        private static bool IsNumeric(string stringSet)
        {
            char[] numofminus = stringSet.Where(x => x == '-').ToArray();
            return (stringSet.StartsWith(new string(numofminus)) && stringSet.Length > numofminus.Count() && stringSet.Where(x => x != '-').ToArray().All(y => char.IsDigit(y))); //Check that all minuses are at the start of the string, and that the string does not contain just minuses
        }
        private static bool IsNumericOrDecimal(string stringSet) //This checks for both numbers and ., whereas the final check only checks for complete decimals
        {
            return (IsNumeric(stringSet) || IsDecimal(stringSet));
        }
        private static bool IsntJustDot(string stringSet)
        {
            return stringSet[0] != '.'; //.25 notation isnt allowed. also this prevents just . inputs
        }
        private static bool IsValidIdentifier(string stringSet)
        {
            if(!IsNotKeyword(stringSet)) { return false; }
            return stringSet.Count(x => x == '_') < stringSet.Count();
        }
        public static bool IsDecimal(string stringSet)
        {
            if(stringSet.Count(x => x == '.') == 1)
            {
                string[] outers = stringSet.Split('.').Where(x => x != "").ToArray();
                foreach(string t in outers)
                {
                    if(!IsInteger(t))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
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
