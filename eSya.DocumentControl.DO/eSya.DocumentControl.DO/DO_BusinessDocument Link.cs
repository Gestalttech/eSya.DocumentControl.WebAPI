using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.DocumentControl.DO
{
    public class DO_BusinessDocument_Link
    {
        public int BusinessKey { get; set; }
        public string CalendarKey { get; set; }
        public int ComboId { get; set; }
        public int FormId { get; set; }
        public int DocumentId { get; set; }
        public string SchemaId { get; set; } 
        public bool UsageStatus { get; set; }
        public bool FreezeStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string? FormName { get; set; }
        public string? DocumentName { get; set; }
    }
}
