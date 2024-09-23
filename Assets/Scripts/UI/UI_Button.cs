using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_PopUp
{
    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScoreText
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        Get<Text>((int)Texts.ScoreText).text = "Bind Text";

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClick);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;

        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

        base.Init();
    }

    public void OnButtonClick(PointerEventData data)
    {
        Debug.Log("클릭 버튼");
    }
}
