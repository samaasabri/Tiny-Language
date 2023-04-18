using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public enum Token_Type
{
    T_int, T_float, T_string, T_read, T_write, T_repeat, T_until, T_if, T_elseif,T_end,
    T_else, T_then, T_return, T_endl, Comment_Statement, Identifiers, LParanthesis, RParanthesis
    , LBracket, RBracket, PlusOp, MinusOp, MultiplyOp, DivideOp, AssignmentOP, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, AndOp, OrOp, number, comma, semeicolon, Strings,T_main,

}
namespace TINY_LANGUAGE_MS2_
{
    public class Token
    {
        public string lexeme;
        public Token_Type token_type;
    }
    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Type> Reserved_Keywords = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> Arithemtic_Operators = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> Assignment_Operators = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> Boolean_Operators = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> Condition_Operators = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> Brackets_Operators = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> commas = new Dictionary<string, Token_Type>();
        Dictionary<string, Token_Type> semicolons = new Dictionary<string, Token_Type>();

        public Scanner()
        {

            Reserved_Keywords.Add("if", Token_Type.T_if);
            Reserved_Keywords.Add("elseif", Token_Type.T_elseif);
            Reserved_Keywords.Add("else", Token_Type.T_else);
            Reserved_Keywords.Add("int", Token_Type.T_int);
            Reserved_Keywords.Add("main", Token_Type.T_main);
            Reserved_Keywords.Add("read", Token_Type.T_read);
            Reserved_Keywords.Add("write", Token_Type.T_write);
            Reserved_Keywords.Add("float", Token_Type.T_float);
            Reserved_Keywords.Add("string", Token_Type.T_string);
            Reserved_Keywords.Add("repeat", Token_Type.T_repeat);
            Reserved_Keywords.Add("until", Token_Type.T_until);
            Reserved_Keywords.Add("return", Token_Type.T_return);
            Reserved_Keywords.Add("endl", Token_Type.T_endl);
            Reserved_Keywords.Add("then", Token_Type.T_then);
            Reserved_Keywords.Add("end", Token_Type.T_end);

            Assignment_Operators.Add(":=", Token_Type.AssignmentOP);

            Arithemtic_Operators.Add("+", Token_Type.PlusOp);
            Arithemtic_Operators.Add("-", Token_Type.MinusOp);
            Arithemtic_Operators.Add("/", Token_Type.DivideOp);
            Arithemtic_Operators.Add("*", Token_Type.MultiplyOp);

            Boolean_Operators.Add("&&", Token_Type.AndOp);
            Boolean_Operators.Add("||", Token_Type.OrOp);

            Condition_Operators.Add("<", Token_Type.LessThanOp);
            Condition_Operators.Add(">", Token_Type.GreaterThanOp);
            Condition_Operators.Add("<>", Token_Type.NotEqualOp);
            Condition_Operators.Add("=", Token_Type.EqualOp);

            Brackets_Operators.Add("(", Token_Type.LBracket);
            Brackets_Operators.Add(")", Token_Type.RBracket);
            Brackets_Operators.Add("{", Token_Type.LParanthesis);
            Brackets_Operators.Add("}", Token_Type.RParanthesis);

            commas.Add(",", Token_Type.comma);

            semicolons.Add(";", Token_Type.semeicolon);


        }
        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();
                int cnt_dots = 0;

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                //identifiers
                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                    j++;
                    //CurrentChar = SourceCode[j];
                    while (true)
                    {
                        if (j == SourceCode.Length)
                        {
                            break;
                        }
                        CurrentChar = SourceCode[j];
                        if ((CurrentChar >= 'A' && CurrentChar <= 'z') || (CurrentChar >= '0' && CurrentChar <= '9'))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }

                        else
                        {
                            break;
                        }
                        j++;
                        if (j == SourceCode.Length)
                        {
                            break;
                        }

                    }
                    i = j - 1;

                    FindTokenClass(CurrentLexeme);
                }
                //number
                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    j++;
                    //CurrentChar = SourceCode[j];

                    while (true)
                    {
                        if (j == SourceCode.Length)
                        {
                            break;
                        }

                        CurrentChar = SourceCode[j];

                        if ((CurrentChar >= '0' && CurrentChar <= '9') || ((SourceCode[j] == '.') && (cnt_dots == 0)))
                        {
                            if (SourceCode[j] == '.')
                            {
                                cnt_dots++;
                            }

                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else if (CurrentChar >= 'A' && CurrentChar <= 'z')
                        {
                            CurrentLexeme += CurrentChar.ToString();

                        }

                        else
                        {
                            break;
                        }
                        j++;
                        if (j == SourceCode.Length)
                        {
                            break;
                        }

                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                // invalid num
                else if (SourceCode[j] == '.')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                //invalid charachters 
                // @
                else if (SourceCode[j] == '@')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }
                //#
                else if (SourceCode[j] == '#')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                //^
                else if (SourceCode[j] == '^')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                // %
                else if (SourceCode[j] == '%')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                // $
                else if (SourceCode[j] == '$')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                //!
                else if (SourceCode[j] == '!')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                //_
                else if (SourceCode[j] == '_')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        if (j == SourceCode.Length || CurrentChar == ' ' || CurrentChar == '\r')
                        {
                            break;
                        }
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                }

                //comment_statement
                else if (SourceCode[j] == '/' && SourceCode[j + 1] == '*')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        if (CurrentLexeme[CurrentLexeme.Length - 1] == '/' && CurrentLexeme[CurrentLexeme.Length - 2] == '*')
                        {
                            break;
                        }
                        j++;
                        if (j == SourceCode.Length || CurrentChar == '\r')
                        {
                            if (!(CurrentLexeme[CurrentLexeme.Length - 1] == '/' && CurrentLexeme[CurrentLexeme.Length - 2] == '*'))
                            {
                                FindTokenClass(CurrentLexeme);
                            }
                            break;
                        }
                    }
                    i = j;
                }

                //string
                else if (SourceCode[j] == '\"')
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        break;
                    }


                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        if (SourceCode[j] != '\"')
                        {
                            CurrentLexeme += CurrentChar.ToString();

                        }
                        else
                        {
                            CurrentLexeme += CurrentChar.ToString();

                            j++;
                            FindTokenClass(CurrentLexeme);
                            break;
                        }
                        j++;
                        if (j == SourceCode.Length || CurrentChar == '\r')
                        {
                            if (CurrentLexeme[CurrentLexeme.Length - 1] != '\"')
                            {
                                FindTokenClass(CurrentLexeme);
                            }
                            break;
                        }
                    }
                    i = j - 1;

                }
                //notequale (<>) operation

                else if (CurrentChar == '<' )
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }
                    CurrentChar = SourceCode[j];

                    if (CurrentChar == '>')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                   
                    else
                    {
                        FindTokenClass(CurrentLexeme);
                        i = j-1;
                    }
                }

                //|| operators 
                else if (CurrentChar == '|')
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }
                    CurrentChar = SourceCode[j];

                    if (CurrentChar == '|')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j;

                }

                //&& operators
                else if (CurrentChar == '&')
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }
                    CurrentChar = SourceCode[j];

                    if (CurrentChar == '&')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j;

                }

                //:= operators
                else if (CurrentChar == ':')
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }
                    CurrentChar = SourceCode[j];

                    if (CurrentChar == '=')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j;

                }
                //Bouns
                else if (CurrentChar == '>')
                {
                    j++;
                    if (j == SourceCode.Length)
                    {
                        FindTokenClass(CurrentLexeme);
                        break;
                    }
                    CurrentChar = SourceCode[j];

                    if (CurrentChar == '=')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j-1;

                }
                else if (SourceCode[j] == '*' && SourceCode[j + 1] == '*')
                {
                    j++;
                    while (true)
                    {
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                        if (CurrentLexeme[CurrentLexeme.Length - 1] == '*' && CurrentLexeme[CurrentLexeme.Length - 2] == '*')
                        {
                            FindTokenClass(CurrentLexeme);
                            break;
                        }
                        j++;
                    }
                    i = j;
                }
                // if it is not a number,identfier or comment_statement, it will go to FindTokenClass to detect the lexeme tybe  
                else
                {
                    FindTokenClass(CurrentLexeme);
                }
            }
            TINY_LANGUAGE_MS2_.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Type TC;
            Token Tok = new Token();
            Tok.lexeme = Lex;
            //Is it a reserved word?
            if (Reserved_Keywords.ContainsKey(Lex))
            {
                Tok.token_type = Reserved_Keywords[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is arithmatic op 
            else if (Arithemtic_Operators.ContainsKey(Lex))
            {
                Tok.token_type = Arithemtic_Operators[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            // is assigment op
            else if (Assignment_Operators.ContainsKey(Lex))
            {
                Tok.token_type = Assignment_Operators[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is bool op
            else if (Boolean_Operators.ContainsKey(Lex))
            {
                Tok.token_type = Boolean_Operators[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is condition op
            else if (Condition_Operators.ContainsKey(Lex))
            {
                Tok.token_type = Condition_Operators[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is bracket 
            else if (Brackets_Operators.ContainsKey(Lex))
            {
                Tok.token_type = Brackets_Operators[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //Is it an identifier?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Type.Identifiers;
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is number
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Type.number;
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is comma
            else if (commas.ContainsKey(Lex))
            {
                Tok.token_type = commas[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            //is semicolon
            else if (semicolons.ContainsKey(Lex))
            {
                Tok.token_type = semicolons[Lex];
                Tok.lexeme = Lex;
                Tokens.Add(Tok);
            }
            else if (IsString(Lex))
            {
                Tok.token_type = Token_Type.Strings;
                Tok.lexeme = Lex;
                Tokens.Add(Tok);

            }
            else
            {
                ERRORS.Error_List.Add(Lex);
            }
        }
        bool isIdentifier(string lex)
        {
            bool isValid = true;
            var rx = new Regex("^[a-zA-Z][a-zA-Z0-9]*", RegexOptions.Compiled);
            if (rx.IsMatch(lex)) isValid = true;
            else isValid = false;
            return isValid;
        }
        bool isNumber(string lex)
        {
            bool isValid = true;
            var rx = new Regex(@"^[0-9]+(\.[0-9]+)?$");
            if (rx.IsMatch(lex)) isValid = true;
            else isValid = false;
            return isValid;
        }
        bool IsString(string lex)
        {
            bool isValid = true;
            var rx = new Regex("\".*\"");
            if (rx.IsMatch(lex)) isValid = true;
            else isValid = false;
            return isValid;
        }


    }
}