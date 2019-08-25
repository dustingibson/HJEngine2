using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.editor
{
    class MapEditor
    {
        public MapEditor()
        {
            //Launch Companion
            Process process = new Process();
            process.StartInfo.FileName = "C:/Users/Dustin/source/repos/HJEngine/HJEngine/res/editor/HJCompanion.exe";
            process.Start();
        }
    }
}
