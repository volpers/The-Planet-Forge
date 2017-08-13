using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Diagnostics;
using Device = SharpDX.Direct3D11.Device;
using DeviceCreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;

namespace The_Planet_Forge_Implementation.src.renderer {

    /// <summary>
    ///  The Core Renderer
    /// </summary>
    public abstract class CoreRenderer : IDisposable {

        private RenderForm window;

        private Texture2D backBuffer;
        private Device device;
        private DeviceContext deviceContext;
        private Factory factory;
        private RenderTargetView renderTargetView;
        private SwapChain swapChain;
        private TimeSpan lastFrameTime;

        public Device Device { get => device; }
        public DeviceContext DeviceContext { get => deviceContext; }
        public RenderTargetView RenderTargetView { get => renderTargetView; }

        public CoreRenderer(string windowName) {
            Init(windowName);
            InitDevice();
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public void Run() {
            Stopwatch clock = new Stopwatch();
            clock.Start();
            lastFrameTime = clock.Elapsed;

            Initialize();
            LoadContent();

            RenderLoop.Run(window, () => {
                TimeSpan timeSinceLastFrame = clock.Elapsed - lastFrameTime;
                lastFrameTime = clock.Elapsed;
                Update(clock.Elapsed, timeSinceLastFrame);
                BeginFrame();
                Draw(clock.Elapsed, timeSinceLastFrame);
                EndFrame();
            });

            UnloadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SwapChainDescription CreateSwapchainDescription() {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowName"></param>
        private void Init(string windowName) {
            window = new RenderForm(windowName);
            window.ClientSize = new System.Drawing.Size(800, 480);
            window.MaximizeBox = false;
            window.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitDevice() {
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, CreateSwapchainDescription(), out device, out swapChain);
            deviceContext = device.ImmediateContext;
            factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(window.Handle, WindowAssociationFlags.IgnoreAll);
            backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(swapChain, 0);
            renderTargetView = new RenderTargetView(device, backBuffer);
            deviceContext.Rasterizer.SetViewport(new Viewport(0, 0, window.ClientSize.Width, window.ClientSize.Height, 0.0f, 1.0f));
            deviceContext.OutputMerger.SetTargets(renderTargetView);
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public virtual void Update(TimeSpan totalGameTime, TimeSpan timeSinceLastFrame) { }
        public virtual void Draw(TimeSpan totalGameTime, TimeSpan timeSinceLastFrame) { }
        public virtual void BeginFrame() { }
        public virtual void EndFrame() {
            swapChain.Present(0, PresentFlags.None);
        }
    }
}