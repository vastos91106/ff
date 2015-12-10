
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Common
{
    public class YearValidationAttribute : RangeAttribute
    {
        public YearValidationAttribute(int maximum = int.MaxValue, int minimum = 1884) : base(minimum, maximum)
        {
            if (maximum > DateTime.Now.Year)
            {
                ErrorMessage = "The field Year must be between 1884 and " + DateTime.Now.Year;
            }
        }

        public YearValidationAttribute(double minimum, double maximum) : base(minimum, maximum)
        {
        }

        public YearValidationAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
        {
        }
    }
}
