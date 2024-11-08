using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Editor
{
    public class TextEditor : MonoBehaviour
    {
        private TMP_InputField _textInputField;
        private Scrollbar _scrollbar;
        private RectTransform _textRect;

        private ItemData _curentData;

        private string DataText { get => _curentData.Content; set => _curentData.Content = value; }

        private bool _hasChanged;

        public void Init(FileList fileList)
        {
            _textInputField = GetComponent<TMP_InputField>();
            _textRect       = _textInputField.textComponent.GetComponent<RectTransform>();
            _scrollbar      = transform.GetComponentInChildren<Scrollbar>();

            fileList.OnSelectionChanged += OnSelectionChanged;

            _textInputField.onValueChanged  .AddListener(OnTextChanged);
            _textInputField.onEndEdit       .AddListener(_ => OnTextEditEnd());
        }

        public void OnSelectionChanged(ItemData data)
        {
            _curentData                 = data;
            _textInputField.text        = data.Content;
            _textRect.anchoredPosition  = Vector2.zero;
            _scrollbar.value            = 0;
        }

        private void OnTextChanged(string text)
        {
            if (DataText == text)
                return;
                
            DataText    = text;
            _hasChanged = true;
        }

        private void OnTextEditEnd()
        {
            if (_hasChanged)
                DataSaveUtility.Save(_curentData);
        }
    }
}