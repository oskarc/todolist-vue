using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public partial class List
    {
        public List()
        {
            ListItem = new HashSet<ListItem>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public int ListId { get; set; }
        public string Title { get; set; }

        public ICollection<ListItem> ListItem { get; set; }
    }
}
