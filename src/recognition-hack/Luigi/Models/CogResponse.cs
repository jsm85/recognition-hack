using System.Collections.Generic;

namespace Luigi.Models
{
    public class CogResponse
    {
        public string Version { get; set; }
        public List<CogResults> Results { get; set; }
    }
}