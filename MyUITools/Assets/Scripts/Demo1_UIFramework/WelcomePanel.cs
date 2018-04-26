using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomePanel : BasePanel {


    protected override void Awake() {
        base.Awake();
    }

    public void ConnectButonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.CONNECT_PANEL);
    }

    public override void OnEnter() {
        base.OnEnter();
        Debug.Log(name + "Enter");
    }

    public override void OnExit() {
        base.OnExit();
        Debug.Log(name + "Exit");
    }
    public override void OnPause() {
        base.OnPause();
        Debug.Log(name + "Pause");
    }

    public override void OnResume() {
        base.OnResume();
        Debug.Log(name + "Resume");
    }
}
