using u17004111_HW06.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace u17004111_HW06.Controllers
{
    public class HomeController : Controller
    {
        BikeStoresDb db = new BikeStoresDb();

        public ActionResult Report()
        {
            return View();
        }

        public JsonResult ChartData()
        {
            var data = db.order_items.ToList();

            Chart _chart = new Chart();
            _chart.datasets = new List<Datasets>();
            int[] monthlyQ = new int[12];

            for (int i = 0; i < 12; i ++)
            {
                monthlyQ[i] = db.order_items.Where(x => x.product.category_id == 6 && x.order.order_date.Month == i+1).ToList().Sum(x => x.quantity);
            }

            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Orders by Month",
                data = monthlyQ,
                backgroundColor = new string[] { "#000000" },
                borderColor = new string[] { "#FFFFFF", "#000000", "#FF00000" },
                borderWidth = "1"

            });
            _chart.datasets = _dataSet;

            return Json(_chart, JsonRequestBehavior.AllowGet);
        }
    }
}