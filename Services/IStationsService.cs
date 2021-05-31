using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface IStationsService
    {
        Task<PagedResult<Station>> GetPagedStations(int page = 1);

        Task<Station> GetStation(int id);

        Task<string> AddStation(Station station);

        Task<bool> StationExists(int id);

        Task<string> UpdateStation(Station station);

        Task RemoveStation(int id);
    }
}
