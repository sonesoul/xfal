global using Microsoft.Xna.Framework;
global using PixelBox.Extensions;

using Microsoft.Xna.Framework.Input;
using System;
using PixelBox.InputHandling;

namespace PixelBox
{
    /// <summary>
    /// Provides methods to update systems such as <see cref="Input"/>, <see cref="Time"/> and <see cref="StepTask.Manager"/>.
    /// </summary>
    public static class Frame
    {
        public static event Action Update, FixedUpdate, Draw;

        private static float fixedUpdateBuffer = 0;

        public static void HandleUpdate(GameTime gameTime, KeyboardState keyState, MouseState mouseState)
        {
            Time.Update(gameTime);
            Input.Update(keyState, mouseState);
            StepTask.Manager.Update();
            
            Update?.Invoke();
            
            if ((fixedUpdateBuffer += Time.Delta) >= Time.FixedDelta)
            {
                FixedUpdate?.Invoke();
                fixedUpdateBuffer -= Time.FixedDelta;
            }
        }
        public static void HandleDraw(GameTime gameTime)
        {
            Time.Update(gameTime);
            Draw?.Invoke();
        }
        public static void HandlePostDraw(GameTime gameTime) => Time.Update(gameTime);
    }
}