using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace The_Planet_Forge_Implementation.src.renderer.datastructs {
    public struct Vertex {
        public Vector4 position;
        public Color4 color;

        public Vertex(float x, float y, float z, float r, float g, float b, float a) {
            position = new Vector4(x, y, z, 1.0f);
            color = new Color4(r, g, b, a);
        }
    }
}
