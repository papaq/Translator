using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{ 
    class SemanticProcessor
    {
        public SemanticProcessor()
        {

        }

        public string translate(LinkedList<TreeNode> list)
        {
            string returnBranch = "";

            returnBranch = program(list.First.Next);
            list = list.Last.Previous.Value.branch;
            if (list.First.Value.branch.Count != 0)
            {
                if (list.First.Value.branch.Last.Value.branch.Count != 0)
                {
                    returnBranch += var(
                        list.First.Value.branch.Last.Value.branch.First);
                }
            }

            returnBranch += statement(list.Last.Previous.Value.branch);
            return returnBranch += endProgram();
        }

        private string program(LinkedListNode<TreeNode> branch)
        {
            Tables.idTable[0].marked = true;
            return branch.Value.branch.First.Value.nonterminal + " PROC FAR\n";
        }

        private IdentifierTable exists(string id)
        {
            for (int i = 0; i < Tables.idTable.Count; i++)
            {
                if (Tables.idTable[i].idName == id)
                {
                    return Tables.idTable[i];
                }
            }

            return null;
        }

        private bool isMarked(IdentifierTable id)
        {
            if (id.marked)
            {
                return true;
            }
            return false;
        }

        private void isErroneous(TreeNode id)
        {
            if (isMarked(exists(id.nonterminal)))
            {
                Tables.errorAdd(id.pos);
            }
            else
            {
                Tables.idTable.Find(x => x.idName.Contains(id.nonterminal)).marked = true;
            }
        }


        private string var(LinkedListNode<TreeNode> branch)
        {
            if (branch.Next.Value.branch.Count == 0)
            {
                if (branch.Value.branch.Last.Previous.Value.branch.First.Value.nonterminal 
                    == "INTEGER")
                {
                    isErroneous(branch.Value.branch.First.Value.branch.First.Value);
                    return "LOCAL " 
                        + branch.Value.branch.First.Value.branch.First.Value.nonterminal 
                        + ":BYTE\nnop \n";
                }
                else
                {
                    isErroneous(branch.Value.branch.First.Value.branch.First.Value);
                    return "LOCAL " 
                        + branch.Value.branch.First.Value.branch.First.Value.nonterminal 
                        + ":WORD\nnop\n\n";
                }
            }
            else
            {
                if (branch.Value.branch.Last.Previous.Value.branch.First.Value.nonterminal 
                    == "INTEGER")
                {
                    isErroneous(branch.Value.branch.First.Value.branch.First.Value);
                    return "LOCAL "
                        + branch.Value.branch.First.Value.branch.First.Value.nonterminal 
                        + ":BYTE\n" + var(branch.Next.Value.branch.First);
                }
                else
                {
                    isErroneous(branch.Value.branch.First.Value.branch.First.Value);
                    return "LOCAL "
                        + branch.Value.branch.First.Value.branch.First.Value.nonterminal
                        + ":WORD\n" + var(branch.Next.Value.branch.First);
                }
            }
        }

        private string statement(LinkedList<TreeNode> list)
        {
            if (list.Count == 0)
            {
                return "nop\n\n";
            }
            else
            {
                return whileLoop(list.First.Value.branch.First)
                    + statement(list.Last.Value.branch);
            }
        } 

        private string whileLoop(LinkedListNode<TreeNode> branch)
        {
            string retStr = "";
                retStr = "PUSH BP\nMOV BP,SP\nPUSH CX\nMOV CX,"
                    + branch.Next.Value.branch.First.Value.branch
                    .First.Value.branch.First.Value.nonterminal
                    + "\n";

                /* This identifier was not diclared */
                if (branch.Next.Value.branch.First.Value.branch
                    .First.Value.branch.First.Value.shortCode > 1000
                    && !isMarked(exists(branch.Next.Value.branch.First.Value.branch
                    .First.Value.branch.First.Value.nonterminal)))
                {
                    Tables.errorAdd(branch.Next.Value.branch.First.Value.branch
                    .First.Value.branch.First.Value.pos);
                }

                if (branch.Next.Value.branch.First.Value.branch.First.Value.shortCode > 1000 
                    && !isMarked(exists(branch.Next.Value.branch
                    .First.Value.branch.First.Value.branch.First.Value.nonterminal)))
                {
                    Tables.errorAdd(branch.Next.Value.branch.First.Value.branch
                        .First.Value.branch.First.Value.pos);
                }

                retStr += Tables.addNewLabel() + ":\n";
                retStr += "CMP CX," + branch.Next.Value.branch.Last.Value.branch
                    .First.Value.branch.First.Value.nonterminal + "\n";

                /* This identifier was not diclared */
                if (branch.Next.Value.branch.Last.Value.branch
                    .First.Value.branch.First.Value.shortCode > 1000
                    && !isMarked(exists(branch.Next.Value.branch.Last.Value.branch
                    .First.Value.branch.First.Value.nonterminal)))
                {
                    Tables.errorAdd(branch.Next.Value.branch.Last.Value.branch
                    .First.Value.branch.First.Value.pos);
                }

                retStr += compareOperation(branch.Next.Value.branch
                    .First.Next.Value.branch.First.Value.nonterminal)
                    + " " + Tables.currentFinishLabel() + "\n";
                retStr += "\n" + statement(branch.Next.Next.Next.Value.branch);
                retStr += "JMP " + Tables.currentStartLabel() + "\n";
                retStr += Tables.currentFinishLabel() + ":\n";

                Tables.deleteCurrentLabel();

                return retStr += "POP CX\nMOV SP,BP\nPOP BP\n\n";
        }

        private string compareOperation(string op)
        {
            switch (op)
            {
                case "=": return "JNE";
                case "<": return "JGE";
                case "<=": return "JG";
                case "<>": return "JE";
                case ">=": return "JL";
                default: return "JLE";
            }
        }

        private string endProgram()
        {
            return "RET\nENDP\n";
        }

    }
}
