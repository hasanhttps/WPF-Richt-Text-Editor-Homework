using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;

namespace WPF_Rich_Text_Editor_Homework {
    public static class Extension {

        public static void UnselectAllText(this System.Windows.Controls.RichTextBox richTextBox) {
            richTextBox.Selection.Select(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
        }

    }
}
