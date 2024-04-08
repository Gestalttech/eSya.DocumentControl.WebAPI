using System;
using System.Collections.Generic;

namespace eSya.DocumentControl.DL.Entities
{
    public partial class GtEbetrm
    {
        public int TranModeId { get; set; }
        public string TranModeDesc { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
