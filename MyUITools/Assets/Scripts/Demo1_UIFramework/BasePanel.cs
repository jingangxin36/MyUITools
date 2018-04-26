using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class BasePanel : MonoBehaviour {

    protected CanvasGroup canvasGroup;
    protected virtual void Awake() {
        name = GetType() + ">>";
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    protected new string name;

    /// <summary>
    /// 开启交互,页面显示
    /// </summary>
    public virtual void OnEnter() {
        //Debug.Log(name + "Enter");

        SetPanelActive(true);
        SetPanelInteractable(true);

    }

    /// <summary>
    /// 界面暂停,关闭交互
    /// </summary>
    public virtual void OnPause() {
        SetPanelInteractable(false);
    }

    /// <summary>
    /// 界面继续,恢复交互
    /// </summary>
    public virtual void OnResume() {
        SetPanelInteractable(true);
    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关闭
    /// </summary>
    public virtual void OnExit() {
        SetPanelActive(false);
        SetPanelInteractable(false);
    }

    private void SetPanelActive(bool isActive) {
        if (isActive ^ gameObject.activeSelf) gameObject.SetActive(isActive);
    }

    private void SetPanelInteractable(bool isInteractable) {
        if (isInteractable ^ canvasGroup.interactable) canvasGroup.interactable = isInteractable;
    }

}
