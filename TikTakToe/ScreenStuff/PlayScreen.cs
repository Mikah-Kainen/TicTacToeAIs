using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;
using TikTakToe.Players;

namespace TikTakToe.ScreenStuff
{
    public class PlayScreen : IScreen
    {
        public List<GameObject> Objects { get; set; }
        public Sprite[][] Tiles { get; set; }

        public Color NextColor { get; set; }
        public Player Player { get; set; }

        public PlayScreen()
        {
            Objects = new List<GameObject>();
            Tiles = new Sprite[3][];

            for (int y = 0; y < 3; y ++)
            {
                Tiles[y] = new Sprite[3];
                for (int x = 0; x < 3; x ++)
                {
                    Tiles[y][x] = new Sprite(Game1.WhitePixel, Color.White, new Vector2(50 + 105 * x, 50 + 105 * y), Vector2.One, new Vector2(100, 100), Vector2.Zero);
                    Objects.Add(Tiles[y][x]);
                }
            }

            NextColor = Color.Red;
            Player = new BasicPlayer(Color.Red);

            Tiles[0][0].Tint = Color.Red;
            Tiles[2][2].Tint = Color.Red;
            Tiles[1][2].Tint = Color.Red;
        }

        public void Update(GameTime gameTime)
        {
            double[] simulatingOutputs = new double[]
            {
                0,
                0,
                0,

                1,
                0,
                0,

                0,
                0,
                0,
            };

            //UpdateTile(GetTile(simulatingOutputs));
            var result = Player.SelectTile(Tiles);
            result.Tint = Color.Red;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < Objects.Count; i ++)
            {
                Objects[i].Draw(spriteBatch);
            }
        }

        public Sprite GetTile(double[] outputs)
        {
            Sprite returnValue = null;
            for(int i = 0; i < outputs.Length; i ++)
            {
                if(outputs[i] == 1)
                {
                    if (returnValue == null)
                    {
                        returnValue = Tiles[i / Tiles.Length][i % Tiles.Length];
                    }
                    else
                    {
                        throw new Exception("Multiple Tiles Selected");
                    }
                }
            }
            if(returnValue != null)
            {
                return returnValue;
            }
            throw new Exception("No Tile Selected");
        }

        public void UpdateTile(Sprite targetTile)
        {
            if(targetTile.Tint != Color.White)
            {
//                throw new Exception("Tile Was Already Selected");
            }
            targetTile.Tint = NextColor;
            if (NextColor == Color.Red)
            {
                NextColor = Color.Blue;
            }
            else
            {
                NextColor = Color.Red;
            }
        }

    }
}
