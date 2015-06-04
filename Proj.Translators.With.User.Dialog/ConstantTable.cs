using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class ConstantTable
    {
        readonly public string constType;
        readonly public string mean;
        readonly public int shortCode;

        public ConstantTable(string type, string mean, int code)
        {
            this.constType = type;
            this.mean = mean;
            this.shortCode = code;
        }
    }
}
