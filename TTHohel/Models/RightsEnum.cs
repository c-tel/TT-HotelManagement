using System;

namespace TTHohel.Models
{
    [Flags]
    public enum RightsEnum
    {
        None = 0,
        Settings = 1,
        Statistic = 2,
        All = Settings | Statistic
    }
}
