﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace MWWebAPI.DBRepository
{
    public abstract class DBRepositoryBase
    {
        protected string MWConnectionString = ConfigurationManager.ConnectionStrings["MachineWorkCS"].ConnectionString;
    }
}