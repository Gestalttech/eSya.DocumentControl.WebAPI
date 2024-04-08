using System;
using System.Collections.Generic;

namespace eSya.DocumentControl.DL.Entities
{
    public partial class GtDncnbc
    {
        public int BusinessKey { get; set; }
        public string CalendarKey { get; set; } = null!;
        public int ComboId { get; set; }
        public int FormId { get; set; }
        public int DocumentId { get; set; }
        public string SchemaId { get; set; } = null!;
        public bool UsageStatus { get; set; }
        public bool FreezeStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
