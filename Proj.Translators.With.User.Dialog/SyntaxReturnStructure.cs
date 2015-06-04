using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    class SyntaxReturnStructure
    {
        public bool tORf;
        public TreeNode treeElement;

        public SyntaxReturnStructure(bool b, TreeNode tree)
        {
            tORf = b;
            treeElement = tree;
        }
    }
}
