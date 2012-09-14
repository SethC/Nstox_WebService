using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class Retailer
    {
        [UpdateableField]
        public int Id { get; set; }

        [UpdateableField]
        public string Name { get; set; }
    }
}
