using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Tracker.Helpers
{
    public static class TempDataExtensions
    {
        public static void SetNotification(
            this ITempDataDictionary tempData,
            string message,
            string type = "Error"
        )
        {
            tempData[type] = message;
        }
    }
}
