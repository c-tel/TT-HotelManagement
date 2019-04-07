using System;

namespace TTHohel.Models
{
    [Flags]
    public enum RightsEnum
    {
        None = 0,
        Settings = 1,
        Statistic = 2,
        ClientDiscount = 4,
        All = Settings | Statistic | ClientDiscount
    }
}
