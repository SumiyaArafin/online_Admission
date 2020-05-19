using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using online_Admission.Models;
using System.IO;

namespace online_Admission.Controllers
{
    public class AdminController : Controller
    {
        private online_admissionEntities7 db = new online_admissionEntities7();



        // GET: Admin
        public ActionResult Index()
        {
            adminviewmodel svm = new adminviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {

                int id = Convert.ToInt32(Session["id"]);
                admin_registration s = db.admin_registration.Where(x => x.r_id == id).SingleOrDefault();
                //ViewBag.name = s.r_fullname;
                

            }

            return View(svm);



            

        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]

        public ActionResult Create(adminviewmodel svm, HttpPostedFileBase imgfile)
        {
            
            
            
            
            string imgpath = uploadimgfile(imgfile);
            if (imgpath.Equals("-1"))
            {
                Response.Write("Some Error");
                return View("index");

            }
            else
            {



                admin_registration s = new admin_registration();

                s.r_cnic = svm.r_cnic;
                s.r_password = svm.r_password;
                s.r_fullname = svm.r_fullname;

                s.r_mobile = svm.r_mobile;
                s.r_date = System.DateTime.Now;
                s.r_email = svm.r_email;
                s.r_image = imgpath;

                db.admin_registration.Add(s);
                db.SaveChanges();


                TempData["name"] = s.r_fullname;
                TempData.Keep();


            }
            return View("successpage");
        }

        public string uploadimgfile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)

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




        public ActionResult successpage()
        {
            return View();

        }

        public ActionResult login()
        {
            if (Session["id"] != null)
            {
                return RedirectToAction("adminpannel");
            }

            return View();

        }
        [HttpPost]
        public ActionResult login(adminviewmodel svm)
        {
            admin_registration s = db.admin_registration.Where(x => x.r_email == svm.r_email && x.r_password == svm.r_password).SingleOrDefault();
            if (s != null)
            {
                Session["id"] = s.r_id;
                return RedirectToAction("adminpannel");

            }
            else
            {
                ViewBag.msg = "No Records were found";

            }
            return View();

        }

        [HttpGet]

        public ActionResult profile()
        {
            adminviewmodel svm = new adminviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {
                int id = Convert.ToInt32(Session["id"]);
                admin_registration s = db.admin_registration.Where(x => x.r_id == id).SingleOrDefault();
                //ViewBag.name = s.r_fullname;
                svm.r_fullname = s.r_fullname;
                svm.r_cnic = s.r_cnic;
                svm.r_image = s.r_image;
                svm.r_mobile = s.r_mobile;
                svm.r_date = s.r_date;
                svm.r_id = s.r_id;
                svm.r_email = s.r_email;
            }

            return View(svm);
        }














        public ActionResult afterlogin()
        {
            adminviewmodel svm = new adminviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {

                var student_registration = db.student_registration.Include(s => s.category).Include(s => s.department).Include(s => s.department1).Include(s => s.department2);
                return View(student_registration.ToList());
            }
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student_registration student_registration = db.student_registration.Find(id);
            if (student_registration == null)
            {
                return HttpNotFound();
            }
            return View(student_registration);
        }

        public ActionResult logout()
        {
            adminviewmodel svm = new adminviewmodel();

            Session["id"] = null;
            return RedirectToAction("login");
        }
        public ActionResult adminpannel()
        {

            adminviewmodel svm = new adminviewmodel();

            if (Session["id"] == null)
            {
                return RedirectToAction("login");

            }
            else
            {
                int id = Convert.ToInt32(Session["id"]);
                admin_registration s = db.admin_registration.Where(x => x.r_id == id).SingleOrDefault();


                ViewBag.applicant = db.student_registration.Count(x => x.r_status == 0 || x.r_status == 1 || x.r_status == -1);
                ViewBag.accept = db.student_registration.Count(x => x.r_status == 1);
                ViewBag.rejected = db.student_registration.Count(x => x.r_status == -1);
                ViewBag.pending = db.student_registration.Count(x => x.r_status == 0);
                List<student_registration> li = db.student_registration.Where(x => x.r_status == 0).ToList();
                List<studentviewmodel> sxmlist = li.Select(x => new studentviewmodel
                {

                    r_fullname = x.r_fullname,
                    r_image = x.r_image,
                    r_cat = x.r_cat,
                    r_id = x.r_id


                }).ToList();
                ViewBag.notification = sxmlist;


                li = db.student_registration.Where(x => x.r_status == 1).ToList();


                sxmlist = li.Select(x => new studentviewmodel
                {

                    r_fullname = x.r_fullname,
                    r_id = x.r_id,
                    r_image = x.r_image,
                    category_name = x.category.category_name,
                    r_status = x.r_status,



                }).ToList();







                return View(sxmlist);
            }


            return View(svm);

        }
        // GET: Admin/Create
        //public ActionResult Create()
        //{
        //    ViewBag.r_cat = new SelectList(db.categories, "category_id", "category_name");
        //    ViewBag.r_p1 = new SelectList(db.departments, "dept_id", "dept_name");
        //    ViewBag.r_p2 = new SelectList(db.departments, "dept_id", "dept_name");
        //    ViewBag.r_p3 = new SelectList(db.departments, "dept_id", "dept_name");
        //    return View();
        //}

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public ActionResult Create([Bind(Include = "r_id,r_cnic,r_password,r_fullname,r_fathername,r_mobile,r_phone,r_hsc_gpa,r_ssc_gpa,r_status,r_date,r_image,r_p1,r_p2,r_p3,r_cat,SSC_marksheet,HSC_marksheet")] student_registration student_registration)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.student_registration.Add(student_registration);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.r_cat = new SelectList(db.categories, "category_id", "category_name", student_registration.r_cat);
        //    ViewBag.r_p1 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p1);
        //    ViewBag.r_p2 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p2);
        //    ViewBag.r_p3 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p3);
        //    return View(student_registration);
        //}

        //// GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student_registration student_registration = db.student_registration.Find(id);
            if (student_registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.r_cat = new SelectList(db.categories, "category_id", "category_name", student_registration.r_cat);
            ViewBag.r_p1 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p1);
            ViewBag.r_p2 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p2);
            ViewBag.r_p3 = new SelectList(db.departments, "dept_id", "dept_name", student_registration.r_p3);
            return View(student_registration);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(student_registration svm)
        {
            student_registration s = db.student_registration.Where(x=>x.r_id==svm.r_id).SingleOrDefault();
            s.r_cnic = svm.r_cnic;
            s.r_password = svm.r_password;
            s.r_fullname = svm.r_fullname;
            s.r_fathername = svm.r_fathername;
            s.r_mobile = svm.r_mobile;
            s.r_phone = svm.r_phone;
            s.r_hsc_gpa = svm.r_hsc_gpa;
            s.r_ssc_gpa = svm.r_ssc_gpa;
            s.r_cat = svm.r_cat;

            s.r_p1 = svm.r_p1;
            s.r_p2 = svm.r_p2;

            s.r_p3 = svm.r_p3;
            s.r_status = 0;
            s.r_date = System.DateTime.Now;


            db.SaveChanges();
            return RedirectToAction("adminpannel");
        }


        public ActionResult Delete_m(int? id)
        {


            Merit m = db.Merits.Where(x => x.m_applicant_id == id).SingleOrDefault();
            db.Merits.Remove(m);
            db.SaveChanges();

            return RedirectToAction("Meritlist");
        }

        // POST: Admin/Delete/5







        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            student_registration t = db.student_registration.Where(x => x.r_id == id).SingleOrDefault();
            Merit m = db.Merits.Where(x => x.m_applicant_id == id).SingleOrDefault();
            //Merit m = db.Merits.Where(x => x.m_applicant_id == id).SingleOrDefault();
            db.student_registration.Remove(t);
            if (m != null)
            {
                db.Merits.Remove(m);
            }

            db.SaveChanges();
            return RedirectToAction("afterlogin");
        }

        // POST: Admin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, student_registration d, Merit mi)
        //{
        //    student_registration t = db.student_registration.Where(x => x.r_id == id).SingleOrDefault();
        //    Merit m = db.Merits.Where(x => x.m_applicant_id == id).SingleOrDefault();
        //    db.student_registration.Remove(t);
        //    db.Merits.Remove(m);

        //    db.SaveChanges();
        //    return RedirectToAction("afterlogin");
        //}
        //[ValidateAntiForgeryToken]

        //[HttpGet]
        public ActionResult Result(studentviewmodel svm)
        { int i = 0;
            List<department> list = db.departments.ToList();
            ViewBag.departmentList = new SelectList(list, "dept_id", "dept_name");
            List<student_registration> li = db.student_registration.Where(x => x.r_p1 == svm.r_p1 || x.r_p2 == svm.r_p1 || x.r_p3 == svm.r_p1 && x.r_status == 1).ToList();
            var studentformeritlist = db.Merits.ToList();
            List<student_registration> filteredstudents = new List<student_registration>();
            int f;
            foreach(student_registration item in li)
            {
                f = 0;
                foreach(var item2 in studentformeritlist)
                {
                    if (item.r_id == item2.m_applicant_id)
                    {
                        f = 1;
                        break;

                    }
                }
                if (f == 0)
                {
                    filteredstudents.Add(item);
                }

            }
            var li2 = filteredstudents.OrderByDescending(x => (x.r_ssc_gpa + x.r_hsc_gpa)).Take(2).ToList();
            foreach (var it in li2)
            {
                i++;
            Merit m = new Merit();
                m.m_applicant_id = it.r_id;
                m.m_dept_id = svm.r_p1;
                m.m_appstatus_id = 1;
                db.Merits.Add(m);
                db.SaveChanges();

            }
            if (i > 0)
            {
                Response.Write("<script>alert('Merit List is genereted successfully')</script>");
                return RedirectToAction("Meritlist");

            }

            return View();
        }
        public ActionResult Meritlist()
        {
            adminviewmodel svm = new adminviewmodel();

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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
