using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Proj.Translators.With.User.Dialog
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        string[] output;

        public OutputWindow()
        {
            InitializeComponent();
        }

        public OutputWindow(string[] outputResults)
        {
            InitializeComponent();
            output = outputResults;
            fillTextOutput();
        }

        private void fillTextOutput()
        {
            foreach (string str in output)
            {
                textOutput.Text += str + "\n";
            }
        }
        public OutputWindow(string outputResults)
        {
            InitializeComponent();
            textOutput.Text = outputResults;
        }
    }
}
