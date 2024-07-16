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
    public Image fireball_icon_transition;
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
    public void skillIconTransition(Image icon, float skill_coolDown_left, float skill_coolDown)
    {
        icon.fillAmount = skill_coolDown_left / skill_coolDown;
    }
}
