using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace PixelBox
{
    public class Asset<T>
    {
        public static ContentManager Content { get; set; }

        public T Value { get; private set; } 

        public string Name { get; private set; }
        public string FilePath { get; private set; }

        public Asset(string path)
        {
            Value = Load(path);
            FilePath = path;
            Name = Path.GetFileNameWithoutExtension(path);
        }

        public static T Load(string path) => Content.Load<T>(path);
        public static List<Asset<T>> LoadFolder(string folder)
        {
            string folderPath = GetFolderPath(folder);

            string[] filePaths = Directory.GetFiles(folderPath, "*.xnb");
            
            List<Asset<T>> assets = new();
            foreach (string filePath in filePaths)
            {
                string name = Path.GetFileNameWithoutExtension(filePath);
                string path = Path.Combine(folder, name);

                assets.Add(new Asset<T>(path));
            }

            return assets;
        }

        public static string GetFolderPath(string folder)
        {
            return Path.Combine(Environment.CurrentDirectory, Content.RootDirectory, folder);
        }   

        public static implicit operator T(Asset<T> asset) => asset.Value;
    }
}