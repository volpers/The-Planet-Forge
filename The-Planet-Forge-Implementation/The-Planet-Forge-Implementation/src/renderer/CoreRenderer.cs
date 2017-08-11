using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Planet_Forge_Implementation.src.renderer
{
    /// <summary>
    ///  The Core Renderer
    /// </summary>
    public class CoreRenderer
    {
        private RenderForm window;

        public CoreRenderer(RenderForm window) {
            Init(window);
        }
                
        private void Init(RenderForm window) {
            this.window = window;
        }
    }
}
