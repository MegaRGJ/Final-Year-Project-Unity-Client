﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonGUI : MonoBehaviour
{
    float DELTATIME = 0.0f;
    AIContoller PLAYER_AICONTOLLER;
    MultiplayerManager MULTIPLAYER_MANAGER;
    void Start()
    {
        GameObject Player = GameObject.Find("Player");
        PLAYER_AICONTOLLER  = Player.GetComponent<AIContoller>();
        GameObject MultiplayerScript = GameObject.Find("MultiplayerScript");
        MULTIPLAYER_MANAGER = MultiplayerScript.GetComponent<MultiplayerManager>();
    }

    void Update()
    {
        DELTATIME += (Time.unscaledDeltaTime - DELTATIME) * 0.1f;
    }

    void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;


        ConnectButton(width, height);
        FPS(width, height);
        AIEnabled(width, height);
    }

    void FPS(int width, int height)
    {
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 3 / 100;
        style.normal.textColor = Color.green;

        float msec = DELTATIME * 1000.0f;
        float fps = 1.0f / DELTATIME;

        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }

    void AIEnabled(int width, int height)
    {
        GUIStyle style = new GUIStyle();

        int labelHeight = height * 2 / 100;
        Rect rect = new Rect(0, labelHeight * 2, width, labelHeight);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 3 / 100;
        style.normal.textColor = Color.green;
        
        string text = string.Format("AI Enabled: " + PLAYER_AICONTOLLER.IS_AI_ENABLED.ToString());
        GUI.Label(rect, text, style);
    } 

    void ConnectButton(int width, int height)
    {
        int buttonHeight = height * 4 / 100;
        int buttonWidth = width * 8 / 100;
        Rect rect = new Rect(0, buttonHeight * 2, buttonWidth, buttonHeight);

        if (GUI.Button(rect, "Connect"))
        {
            MULTIPLAYER_MANAGER.SendConnectRequest();
        }
    }
}
