using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PIFitness.GPX
{
    [Table("RunKeepers")]
    public class GPXEntry
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string Data { get; set; }
        public bool Processed { get; set; }

    }
}
