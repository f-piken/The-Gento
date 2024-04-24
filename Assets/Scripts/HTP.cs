using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HTP : MonoBehaviour
{
    public GameObject[] gambar = new GameObject[4];
    public int nilai=0;

    public void mulai1(){
        kanan();
        nilai++;
    }
    public void mulai2(){
        kiri();
        nilai--;
    }
    public void kanan(){
        if(nilai<4)
        {
            for (int i = 0 ; i <= nilai; i++)
            {
                gambar[i].SetActive(false);
                gambar[i+1].SetActive(true);
            }
        }
    }
    public void kiri(){
        if(nilai>0)
        {
            for (int i = nilai ; i >= nilai; i--)
            {
                gambar[i].SetActive(false);
                gambar[i+1].SetActive(true);
            }
        }
    }

}
