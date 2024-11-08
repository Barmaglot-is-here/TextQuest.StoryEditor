using UnityEngine;
using UnityEngine.EventSystems;

namespace StorySystem.Editor
{
    public class KeyboardHandler
    {
        private readonly StoryEditor _editor;

        private FileList FileList           => _editor.FileList;
        private ActionsBlock ActionsBlock   => _editor.ActionsBlock;

        private ListItem SelectedItem   => FileList.SelectedItem;
        private ItemData CurrentData    => SelectedItem.Data;

        public KeyboardHandler(StoryEditor editor)
        {
            _editor         = editor;
        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.O))
                    ActionsBlock.OpenSaveFolder();

                if (Input.GetKeyDown(KeyCode.P))
                    ActionsBlock.Run(CurrentData.Content);

                if (Input.GetKeyDown(KeyCode.S))
                    DataSaveUtility.Save(CurrentData);

                if (Input.GetKeyDown(KeyCode.N))
                    FileList.AddNew();
            }

            if (Input.GetKeyDown(KeyCode.Delete) &&
                EventSystem.current.currentSelectedGameObject == SelectedItem.gameObject)
                FileList.DestroySelected();

            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.LeftArrow))
                _editor.FocusOnFileList();

            if (Input.GetKeyUp(KeyCode.F2))
                _editor.FocusOnName();
        }
    }
}