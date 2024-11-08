using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StorySystem.Editor
{
    internal static class DataSaveUtility
    {
        public static Action OnSave { get; set; }
        public static Action OnNameChanged { get; set; }

        public static void Save(ItemData data)
        {
            string savePath     = GetDataPath(data);

            if (!Directory.Exists(EditorConfig.SAVE_FOLDER))
                Directory.CreateDirectory(EditorConfig.SAVE_FOLDER);

            File.WriteAllText(savePath, data.Content);

            OnSave.Invoke();
        }

        public static IEnumerable<ItemData> LoadItems()
        {
            if (!Directory.Exists(EditorConfig.SAVE_FOLDER))
            {
                Directory.CreateDirectory(EditorConfig.SAVE_FOLDER);

                return Enumerable.Empty<ItemData>();
            }

            return Directory.EnumerateFiles(EditorConfig.SAVE_FOLDER)
                .Where(f => f.EndsWith(StoryConfig.FILE_EXTENSION))
                .Select(f => Load(f));
        }

        private static ItemData Load(string path)
        {
            string name     = Path.GetFileNameWithoutExtension(path);
            string content  = File.ReadAllText(path);

            return new(name, content);
        }

        public static void ChangeName(ItemData data, string oldName)
        {
            string oldPath = GetDataPath(oldName);
            string newPath = GetDataPath(data);

            File.Move(oldPath, newPath);

            OnNameChanged.Invoke();
        }

        public static void Delete(ItemData data)
        {
            string path = GetDataPath(data);

            File.Delete(path);
        }

        private static string GetDataPath(ItemData data)
            => GetDataPath(data.Name);
        private static string GetDataPath(string name)
            => Path.Combine(EditorConfig.SAVE_FOLDER, name + StoryConfig.FILE_EXTENSION);
    }
}