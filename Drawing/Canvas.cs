
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PixelBox.Drawing
{
    public interface ICanvas
    {
        public ref RenderOptions Options { get; }
        public RectangleF Bounds { get; }
    }

    public class Canvas : ICanvas
    {
        public ref Vector2 Position => ref _position;
        public float Zoom { get; set; } = 1f;
        public ref int BackgroundColor => ref _backgroundColor;
        public ref RenderOptions Options => ref _options;

        public RectangleF Bounds { get => _bounds; set => SetBounds(value); }
        public RenderSource Source { get => _source; set => SetSource(value); }
        public Vector2 Size { get => Bounds.Size; set => SetSize(value); } 
        public ref Vector2 Location => ref _bounds.Location;

        public event Action Began, Ended;

        protected RectangleF _bounds;
        protected RenderSource _source;
        protected RenderTarget2D _target;

        protected Vector2 _position = Vector2.Zero;
        protected RenderOptions _options = new();
        protected int _backgroundColor = Palette.Black;

        public Canvas(RenderSource source, Vector2 size)
        {
            Source = source;
            Bounds = new(Vector2.Zero, size);
        }
        
        public virtual void Begin()
        {
            var graphics = Source.Graphics;

            graphics.SetRenderTarget(_target);
            graphics.Clear(Palette.GetColor(BackgroundColor));

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
            _bounds.Size = newSize;
        }
        protected virtual void SetSource(RenderSource source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source), "Render source can't be null."); ;
        }
    }
}