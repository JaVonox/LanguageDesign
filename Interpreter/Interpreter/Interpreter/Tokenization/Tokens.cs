using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Tokens
{
    public enum TokenType
    {
        Null,
        End,
        Identifier,
        Integer,
        MathOperation,
        Bracket,
    }
    public class Token
    {
        public TokenType type;
        public string contents = "";
    }

    public class TokenHandler
    {
        public void CreateTokens(string data, ref List<Token> tokens)
        {
            try
            {
                int tokenID = tokens.Count;

                for (int i = 0; i < data.Length; i++)
                {
                    if (tokens.Count <= tokenID)
                    {
                        tokens.Add(new Token());
                    }

                    SortToken(ref tokens, data[i], ref tokenID);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception while producing tokens");
            }
        }
        public void SortToken(ref List<Token> tokens, char newChar, ref int tokenID)
        {
            if (newChar == ' ' || newChar == '\t') //If space or tab
            {
                if(tokens[tokenID].contents != "") //If this is not a blank set, then increment the token counter
                {
                    tokenID++; //Increment tokenID on end of token
                }
            }
            else if (newChar == ';') //End line character
            {
                if (tokens[tokenID].contents != "") //If empty token
                {
                    tokens[tokenID].contents = ";";
                    tokens[tokenID].type = TokenType.End;
                    tokenID++;
                }
                else //If token selected has contents
                {
                    Token endToken = new Token();
                    endToken.contents = ";";
                    endToken.type = TokenType.End;
                    tokens.Add(endToken);
                    tokenID+=2; //Increment for both generated tokens
                }
            }
            else
            {
                tokens[tokenID].contents += newChar;
                tokens[tokenID].type = TokenType.Identifier;
            }
        }
    }
}
