using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_ebis.Model
{
    public class Operation
    {
        public DateTime DateDebut { get; set; } 
        public DateTime DateFin { get; set; }   
        public int IdOperation { get; set; }   
        public string TypeCharge { get; set; }
        public int KwHConsomme { get; set; }
        public int IdBorne { get; set; }

    }
}
