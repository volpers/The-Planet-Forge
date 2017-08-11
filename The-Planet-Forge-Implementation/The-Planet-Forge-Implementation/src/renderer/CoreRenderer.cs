using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using DeviceCreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct3D;
using SharpDX;

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
        private Device device;
        private DeviceContext deviceContext;
        private SwapChain swapChain;
        private Factory factory;

        private RenderTargetView renderTargetView;
        private Texture2D backBuffer;

        public CoreRenderer(RenderForm window) {
            Init(window);
        }
                
        private void Init(RenderForm window) {
            this.window = window;           
            InitDevice();
        }

        public void loop () {
            RenderLoop.Run(window, () => {
                Update();
            });
        }

        public void Dispose() {
            renderTargetView.Dispose();
            backBuffer.Dispose();
            deviceContext.ClearState();
            deviceContext.Flush();
            device.Dispose();
            deviceContext.Dispose();
            swapChain.Dispose();
            factory.Dispose();
        }

        private void Update() {
            deviceContext.ClearRenderTargetView(renderTargetView, Color.CornflowerBlue);
            swapChain.Present(0, PresentFlags.None);
        }

        private void InitDevice() {
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, createSwapchainDescription(), out device, out swapChain);
            deviceContext = device.ImmediateContext;
            factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(window.Handle, WindowAssociationFlags.IgnoreAll);
            backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(swapChain, 0);
            renderTargetView = new RenderTargetView(device, backBuffer);
            deviceContext.Rasterizer.SetViewport(new Viewport(0, 0, window.ClientSize.Width, window.ClientSize.Height, 0.0f, 1.0f));
            deviceContext.OutputMerger.SetTargets(renderTargetView);
        }

        private SwapChainDescription createSwapchainDescription() {
            return new SwapChainDescription {
                BufferCount = 1,
                ModeDescription = new ModeDescription(window.ClientSize.Width, window.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = window.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
        }      
    }
}
