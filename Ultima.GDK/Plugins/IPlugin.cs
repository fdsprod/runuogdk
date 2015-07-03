using System;
using System.Collections.Generic;
using System.Text;

namespace Ultima.GDK.Plugins
{
    public interface IPlugin
    {
        string PluginName { get; }
        string AuthorName { get; }
        string Description { get; }
        Version Version { get; }
        void Initialize();
    }
}
