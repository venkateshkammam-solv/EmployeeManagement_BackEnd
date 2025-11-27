using System;

namespace MyAzureFunctionApp.Shared
{
    public static class ValidationHelper
    {
        public static bool IsDobMatchingAge(DateTime dob, int age)
        {
            var today = DateTime.Today;
            int calculatedAge = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-calculatedAge))
            {
                calculatedAge--;
            }

            return calculatedAge == age;
        }
    }
}
