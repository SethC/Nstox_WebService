using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DataPusher.BOService;

namespace NSTOX.DataPusher.Model
{
    class LocalFile
    {
        public BOFile BOFile { get; set; }
        public byte[] FileContent { get; set; }
    }
}
