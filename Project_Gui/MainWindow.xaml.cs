using Microsoft.Win32;
using Project_Logic.CodeExecution;
using Project_Logic.Entities;
using Project_Logic.Obfuscators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                LeftTextBoxCode.Text = File.ReadAllText(txtFiles[0]);
            }
        }

        private void Obfuscate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var obfucateCode = _codeObfuscator.Obfuscate(LeftTextBoxCode.Text);
                RightTextBoxCode.Text = obfucateCode;
            }
            catch (Exception ex)
            {
                RightTextBoxCode.Text = ex.Message;
            }
        }

        private void RunAll_Click(object sender, RoutedEventArgs e)
        {
            LeftTextBoxResult.Text = "";
            RightTextBoxResult.Text = "";

            RunCode(LeftTextBoxCode.Text, LeftTextBoxResult);
            RunCode(RightTextBoxCode.Text, RightTextBoxResult);

            LeftTextBoxResult.Background = LeftTextBoxResult.Text.Contains("RESULT:")
                ? new SolidColorBrush(Color.FromRgb(154, 250, 135))
                : new SolidColorBrush(Color.FromRgb(250, 134, 121));

            RightTextBoxResult.Background = RightTextBoxResult.Text.Contains("RESULT:")
                ? new SolidColorBrush(Color.FromRgb(154, 250, 135))
                : new SolidColorBrush(Color.FromRgb(250, 134, 121));
        }

        private void RunCode(string code, TextBox resultTB)
        {
            try
            {
                CompilationInfo? compiledCode = Compilator.Compile(code);

                if (compiledCode.CompilationFailures != null)
                {
                    setCompilationErrors(resultTB, compiledCode.CompilationFailures);
                }
                else
                {
                    ExecutionInfo? executionInfo = Executor.Execute(compiledCode.CompilationMS);

                    if (executionInfo.Failure != null)
                    {
                        setExecutionError(resultTB, executionInfo.Failure);
                    }
                    else
                    {
                        resultTB.AppendText("RESULT: ");
                        resultTB.AppendText(executionInfo.Result!.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                resultTB.AppendText($"Source: {ex.Source} \n");
                resultTB.AppendText($"Message: {ex.Message} \n");
                resultTB.AppendText($"StackTrace: {ex.StackTrace} \n");
            }
        }

        private void setCompilationErrors(TextBox tb, IEnumerable<CompilationFailure> failures)
        {
            tb.AppendText("Compilation Errors: \n");

            foreach (var error in failures)
            {
                tb.AppendText($"Error ID: {error.ErrorId}\n");
                tb.AppendText($"Message: {error.Message}\n");
                tb.AppendText($"Severity: {error.Severity}\n");
                tb.AppendText($"Start Line: {error.StartLine}\n");
                tb.AppendText($"Start Column: {error.StartColumn}\n\n");
            }
        }

        private void setExecutionError(TextBox tb, ExecutionFailure failure)
        {
            tb.AppendText("Execution Errors: \n");

            tb.AppendText($"Exec Exception: {failure.ExecException}\n");
            tb.AppendText($"Message: {failure.Message}\n");
            tb.AppendText($"Stack Trace: {failure.StackTrace}\n");           
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
                    LeftTextBoxCode.Text = fileContent;
                }
                catch (Exception ex)
                {
                    RightTextBoxCode.Text = $"Błąd odczytu pliku: {ex.Message}";
                }
            }
        }

        private void CodeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = CodeComboBox.SelectedIndex;
                string[] txtFiles = Directory.GetFiles(_txtCodesPath, "*.txt");
                LeftTextBoxCode.Text = File.ReadAllText(txtFiles[selectedIndex]);
            }
            catch (Exception ex)
            {
                RightTextBoxCode.Text = ex.Message;
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            RightTextBoxCode.Text = "";
            LeftTextBoxCode.Text = "";
            LeftTextBoxResult.Text = "";
            RightTextBoxResult.Text = "";
        }
    }
}
