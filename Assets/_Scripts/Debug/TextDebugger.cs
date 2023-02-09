using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDebugger : MonoBehaviour
{
    public static TextDebugger Instance;
    [SerializeField] TextMeshProUGUI debugTextMesh;
    [SerializeField] private float textMaxTime = 5.0f;
    private List<TextDebugStruct> textOnScreen = new List<TextDebugStruct>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    public int AddScreenDebug(string InDebugString, float InMaxTimeAlive, bool InStaysForever = false)
    {
        TextDebugStruct debug = new TextDebugStruct
        {
            text = InDebugString,
            timeAdded = Time.time,
            bStayForever = InStaysForever,
            maxTimeAlive = (InMaxTimeAlive > 0) ? InMaxTimeAlive : textMaxTime
        };
        
        debug.ID = debug.GetHashCode();
        textOnScreen.Add(debug);
        
        return debug.ID;
    }

    /// <summary>
    /// This shouldn't be used for timed debugg's, only for static ones
    /// </summary>
    public void RemoveScreenDebug(int InDebugHash)
    {
        for (int i = 0; i < textOnScreen.Count; i++)
        {
            if (textOnScreen[i].ID == InDebugHash)
            {
                textOnScreen.RemoveAt(i);
                return;
            }
        }
    }

    public void UpdateDebugText(int InDebugHash, string InText)
    {
        for (int i = 0; i < textOnScreen.Count; i++)
        {
            if (textOnScreen[i].ID == InDebugHash)
            {
                textOnScreen[i].text = InText;
            }
        }
    }

    private void Update()
    {
        string textToShow = "";
        for (int i = 0; i < textOnScreen.Count; i++)
        {
            if (!textOnScreen[i].bStayForever)
            {
                if (textOnScreen[i].timeAdded + textOnScreen[i].maxTimeAlive < Time.time)
                {
                    textOnScreen.RemoveAt(i);
                    continue;
                }
            }

            textToShow += textOnScreen[i].text +"\n";
        }
        debugTextMesh.text = textToShow;
    }
}

public class TextDebugStruct
{
    public int ID;
    public string text;
    public float timeAdded;
    public bool bStayForever;
    public float maxTimeAlive;
}
