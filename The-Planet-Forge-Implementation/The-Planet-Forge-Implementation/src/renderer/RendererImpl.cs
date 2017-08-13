using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.IO;
using The_Planet_Forge_Implementation.src.renderer.datastructs;

namespace The_Planet_Forge_Implementation.src.renderer {

    internal class RendererImpl : CoreRenderer {
        private PixelShader pixelShader;
        private SharpDX.Direct3D11.Buffer vertexBuffer;
        private VertexShader vertexShader;

        public RendererImpl(string windowName) : base(windowName) {

        }

        public override void Draw(TimeSpan totalGameTime, TimeSpan timeSinceLastFrame) {
            DeviceContext.ClearRenderTargetView(RenderTargetView, Color.CornflowerBlue);

            DeviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, 32, 0));
            DeviceContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

            DeviceContext.Draw(3, 0);

            base.Draw(totalGameTime, timeSinceLastFrame);
        }

        public override void LoadContent() {
            base.LoadContent();           
            ShaderBytecode vertexShaderByteCode = ShaderBytecode.CompileFromFile(Directory.GetCurrentDirectory() + "/shaders/shaders.hlsl", "VShader", "vs_4_0");
            ShaderBytecode pixelShaderByteCode = ShaderBytecode.CompileFromFile(Directory.GetCurrentDirectory() + "/shaders/shaders.hlsl", "PShader", "ps_4_0");

            vertexShader = new VertexShader(Device, vertexShaderByteCode);
            pixelShader = new PixelShader(Device, pixelShaderByteCode);

            DeviceContext.VertexShader.Set(vertexShader);
            DeviceContext.PixelShader.Set(pixelShader);

            InputElement[] elements = new InputElement[] {
                new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32A32_Float, 0, 0),
                new InputElement("COLOR"   , 0, SharpDX.DXGI.Format.R32G32B32A32_Float, 16, 0),
            };

            Vertex[] vertices = createVertices();

            BufferDescription description = new BufferDescription(32 * 3, ResourceUsage.Dynamic, BindFlags.VertexBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
            vertexBuffer = SharpDX.Direct3D11.Buffer.Create(Device, vertices, description);

            DeviceContext.InputAssembler.InputLayout = new InputLayout(Device, ShaderSignature.GetInputSignature(vertexShaderByteCode), elements);


        }

        private Vertex[] createVertices() {
            return new Vertex[] {
                new Vertex(0.0f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f),
                new Vertex(0.5f, -.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f),
                new Vertex(-.5f, -.5f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f),
            };
        }
    }
}