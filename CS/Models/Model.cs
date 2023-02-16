using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class BatchEditRepository
    {

        public static List<GridDataItem> GridData
        {
            get
            {
                var key = "14FAA4331-CF79-4869-9481f-9316AAE81263";
                var Session = HttpContext.Current.Session;
                if (Session[key] == null)
                    Session[key] = Enumerable.Range(0, 100).Select(i => new GridDataItem
                    {
                        ProjectID = i,
                        ProjectGroup = i % 2 == 0 ? "ProjectGroup 1" : "ProjectGroup 2",
                        ProjectCategory = i % 3 == 0 ? "ProjectCategory 1" : "ProjectCategory 2",
                    }).ToList();
                return (List<GridDataItem>)Session[key];
            }
        }

    }

    public class GridDataItem
    {
        public int? ProjectID { get; set; }

        public string ProjectGroup { get; set; }

        public string ProjectCategory { get; set; }
    }


}