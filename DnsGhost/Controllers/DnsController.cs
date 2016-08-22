using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DnsGhost.Models;
using Microsoft.Ajax.Utilities;

namespace DnsGhost.Controllers
{
    public class DnsController : Controller
    {
        private DnsContext db = new DnsContext();

        //
        // GET: /Dns/

        public ActionResult Index()
        {
            return View();//db.Entries.ToList());
            //return new ContentResult() { Content = "use the http://ownmeca0.w15.wh-2.com/DnsGhost/dns/retrieve/username/computername" };
        }

        public ActionResult Admin()
        {
            return View(db.Entries.ToList());
        }


        public ActionResult RetrieveAll(string user)
        {
            var sql = string.Format("select DnsEntries.* from DnsEntries join Users on Users.Id = DnsEntries.User_Id and Users.Name = '{0}'", user);

            if (!db.Entries.SqlQuery(sql).Any())
            {
                return HttpNotFound();
            }

            var sb = new StringBuilder();
            foreach (var dnsentry in db.Entries.SqlQuery(sql))
            {
                sb.AppendLine(string.Format("<div>{0} {1}</div>",dnsentry.Token, dnsentry.IpValue));
            }

            return new ContentResult() { Content = sb.ToString() };  
        }

        public ActionResult Retrieve(string user, string token)
        {
            if (token == "all")
                return RetrieveAll(user);
            var sql = string.Format("select DnsEntries.* from DnsEntries join Users on Users.Id = DnsEntries.User_Id and Users.Name = '{0}' where Token = '{1}'",user, token);
            DnsEntry dnsentry = db.Entries.SqlQuery(sql).FirstOrDefault();
            if (dnsentry == null)
            {
                return HttpNotFound();
            }
            return new ContentResult() { Content = dnsentry.IpValue };
        }

        private string ClientIpAddress()
        {
            string strIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                                  Request.ServerVariables["REMOTE_ADDR"];
            return strIpAddress;
        }

        string response = @"<!DOCTYPE html>
<html>
<body>
<h3>Current address of {2} has been stored for computer {1} on behalf of user {0}</h3>
<div>Later on, to obtain this IP address please visit <a href=""http://ownmeca0.w15.wh-2.com/DnsGhost/dns/retrieve/{0}/{1}"">http://ownmeca0.w15.wh-2.com/DnsGhost/dns/retrieve/{0}/{1}</a></div>
You can automate this process by installing the <a href=""http://ownmeca0.w15.wh-2.com/dnsGhost.zip"">DnsGhost</a> service on your PC
<div/>
<div/>
<div>To insure you the program is not mailitious the source code is included.</div>
<div>After unzipping the file don't forget to change the .config file, instructions included inside!!!</div>
</body>
</html>
";


        public ActionResult Update(string user, string token)
        {
            User usr = db.Users.SqlQuery(string.Format("select * from Users where Name = '{0}'", user)).FirstOrDefault();
            if (usr == null)
            {
                return HttpNotFound();
            }

            var ipAddress = ClientIpAddress();

            var sql = string.Format("select DnsEntries.* from DnsEntries join Users on Users.Id = DnsEntries.User_Id and Users.Name = '{0}' where Token = '{1}'",user, token);
            var dnsentry = db.Entries.SqlQuery(sql).FirstOrDefault();
            if (dnsentry == null)
            {
                dnsentry = new DnsEntry() { Id = Guid.NewGuid(), IpValue = ipAddress, User = usr, Token = token };
                db.Entries.Add(dnsentry);
            }
            else
            {
                var dnse = db.Entries.Find(dnsentry.Id);
                dnse.IpValue = ipAddress;
                dnse.User = usr;
                db.Entry(dnse).State = EntityState.Modified;
            }
            try
            {
                // Your code...
                // Could also be before try if you know the exception occurs in SaveChanges

                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            //return new ContentResult() { Content = string.Format("Stored {0} {1} {2}", user, token, ipAddress) };
            return new ContentResult() { Content = string.Format(response, user, token, ipAddress) };
            
        }


        public ActionResult Details(Guid id = default(Guid))
        {
            DnsEntry dnsentry = db.Entries.Find(id);
            if (dnsentry == null)
            {
                return HttpNotFound();
            }
            return View(dnsentry);
        }

        //
        // GET: /Dns/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Dns/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DnsEntry dnsentry)
        {
            if (ModelState.IsValid)
            {
                dnsentry.Id = Guid.NewGuid();
                db.Entries.Add(dnsentry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dnsentry);
        }

        //
        // GET: /Dns/Edit/5

        public ActionResult Edit(Guid id = default(Guid))
        {
            DnsEntry dnsentry = db.Entries.Find(id);
            if (dnsentry == null)
            {
                return HttpNotFound();
            }
            return View(dnsentry);
        }

        //
        // POST: /Dns/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DnsEntry dnsentry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dnsentry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dnsentry);
        }

        //
        // GET: /Dns/Delete/5

        public ActionResult Delete(Guid id = default(Guid))
        {
            DnsEntry dnsentry = db.Entries.Find(id);
            if (dnsentry == null)
            {
                return HttpNotFound();
            }
            return View(dnsentry);
        }

        //
        // POST: /Dns/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DnsEntry dnsentry = db.Entries.Find(id);
            db.Entries.Remove(dnsentry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}