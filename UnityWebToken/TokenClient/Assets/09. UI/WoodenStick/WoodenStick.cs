using System;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class WoodenStick
{
    private int level = 1;
    public int Level {
        get => level;
        set {
            level = value;

            cost = (long)Mathf.Pow(2, level);

            if(titleLabel != null)
                titleLabel.text = $"{level}강화 나무 몽둥이";
            if (levelLabel != null)
                levelLabel.text = $"Lv {level}";
            if(costLabel != null)
                costLabel.text = $"가격 : {cost}\n확률 : {(decimal)level / (decimal)cost * (decimal)100}";
            if(nameLabel != null)
            {
                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < level; i++)
                    builder.Append("겁나 ");
                nameLabel.text = $"{builder.ToString()}\n강한 몽둥이";
            }
        }
    }

    private long money;
    public long Money {
        get => money;
        set {
            money = value;
            if(moneyLabel != null)
                moneyLabel.text = $"돈 : {money}";
        }
    }

    private bool shield;
    public bool Shield {
        get => shield;
        set {
            shield = value;

            shieldButton.text = shield ? "업그레이드 딱대" : "파괴방지권 구매";
        }
    }

    private Label titleLabel;
    private Label levelLabel;
    private Label nameLabel;
    private Label costLabel;
    private Label moneyLabel;

    private Button shieldButton;
    
    private long cost;

    public WoodenStick(VisualElement root)
    {
        titleLabel = root.Q<Label>("TitleLabel");
        levelLabel = root.Q<Label>("LevelLabel");
        nameLabel = root.Q<Label>("NameLabel");
        costLabel = root.Q<Label>("CostLabel");
        moneyLabel = root.Q<Label>("MoneyLabel");

        #region Register
        shieldButton = root.Q<Button>("BuyButton");
        shieldButton.RegisterCallback<ClickEvent>(BuyButtonClickHandle);
        root.Q<Button>("SoldButton").RegisterCallback<ClickEvent>(SoldButtonClickHandle);
        root.Q<Button>("UpgradeButton").RegisterCallback<ClickEvent>(UpgradeButtonClickHandle);
        #endregion

        Level = 1;
        Money = 0;
    }

    private Random random = new Random();
    private void UpgradeButtonClickHandle(ClickEvent evt)
    {
        if (GameManager.Instance.IsLoggedIn == false)
            return;

        long rand = LongRandom(0, cost, random);
        if(Level > rand)
        {
            Level++;
            Debug.Log("성공");
        }
        else
        {
            if(Shield == false)
                Level = 1;

            Debug.Log("실패");
        }

        Shield = false;
    }

    private void SoldButtonClickHandle(ClickEvent evt)
    {
        if(GameManager.Instance.IsLoggedIn == false)
            return;

        Money += cost;
        Level = 1;
    }

    private void BuyButtonClickHandle(ClickEvent evt)
    {
        if (GameManager.Instance.IsLoggedIn == false)
            return;

        if(Shield == false && Money >= cost)
        {
            Shield = true;
            Money -= cost;
        }
    }

    long LongRandom(long min, long max, Random rand)
    {
        byte[] buf = new byte[8];
        rand.NextBytes(buf);
        long longRand = BitConverter.ToInt64(buf, 0);

        return (Math.Abs(longRand % (max - min)) + min);
    }
}
