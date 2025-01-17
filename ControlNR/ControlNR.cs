namespace �ontrol
{
    using Exiled.API.Features;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomRoles.API.Features;
    using LiteDB;
    using System.IO;
    using HarmonyLib;
    using System;
    using MEC;
    using Control.Extensions;
    using CommandSystem.Commands.RemoteAdmin;
    using System.Net;

    public class ControlNR : Plugin<Config>
    {
        public static ControlNR Singleton;
        public override string Name => "ControlNR";
        public override string Author => "swd";
        public override System.Version Version => new System.Version(2, 2, 0);

        private Harmony harmony;

        private Control.Handlers.Handler EventsHandler;

        public LiteDatabase db;
        public LiteDatabase XPdb;
        public override void OnEnabled()
        {
            if (!Directory.Exists(Path.Combine(Paths.Configs, "ControlNR"))) Directory.CreateDirectory(Path.Combine(Paths.Configs, "ControlNR"));
            if (!Directory.Exists(Path.Combine(Paths.Configs, "ControlNR/Music"))) Directory.CreateDirectory(Path.Combine(Paths.Configs, "ControlNR/Music"));

            db = new LiteDatabase(Path.Combine(Paths.Configs, @"ControlNR/LimitDonator.db"));
            XPdb = new LiteDatabase(Path.Combine(Paths.Configs, @"ControlNR/XPUser.db"));
            Singleton = this;

            harmony = new Harmony($"ControlNR - {DateTime.Now.Ticks}");

            EventsHandler = new Control.Handlers.Handler();

            CustomItem.RegisterItems();
            CustomRole.RegisterRoles(false, null, true, Assembly);

            if (Control.CustomRoles.SCP343.HintCooldownCoroutineHandle == null || !Control.CustomRoles.SCP343.HintCooldownCoroutineHandle.Value.IsValid || !Control.CustomRoles.SCP343.HintCooldownCoroutineHandle.Value.IsRunning)
                Control.CustomRoles.SCP343.HintCooldownCoroutineHandle = Timing.RunCoroutine(Control.CustomRoles.SCP343.HintCoroutine());

            if (HintExtensions.WriteHintCoroutineHandle == null || !HintExtensions.WriteHintCoroutineHandle.Value.IsValid || !HintExtensions.WriteHintCoroutineHandle.Value.IsRunning)
                HintExtensions.WriteHintCoroutineHandle = Timing.RunCoroutine(HintExtensions.WriteHint());

            if (ClearMapExtensions.ClearMapCoroutineHandle == null || !ClearMapExtensions.ClearMapCoroutineHandle.Value.IsValid || !ClearMapExtensions.ClearMapCoroutineHandle.Value.IsRunning)
                ClearMapExtensions.ClearMapCoroutineHandle = Timing.RunCoroutine(ClearMapExtensions.ClearMap());

            harmony.PatchAll();
            
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            harmony.UnpatchAll();
            harmony = null;

            db.Dispose();
            db = null;

            XPdb.Dispose();
            XPdb = null;

            EventsHandler.Dispose();
            EventsHandler = null;

            CustomItem.UnregisterItems();
            CustomRole.UnregisterRoles();

            Singleton = null;

            base.OnDisabled();
        }
    }
}