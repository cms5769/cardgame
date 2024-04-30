using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stela : MonoBehaviour
{
    private int stela=0;

    public void SetStela(int stela)
    {
        this.stela = stela;
    }
    
    public int getStela(bool isStela)
    {
        return stela;
    }
}
