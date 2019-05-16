using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classifieds.Domain.Model
{
    [Table(name: "menu")]
    public class Menu
    {
        [Key]
        [Column(name: "pk_menu_id")]
        public long ID { set; get; }

        [Column(name: "menu_name")]
        public String Name { set; get; }

        [Column(name: "admin_menu")]
        public int Admin { set; get; }

        [Column(name: "icon")]
        public String Icon { set; get; }

        [Column(name: "label")]
        public String Label { set; get; }

        [Column(name: "menu_desc")]
        public String Desc { set; get; }

        [Column(name: "menu_status")]
        public int Active { set; get; }

        [Column(name: "menu_type")]
        public String Type { set; get; }

        [Column(name: "url")]
        public String Url { set; get; }

        [Column(name: "fk_menu_id")]
        public long? ParentID { set; get; }
        [ForeignKey("ParentID")]
        public virtual Menu Parent { set; get; }

        public virtual IEnumerable<Menu> SubMenus { set; get; }

    }
}
