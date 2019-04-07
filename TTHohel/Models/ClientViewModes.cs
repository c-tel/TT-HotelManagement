using System;

namespace TTHohel.Models
{
    [Flags]
    public enum ClientViewModes
    {
        Creation = 1,
        Info = 2,
        Editing = 4,
    }
}
