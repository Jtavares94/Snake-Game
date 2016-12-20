using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Grid
    {
        public const int Empty_Tile = 0;
        public const int Wall_Tile = 1;
        public const int Powerup_Tile = 2;

        private readonly int[,] grid;
        private readonly Random random = new Random();
        private readonly Vector2 tileSize;
        private readonly GameContentManager contentManager;

        public Grid(GameContentManager contentManager, Vector2 tileSize, int[,] grid)
        {
            this.grid = grid;
            this.tileSize = tileSize;
            this.contentManager = contentManager;
        }

        public bool AddRandomPowerUp(List<Vector2> occupied)
        {
            List<Vector2> availablePositions = GetAvailablePositions(occupied);
            if (availablePositions.Count > 0)
            {
                AddPowerUp(availablePositions);
                return true;
            }
            return false;         
        }
        private void AddPowerUp(List<Vector2> availablePositions)
        {
            Vector2 pos = availablePositions[random.Next(availablePositions.Count - 1)];
            grid[(int)pos.X, (int)pos.Y] = Powerup_Tile;
        }

        private List<Vector2> GetAvailablePositions(List<Vector2> occupied)
        {
            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i <= grid.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= grid.GetUpperBound(1); j++)
                {
                    if (grid[i, j] == 0 && !occupied.Any(o => o.X == i && o.Y == j))
                        positions.Add(new Vector2 { X = i, Y = j });
                }
            }
            return positions;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y <= grid.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= grid.GetUpperBound(0); x++)
                {
                    int value = grid[x, y];
                    DrawTile(value, x * (int)tileSize.X, y * (int)tileSize.Y, spriteBatch);
                }
            }
        }

        private void DrawTile(int value, int x, int y, SpriteBatch spriteBatch)
        {
            if (value == Powerup_Tile)
                spriteBatch.Draw(contentManager.Get<Texture2D>("apple"), new Vector2(x, y), null);
            else if (value == Wall_Tile)
                spriteBatch.Draw(contentManager.Get<Texture2D>("wall"), new Vector2(x, y), null);
        }

        public void Clear(int x, int y)
        {
            grid[x, y] = Empty_Tile;
        }

        public int Tile(int x, int y)
        {
            return grid[x, y];
        }
    }
}
