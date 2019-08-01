using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baza.Models
{
    public class LinksService
    {
        public Links Link { get; set; }
        public IEnumerable<Links> Links { get; set; }
    }
}
