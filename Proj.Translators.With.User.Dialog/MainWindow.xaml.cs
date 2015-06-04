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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Proj.Translators.With.User.Dialog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileAddress = "D:/University/Usix/ProjectinTranslators/Proj.Translators.With.User.Dialog/FILES/TestProgram.txt";
        string MyVarodt = "D:/University/Usix/ProjectinTranslators/Proj.Translators.With.User.Dialog/FILES/MyVar.txt";
        List<LexemArray> codedLexemLine;
        string programCode;
        string assCode;

        public MainWindow()
        {
            InitializeComponent();
            outputProgramCode();
            messageBox.Text = "";
        }

        private void outputProgramCode()
        {
            fileAddress = fileLocation.Text;
            string[] programCode = Files.returnAllLines(fileLocation.Text);
            if (programCode.Length > 0)
            {
                programField.Text = "";
                for (int i = 0; i < programCode.Length; i++)
                {
                    programField.Text += programCode[i] + "\n";
                }
            }
            else
            {
                printMessage("Invalid file address!");
            }

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            outputProgramCode();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            //            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files
            //            (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {

                // Open document
                string filename = dlg.FileName;
                fileLocation.Text = filename;
                outputProgramCode();
            }
        }

        private void fileLocation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                outputProgramCode();
            }
        }

        private void buttonLex_Click(object sender, RoutedEventArgs e)
        {
            Tables.clearAllTables();
            programCode = programField.Text;
            LexicalAnalizer lex = new LexicalAnalizer(programCode);
            printMessage("Lexical Analisys has started!");
            lex.lookThrough();
            codedLexemLine = lex.returnLexemLine();
            printMessage("Lexical Analisys has successfully completed!");
            printMessage(Tables.errors.Count + " error(s) have been found!");
            if (Tables.errors.Count < 1)
            {
                buttonParce.IsEnabled = true;
            }
            buttonSem.IsEnabled = false;
            w1.IsEnabled = true;
            f1.IsEnabled = true;
            w2.IsEnabled = false;
            f2.IsEnabled = false;
            w3.IsEnabled = false;
            f3.IsEnabled = false;
            wAll.IsEnabled = false;
            fAll.IsEnabled = false;
        }

        private void printMessage(string message)
        {
            messageBox.AppendText("-----------------------------------------------\n" + message + "\n");
            messageBox.ScrollToEnd();
        }

        private void w1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (codedLexemLine.Count > 0)
            {
                OutputWindow window = new OutputWindow(returnLexemsStringInArray());
                window.Title = "Lexems";
                window.Show();
                printMessage("Lexem string has been displayed!");
                return;
            }
            printMessage("Lexem string does not exist!");
        }

        private string[] returnLexemsStringInArray()
        {
            string[] resultStrings = new string[codedLexemLine.Count * 4];
            int i = 0;
            foreach (LexemArray lexem in codedLexemLine)
            {
                resultStrings[i++] += lexem.startPos.ToString() + "  " + lexem.lexemType; //(n-1)/4
                resultStrings[i++] += "     " + lexem.mean;
                resultStrings[i++] += "     " + lexem.shortCode;
                resultStrings[i++] += "\n";
            }

            return resultStrings;
        }

        private void f1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string lexemsOutFileName = fileAddress.Substring(0, fileAddress.LastIndexOf('/') + 1) + "LEXEMS_STRING.txt";
            Files.writeFile(lexemsOutFileName, returnLexemsStringInArray());
            printMessage("Lexem string has been printed into " + lexemsOutFileName + " !");
        }

        private void buttonParce_Click(object sender, RoutedEventArgs e)
        {
            wAll.IsEnabled = false;
            fAll.IsEnabled = false;
            if (codedLexemLine.Count > 0)
            {
                SyntaxAnalizer sith = new SyntaxAnalizer(codedLexemLine);
                printMessage("Parcing has started!");
                if (sith.parse())
                {
                    printMessage("Parcing has successfully finished!");
                    buttonSem.IsEnabled = true;
                    w2.IsEnabled = true;
                    f2.IsEnabled = true;
                    w3.IsEnabled = false;
                    f3.IsEnabled = false;
                    return;
                }
                else
                {
                    printMessage("Parcing has stopped!");
                    printMessage(Tables.errors.Count + " error(s) have been found!");
                    return;
                }
            }
            printMessage("Fail! No lexems have been found!");

        }

        private void w2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeWindow tree = new TreeWindow();
            tree.Show();
            printMessage("Lexem tree has been displayed!");
        }

        private void f2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string lexemTreeOutFileName = fileAddress.Substring(0, fileAddress.LastIndexOf('/') + 1) + "LEXEM_TREE.txt";
            Files.writeFile(lexemTreeOutFileName, returnTreeStructure());
            printMessage("Lexem tree has been printed into " + lexemTreeOutFileName + " !");
        }

        private string returnTreeStructure()
        {
            string head = Tables.tree.nonterminal + "\n";
            if (Tables.tree.branch.Count > 0)
            {
                head += treeChild(Tables.tree.branch, 1);
            }

            return head;
        }

        private string treeChild(LinkedList<TreeNode> list, int n)
        {
            string returnBranch = "";
            foreach (TreeNode branch in list)
            {
                string temp = "";
                for (int i = 0; i < n; i++)
                {
                    temp += "    ";
                }
                temp += branch.nonterminal + "\n";
                if (branch.branch.Count > 0)
                {
                    temp += treeChild(branch.branch, n + 1);
                }

                returnBranch += temp;
            }

            return returnBranch;
        }

        private void buttonSaveProgram_Click(object sender, RoutedEventArgs e)
        {
            string changedProgramFileName = fileAddress.Substring(0, fileAddress.LastIndexOf('.')) + "_CHANGED.txt";
            Files.writeFile(changedProgramFileName, programField.Text);
            printMessage("Program code has been saved into " + changedProgramFileName + " !");
        }

        private void programField_KeyDown(object sender, KeyEventArgs e)
        {
            buttonSaveProgram.IsEnabled = true;
        }

        private void buttonSem_Click(object sender, RoutedEventArgs e)
        {
            SemanticProcessor semka = new SemanticProcessor();
            printMessage("Translating!");
            assCode = semka.translate(Tables.tree.branch.First.Value.branch);

            if (Tables.errors.Count < 1)
            {
                printMessage("Translation was successful!");
                w3.IsEnabled = true;
                f3.IsEnabled = true;

                wAll.IsEnabled = false;
                fAll.IsEnabled = false;
            }

            printMessage(Tables.errors.Count + " error(s) have been found!");
        }

        private void w3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OutputWindow window = new OutputWindow(assCode);
            window.Title = "Translated Code";
            window.Show();
            printMessage("Assembler code has been displayed!");
            return;
        }

        private void buttonShowRules_Click(object sender, RoutedEventArgs e)
        {
            OutputWindow window = new OutputWindow(Files.returnAllLines(MyVarodt));
            window.Title = "My Rules";
            window.Show();
            printMessage("Rules have been displayed!");
            return;
        }

        private void f3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string codeOutFileName = fileAddress.Substring(0, fileAddress.LastIndexOf('/') + 1) + "ASSEMBLER_CODE.txt";
            Files.writeFile(codeOutFileName, assCode.Split('\n'));
            printMessage("Assembler code has been printed into " + codeOutFileName + " !");
        }

        private void buttonAll_Click(object sender, RoutedEventArgs e)
        {
            buttonLex_Click(sender, e);
            if (Tables.errors.Count < 1)
            {
                buttonParce_Click(sender, e);
                if (Tables.errors.Count < 1)
                {
                    buttonSem_Click(sender, e);
                    wAll.IsEnabled = true;
                    fAll.IsEnabled = true;
                    return;
                }
            }
            wAll.IsEnabled = false;
            fAll.IsEnabled = false;
        }

        private void buttonErrors_Click(object sender, RoutedEventArgs e)
        {
            if (Tables.errors.Count > 0)
            {
                OutputWindow window = new OutputWindow(errorLines());
                window.Title = "Errors List";
                window.Show();
                printMessage("Errors list has been displayed!");
                return;
            }
            printMessage("There are no errors!");
        }

        private void wErr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            buttonErrors_Click(sender, e);
        }

        private string[] errorLines()
        {
            string[] lines = new string[Tables.errors.Count];
            for (int i = 0; i < Tables.errors.Count; i++)
            {
                lines[i] = i.ToString() + "  Error starting with " + Tables.errors[i].errorPos + " letter in original code file;";
            }
            return lines;
        }

        private void fErr_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Tables.errors.Count > 0)
            {
                string codeOutFileName = fileAddress.Substring(0, fileAddress.LastIndexOf('/') + 1) + "ERROR_LINES.txt";
                Files.writeFile(codeOutFileName, errorLines());
                printMessage("Error lines have been printed into " + codeOutFileName + " !");
            }
            printMessage("There are no errors!");
        }

    }
}
