using System.Collections;
using UnityEngine;
using TMPro;

public enum GorevTipi
{
    None,
    EsyaSabitleme,
    CantaHazirlama
}

public class TutorialManager : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public TextMeshProUGUI gorevMetni;

    [Header("Ses Kaynaðý")]
    public AudioSource sesKaynagi;

    [Header("Bölüm 1: Giriþ")]
    public AudioClip ses1_Giris;
    public AudioClip ses2_OrtamTanitim;

    [Header("Bölüm 2: Risk Avý (Sabitleme)")]
    public AudioClip ses3_SabitlemeGorevi;
    public AudioClip ses4_SabitlemeBasarili;
    public int sabitlemeHedefSayisi = 6;
    private int sabitlemeMevcutSayi = 0;

    [Header("Bölüm 3: Çanta Hazýrlýðý")]
    public AudioClip ses5_CantaGorevi;
    public AudioClip ses6_CantaBasarili;
    public int cantaHedefSayisi = 5;
    private int cantaMevcutSayi = 0;

    [Header("Bölüm 4: Deprem")]
    public AudioClip ses7_HayatUcgeni;
    public AudioClip ses8_DepremBasliyor;

    [Header("Bölüm 5: Sonuç")]
    public AudioClip ses9_Tahliye;
    public AudioClip ses10_Bitis;

    [Header("Aktif Görev")]
    public GorevTipi aktifGorev = GorevTipi.None;

    private bool sabitlemeGoreviTamamlandi = false;
    private bool cantaGoreviTamamlandi = false;
    private bool depremSarsintisiBitti = false;

    void Start()
    {
        GorevMetniGuncelle("Eðitim Yükleniyor...");
        StartCoroutine(EgitimAkisi());
    }

    IEnumerator EgitimAkisi()
    {
        // --- ADIM 1: GÝRÝÞ ---
        GorevMetniGuncelle("Hoþgeldiniz\nSimülasyon Baþlatýlýyor");
        yield return StartCoroutine(SesCalVeBekle(ses1_Giris));

        yield return new WaitForSeconds(0.5f);

        // --- ADIM 2: ORTAMI GEZME VE GERÝ SAYIM (YENÝLENEN KISIM) ---
        // Önce ses çalsýn
        GorevMetniGuncelle("Evi Tanýyýn\nEtrafýnýza Göz Atýn");
        yield return StartCoroutine(SesCalVeBekle(ses2_OrtamTanitim));

        // Ses bittikten sonra 30 saniye geri sayým baþlasýn
        float gezmeSuresi = 30f;
        while (gezmeSuresi > 0)
        {
            // UI'ý her saniye güncelle
            GorevMetniGuncelle($"Evi Tanýyýn\nEtrafýnýza Göz Atýn\nKalan Süre: {Mathf.CeilToInt(gezmeSuresi)}");

            yield return new WaitForSeconds(1f); // 1 saniye bekle
            gezmeSuresi--; // Süreyi azalt
        }

        // --- ADIM 3: EÞYA SABÝTLEME ---
        sabitlemeMevcutSayi = 0;
        aktifGorev = GorevTipi.EsyaSabitleme;

        GorevMetniGuncelle($"GÖREV 1:\nTehlikeli Eþyalarý Sabitle\n({sabitlemeMevcutSayi}/{sabitlemeHedefSayisi})");

        yield return StartCoroutine(SesCalVeBekle(ses3_SabitlemeGorevi));

        Debug.Log("Sabitleme bekleniyor...");
        yield return new WaitUntil(() => sabitlemeGoreviTamamlandi);

        aktifGorev = GorevTipi.None;
        GorevMetniGuncelle("TEBRÝKLER!\nTüm Eþyalar Sabitlendi");

        yield return StartCoroutine(SesCalVeBekle(ses4_SabitlemeBasarili));
        yield return new WaitForSeconds(1f);

        // --- ADIM 4: ÇANTA HAZIRLAMA ---
        cantaMevcutSayi = 0;
        aktifGorev = GorevTipi.CantaHazirlama;

        GorevMetniGuncelle($"GÖREV 2:\nDeprem Çantasýný Hazýrla\n({cantaMevcutSayi}/{cantaHedefSayisi})");

        yield return StartCoroutine(SesCalVeBekle(ses5_CantaGorevi));

        Debug.Log("Çanta bekleniyor...");
        yield return new WaitUntil(() => cantaGoreviTamamlandi);

        aktifGorev = GorevTipi.None;
        GorevMetniGuncelle("HARÝKA!\nÇanta Hazýr");

        yield return StartCoroutine(SesCalVeBekle(ses6_CantaBasarili));
        yield return new WaitForSeconds(1f);

        // --- ADIM 5: HAYAT ÜÇGENÝ ---
        // Burada da benzer bir geri sayým yapabiliriz (ses7'den sonra)
        GorevMetniGuncelle("GÖREV 3:\nHayat Üçgeni Bölgesi Belirle");
        yield return StartCoroutine(SesCalVeBekle(ses7_HayatUcgeni));

        float hayatUcgeniSuresi = 30f;
        while (hayatUcgeniSuresi > 0)
        {
            GorevMetniGuncelle($"GÖREV 3:\nHayat Üçgeni Bölgesi Belirle\nKalan Süre: {Mathf.CeilToInt(hayatUcgeniSuresi)}");
            yield return new WaitForSeconds(1f);
            hayatUcgeniSuresi--;
        }

        // --- DEPREM ANI ---
        GorevMetniGuncelle("DEPREM OLUYOR!\nÇÖK - KAPAN - TUTUN");
        yield return StartCoroutine(SesCalVeBekle(ses8_DepremBasliyor));

        Debug.Log("Sarsýntý bekleniyor...");
        yield return new WaitUntil(() => depremSarsintisiBitti);

        // --- ADIM 6: TAHLÝYE ---
        yield return new WaitForSeconds(2f);
        GorevMetniGuncelle("Sakin Olun\nBinayý Güvenle Terk Edin");
        yield return StartCoroutine(SesCalVeBekle(ses9_Tahliye));

        yield return new WaitForSeconds(3f);
        GorevMetniGuncelle("EÐÝTÝM TAMAMLANDI");
        yield return StartCoroutine(SesCalVeBekle(ses10_Bitis));
    }

    IEnumerator SesCalVeBekle(AudioClip klip)
    {
        if (klip != null)
        {
            sesKaynagi.PlayOneShot(klip);
            yield return new WaitForSeconds(klip.length);
        }
    }

    public void GoreviIlerlet(GorevTipi gelenGorev)
    {
        if (aktifGorev != gelenGorev) return;

        if (gelenGorev == GorevTipi.EsyaSabitleme)
        {
            sabitlemeMevcutSayi++;
            GorevMetniGuncelle($"GÖREV 1:\nTehlikeli Eþyalarý Sabitle\n({sabitlemeMevcutSayi}/{sabitlemeHedefSayisi})");

            if (sabitlemeMevcutSayi >= sabitlemeHedefSayisi)
            {
                sabitlemeGoreviTamamlandi = true;
            }
        }
        else if (gelenGorev == GorevTipi.CantaHazirlama)
        {
            cantaMevcutSayi++;
            GorevMetniGuncelle($"GÖREV 2:\nDeprem Çantasýný Hazýrla\n({cantaMevcutSayi}/{cantaHedefSayisi})");

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

    public void DepremBitti()
    {
        depremSarsintisiBitti = true;
    }
}