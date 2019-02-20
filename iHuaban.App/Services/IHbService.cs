﻿using iHuaban.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHuaban.App.Services
{
    public interface IHbService<T> where T : new()
    {
        T Get(int limit = 0, long max = 0);
        Task<T> GetAsync(int limit = 0, long max = 0);
    }
}
