using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using WilliamWilling.Ginger.Models;

namespace WilliamWilling.Ginger.Controllers
{
    public class GoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return RedirectToAction("Details", new { id = CreateBoard() });
        }

        private string CreateBoard()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TableStorage"].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("ginger");
            table.CreateIfNotExists();

            bool created = false;
            BoardEntity board = new BoardEntity("go");

            while (!created)
            {
                try
                {
                    var operation = TableOperation.Insert(board);
                    table.Execute(operation);
                    created = true;
                }
                catch (StorageException e)
                {
                    if (e.RequestInformation.HttpStatusCode != 409)
                    {
                        throw;
                    }

                    board = new BoardEntity("go");
                }
            }

            return board.RowKey;
        }

        
	}
}