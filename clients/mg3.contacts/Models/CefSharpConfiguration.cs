using System.Collections.Generic;

namespace Models
{
    public class CefSharpConfiguration
    {
        public CefSharpConfiguration()
        {
            Address = "about:blank";
            DisplayPdf = true;
            SchemeList = new List<string>();
            ShowDevtools = true;
        }

        public string Address { get; set; }
        public bool DisplayPdf { get; set; }
        public string SchemeName { get; set; }
        public bool ShowDevtools { get; set; }

        public List<string> SchemeList { get; set; }
    }
}
