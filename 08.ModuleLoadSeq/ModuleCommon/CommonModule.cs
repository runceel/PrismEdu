using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleCommon
{
    public class CommonModule : IModule
    {
        public void Initialize()
        {
            Debug.WriteLine("CommonModule initialized.");
        }
    }
}
