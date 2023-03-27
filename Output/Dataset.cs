using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8618
#pragma warning disable IDE1006

namespace Solis.Output
{
    internal class Dataset
    {
        public List<Instructions> dataset { get; set; }
    }

    internal class Instructions
    {
        public string instruction { get; set; }
        public string? input { get; set; }
        public string output { get; set; }
    }
}
