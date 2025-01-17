﻿using System.Collections.Generic;
using Exiled.API.Features;
using Control.Handlers.Events.API.Serialization;

using Badge = Control.Handlers.Events.API.Features.Badge;

namespace Control.Handlers.Events.API
{
    using System.Linq;
    using Exiled.API.Extensions;
    using MEC;
    using PlayerRoles;
    using Сontrol;

    public static class Extensions
    {
        public static PlayerLog GetLog(this Player ply)
        {
            PlayerLog toInsert = null;
            if (!API.TryGetLog(ply.UserId, out var log))
            {
                toInsert = new PlayerLog()
                {
                    ID = ply.UserId,
                    LVL = 0,
                    XP = 0,
                    Nickname = ply.Nickname,
                    DNT = ply.DoNotTrack,
                };
                ControlNR.Singleton.XPdb.GetCollection<PlayerLog>("Players").Insert(toInsert);
            }

            if (log is null)
                return toInsert;

            PlayerLog updateData = new PlayerLog()
            {
                ID = ply.UserId,
                LVL = log.LVL,
                XP = log.XP,
                Nickname = ply.Nickname,
                DNT = ply.DoNotTrack,
            };
            ControlNR.Singleton.XPdb.GetCollection<PlayerLog>("Players").Update(updateData);

            return log;
        }

        public static void UpdateLog(this PlayerLog log)
        {
            ControlNR.Singleton.XPdb.GetCollection<PlayerLog>("Players").Update(log);
        }

        public static void AddXP(this PlayerLog log, int amount)
        {
            log.XP += amount;
            Player ply = Player.Get(log.ID);

            int XPPerLevel = ControlNR.Singleton.Config.XPSystem.XPPerLevel + (ControlNR.Singleton.Config.XPSystem.XPPerNewLevel * log.LVL);

            int lvlsGained = log.XP / XPPerLevel;
            if (lvlsGained > 0)
            {
                log.LVL += lvlsGained;
                log.XP -= lvlsGained * XPPerLevel;
                if (ControlNR.Singleton.Config.XPSystem.ShowAddedLVL && ply != null)
                {
                    Control.Extensions.HintExtensions.XPHintQueue.Add((ply, ControlNR.Singleton.Config.XPSystem.AddedLVLHint
                        .Replace("%level%", log.LVL.ToString()), 12f));
                }

                ply.RankName = "";
            }
            
            else if (ControlNR.Singleton.Config.XPSystem.ShowAddedXP && ply != null)
            {
                Control.Extensions.HintExtensions.XPHintQueue.Add((ply, $"+ <color=green>{amount}</color> XP", 12f));
            }
            log.UpdateLog();
        }
    }
}