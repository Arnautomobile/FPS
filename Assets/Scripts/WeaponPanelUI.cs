using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _aimDot;
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private GameObject[] _infos;
    [SerializeField] private GameObject _ammoInfo;
    [SerializeField] private GameObject _meleeInfo;
    [SerializeField] private GameObject[] _ammoImages;
    [SerializeField] private TextMeshProUGUI _lethalText;
    [SerializeField] private TextMeshProUGUI _tacticalText;

    private WeaponsManager _weaponsManager;
    private TextMeshProUGUI _ammoLeft;
    private TextMeshProUGUI _totalAmmo;


    void Start()
    {
        _weaponsManager = _player.GetComponent<WeaponsManager>();
        _ammoLeft = _ammoInfo.GetComponentsInChildren<TextMeshProUGUI>()[0];
        _totalAmmo = _ammoInfo.GetComponentsInChildren<TextMeshProUGUI>()[1];
    }


    void Update()
    {
        _lethalText.text = _weaponsManager.Throwables[ConsummableType.GRENADE] + "";
        _tacticalText.text = _weaponsManager.Throwables[ConsummableType.FLASHBANG] + "";


        for (int i = 0; i < 2; i++) {
            RectTransform rectTransform = _weapons[i].GetComponent<RectTransform>();
            Image image = _weapons[i].GetComponent<Image>();
            Color color = image.color;
            WeaponScript weaponScript = _weaponsManager.Weapons[i];

            if (i == _weaponsManager.Selected) {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 2.5f);
                color.a = 0.27f;
                image.color = color;
            }
            else {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -2.5f);
                color.a = 0.08f;
                image.color = color;
            }

            if (weaponScript == null) {
                _infos[i].SetActive(false);
            }
            else {
                _infos[i].SetActive(true);
                _infos[i].GetComponentInChildren<TextMeshProUGUI>().text = weaponScript.gameObject.name;
                _infos[i].GetComponentInChildren<Image>().sprite = weaponScript.Sprite;
            }
        }

        WeaponScript selectedWeapon = null;
        if (_weaponsManager.Selected < 2)
            selectedWeapon = _weaponsManager.Weapons[_weaponsManager.Selected];

        if (selectedWeapon == null || selectedWeapon.Type == WeaponType.MELEE) {
            _meleeInfo.SetActive(true);
            _ammoInfo.SetActive(false);
            _aimDot.SetActive(true);
            return;
        }
        _meleeInfo.SetActive(false);
        _ammoInfo.SetActive(true);

        for (int i = 0; i < _ammoImages.Length; i++) {
            if ((int)selectedWeapon.Type == i) {
                _ammoImages[i].SetActive(true);
            }
            else {
                _ammoImages[i].SetActive(false);
            }
        }

        FireArm fireArm = (FireArm)selectedWeapon;
        _aimDot.SetActive(!fireArm.Aiming);

        string ammoText = "";
        if (fireArm.BulletsLeft < 10) {
            ammoText += "0";
        }
        ammoText += fireArm.BulletsLeft;

        _ammoLeft.text = ammoText;
        _totalAmmo.text = _weaponsManager.Ammunitions[fireArm.Type].ToString();
    }
}
