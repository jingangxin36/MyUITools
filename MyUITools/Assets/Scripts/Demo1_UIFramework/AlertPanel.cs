using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AlertPanel : BasePanel {
    public Text alertText;
    public Button confirmButton;
    public Button cancelButton;

    public static AlertPanel Instance { get; set; }


    protected override void  Awake() {
        //Debug.Log(name + "Awake");
        base.Awake();
        Instance = this;
        cancelButton.onClick.AddListener(ClosePanel);
    }

    //void OnEnable() {
    //    Debug.Log(name + "OnEnable");
    //}

    //void Start() {
    //    Debug.Log(name + "Start");
    //}

    //void OnDisable() {
    //    Debug.Log(name + "OnDisable");
    //}

    public void ClosePanel() {
        UIManager.Instance.BackToLastPanel();
    }

    public void ShowAlert(UnityAction confirmAction, string showText) {
        alertText.text = showText;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(confirmAction);
    }

    public override void OnEnter() {
        Debug.Log(name + "Enter");
        base.OnEnter();
    }

    public override void OnExit() {
        Debug.Log(name + "Exit");
        base.OnExit();
    }
    public override void OnPause() {
        Debug.Log(name + "Pause");
        base.OnPause();
    }

    public override void OnResume() {
        Debug.Log(name + "Resume");
        base.OnResume();
    }




}
