using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelBox.Drawing
{
    public class Drawer
    {
        public RenderSource Source => Canvas.Source;
        public RenderOptions Options { get; set; } = new();

        public Canvas Canvas { get; set; }

        public bool UseStretching { get; set; } = false;
        public Color BackgroundColor { get; set; } = Color.Black;

        public Camera MainCamera { get; }

        private GraphicsDevice Graphics => Source.Graphics;
        private SpriteBatch SpriteBatch => Source.SpriteBatch;

        private readonly SortedDictionary<int, Camera> renderers = new();

        private Rectangle destination;
        private Rectangle windowBounds;

        public Drawer(Canvas canvas)
        {
            Canvas = canvas;
            MainCamera = CreateCamera();
        }
        public Drawer(RenderSource source, Vector2 size) : this(new Canvas(source, size)) { }

        public void AddCamera(Camera item, int order) => renderers.Add(order, item);
        public void AddCamera(Camera item)
        {
            AddCamera(item, renderers.Count > 0 ? renderers.Last().Key + 1 : 0);
        }
        public void RemoveCamera(int order) => renderers.Remove(order);
        public Camera GetCamera(int order) => renderers[order];

        public void Draw()
        {
            RenderAll();
            Clear();
            DrawCanvas();
        }

        public void RenderAll()
        {
            MainCamera.Render();

            foreach (var item in renderers.Values)
            {
                item.Render();
            }

            Canvas.Begin();

            void DrawItem(Camera item)
            {
                SpriteBatch.Draw(item.CurrentPicture, new Rectangle(windowBounds.Location, Canvas.Size.ToPoint()), Color.White);
            }

            DrawItem(MainCamera);

            foreach (var item in renderers.Values)
            {
                DrawItem(item);
            }

            Canvas.End();
        }
        public void DrawCanvas()
        {
            SpriteBatch batch = SpriteBatch;

            if (windowBounds != Graphics.Viewport.Bounds)
            {
                destination = GetDestination();

                windowBounds = Graphics.Viewport.Bounds;
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

            return canvasPoint.Floored();
        }
        public Vector2 ScreenToWorldPoint(Vector2 point, Camera camera)
        {
            Vector2 canvasPoint = ScreenToCanvasPoint(point);

            return (canvasPoint / (Canvas.Size / camera.Size)).Floored();
        }
        public Vector2 ScreenToWorldPoint(Vector2 point) => ScreenToWorldPoint(point, MainCamera);

        public static Vector2 NormalizePoint(Vector2 point, Rectangle destination)
        {
            return (point - destination.Location.ToVector2()) / destination.Size.ToVector2();
        }
        
        public Rectangle GetDestination()
        {
            if (UseStretching)
            {
                return Graphics.Viewport.Bounds;
            }
            else
            {
                return FitRectangle(Canvas.Size, Graphics.Viewport.Bounds.Size.ToVector2());
            }
        }

        public static Rectangle FitRectangle(in Vector2 sourceSize, in Vector2 targetSize)
        {
            Point size = targetSize.ToPoint();
            
            int targetWidth = (int)targetSize.X;
            int targetHeight = (int)targetSize.Y;

            float originalAspect = sourceSize.X / sourceSize.Y;
            float targetAspect = (float)targetWidth / targetHeight;

            //target rect wider than original
            if (targetAspect > originalAspect)
            {
                int finalWidth = (int)(size.Y * originalAspect);

                Point finalLocation = new(
                    (targetWidth - finalWidth) / 2,
                    (targetHeight / 2) - (size.Y / 2));

                Point finalSize = new(finalWidth, size.Y);

                return new(finalLocation, finalSize);
            }
            //target rect higher than original
            else
            {
                int finalHeight = (int)(size.X / originalAspect);

                Point finalLocation = new(
                    (targetWidth / 2) - (size.X / 2), 
                    (targetHeight - finalHeight) / 2);

                Point finalSize = new(size.X, finalHeight);

                return new(finalLocation, finalSize);
            }
        }
        public static Rectangle StretchRectangle(in Vector2 sourceSize, in Rectangle target)
        {
            Vector2 targetSize = target.Size.ToVector2();

            Point size = targetSize.ToPoint();
            Point location = target.Location;
            
            return new Rectangle(location, size);
        }
    }
}