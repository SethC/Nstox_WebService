using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class UpdateableField : Attribute
    {
        public string ParamName { get; set; }
        public bool ExcludeOnInsert { get; set; }
        public bool ExcludeOnUpdate { get; set; }
    }

}
