using UnityEngine;

public class UIRoot : MonoBehaviour {

	void Start () {
        UIManager.Instance.PushPanel(UIPanelType.WELCOME_PANEL);
    }

}
