using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace testerV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static string _testingExe;
        private static string _testingTxt;
        private static string _workingDirectory;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void chooseExeButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executable files| *.exe";
            dlg.InitialDirectory = _workingDirectory;
            var result = dlg.ShowDialog();
            if (result == true)
            {
                _testingExe = dlg.FileName;
                programLabel.Content = "Program: " + Path.GetFileName(_testingExe);  
            }
        }

        private void chooseTxtButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            _workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text files| *.txt";
            dlg.InitialDirectory = _workingDirectory;
            var result = dlg.ShowDialog();
            if (result == true)
            {
                _testingTxt = dlg.FileName;
                inputLabel.Content = "Vstup: " + Path.GetFileName(_testingTxt);
            }
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader(_testingTxt))
                {
                    String text = sr.ReadToEnd();
                    inputBox.Text = text;
                    inputBox.TextAlignment = TextAlignment.Left;
                }

                
                outputBox.Text = RunTestingExe;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "File reading Error");
            }
        }

        private static string RunTestingExe
        {
            get
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = _testingExe,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                };

                try
                {
                    Process program;
                    using (program = Process.Start(startInfo))
                    {
                        StreamWriter input = program?.StandardInput;
                        string inputText = File.ReadAllText(_testingTxt);
                        input?.Write(inputText);
                        input?.Close();
                        StreamReader reader = program?.StandardOutput;
                        string result = reader?.ReadToEnd();
                        reader?.Close();
                        return (result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            inputBox.Text = "Input";
            inputBox.TextAlignment = TextAlignment.Center;
            outputBox.Text = "Output";
            outputBox.TextAlignment = TextAlignment.Center;
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = File.CreateText(_workingDirectory + "output.txt"))
            {
                sw.Write(outputBox.Text);
            }
        }
    }
}
