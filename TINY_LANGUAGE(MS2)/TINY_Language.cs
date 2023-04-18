using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TINY_LANGUAGE_MS2_
{
    public static class TINY_LANGUAGE_MS2_
    {
        public static Scanner tinylanguage_scanner = new Scanner();
        public static Parser TinyLanguageParser = new Parser();
       // public static List<string> Lexemes = new List<string>();
        public static List<Token> TokenStream = new List<Token>();
        public static Node treeroot;


        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            tinylanguage_scanner.StartScanning(SourceCode);
            //Parser
            TinyLanguageParser.StartParsing(TokenStream);
            treeroot = TinyLanguageParser.root;
        }


        //static void SplitLexemes(string SourceCode)
        //{
        //    string[] Lexemes_arr = SourceCode.Split(' ');
        //    for (int i = 0; i < Lexemes_arr.Length; i++)
        //    {
        //        if (Lexemes_arr[i].Contains("\r\n"))
        //        {
        //            Lexemes_arr[i] = Lexemes_arr[i].Replace("\r\n", string.Empty);
        //        }
        //        Lexemes.Add(Lexemes_arr[i]);
        //    }

        //}


    }
}
