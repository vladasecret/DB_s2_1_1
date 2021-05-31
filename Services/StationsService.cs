using DB_s2_1_1.EntityModels;
using DB_s2_1_1.PagedResult;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public class StationsService : IStationsService
    {
        private readonly TrainsContext _context;

        public StationsService(TrainsContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Station>> GetPagedStations(int page = 1)
        {
            return await _context.Stations.AsNoTracking().GetPaged(page);
        }

        public async Task<Station> GetStation(int id)
        {
            return await _context.Stations.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<string> AddStation(Station station)
        {
            try
            {
                await _context.AddAsync(station);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return $"Station with name \"{station.Name}\" already exists.";
                }
                return "Error writing to database.";
            }
            return null;
        }

        public async Task<string> UpdateStation(Station station)
        {
            try
            {
                _context.Update(station);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                if (exc.InnerException is SqlException innerExc)
                {
                    if (innerExc.Number == 2627)
                        return $"Station with name \"{station.Name}\" already exists.";
                }

                return "Error writing to database.";
            }

            return null;
        }

        public async Task<bool> StationExists(int id)
        {
            return await _context.Stations.AsNoTracking().AnyAsync(s => s.Id == id);
        }


        public async Task RemoveStation(int id)
        {
            Station station = await GetStation(id);
            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
        }
    }
}
