using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eSya.ManageRates.WebAPI.Utility
{
    public class CustomException
    {

        public static string GetExceptionMessage(Exception ex)
        {
            string exMsg = ex.ToString();
            if (ex.InnerException != null)
                exMsg = ex.InnerException.Message;
            if (ex.Message != null)
                exMsg = ex.Message;
            return exMsg;
        }
    }
}
