using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe.ScreenStuff
{
    public class ScreenManager
    {
        public List<IScreen> PreviousScreens { get; private set; }
        public IScreen CurrentScreen { get; private set; }

        public ScreenManager()
        {

        }

        public void Update(GameTime gameTime)
        {
            CurrentScreen.Update(gameTime);
            foreach(IScreen screen in PreviousScreens)
            {
                screen.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentScreen.Draw(spriteBatch);   
            foreach(IScreen screen in PreviousScreens)
            {
                screen.Draw(spriteBatch);
            }
        }

        public void SetScreen(IScreen nextScreen)
        {
            if (CurrentScreen != null)
            {
                PreviousScreens.Insert(0, CurrentScreen);
            }
            CurrentScreen = nextScreen;
        }
    }
}
