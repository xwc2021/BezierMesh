using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class WaitForEditorLoad
{
    static WaitForEditorLoad()
    {
        UnityEditor.SceneManagement.EditorSceneManager.sceneOpened +=
             SceneOpenedCallback;

        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    static void SceneOpenedCallback(Scene _scene,
        UnityEditor.SceneManagement.OpenSceneMode _mode)
    {

    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
    }
}
