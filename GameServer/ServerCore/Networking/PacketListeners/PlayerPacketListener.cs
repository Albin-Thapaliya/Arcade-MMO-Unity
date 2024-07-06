﻿using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.EntityShared;
using MapHandler;
using ServerCore.GameServer.Players.Evs;
using Storage.Players;
using System;

namespace ServerCore.Networking.PacketListeners
{
    public class PlayerPacketListener : IEventListener
    {
        [EventMethod] 
        public void OnPlayerMovePath(EntityMovePacket packet)
        {
            var player = Server.GetPlayerByConnectionId(packet.ClientId);
            var distanceMoved = MapHelpers.GetDistance(player.GetPosition(), packet.To);
            var timeToMove = Formulas.GetTimeToMoveBetweenTwoTiles(player.MoveSpeed);
            var now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var lastMovementArrival = now + timeToMove;

            // Player tryng to hack ?
            if (distanceMoved > 1 || now < player.CanMoveAgainTime)
            {
                // send player back to the position client-side
                player.Tcp.Send(new SyncPacket()
                {
                    Position = player.GetPosition()
                });
                return;
            }

            var playerMoveEvent = new PlayerMoveEvent()
            {
                From = packet.From,
                To = packet.To,
                Player = player
            };
            Server.Events.Call(playerMoveEvent);

            if (playerMoveEvent.IsCancelled)
            {
                // send player back to the position client-side
                player.Tcp.Send(new SyncPacket()
                {
                    Position = player.GetPosition()
                });
                return;
            }

            // subtract the player latency for possibility of lag for a smoother movement
            player.CanMoveAgainTime = now + timeToMove - player.Tcp.Latency;

            // Updating player position locally
            player.X = packet.To.X;
            player.Y = packet.To.Y;

            // updating in database
            PlayerService.UpdatePlayerPosition(player, player.X, player.Y);
        }
    }
}
