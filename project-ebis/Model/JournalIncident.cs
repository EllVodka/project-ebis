﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_ebis.Model
{
    public class JournalIncident
    {
        public int IdBorne { get; set; }
        public string TypeIncident { get; set; }
        public string DateIncident { get; set; }
        public string DetailIncident { get; set; }
    }
}
