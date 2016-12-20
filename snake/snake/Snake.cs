using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    class Snake
    {
        private readonly SortedList<int, SnakePart> tail;
        private readonly Queue<int> newParts;
        private readonly GameContentManager contentManager;
        private SnakePart Head { get; set; }
        private int currentXDirection;
        private int currentYDirection;
        private int nextXDirection;
        private int nextYDirection;
        private int movementTimer;
        private int movementTresholdMilliseconds = 150;
        private readonly Vector2 tileSize;

        public int X { get { return Head.X; } }
        public int Y { get { return Head.Y; } }

        public Snake(GameContentManager contentManager, int xStart, int yStart, int initialLenght, int xDirection, int yDirection, Vector2 tileSize)
        {
            this.contentManager = contentManager;
            this.tileSize = tileSize;
            Head = CreateHead(xStart, yStart);
            ChangeDirection(xDirection, yDirection);
            tail = new SortedList<int, SnakePart>();
            for (int i = 1; i < initialLenght; i++)
            {
                AddTail(Head.X - i, Head.Y);
            }
            newParts = new Queue<int>();
        }

        public void Update(GameTime gameTime)
        {
            movementTimer += gameTime.ElapsedGameTime.Milliseconds;
            if (movementTimer >= movementTresholdMilliseconds)
            {
                movementTimer = 0;

                currentXDirection = nextXDirection;
                currentYDirection = nextYDirection;

                int nextX = Head.X, nextY = Head.Y;
                UpdateHead();
                UpdateTail(ref nextX, ref nextY);
                AddTailPart(nextX, nextY);
            }
        }

        public void AddTailPart(int nextX, int nextY)
        {
            if (newParts.Count > 0)
            {
                AddTail(nextX, nextY);
                newParts.Dequeue();
            }
        }

        public void UpdateTail(ref int nextX, ref int nextY)
        {
            foreach (var part in tail)
            {
                int lastX = part.Value.X, lastY = part.Value.Y;
                part.Value.X = nextX;
                part.Value.Y = nextY;
                nextX = lastX;
                nextY = lastY;
            }
        }

        public void UpdateHead()
        {
            Head.X += currentXDirection;
            Head.Y += currentYDirection;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var part in tail)
            {
                part.Value.Draw(contentManager, spriteBatch);
            }
            Head.Draw(contentManager, spriteBatch);
        }

        public void ChangeDirection(int xDirection, int yDirection)
        {
            if (currentXDirection != 0 && xDirection != 0 || currentYDirection != 0 && yDirection != 0)
                return;
            if (nextXDirection != 0 && xDirection != 0 || nextYDirection != 0 && yDirection != 0)
                return;

            nextXDirection = xDirection;
            nextYDirection = yDirection;
        }

        public void AddTail()
        {
            newParts.Enqueue(1);
        }

        public List<Vector2> GetPositions()
        {
            List<Vector2> positions = tail.Select(t => new Vector2 { X = t.Value.X, Y = t.Value.Y }).ToList();
            positions.Add(new Vector2 { X = Head.X, Y = Head.Y });
            return positions;
        }

        public bool InTail(int x, int y)
        {
            return tail.Any(t => t.Value.X == x && t.Value.Y == y);
        }

        private void AddTail(int x, int y)
        {
            tail.Add(tail.Count + 1, CreateTail(x, y));
        }
        private SnakePart CreateHead(int x, int y)
        {
            return new SnakePart
            {
                X = x,
                Y = y,
                TileSize = tileSize,
                Texture = "darkgreen"
            };
        }
        private SnakePart CreateTail(int x, int y)
        {
            return new SnakePart
            {
                X = x,
                Y = y,
                TileSize = tileSize,
                Texture = "lightblue"
            };
        }

        private class SnakePart
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string Texture { private get; set; }
            public Vector2 TileSize { get; set; }

            public void Draw(GameContentManager contentManager, SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(contentManager.Get<Texture2D>(Texture), new Vector2(X * (int)TileSize.X, Y * (int)TileSize.Y), Color.White);
            }
        }
    }    
}
