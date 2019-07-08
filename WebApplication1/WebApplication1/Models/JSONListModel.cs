using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class JSONListModel
    {
        public int ListId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public List<JSONListItem> ListItem { get; set; }
    }
}
