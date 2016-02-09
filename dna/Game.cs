/*
References required:
SharpDX
SharpDX.D3DCompiler
SharpDX.Desktop (not specified in tutorials, but contains .Windows and RenderForm class!!!)
SharpDX.Mathematics (not specified in tutorials, but required for SharpDX.Color!!!!)
SharpDX.Direct3D11
SharpDX.DXGI
*/


using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3D11 = SharpDX.Direct3D11;



namespace dna
{
    public class Game : IDisposable
    {
        private RenderForm renderForm;

        private const int Width = 1280;
        private const int Height = 720;

        private D3D11.Device d3dDevice;
        private D3D11.DeviceContext d3dDeviceContext;
        private SwapChain swapChain;

        private D3D11.RenderTargetView renderTargetView;



        public Game()
        {
            renderForm = new RenderForm("My first SharpDX app");
            renderForm.ClientSize = new Size(Width, Height);
            renderForm.AllowUserResizing = false;

            InitializeDeviceResources();
        }

        public void Run()
        {
            RenderLoop.Run(renderForm, RenderCallback);
        }

        private void RenderCallback()
        {
            Draw();
        }

        public void Dispose()
        {
            renderTargetView.Dispose();
            swapChain.Dispose();
            d3dDevice.Dispose();
            d3dDeviceContext.Dispose();
            renderForm.Dispose();
        }

        private void InitializeDeviceResources()
        {   
            ModeDescription backBufferDesc = new ModeDescription(Width, Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);

            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = renderForm.Handle,
                IsWindowed = true
            };

            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out d3dDevice, out swapChain);
            d3dDeviceContext = d3dDevice.ImmediateContext;

            using (D3D11.Texture2D backBuffer = swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                renderTargetView = new D3D11.RenderTargetView(d3dDevice, backBuffer); //d3d11Device
            }

            d3dDeviceContext.OutputMerger.SetRenderTargets(renderTargetView); //d3d11DeviceContext
        }

        private void Draw()
        {
            d3dDeviceContext.ClearRenderTargetView(renderTargetView, new SharpDX.Color(32, 103, 178)); //new SharpDX.Color(32, 103, 178));
            swapChain.Present(1, PresentFlags.None);
        }
    }
}
