using System;

namespace PixelBox
{
    public class YieldInstruction
    {
        public delegate void RefFloat(ref float e);

        public IEnumerator WaitWhile(Func<bool> condition)
        {
            while (condition())
            {
                yield return null;
            }
        }
        public IEnumerator WaitForFixedUpdate()
        {
            bool exit = false;
            void TriggerExit() => exit = true;

            Frame.FixedUpdate += TriggerExit;
            while (!exit)
            {
                yield return null;
            }
            Frame.FixedUpdate -= TriggerExit;
        }
        public IEnumerator WaitForDraw() 
        {
            bool exit = false;
            void TriggerExit() => exit = true;

            Frame.Draw += TriggerExit;
            while (!exit)
            {
                yield return null;
            }
            Frame.Draw -= TriggerExit;
        }

        public IEnumerator Interpolate(Func<float, float> action)
        {
            float elapsed = 0;

            while (elapsed >= 0 && elapsed <= 1)
            {
                elapsed = action(elapsed);

                if (elapsed < 0 || elapsed > 1)
                {
                    yield return null;

                    action(elapsed.Clamp01());

                    break;
                }

                yield return null;
            }
        }
    }
}