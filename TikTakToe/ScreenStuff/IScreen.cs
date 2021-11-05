using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Text;

using TikTakToe.DrawStuff;

namespace TikTakToe.ScreenStuff
{
    public interface IScreen
    {
        public List<GameObject> Objects { get; set; }

        public void Update(GameTime gameTime);
        public void Draw(SpriteBatch spriteBatch);
    }
}
