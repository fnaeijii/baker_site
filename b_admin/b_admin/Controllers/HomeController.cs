using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using b_admin.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Web.Security;
using System.Threading;
using System.Text;

namespace b_admin.Controllers
{
    public class HomeController : Controller
    {
        Naeiji1465_baker context=new Naeiji1465_baker();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult user_management()
        {
            return View();
        }

        public ActionResult orders()
        {
            return View();
        }
        public ActionResult dargah()
        {
            return View();
        }
        public ActionResult getcomm()
        {
            try
            {
                var res = context.tbl_Comm.Where(x => x.Typee != "Q").Select(x => new { x.pkID, x.Name, x.Valuee, x.Typee }).ToList();
                return Json(new { state = true, m = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { state = false, m = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getuser()
        {
            try
            {
                var res = context.tbl_User.Select(x => new { x.pkID, x.Name, x.Phone, x.Status }).ToList();
                return Json(new { state = true, m = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { state = false, m = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getorders()
        {
            try
            {
                var res = context.tbl_order.Where(x=>x.pay==true).Select(x => new { x.pkID, x.number, x.fkUserID, x.pay,x.time,x.dis ,x.send,x.factornumber}).ToList();
                
                return Json(new { state = true, m = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { state = false, m = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public  ActionResult getU(int id)
        {
            var res=context.tbl_User.Where(x=>x.pkID==id).Select(x => new { x.pkID, x.Name, x.Phone, x.Status,x.address }).ToList();
            return Json( res , JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult commtblch(int id, string valuee)
        {
            try
            {
                var res = context.tbl_Comm.Where(x => x.pkID == id).Single();
                res.Valuee = valuee;
                context.SaveChanges();
                return Json(new { state = true, m = valuee }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { state = false, m = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult usertblch(int id, string valuee)
        {
            try
            {
                var res = context.tbl_User.Where(x => x.pkID == id).Single();
                res.Status = valuee;
                context.SaveChanges();
                return Json(new { state = true, m = valuee }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { state = false, m = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult chgimg(int id)

        {

            try
            {




                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var file = System.Web.HttpContext.Current.Request.Files["image"];


                    if (file.ContentLength > 3000000) { return Json(new { state = false, m = "حجم عکس باید کوچکتر از 3 مگا بایت باشد" }, JsonRequestBehavior.AllowGet); }

                    FtpWebRequest ftpRequest =
                   (FtpWebRequest)WebRequest.Create("ftp://irwrs1.dnswebhost.com/httpdocs/baker_img");



                    ftpRequest.Credentials = new NetworkCredential("sysuser_2b0", "$vpjjn1T8y0ZDbl&");
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    ftpRequest.EnableSsl = true;
                    FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());



                    string imgname = "_" + id + "_";
                    string pic = "";
                    string line = streamReader.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains(imgname))
                        {
                            pic = line;
                            line = streamReader.ReadLine();


                            break;
                        }
                        else
                        {
                            line = streamReader.ReadLine();
                        }
                    }

                    streamReader.Close();

                    if (pic != "")
                    {
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://irwrs1.dnswebhost.com/httpdocs/baker_img/" + pic);
                        request.Method = WebRequestMethods.Ftp.DeleteFile;
                        request.Credentials = new NetworkCredential("sysuser_2b0", "$vpjjn1T8y0ZDbl&");

                        using (FtpWebResponse response2 = (FtpWebResponse)request.GetResponse())
                        {
                            pic = response2.StatusDescription;
                        }

                    }





                    string extention = Path.GetExtension(file.FileName).ToLower();

                    var uploadurl = "ftp://irwrs1.dnswebhost.com/httpdocs/baker_img";
                    var uploadfilename = "_" + id + "_(" + DateTime.Now + ")" + extention;
                    uploadfilename = uploadfilename.Replace(@"/", "-");
                    uploadfilename = uploadfilename.Replace(@":", "-");
                    var username = "sysuser_2b0";
                    var password = "$vpjjn1T8y0ZDbl&";
                    Stream streamObj = file.InputStream;
                    byte[] buffer = new byte[file.ContentLength];
                    streamObj.Read(buffer, 0, buffer.Length);
                    streamObj.Close();
                    streamObj = null;
                    string ftpurl = String.Format("{0}/{1}", uploadurl, uploadfilename);
                    var requestObj = FtpWebRequest.Create(ftpurl) as FtpWebRequest;

                    requestObj.Method = WebRequestMethods.Ftp.UploadFile;
                    requestObj.Credentials = new NetworkCredential(username, password);

                    Stream requestStream = requestObj.GetRequestStream();

                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Flush();
                    requestStream.Close();
                    requestObj = null;





                    var res = context.tbl_Comm.Where(x => x.pkID == id).Single();
                    res.Valuee = "http://infinite20.xyz/baker_img/" + uploadfilename;


                    context.SaveChanges();


                    return Json(new { state = true, nimg = uploadfilename, m = "عکس با موفقیت بارگزاری شد" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, m = "عکس یافت نشد" }, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception e)
            {
                return Json(new { state = false, m = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public void send(bool state)
        {
            try
            {
                if (state)
                {
                    var res = context.tbl_Comm.Where(x => x.Name == "ارسال").SingleOrDefault();
                    res.Valuee = "true";
                    context.SaveChanges();
                }
                else
                {
                    var res = context.tbl_Comm.Where(x => x.Name == "ارسال").SingleOrDefault();
                    res.Valuee = "false";
                    context.SaveChanges();
                }
            }catch(Exception e)
            {

            }
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult user_login(string pn, string pas)
        {
            int status = 0;
            var user = context.tbl_Owner.Where(x => x.usename == pn).SingleOrDefault();

            if (user != null)
            {
                if (user.password == pas)
                {
                    
                    var cookieText = Encoding.UTF8.GetBytes(user.pkID.ToString());
                    var encryptedValue = Convert.ToBase64String(cookieText);



                    Response.Cookies["enfinit@tryadmin"].Value = encryptedValue;
                    Response.Cookies["enfinit@tryadmin"].Expires = DateTime.Now.AddDays(500);

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

        public void setdargah(string send,string result,string pin ,string callback)
        {
            var bytes = Convert.FromBase64String(Request.Cookies["enfinit@tryadmin"].Value);
            string result1 = Encoding.UTF8.GetString(bytes);
            int userid = int.Parse(result1);
            var user = context.tbl_Owner.Where(x => x.pkID == userid).SingleOrDefault();

            user.GatewaySend=send;
            user.GatewayResult=result;
            user.pin=pin;
            user.callback=callback;
            context.SaveChanges();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult removeorder(int vid)
        {

            
            var visit = context.tbl_order.Where(x => x.pkID == vid).SingleOrDefault();

            context.tbl_order.Remove(visit);
            context.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}