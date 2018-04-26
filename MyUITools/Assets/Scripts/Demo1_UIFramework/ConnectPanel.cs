using UnityEngine;

public class ConnectPanel : BasePanel {

    protected override void Awake() {
        base.Awake();
    }

    public void ResetPasswordButtonOnClick() {
        UIManager.Instance.PushPanel(UIPanelType.ALERT_PANEL, true);
        AlertPanel.Instance.ShowAlert(SwitchToHomwPanel, "修改密码成功! 是否开始办理业务?");
    }

    public void SwitchToHomwPanel() {
        UIManager.Instance.PushPanel(UIPanelType.HOME_PANEL, false);
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
