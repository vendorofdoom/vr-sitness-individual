using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public void Awake()
    {
        current = this;
    }

    public event Action onSceneBegin;
    public void SceneBegin()
    {
        if (onSceneBegin != null)
        {
            onSceneBegin();
        }
    }

    public event Action onExerciseBegin;
    public void ExerciseBegin()
    {
        if (onExerciseBegin != null)
        {
            onExerciseBegin();
        }
    }

    public event Action onExerciseEnd;
    public void ExerciseEnd()
    {
        if (onExerciseEnd != null)
        {
            onExerciseEnd();
        }
    }
    
    public event Action onExerciseExit;
    public void ExerciseExit()
    {
        if (onExerciseExit != null)
        {
            onExerciseExit();
        }
    }

    public event Action onDebriefEnd;
    public void DebriefEnd()
    {
        if (onDebriefEnd != null)
        {
            onDebriefEnd();
        }
    }
}
