using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TobaccoShop.DAL.Entities.Identity
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public ICollection<Order> Orders { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public ClientProfile()
        {
            Orders = new List<Order>();
        }
    }
}
