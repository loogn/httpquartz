using System;

namespace HttpQuartz.Client.Models
{
    [Serializable]
    public class TriggerKeyModel
    {
        public TriggerKeyModel()
        {
        }

        public TriggerKeyModel(string name, string group)
        {
            Name = name;
            Group = group;
        }

        public string Name { get; set; }
        public string Group { get; set; }
    }
}