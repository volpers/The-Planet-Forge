using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;

namespace The_Planet_Forge_Implementation.renderer._3dstuff {
    public class D3DFunctions {

        private Device _device;
        private Texture2D _backBuffer;
        private Factory _factory;
        private SwapChain _swapChain;
        private DeviceContext _deviceContext;
        private RenderTargetView _renderTargetView;
        private RenderForm _window;

        private Texture2D _depthBuffer;
        private DepthStencilState _depthStencilState;
        private DepthStencilStateDescription _depthStencilStateDescription;
        private DepthStencilView _depthStencilView;
        private RasterizerState _rasterizerState;

        private Matrix _projectionMatrix;
        private Matrix _worldMatrix;
        private Matrix _orthoMatrix;


        public Device Device => _device;
        public DeviceContext DeviceContext => _deviceContext;
        public RenderTargetView RenderTargetView => _renderTargetView;
        public SwapChain SwapChain => _swapChain;

        public void Init(RenderForm window) {
            _window = window;
            InitGraphics();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            _renderTargetView.Dispose();
            _backBuffer.Dispose();
            _deviceContext.ClearState();
            _deviceContext.Flush();
            _device.Dispose();
            _deviceContext.Dispose();
            _swapChain.Dispose();
            _factory.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SwapChainDescription CreateSwapchainDescription() {
            return new SwapChainDescription {
                BufferCount = 1,
                ModeDescription = new ModeDescription(_window.ClientSize.Width, _window.ClientSize.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = _window.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };
        }
        /// <summary>
        /// 
        ///	// Set up the description of the depth buffer.
        ///	depthBufferDesc.Width = screenWidth;
        ///	depthBufferDesc.Height = screenHeight;
        ///	depthBufferDesc.MipLevels = 1;
        ///	depthBufferDesc.ArraySize = 1;
        ///	depthBufferDesc.Format = DXGI_FORMAT_D24_UNORM_S8_UINT;
        ///	depthBufferDesc.SampleDesc.Count = 1;
        ///	depthBufferDesc.SampleDesc.Quality = 0;
        ///	depthBufferDesc.Usage = D3D11_USAGE_DEFAULT;
        ///	depthBufferDesc.BindFlags = D3D11_BIND_DEPTH_STENCIL;
        ///	depthBufferDesc.CPUAccessFlags = 0;
        ///	depthBufferDesc.MiscFlags = 0;
        /// </summary>
        /// <returns></returns>
        private Texture2DDescription CreateDepthBufferDescription() {
            return new Texture2DDescription {
                Width = _window.Width,
                Height = _window.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription {
                    Count = 1,
                    Quality = 1
                },
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = 0,
                OptionFlags = 0
            };
        }

        /// <summary>
        /// 		depthStencilDesc.DepthEnable = true;
        ///	depthStencilDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
        ///	depthStencilDesc.DepthFunc = D3D11_COMPARISON_LESS;
        ///
        ///	depthStencilDesc.StencilEnable = true;
        ///	depthStencilDesc.StencilReadMask = 0xFF;
        ///	depthStencilDesc.StencilWriteMask = 0xFF;
        ///
        ///	// Stencil operations if pixel is front-facing.
        ///	depthStencilDesc.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
        ///	depthStencilDesc.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_INCR;
        ///	depthStencilDesc.FrontFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
        ///	depthStencilDesc.FrontFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
        ///
        ///	// Stencil operations if pixel is back-facing.
        ///	depthStencilDesc.BackFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
        ///	depthStencilDesc.BackFace.StencilDepthFailOp = D3D11_STENCIL_OP_DECR;
        ///	depthStencilDesc.BackFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
        ///	depthStencilDesc.BackFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
        /// </summary>
        /// <returns></returns>
        private DepthStencilStateDescription CreateDepthStencilStateDescription() {
            return new DepthStencilStateDescription {
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,
                BackFace = new DepthStencilOperationDescription {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    Comparison = Comparison.Always,
                    PassOperation =  StencilOperation.Keep
                },
                FrontFace = new DepthStencilOperationDescription {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    Comparison = Comparison.Always,
                    PassOperation = StencilOperation.Keep
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGraphics() {
            _deviceContext = _device.ImmediateContext;
            _factory = _swapChain.GetParent<Factory>();
            _factory.MakeWindowAssociation(_window.Handle, WindowAssociationFlags.IgnoreAll);
            _backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(_swapChain, 0);
            _renderTargetView = new RenderTargetView(_device, _backBuffer);
            _deviceContext.Rasterizer.SetViewport(new Viewport(0, 0, _window.ClientSize.Width, _window.ClientSize.Height, 0.0f, 1.0f));
            _deviceContext.OutputMerger.SetTargets(RenderTargetView);

            InitDepthBuffer();
        }

        private void InitDepthBuffer() {
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, CreateSwapchainDescription(), out _device, out _swapChain);
            _depthBuffer = new Texture2D(_device, CreateDepthBufferDescription());
            _depthStencilState = new DepthStencilState(_device, CreateDepthStencilStateDescription());
            _deviceContext.OutputMerger.SetDepthStencilState(_depthStencilState);
        }
    }
}



