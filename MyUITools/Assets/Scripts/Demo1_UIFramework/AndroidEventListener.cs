using System.Collections;
using UnityEngine;

public class AndroidEventListener : MonoBehaviour {
    public GameObject hintLableGameObject;
    private int mEscapeTimes;

	void Awake () {
        hintLableGameObject = Instantiate(hintLableGameObject);
        hintLableGameObject.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!UIManager.Instance.BackToLastPanel()) {
                mEscapeTimes++;
                ShowHint();
                Debug.Log(mEscapeTimes);
                StartCoroutine(ResetTimes());
                if (mEscapeTimes >= 2) {
                    Application.Quit();
                    Debug.Log("Application.Quit");
                }
            }
        }
    }

    private void ShowHint() {
        if (hintLableGameObject != null) {
            //使得每次提示的lable都显示在最顶层
            hintLableGameObject.transform.SetAsLastSibling();
            hintLableGameObject.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 1秒之后重置计时器
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetTimes() {
        yield return new WaitForSeconds(1);
        mEscapeTimes = 0;
        hintLableGameObject.gameObject.SetActive(false);
    }
}
