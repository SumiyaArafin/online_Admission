using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace online_Admission.Models
{
    public class StudentComparer: IEqualityComparer<student_registration>
    {
        public bool Equals(student_registration x, Merit y)
        {
            if (x.r_id == y.m_applicant_id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Equals(student_registration x, student_registration y)
        {
            throw new NotImplementedException();
        }

        public int GetHashcode(student_registration obj)
        {
            return obj.r_id.GetHashCode();
        }

        public int GetHashCode(student_registration obj)
        {
            throw new NotImplementedException();
        }
    }
}