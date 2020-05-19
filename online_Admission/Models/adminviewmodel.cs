using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_Admission.Models
{
    public class adminviewmodel
    {
        public int r_id { get; set; }
        public string r_email { get; set; }
        public string r_cnic { get; set; }
        public string r_password { get; set; }
        public string r_fullname { get; set; }
        public string r_mobile { get; set; }
        public System.DateTime r_date { get; set; }
        public string r_image { get; set; }

    }
}