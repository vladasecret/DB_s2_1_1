using DB_s2_1_1.EntityModels;
using DB_s2_1_1.ViewModel;
using DB_s2_1_1.ViewModel.Routes;
using DB_s2_1_1.ViewModel.Trains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.Services
{
    public interface ITrainsService
    {
        Task<TrainsIndex> GetTrainsIndex(TrainsIndex trainsIndex);

        Task<Train> GetTrain(int id);
        Task<TrainsViewModel> GetTrainWithDetails(int id);

        TrainEditing GetTrainEditing(TrainEditing trainsCreate = null);

        Task<TrainEditing> GetTrainEditing(int trainId);

        Task AddTrain(TrainEditing trainsCreate);

        Task<string> UpdateTrain(TrainEditing trainEditing);

        Task<TrainsEditBrigade> GetTrainsEditBrigade(int trainId, int page);

        Task<string> UpdateTrainBrigade(TrainsEditBrigade trainsEditBrigade);

        Task RemoveTrain(int id);
    }
}
