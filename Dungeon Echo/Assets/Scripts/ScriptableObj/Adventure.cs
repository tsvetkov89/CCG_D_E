using System.Collections;
using System.Collections.Generic;
using EnumNamespace;
using UnityEngine;
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
        public string goodOutcome;
        public string badOutcome;
        public Sprite art;
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
