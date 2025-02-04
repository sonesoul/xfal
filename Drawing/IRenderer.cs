using Microsoft.Xna.Framework.Graphics;

namespace PixelBox.Drawing
{
    public interface IRenderer : ICanvas
    {
        public Texture2D CurrentPicture { get; }
        public bool IsVisible { get; set; }
        public void Render();
    }
}