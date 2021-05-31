using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using DB_s2_1_1.ViewModel.Routes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface IRoutesService
    {
        Task<RoutesIndex> GetRoutesIndex(int searchRoute, string searchCity, int page = 1);

        Task<Route> GetRoute(int id);

        SelectList GetStationsSelectList(Station selectedStation = null);

        Task<RoutesStationViewModel> GetRoutesStationViewModel(int routesStationId);

        Task<RoutesStationViewModel> GetNewRoutesStationViewModel(int routeId);

        Task<string> AddNewRoute(Route route, int stationId);

        Task<string> AddRouteStation(RoutesStationViewModel rs);

        Task<bool> RouteExists(int id);

        Task<string> UpdateRouteStation(RoutesStationViewModel routesEditStation);

        Task RemoveStation(int routeId, int stationId);

        Task RemoveTrain(int routeId, int trainId);

        Task RemoveRoute(int routeId);
    }
}
