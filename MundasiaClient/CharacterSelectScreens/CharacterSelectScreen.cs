using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mundasia.Interface
{
   [System.ComponentModel.DesignerCategory("")] 
    public class CharacterSelectScreen: Panel
    {
        public CharacterSelectScreen() { }

        public static CharacterSelectScreen _panel;

        public static void Set()
        {
            if(_panel == null)
            {
                _panel = new CharacterSelectScreen();
            }


        }

    }
}
