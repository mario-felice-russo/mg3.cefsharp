using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace Handlers
{
    public class SchemeHandlerFactory : ISchemeHandlerFactory
    {
        public SchemeHandlerFactory(List<string> schemeList)
        {
            SchemeList = schemeList;
        }

        public List<string> SchemeList { get; private set; }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            IResourceHandler result = new ResourceHandler();

            if (SchemeList.Contains(schemeName))
            {
                var uri = new Uri(request.Url);
                var error404 = Environment.CurrentDirectory + "/udf/errors/" + "error404.html";
                var file = Environment.CurrentDirectory + "/" + uri.Authority + uri.AbsolutePath.TrimEnd('/');

                var fileExtension = Path.GetExtension(file);
                var mimeType = ResourceHandler.GetMimeType(fileExtension);

                if (File.Exists(file))
                {
                    result = ResourceHandler.FromFilePath(file, mimeType);
                }
                else
                {
                    result = ResourceHandler.FromFilePath(error404, mimeType);
                }
            }

            return result;
        }
    }
}
