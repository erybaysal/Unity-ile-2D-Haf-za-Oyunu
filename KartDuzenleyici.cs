using UnityEngine;
using UnityEngine.UI;

public class KartDuzenleyici : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public GameObject kartPrefab;
    public int kartSayisi;


    void Start()
    {
        DuzenleKartlar();
    }
    private void DuzenleKartlar()
    {

        for (int y = 0; y < kartSayisi; y++)
        {
            
                GameObject kart = Instantiate(kartPrefab, transform);
        }
    }
}
