using Lamb.UI.SettingsMenu;
using COTL_API.Saves;
using src.Extensions;
using UnityEngine;
using Lamb.UI;

namespace COTL_API.UI.Settings;

public class SkinSettings : UISubmenuBase
{

    [SerializeField]
    private MMHorizontalSelector _skinName;

    public void Init()
    {
        name = "SkinSettings";
        
        _animator = gameObject.AddComponent<Animator>();
        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        _canvas = gameObject.AddComponent<Canvas>();
        
        // _skinName = gameObject.AddComponent<MMHorizontalSelector>();
        // _skinName._animator = gameObject.AddComponent<Animator>();
        // _skinName._canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    
    public override void Awake()
    {
        base.Awake();
        
        // _skinName.name = "Selected Skin";
        // _skinName._text.text = "Selected Skin";
    }

    private void Start()
    {
        // _skinName.PrefillContent("Default");
        // _skinName.OnSelectionChanged += OnSkinValueChanged;
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