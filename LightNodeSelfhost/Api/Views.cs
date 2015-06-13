using LightNode.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightNodeSelfHost.Pages
{
    /// <summary>
    /// View を返します。
    /// </summary>
    
    class Views : RazorContractBase
    {
        [IgnoreClientGenerate]
        [Html]
        public string Index()
        {
            return this.View("Index.cshtml");
        }
    }
}
