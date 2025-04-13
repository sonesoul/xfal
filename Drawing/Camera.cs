using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PixelBox.Drawing
{
    public delegate void DrawAction(DrawContext context);

    [DebuggerDisplay("{ToString(),nq}")]
    public class Camera : Canvas
    {
        public const int DefaultLayer = 0;

        public DrawContext Context { get; private set; }
        public bool IsVisible { get; set; } = true;

        //TODO: split RenderOptions between layers of camera instead of cameras 
        private readonly SortedDictionary<int, HashSet<DrawAction>> renderPipeline = new();

        public Camera(RenderSource source, Vector2 size) : base(source, size)
        {
            AddLayer(DefaultLayer);
        }

        public void AddLayer(int layer) => renderPipeline.Add(layer, new());
        public void RemoveLayer(int layer) => renderPipeline.Remove(layer);

        public void Register(DrawAction drawAction, int layer = DefaultLayer)
        {
            if (drawAction == null)
                throw new ArgumentException("DrawAction can't be null", nameof(drawAction));

            if (!renderPipeline.ContainsKey(layer))
                throw new ArgumentException("Layer doesn't exist");

            renderPipeline[layer].Add(drawAction);
        }
        public void Unregister(DrawAction drawAction, int layer = DefaultLayer) => renderPipeline[layer].Remove(drawAction);

        public void Render()
        {
            Begin();

            foreach (var layer in renderPipeline.Values)
            {
                foreach (var drawAct in layer)
                {
                    drawAct(Context);
                }
            }
            
            End();
        }
        
        protected override void SetSource(RenderSource source)
        {
            base.SetSource(source);
            Context = new(source);
        }
        public override string ToString() => $"{Position} {Size.X}x{Size.Y}";
    }
}