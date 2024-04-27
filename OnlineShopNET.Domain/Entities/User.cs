using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopNET.Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id {  get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int role_type { get; set; }
        public DateTime created_at { get; set; }
    }
}
