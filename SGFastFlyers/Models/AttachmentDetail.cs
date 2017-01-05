

namespace SGFastFlyers.Models
{
    using System.Web;
    public class AttachmentDetail
    {
        public HttpPostedFileBase File;

        public string FileName;

        public int OrderID { get; set; }
    }
}