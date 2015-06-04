using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Translators.With.User.Dialog
{
    public class IdentifierTable
    {
        readonly public string idName;
        readonly public int shortCode;
        public bool marked = false;

        public IdentifierTable(string name, int code)
        {
            this.idName = name;
            this.shortCode = code;
        }

        
    }
}
