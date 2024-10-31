using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BClouderTask
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public int UserID { get; set; }
        public BClouderUser? User { get; set; }
        public Status TaskStatus { get; set; }
        public enum Status
        {
            Pending,     // 0
            InProgress,  // 1
            Completed     // 2
        }
        public bool Active { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? Deleted { get; set; }
    }
}
