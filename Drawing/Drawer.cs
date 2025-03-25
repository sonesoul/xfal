using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelBox.Drawing
{
    public class Drawer<T> where T : IRenderer
    {
        public Color BackgroundColor { get; set; } = Color.Black;
        
        public RenderSource Source => Canvas.Source;
        public RenderOptions Options { get; set; } = new();

        public Canvas Canvas { get; set; }
        public bool UseStretching { get; set; } = false;

        private GraphicsDevice Graphics => Source.Graphics;
        private SpriteBatch SpriteBatch => Source.SpriteBatch;

        private readonly SortedDictionary<int, T> renderers = new();

        private Rectangle destination;
        private Rectangle windowBounds;
        private RectangleF canvasBounds;

        public Drawer(Canvas canvas)
        {
            Canvas = canvas;
        }
        public Drawer(RenderSource source, Vector2 size) : this(new Canvas(source, size)) { }

        public void Add(T item, int order) => renderers.Add(order, item);
        public void Add(T item)
        {
            Add(item, renderers.Count == 0 ? 0 : renderers.Last().Key + 1);
        }
        public void Remove(int order) => renderers.Remove(order);
        public T GetByOrder(int order) => renderers[order];

        public void Draw()
        {
            RenderAll();
            Clear();
            DrawCanvas();
        }

        public void RenderAll()
        {
            foreach (var item in renderers.Values)
            {
                item.Render();
            }

            Canvas.Begin();

            foreach (var item in renderers.Values)
            {
                SpriteBatch.Draw(item.CurrentPicture, item.Bounds.Location, Color.White);
            }

            Canvas.End();
        }
        public void DrawCanvas()
        {
            SpriteBatch batch = SpriteBatch;

            if (canvasBounds != Canvas.Bounds || windowBounds != Graphics.Viewport.Bounds)
            {
                destination = GetDestination();

                windowBounds = Graphics.Viewport.Bounds;
                canvasBounds = Canvas.Bounds;
            }

            batch.Begin(Options);
            
            batch.Draw(
                Canvas.GetRenderTarget(),
                destination, 
                Color.White);

            batch.End();
        }
        public void Clear()
        {
            Graphics.Clear(BackgroundColor);
        }

        public Camera CreateCamera()
        {
            return new(Source, Canvas.Size)
            {
                BackgroundColor = Canvas.BackgroundColor,
                Options = Options
            };
        }

        public Vector2 ScreenToCanvasPoint(Vector2 point)
        {
            Vector2 normalizedPoint = NormalizePoint(point, destination);
            Vector2 canvasPoint = normalizedPoint * Canvas.Size;

            return ((canvasPoint + Canvas.Location) / Canvas.Zoom).Floored();
        }
        public Vector2 ScreenToWorldPoint(Vector2 point, Camera camera)
        {
            Vector2 canvasPoint = ScreenToCanvasPoint(point);
            float zoom = camera.Zoom;
            
            return (canvasPoint / zoom) + camera.Position - (camera.Bounds.Location / zoom);
        }

        public Vector2 NormalizePoint(Vector2 point, Rectangle destination)
        {
            return (point - destination.Location.ToVector2()) / destination.Size.ToVector2();
        }
        
        public Rectangle GetDestination()
        {
            if (UseStretching)
            {
                return StretchRectangle(Canvas.Bounds, Graphics.Viewport.Bounds);
            }
            else
            {
                return FitRectangle(Canvas.Bounds, Graphics.Viewport.Bounds);
            }
        }

        public static Rectangle FitRectangle(in RectangleF source, in Rectangle target)
        {
            Vector2 sourceSize = source.Size;
            Vector2 targetSize = target.Size.ToVector2();

            Point size = targetSize.ToPoint();
            Point location = target.Location + source.Location.ToPoint();

            int targetWidth = target.Width;
            int targetHeight = target.Height;

            float originalAspect = sourceSize.X / sourceSize.Y;
            float targetAspect = (float)targetWidth / targetHeight;

            //target rect wider than original
            if (targetAspect > originalAspect)
            {
                int finalWidth = (int)(size.Y * originalAspect);

                Point finalLocation = new Point(
                    (targetWidth - finalWidth) / 2,
                    (targetHeight / 2) - (size.Y / 2)) + location;

                Point finalSize = new(finalWidth, size.Y);

                return new(finalLocation, finalSize);
            }
            //target rect higher than original
            else
            {
                int finalHeight = (int)(size.X / originalAspect);

                Point finalLocation = new Point(
                    (targetWidth / 2) - (size.X / 2), 
                    (targetHeight - finalHeight) / 2) + location;

                Point finalSize = new(size.X, finalHeight);

                return new(finalLocation, finalSize);
            }
        }
        public static Rectangle StretchRectangle(in RectangleF source, in Rectangle target)
        {
            Vector2 targetSize = target.Size.ToVector2();

            Point size = targetSize.ToPoint();
            Point location = target.Location + source.Location.ToPoint();

            return new Rectangle(location, size);
        }
    }
}