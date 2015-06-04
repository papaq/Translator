using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    static class Tables
    {
        public static string[] multDelimiter = new string[3] { "<=", ">=", "<>" };
        public static KeyWords keyWords = new KeyWords();
        public static List<ErrorList> errors = new List<ErrorList>();
        public static List<IdentifierTable> idTable = new List<IdentifierTable>();
        public static List<ConstantTable> constTable = new List<ConstantTable>();
        public static ASCII ascii = new ASCII();
        public static TreeNode tree = new TreeNode();
        public static List<LabelForLOOP> labelList = new List<LabelForLOOP>();

        public static string returnStringFromTable(int lexemCode)
        {
            if (lexemCode < 0 || lexemCode > idTable.Count + 1000)
            {
                return "";
            }
            if (lexemCode < 256)
            {
                return ascii.stringASCII[lexemCode].ToString();
            }
            if (lexemCode < 401)
            {
                return multDelimiter[lexemCode - 301];
            }
            if (lexemCode < 501)
            {
                return keyWords.keyWords[lexemCode - 401];
            }
            if (lexemCode < 1001)
            {
                return constTable[lexemCode - 501].mean;
            }
            return idTable[lexemCode - 1001].idName;
        }

        public static void errorAdd(int n)
        {
            errors.Add(new ErrorList(n)); 
        }

        public static TreeNode branchAdd(string nonterm, int lexemPos)
        {
            return new TreeNode();
        }

        public static bool idIsMarked(int code)
        {
            foreach (IdentifierTable id in Tables.idTable)
            {
                if (id.shortCode == code)
                {
                    if (id.marked)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string addNewLabel()
        {
            string newLabel = "Label" + Tables.labelList.Count.ToString();
            Tables.labelList.Add(new LabelForLOOP(newLabel));
            return newLabel;
        }

        public static string currentStartLabel()
        {
            return Tables.labelList[Tables.labelList.Count - 1].newLabel.ToString();
        }

        public static string currentFinishLabel()
        {
            int size = Tables.labelList.Count - 1;
            return Tables.labelList[size].newLabel + "END";
        
        }
        public static void deleteCurrentLabel()
        {
            Tables.labelList.RemoveAt(Tables.labelList.Count - 1);
        }

        public static void clearAllTables()
        {
            errors.Clear();
            idTable.Clear();
            constTable.Clear();
            tree = new TreeNode();
        }
    }
}
