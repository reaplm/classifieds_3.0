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
        public string Name { set; get; }

        [Column(name: "admin_menu")]
        public int Admin { set; get; }

        [Column(name: "icon")]
        public string Icon { set; get; }

        [Column(name: "label")]
        public string Label { set; get; }

        [Column(name: "menu_desc")]
        public string Desc { set; get; }

        [Column(name: "menu_status")]
        public int Active { set; get; }

        [Column(name: "menu_type")]
        public string Type { set; get; }

        [Column(name: "url")]
        public string Url { set; get; }

        [Column(name: "fk_menu_id")]
        public long? ParentID { set; get; }
        [ForeignKey("ParentID")]
        public virtual Menu Parent { set; get; }

        public virtual IEnumerable<Menu> SubMenus { set; get; }

    }
}
