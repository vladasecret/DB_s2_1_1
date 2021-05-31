using DB_s2_1_1.EntityModels;
using DB_s2_1_1.Services;
using DB_s2_1_1.ViewModel;
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
        private readonly IManualRawSqlService manualRawSqlService;

        public ManualRawSqlController(IManualRawSqlService manualRawSqlService)
        {
            this.manualRawSqlService = manualRawSqlService;
        }
        // GET: ManualRawSqlController
        public ActionResult Index(ManualQueryViewModel manualQuery)
        {
            ManualQueryViewModel model;
            if (manualQuery != null)
            {

                model = manualRawSqlService.ExecuteQuery(manualQuery.InsertedQuery, manualQuery.Page);
            }
            else model = new();

            return View(model);
        }
    }
}

