using System;

namespace StorySystem.Editor
{
    public class ItemData
    {
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;

                OnNameChanged?.Invoke(_name);
            }
        }
        public string Content { get; set; }

        public Action<string> OnNameChanged { get; set; }

        public ItemData(string name) : this(name, default) { }

        public ItemData(string name, string content)
        {
            _name   = name;
            Content = content;
        }
    }
}