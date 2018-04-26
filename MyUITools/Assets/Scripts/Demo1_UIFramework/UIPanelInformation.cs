using System;
using UnityEngine;

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
