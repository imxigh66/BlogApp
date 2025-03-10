using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Comments
{
    public class CommentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int? CommentId { get; set; }
    }
}
