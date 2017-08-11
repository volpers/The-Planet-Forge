using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Planet_Forge_Implementation.src
{
    internal static class Program
    {
        [STAThread]
        private static void Main() {

            MainFrame frame = new MainFrame("The Planet Forge");
            Application.EnableVisualStyles();           
            Application.Run(frame);

        }                                
    }
}
