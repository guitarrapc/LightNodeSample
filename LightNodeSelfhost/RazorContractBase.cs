using System;
using System.IO;
using LightNode.Formatter;
using LightNode.Server;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace LightNodeSelfHost
{
    public abstract class RazorContractBase : LightNode.Server.LightNodeContract
    {
        static readonly IRazorEngineService razor = CreateRazorEngineService();

        static IRazorEngineService CreateRazorEngineService()
        {
            var config = new TemplateServiceConfiguration();
            config.DisableTempFileLocking = true;
            config.CachingProvider = new DefaultCachingProvider(_ => { });
            config.TemplateManager = new DelegateTemplateManager(name =>
            {
                // import from "Views" directory
                var viewPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Views", name);
                return System.IO.File.ReadAllText(viewPath);
            });
            return RazorEngineService.Create(config);
        }

        protected string View(string viewName)
        {
            return View(viewName, new object());
        }

        protected string View(string viewName, object model)
        {
            var type = model.GetType();
            if (razor.IsTemplateCached(viewName, type))
            {
                return razor.Run(viewName, type, model);
            }
            else
            {
                return razor.RunCompile(viewName, type, model);
            }
        }
    }

    public class Html : LightNode.Server.OperationOptionAttribute
    {
        public Html(AcceptVerbs acceptVerbs = AcceptVerbs.Get | AcceptVerbs.Post)
            : base(acceptVerbs, typeof(HtmlContentFormatterFactory))
        {

        }
    }
}