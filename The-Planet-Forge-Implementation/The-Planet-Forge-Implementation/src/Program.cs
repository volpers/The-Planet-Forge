﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;
using System.Windows.Forms;
using The_Planet_Forge_Implementation.src.renderer;

namespace The_Planet_Forge_Implementation.src
{
    internal static class Program
    {
        private static CoreRenderer renderer;

        [STAThread]
        private static void Main() {
            RenderForm window = new RenderForm("The Planet Forge");
            renderer = new CoreRenderer(window);
            renderer.loop();
        }
    }
}
