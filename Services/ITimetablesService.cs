using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.ViewModel;
using DB_s2_1_1.ViewModel.Routes;
using DB_s2_1_1.ViewModel.Timetables;
using DB_s2_1_1.ViewModel.Trains;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface ITimetablesService
    {
        Task<PagedResult<Timetable>> GetTimetablesIndex(int page);

        Task<Timetable> GetTimetableDetails(int id);

        TimetablesCreateModel GetTimetablesCreateModel();

        Task<string> GenerateTimetables(TimetablesCreateModel createModel);

        SelectList GetTrainsIdSelectList(int? selectedId = null);
    }
}
