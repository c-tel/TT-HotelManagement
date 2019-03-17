using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTHohel.UserControls.DropdownAutocomplete
{
    public class DropdownItem
    {
        public string ItemTitle { get; set; }
        public object ItemKey { get; set; }

        public override string ToString()
        {
            return ItemTitle;
        }
    }
}
