using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerInfoModel {

    public int level = 1;

    public int score = 0;

    public int life = 3;

    public int HP = 3;

    public void ResetHP()
    {
        HP = 3;
    }

}
