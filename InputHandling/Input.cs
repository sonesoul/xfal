using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace xfal.InputHandling
{
    public static class Input
    {
        public static event Action<Key> KeyPressed, KeyReleased;

        public static Vector2 MousePosition { get; private set; }

        public static ref KeyboardState KeyState => ref _keyState;
        public static ref MouseState MouseState => ref _mouseState;

        private readonly static List<KeyBinding> _binds = new();

        private readonly static Key[] _allKeys = Enum.GetValues<Key>();
        private readonly static int _keyCount = _allKeys.Length;
        
        private readonly static bool[] _previous = new bool[_keyCount];

        private static MouseState _mouseState;
        private static KeyboardState _keyState;

        private const int MouseStartIndex = 1000;

        public static void Update(in KeyboardState keyboardState, in MouseState mouseState)
        {
            _keyState = keyboardState;
            _mouseState = mouseState;

            MousePosition = mouseState.Position.ToVector2();

            for (int i = 0; i < _keyCount; i++)
            {
                Key k = _allKeys[i];  
                
                bool isDown = IsKeyDown(k);
                bool wasDown = _previous[i];

                if (!wasDown && isDown)
                {
                    KeyPressed?.Invoke(k);
                }
                if (wasDown && !isDown)
                {
                    KeyReleased?.Invoke(k);
                }

                _previous[i] = isDown;
            }

            foreach (var item in _binds)
            {
                item?.Update();
            }
        }

        public static void AddBind(KeyBinding binding) => _binds.Add(binding);
        public static void RemoveBind(KeyBinding binding) => _binds.Remove(binding);

        public static KeyBinding Bind(Key key, KeyPhase phase, Action action)
        {
            KeyBinding binding = new(key, phase, action);

            AddBind(binding);
            return binding;
        }
        public static void BindSingle(Key key, KeyPhase phase, Action action)
        {
            KeyBinding binding = null;
            
            void AutoUnbind()
            {
                action?.Invoke();
                RemoveBind(binding);
            }

            binding = new(key, phase, AutoUnbind);
            AddBind(binding);
        }
        public static void BindSingle(KeyBinding binding)
        {
            Action action = binding.Action;

            void AutoUnbind()
            {
                action?.Invoke();
                RemoveBind(binding);
            }

            binding.Action = AutoUnbind;
            AddBind(binding);
        }

        public static bool IsKeyDown(Key key)
        {
            if ((int)key >= MouseStartIndex) //is it a mouse key
            {
                return IsMouseKeyDown(key);
            }
            else
            {
                return KeyState.IsKeyDown((Keys)key);
            }
        }
        private static bool IsMouseKeyDown(Key key) => key switch
        {
            Key.MouseLeft => MouseState.LeftButton == ButtonState.Pressed,
            Key.MouseRight => MouseState.RightButton == ButtonState.Pressed,
            Key.MouseMiddle => MouseState.MiddleButton == ButtonState.Pressed,
            Key.MouseX1 => MouseState.XButton1 == ButtonState.Pressed,
            Key.MouseX2 => MouseState.XButton2 == ButtonState.Pressed,
            _ => throw new InvalidOperationException($"\"{key}\" is not a mouse key or it doesn't exist in the {nameof(Key)} enumeration."),
        };
    }
}