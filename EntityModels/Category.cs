using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DB_s2_1_1.EntityModels
{
    public class Category
    {
        [Display(Name = "Category id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Category name")]
        public string Name { get; set; }

        [Display(Name = "Speed")]
        //[DataType(DataType., ErrorMessage = "Must be a Deciaml!")]
        public double Speed { get; set; }

        public ICollection<Train> Trains { get; set; }
    }
}
