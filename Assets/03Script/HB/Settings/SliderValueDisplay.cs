using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    [Header("연결 설정")]
    [SerializeField] private Slider _slider;                    // 값을 가져올 슬라이더
    [SerializeField] private TextMeshProUGUI _textDisplay;        // 값을 표시할 텍스트

    private void Awake()
    {
        if (_slider != null)
        {
            // 값이 변할 때마다 실행될 리스너
            _slider.onValueChanged.AddListener(UpdateVolumeText);

            //시작할 때 현재 슬라이더 값으로 초기화
            UpdateVolumeText(_slider.value);
        }
    }

    private void UpdateVolumeText(float value)
    {
        if(_textDisplay != null)
        {
            // 정수만 출력
            _textDisplay.text = value.ToString("0");
        }
    }

    private void OnDestroy()
    {
        if (_slider != null)
        {
            _slider.onValueChanged.RemoveListener(UpdateVolumeText);
        }
    }
}
