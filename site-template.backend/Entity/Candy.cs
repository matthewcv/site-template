using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mywebsite.backend.Entity
{
    public class Candy:EntityBase
    {
        private List<CandyColor> _colors;
        public string Name { get; set; }

        public List<CandyColor> Colors
        {
            get { return _colors ?? (_colors = new List<CandyColor>()); }
            set { _colors = value; }
        }


        public int Total { get; set; }

        
    }
}
