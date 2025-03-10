using System;
using UnityEngine;
using UnityEngine.UI;


public class PlayerToolSelector : MonoBehaviour
{
    public enum Tool { None, Sow, Hoe, Axe, Hammer, Pickaxe }
    public Tool activeTool;

    [Header("Elements")]
    [SerializeField] private Image[] toolImages;
    [SerializeField] private Button NoneBtn;

    [Header("Settings")]
    [SerializeField] private Color selectedToolColor;
    public bool NoneToolActive;

    [Header("Actions")]
    public Action<Tool> onToolSelected;
    public Action ActiveFarmTool;

    void Start()
    {
        NoneBtn.onClick.RemoveListener(OnNoneButtonPressed); // Xóa listener cũ (nếu có)
        NoneBtn.onClick.AddListener(OnNoneButtonPressed);
        NoneToolActive = true;
    }


    // Hàm được gọi khi nút "None" được bấm


    public void OnNoneButtonPressed()
    {

        if (activeTool == Tool.None && NoneToolActive == true)
        {
            NoneToolActive = false;
        }
        else if (activeTool == Tool.None && NoneToolActive == false)
        {
            NoneToolActive = true;
        }

        ActiveFarmTool?.Invoke();
    }

    public void SelectTool(int toolIndex)
    {
        activeTool = (Tool)toolIndex;  // Lấy vị trí int theo vị trí index của Tool
        for (int i = 0; i < toolImages.Length; i++)
            toolImages[i].color = i == toolIndex ? selectedToolColor : Color.white;

        // Gọi sự kiện khi công cụ được chọn
        onToolSelected?.Invoke(activeTool);
    }

    public bool CanSow()
    {
        return activeTool == Tool.Sow;
    }


    public bool CanHoe()
    {
        return activeTool == Tool.Hoe;
    }

    public bool CanChop()
    {
        return activeTool == Tool.Axe;
    }

    public bool CanBuild()
    {
        return activeTool == Tool.Hammer;
    }


    public bool CanMine()
    {
        return activeTool == Tool.Pickaxe;
    }
}
