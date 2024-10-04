using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PuzzleManagement;

public class StarDrawLogic : Singleton<StarDrawLogic>
{
    //Responsible for handling all logic related to casting spells
    public static event System.EventHandler<int> OnNodeSelected;

    public GameObject proc;

    [Serializable]
    public struct OnSpellCastArgs
    {
        public SpellType spellType;
        public List<int> pattern;

        public OnSpellCastArgs(SpellType st, List<int> p)
        {
            spellType = st;
            pattern = p;
        }
    }
    //Fired when there is a valid spell cast
    public static event System.EventHandler<OnSpellCastArgs> OnSpellCast;

    [SerializeField] private int startNode;
    [SerializeField] private int endNode;

    [Serializable]
    public struct SpellData
    {
        public SpellType spellType;
        public string pattern; //C: only a string here because it makes the editor cleaner
        public bool unlocked;
    }

    [SerializeField] private List<SpellData> spellData;


    private List<int> pattern = new List<int>();
    private bool activePattern = false;
    private bool activeLineAnimation = false;


    private void Awake() {
        InitializeSingleton();
    }

    private void OnEnable() {
        NodeInput.OnInputPress += OnInputPress;
    }

    private void OnDisable() {
        NodeInput.OnInputPress -= OnInputPress;
    }

    private void OnInputPress(object sender, int inputNum) {
        if(!activePattern && inputNum == startNode && !activeLineAnimation) {
            StartPattern();
        }
        else if(activePattern && inputNum == endNode) {
            EndPattern();
        }
        // else if(activePattern && !pattern.Contains(inputNum)) {
        else if(activePattern) {
            AddToPattern(inputNum);
        }
        else {
            //invalid input: active node, non-start node on inactive pattern, etc
            //handle at will
            //print("Invalid node Clicked");
        }
    }

    private void StartPattern() {
        activePattern = true;
        pattern = new List<int>();
        AddToPattern(startNode);
        //sfx, vfx, etc
        //AudioControl.Instance.PlaySFX("Node1", PlayerController.Instance.gameObject, 0f, 0.33f);
    }

    private void AddToPattern(int num) {
        pattern.Add(num);
        OnNodeSelected.Invoke(this, num);
        string nodeName = "Node" + (num.ToString());
        if (num != 9) {
            //AudioControl.Instance.PlaySFX(nodeName, PlayerController.Instance.gameObject, 0f, 0.33f);
        }
        //PrintPattern();
    }

    private void EndPattern() {
        AddToPattern(endNode);
        activePattern = false;
        CompareSpellCast();
        ResetPattern();
        //sfx, vfx, etc
    }

    private void ResetPattern() {
        pattern = new List<int>();
    }

    private void CompareSpellCast()
    {
        string patternString = GetPatternString();
        foreach (SpellData sd in spellData) {
            if(sd.unlocked && patternString.Equals(sd.pattern)) 
            {
                //OnSpellCast?.Invoke(this, new OnSpellCastArgs(sd.spellType, pattern));
                StartCoroutine(Completed());
                BlockClicks();
                return;
            }
        }
        OnInvalidPattern();
    }

    IEnumerator Completed()
    {
        AudioManager.Instance.FadeMusic(true, true);
        NotificationManager.Instance.TestPuzzleCompleteNotification();
        GameManager.Instance.puzzleComplete = true;
        yield return new WaitForSeconds(4f);
        proc.GetComponent<PuzzleProc>().PuzzleInit();
    }

    private void OnInvalidPattern()
    {
        print("Invalid Pattern");
        //AudioControl.Instance.PlaySFX("NodeWrong", PlayerController.Instance.gameObject, 0f, 0.25f);
        OnSpellCast?.Invoke(this, new OnSpellCastArgs(SpellType.NONE, pattern));
        BlockClicks();
    }

    public void UnlockSpell(SpellType spellType)
    {
        for(int i = 0; i < spellData.Count; i++) {
            SpellData sd = spellData[i];
            if(spellType == sd.spellType) 
            {
                sd.unlocked = true;
                return;
            }
        }
    }

    private void BlockClicks()
    {
        StartCoroutine(Blocker());
    }

    private IEnumerator Blocker()
    {
        activeLineAnimation = true;
        yield return new WaitForSeconds(1);
        activeLineAnimation = false;
    }

    private void PrintPattern() {
        string patternString = "";
        foreach (int num in pattern)
            patternString += num + ", ";
        print(patternString);
    }

    private string GetPatternString()
    {
        string result = "";
        foreach(int num in pattern)
            result += num.ToString().ToCharArray()[0]; //C: C# really hates type conversion lol
        return result;
    }
}

public enum SpellType {
    FIREBALL,
    CHAIR,
    ICEBALL,
    WINDBLAST,
    PIPLUP,
    NONE
}
