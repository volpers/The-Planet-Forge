using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Windows;
using System.Windows.Forms;

namespace The_Planet_Forge_Implementation.src
{
    internal static class Program
    {
        [STAThread]
        private static void Main() {
            RenderForm window = new RenderForm("The Planet Forge");
            Application.Run(window);
        }                                
    }
}
