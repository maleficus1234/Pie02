using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;

namespace Pie.VisualStudio
{
    class PieLanguageService
            : LanguageService
    {
        private LanguagePreferences m_preferences;


        public override LanguagePreferences GetLanguagePreferences()
        {
            if (m_preferences == null)
            {
                m_preferences = new LanguagePreferences(this.Site,
                                                        typeof(PieLanguageService).GUID,
                                                        this.Name);
                m_preferences.Init();
            }
            return m_preferences;
        }

        private PieScanner m_scanner;

        public override IScanner GetScanner(IVsTextLines buffer)
        {
            if (m_scanner == null)
            {
                m_scanner = new PieScanner(buffer);
            }
            return m_scanner;
        }

        public override void OnIdle(bool periodic)
        {
            base.OnIdle(periodic);
            Console.WriteLine(periodic);
        }

        public override AuthoringScope ParseSource(ParseRequest req)
        {
            return new PieAuthoringScope();
        }

        public override string Name
        {
            get { return "Pie Language"; }
        }

        public override string GetFormatFilterList()
        {
            return "*.pie";
        }
    }
}
