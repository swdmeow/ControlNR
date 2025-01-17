﻿namespace Control.Handlers
{
    internal sealed class Handler
    {
        private Control.Handlers.Events.PlayerHandler PlayerHandler;
        private Control.Handlers.Events.ServerHandler ServerHandler;
        private Control.Handlers.Events.Scp330Handler Scp330Handler;
        private Control.Handlers.Events.Scp096Handler Scp096Handler;

        private Control.Handlers.Events.MapHandler MapHandler;
        private Control.Handlers.Events.WarheadMusic WarheadMusic;
        private Control.Handlers.Events.Scp914Handler Scp914Handler;
        private Control.Handlers.Events.UnlimitedAmmoHandler UnlimitedAmmoHandler;
        private Control.Handlers.Events.RemoteKeycardHandler RemoteKeycardHandler;
        private Control.Handlers.Events.WarheadDecontamition WarheadDecontamition;
        private Control.Handlers.Events.XPHandler XPHandler;

        public Handler()
        {
            PlayerHandler = new Control.Handlers.Events.PlayerHandler();
            ServerHandler = new Control.Handlers.Events.ServerHandler();
            MapHandler = new Control.Handlers.Events.MapHandler();
            Scp330Handler = new Control.Handlers.Events.Scp330Handler();
            WarheadMusic = new Control.Handlers.Events.WarheadMusic();
            Scp914Handler = new Control.Handlers.Events.Scp914Handler();
            Scp096Handler = new Control.Handlers.Events.Scp096Handler();
            // Features
            UnlimitedAmmoHandler = new Control.Handlers.Events.UnlimitedAmmoHandler();
            RemoteKeycardHandler = new Control.Handlers.Events.RemoteKeycardHandler();
            WarheadDecontamition = new Control.Handlers.Events.WarheadDecontamition();
            XPHandler = new Control.Handlers.Events.XPHandler();
        }
        public void Dispose()
        {
            ServerHandler.OnDisabled();
            Scp330Handler.OnDisabled();
            MapHandler.OnDisabled();
            PlayerHandler.OnDisabled();
            WarheadMusic.OnDisabled();
            Scp914Handler.OnDisabled();
            RemoteKeycardHandler.OnDisabled();
            UnlimitedAmmoHandler.OnDisabled();
            XPHandler.OnDisabled();
            WarheadDecontamition.OnDisabled();
            Scp096Handler.OnDisabled();

            Scp096Handler = null;
            RemoteKeycardHandler = null;
            XPHandler = null;
            UnlimitedAmmoHandler = null;
            PlayerHandler = null;
            ServerHandler = null;
            MapHandler = null;
            Scp330Handler = null;
            WarheadMusic = null;
            WarheadDecontamition = null;
        }
    }
}