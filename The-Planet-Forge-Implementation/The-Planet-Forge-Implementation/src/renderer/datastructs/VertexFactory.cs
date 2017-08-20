using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Planet_Forge_Implementation.src.renderer.datastructs {
    public class VertexFactory {

        public static Vertex[] CreateFlatTriangle() {
            return new Vertex[] {
                new Vertex(0.0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(0.5f, -.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f),
                new Vertex(-.5f, -.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f),
            };
           
        }

        public static Vertex[] Create3DCube(float size) {
            float side = size / 2f;
            return new Vertex[] {
                new Vertex(-side, -side, -side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(side, -side, -side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(-side, side, -side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(side, side, -side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(-side, -side, side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(side, -side, side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(-side, side, side, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(side, side, side, 1.0f, 0.0f, 0.0f, 1.0f),
            };
        }

    }
}
