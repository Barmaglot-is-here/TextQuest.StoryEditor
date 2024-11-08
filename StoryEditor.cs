using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Editor
{
    public class StoryEditor : MonoBehaviour
    {
        public FileList FileList { get; private set; }
        public TextEditor TextEditor { get; private set; }
        public NameEditor NameEditor { get; private set; }
        public ActionsBlock ActionsBlock { get; private set; }

        private KeyboardHandler _keyboardHandler;

        private Selectable _textInputField;
        private TMP_InputField _nameInputField;

        private void Awake()
        {
            FileList       = FindFirstObjectByType<FileList>();
            TextEditor     = FindFirstObjectByType<TextEditor>();
            NameEditor     = FindFirstObjectByType<NameEditor>();
            ActionsBlock   = FindFirstObjectByType<ActionsBlock>();

            _textInputField = TextEditor.GetComponent<Selectable>();
            _nameInputField = NameEditor.GetComponent<TMP_InputField>();

            DataSaveUtility.OnSave          += () => Notification.Send(EditorConfig.SAVE_MESSAGE);
            DataSaveUtility.OnNameChanged   += () => Notification.Send(EditorConfig.SAVE_MESSAGE);

            FileList       .Init();
            TextEditor     .Init(FileList);
            NameEditor     .Init(FileList);

            _keyboardHandler = new(this);
            _nameInputField.onEndEdit.AddListener(_ => FocusOnText());
        }

        private void Update() => _keyboardHandler.Update();

        public void FocusOnText()       => _textInputField.Select();
        public void FocusOnName()       => _nameInputField.Select();
        public void FocusOnFileList()   => FileList.SelectedItem.GetComponent<Selectable>().Select();

        public void Close()
        {
            OnApplicationQuit();

            gameObject.SetActive(false);
        }

        private void OnApplicationQuit() => DataSaveUtility.Save(FileList.SelectedItem.Data);
    }
}