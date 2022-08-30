using src.UINavigator;
using src.Extensions;
using UnityEngine;
using Lamb.UI;
using Rewired;
using System;
using COTL_API.Saves;

namespace COTL_API.UI.Settings;


public class SkinSettings : UISubmenuBase
{

    [SerializeField]
    private MMHorizontalSelector _skinName;

    public override void Awake()
    {
        base.Awake();
        name = "Skins";

        _skinName = new();
        _skinName.name = "Selected Skin";

        _skinName.PrefillContent("Default");
        //_skinName._text.text = "Selected Skin";
    }

    private void Start()
    {
        _skinName.OnSelectionChanged += OnSkinValueChanged;
    }

    public void Reset()
    {
        APIDataManager.APIData.SetValue<string>("selectedSkin", null);
    }

    public void Configure()
    {
        //_skinName.ContentIndex = _skinName.Content.IndexOf(APIDataManager.apiData.GetValueAsString("selectedSkin"));
    }

    private void OnSkinValueChanged(int index)
    {
        string skinName = _skinName.Content[index];
        Plugin.Logger.LogInfo($"Selected Skin: {skinName}");
    }
}