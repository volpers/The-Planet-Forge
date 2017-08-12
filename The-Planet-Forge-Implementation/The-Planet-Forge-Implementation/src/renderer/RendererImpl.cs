using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Planet_Forge_Implementation.src.renderer {
    class RendererImpl : CoreRenderer {

        public RendererImpl(string windowName) : base(windowName) {
            
        }

        public override void Draw(TimeSpan totalGameTime, TimeSpan timeSinceLastFrame) {
            DeviceContext.ClearRenderTargetView(RenderTargetView, Color.CornflowerBlue);
            base.Draw(totalGameTime, timeSinceLastFrame);
            
        }
    }
}
