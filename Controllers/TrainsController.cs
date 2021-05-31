using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DB_s2_1_1.EntityModels;
using DB_s2_1_1.ViewModel;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.Services;
using DB_s2_1_1.ViewModel.Trains;

namespace DB_s2_1_1.Controllers
{
    public class TrainsController : Controller
    {
        private readonly ITrainsService trainsService;

        public TrainsController(ITrainsService trainsService)
        {
            this.trainsService = trainsService;
        }

        // GET: Trains
        public async Task<IActionResult> Index(TrainsIndex trainsIndex)
        {

            return View(await trainsService.GetTrainsIndex(trainsIndex));
        }

        // GET: Trains/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var train = await trainsService.GetTrainWithDetails(id);
            if (train == null)
            {
                return NotFound();
            }

            return View(train);
        }
        

        // GET: Trains/Create
        public IActionResult Create()
        {                
            return View(trainsService.GetTrainEditing());
        }

        

        // POST: Trains/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainEditing trainsCreate)
        {
            if (ModelState.IsValid)
            {
                await trainsService.AddTrain(trainsCreate);
                return RedirectToAction(nameof(Index));
            }
            return View(trainsService.GetTrainEditing(trainsCreate));
        }

        

       // GET: Trains/Edit/5
       public async Task<IActionResult> Edit(int id)
       {


           if (id == 0)
           {
               return NotFound();
           }

           var train = await trainsService.GetTrainEditing(id);
           if (train == null)
           {
               return NotFound();
           }
           return View(train);
       }

        

        // POST: Trains/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Train, Id, RouteId, CategoryId, StationId, SeatsQty ")]TrainEditing trainEditing)
        {
            if (trainEditing == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string error = await trainsService.UpdateTrain(trainEditing);
                if (string.IsNullOrWhiteSpace(error))
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", error);
            }
            
            return View(trainsService.GetTrainEditing(trainEditing));
        }

        

       // GET: 
       public async Task<IActionResult> EditBrigade(int id, int page = 1)
       {
           if (id == 0)
           {
               return NotFound();
           }

           var trainEditBrigade = await trainsService.GetTrainsEditBrigade(id, page);

           if (trainEditBrigade == null)
           {
               return NotFound();
           }
          
           return View(trainEditBrigade);
       }

        
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> EditBrigade(TrainsEditBrigade trainsEditBrigade)
       {
           if (trainsEditBrigade == null)
           {
               return NotFound();
           }

            string error = await trainsService.UpdateTrainBrigade(trainsEditBrigade);
            if (!string.IsNullOrWhiteSpace(error))
                ModelState.AddModelError("", error);
           
           return RedirectToAction(nameof(EditBrigade), new {Id = trainsEditBrigade.GetTrainId(), page = trainsEditBrigade.GetPage()});
       }
        

       // GET: Trains/Delete/5
       public async Task<IActionResult> Delete(int id)
       {
           if (id == 0)
           {
               return NotFound();
           }
            
            var train = await trainsService.GetTrain(id);
           if (train == null)
           {
               return NotFound();
           }

           return View(train);
       }
        

       // POST: Trains/Delete/5
       [HttpPost, ActionName("Delete")]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> DeleteConfirmed(int id)
       {
            await trainsService.RemoveTrain(id);
           return RedirectToAction(nameof(Index));
       }

       

      

    }
}
