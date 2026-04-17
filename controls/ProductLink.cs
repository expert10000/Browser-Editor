using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrowser.Controls.BrowserTabStrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductLinks")]
    public class ProductLink
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        public string Status { get; set; } = "pending"; 

        public string LastError { get; set; }
    }

}
