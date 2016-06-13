using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeduShop.Model.Models
{
    public class ApplicationUserGroup
    {
        [StringLength(128)]
        [Key]
        [Column(Order = 1)]
        public string UserId { set; get; }

        [Key]
        [Column(Order = 2)]
        public int GroupId { set; get; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { set; get; }

        [ForeignKey("GroupId")]
        public virtual ApplicationGroup ApplicationGroup { set; get; }
    }
}
