using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class imgDegistir : MonoBehaviour
{

    //kart özellikleri
    [SerializeField] Image on_yuz;
    [SerializeField] int kart_no;
    [SerializeField] bool eslesti = false;
    [SerializeField] AudioSource src;
    [SerializeField] TMP_Text tmpText;
    [SerializeField] TMP_Text target;

    //kart islemleri
    static Queue<imgDegistir> images;
    bool corotineAllowed = true;

    //kart dondurme
    private RectTransform imageRectTransform;
    private Quaternion originalRotation;

    public float rotationSpeed = 2.0f; 
    private bool canToggle = true;
    static int ciftKart = 0;

    //buton islemleri
    int islem;
    bool islemSecildi = false;
    [SerializeField] Button carpým_Btn;
    [SerializeField] Button toplam_Btn;
    [SerializeField] Button cýkarým_Btn;
    [SerializeField] Button bolum_Btn;

    private void Start()
    {

        //tmp objesinin scale ayarý
        Vector3 initialScale = new Vector3(0.6f, 3.4154f, 1.7f);
        tmpText.transform.localScale = initialScale;

        //transparan TMP Yapýyoruz
        Color newColar = tmpText.color;
        newColar.a = 0;
        tmpText.color = newColar;
        images = new Queue<imgDegistir>();

        //kartlara sayý atama
        kart_no = Random.Range(1, 10);
        tmpText.text = kart_no.ToString();

        //Targete sayý atama
        kart_no = Random.Range(1, 10);
        target.text = kart_no.ToString();

        //kart dondurme
        imageRectTransform = GetComponent<RectTransform>();

    }

    public void islemSec(int islemNo)
    {
         islem=islemNo;
         islemSecildi = true;
    }

    public void OnImageClick()
    {
        ciftKart++;
        Debug.Log(ciftKart);
        if (corotineAllowed && canToggle && ciftKart <= 2)
        {
            Debug.Log("baSÝLDÝ");

            StartCoroutine(kart_Ac());
        }
    }

    IEnumerator kart_Ac()
    {

        corotineAllowed = false;
        originalRotation = this.imageRectTransform.rotation;
       

        //TMP
        Quaternion currentRotation = this.tmpText.transform.rotation;
        Quaternion newRotation = Quaternion.Euler(currentRotation.eulerAngles.x, currentRotation.eulerAngles.y + 180f, currentRotation.eulerAngles.z);
        this.tmpText.transform.rotation = newRotation;

        images.Enqueue(this);
        yield return new WaitForEndOfFrame(); // Bir sonraki frame'in sonunu bekler

        for (float i = 0; i < 180; i += 1f)
        {

            this.imageRectTransform.rotation = originalRotation * Quaternion.Euler(0, i, 0);
            yield return new WaitForEndOfFrame(); // Bir sonraki frame'in sonunu bekler

            if (Convert.ToInt32(i) == 90)
            {
                Color newColar = tmpText.color;
                newColar.a = 1;
                this.tmpText.color = newColar;
                yield return new WaitForEndOfFrame(); // Bir sonraki frame'in sonunu bekler

            }

        }
        
        corotineAllowed = true;
        yield return new WaitForEndOfFrame(); // Bir sonraki frame'in sonunu bekler
    }

    IEnumerator kart_Kapat(imgDegistir k1, imgDegistir k2)
    {

        corotineAllowed = false;
        yield return new WaitForSeconds(1f);

        //TMP döndürme
        Quaternion currentRotation_k1 = k1.tmpText.transform.rotation;
        Quaternion currentRotation_k2 = k2.tmpText.transform.rotation;

        Quaternion newRotation_k1 = Quaternion.Euler(currentRotation_k1.eulerAngles.x, currentRotation_k1.eulerAngles.y + 180f, currentRotation_k1.eulerAngles.z);
        Quaternion newRotation_k2 = Quaternion.Euler(currentRotation_k2.eulerAngles.x, currentRotation_k2.eulerAngles.y + 180f, currentRotation_k2.eulerAngles.z);

        k1.tmpText.transform.rotation = newRotation_k1;
        k2.tmpText.transform.rotation = newRotation_k2;

        //kartý döndürme
        for (float i = 180; i > 0; i -= 1f)
        {

            k1.imageRectTransform.rotation = originalRotation * Quaternion.Euler(0, i, 0);

            k2.imageRectTransform.rotation = originalRotation * Quaternion.Euler(0, i, 0);
            yield return new WaitForEndOfFrame();


            //TMP objesinin rotasyonunun düzeltilmesi 

            if (Convert.ToInt32(i) == 90)
            {

                //TMP objesinin transparan yapýlmasý
                Color newColar = tmpText.color;
                newColar.a = 0;
                k1.tmpText.color = newColar;
                k2.tmpText.color = newColar;


                //kartlarýn beyaza dönmesi
                k1.on_yuz.color = Color.white;
                k2.on_yuz.color = Color.white;

            }

        }

        corotineAllowed = true;
        yield return new WaitForEndOfFrame();

    }

    IEnumerator EslesmeKontrol()
    {

        yield return new WaitForSeconds(0.3f);

        //kartlarý kuyruktan al
        imgDegistir kart1 = images.Dequeue();
        imgDegistir kart2 = images.Dequeue();

        int kart1NO = int.Parse(kart1.tmpText.text);
        int kart2NO = int.Parse(kart2.tmpText.text);

        //kart secimine izin ver
        ciftKart = 0;


        //secime göre islem yap
        switch (islem)
        {

            case 1:


                if (float.Parse(target.text) == ((float)kart1NO / (float)kart2NO))
                {

                    Debug.Log("kart bolüm sonujcu=============    " + ((float)kart1NO / (float)kart2NO));
                    kart1.eslesti = true;
                    kart2.eslesti = true;
                    kart1.on_yuz.color = Color.yellow;
                    kart2.on_yuz.color = Color.yellow;
                    target.text = Random.Range(1, 10).ToString();
                }


                else
                {
                    Debug.Log("eslesme YOK");
                    kart1.on_yuz.color = Color.red;
                    kart2.on_yuz.color = Color.red;
                    yield return StartCoroutine(kart_Kapat(kart1, kart2)); //kartlarý kapat
                }

                break;

            case 2:

                if (int.Parse(target.text) == kart1NO + kart2NO)
                {
                    Debug.Log("eslesti");
                    kart1.eslesti = true;
                    kart2.eslesti = true;
                    kart1.on_yuz.color = Color.yellow;
                    kart2.on_yuz.color = Color.yellow;
                    target.text = Random.Range(1, 10).ToString();
                }


                else
                {
                    Debug.Log("eslesme YOK");
                    kart1.on_yuz.color = Color.red;
                    kart2.on_yuz.color = Color.red;
                    yield return StartCoroutine(kart_Kapat(kart1, kart2)); //kartlarý kapat
                }

                break;

            case 3:

                islemSecildi = false;
                islem = -1;

                if (int.Parse(target.text) == kart1NO - kart2NO)
                {
                    Debug.Log("eslesti");
                    kart1.eslesti = true;
                    kart2.eslesti = true;
                    kart1.on_yuz.color = Color.yellow;
                    kart2.on_yuz.color = Color.yellow;
                    target.text = Random.Range(1, 10).ToString();
                }


                else
                {
                    Debug.Log("eslesme YOK");
                    kart1.on_yuz.color = Color.red;
                    kart2.on_yuz.color = Color.red;
                    yield return StartCoroutine(kart_Kapat(kart1, kart2)); //kartlarý kapat
                }
                break;

            case 4:

                islemSecildi = false;
                islem = -1;

                if (int.Parse(target.text) == kart1NO * kart2NO)
                {
                    Debug.Log("eslesti");
                    kart1.eslesti = true;
                    kart2.eslesti = true;
                    kart1.on_yuz.color = Color.yellow;
                    kart2.on_yuz.color = Color.yellow;
                    target.text = Random.Range(1, 10).ToString();
                }


                else
                {
                    Debug.Log("eslesme YOK");
                    kart1.on_yuz.color = Color.red;
                    kart2.on_yuz.color = Color.red;
                    yield return StartCoroutine(kart_Kapat(kart1, kart2)); //kartlarý kapat
                }
                break;

            default:
                Debug.Log("islem sec");
                break;
        }

    }

    private void  Update()
    {
        //islem secimi 
        bolum_Btn  .onClick .AddListener(() => islemSec(1));
        toplam_Btn .onClick .AddListener(() => islemSec(2));
        cýkarým_Btn.onClick .AddListener(() => islemSec(3));
        carpým_Btn .onClick .AddListener(() => islemSec(4));

        //acýk kart kontrolü
        if (images.Count == 2 && islemSecildi && eslesti==false)
        {
            islemSecildi = false;
            StartCoroutine(EslesmeKontrol()); 
        }
    }

    public void ResetToggle()
    {
        canToggle = true;
    }

}
