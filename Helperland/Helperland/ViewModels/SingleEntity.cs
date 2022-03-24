using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class SingleEntity<T> where T : class
    {
        public T Result { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string Url { get; set; }
    }
}
    