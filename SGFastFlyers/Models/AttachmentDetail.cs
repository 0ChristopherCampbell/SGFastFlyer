

namespace SGFastFlyers.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;
    public class AttachmentDetail
    {

        public int ID { get; set; }

        public HttpPostedFileBase File;

        public string FileName;

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public virtual Order Order { get; set; }
    }
}