using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("PODImages")]
    public class PODImages
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int ID { get; set; }
        public long PODHeaderId { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class PODImageList
    {
        public string ImgName { get; set; }
        public string InvoiceNo { get; set; }
    }
}
