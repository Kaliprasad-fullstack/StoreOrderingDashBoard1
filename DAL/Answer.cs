using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Table("AnswerTbl")]
    public class Answer
    {
        
        public Answer(int Id,string Answers)
        {
            this.Id = Id;
            this.Answers = Answers;
        }
        public Answer()
        {
        }
        [Key]
        public int Id { get; set; }
        public string Answers { get; set; }
        public virtual Question Question { get; set; }
        public virtual Users Users { get; set; }
    }
}
