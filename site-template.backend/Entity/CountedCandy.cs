using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mywebsite.backend.Entity
{
    public class CountedCandy:EntityBase
    {
        public int CandyId { get; set; }

        private List<CandyColor> _colors;

        public List<CandyColor> Colors
        {
            get { return _colors ?? (_colors = new List<CandyColor>()); }
            set { _colors = value; }
        }

        public int ProfileId { get; set; }

        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public Candy Candy { get; set; }

        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public Profile Profile { get; set; }
    }
}
