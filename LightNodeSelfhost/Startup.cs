using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Web.Http.ValueProviders;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;
using LightNode.Server;
using LightNode.Formatter;
using LightNode.Swagger;

[assembly: OwinStartup(typeof(LightNodeSelfHost.Startup))]
namespace LightNodeSelfHost
{
    /// <summary>
    /// Owin Startup Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Top Shelf Service Name
        /// </summary>
        public string ServiceName { get; private set; }
        private static IDisposable _application;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="serviceName"></param>
        public Startup(string serviceName)
        {
            this.ServiceName = serviceName;
        }

        /// <summary>
        /// TopShelfからの開始用
        /// </summary>
        public void Start()
        {
            string uri = string.Format("http://localhost:8080/");
            _application = WebApp.Start<Startup>(uri); ;
        }

        /// <summary>
        /// TopShelfからの停止用
        /// </summary>
        public void Stop()
        {
            _application?.Dispose();
        }

        /// <summary>
        /// Owin よみこみ
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // 各種有効化
            app.UseRequestScopeContext();　// Context の取得
            app.UseFileServer(); // FileServer 用の読み込みだよ (Owin からFileアクセス許可を許容するために必要)

            // api 処理
            app.Map("/api", builder =>
            {
                var option = new LightNodeOptions(AcceptVerbs.Get | AcceptVerbs.Post, new JsonNetContentFormatter())
                {
                    ParameterEnumAllowsFieldNameParse = true, // If you want to use enums human readable display on Swagger, set to true
                    ErrorHandlingPolicy = ErrorHandlingPolicy.ReturnInternalServerErrorIncludeErrorDetails,
                    OperationMissingHandlingPolicy = OperationMissingHandlingPolicy.ReturnErrorStatusCodeIncludeErrorDetails,
                };

                // LightNode つかうよ
                builder.UseLightNode(option);
            });

            // page 処理
            app.Map("/pages", builder =>
            {
                // LightNode つかうにゃ
                builder.UseLightNode(new LightNodeOptions(AcceptVerbs.Get, new JsonNetContentFormatter()));
            });

            // Swagger くみこむにゃん
            app.Map("/swagger", builder =>
            {
                var xmlName = "LightNodeSelfHost.xml";
                var xmlPath = AppDomain.CurrentDomain.BaseDirectory + @"bin\" + xmlName;

                builder.UseLightNodeSwagger(new SwaggerOptions("LightNodeSelfHost", "/api")
                {
                    XmlDocumentPath = xmlPath,
                    IsEmitEnumAsString = true // Enumを文字列で並べたいならtrueに
                });
            });
        }
    }
}
