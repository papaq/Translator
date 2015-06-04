using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class TreeNode
    {
        public LinkedList<TreeNode> branch;
        public string nonterminal;
        public int shortCode;
        public int pos;

        public TreeNode(string nonterm, int code, int position)
        {
            branch = new LinkedList<TreeNode>();
            this.nonterminal = nonterm;
            this.shortCode = code;
            this.pos = position;
        }

        public TreeNode()
        {
            branch = new LinkedList<TreeNode>();
        }
    }
}
