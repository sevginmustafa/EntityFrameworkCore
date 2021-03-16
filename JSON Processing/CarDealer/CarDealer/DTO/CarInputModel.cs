﻿using System.Collections.Generic;

namespace CarDealer.DTO
{
    public class CarInputModel
    {
        public CarInputModel()
        {
            PartsId = new HashSet<int>();
        }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public ICollection<int> PartsId { get; set; }
    }
}
