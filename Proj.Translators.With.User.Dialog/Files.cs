using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Proj.Translators.With.User.Dialog
{
    public static class Files
    {
        public static string[] returnAllLines(string inFile)
        {
            if (File.Exists(inFile))
            {
                try
                {
                    return File.ReadAllLines(inFile);
                }
                catch(Exception)
                { }
            }

            return new string[0];
        }

        public static void writeFile(string fileName, string[] strings)
        {
            StreamWriter outFile = new StreamWriter(File.Create(fileName));

            foreach (string str in strings)
            {
                outFile.WriteLine(str);
            }

            outFile.Close();
        }

        public static void writeFile(string fileName, string str)
        {
            StreamWriter outFile = new StreamWriter(File.Create(fileName));

            outFile.WriteLine(str);

            outFile.Close();
        }
    }
}
