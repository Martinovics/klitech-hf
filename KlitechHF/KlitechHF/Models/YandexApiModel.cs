using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlitechHF.Models
{
    public class YandexApiLookupModel
    {
        public Head Head { get; set; }
        public Def[] Def { get; set; }
    }

    public class Head  // empty
    {
    }

    public class Def
    {
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Ts { get; set; }
        public Tr[] Tr { get; set; }
    }

    public class Tr
    {
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Gen { get; set; }
        public int Fr { get; set; }
        public Syn[] Syn { get; set; }
        public Mean[] Mean { get; set; }
    }

    public class Syn
    {
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Gen { get; set; }
        public int Fr { get; set; }
    }

    public class Mean
    {
        public string Text { get; set; }
    }

}
