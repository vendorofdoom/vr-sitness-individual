using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialoguePre
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}

[System.Serializable]
public class DialoguePost
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}

[System.Serializable]
public class DialogueExit
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
