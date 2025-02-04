using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelBox.InputHandling
{
    public static class Input
    {
        public static event Action<Key> KeyPressed, KeyHolded, KeyReleased;

        public static AxisCulture AxisCulture { get; set; } = AxisCulture.WASD;
        public static Vector2 Axis => _axis;
        public static Vector2 MousePosition { get; private set; }

        public static Key[] PressedKeys => pressedKeys.ToArray();

        public static ref KeyboardState KeyState => ref _keyState;
        public static ref MouseState MouseState => ref _mouseState;

        private readonly static List<KeyBinding> binds = new();
        private readonly static HashSet<Key> wasPressed = new();
        private readonly static Queue<Key> toRemove = new();
        private readonly static Key[] mouseKeys =
        {
            Key.MouseLeft,
            Key.MouseRight,
            Key.MouseMiddle,
            Key.MouseX1,
            Key.MouseX2
        };
        private readonly static HashSet<Key> pressedKeys = new(Enum.GetValues(typeof(Key)).Length + mouseKeys.Length);

        private static Vector2 _axis = Vector2.Zero;
        private static MouseState _mouseState;
        private static KeyboardState _keyState;

        private const int MouseStartIndex = 1000;

        public static void Update(in KeyboardState keyboardState, in MouseState mouseState)
        {
            _keyState = keyboardState;
            _mouseState = mouseState;

            MousePosition = mouseState.Position.ToVector2();

            UpdatePressedKeys();

            foreach (var key in pressedKeys)
            {
                if (!wasPressed.Contains(key))
                {
                    wasPressed.Add(key);
                    KeyPressed?.Invoke(key);
                }
                else
                {
                    KeyHolded?.Invoke(key);
                }
            }

            foreach (var key in wasPressed)
            {
                if (!pressedKeys.Contains(key))
                {
                    toRemove.Enqueue(key);
                    KeyReleased?.Invoke(key);
                }
            }

            while (toRemove.Count > 0)
            {
                Key key = toRemove.Dequeue();
                wasPressed.Remove(key);
            }

            UpdateAxis();

            foreach (var item in binds)
            {
                item?.Update();
            }
        }

        private static void UpdatePressedKeys()
        {
            pressedKeys.Clear();

            foreach (var key in KeyState.GetPressedKeys())
            {
                pressedKeys.Add((Key)key);
            }

            foreach (var k in mouseKeys)
            {
                if (IsKeyDown(k))
                {
                    pressedKeys.Add(k);
                }
            }
        }

        public static void AddBind(KeyBinding binding) => binds.Add(binding);
        public static void RemoveBind(KeyBinding binding) => binds.Remove(binding);

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

        private static void UpdateAxis()
        {
            AxisCulture.Deconstruct(out var up, out var down, out var left, out var right);

            _axis.X = (IsKeyDown(left) ? 1 : 0) - (IsKeyDown(right) ? 1 : 0);
            _axis.Y = (IsKeyDown(up) ? 1 : 0) - (IsKeyDown(down) ? 1 : 0);
        }
    }
}