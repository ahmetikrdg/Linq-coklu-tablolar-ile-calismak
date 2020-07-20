using System;
using System.Collections.Generic;

namespace EFD.Data.EfCore
{
    public partial class EmployeePerformans
    {
        public int EmployeeId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int? SatisAdeti { get; set; }
    }
}
