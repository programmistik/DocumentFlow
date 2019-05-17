﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string LangCode { get; set; }
        public string LangCultureCode { get; set; }
        public string LangName { get; set; }
    }
}
