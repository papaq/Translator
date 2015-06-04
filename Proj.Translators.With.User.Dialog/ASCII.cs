using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class ASCII
    {
        public string stringASCII;

        public ASCII()
        {
            stringASCII = makestringASCII();
        }

        private string makestringASCII()
        {
            StringBuilder strASCII = new StringBuilder(256);
            for (int i = 0; i < 256; i++)
            {
                strASCII.Append((char)i);
            }
            return strASCII.ToString();
        }
    }
}
