using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class ListItem
    {
        public int Id { get; set; }
        public string ListItemText { get; set; }
        public bool Completed { get; set; }
        public int ListId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }

        public List List { get; set; }
    }
}
