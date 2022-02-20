using System;
using System.Collections.Generic;

namespace Helperland.Models
{
    public partial class City
    {
        public City()
        {
            Zipcode = new HashSet<Zipcode>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Zipcode> Zipcode { get; set; }
    }
}
