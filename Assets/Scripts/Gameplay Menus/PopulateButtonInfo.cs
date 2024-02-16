using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateButtonInfo : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image image;

    public void Populate(string nameStr, int price, Sprite img)
    {
        nameText.text = nameStr;
        priceText.text = "$" + price;
        image.sprite = img;
    }

}
