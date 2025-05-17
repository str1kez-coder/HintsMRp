using System;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;

namespace HintsMRp
{
    public class Main : Plugin<Config>
    {
        public override string Author => "str1kez";
        public override string Name => "HintsMRp";
        public override string Prefix => "HintsMRp";
        public override Version Version => new Version(1, 1, 0);

        private EventHandlers _handlers;

        public override void OnEnabled()
        {
            _handlers = new EventHandlers(this);

            Exiled.Events.Handlers.Player.ChangingRole += _handlers.OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying += _handlers.OnDestroying;

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= _handlers.OnChangingRole;
            Exiled.Events.Handlers.Player.Destroying -= _handlers.OnDestroying;

            _handlers = null;
            base.OnDisabled();
        }
    }
}