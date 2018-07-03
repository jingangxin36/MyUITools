using System;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour {

    private static UIManager sInstanceUiManager;
    private Dictionary<UIPanelType, string> mPanelPathDictionary;//存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> mPanelPool;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> mPanelStack;


    private Transform mUIRootTransform;

    public static UIManager Instance
    {
        get { return GetInstance(); }
    }

    [Serializable]
    public class UIPanelTypeJson {
        public List<UIPanelInformation> infoList;
    }
    /// <summary>
    /// 实例化UIManager
    /// </summary>
    /// <returns></returns>
    private static UIManager GetInstance() {
        if (sInstanceUiManager == null) {
            var go = new GameObject("UIManager");
            sInstanceUiManager = go.AddComponent<UIManager>();
            //如果有场景切换
            //DontDestroyOnLoad(go);
        }
        return sInstanceUiManager;
    }


    void Awake() {
        ParseUIPanelTypeJson();
        mUIRootTransform = GameObject.Find("Canvas").transform;
    }

    /// <summary>
    /// 从json配置文件解析为相对应的object类
    /// </summary>
    private void ParseUIPanelTypeJson() {
        mPanelPathDictionary = new Dictionary<UIPanelType, string>();
        TextAsset textAsset = Resources.Load<TextAsset>("Demo1_UIFramework/UIPanelType");
        //将json对象转化为UIPanelTypeJson类
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(textAsset.text);
        foreach (UIPanelInformation info in jsonObject.infoList) {
            mPanelPathDictionary.Add(info.panelType, info.path);
        }
    }

    /// <summary>
    /// 获得一个指定页面
    /// </summary>
    /// <param name="panelType">指定页面类型</param>
    /// <returns>返回该页面的BasePanel</returns>
    private BasePanel GetPanel(UIPanelType panelType) {
        if (mPanelPool == null) {
            mPanelPool = new Dictionary<UIPanelType, BasePanel>();
        }
        BasePanel panel;
        //从页面池中尝试找到指定页面的示例
        mPanelPool.TryGetValue(panelType, out panel);
        if (panel == null) {
            //如果找不到, 就从配置字典中获得该页面的路径, 去实例化
            mPanelPool.Remove(panelType);
            string path;
            mPanelPathDictionary.TryGetValue(panelType, out path);
            GameObject instancePanel = Instantiate(Resources.Load("Demo1_UIFramework/" + path)) as GameObject;
            if (instancePanel != null) {
                instancePanel.transform.SetParent(mUIRootTransform, false);
                var targetPanel = instancePanel.GetComponent<BasePanel>();
                mPanelPool.Add(panelType, targetPanel);
                return targetPanel;
            }
        }
        return panel;
    }

    /// <summary>
    /// 显示指定的面板
    /// </summary>
    /// <param name="panelType"></param>
    public void PushPanel(UIPanelType panelType) {
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();
        //判断一下栈里面是否有页面
        if (mPanelStack.Count > 0) {
            var topPanel = mPanelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panel.transform.SetAsLastSibling();
        mPanelStack.Push(panel);        
    }
    /// <summary>
    /// 关闭页面并显示新的页面
    /// </summary>
    /// <param name="panelType"></param>
    /// <param name="isPopCurrentPanel">true时, 关闭当前页面; false时, 关闭所有页面</param>
    public void PushPanel(UIPanelType panelType, bool isPopCurrentPanel) {
        if (isPopCurrentPanel) {
            PopCurrentPanel();
        }
        else {
            PopAllPanel();
        }
        PushPanel(panelType);
    }
    /// <summary>
    /// 返回上一个页面
    /// </summary>
    /// <returns></returns>
    public bool BackToLastPanel() {
        //判断当前栈是否为空??表示是否可以返回
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();
        if (mPanelStack.Count <= 1) return false;
        //关闭栈顶页面的显示
        var topPanel1 = mPanelStack.Pop();
        topPanel1.OnExit();
        //恢复此时栈顶页面的交互
        BasePanel topPanel2 = mPanelStack.Peek();
        topPanel2.OnResume();
        return true;
    }

    /// <summary>
    /// 隐藏当前面板
    /// </summary>
    private void PopCurrentPanel() {
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();
        if (mPanelStack.Count <= 0) return;
        //关闭栈顶页面的显示
        BasePanel topPanel = mPanelStack.Pop();
        topPanel.OnExit();
    }

    /// <summary>
    /// 隐藏所有面板
    /// </summary>
    private void PopAllPanel() {
        if (mPanelStack == null)
            mPanelStack = new Stack<BasePanel>();
        if (mPanelStack.Count <= 0) return;
        //关闭栈里面所有页面的显示
        while (mPanelStack.Count > 0) {
            BasePanel topPanel = mPanelStack.Pop();
            topPanel.OnExit();
        }
    }
    /// <summary>
    /// 切换场景前,调用该方法来清空当前场景的数据
    /// </summary>
    public void RefreshDataOnSwitchScene() {
        mPanelPathDictionary.Clear();
        mPanelStack.Clear();
    }
}
