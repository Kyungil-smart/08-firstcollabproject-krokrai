using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisposeFood : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _recpieName;
    [SerializeField] TextMeshProUGUI[] _texts; // 1. alert, 2 : 남은 갯수, 3 : 아니오, 4 : 네;
    [SerializeField] AddressableImageLoader _imageLoader;
    [SerializeField] TranslationDataReader _reader;

    TranslationData[] _datas;
    RecipeContainer _recipeData;
    DishUI _ui;

    private void Start()
    {
        _datas[0] = _reader.GetTranslationData("Discard_Confirm");
        _datas[1] = _reader.GetTranslationData("Remaining");
        _datas[2] = _reader.GetTranslationData("No");
        _datas[3] = _reader.GetTranslationData("Yes");
    }

    public void SetData(RecipeContainer So, DishUI ui)
    {
        _recipeData = So;
        _ui = ui;
        //_imageLoader.SetImage(So.dish_Sprite);

        switch (DataTower.instance.languageSetting)
        {
            case Language.KOR:
                _recpieName.text = So.recipe_KName;
                _texts[0].text = _datas[0].Kor;
                _texts[1].text = $"{_datas[1].Kor} : {_recipeData.yield}";
                _texts[2].text = _datas[2].Kor;
                _texts[3].text = _datas[3].Kor;

                break;
            case Language.ENG:
                _recpieName.text = So.recipe_EName;
                _texts[0].text = _datas[0].En;
                _texts[1].text = $"{_datas[1].En} : {_recipeData.yield}";
                _texts[2].text = _datas[2].En;
                _texts[3].text = _datas[3].En;
                break;
        }
    }

    public void Cancel()
    {
        _ui = null;
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        _ui.DeleteMenuinPopup();
        gameObject.SetActive(false);
    }
}
