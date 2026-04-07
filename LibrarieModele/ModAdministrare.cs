using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    [Flags]
    public enum ModAdministrare
    {
        Niciuna = 0,
        Oral = 1,
        Injectabil = 2,
        Topic = 4,
        Inhalator = 8
    }
}
