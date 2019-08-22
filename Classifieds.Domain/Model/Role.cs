using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name:"user_roles")]
    public class Role
    {
        [Key]
        [Column(name: "pk_role_id")]
        public long ID { set; get; }

        [ForeignKey("UserID")]
        public User User { set; get; }

        [Column(name: "fk_user_id")]
        public long UserID { set; get; }

        [Column(name: "name")]
        public string Name { set; get; }
    }
}
