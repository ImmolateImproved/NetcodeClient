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

    public static T GetLogicComponent<T>() where T : Logic
    {
        return (T)(inst.logicHasMap[typeof(T)]);
    }
}