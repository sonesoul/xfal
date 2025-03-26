using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelBox.Drawing
{
    public class Canvas
    {
        public ref Vector2 Position => ref _position;
        public float Zoom { get; set; } = 1f;
        public ref Color BackgroundColor => ref _backgroundColor;
        public ref RenderOptions Options => ref _options;

        public RenderSource Source { get => _source; set => SetSource(value); }
        public Vector2 Size { get => _size; set => SetSize(value); } 
        

        public event Action Began, Ended;

        protected RectangleF _bounds;
        protected RenderSource _source;
        protected RenderTarget2D _target;

        protected Vector2 _position = Vector2.Zero;
        protected RenderOptions _options = new();
        protected Color _backgroundColor = Color.Black;

        protected Vector2 _size;

        public Canvas(RenderSource source, Vector2 size)
        {
            Source = source;
            SetSize(size);
        }
        
        public virtual void Begin()
        {
            var graphics = Source.Graphics;

            graphics.SetRenderTarget(_target);
            graphics.Clear(BackgroundColor);

            Source.SpriteBatch.Begin(Options, GetViewMatrix());
            Began?.Invoke();
        }
        public virtual void End()
        {
            Source.SpriteBatch.End();
            Source.Graphics.SetRenderTarget(null);
            Ended?.Invoke();
        }

        public virtual Matrix GetViewMatrix() => Matrix.CreateTranslation(new(-Position, 0)) * Matrix.CreateScale(Zoom, Zoom, 1);
        public virtual RenderTarget2D GetRenderTarget() => _target;

        protected virtual void SetBounds(RectangleF newBounds)
        {
            if (_bounds.Size != newBounds.Size)
            {
                SetSize(newBounds.Size);
            }

            _bounds.Location = newBounds.Location;
        }
        protected virtual void SetSize(Vector2 newSize)
        {
            _target = new(Source.Graphics, (int)newSize.X, (int)newSize.Y);
            _size = newSize;
        }
        protected virtual void SetSource(RenderSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source), "Render source can't be null."); ;
        }
    }
}