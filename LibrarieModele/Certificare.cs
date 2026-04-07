using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Flags]
    public enum Certificare
    {
        Niciuna = 0,
        GMP = 1,
        ISO = 2,
        Bio = 4,
        UE = 8
    }
}
