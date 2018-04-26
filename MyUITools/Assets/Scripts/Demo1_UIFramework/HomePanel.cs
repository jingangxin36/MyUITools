using UnityEngine;

public class HomePanel : BasePanel {

    protected override void Awake() {
        base.Awake();
    }

    public void SignOutButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL);
        AlertPanel.Instance.ShowAlert(SwitchToWelcomePanel, "是否注销当前账号?");
    }

    private void SwitchToWelcomePanel() {
        UIManager.Instance.PushPanel(UIPanelType.WELCOME_PANEL, false);
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
