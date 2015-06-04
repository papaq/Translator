using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class Tests
    {
        public void makeTests()
        {
            //test1();
            //test2();
        }

        private void test1()
        {
            LexicalAnalizer lex = new LexicalAnalizer("blabla");
            Console.WriteLine(lex.programCode);

            Console.WriteLine(Tables.ascii.stringASCII);

            if (Tables.ascii.stringASCII[10] == (char)10)
            {
                Console.WriteLine("yes!!!!!");
            }
            else
            {
                Console.WriteLine("no!!!!");
            }
            Console.WriteLine(Tables.ascii.stringASCII.Substring(65, 26));
        }

        private void test2()
        {
            string programCode = "VAR X: INTEGER (*фывдлпо*) BEGIN 345A345";

            LexicalAnalizer lex = new LexicalAnalizer(programCode);
            List<LexemArray> codedLexemLine = lex.returnLexemLine();

            Console.WriteLine(programCode);
            Console.WriteLine("\n");

            lex.lookThrough();

            for (int i = 0; i < codedLexemLine.Count(); i++)
            {
                Console.WriteLine(codedLexemLine[i].lexemType);
                Console.WriteLine(codedLexemLine[i].mean);
                Console.WriteLine(codedLexemLine[i].shortCode);
                Console.WriteLine("\n");
            }
        }
    }
}
