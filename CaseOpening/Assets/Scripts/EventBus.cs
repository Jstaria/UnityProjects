using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : Singleton<EventBus>
{
    [SerializeField] private List<KeyValue<string, UnityEvent>> eventList;

    private Dictionary<string, UnityEvent> EventDict;

    public void RemoveListener(string name, UnityAction function)
    {
        if (EventDict == null)
            return;

        if (!EventDict.ContainsKey(name))
            return;

        EventDict[name].RemoveListener(function);
    }

    public void AddListener(string name, UnityAction function)
    {
        if (EventDict == null)
            CreateDict();

        if (!EventDict.ContainsKey(name))
            EventDict.Add(name, new UnityEvent());

        EventDict[name].AddListener(function);
    }

    private void CreateDict()
    {
        EventDict = new Dictionary<string, UnityEvent>();

        foreach (KeyValue<string, UnityEvent> pair in eventList)
        {
            EventDict.Add(pair.key, pair.value);
        }
    }

    public void NotifyListeners(string name)
    {
        if (EventDict == null)
            CreateDict();

        if (!EventDict.ContainsKey(name))
            return;

        EventDict[name].Invoke();
    }
}
