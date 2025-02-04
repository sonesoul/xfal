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
    public static class Time
    {
        public static float Delta { get; private set; }
        public static float RealDelta { get; private set; }
        public static float FixedDelta { get; set; } = 1.0f / 120;

        public static GameTime GameTime { get; private set; }
       
        public static float TimeScale { get => _timeScale; set => _timeScale = value.ClampMin(0); }

        private static float _timeScale = 1f;

        public static void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            
            RealDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Delta = RealDelta * TimeScale;
        }
    }
}