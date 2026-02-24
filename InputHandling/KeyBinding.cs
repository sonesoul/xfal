using System;

namespace xfal.InputHandling
{
    public class KeyBinding
    {
        public Action Action { get; set; }

        public KeyPhase TriggerPhase { get; set; }
        public Key Key { get; set; }
        public bool IsDown { get; private set; }

        private bool wasDown = false;

        public KeyBinding(Key key, KeyPhase keyPhase, Action action)
        {
            Key = key;
            TriggerPhase = keyPhase;
            Action = action;
        }

        public void Update()
        {
            bool isDown = IsDown = Input.IsKeyDown(Key);

            if (TriggerPhase == KeyPhase.Press)
            {
                if (!wasDown && isDown)
                    Action?.Invoke();
            }
            else if (TriggerPhase == KeyPhase.Hold) 
            {
                if (wasDown && isDown)
                    Action?.Invoke();
            }
            else if (TriggerPhase == KeyPhase.Release)
            {
                if (wasDown && !isDown)
                    Action?.Invoke();
            }

            wasDown = isDown;
        }

        public void Unbind() => Input.RemoveBind(this);
    }
}