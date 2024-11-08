using UnityEngine;

namespace StorySystem.Editor
{
    public static class EditorConfig
    {
        public const string DEFAULT_ITEM_NAME   = "��������";
        public const string SAVE_MESSAGE        = "���������";
#if UNITY_EDITOR
        public static readonly string SAVE_FOLDER = Application.dataPath + "\\Game\\Story";
#else
        public static readonly string SAVE_FOLDER = Application.dataPath + "\\Story";
#endif
    }
}