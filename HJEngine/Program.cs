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
            Game game = new Game(1600, 900);
            using (Display display = new Display(game))
            {
                display.Run(60);
            }
        }
    }
}
 