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

        public ScreenManager()
        {
            PreviousScreens = new List<IScreen>();
        }

        public void Update(GameTime gameTime)
        {
           PreviousScreens[0].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = PreviousScreens.Count - 1; i >= 0; i--)
            {
                PreviousScreens[i].Draw(spriteBatch);
            }
        }

        public void SetScreen(IScreen nextScreen)
        {
            PreviousScreens.Insert(0, nextScreen);
        }

        public void Clear()
        {
            PreviousScreens.Clear();
            GC.Collect();
        }
    }
}
