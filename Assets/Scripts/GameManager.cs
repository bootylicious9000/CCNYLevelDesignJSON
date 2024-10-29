using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ActorController MainNPC;
    public static GameManager Singleton;
    public TextMeshPro HealthDisplay;
    public TextMeshPro DialogueDisplay;
    public SpriteRenderer Fader;
    public TextAsset JSON;
    public AudioSource AS;
    public LevelJSON Script;
    public List<ActorController> Actors;
    public Dictionary<string, List<ActorController>> ActorDict = new Dictionary<string, List<ActorController>>();
    public float Clock;
    public List<EventJSON> Queue = new List<EventJSON>();
    // private float DialogueCountdown = 0;
    private bool RoundBegun = false;

    private void Awake()
    {
        GameManager.Singleton = this;
    }

    void Start()
    {
        AS.Stop();
        Script = JSONReader.ParseJSON(JSON.text);
        foreach(EventJSON e in Script.Events)
            Queue.Add(e);
        StartCoroutine(GameMaster.Fade(Fader,false,0.5f));
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
        // if (DialogueCountdown > 0)
        // {
        //     DialogueCountdown -= Time.deltaTime;
        //     if (DialogueCountdown <= 0)
        //         DialogueDisplay.text = "";
        // }
        Clock += Time.deltaTime;
        while (Queue.Count > 0 && Queue[0].Time <= Clock)
        {
            EventJSON e = Queue[0];
            Queue.RemoveAt(0);
            ResolveEvent(e);
        }

        if (Queue.Count == 0)
        {
            StartCoroutine(LevelComplete());
        }

        
        if (PlayerController.Player != null)
        {
            HealthDisplay.text = "Health: " + Mathf.Ceil(PlayerController.Player.Health);
        }
        else
        {
            HealthDisplay.text = "GAME OVER";
        }
    }

    public void AddActor(ActorController a)
    {
        Actors.Add(a);
        if(!ActorDict.ContainsKey(a.gameObject.name))
            ActorDict.Add(a.gameObject.name,new List<ActorController>());
        ActorDict[a.gameObject.name].Add(a);
    }

    public void RemoveActor(ActorController a)
    {
        Actors.Remove(a);
        ActorDict.Remove(a.gameObject.name);
    }

    public void ResolveEvent(EventJSON e)
    {
        List<ActorController> who = new List<ActorController>();
        if (!string.IsNullOrEmpty(e.Who)) who.AddRange(ActorDict[e.Who]);
        else who.Add(MainNPC);
        foreach(ActorController w in who)
            w.TakeEvent(e);
        if (e.Dialogue != null)
        {
           HandleDialogue(e.Dialogue); 
        }
    }

    public void HandleDialogue(string d, float duration=0)
    {
        DialogueDisplay.text = d;
        // DialogueCountdown = duration > 0 ? duration : 999;
    }

    public IEnumerator LevelComplete()
    {
        yield return StartCoroutine(GameMaster.Fade(Fader));
        yield return new WaitForSeconds(0.5f);
        GameMaster.NextStage();
    }
}
