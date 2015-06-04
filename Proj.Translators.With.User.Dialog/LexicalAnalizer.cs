using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class LexicalAnalizer
    {
        private string programCode;
        int programLength;
        
        List<LexemArray> codedLexemLine = new List<LexemArray>();

        public LexicalAnalizer(string code)
        {
            programCode = code;
            programLength = code.Length;
        }

        public List<LexemArray> returnLexemLine()
        {
            return codedLexemLine;
        }

        public void lookThrough()
        {
            int tempPosition = 0;

            if (programLength > 0)
            {
                do
                {
                    tempPosition = detectSymbol(programCode[tempPosition].ToString(),
                        tempPosition);
                } while (tempPosition < programLength);
            }
            
        }

        private int whiteSpace(int n)
        {
            return n + 1;
        }

        private int error(string strCode, int n)
        {
            Tables.errors.Add(new ErrorList(n));
            codedLexemLine.Add(new LexemArray("SYMBOL", strCode[n].ToString(), 
                Tables.ascii.stringASCII.IndexOf(strCode[n]), n));
            return n + 1;
        }

        private int semicolonSymbol(string strCode, int n)
        {
            codedLexemLine.Add(new LexemArray("SEMICOLON", ";", 59, n));
            return n + 1;
        }

        private int colonSymbol(string strCode, int n)
        {
            codedLexemLine.Add(new LexemArray("COLON", ":", 58, n));
            return n + 1;
        }

        private int fullStopSymbol(string strCode, int n)
        {
            codedLexemLine.Add(new LexemArray("FULLSTOP", ".", 46, n));
            return n + 1;
        }

        private int number(string strCode, int n)
        {
            int temp, index;
            string output;
            string numbers = Tables.ascii.stringASCII.Substring(48, 10);

            for (temp = n + 1; (temp < programLength) 
                && (numbers.Contains(strCode[temp])); temp++) { }

            output = strCode.Substring(n, temp - n);

            if ((index = isInTable(Tables.constTable, output)) == -1)
            {
                index = addToTable(Tables.constTable, output);
            }

            codedLexemLine.Add(new LexemArray("CONSTANT", output, index, n));

            return temp;
        }

        private int identifier(string strCode, int n)
        {
            int temp, index;
            string output;
            string letters = Tables.ascii.stringASCII.Substring(65, 26);
            string numbers = Tables.ascii.stringASCII.Substring(48, 10);

            for (temp = n + 1; (temp < programLength) 
                && (letters.Contains(strCode[temp]) 
                || numbers.Contains(strCode[temp])); temp++) { }

            output = strCode.Substring(n, temp - n);

            /* check if this string is a reserved word */
            if ((index = isReserved(Tables.keyWords.keyWords, output)) != -1)
            {
                codedLexemLine.Add(new LexemArray("RESERVED", output, index, n));
            }
            else
            {
                //if ((index = isInTable(Tables.idTable, output)) == -1)
                //{
                    index = addToTable(Tables.idTable, output);
                //}
                codedLexemLine.Add(new LexemArray("IDENTIFIER", output, index, n));
            }

            return temp;
        }

        private int delimiter(string strCode, int n)
        {
            int temp;
            string output;

            if ((n + 1 != programLength) 
                && (Tables.multDelimiter.Contains(strCode.Substring(n, 2))))
            {
                output = strCode.Substring(n, 2);
                codedLexemLine.Add(new LexemArray("MULTDELIMITER", 
                    output, Array.IndexOf(Tables.multDelimiter, output) + 301, n));
                temp = n + 2;
            }
            else
            {
                output = strCode.Substring(n, 1);
                codedLexemLine.Add(new LexemArray("DELIMITER", 
                    output, Tables.ascii.stringASCII.IndexOf(output), n));
                temp = n + 1;
            }

            return temp;
        }

        private int comment(string strCode, int n)
        {
            int temp = n + 1;

            if (temp >= programLength)
            {
                return temp;
            }

            if (strCode[temp] != '*')
            {
                Tables.errors.Add(new ErrorList(n));
                return temp;
            }

            while (true)
            {
                temp += 1;
                if (temp >= programLength)
                {
                    Tables.errors.Add(new ErrorList(temp));
                    return temp;
                }

                if (strCode[temp] == '*')
                {
                    if (temp + 1 == programLength)
                    {
                        Tables.errors.Add(new ErrorList(temp));
                        return temp + 1;
                    }
                    if (strCode[temp + 1] == ')')
                    {
                        return temp + 2;
                    }
                }
            }
        }

        private int isInTable(List<ConstantTable> list, string mean)
        {
            int length = list.Count();

            for (int i = 0; i < length; i++)
            {
                if (list[i].mean == mean)
                {
                    return list[i].shortCode;
                }
            }
            return -1;
        }

        private int addToTable(List<IdentifierTable> list, string id)
        {
            int index = list.Count() + 1001;
            list.Add(new IdentifierTable(id, index));
            return index;
        }

        private int addToTable(List<ConstantTable> list, string mean)
        {
            int index = list.Count() + 501;
            list.Add(new ConstantTable("UNSIGNED-INTEGER", mean, index));
            return index;
        }

        private int isReserved(string[] container, string id)
        {
            if (container.Contains(id))
            {
                return Array.IndexOf(container, id) + 401;
            }
            return -1;
        }

        private int detectSymbol(string s, int n)
        {
            int t = Tables.ascii.stringASCII.IndexOf(s);
            switch (t)
            {
                case 9:
                case 10:
                case 32: return whiteSpace(n);
                case 40: return comment(programCode, n);
                case 46: return fullStopSymbol(programCode, n);
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57: return number(programCode, n);
                case 58: return colonSymbol(programCode, n);
                case 59: return semicolonSymbol(programCode, n);
                case 60:
                case 61:
                case 62: return delimiter(programCode, n);
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90: return identifier(programCode, n);
                default: return error(programCode, n);
            }
        }
    }
}
