using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using Microsoft.VisualStudio.OLE.Interop;
using MPF = Microsoft.VisualStudio.Package;
using System.ComponentModel.Design;
using System.Diagnostics;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;


using Pie02CS;
using Pie02CS.Rules;

namespace Pie.VisualStudio
{
    class PieScanner
            : IScanner
    {
        private IVsTextBuffer m_buffer;
        string m_source;

        Irony.Parsing.Parser parser;
        PieGrammar grammar;

        string[] reservedWords =
        {
            "namespace",
            "class"
        };

        public PieScanner(IVsTextBuffer buffer)
        {
            m_buffer = buffer;

            grammar = new PieGrammar();
            parser = new Irony.Parsing.Parser(grammar);
            parser.Context.Mode = Irony.Parsing.ParseMode.VsLineScan;
        }

        bool IScanner.ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
        {

            Irony.Parsing.Token token = parser.Scanner.VsReadToken(ref state);

            // !EOL and !EOF
            if (token != null && token.Terminal != grammar.Eof && token.Category != Irony.Parsing.TokenCategory.Error)
            {
                tokenInfo.StartIndex = token.Location.Position;
                tokenInfo.EndIndex = tokenInfo.StartIndex + token.Length - 1;

                Debug.WriteLine("token: '" + token.Text + "'");

                if (token.EditorInfo != null)
                {
                    tokenInfo.Color = (Microsoft.VisualStudio.Package.TokenColor)token.EditorInfo.Color;
                    tokenInfo.Type = (Microsoft.VisualStudio.Package.TokenType)token.EditorInfo.Type;

                    if (token.KeyTerm != null)
                    {
                        tokenInfo.Trigger =
                            (Microsoft.VisualStudio.Package.TokenTriggers)token.KeyTerm.EditorInfo.Triggers;
                    }
                    else
                    {
                        tokenInfo.Trigger =
                            (Microsoft.VisualStudio.Package.TokenTriggers)token.EditorInfo.Triggers;
                    }
                }
                else
                {
                    tokenInfo.Color = TokenColor.Text;
                    tokenInfo.Type = TokenType.Text;
                }



                return true;
            }

            return false;
        }

        void IScanner.SetSource(string source, int offset)
        {
            // Stores line of source to be used by ScanTokenAndProvideInfoAboutIt.
            parser.Scanner.VsSetSource(source, offset);
        }
    }
}
