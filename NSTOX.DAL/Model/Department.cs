using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class Department : ModelBase
    {
        [UpdateableField]
        public int Id { get; set; }
        [UpdateableField]
        public string Description { get; set; }
        public DateTime? DateAdd { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime? DateRemoved { get; set; }
    }
}
