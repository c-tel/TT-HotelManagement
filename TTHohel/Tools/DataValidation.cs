using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TTHohel.Tools
{

    public static class DataValidation
    {
        private static Regex phoneValid = new Regex(@"^\+380([0-9]){9}$");
        private static Regex passportValid = new Regex(@"^([а-яА-Я]){2}([0-9]){6}$");

        public static bool ValidateTelNum(string num)
        {
            if (phoneValid.IsMatch(num))
                return true;
            return false;
        }

        public static bool ValidatePassport(string passport)
        {
            if (passportValid.IsMatch(passport))
                return true;
            return false;
        }

    }

    public enum AddResult
    {
        [Description("Створено.")]
        Success,
        [Description("Щось пішло не так...")]
        Error,
        [Description("Не вдалося створити. Такий запис вже є.")]
        AlreadyCreated,
        [Description("Перевірте правильність вводу!")]
        InvalidInput
    }

    public enum PayResult
    {
        [Description("Оплату додано!")]
        Success,
        [Description("Щось пішло не так...")]
        Error,
        [Description("Забагато! :)")]
        TooMuch,
        [Description("Перевірте правильність вводу!")]
        InvalidInput
    }
}
