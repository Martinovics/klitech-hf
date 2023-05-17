using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlitechHF.Models
{
    public class Synonyms
    {
        public Response[] Response { get; set; }
    }

    public class Response
    {
        public List List { get; set; }
    }

    public class List
    {
        public string Category { get; set; }
        public string Synonyms { get; set; }
    }
}
