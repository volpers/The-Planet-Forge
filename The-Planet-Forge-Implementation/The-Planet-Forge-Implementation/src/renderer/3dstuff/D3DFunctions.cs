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
        private DepthStencilView _depthStencilView;
        private RasterizerState _rasterizerState;

        private Matrix _projectionMatrix;
        private Matrix _worldMatrix;
        private Matrix _orthoMatrix;

        private float _screenNear = 0f;
        private float _screenFar = 0f;


        public Device Device => _device;
        public DeviceContext DeviceContext => _deviceContext;
        public RenderTargetView RenderTargetView => _renderTargetView;
        public SwapChain SwapChain => _swapChain;

        public DepthStencilView DepthStencilView => _depthStencilView;

        public Matrix ProjectionMatrix => _projectionMatrix;
        public Matrix WorldMatrix => _worldMatrix;
        public Matrix OrthoMatrix => _orthoMatrix;

        public void Init(RenderForm window, float screenNear, float screenFar) {
            _window = window;
            _screenNear = screenNear;
            _screenFar = screenFar;
            InitGraphics();
            InitProjectionMatrices();
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
        private void InitGraphics() {
            _deviceContext = _device.ImmediateContext;
            _factory = _swapChain.GetParent<Factory>();
            _factory.MakeWindowAssociation(_window.Handle, WindowAssociationFlags.IgnoreAll);
            _backBuffer = SharpDX.Direct3D11.Resource.FromSwapChain<Texture2D>(_swapChain, 0);
            _renderTargetView = new RenderTargetView(_device, _backBuffer);

            InitDepthBuffer();
            InitRasterizerState();
            _deviceContext.OutputMerger.SetRenderTargets(_depthStencilView, _renderTargetView);
            _deviceContext.Rasterizer.SetViewport(new Viewport(0, 0, _window.ClientSize.Width, _window.ClientSize.Height, 0.0f, 1.0f));
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitProjectionMatrices() {
            float fieldOfView = (float)MathUtil.Pi / 4.0f;
            float aspectRatio = (float) _window.Width / (float) _window.Height;
            _projectionMatrix = Matrix.PerspectiveFovLH(fieldOfView, aspectRatio, _screenNear, _screenFar);
            _worldMatrix = Matrix.Identity;
            _orthoMatrix = Matrix.OrthoLH(_window.Width, _window.Height, _screenNear, _screenFar);
        }

        /// <summary>
        /// depthStencilViewDesc.Format = DXGI_FORMAT_D24_UNORM_S8_UINT;
        ///	depthStencilViewDesc.ViewDimension = D3D11_DSV_DIMENSION_TEXTURE2D;
        ///	depthStencilViewDesc.Texture2D.MipSlice = 0;
        /// </summary>
        private void InitDepthBuffer() {
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, CreateSwapchainDescription(), out _device, out _swapChain);
            _depthBuffer = new Texture2D(_device, CreateDepthBufferDescription());
            _depthStencilState = new DepthStencilState(_device, CreateDepthStencilStateDescription());
            _deviceContext.OutputMerger.SetDepthStencilState(_depthStencilState);

            var depthStencilViewDescription = new DepthStencilViewDescription {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = DepthStencilViewDimension.Texture2D,
                Texture2D = new DepthStencilViewDescription.Texture2DResource {
                    MipSlice = 0
                }
            };
            _depthStencilView = new DepthStencilView(_device, _depthBuffer, depthStencilViewDescription);
        }

        private void InitRasterizerState() {
            _rasterizerState = new RasterizerState(_device, CreateRasterizerStateDescription());
            _deviceContext.Rasterizer.State = _rasterizerState;
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
                    PassOperation = StencilOperation.Keep
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
        /// This will give us control over how polygons are rendered. We can do things like make our scenes render in wireframe mode or have DirectX draw both the front and back faces of polygons	
        /// </summary>
        /// <returns></returns>
        private RasterizerStateDescription CreateRasterizerStateDescription() {
            return new RasterizerStateDescription {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
        }
    }
}



