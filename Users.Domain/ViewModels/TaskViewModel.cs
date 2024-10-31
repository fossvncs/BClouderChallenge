using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class TaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public _Status _TaskStatus { get; set; } // Incluir status como enum

        public enum _Status
        {
            Pending,     // 0
            InProgress,  // 1
            Completed    // 2
        }
    }
}
