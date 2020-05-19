using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace online_Admission.Models
{
    public class studentviewmodel
    {

        public int r_id { get; set; }
        [Required(ErrorMessage = "*")]
        public string r_cnic { get; set; }
        [Required(ErrorMessage = "*")]

        public string r_password { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$")]

        public string r_fullname { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^[a-zA-Z\s]+$")]

        public string r_fathername { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^[a-zA-Z\s]+$")]

        public string r_mobile { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^([01]|\+88)?\d{11}")]

        public string r_phone { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^([01]|\+88)?\d{11}")]

        public double r_hsc_gpa { get; set; }
        [Required(ErrorMessage = "*")]

        public double r_ssc_gpa { get; set; }
        [Required(ErrorMessage = "*")]

        public int r_status { get; set; }
        [Required(ErrorMessage = "*")]

        public System.DateTime r_date { get; set; }
        [Required(ErrorMessage = "*")]

        public string r_image { get; set; }
        [Required(ErrorMessage = "*")]

        public int r_p1 { get; set; }
        [Required(ErrorMessage = "*")]

        public int r_p2 { get; set; }
        [Required(ErrorMessage = "*")]

        public int r_p3 { get; set; }
        [Required(ErrorMessage = "*")]

        public int r_cat { get; set; }
        [Required(ErrorMessage = "*")]

        public string SSC_marksheet { get; set; }
        [Required(ErrorMessage = "*")]

        public string HSC_marksheet { get; set; }

        public int dept_id { get; set; }
        public string dept_name { get; set; }
        public string category_name { get; set; }
        public int m_id { get; set; }
        public Nullable<int> m_applicant_id { get; set; }
        public Nullable<int> m_dept_id { get; set; }
        public Nullable<int> m_appstatus_id { get; set; }

        public string status_name { get; set; }

    
    }
}