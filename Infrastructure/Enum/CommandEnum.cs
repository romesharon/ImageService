﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    public enum CommandEnum : int
    {
        NewFileCommand = 0,
        CloseCommand = 1,
        Settings = 2,
        Log = 3
    }
}
