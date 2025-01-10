using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table(name: "UploadFileMaster")]
    public class UploadedFilesMst
    {
        [Key]
        public Int64 Id { get; set; }

        [ForeignKey("Customer")]
        public int CustId { get; set; }
        public virtual Customer Customer { get; set; }

        public string User_FileName { get; set; }
        public string Filepath { get; set; }
        public string System_FileName { get; set; }

        [ForeignKey("Users")]
        public int? Upload_By { get; set; }
        public virtual Users Users { get; set; }
        public DateTime? Upload_Date { get; set; }

        public int Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public int Isdeleted { get; set; }
        public string FileType { get; set; }
    }

    public class File_Error_Logs
    {
        [Key]
        public Int64 Id { get; set; }

        [ForeignKey("UploadFileMaster")]
        public long? UploadedFileMst_ID { get; set; }
        public virtual UploadedFilesMst UploadFileMaster { get; set; }

        [ForeignKey("OrderExcels")]
        public long OrderExcels_ID { get; set; }
        public virtual OrderExcel OrderExcels { get; set; }

        public string Error_Desc { get; set; }
        public string Error_Type { get; set; }

        public DateTime System_Date_Time { get; set; }
    }
    [NotMapped]
    public class UploadedFilesView
    {
        public Int64 Id { get; set; }
        public DateTime? Upload_Date { get; set; }
        public string User_FileName { get; set; }
        public string FileType { get; set; }
        public string Status { get; set; }
        public int Errors { get; set; }
    }
    [NotMapped]
    public class File_Error_LogsView
    {        
        public string Error_Desc { get; set; }
    }
}