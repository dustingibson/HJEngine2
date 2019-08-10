using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace HJEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Display display = new Display(1600,900,"Blah"))
            {
                display.Run(60);
            }
        }
    }
}
 