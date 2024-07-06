﻿using Common.Networking.Packets;
using CommonCode.EventBus;
using MapHandler;
using ServerCore.GameServer.Players.Evs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore.Game.Monsters
{
    public class MonsterListener : IEventListener
    {
        [EventMethod]
        public void OnMonsterSpawn(MonsterSpawnEvent ev)
        {
            Log.Info($"MONSTER SPAWNED {ev.Position.X} - {ev.Position.Y}");
        }
    }
}
