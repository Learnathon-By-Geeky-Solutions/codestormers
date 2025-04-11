﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmoVerse.Application.DTOs
{
    public class SatelliteDto
    {
        public required string Name { get; set; }

        public double Size { get; set; }
        public double DistanceFromPlanet { get; set; }
        public double OrbitalPeriod { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
