﻿using REST.Api.Entities;
using REST.Api.Helpers;
using System;
using System.Collections.Generic;


namespace REST.Api.Services
{
    /// <summary>
    /// interface ISchlagRepository
    /// </summary>
    public interface ISchlagRepository
    {
        PagedList<Schlag> GetSchlaege(SchlagResourceParameter schlagResourceParameters);
        Schlag GetSchlag(Guid guid);
        IEnumerable<Schlag> GetSchlaege(IEnumerable<Guid> guids);
        void AddSchlag(Schlag schlag);
        void DeleteSchlag(Schlag schlag);
        void UpdateSchlag(Schlag schlag);
        bool SchlagExists(Guid guid);
        bool Save();
    }
}
