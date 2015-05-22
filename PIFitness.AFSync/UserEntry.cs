using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PIFitness.AFSync
{
    [Table("AspNetUsers")]
    public class UserEntry
    {
        [Key]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FitbitAuthToken { get; set; }
        public string FitbitAuthTokenSecret { get; set; }
        public string FitbitUserId { get; set; }
        public string UserName { get; set; }
    }
}
