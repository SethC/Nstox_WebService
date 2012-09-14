using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSTOX.DAL.Model
{
    public class DataError : ModelBase
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [UpdateableField]
        public InputFileType Source { get; set; }

        [UpdateableField]
        public long ElementId { get; set; }

        [UpdateableField]
        public string Note { get; set; }
    }
}
