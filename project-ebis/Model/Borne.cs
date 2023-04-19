using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_ebis.Model
{
    public class Borne
    {
        public string NomSecteur { get; set; }
        public string NomStation { get; set; }
        public int IdBorne { get; set; }
        public string DateMiseEnService { get; set; }    
        public string DerniereMaintenance { get; set; } 
        public string TypeCharge { get; set; }  

    }
}
