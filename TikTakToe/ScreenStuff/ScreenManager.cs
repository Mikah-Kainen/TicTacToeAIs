using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.ScreenStuff
{
    public class ScreenManager
    {
        public List<IScreen> PreviousScreens { get; set; }
        public IScreen CurrentScreen { get; set; }

        public ScreenManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);   
        }
    }
}
