// D:\ShiBoo\Models\Shift.cs
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShiBoo.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [NotMapped] 
        public string UserName { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public string ShiftName { get; set; } = string.Empty;
        
        // THÊM DÒNG NÀY
        public string Note { get; set; } = string.Empty; 

        public string Status { get; set; } = "Pending";
        public string RequestType { get; set; } = "None";
    }
}