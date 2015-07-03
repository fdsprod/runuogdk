using System;
using System.Collections.Generic;
using System.Text;

namespace Ultima.GDK.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        public abstract string PluginName { get; }
        public abstract string AuthorName { get; }
        public abstract string Description { get; }
        public abstract Version Version { get; }

        public abstract void Initialize();
    }
}
