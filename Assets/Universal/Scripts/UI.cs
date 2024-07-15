using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;
    public GameObject player_HP_Bar;
    public TextMeshProUGUI player_HP_Text;
    void Start()
    {
        updatePlayerHP_Bar_UI();
    }
    public void updatePlayerHP_Bar_UI()
    {
        Vector3 originalScale = player_HP_Bar.transform.localScale;
        player_HP_Bar.transform.localScale = new Vector3(player.current_HP / player.max_HP, originalScale.y, originalScale.z);
        player_HP_Text.text = $"{player.current_HP} / {player.max_HP}";
    } 
}
