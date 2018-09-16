## Unity实现基于UGUI的简易UI框架

---
图片丢失 
请转到https://blog.csdn.net/jingangxin666/article/details/80092801 查看

---

### 什么是UI框架？

UI框架中的 `UIManager` 管理场景中所有的面板, 控制面板之间的跳转. 

### 本Demo实现以下功能:
![这里写图片描述](https://img-blog.csdn.net/20180426133844375?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

![这里写图片描述](https://img-blog.csdn.net/20180426133849758?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

![这里写图片描述](https://img-blog.csdn.net/2018042613385788?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

![这里写图片描述](https://img-blog.csdn.net/20180426133903765?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

- 关闭当前页面
- 显示新的页面
  - 叠加显示
  - 关闭当前页面并显示
  - 关闭所有页面并显示
- 安卓返回键响应
    - 返回上一个页面
    - 退出程序

### 设计UI页面

先搭建好所有的UI界面, 并保存为prefab

![这里写图片描述](https://img-blog.csdn.net/20180426133925655?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

### 通过Json和枚举保存所有面板的信息

- [json](https://zh.wikipedia.org/wiki/JSON): json用于描述资料结构，有两种结构存在：

  - 对象 (object)：一个对象包含一系列非排序的名称／值对(pair)，一个对象以`{`开始，并以`}`结束。每个名称／值对之间使用`:`分区。
  - 数组 (array)：一个数组是一个值(value)的集合，一个数组以`[`开始，并以`]`结束。数组成员之间使用`,`分区。数组成员具体的格式如下：
    - 名称／值（pair）：名称和值之间使用`:`隔开，一般的形式是：`{name:value}`

- `UIPanelType.json`:  保存这个工程所有的UI面板类型及其相应的prefab的路径, 此时主要该文件和prefab文件都需要位于`Resources`文件夹路径下

  ```
  {
  	"infoList":
  		[
              {"panelTypeString":"ALERT_PANEL",
              "path":"AlertPanel"},

              {"panelTypeString":"CONNECT_PANEL",
              "path":"ConnectPanel"},

              {"panelTypeString":"HOME_PANEL",
              "path":"HomePanel"},

              {"panelTypeString":"WELCOME_PANEL",
              "path":"WelcomePanel"}	
  		]
  }
  ```

  ​

- `UIPanelType.cs`: 保存这个工程所有的UI面板类型

  ```
  public enum UIPanelType {
      ALERT_PANEL,
      CONNECT_PANEL,
      HOME_PANEL,
      WELCOME_PANEL
  }
  ```

  ​


### 开发UIManager解析面板信息Json



- `JsonUtility`的使用:

| [FromJson](https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html) | Create an object from its JSON representation.               |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [FromJsonOverwrite](https://docs.unity3d.com/ScriptReference/JsonUtility.FromJsonOverwrite.html) | Overwrite data in an object by reading from its JSON representation. |
| [ToJson](https://docs.unity3d.com/ScriptReference/JsonUtility.ToJson.html) | Generate a JSON representation of the public fields of an object. |

注意json中的key值和所对应的unity类中的字段名要保持一致

```

[Serializable]
public class UIPanelInformation : ISerializationCallbackReceiver {
    [NonSerialized]
    public UIPanelType panelType;
    public string panelTypeString;
    public string path;

    public void OnAfterDeserialize() {
        //反序列化之后, 将一个或多个枚举字符串表示(panelTypeString)转换成等效的枚举对象(UIPanelType)。
        UIPanelType type = (UIPanelType)Enum.Parse(typeof(UIPanelType), panelTypeString);
        panelType = type;
    }
    public void OnBeforeSerialize() {
        //啥都不用做
    }
}

[Serializable]
public class UIPanelTypeJson {
	//用来储存json对象所对应的类的列表
	public List<UIPanelInformation> infoList;
}


```



在`UIManager.cs`中 :

```
    private void ParseUIPanelTypeJson() {
        mPanelPathDictionary = new Dictionary<UIPanelType, string>();
        TextAsset textAsset = Resources.Load<TextAsset>("Demo1/UIPanelType");
        //将json对象转化为UIPanelTypeJson类
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(textAsset.text);
        foreach (UIPanelInformation info in jsonObject.infoList) {
            mPanelPathDictionary.Add(info.panelType, info.path);
        }
    }
```



### 开发BasePanel面板基类

![这里写图片描述](https://img-blog.csdn.net/20180426133949185?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)


![这里写图片描述](https://img-blog.csdn.net/2018042613404166?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)



**为什么使用`virtual`关键字而不是`abstract`关键字来修饰?**

- 使得子类可以选择是否需要重写方法
- //todo

### 控制UI面板Prefab的实例化创建和管理

- `mPanelPathDictionary`: 路径字典, 解析保存所有的面板信息(枚举类型和prefab路径)
- `mPanelPool`: 实例对象池, 保存所有已实例化的面板
- `mPanelStack`: 页面栈, 管理各个页面切换任务

### 控制面板之间的跳转

**主要方法:** 

- `PushPanel(UIPanelType panelType)` : 新增一个页面, 此时当前页面需要被暂停
- `PushPanel(UIPanelType panelType, bool isPopCurrentPanel)` : 新增一个页面, 此时当前页面或所有页面需要被关闭
- `BackToLastPanel` : 如果`mPanelStack.Count`为1, 则提示退出程序; 如果不为1, 则弹出当前页面, 并显示此时`mPanelStack`栈顶页面

### 其它问题

#### 如何添加新的页面?

- 制作prefab, 继承BasePanel
- 重写虚函数
- 添加枚举类型
- 添加json信息

#### 如何控制一个面板不可交互 / 忽略点击事件?

- 了解一下: [`CanvasGroup`](https://docs.unity3d.com/Manual/class-CanvasGroup.html) 组件, 属性有:

  | [alpha](https://docs.unity3d.com/ScriptReference/CanvasGroup-alpha.html) | Set the alpha of the group.设置该组的透明度。                |
  | ------------------------------------------------------------ | ------------------------------------------------------------ |
  | [blocksRaycasts](https://docs.unity3d.com/ScriptReference/CanvasGroup-blocksRaycasts.html) | Does this group block raycasting (allow collision).该组是否忽略投射（允许碰撞）。 |
  | [ignoreParentGroups](https://docs.unity3d.com/ScriptReference/CanvasGroup-ignoreParentGroups.html) | Should the group ignore parent groups?是否忽略父物体组？     |
  | [**interactable** ](https://docs.unity3d.com/ScriptReference/CanvasGroup-interactable.html) | Is the group interactable (are the elements beneath the group enabled).是否打开交互(在该组之下启用该元素)。 |


- `CanvasGroup`的典型用途是：

  - 通过在GameObject上添加一个`CanvasGroup`并控制其`Alpha`属性来实现整体淡入或淡出。
  - 通过将父物体的`CanvasGroup`组件中Interactable属性设置为false，从而使子物体一组控件不可交互（“变灰”）。
  - 通过将`CanvasGroup`组件放置在元素或其父项之一上并将其`BlockBlockcast`属性设置为false，使一个或多个UI元素不会阻挡鼠标事件(即射线可穿透它)。

- 参考解决方案: 

  1. 组件要求:

     ![这里写图片描述](https://img-blog.csdn.net/2018042613430894?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
  2. 相关代码为: 

  ```
  [RequireComponent(typeof(CanvasGroup))] 
  public class BasePanel : MonoBehaviour {
      protected CanvasGroup canvasGroup;
      protected virtual void Awake() {	
          canvasGroup = gameObject.GetComponent<CanvasGroup>();
          
          //其它代码
      }
      
      private void SetPanelInteractable(bool isInteractable) {
      	if (isInteractable ^ canvasGroup.interactable) canvasGroup.interactable = isInteractable;
      }
      
      //其它代码
  }

  ```

  ​



#### 在游戏运行中如何使得新创建的面板显示在最上面?如何修改UGUI的显示层级？

- 了解一下: [UGUI物体的渲染顺序](http://www.cnblogs.com/guxin/p/unity-ugui-gameobject-render-order.html)

  - `SetAsFirstSibling`：移动到所有兄弟节点的第一个位置（Hierarchy同级最上面，**先渲染，显示在最下面**）
  - `SetAsLastSibling`：移动到所有兄弟节点的最后一个位置（Hierarchy同级最下面，**后渲染，显示在最上面**）
  - `GetSiblingIndex` ：获得该元素在当前兄弟节点层级的位置
  - `SetSiblingIndex` ：设置该元素在当前兄弟节点层级的位置

- 参考解决方案: 

  ```
          gameObject.transform.SetAsLastSibling();
  ```

- 留意Hierarchy窗口

  ![这里写图片描述](https://img-blog.csdn.net/20180426134334320?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
  

#### 页面重新出现的时候, `Awake`和`Start`函数内的数据刷新不了?

- 了解一下Unity脚本生命周期:

  ![这里写图片描述](https://img-blog.csdn.net/20180426134355878?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

- 测试一下, 在`Awake`, `OnEnable`, `Start`, `OnDisable`, `OnEnter`, `OnExit`函数里面加上`Debug.Log`输出, 结果如图:
  
![这里写图片描述](https://img-blog.csdn.net/20180426134414205?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

  `AlertPanel.cs`的函数调用顺序为:
  
![这里写图片描述](https://img-blog.csdn.net/20180426134427457?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

  可见, `Awake`, `Start`函数只调用一次, 因此, 每次显示和隐藏页面时的数据操作,可以通过`OnEnter`, `OnExit`这两个方法来实现

- 参考解决方案: 

  - 在 `OnEnter`, `OnExit`函数里面进行关于即时数据的操作; 
  - 在`Awake`,  `Start`里面进行各种初始化操作

- 注意:

  - prefab的面板要设置可见, 不然会有奇怪的事情发生!

![这里写图片描述](https://img-blog.csdn.net/20180426134451119?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2ppbmdhbmd4aW42NjY=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)

#### 如果需要切换场景呢?

切换时当前场景内的物体都会被清空, 但我们可以通过`DontDestroyOnLoad`来保留`UIManager`单例, 同时将 `mPanelPool`和`mPanelStack`清空就好了

```
    /// <summary>
    /// 实例化UIManager
    /// </summary>
    /// <returns></returns>
    private static UIManager GetInstance() {
        if (sInstanceUiManager == null) {
            var go = new GameObject("UIManager");
            sInstanceUiManager = go.AddComponent<UIManager>();
            //如果有场景切换
            DontDestroyOnLoad(go);
        }
        return sInstanceUiManager;
    }
    
    /// <summary>
    /// 切换场景前,调用该方法来清空当前场景的数据
    /// </summary>
    public void RefreshDataOnSwitchScene() {
        mPanelPathDictionary.Clear();
        mPanelStack.Clear();
    }
```

如何切换场景?

```
        UIManager.Instance.RefreshDataOnSwitchScene();
        SceneManager.LoadScene("NewScene");

```


### 个人博客

[jingangxin36](https://blog.csdn.net/jingangxin666)

### 个人邮箱

jingangxin36@foxmail.com
