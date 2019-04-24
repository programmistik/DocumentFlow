using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace DocumentFlow.Services.WebBrowserServices
{
    public static class DialogBox
    {
        public static Color Pick()
        {
            Color col = new Color();

            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.FullOpen = true;
                DialogResult result = colorDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    col.A = colorDialog.Color.A;
                    col.B = colorDialog.Color.B;
                    col.G = colorDialog.Color.G;
                    col.R = colorDialog.Color.R;
                }
            }
            return col;
        }


        public static string SelectFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                DialogResult result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
                return "";
            }

        }


        public static string SaveFile()
        {
            using (SaveFileDialog SaveFileDialog = new SaveFileDialog())
            {

                SaveFileDialog.InitialDirectory = @"C:\";
                SaveFileDialog.Filter = "txt files (*.htm)|*.htm|All files (*.html)|*.html";
                SaveFileDialog.FilterIndex = 2;
                SaveFileDialog.RestoreDirectory = true;

                if (SaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return SaveFileDialog.FileName;
                }
                return "";
            }

        }


    }
}
