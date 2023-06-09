using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Rich_Text_Editor_Homework {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        bool isbold = false, isitalic = false, isunderline = false, isStrike = false;
        bool? autoSave = false;
        string currentText;

        private void BoldButton_Click(object sender, RoutedEventArgs e) {
            if (!isbold) isbold = true;
            else isbold = false;
        }

        private void richTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (isbold) richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            else richTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal); 
            if (isitalic) richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
            else richTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            FontFamily.ItemsSource = Fonts.SystemFontFamilies.Select(f => f.ToString());
            FontSize.ItemsSource = new string[] { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" };
        }

        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            richTextBox.FontFamily = new(FontFamily.SelectedItem as string);
        }

        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            richTextBox.FontSize = Convert.ToDouble(FontSize.SelectedItem as string);
        }

        private void TextColor_Click(object sender, RoutedEventArgs e) {
            ColorDialog colordialog = new ColorDialog();
            if (colordialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                richTextBox.SelectAll();
                richTextBox.Foreground = new SolidColorBrush(Color.FromRgb(colordialog.Color.R, colordialog.Color.G, colordialog.Color.B));
            }
        }

        private void ColorFill_Click(object sender, RoutedEventArgs e) {
            ColorDialog colordialog = new ColorDialog();
            if (colordialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                richTextBox.SelectAll();
                richTextBox.Background = new SolidColorBrush(Color.FromRgb(colordialog.Color.R, colordialog.Color.G, colordialog.Color.B));
            }
        }

        private void UploadFile_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true) {
                filepath.Text = openFileDialog.FileName;
                richTextBox.SelectAll();
                richTextBox.Selection.Text = File.ReadAllText(openFileDialog.FileName);
                richTextBox.UnselectAllText(); // It is my function added with extension method
            }
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e) {
            if (filepath.Text != "") {
                richTextBox.SelectAll();
                File.WriteAllText(filepath.Text, richTextBox.Selection.Text);
                richTextBox.Selection.Select(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            }
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true) {
                richTextBox.SelectAll();
                File.WriteAllText(saveFileDialog.FileName, richTextBox.Selection.Text);
            }
        }

        private void SaveButon_Click(object sender, RoutedEventArgs e) {
            if (filepath.Text != "") {
                richTextBox.SelectAll();
                File.WriteAllText(filepath.Text, richTextBox.Selection.Text);
                richTextBox.Selection.Text = richTextBox.Selection.Text;
            }
            else {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == true) {
                    richTextBox.SelectAll();
                    File.WriteAllText(saveFileDialog.FileName, richTextBox.Selection.Text);
                }
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e) {
            if (richTextBox.Selection.Text.Length > 0) {
                System.Windows.Clipboard.SetText(richTextBox.Selection.Text);
                richTextBox.Selection.Text = string.Empty;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e) {
            if (richTextBox.Selection.Text.Length > 0) {
                System.Windows.Clipboard.SetText(richTextBox.Selection.Text);
            }
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e) {
            if (System.Windows.Clipboard.ContainsText()) {
                richTextBox.CaretPosition.InsertTextInRun(System.Windows.Clipboard.GetText());
            }
        }

        private void SelectAllCheckBox_Checked(object sender, RoutedEventArgs e) {
            richTextBox.SelectAll();
            string a = richTextBox.Selection.Text;
        }

        private void AutoSave_Checked(object sender, RoutedEventArgs e) {
            autoSave = true;
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (autoSave is bool s && s) SaveMenuItem_Click(sender, e);
        }

        private void AutoSave_Unchecked(object sender, RoutedEventArgs e) {
            autoSave = false;
        }

        private void Strike_Click(object sender, RoutedEventArgs e) {
            richTextBox.SelectAll();
            if (!isStrike) {
                richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
                richTextBox.Selection.Select(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
                isStrike = true;
             }
            else {
                richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                richTextBox.Selection.Select(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
                isStrike = false;
            }
        }

        private void Italic_Click(object sender, RoutedEventArgs e) {
            if (!isitalic) isitalic = true;
            else isitalic = false;
        }

        private void Underline_Click(object sender, RoutedEventArgs e) {
            richTextBox.SelectAll();
            if (!isunderline) {
                richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                richTextBox.Selection.Select(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
                isunderline = true;
             }
            else {
                richTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                richTextBox.Selection.Select(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
                isunderline = false;
            }
        }
    }
}