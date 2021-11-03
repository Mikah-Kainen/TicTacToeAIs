using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Text;

namespace TikTakToe
{
    public class InputManager
    {
        public KeyboardState PreviousKeyBoard { get; set; }
        public KeyboardState KeyBoard { get; set; }
        public MouseState Mouse { get; set; }
        public InputManager()
        {
            KeyBoard = Keyboard.GetState();
            Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        public void Update(GameTime gameTime)
        {
            PreviousKeyBoard = KeyBoard;
            KeyBoard = Keyboard.GetState();
            Mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }


        public bool WasKeyPressed(Keys targetKey)
        {
            if(PreviousKeyBoard == null)
            {
                return false;
            }
            return PreviousKeyBoard.IsKeyDown(targetKey) && KeyBoard.IsKeyUp(targetKey);
        }
    }
}
