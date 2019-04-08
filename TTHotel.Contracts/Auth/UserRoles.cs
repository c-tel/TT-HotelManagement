
using System.ComponentModel;

namespace TTHotel.Contracts.Auth
{
    public enum UserRoles
    {
        [Description("Адміністратор")]
        Administrator,
        [Description("Керівник")]
        Head,
        [Description("Прибиральниця")]
        Maid
    }
}
