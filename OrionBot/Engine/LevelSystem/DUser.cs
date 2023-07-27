﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionBot.Engine.LevelSystem
{
    public class DUser
    {
        public string UserName { get; set; }

        public ulong guildID { get; set; }

        public string avatarURL { get; set; }

        public double XP { get; set; }

        public int Level { get; set; }
    }
}
