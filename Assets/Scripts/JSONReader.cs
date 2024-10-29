using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JSONReader
{
    public static LevelJSON ParseJSON(string txt)
    {
        return JsonUtility.FromJson<LevelJSON>(txt);
    }
}

[System.Serializable]
public class LevelJSON
{
    public string Title;
    public string Author;
    public List<EventJSON> Events;
}

[System.Serializable]
public class EventJSON
{
    public float Time;
    public string Anim;
    public string Action;
    public float Amt;
    public string Dialogue;
    public string Who;
}
