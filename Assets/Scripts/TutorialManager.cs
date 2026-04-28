using System.Collections;
using UnityEngine;
using TMPro;

public enum GorevTipi
{
    None,
    EsyaSabitleme,
    CantaHazirlama,
    DepremAni
}

public class TutorialManager : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI gorevMetni;

    [Header("Audio Source")]
    public AudioSource sesKaynagi;

    [Header("Earthquake Sounds")]
    public AudioClip sirenSesi;

    [Header("Systems")]
    public DepremSistemi depremSistemi;

    [Header("Safe Zone Boxes")]
    public GameObject[] guvenliAlanKutulari;

    [Header("Part 1: Introduction")]
    public AudioClip ses1_Giris;
    public AudioClip ses2_OrtamTanitim;

    [Header("Part 2: Risk Hunt")]
    public AudioClip ses3_SabitlemeGorevi;
    public AudioClip ses4_SabitlemeBasarili;
    public int sabitlemeHedefSayisi = 6;
    private int sabitlemeMevcutSayi = 0;

    [Header("Part 3: Emergency Bag")]
    public AudioClip ses5_CantaGorevi;
    public AudioClip ses6_CantaBasarili;
    public int cantaHedefSayisi = 5;
    private int cantaMevcutSayi = 0;

    [Header("Part 4: Earthquake")]
    public AudioClip ses7_HayatUcgeni;
    public AudioClip ses8_DepremBasliyor;

    [Header("Part 5: Result")]
    public AudioClip ses9_Tahliye;
    public AudioClip ses10_Bitis;

    [Header("Active Task")]
    public GorevTipi aktifGorev = GorevTipi.None;

    private bool sabitlemeGoreviTamamlandi = false;
    private bool cantaGoreviTamamlandi = false;
    private bool oyuncuGuvende = false;

    void Start()
    {
        if (guvenliAlanKutulari != null)
        {
            foreach (GameObject kutu in guvenliAlanKutulari)
            {
                if (kutu != null)
                {
                    kutu.SetActive(false);
                }
            }
        }

        GorevMetniGuncelle("Training is Loading...");
        StartCoroutine(EgitimAkisi());
    }

    public void OyuncuGuvenliAlandaDurumu(bool durum)
    {
        oyuncuGuvende = durum;
    }

    IEnumerator EgitimAkisi()
    {
        GorevMetniGuncelle("Welcome\nSimulation Starting");
        yield return StartCoroutine(SesCalVeBekle(ses1_Giris));
        yield return new WaitForSeconds(0.5f);

        GorevMetniGuncelle("Explore the House\nLook Around");
        yield return StartCoroutine(SesCalVeBekle(ses2_OrtamTanitim));

        float gezmeSuresi = 10f;

        while (gezmeSuresi > 0)
        {
            GorevMetniGuncelle($"Explore the House\nLook Around\nTime Remaining: {Mathf.CeilToInt(gezmeSuresi)}");
            yield return new WaitForSeconds(1f);
            gezmeSuresi--;
        }

        sabitlemeMevcutSayi = 0;
        aktifGorev = GorevTipi.EsyaSabitleme;

        GorevMetniGuncelle($"TASK 1:\nSecure Dangerous Objects\n({sabitlemeMevcutSayi}/{sabitlemeHedefSayisi})");

        yield return StartCoroutine(SesCalVeBekle(ses3_SabitlemeGorevi));
        yield return new WaitUntil(() => sabitlemeGoreviTamamlandi);

        aktifGorev = GorevTipi.None;
        GorevMetniGuncelle("CONGRATULATIONS!\nAll Objects Secured");

        yield return StartCoroutine(SesCalVeBekle(ses4_SabitlemeBasarili));
        yield return new WaitForSeconds(1f);

        cantaMevcutSayi = 0;
        aktifGorev = GorevTipi.CantaHazirlama;

        GorevMetniGuncelle($"TASK 2:\nPrepare an Emergency Kit\n({cantaMevcutSayi}/{cantaHedefSayisi})");

        yield return StartCoroutine(SesCalVeBekle(ses5_CantaGorevi));
        yield return new WaitUntil(() => cantaGoreviTamamlandi);

        aktifGorev = GorevTipi.None;
        GorevMetniGuncelle("GREAT!\nBag is Ready");

        yield return StartCoroutine(SesCalVeBekle(ses6_CantaBasarili));
        yield return new WaitForSeconds(1f);

        GorevMetniGuncelle("TASK 3:\nFind a Life Triangle Zone\n(Under Table / Next to Sofa)");

        yield return StartCoroutine(SesCalVeBekle(ses7_HayatUcgeni));

        if (guvenliAlanKutulari != null)
        {
            foreach (GameObject kutu in guvenliAlanKutulari)
            {
                if (kutu != null)
                {
                    kutu.SetActive(true);
                }
            }
        }

        float hayatUcgeniSuresi = 10f;

        while (hayatUcgeniSuresi > 0)
        {
            GorevMetniGuncelle($"TASK 3:\nFind a Life Triangle Zone\nTime Remaining: {Mathf.CeilToInt(hayatUcgeniSuresi)}");
            yield return new WaitForSeconds(1f);
            hayatUcgeniSuresi--;
        }

        aktifGorev = GorevTipi.DepremAni;

        if (ses8_DepremBasliyor != null && sesKaynagi != null)
        {
            sesKaynagi.PlayOneShot(ses8_DepremBasliyor);
        }

        float sarsintiSuresi = 20f;

        if (sirenSesi != null && sesKaynagi != null)
        {
            sesKaynagi.clip = sirenSesi;
            sesKaynagi.loop = true;
            sesKaynagi.Play();
        }

        if (depremSistemi != null)
        {
            depremSistemi.DepremiBaslat(sarsintiSuresi);
        }

        float kalanSarsinti = sarsintiSuresi;

        while (kalanSarsinti > 0)
        {
            if (oyuncuGuvende)
            {
                GorevMetniGuncelle($"<color=green>YOU ARE SAFE!</color>\nWait Until the Shaking Stops\n({Mathf.CeilToInt(kalanSarsinti)})");
            }
            else
            {
                GorevMetniGuncelle($"<color=red>YOU ARE IN DANGER!</color>\nGet Under a Table!\nDROP - COVER - HOLD ON\n({Mathf.CeilToInt(kalanSarsinti)})");
            }

            yield return new WaitForSeconds(0.2f);
            kalanSarsinti -= 0.2f;
        }

        if (sesKaynagi != null && sesKaynagi.clip == sirenSesi)
        {
            sesKaynagi.Stop();
            sesKaynagi.loop = false;
            sesKaynagi.clip = null;
        }

        yield return new WaitForSeconds(2f);

        aktifGorev = GorevTipi.None;

        GorevMetniGuncelle("Stay Calm\nEvacuate the Building Safely");
        yield return StartCoroutine(SesCalVeBekle(ses9_Tahliye));

        yield return new WaitForSeconds(3f);

        GorevMetniGuncelle("TRAINING COMPLETED");
        yield return StartCoroutine(SesCalVeBekle(ses10_Bitis));
    }

    IEnumerator SesCalVeBekle(AudioClip klip)
    {
        if (klip != null && sesKaynagi != null)
        {
            sesKaynagi.PlayOneShot(klip);
            yield return new WaitForSeconds(klip.length);
        }
    }

    public void GoreviIlerlet(GorevTipi gelenGorev)
    {
        if (aktifGorev != gelenGorev)
        {
            return;
        }

        if (gelenGorev == GorevTipi.EsyaSabitleme)
        {
            sabitlemeMevcutSayi++;

            GorevMetniGuncelle($"TASK 1:\nSecure Dangerous Objects\n({sabitlemeMevcutSayi}/{sabitlemeHedefSayisi})");

            if (sabitlemeMevcutSayi >= sabitlemeHedefSayisi)
            {
                sabitlemeGoreviTamamlandi = true;
            }
        }
        else if (gelenGorev == GorevTipi.CantaHazirlama)
        {
            cantaMevcutSayi++;

            GorevMetniGuncelle($"TASK 2:\nPrepare an Emergency Kit\n({cantaMevcutSayi}/{cantaHedefSayisi})");

            if (cantaMevcutSayi >= cantaHedefSayisi)
            {
                cantaGoreviTamamlandi = true;
            }
        }
    }

    void GorevMetniGuncelle(string yeniMetin)
    {
        if (gorevMetni != null)
        {
            gorevMetni.text = yeniMetin;
        }
    }

    public void GoreviIlerlet(int gelenGorevIndex)
    {
        GoreviIlerlet((GorevTipi)gelenGorevIndex);
    }
}