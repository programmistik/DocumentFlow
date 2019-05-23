using DocumentFlow.CustomUserControls;
using DocumentFlow.ModalWindows;
using DocumentFlow.Models;
using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;

namespace DocumentFlow.Services.WebBrowserServices
{
    public static class Gui
    {
        public static WPFWebBrowser webBrowser;
        public static HtmlEditor htmlEditor;


        public static List<string> RibbonComboboxFontSizeInitialisation()
        {
            List<string> items = new List<string>();

            for (int x = 1; x <= 7; x++)
            {
                items.Add(x.ToString());
            }
            return items;
        }

        public static void SettingsFontColor()
        {
            webBrowser.doc = webBrowser.webBrowser.Document as HTMLDocument;
            if (webBrowser.doc != null)
            {
                System.Windows.Media.Color col = DialogBox.Pick();
                string colorstr = string.Format("#{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
                webBrowser.doc.execCommand("ForeColor", false, colorstr);
            }
        }

        //public static void SettingsBackColor()
        //{
        //    webBrowser.doc = webBrowser.webBrowser.Document as HTMLDocument;
        //    if (webBrowser.doc != null)
        //    {
        //        System.Windows.Media.Color col = DialogBox.Pick();
        //        string colorstr = string.Format("#{0:X2}{1:X2}{2:X2}", col.R, col.G, col.B);
        //        webBrowser.doc.body.style.background = colorstr;
        //    }
        //}

        public static void SettingsAddLink()
        {
            using (LinkWin link = new LinkWin(webBrowser.doc))
            {
                link.ShowDialog();
            }
        }

        public static void SettingsAddImage()
        {
            //using (ImageWin image = new ImageWin(webBrowser.doc))
            //{
            //    image.ShowDialog();
            //}
            string selectedPath;
            var entity = new AppDbContext();            
            var constants = entity.Constants.FirstOrDefault();
            var imgDir = constants.FilePath;

            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.png.*)|*.png|All files (*.gif)|*.gif|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedPath = openFileDialog.FileName;
                }
                else
                    return;
            }
            // copy file to FilePath directory
            var newPath = imgDir + "//" +System.IO.Path.GetFileName(selectedPath);
            var ext = Path.GetExtension(newPath);
            while (File.Exists(newPath))
            {
                newPath = imgDir + "//" + Path.GetFileNameWithoutExtension(newPath) + "1" + ext;
            }
            File.Copy(selectedPath, newPath);

            dynamic r = webBrowser.doc.selection.createRange();
            r.pasteHTML(string.Format(@"<img alt=""{1}"" src=""{0}"">", newPath, "image is here"));

        }

        public static void RibbonButtonSave()
        {
            dynamic doc = webBrowser.doc;
            var htmlText = doc.documentElement.InnerHtml;
            string path = DialogBox.SaveFile();
            if (path != "")
                File.WriteAllText(DialogBox.SaveFile(), htmlText);
        }

        public static void RibbonComboboxFonts(ComboBox RibbonComboboxFonts)
        {
            var doc = webBrowser.webBrowser.Document as HTMLDocument;
            if (doc != null)
            {
                doc.execCommand("FontName", false, RibbonComboboxFonts.SelectedItem.ToString());
            }
        }

        //public static Task<bool> RibbonComboboxFontsAsync(ComboBox RibbonComboboxFonts)
        //{
        //    return Task.Run(() =>
        //    {
        //        var doc = webBrowser.webBrowser.Document as HTMLDocument;
        //        if (doc != null)
        //        {
        //            return doc.execCommand("FontName", false, RibbonComboboxFonts.SelectedItem.ToString());
        //        }
        //        return false;
        //    });
        //}

        public static void RibbonComboboxFontHeight(ComboBox RibbonComboboxFontHeight)
        {
            IHTMLDocument2 doc = webBrowser.webBrowser.Document as IHTMLDocument2;
            if (doc != null)
            {
                doc.execCommand("FontSize", false, RibbonComboboxFontHeight.SelectedItem);
            }
        }

        public static void RibbonComboboxFormat(ComboBox RibbonComboboxFormat)
        {
            //string ID = ((Items)(RibbonComboboxFormat.SelectedItem)).Value;

            //webBrowser.doc = webBrowser.webBrowser.Document as HTMLDocument;
            //if (webBrowser.doc != null)
            //{
            //    webBrowser.doc.execCommand("FormatBlock", false, ID);
            //}
        }

        public static void EditWeb()
        {
            if (webBrowser.Visibility == Visibility.Visible) return;
            htmlEditor.Visibility = Visibility.Hidden;
            webBrowser.Visibility = Visibility.Visible;
            htmlEditor.Editor.SelectAll();

            webBrowser.doc.body.innerHTML = htmlEditor.Editor.Selection.Text;

        }

        public static void ViewHTML()
        {
            if (htmlEditor.Visibility == Visibility.Visible) return;
            htmlEditor.Visibility = Visibility.Visible;
            webBrowser.Visibility = Visibility.Hidden;

            htmlEditor.Editor.Selection.Text = webBrowser.doc.documentElement.innerHTML;
        }

        public static void newdocument()
        {
            webBrowser.newWb("");
        }

        public static void newdocumentFile()
        {
            webBrowser.newWb(DialogBox.SelectFile());
        }

    }
}
