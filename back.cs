using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class back : MonoBehaviour
{

    [SerializeField] private AudioClip _clicked;
    [SerializeField] private AudioSource source;

    int yeniSahneIndeksi=0;

    public void OnMouseDown()
    {
        source.PlayOneShot(_clicked);
        SceneManager.LoadScene(yeniSahneIndeksi);

    }


}
