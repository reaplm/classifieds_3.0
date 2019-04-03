using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Classifieds.Web.Models
{
    public class AdvertViewModel
    {

        
        public long ID { set; get; }

        
        public String Status { set; get; }

       
        public DateTime ApprovedDate { set; get; }

        
        public DateTime PublishedDate { set; get; }

        public DateTime RejectedDate { set; get; }

        public DateTime SubmittedDate { set; get; }


    
        public long MenuID { set; get; }
      
        public Menu Menu { set; get; }
    }
}
