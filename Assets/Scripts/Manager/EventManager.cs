using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Manager
{
    public class EventParameters : Attribute
    {
        public readonly Type[] Types;
        public readonly string Message;

        public EventParameters(string message)
        {
            Message = message;
        }
        public EventParameters(Type[] types, string message)
        {
            Message = message;
            Types = types;
        }
    }

    public class EventManager : MonoBehaviour
    {
        // A dictionary of all the actions that will be triggered by the events
        private Dictionary<EventKeys, Action<object[]>> _eventDictionary;

        // Singleton pattern for the EventManager
        private static EventManager _eventManager;

        private static EventManager Instance
        {
            get
            {
                if (_eventManager)
                {
                    return _eventManager;
                }

                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (_eventManager)
                {
                    _eventManager.Init();
                }
                else
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }

                return _eventManager;
            }
        }

        private void Init()
        {
            _eventDictionary ??= new Dictionary<EventKeys, Action<object[]>>();
        }

        // Function to start listening to an event
        public static void StartListening(EventKeys eventType, Action<object[]> listener)
        {
            if (Instance._eventDictionary.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent += listener;
            }
            else
            {
                thisEvent = listener;
            }

            Instance._eventDictionary[eventType] = thisEvent;
        }

        public static void StopListening(EventKeys eventType, Action<object[]> listener)
        {
            if (_eventManager == null)
            {
                return;
            }

            if (Instance._eventDictionary.TryGetValue(eventType, out var thisEvent))
            {
                thisEvent -= listener;
                Instance._eventDictionary[eventType] = thisEvent;
            }
        }

        // Function to trigger an event
        public static void TriggerEvent(EventKeys eventType, params object[] parameters)
        {
            if (!Instance._eventDictionary.TryGetValue(eventType, out var thisEvent))
            {
                return;
            }

            if (!TriggerEventParameterCheck(eventType, parameters))
            {
                return;
            }

            Debug.Log($"Triggering event {eventType}, invocationListLength: {thisEvent.GetInvocationList().Length}");

            foreach (var singleCast in thisEvent.GetInvocationList().Cast<Action<object[]>>())
            {
                try
                {
                    singleCast.Invoke(parameters);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }

        private static bool TriggerEventParameterCheck(EventKeys eventType, IReadOnlyList<object> parameters)
        {
            var eventParamsAttr = typeof(EventKeys).GetField(eventType.ToString()).GetCustomAttribute<EventParameters>();

            if (eventParamsAttr == null)
            {
                Debug.LogError($"Error: {eventType} does not have an EventParameters attribute.");
                return false;
            }

            if (eventParamsAttr.Types == null)
            {
                Debug.LogError($"Error: {eventType} does not have any expected parameter types.");
                return false;
            }

            // Check if the number of parameters matches
            if (eventParamsAttr.Types.Length != parameters.Count)
            {
                Debug.LogError($"Error: {eventType} expects {eventParamsAttr.Types.Length} parameters, but received {parameters.Count}.");
                return false;
            }

            // Check if the parameter types match
            for (var i = 0; i < eventParamsAttr.Types.Length; i++)
            {
                if (eventParamsAttr.Types[i] == parameters[i].GetType())
                {
                    continue;
                }

                Debug.LogError($"Error: {eventType} expects parameter {i + 1} to be of type {eventParamsAttr.Types[i]}, but received {parameters[i].GetType()}.");
                return false;
            }

            return true;
        }
    }
}