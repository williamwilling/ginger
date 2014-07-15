﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            ViewBag.Id = id;
            return View();
        }

        public ActionResult Create()
        {
            return RedirectToAction("Details", new { id = CreateBoard() });
        }

        [HttpPost]
        public void Edit(string id, string contents)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["TableStorage"].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("ginger");
            table.CreateIfNotExists();

            var board = new BoardEntity("go");
            board.RowKey = id;
            board.Contents = contents;
            board.ETag = "*";

            try
            {
                var operation = TableOperation.Replace(board);
                table.Execute(operation);
            }
            catch (Exception e)
            {
                int b = 3;
            }
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