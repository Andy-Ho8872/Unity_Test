using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Player player;
    public GameObject player_HP_Bar;
    public GameObject hitNumberPrefab;
    [Header("Only can be used in UI_canvas")]
    public TextMeshProUGUI player_HP_Text;
    [Header("Skill Data")]
    public Image[] skill_icon_transitions;
    public TextMeshProUGUI[] skill_coolDown_timers; 
    void Start()
    {
        updatePlayerHP_Bar_UI();
    }
    // The function will update the player health bar
    public void updatePlayerHP_Bar_UI()
    {
        Vector3 originalScale = player_HP_Bar.transform.localScale;
        player_HP_Bar.transform.localScale = new Vector3(player.current_HP / player.max_HP, originalScale.y, originalScale.z);
        player_HP_Text.text = $"{player.current_HP} / {player.max_HP}";
    }
    // The function will change the icon's appearance when it is on coolDown
    public void skillIconTransition(int skill_index, float skill_coolDown_left, float skill_coolDown)
    {
        Image current_skill = skill_icon_transitions[skill_index];
        float roundedValue = Mathf.Round(skill_coolDown_left);
        // skill background shader
        current_skill.fillAmount = skill_coolDown_left / skill_coolDown;
        // show the coolDown left of the skill
        if (current_skill.fillAmount != 0) skill_coolDown_timers[skill_index].text = $"{roundedValue}s";
        // if the coolDown is over, clear the text
        else skill_coolDown_timers[skill_index].text = "";
    }
    // The function will generate the damage number and it will be shown above the position
    public void generateHitNumber(int number, Vector3 spawnPosition)
    {
        GameObject hitNumber = Instantiate(hitNumberPrefab, spawnPosition, Quaternion.identity);
        hitNumber.GetComponent<TextMeshPro>().text = $"{number}";
        Destroy(hitNumber, 0.5f); 
    }
}
