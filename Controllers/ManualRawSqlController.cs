using DB_s2_1_1.EntityModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Controllers
{
    public class ManualRawSqlController : Controller
    {
        private readonly IConfiguration configuration;

        public ManualRawSqlController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // GET: ManualRawSqlController
        public ActionResult Index(string query)
        {
            DataTable dt = new();

            if (!string.IsNullOrEmpty(query)) {
                ViewData["InsertedQuery"] = query;
                using SqlConnection sqlConnection = new(configuration.GetConnectionString("ManualRawSqlConnection"));
                sqlConnection.Open();
                using SqlCommand cmd = new(query, sqlConnection);
                try
                {
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                    
                }
                catch (SqlException exc)
                {
                    ViewData["ErrorMsg"] = exc.Message;
                }
            }
            return View(dt);
        }

        // GET: ManualRawSqlController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManualRawSqlController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManualRawSqlController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManualRawSqlController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManualRawSqlController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManualRawSqlController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManualRawSqlController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
