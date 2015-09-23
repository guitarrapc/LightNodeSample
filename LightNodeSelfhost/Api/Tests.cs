using LightNode.Server;
using LightNodeSelfHost.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNodeSelfHost.Api
{
    /// <summary>
    /// Api のテストです
    /// </summary>
    public class Tests : LightNodeContract
    {
        /// <summary>
        /// Hellow World を返します。
        /// </summary>
        /// <returns></returns>
        [Get, Post]
        public string ApiTest()
        {
            return "Hello World!";
        }

        /// <summary>
        /// 実装されているAPIの一覧を返します。
        /// </summary>
        /// <remarks>APIのUriを返します。</remarks>
        /// <returns></returns>
        [Post]
        public string[] ListApi()
        {
            var apis = LightNodeServerMiddleware.GetRegisteredHandlersInfo();
            var key = apis.Select(x => x.Key).First();
            return apis[key].SelectMany(x => x.RegisteredHandlers).Select(x => x.Key).ToArray();
        }

        /// <summary>
        /// Hoge と Fuga の Echo を返します
        /// </summary>
        /// <returns></returns>
        [Post]
        public TestResponse Echo(string x, string y)
        {
            return new TestResponse
            {
                Hoge = x,
                Fuga =y,
            };
        }
    }
}
