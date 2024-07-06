using MapHandler;
using ServerCore.GameServer.Players.Evs;
using System;

namespace ServerCore.Game.Monsters.Behaviours.MoveBehaviours
{
    public class RandomWalk : IMonsterMovement
    {
        public void PerformMovement(Monster monster)
        {
            var oldPosition = monster.Position;
            var newPosition = new Position(oldPosition.X, oldPosition.Y);
            var rnd = new Random().Next(1, 5);

            switch (rnd)
            {
                case 1:
                    newPosition.X += 1; break;
                case 2:
                    newPosition.Y += 1; break;
                case 3:
                    newPosition.X -= 1; break;
                case 4:
                    newPosition.Y -= 1; break;
                default:
                    break;
            }

            if(Server.Map.IsPassable(newPosition.X - 1, newPosition.Y)) {
                newPosition.X -= 1;
            }
            if(Server.Map.IsPassable(newPosition.X + 1, newPosition.Y)) {
                newPosition.X += 1;
            }
           
            if (Server.Map.IsPassable(newPosition.X, newPosition.Y))
            {
                var monsterMoveEvent = new EntityMoveEvent()
                {
                    Entity = monster,
                    To = newPosition
                };
                Server.Events.Call(monsterMoveEvent);
            }
        }
    }
}
