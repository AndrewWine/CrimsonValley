using UnityEditor;
using UnityEngine;

public class NoneTool : MonoBehaviour
{
    [Header("Elements")]
    private PlayerToolSelector playerToolSelector;

    [Header("Settings")]
    [SerializeField] private GameObject SnowTool;
    [SerializeField] private GameObject HoeTool;
    [SerializeField] private GameObject AxeTool;
    [SerializeField] private GameObject HammerTool;
    [SerializeField] private GameObject Pickaxe;
    [SerializeField] private GameObject Shovel;



    void Start()
    {
        playerToolSelector = GetComponent<PlayerToolSelector>();
        playerToolSelector.ActiveFarmTool += NoneToolSelectedCallBack;
    }

    private void OnDestroy()
    {
        playerToolSelector.ActiveFarmTool -= NoneToolSelectedCallBack;
    }



    private void NoneToolSelectedCallBack()
    {

        if (playerToolSelector.NoneToolActive == true)
        {
            SnowTool.SetActive(false);
            HoeTool.SetActive(false);
            AxeTool.SetActive(false);
            HammerTool.SetActive(false);
            Pickaxe.SetActive(false);
            Shovel.SetActive(false);

        }
        else
        {
            SnowTool.SetActive(true);
            HoeTool.SetActive(true);
            AxeTool.SetActive(true);
            HammerTool.SetActive(true);
            Pickaxe.SetActive(true);
            Shovel.SetActive(true);
        }

    }





}
