using DB_s2_1_1.EntityModels;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.ViewModel.Trains
{
    public class TrainsFilters
    {
        public int? SearchRoute { get; set; }
        public int? SearchCategory { get; set; } 
        public int? SearchSeats { get; set; }

        
        public ExpressionStarter<Train> GetPredicate()
        {
            var predicate = PredicateBuilder.New<Train>(true);

            if (SearchCategory > 0)
                predicate.And(e => e.CategoryId == SearchCategory);

            if (SearchSeats > 0)
                predicate.And(e => e.SeatsQty >= SearchSeats);

            if (SearchRoute > 0)
                predicate.And(e => e.RouteId == SearchRoute);

            return predicate;
        }
    }
}
