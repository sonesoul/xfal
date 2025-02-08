namespace PixelBox
{
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