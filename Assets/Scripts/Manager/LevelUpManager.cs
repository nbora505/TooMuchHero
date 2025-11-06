using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    public void AddExp(TokenController TC)
    {
        Slider slider_exp = TC.slider_exp;
        int lv = TC.curLv;
        int exp = TC.curExp;
        exp++;

        if (lv == 1)
        {
            slider_exp.value += 0.5f;
            if (exp >= 2)
            {
                lv++;
                exp = 0;
                slider_exp.value = 0f;
                TC.text_decription.text = TC.data.skillDescription[1];
            }
            AddAtkOrHp(TC, true);
            AddAtkOrHp(TC, false);
        }
        else if (lv == 2) 
        {
            slider_exp.value += 0.34f;
            if (exp >= 3)
            {
                lv++;
                exp = 0;
                slider_exp.value = 1f;
                TC.text_decription.text = TC.data.skillDescription[2];
            }
            AddAtkOrHp(TC, true);
            AddAtkOrHp(TC, false);
        }
        else exp--;

        TC.curLv = lv;
        TC.curExp = exp;
        ResetTokenUI(TC);
    }

    public void AddAtkOrHp(TokenController TC, bool isAtk)
    {
        if (isAtk)
        {
            TC.curAtk += 1;
        }
        else
        {
            TC.curHp += 1;
        }
        ResetTokenUI(TC);
    }

    public void ResetTokenUI(TokenController TC)
    {
        TC.text_lv.text = TC.curLv.ToString();
        TC.text_atk.text = TC.curAtk.ToString();
        TC.text_hp.text = TC.curHp.ToString();
    }
}
