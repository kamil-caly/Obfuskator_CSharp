using Microsoft.Win32;
using Project_Logic;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Project_Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CodeObfuscator _codeObfuscator;
        private readonly string _txtCodesPath;
        public MainWindow()
        {
            InitializeComponent();

            _codeObfuscator = new CodeObfuscator();

            string basePath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            _txtCodesPath = Directory.GetParent(
                Directory.GetParent(
                    Directory.GetParent(
                        Directory.GetParent(basePath)!.ToString())!.ToString())!.ToString())!.ToString() + "\\TxtCodes";

            InitializeCodeComboBox();
        }

        private void InitializeCodeComboBox()
        {
            var txtFiles = Directory.GetFiles(_txtCodesPath, "*.txt");

            foreach (var txtFile in txtFiles)
            {
                CodeComboBox.Items.Add(Path.GetFileName(txtFile));
            }

            if (CodeComboBox.Items.Count > 0)
            {
                CodeComboBox.SelectedIndex = 0;
                LeftTextBox.Text = File.ReadAllText(txtFiles[0]);
            }
        }

        private void Obfuscate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obfucateCode = _codeObfuscator.Obfuscate(LeftTextBox.Text);
                RightTextBox.Text = obfucateCode;
            }
            catch (Exception ex)
            {
                RightTextBox.Text = ex.Message;
            }
        }

        private void Deobfuscate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deobfuscateCode = _codeObfuscator.Deobfuscate(LeftTextBox.Text);
                RightTextBox.Text = deobfuscateCode;
            }
            catch (Exception ex)
            {
                RightTextBox.Text = ex.Message;
            }
        }

        private void ReadFromFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string fileDestination = openFileDialog.FileName;

                try
                {
                    string fileContent = File.ReadAllText(fileDestination);
                    LeftTextBox.Text = fileContent;
                }
                catch (Exception ex)
                {
                    RightTextBox.Text = $"Błąd odczytu pliku: {ex.Message}";
                }
            }
        }

        private void CodeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = CodeComboBox.SelectedIndex;
                string[] txtFiles = Directory.GetFiles(_txtCodesPath, "*.txt");
                LeftTextBox.Text = File.ReadAllText(txtFiles[selectedIndex]);
            }
            catch (Exception ex)
            {
                RightTextBox.Text = ex.Message;
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            RightTextBox.Text = "";
            LeftTextBox.Text = "";
        }
    }
}
