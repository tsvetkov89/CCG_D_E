using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Item", menuName = "Adventure")]
public class Adventure : ScriptableObject
{
    [Header("Описание приключения")] [Space]
    [Tooltip("Название приключения (код)")] public AdventureEvent adventureEvent;
    [Tooltip("Название приключения (игровое)")]public string displayName;
    [TextArea(3, 5)][Tooltip("Описание приключения")] public string description;
    [Tooltip("Арт приключения")] public Sprite artAdventure;
    [Tooltip("События приключения")]public List<Event> events;
    [System.Serializable] public class Event
    {
        public NameEvent nameEvent;
        public string label;
        [FormerlySerializedAs("Outcomes")] public List<DataOutcome> outcomes;
    }
    [System.Serializable]
    public class DataOutcome
    {
        public string name;
        public Sprite art;
        public string description;
        public List<RewordEvent> reword;
    }

    public DataEvent GetDataEvent()
    {
        var typeEvent = new DataEvent()
        {
            NameEvent = displayName, Art = artAdventure, Description = description,
            Events = events
        };
        return typeEvent;
    }
    
}
