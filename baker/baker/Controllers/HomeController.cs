using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using baker.Models;
using System.Globalization;
using System.Web.Security;
using System.Net;

namespace baker.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Bdata context = new Bdata();

        public ActionResult Index()
        {
            var res = context.tbl_Comm.ToList();
            ViewBag.title = res.Where(x => x.Name == "تیتر اصلی").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.banerimg = res.Where(x => x.Name == "عکس بنر").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.address = res.Where(x => x.Name == "نشانی").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.phone = res.Where(x => x.Name == "شماره تماس").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.email = res.Where(x => x.Name == "ایمیل").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.dis = res.Where(x => x.Name == "توضیحات").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.Nname = res.Where(x => x.Name == "اسم نان").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.Nprice = res.Where(x => x.Name == "قیمت نان").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.Nimg = res.Where(x => x.Name == "تصویر نان").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.Ndis = res.Where(x => x.Name == "توضیح نان").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.Psend = res.Where(x => x.Name == "هزینه ارسال").Select(x => x.Valuee).SingleOrDefault();



            ViewBag.sattime = res.Where(x => x.pkID == 12).Select(x => x.Valuee).SingleOrDefault();

            ViewBag.suntime = res.Where(x => x.pkID == 13).Select(x => x.Valuee).SingleOrDefault();

            ViewBag.montime = res.Where(x => x.pkID == 14).Select(x => x.Valuee).SingleOrDefault();

            ViewBag.tuetime = res.Where(x => x.pkID == 15).Select(x => x.Valuee).SingleOrDefault();

            ViewBag.wentime = res.Where(x => x.pkID == 16).Select(x => x.Valuee).SingleOrDefault();

            ViewBag.thutime = res.Where(x => x.pkID == 17).Select(x => x.Valuee).SingleOrDefault();

            ViewBag.fritime = res.Where(x => x.pkID == 18).Select(x => x.Valuee).SingleOrDefault();
            return View();
        }
        public ActionResult login()
        {
            var res = context.tbl_Comm.ToList();
          

            ViewBag.banerimg = res.Where(x => x.Name == "عکس بنر").Select(x => x.Valuee).SingleOrDefault();

            return View();
        }
        public ActionResult user_account()
        {
            var res = context.tbl_Comm.ToList();
            ViewBag.title = res.Where(x => x.Name == "تیتر اصلی").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.banerimg = res.Where(x => x.Name == "عکس بنر").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.address = res.Where(x => x.Name == "نشانی").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.phone = res.Where(x => x.Name == "شماره تماس").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.email = res.Where(x => x.Name == "ایمیل").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.Psend = res.Where(x => x.Name == "*هزینه ارسال").Select(x => x.Valuee).SingleOrDefault();

            ViewBag.send = res.Where(x => x.Name == "ارسال").Select(x => x.Valuee).SingleOrDefault();

            if (Request.Cookies["enfinit@try1"] != null) { 
            var bytes = Convert.FromBase64String(Request.Cookies["enfinit@try1"].Value);
            string result = Encoding.UTF8.GetString(bytes);
            int userid = int.Parse(result);
            var user = context.tbl_User.Where(x => x.pkID == userid).SingleOrDefault();

            ViewBag.name = user.Name;
            ViewBag.inventory = user.inventory;
            ViewBag.useraddress = user.address;
            ViewBag.status = user.Status;
        }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult user_login(string pn, string pas)
        {
            //Session.Timeout = 30;
            int status = 0;
            var user = context.tbl_User.Where(x => x.Phone == pn).SingleOrDefault();

            if (user != null)
            {
                if (user.Password == pas)
                {
                    //Session["userid"] = user.pkID;
                    var cookieText = Encoding.UTF8.GetBytes(user.pkID.ToString());
                    var encryptedValue = Convert.ToBase64String(cookieText);



                    Response.Cookies["enfinit@try1"].Value = encryptedValue;
                    Response.Cookies["enfinit@try1"].Expires = DateTime.Now.AddDays(500);
                   
                    status = 1; //login ok
                }
                else
                {
                    status = 2; //wrong pass


                }


            }

            else
            {
                status = 3; //dosent match pn

            }


            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult user_signup(string namee, string phone,string address, string pass,string pass2)
        {

            int statee = 0;
            if (namee == "" || phone == "" || pass == "" || pass2 == "")
            {
                statee = 3;
            }
            else
            {
                if (pass == pass2)
                {


                    var p = context.tbl_User.Where(x => x.Phone == phone).SingleOrDefault();
                    if (p == null)
                    {
                        tbl_User newU = new tbl_User();
                        newU.Name = namee;
                        newU.Phone = phone;
                        newU.address = address;
                        newU.Password = pass;
                        newU.Status = "عادی";
                        context.tbl_User.Add(newU);
                        context.SaveChanges();
                        var cookieText = Encoding.UTF8.GetBytes(newU.pkID.ToString());
                        var encryptedValue = Convert.ToBase64String(cookieText);



                        Response.Cookies["enfinit@try1"].Value = encryptedValue;
                        Response.Cookies["enfinit@try1"].Expires = DateTime.Now.AddDays(500);

                        statee = 2;

                    }
                    else
                    {
                        statee = 1;
                    }

                    context.SaveChanges();

                }
            }
            return Json(statee, JsonRequestBehavior.AllowGet);

        }
        public void logout()
        {
            Response.Cookies["enfinit@try1"].Expires = DateTime.Now.AddDays(-1);
          
            Response.Redirect("/Home/index");
        }

        public ActionResult checksend()
        {
            var res = context.tbl_Comm.Where(x => x.Name == "ارسال").Select(x => x.Valuee).SingleOrDefault();
            var Psend = context.tbl_Comm.Where(x => x.Name == "*هزینه ارسال").Select(x => x.Valuee).SingleOrDefault();
           
                var bytes = Convert.FromBase64String(Request.Cookies["enfinit@try1"].Value);
                string result = Encoding.UTF8.GetString(bytes);
                int userid = int.Parse(result);
                var user = context.tbl_User.Where(x => x.pkID == userid).SingleOrDefault();

                return Json(new { Psend = Psend, useraddress = user.address, res = res }, JsonRequestBehavior.AllowGet);
             
        }
        public ActionResult checksend2()
        {
            var res = context.tbl_Comm.Where(x => x.Name == "ارسال").Select(x => x.Valuee).SingleOrDefault();
            var Psend = context.tbl_Comm.Where(x => x.Name == "*هزینه ارسال").Select(x => x.Valuee).SingleOrDefault();

            return Json(new { Psend = Psend, res = res }, JsonRequestBehavior.AllowGet);


        }

        public void changeaddress(string address)
        {
           
            var bytes = Convert.FromBase64String(Request.Cookies["enfinit@try1"].Value);
            string result = Encoding.UTF8.GetString(bytes);
            int userid = int.Parse(result);
            var user = context.tbl_User.Where(x => x.pkID == userid).SingleOrDefault();

            user.address = address;
            context.SaveChanges();
        }
        public ActionResult getfactor(int factor,bool status)
        {
            var price= int.Parse(context.tbl_Comm.Where(x => x.Name == "قیمت نان").Select(x => x.Valuee).SingleOrDefault());
          
             price *=factor;
            if (status == true)
            {
                var Psend = int.Parse(context.tbl_Comm.Where(x => x.Name == "*هزینه ارسال").Select(x => x.Valuee).SingleOrDefault());
                price += Psend;
            }
           
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        public void inventory(int price)
        {
            var bytes = Convert.FromBase64String(Request.Cookies["enfinit@try1"].Value);
            string result = Encoding.UTF8.GetString(bytes);
            int userid = int.Parse(result);
            var user = context.tbl_User.Where(x => x.pkID == userid).SingleOrDefault();


            user.inventory += price;
            context.SaveChanges();

        }


        public ActionResult invoice(int factor,string time,string dis,int price ,bool send)
        {
            var factornumber=0;
            try
            {
                factornumber = context.tbl_order.Max(x => x.factornumber);
               
            }
           catch(Exception e)
            {
                
            }
            factornumber++;
            var bytes = Convert.FromBase64String(Request.Cookies["enfinit@try1"].Value);
            string result = Encoding.UTF8.GetString(bytes);
            int userid = int.Parse(result);
            var user = context.tbl_User.Where(x => x.pkID == userid).SingleOrDefault();

          

            tbl_order ni = new tbl_order();
            if (user.inventory >= price)
            {
                user.inventory -= price;
                context.SaveChanges();
                ni.pay = true;
            }
            else
            {
                ni.pay = false;
            }
                ni.fkUserID = userid;
                ni.price = price;
                ni.time = time;
                ni.number = factor;
                ni.send = send;
                ni.transid = "0";
                ni.trackingid = "0";
                ni.factornumber = factornumber;
            context.tbl_order.Add(ni);
            context.SaveChanges();
        
          
            return Json(new {user=user,pay=ni.pay,  fn = factornumber, price = price }, JsonRequestBehavior.AllowGet);

        }
      

    }
}