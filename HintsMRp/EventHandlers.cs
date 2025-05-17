using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using MEC;

namespace HintsMRp
{
    public class EventHandlers
    {
        private readonly Dictionary<Exiled.API.Features.Player, CoroutineHandle> _playerTimers;
        private readonly Dictionary<Exiled.API.Features.Player, string> _lastHints;
        private readonly Main _plugin;

        public EventHandlers(Main plugin)
        {
            _plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            _playerTimers = new Dictionary<Exiled.API.Features.Player, CoroutineHandle>();
            _lastHints = new Dictionary<Exiled.API.Features.Player, string>();
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null) return;

            if (_playerTimers.TryGetValue(ev.Player, out var oldHandle))
            {
                Timing.KillCoroutines(oldHandle);
                _playerTimers.Remove(ev.Player);
            }

            if (ShouldShowHints(ev.NewRole))
            {
                float interval = GetIntervalForRole(ev.NewRole);
                var handle = Timing.RunCoroutine(ShowHintsCoroutine(ev.Player, interval));
                _playerTimers.Add(ev.Player, handle);
            }
        }

        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (ev.Player == null) return;

            if (_playerTimers.TryGetValue(ev.Player, out var handle))
            {
                Timing.KillCoroutines(handle);
                _playerTimers.Remove(ev.Player);
            }
            _lastHints.Remove(ev.Player);
        }

        private static bool ShouldShowHints(RoleTypeId role)
        {
            return role != RoleTypeId.None && role != RoleTypeId.Spectator;
        }

        private float GetIntervalForRole(RoleTypeId role)
        {
            return role.GetTeam() == Team.Dead || role == RoleTypeId.Tutorial
                ? _plugin.Config.ObserverInterval
                : _plugin.Config.PlayerInterval;
        }

        private IEnumerator<float> ShowHintsCoroutine(Exiled.API.Features.Player player, float interval)
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(interval);

                if (player == null || !player.IsConnected || player.Role == RoleTypeId.None)
                    yield break;

                List<string> hints = GetHintsForRole(player.Role);
                if (hints.Count == 0) continue;

                var availableHints = hints.Where(h => !_lastHints.TryGetValue(player, out var lastHint) || h != lastHint).ToList();

                if (availableHints.Count == 0)
                    availableHints = hints.ToList();

                string randomHint = availableHints[UnityEngine.Random.Range(0, availableHints.Count)];
                _lastHints[player] = randomHint;

                ShowStyledHint(player, randomHint);
            }
        }

        private void ShowStyledHint(Exiled.API.Features.Player player, string message)
        {
            string styledMessage = $"<voffset=15em><size={_plugin.Config.TextSize}><color={_plugin.Config.TextColor}><b>✧ {message} ✧</b></color></size></voffset>";
            player.ShowHint(styledMessage, _plugin.Config.HintDuration);
        }

        private List<string> GetHintsForRole(RoleTypeId role)
        {
            return role.GetTeam() == Team.Dead || role == RoleTypeId.Tutorial
                ? new List<string>(_plugin.Config.ObserverHints)
                : new List<string>(_plugin.Config.PlayerHints);
        }
    }
}