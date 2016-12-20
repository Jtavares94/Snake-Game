using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Snake
{
    public enum CollisionType
    {
        None,
        PowerUp,
        Fatal
    }

    class CollisionManager
    {
        public CollisionType Collision(Grid grid, Snake snake)
        {
            if (snake.InTail(snake.X, snake.Y))
                return CollisionType.Fatal;
            if (grid.Tile(snake.X, snake.Y) == Grid.Wall_Tile)
                return CollisionType.Fatal;
            if (grid.Tile(snake.X, snake.Y) == Grid.Powerup_Tile)
                return CollisionType.PowerUp;
            return CollisionType.None;
        }
    }
}
