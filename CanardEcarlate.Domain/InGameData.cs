﻿using CanardEcarlate.Domain.Cards;
using CanardEcarlate.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.Domain
{
    public class InGameData
    {
        public Player CurrentPlayer { get; set; }
        public Player PreviousPlayer { get; set; }
        public ICard DrawnCard { get; set; }
        public int NbRound { get; set; }
        public int NbGreenDrawn { get; set; }
        public int NbDrawnDuringRound { get; set; }
        public IRole WinnerRole { get; set; } = null;
    }
}
