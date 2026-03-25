/*
 * 조리 형태 표기용 Enum
 */

/// <summary>
/// NULL  : 숫자 형태의 글자가 아니거나, 비어 있는 경우
/// FRIED : 튀김
/// BOILDE : 삶음
/// STIR_FRIED : 볶음
/// GRILLED : 구음
/// BRAISED : 조림
/// RAW : 날것, 회
/// </summary>
public enum ERecipe_Type
{
    Null = -1, FRIED = 0, BOILED, STIR_FRIED, GRILLED, BRAISED, RAW
}
