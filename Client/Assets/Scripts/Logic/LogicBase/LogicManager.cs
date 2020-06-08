using System;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour
{
    private static LogicManager inst;

    [SerializeField]
    private Logic[] logics;

    private Dictionary<Type, Logic> logicHasMap = new Dictionary<Type, Logic>();

    private void Awake()
    {
        inst = this;

        for (int i = 0; i < logics.Length; i++)
        {
            logics[i] = Instantiate(logics[i]);

            logicHasMap.Add(logics[i].GetType(), logics[i]);
        }

        foreach (var item in logics)
        {
            item.Init();
        }
    }

    private void OnEnable()
    {
        foreach (var item in logics)
        {
            item.MyOnEnable();
        }
    }

    private void OnDisable()
    {
        foreach (var item in logics)
        {
            item.MyOnDisable();
        }
    }

    private void OnDestroy()
    {
        foreach (var item in logics)
        {
            item.MyOnDestroy();
        }
    }

    public static T GetLogicComponent<T>() where T : class
    {
        if (inst.logicHasMap.TryGetValue(typeof(T), out var logic))
        {
            return logic as T;
        }

        foreach (var item in inst.logicHasMap.Values)
        {
            if (item is T)
            {
                return item as T;
            }
        }

        return null;
    }
}