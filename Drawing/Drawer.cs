using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PixelBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelBox.Drawing
{
    public class Drawer
    {
        public delegate Rectangle ScaleFunction(in Vector2 source, in Rectangle target);

        public RenderSource Source => Canvas.Source;
        public RenderOptions Options { get; set; } = new();

        public Canvas Canvas { get; set; }

        public ScaleFunction ScaleFunc { get; set; } = null;
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
                SpriteBatch.Draw(item.RenderTarget, new Rectangle(windowBounds.Location, Canvas.Size.ToPoint()), Color.White);
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
                destination = ScaleFunc.Invoke(Canvas.Size, Graphics.Viewport.Bounds);
                windowBounds = Graphics.Viewport.Bounds;
            }

            batch.Begin(Options);
            
            batch.Draw(
                Canvas.RenderTarget,
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

            return (canvasPoint / (Canvas.Size / camera.Size) + camera.Position).Floored();
        }
        public Vector2 ScreenToWorldPoint(Vector2 point) => ScreenToWorldPoint(point, MainCamera);

        public static Vector2 NormalizePoint(Vector2 point, Rectangle destination)
        {
            return (point - destination.Location.ToVector2()) / destination.Size.ToVector2();
        }
    }
}