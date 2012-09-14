using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class DepartmentAudit : ModelBase
    {
        [UpdateableField]
        public int DepartmentId { get; set; }
        [UpdateableField]
        public string OldValue { get; set; }
        [UpdateableField]
        public string NewValue { get; set; }
    }
}
