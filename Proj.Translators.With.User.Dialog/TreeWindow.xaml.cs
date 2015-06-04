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
using System.IO;
using System.Collections.ObjectModel;

namespace Proj.Translators.With.User.Dialog
{
    /// <summary>
    /// Interaction logic for TreeWindow.xaml
    /// </summary>
    public partial class TreeWindow : Window
    {
        public TreeWindow()
        {
            InitializeComponent();

            treeViewBox.Items.Add(returnTreeStructure());
        }

        private TreeViewItem returnTreeStructure()
        {
            TreeViewItem head = new TreeViewItem();
            head.Header = Tables.tree.nonterminal;
            if (Tables.tree.branch.Count > 0)
            {
                head.Items.Add(treeChild(Tables.tree.branch));  
            }
            
            return head;
        }

        private TreeViewItem treeChild(LinkedList<TreeNode> list)
        {
            TreeViewItem returnBranch = new TreeViewItem();
            foreach (TreeNode branch in list)
            {
                TreeViewItem temp = new TreeViewItem();
                temp.Header = branch.nonterminal;
                if (branch.branch.Count > 0)
                {
                    temp.Items.Add(treeChild(branch.branch));
                    //temp = (treeChild(branch.branch));
                }

                //foreach(TreeViewItem boxi in temp.Items)
                //{
                //    returnBranch.Items.Add(boxi);
                //}

                returnBranch.Items.Add(temp);
                returnBranch.Header = "---------------------";
            }

            return returnBranch;
        }
    }
        
}
