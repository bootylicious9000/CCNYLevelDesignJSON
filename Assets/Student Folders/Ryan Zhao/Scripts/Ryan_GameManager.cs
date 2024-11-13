using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This script handles the game's backend
//You really shouldn't mess with it
//Just make sure to set MainNPC and JSON in the editor
public class Ryan_GameManager : MonoBehaviour
{
    [Header("Set To Your Main NPC")]
    public Ryan_ActorController MainNPC;
    [Header("Drag Your JSON File Here")]
    public TextAsset JSON;
    public static Ryan_GameManager Singleton;
    [Header("Ignore These")]
    public TextMeshPro HealthDisplay;
    public TextMeshPro DialogueDisplay;
    public SpriteRenderer Fader;
    public AudioSource AS;
    public Ryan_LevelJSON Script;
    public List<Ryan_ActorController> Actors;
    public Dictionary<string, List<Ryan_ActorController>> ActorDict = new Dictionary<string, List<Ryan_ActorController>>();
    public float Clock;
    public List<Ryan_EventJSON> Queue = new List<Ryan_EventJSON>();
    private bool RoundBegun = false;

    private void Awake()
    {
        Ryan_GameManager.Singleton = this;
    }

    void Start()
    {
        AS.Stop();
        Script = Ryan_JSONReader.ParseJSON(JSON.text);
        foreach(Ryan_EventJSON e in Script.Events)
            Queue.Add(e);
        StartCoroutine(Ryan_GameMaster.Fade(Fader,false,0.5f));
        DialogueDisplay.text = Script.Title + "\n" + Script.Author;
    }

    // Update is called once per frame
    void Update()
    {
        if (!RoundBegun)
        {
            if (Input.anyKey && Time.time > 0.5f)
            {
                RoundBegun = true;
                DialogueDisplay.text = "";
                AS.Play();
            }
            return;
        }
        Clock += Time.deltaTime;
        while (Queue.Count > 0 && Queue[0].Time <= Clock)
        {
            Ryan_EventJSON e = Queue[0];
            Queue.RemoveAt(0);
            ResolveEvent(e);
        }

        if (Queue.Count == 0)
        {
            StartCoroutine(LevelComplete());
        }

        
        if (Ryan_PlayerController.Player != null)
        {
            HealthDisplay.text = "Health: " + Mathf.Ceil(Ryan_PlayerController.Player.Health);
        }
        else
        {
            HealthDisplay.text = "GAME OVER";
        }
    }

    public void AddActor(Ryan_ActorController a)
    {
        Actors.Add(a);
        if(!ActorDict.ContainsKey(a.gameObject.name))
            ActorDict.Add(a.gameObject.name,new List<Ryan_ActorController>());
        ActorDict[a.gameObject.name].Add(a);
    }

    public void RemoveActor(Ryan_ActorController a)
    {
        Actors.Remove(a);
        ActorDict.Remove(a.gameObject.name);
    }

    public void ResolveEvent(Ryan_EventJSON e)
    {
        List<Ryan_ActorController> who = new List<Ryan_ActorController>();
        if (!string.IsNullOrEmpty(e.Who)) who.AddRange(ActorDict[e.Who]);
        else who.Add(MainNPC);
        foreach(Ryan_ActorController w in who)
            w.TakeEvent(e);
        if (e.Dialogue != null)
        {
           HandleDialogue(e.Dialogue); 
        }
    }

    public void HandleDialogue(string d, float duration=0)
    {
        DialogueDisplay.text = d;
    }

    public IEnumerator LevelComplete()
    {
        yield return StartCoroutine(Ryan_GameMaster.Fade(Fader));
        yield return new WaitForSeconds(0.5f);
        Ryan_GameMaster.NextStage();
    }
}
