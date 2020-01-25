﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FrontendService.Models;

namespace FrontendService.Agents.Abstractions
{
    public interface IVoorraadAgent
    {
        Task<IEnumerable<VoorraadMagazijn>> GetAllVoorraadAsync();
    }
}
