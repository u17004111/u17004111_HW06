﻿using System.Web;
using System.Web.Mvc;

namespace u17004111_HW06
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
