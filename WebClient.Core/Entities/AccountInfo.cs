using System;
using System.Collections.Generic;
using System.Text;

namespace WebClient.Core.Entities
{
    public class AccountInfo
    {
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int KindOfInvoice { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
    }
}
