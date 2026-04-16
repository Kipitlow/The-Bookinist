using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{

    public void SuccessQuest(Button quest)
    {
        quest.interactable = true;
    }

    public void changeColor(Image image)
    {
        image.color = new Color32(231,154,62,255);
    }
}
