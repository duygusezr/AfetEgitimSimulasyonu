using System.Collections;
using UnityEngine;

public class DepremSistemi : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform oyuncuKarakteri; // XR Origin
    public float sarsintiSiddeti = 0.8f;
    public float sarsintiHizi = 18f;

    [Header("Eþya Fiziði")]
    public bool esyalariSalla = true;
    public float esyaSallamaGucu = 1.5f;

    private Vector3 orijinalPozisyon;

    public void DepremiBaslat(float sure)
    {
        // Deprem baþlarken önce eþyalarýn kilidini açýyoruz
        EsyalarinFiziginiBaslat();

        StartCoroutine(DepremSureci(sure));
    }

    // YENÝ FONKSÝYON: Uyuyan eþyalarý uyandýrýr
    void EsyalarinFiziginiBaslat()
    {
        Rigidbody[] tumEsyalar = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

        foreach (Rigidbody rb in tumEsyalar)
        {
            // Eðer obje oyuncu deðilse (Karakteri düþürmeyelim)
            if (rb.transform != oyuncuKarakteri && rb.transform.root != oyuncuKarakteri)
            {
                // Kinematic kilidini kaldýr, artýk yerçekimi ve sarsýntýdan etkilensin
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }

    IEnumerator DepremSureci(float sure)
    {
        // Oyuncunun o anki konumunu kaydet
        if (oyuncuKarakteri != null)
        {
            orijinalPozisyon = oyuncuKarakteri.localPosition;
        }

        float gecenSure = 0f;
        float vurusZamanlayicisi = 0f;

        while (gecenSure < sure)
        {
            // 1. OYUNCUYU SALLA
            if (oyuncuKarakteri != null)
            {
                float x = (Mathf.PerlinNoise(Time.time * sarsintiHizi, 0f) - 0.5f) * sarsintiSiddeti;
                float y = (Mathf.PerlinNoise(0f, Time.time * sarsintiHizi) - 0.5f) * sarsintiSiddeti;
                oyuncuKarakteri.localPosition = orijinalPozisyon + new Vector3(x, y, 0);
            }

            // 2. EÞYALARI SALLA
            if (esyalariSalla)
            {
                vurusZamanlayicisi -= Time.deltaTime;

                if (vurusZamanlayicisi <= 0)
                {
                    Rigidbody[] tumEsyalar = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

                    foreach (Rigidbody rb in tumEsyalar)
                    {
                        // Sadece artýk Kinematic OLMAYANLARI salla
                        if (!rb.isKinematic)
                        {
                            Vector3 rastgeleYon = Random.insideUnitSphere;
                            rastgeleYon.y = 0; // Yukarý uçmayý engelle
                            rb.AddForce(rastgeleYon * esyaSallamaGucu, ForceMode.Impulse);
                        }
                    }
                    vurusZamanlayicisi = 0.1f; // Saniyede 10 vuruþ
                }
            }

            gecenSure += Time.deltaTime;
            yield return null;
        }

        // Deprem bitti, oyuncuyu yerine koy
        if (oyuncuKarakteri != null)
            oyuncuKarakteri.localPosition = orijinalPozisyon;
    }
}