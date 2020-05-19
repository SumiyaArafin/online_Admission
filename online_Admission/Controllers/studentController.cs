using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using online_Admission.Models;
using System.IO;
using System.Net;
using System.Data.Entity;


namespace online_Admission.Controllers
{            
    public class studentController : Controller
    {
        private online_admissionEntities7 db = new online_admissionEntities7();


        // GET: student
        public ActionResult Index()
        {

            List<department> list = db.departments.ToList();
            ViewBag.departmentList = new SelectList(list, "dept_id", "dept_name");

            List<category> list2 = db.categories.ToList();
            ViewBag.categoryList = new SelectList(list2, "category_id", "category_name");


            return View();
        }
        [HttpPost]
        public ActionResult Create(studentviewmodel svm, HttpPostedFileBase imgfile, HttpPostedFileBase HSCfile, HttpPostedFileBase SSCfile)
        {

            List<department> list = db.departments.ToList();
            ViewBag.departmentList = new SelectList(list, "dept_id", "dept_name");

            List<category> list2 = db.categories.ToList();
            ViewBag.categoryList = new SelectList(list2, "category_id", "category_name");


            string imgpath= uploadimgfile(imgfile);
            string hscpath = uploadfile(HSCfile);
            string sscpath = uploadfile(SSCfile);

            if (imgpath.Equals("-1")|| hscpath.Equals("-1")|| sscpath.Equals("-1"))
            {
                Response.Write("Some Error");
                return View("index");

            }
            else
            {
                student_registration s = new student_registration();


                s.r_cnic = svm.r_cnic;
                s.r_password = svm.r_password;
                s.r_fullname = svm.r_fullname;
                s.r_fathername = svm.r_fathername;
                s.r_mobile = svm.r_mobile;
                s.r_phone = svm.r_phone;
                s.r_hsc_gpa = svm.r_hsc_gpa;
                s.r_ssc_gpa = svm.r_ssc_gpa;
                s.r_cat = svm.r_cat;
                s.r_image = imgpath;
                s.SSC_marksheet = sscpath;

                s.HSC_marksheet = hscpath;

                s.r_p1 = svm.r_p1;
                s.r_p2 = svm.r_p2;

                s.r_p3 = svm.r_p3;
                s.r_status = 0;
                s.r_date = System.DateTime.Now;

                db.student_registration.Add(s);
                db.SaveChanges();


                TempData["name"] = s.r_fullname;
                TempData.Keep();


            }
            return View("successpage");
        }



        public ActionResult successpage()
        {
            return View();

        }

        [HttpGet]

        public ActionResult profile()
        {
            studentviewmodel svm = new studentviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {
                int id = Convert.ToInt32(Session["id"]);
                student_registration s = db.student_registration.Where(x => x.r_id ==id ).SingleOrDefault();
                ViewBag.name = s.r_fullname;
                if (s.r_status == 0)
                {
                    ViewBag.msg = "Applied";
                }
                else if (s.r_status == 1)
                {
                    ViewBag.msg = "Approved";

                }
                else
                {
                    ViewBag.msg = "Rejected";

                }

                svm.r_fullname = s.r_fullname;
                svm.r_fathername = s.r_fathername;
                svm.r_cnic = s.r_cnic;
                svm.r_image = s.r_image;
                svm.r_mobile = s.r_mobile;
                svm.r_hsc_gpa = s.r_hsc_gpa;
                svm.r_ssc_gpa = s.r_ssc_gpa;
                svm.r_date = s.r_date;
                svm.SSC_marksheet = s.SSC_marksheet;
                svm.HSC_marksheet = s.HSC_marksheet;
                svm.r_p1 = s.r_p1;
                svm.r_p2 = s.r_p2;
                svm.r_p3 = s.r_p3;
                svm.r_id = s.r_id;
            }
            
            return View(svm);

        }

        public ActionResult login()
        {
            if (Session["id"] !=null)
            {
                return RedirectToAction("profile");
            }

                return View();

        }
        [HttpPost]
        public ActionResult login(studentviewmodel svm)
        {
            student_registration s = db.student_registration.Where(x => x.r_cnic == svm.r_cnic && x.r_password == svm.r_password).SingleOrDefault();
            if (s != null)
            {
                Session["id"] = s.r_id;
                return RedirectToAction("profile");

            }
            else
            {
                ViewBag.msg = "No Records were found";

            }
            return View();

        }

        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if(file!=null && file.ContentLength > 0)

            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);


                    }
                    catch (Exception Ex)
                    {
                        path = "-1";

                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg, jpeg, png formats are allowed'); </script>");
                }

            }
            else
            {
                Response.Write("<script>alert('please select a file'); </script>");
            }
            return path;
        }


        public string uploadfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)

            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".pdf"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);


                    }
                    catch (Exception Ex)
                    {
                        path = "-1";

                    }
                }
                else
                {
                    Response.Write("<script>alert('Only pdf formats are allowed'); </script>");
                }

            }
            else
            {
                Response.Write("<script>alert('please select a file'); </script>");
            }
            return path;
        }



        public ActionResult Meritlist()
        {
            studentviewmodel svm = new studentviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {

                List<Merit> li = db.Merits.ToList();




                return View(li);
            }
        }

        public ActionResult logout()
        {
            adminviewmodel svm = new adminviewmodel();

            Session["id"] = null;
            return RedirectToAction("login");
        }


        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult edit()
        {
            List<department> list = db.departments.ToList();
            ViewBag.departmentList = new SelectList(list, "dept_id", "dept_name");

            List<category> list2 = db.categories.ToList();
            ViewBag.categoryList = new SelectList(list2, "category_id", "category_name");

            studentviewmodel svm = new studentviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {
                int id = Convert.ToInt32(Session["id"]);
                student_registration s = db.student_registration.Where(x => x.r_id == id).SingleOrDefault();
                ViewBag.name = s.r_fullname;
                if (s.r_status == 0)
                {
                    ViewBag.msg = "Applied";
                }
                else if (s.r_status == 1)
                {
                    ViewBag.msg = "Approved";

                }
                else
                {
                    ViewBag.msg = "Rejected";

                }

                svm.r_fullname = s.r_fullname;
                svm.r_password = s.r_password;
                svm.r_fathername = s.r_fathername;
                svm.r_cnic = s.r_cnic;
                svm.r_image = s.r_image;
                svm.r_mobile = s.r_mobile;
                svm.r_hsc_gpa = s.r_hsc_gpa;
                svm.r_ssc_gpa = s.r_ssc_gpa;
                svm.r_mobile = s.r_mobile;
                svm.r_phone = s.r_phone;
                svm.r_date = s.r_date;
                svm.SSC_marksheet = s.SSC_marksheet;
                svm.HSC_marksheet = s.HSC_marksheet;
                svm.r_p1 = s.r_p1;
                svm.r_p2 = s.r_p2;
                svm.r_p3 = s.r_p3;
                svm.r_id = s.r_id;
                svm.r_cat = s.r_cat;
            }

            return View(svm);
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //student_registration student_registration = db.student_registration.Find(id);
            //if (student_registration == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.r_cat = new SelectList(db.categories, "category_id", "category_name", student_registration.r_cat);
            //ViewBag.r_p1 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p1);
            //ViewBag.r_p2 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p2);
            //ViewBag.r_p3 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p3);
            //return View(student_registration);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult edit(student_registration svm)
        //{
        //    student_registration s = db.student_registration.Where(x => x.r_cnic == svm.r_cnic).SingleOrDefault();
        //    s.r_status = svm.r_status;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public ActionResult edit(studentviewmodel svm, HttpPostedFileBase imgfile, HttpPostedFileBase HSCfile, HttpPostedFileBase SSCfile)
        {
            List<department> list = db.departments.ToList();
            ViewBag.departmentList = new SelectList(list, "dept_id", "dept_name");

            List<category> list2 = db.categories.ToList();
            ViewBag.categoryList = new SelectList(list2, "category_id", "category_name");


            string imgpath = uploadimgfile(imgfile);
            string hscpath = uploadfile(HSCfile);
            string sscpath = uploadfile(SSCfile);

            if (imgpath.Equals("-1") || hscpath.Equals("-1") || sscpath.Equals("-1"))
            {
                Response.Write("Some Error");
                return View("edit");

            }
            else
            {
                int id = Convert.ToInt32(Session["id"]);
                student_registration s = db.student_registration.Where(x=>x.r_id==id).SingleOrDefault();

                s.r_cnic = svm.r_cnic;
                s.r_password = svm.r_password;
                s.r_fullname = svm.r_fullname;
                s.r_fathername = svm.r_fathername;
                s.r_mobile = svm.r_mobile;
                s.r_phone = svm.r_phone;
                s.r_hsc_gpa = svm.r_hsc_gpa;
                s.r_ssc_gpa = svm.r_ssc_gpa;
                s.r_cat = svm.r_cat;
                s.r_image = imgpath;
                s.SSC_marksheet = sscpath;

                s.HSC_marksheet = hscpath;

                s.r_p1 = svm.r_p1;
                s.r_p2 = svm.r_p2;

                s.r_p3 = svm.r_p3;
                s.r_status = 0;
                s.r_id = svm.r_id;
                db.SaveChanges();




            }
            return RedirectToAction("login");

        }






    }
}