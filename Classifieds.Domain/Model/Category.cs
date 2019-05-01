using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classifieds.Domain.Model
{
    [Table(name: "category")]
    public class Category
    {
        [Key]
        [Column(name: "pk_cat_id")]
        public long ID { set; get; }

        [Column(name:"cat_name")]
        public String Name { set; get; }

        [Column(name: "cat_desc")]
        public String Desc { set; get; }

        [Column(name: "label")]
        public String Label { set; get; }

        [Column(name: "url")]
        public String Url { set; get; }

        [Column(name: "icon")]
        public String Icon { set; get; }

        [Column(name:"cat_status")]
        public int Status { set; get; }

        [Column(name:"fk_cat_id")]
        public long? ParentID { set; get; }
        [ForeignKey("ParentID")]
        public virtual Category Parent { set; get; }

        public virtual List<Category> SubCategories { set; get; }
        public virtual List<Advert> Adverts { set; get; }

    }
}
