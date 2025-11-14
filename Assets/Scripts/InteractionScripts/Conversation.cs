using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Conversation : MonoBehaviour
{
    public Dialogue[] Dialogues;
    public int currentIndex;
    public Dialogue currentDialogue;
    public TextMeshProUGUI dia;
    public TextMeshProUGUI Name;
    public Image MainPortrait;
    public Image OppoPortrait;
    public GameObject dialogueModal;
    void Start()
    {
        currentIndex = 0;
        MainPortrait.enabled = false;
        OppoPortrait.enabled = false;
        dialogueModal.SetActive(false);
        
    }
    

    public void DoTalk()
    {
        dialogueModal.SetActive(true);
        StartCoroutine(TalkFlow());
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) DoTalk();
    }

    private IEnumerator TalkFlow()
    {
        while (currentIndex < Dialogues.Length)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentDialogue = Dialogues[currentIndex];// TODO 이거 인물 따라서 출력하는 배열 바꿔야됨
                if (currentDialogue.isMainCharacter)
                {
                    OppoPortrait.enabled = false;
                    MainPortrait.enabled = true;
                    Name.text = currentDialogue.Name;
                    MainPortrait.sprite = currentDialogue.MainCharacterPortrait;
                }
                else
                {
                    MainPortrait.enabled = false;
                    OppoPortrait.enabled = true;
                    Name.text = currentDialogue.Name;
                    OppoPortrait.sprite = currentDialogue.OpponentCharacterPortrait;
                }
                dia.text = currentDialogue.dia;
                currentIndex++;
            }
            yield return null;
        }
    }
}
