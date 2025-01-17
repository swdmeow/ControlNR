﻿namespace Control.Commands
{
    using System;
    using CommandSystem;
    using Exiled.API.Features;
    using RemoteAdmin;
    using Exiled.CustomRoles.API.Features;
    using System.Collections.Generic;
    using Exiled.CustomItems.API.Features;
    using Control.CustomItems;
    using System.Linq;
    using PlayerRoles;
    using CustomPlayerEffects;
    using UnityEngine;
    using MEC;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class Res : ICommand
    {
        public string Command { get; } = "res";

        public static List<Player> DiedWithSCP500R = new List<Player>();
        public static List<RoleTypeId> RoleDiedWithSCP500R = new List<RoleTypeId>();
        public static List<StatusEffectBase> StatusEffectBase = new List<StatusEffectBase>();
        public string[] Aliases { get; } = new string[] { "respawn" };
        public string Description { get; } = "Команда для возвраждения, если вы имели на тот момент SCP-500-R..";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if(player.IsAlive)
            {
                response = "... что ты пытаешься сделать?";
                return false;
            }

            if (DiedWithSCP500R.Contains(player))
            {
                Player oldPlayer = DiedWithSCP500R.Find(x => x == player);
                RoleTypeId role = RoleDiedWithSCP500R.First();

                player.Role.Set(role, RoleSpawnFlags.None);

                if(Warhead.IsDetonated == true)
                {
                    Timing.CallDelayed(0.1f, () =>
                    {
                        player.Position = Room.Get(Exiled.API.Enums.RoomType.Surface).transform.position + Vector3.up;
                    });
                }

                foreach(StatusEffectBase effect in StatusEffectBase)
                {
                    player.EnableEffect(effect, effect.Duration);
                }
                player.ShowHint("", 0.1f);
                DiedWithSCP500R.Clear();
                RoleDiedWithSCP500R.Clear();
                StatusEffectBase.Clear();
                response = "Успешно?..";
                return true;
            }

            response = "... что ты пытаешься сделать?";
            return false;
        }
    }
}