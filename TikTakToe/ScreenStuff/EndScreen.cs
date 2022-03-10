using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using NeuralNetwork;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.ScreenStuff
{
    public class EndScreen : IScreen
    {
        public Color Winner { get; set; }
        public List<GameObject> Objects { get; set; }
        private Sprite replayButton;
        private Sprite saveButton;
        private Sprite loadButton;
        private NeuralNet endNet;
        public EndScreen(Color winner, NeuralNet endNet, Rectangle screen)
        {
            Vector2 windowSize = new Vector2(400, 200);
            Vector2 pos = new Vector2((screen.Width - windowSize.X) / 2f, (screen.Height - windowSize.Y) / 2f);
            Objects = new List<GameObject>();
            replayButton = new Sprite(Game1.WhitePixel, winner, pos, Vector2.One, windowSize, Vector2.Zero);
            saveButton = new Sprite(Game1.WhitePixel, Color.DarkGoldenrod, new Vector2(pos.X, pos.Y + windowSize.Y), Vector2.One, new Vector2(windowSize.X, 50), Vector2.Zero);
            loadButton = new Sprite(Game1.WhitePixel, Color.Purple, new Vector2(pos.X, pos.Y + windowSize.Y + 50), Vector2.One, new Vector2(windowSize.X, 50), Vector2.Zero);
            Objects.Add(replayButton);
            Objects.Add(saveButton);
            Objects.Add(loadButton);
            this.endNet = endNet;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].Update(gameTime);
            }
            Game1.InputManager.Update(gameTime);
            if (Game1.InputManager.PreviousMouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && Game1.InputManager.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                if (replayButton.HitBox.Contains(Game1.InputManager.Mouse.Position))
                {
                    Game1.ScreenManager.Clear();
                    Game1.ScreenManager.SetScreen(new GridBoardVersionPlayScreen(null));
                }
                else if(saveButton.HitBox.Contains(Game1.InputManager.Mouse.Position))
                {
                    Game1.ScreenManager.Clear();
                    endNet.SaveToFile("NeuralNet.json");
                    Game1.ScreenManager.SetScreen(new GridBoardVersionPlayScreen(endNet));
                }
                else if(loadButton.HitBox.Contains(Game1.InputManager.Mouse.Position))
                {
                    Game1.ScreenManager.Clear();
                    Game1.ScreenManager.SetScreen(new GridBoardVersionPlayScreen(NeuralNetwork.TurnBasedBoardGameTrainerStuff.TurnBasedBoardGameTrainer<GridBoardState, GridBoardSquare, MoveStats>.LoadNet("NeuralNet.json")));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < Objects.Count; i ++)
            {
                Objects[i].Draw(spriteBatch);
            }
        }

    }
}
