using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class SyntaxAnalizer
    {
        List<LexemArray> codedLexemLine;
        //TreeNode current;
        int n = 0;

        public SyntaxAnalizer(List<LexemArray> codedLexemLine)
        {
            this.codedLexemLine = codedLexemLine;
        }

        public bool parse()
        {
            if (signalProgram())
            {
                return true;
            }
            return false;
        }

        private bool signalProgram()
        {
            Tables.tree.nonterminal = "SIGNAL-PROGRAM";

            SyntaxReturnStructure tempSRS = program();
            if (tempSRS.tORf)
            {
                Tables.tree.branch.AddLast(tempSRS.treeElement);
                if (n == codedLexemLine.Count - 1)
                {
                    return true;
                }
            }

            Tables.errorAdd(codedLexemLine[n].startPos);
            return false;
        }

        private SyntaxReturnStructure program()
        {
            TreeNode tempBrench = new TreeNode("PROGRAM", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (isEqual(codedLexemLine[n].shortCode, "PROGRAM"))
            {
                tempBrench.branch.AddLast(new TreeNode("PROGRAM", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                SyntaxReturnStructure tempSRS = programIdentifier();

                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);

                    if (isEqual(codedLexemLine[++n].shortCode, ";"))
                    {
                        tempBrench.branch.AddLast(new TreeNode(";", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                        tempSRS = block();

                        if (tempSRS.tORf)
                        {
                            tempBrench.branch.AddLast(tempSRS.treeElement);

                            if (isEqual(codedLexemLine[++n].shortCode, "."))
                            {
                                tempBrench.branch.AddLast(new TreeNode(".", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                                return new SyntaxReturnStructure(true, tempBrench);
                            }
                        }
                    }
                }
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure programIdentifier()
        {
            TreeNode tempBrench = new TreeNode("PROGRAM-IDENTIFIER", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (codedLexemLine[++n].shortCode > 1000)
            {
                tempBrench.branch.AddLast(new TreeNode(codedLexemLine[n].mean, codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                return new SyntaxReturnStructure(true, tempBrench);
            }
            return new SyntaxReturnStructure(false, tempBrench);
        }

        private bool isEqual(int firstCode, string secondStr)
        {
            return (Tables.returnStringFromTable(firstCode) == secondStr);
        }

        private SyntaxReturnStructure block()
        {
            TreeNode tempBrench = new TreeNode("BLOCK", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);
            SyntaxReturnStructure tempSRS = variableDeclaration();

            if (tempSRS.tORf)
            {
                tempBrench.branch.AddLast(tempSRS.treeElement);

                if (isEqual(codedLexemLine[++n].shortCode, "BEGIN"))
                {
                    tempBrench.branch.AddLast(new TreeNode("BEGIN", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                    tempSRS = statementsList();

                    if (tempSRS.tORf)
                    {
                        tempBrench.branch.AddLast(tempSRS.treeElement);

                        if (isEqual(codedLexemLine[++n].shortCode, "END"))
                        {
                            tempBrench.branch.AddLast(new TreeNode("END", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                            return new SyntaxReturnStructure(true, tempBrench);
                        }
                    }
                }
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure variableDeclaration()
        {
            TreeNode tempBrench = new TreeNode("VARIABLE-DECLARATION", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (isEqual(codedLexemLine[n+1].shortCode, "VAR"))
            {
                n++;
                tempBrench.branch.AddLast(new TreeNode("VAR", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                SyntaxReturnStructure tempSRS = declarationsList();

                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);
                    return new SyntaxReturnStructure(true, tempBrench);
                }

                return new SyntaxReturnStructure(false, tempBrench);
            }
            return new SyntaxReturnStructure(true, tempBrench);
        }

        private SyntaxReturnStructure declarationsList()
        {
            TreeNode tempBrench = new TreeNode("DECLARATIONS-LIST", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (codedLexemLine[n+1].shortCode > 1000)
            {
                SyntaxReturnStructure tempSRS = declaration();

                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);
                    tempSRS = declarationsList();

                    if (tempSRS.tORf)
                    {
                        tempBrench.branch.AddLast(tempSRS.treeElement);
                        return new SyntaxReturnStructure(true, tempBrench);
                    }
                }

                return new SyntaxReturnStructure(false, tempBrench);
            }

            return new SyntaxReturnStructure(true, tempBrench);
        }

        private SyntaxReturnStructure declaration()
        {
            TreeNode tempBrench = new TreeNode("DECLARATION", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);
            SyntaxReturnStructure tempSRS = variableIdentifier();
            if (tempSRS.tORf)
            {
                tempBrench.branch.AddLast(tempSRS.treeElement);

                if (isEqual(codedLexemLine[++n].shortCode, ":"))
                {
                    tempBrench.branch.AddLast(new TreeNode(":", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                    tempSRS = attribute();

                    if (tempSRS.tORf)
                    {
                        tempBrench.branch.AddLast(tempSRS.treeElement);

                        if (isEqual(codedLexemLine[++n].shortCode, ";"))
                        {
                            tempBrench.branch.AddLast(new TreeNode(";", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                            return new SyntaxReturnStructure(true, tempBrench);
                        }
                    }
                }
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure variableIdentifier()
        {
            TreeNode tempBrench = new TreeNode("VARIABLE-IDENTIFIER", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (codedLexemLine[++n].shortCode > 1000)
            {
                tempBrench.branch.AddLast(new TreeNode(codedLexemLine[n].mean, codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                return new SyntaxReturnStructure(true, tempBrench);
            }
            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure attribute()
        {
            TreeNode tempBrench = new TreeNode("ATTRIBUTE", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);
            n++;

            if (isEqual(codedLexemLine[n].shortCode, "INTEGER"))
            {
                tempBrench.branch.AddLast(new TreeNode("INTEGER", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                return new SyntaxReturnStructure(true, tempBrench);
            }
            else
            {
                if(isEqual(codedLexemLine[n].shortCode, "FLOAT"))
                {
                    tempBrench.branch.AddLast(new TreeNode("FLOAT", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                    return new SyntaxReturnStructure(true, tempBrench);
                }
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure statementsList()
        {
            TreeNode tempBrench = new TreeNode("STATEMENT-LIST", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (isEqual(codedLexemLine[n + 1].shortCode, "WHILE"))
            {
                SyntaxReturnStructure tempSRS = statement();

                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);
                    tempSRS = statementsList();

                    if (tempSRS.tORf)
                    {
                        tempBrench.branch.AddLast(tempSRS.treeElement);
                        return new SyntaxReturnStructure(true, tempBrench);
                    }
                }

                return new SyntaxReturnStructure(false, tempBrench);
            }

            return new SyntaxReturnStructure(true, tempBrench);
        }

        private SyntaxReturnStructure statement()
        {
            TreeNode tempBrench = new TreeNode("STATEMENT", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (isEqual(codedLexemLine[++n].shortCode, "WHILE"))
            {
                tempBrench.branch.AddLast(new TreeNode("WHILE", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                SyntaxReturnStructure tempSRS = conditionalExpression();

                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);

                    if (isEqual(codedLexemLine[++n].shortCode, "DO"))
                    {
                        tempBrench.branch.AddLast(new TreeNode("DO", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                        tempSRS = statementsList();

                        if (tempSRS.tORf)
                        {
                            tempBrench.branch.AddLast(tempSRS.treeElement);

                            if (isEqual(codedLexemLine[++n].shortCode, "ENDWHILE"))
                            {
                                tempBrench.branch.AddLast(new TreeNode("ENDWHILE", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));

                                if (isEqual(codedLexemLine[++n].shortCode, ";"))
                                {
                                    tempBrench.branch.AddLast(new TreeNode(";", codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                                    return new SyntaxReturnStructure(true, tempBrench);
                                }
                            }
                        }
                    }
                }
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure conditionalExpression()
        {
            TreeNode tempBrench = new TreeNode("CONDITIONAL-EXPRESSION", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);
            SyntaxReturnStructure tempSRS = expression();

            if (tempSRS.tORf)
            {
                tempBrench.branch.AddLast(tempSRS.treeElement);
                tempSRS = comparisonOperator();

                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);
                    tempSRS = expression();

                    if (tempSRS.tORf)
                    {
                        tempBrench.branch.AddLast(tempSRS.treeElement);
                        return new SyntaxReturnStructure(true, tempBrench);
                    }
                }
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure expression()
        {
            TreeNode tempBrench = new TreeNode("EXPRESSION", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);
            SyntaxReturnStructure tempSRS = variableIdentifier();
            if (tempSRS.tORf)
            {
                tempBrench.branch.AddLast(tempSRS.treeElement);
                return new SyntaxReturnStructure(true, tempBrench);
            }
            else
            {
                tempSRS = unsignedInteger(n);
                if (tempSRS.tORf)
                {
                    tempBrench.branch.AddLast(tempSRS.treeElement);
                    return new SyntaxReturnStructure(true, tempBrench);
                }
            }
            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure unsignedInteger(int shortCode)
        {
            TreeNode tempBrench = new TreeNode("UNSIGNED-INTEGER", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if (codedLexemLine[shortCode].shortCode > 500 && codedLexemLine[shortCode].shortCode < 1001)
            {
                tempBrench.branch.AddLast(new TreeNode(codedLexemLine[n].mean, codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                return new SyntaxReturnStructure(true, tempBrench);
            }
            return new SyntaxReturnStructure(false, tempBrench);
        }

        private SyntaxReturnStructure comparisonOperator()
        {
            n++;
            TreeNode tempBrench = new TreeNode("COMPARISON-OPERATOR", codedLexemLine[n].shortCode, codedLexemLine[n].startPos);

            if ((codedLexemLine[n].shortCode > 300 && codedLexemLine[n].shortCode < 401) 
                || (codedLexemLine[n].shortCode > 59 && codedLexemLine[n].shortCode < 63))
            {
                tempBrench.branch.AddLast(new TreeNode(codedLexemLine[n].mean, codedLexemLine[n].shortCode, codedLexemLine[n].startPos));
                return new SyntaxReturnStructure(true, tempBrench);
            }

            return new SyntaxReturnStructure(false, tempBrench);
        }
    }
}
