using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using The_Planet_Forge_Implementation.renderer._3dstuff;
using Device = SharpDX.Direct3D11.Device;
using DeviceCreationFlags = SharpDX.Direct3D11.DeviceCreationFlags;

namespace The_Planet_Forge_Implementation.renderer {

    /// <summary>
    ///  The Core Renderer
    /// </summary>
    public abstract class CoreRenderer : IDisposable {
        
        private TimeSpan _lastFrameTime;
        private D3DFunctions _d3DFunctions;
        private RenderForm _window;

        public DeviceContext DeviceContext => _d3DFunctions.DeviceContext;
        public RenderTargetView RenderTargetView => _d3DFunctions.RenderTargetView;
        public Device Device => _d3DFunctions.Device;

        protected CoreRenderer(string windowName) {
            Init(windowName);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
           _d3DFunctions.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run() {
            var clock = new Stopwatch();
            clock.Start();
            _lastFrameTime = clock.Elapsed;

            Initialize();
            LoadContent();

            RenderLoop.Run(_window, () => {
                var timeSinceLastFrame = clock.Elapsed - _lastFrameTime;
                _lastFrameTime = clock.Elapsed;
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
        /// <param name="windowName"></param>
        private void Init(string windowName) {
            _window = new RenderForm(windowName) {
                ClientSize = new System.Drawing.Size(800, 480),
                MaximizeBox = false,
                FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            };
            _d3DFunctions = new D3DFunctions();
            _d3DFunctions.Init(_window);
        }

        public virtual void Initialize() { }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        public virtual void Update(TimeSpan totalGameTime, TimeSpan timeSinceLastFrame) { }
        public virtual void Draw(TimeSpan totalGameTime, TimeSpan timeSinceLastFrame) { }
        public virtual void BeginFrame() { }
        public virtual void EndFrame() {
            _d3DFunctions.SwapChain.Present(0, PresentFlags.None);
        }
    }
}