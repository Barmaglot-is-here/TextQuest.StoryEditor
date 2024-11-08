using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Editor
{
    public class ActionsBlock : MonoBehaviour
    {
        public Button OpenSaveFolderButton { get; private set; }
        public Button RunButton { get; private set; }

        private ItemData _itemData;

        private void Awake()
        {
            OpenSaveFolderButton    = transform.GetChild(0).GetComponent<Button>();
            RunButton               = transform.GetChild(1).GetComponent<Button>();

            OpenSaveFolderButton.onClick   .AddListener(OpenSaveFolder);
            RunButton.onClick              .AddListener(() => Run(_itemData.Content));

            var fileList = FindFirstObjectByType<FileList>();
            fileList.OnSelectionChanged += (_ => _itemData = _);
        }

        public void OpenSaveFolder() => Process.Start(EditorConfig.SAVE_FOLDER);

        public void Run(string source)
        {

        }
    }
}