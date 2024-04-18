using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillBarUI : BaseUI
{
    TextMeshProUGUI goldCount;
    TextMeshProUGUI monsterCount;
    int monsters = 0;

    void Start()
    {
        goldCount = GetUI<TextMeshProUGUI>("GoldCountText");
        monsterCount = GetUI<TextMeshProUGUI>("MonsterCountText");

        Manager.Game.goldCountChanged += UpdateGoldCount;
        Manager.Event.voidEventDic["enemySpawned"].OnEventRaised += AddMonsterCount;
        Manager.Event.voidEventDic["enemyDied"].OnEventRaised += SubtractMonsterCount;
    }

    private void OnDestroy()
    {
        Manager.Game.goldCountChanged -= UpdateGoldCount;
        Manager.Event.voidEventDic["enemySpawned"].OnEventRaised -= AddMonsterCount;
        Manager.Event.voidEventDic["enemyDied"].OnEventRaised -= SubtractMonsterCount;
    }

    private void UpdateGoldCount(int count)
    {
        goldCount.text = count.ToString();
    }

    private void AddMonsterCount()
    {
        monsters++;
        monsterCount.text = monsters.ToString();
    }

    private void SubtractMonsterCount()
    {
        monsters--;
        monsterCount.text = monsters.ToString();
    }
}
