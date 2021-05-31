using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class OldUser
    {
     
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string SecretPhrase { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
