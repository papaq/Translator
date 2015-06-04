using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class LexemArray
    {
        readonly public string lexemType;
        readonly public string mean = null;
        readonly public int shortCode = -1;
        readonly public int startPos = 0;

        public LexemArray(string type, string mean, int code, int pos)
        {
            this.lexemType = type;
            this.mean = mean;
            this.shortCode = code;
            this.startPos = pos;
        }

    }
}
