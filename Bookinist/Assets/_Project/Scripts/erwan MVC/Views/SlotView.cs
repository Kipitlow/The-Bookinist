using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotView : MonoBehaviour
{
    public SlotModel SlotModel { get; private set; }

    public void Init(SlotModel slotModel)
    {
        SlotModel = slotModel;
        slotModel.SlotView = this;
    }
}
