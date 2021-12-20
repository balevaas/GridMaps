using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridView : MonoBehaviour
{
    private GridMap gridMap = new GridMap();

    // Размер карты
    [Header("Map size")]
    public int mapWidthItemCount = 10;
    public int mapHeightItemCount = 10;

    // Массивы для стороны героев
    [Header("Light")]
    public Sprite[] lightSprites;
    public Sprite[] lightAdditionalSprites;

    // Массивы для стороны врагов
    [Header("Night")]
    public Sprite[] nightSprites;
    public Sprite[] nightAdditionalSprites;

    // Префаб для тайлов
    [Header("Prefabs")]
    public GameObject itemPrefab;

    // Коэффициент для смещения по вертикале элементов
    private readonly float mapItemOffsetFactor = 1.3f;

    /// <summary>
    /// Ширина области карты, зависящая от размера canvas
    /// </summary>
    private float MapWidth
    {
        get
        {
            return GetComponent<RectTransform>().rect.width;
        }
    }

    /// <summary>
    /// Высота области карты, зависящая от размеров canvas
    /// </summary>
    private float MapHeight
    {
        get
        {
            return GetComponent<RectTransform>().rect.height;
        }
    }

    /// <summary>
    /// Размер элемента карты в зависимости от размера всей карты
    /// </summary>
    private Vector2 MapItemSize
    {
        get
        {
            var defaultItemSize = itemPrefab.GetComponent<RectTransform>().sizeDelta;
            // множитель соотношения сторон для ячейки префаба
            var aspectFactor = defaultItemSize.y / defaultItemSize.x;

            // расчет ширины ячейки под ширину экрана
            var itemWidth = MapWidth / mapWidthItemCount;
            // высота расчитывается пропорционально от новой ширины
            return new Vector2(itemWidth, itemWidth * aspectFactor);
        }
    }

    void Start()
    {
        CreateMap();
    }

    /// <summary>
    /// Создание карты
    /// </summary>
    private void CreateMap()
    {
        // количество элементов, которое можем уместить по высоте карты в зависимости от размера ячейки
        var mapHeightItemCount = (int)(MapHeight / Mathf.Ceil(MapItemSize.y / mapItemOffsetFactor));
        // сгенерить матрицу карты
        var matrix = gridMap.GenerateMatrix(mapWidthItemCount, this.mapHeightItemCount <= mapHeightItemCount ? this.mapHeightItemCount : mapHeightItemCount);

        // смещение ячеек по горизонтале
        var deltaPositionX = MapWidth / mapWidthItemCount;
        // смещение ячеек по вертикале
        var deltaPositionY = MapItemSize.y / mapItemOffsetFactor;

        for (int i = 0; i < matrix.Length; i++)
        {
            // смещение для каждой нечетной линии карты
            var offsetX = i % 2 == 0 ? 0 : MapItemSize.x / 2;
            var offsetY = 0;

            // начальная координата ячейки по X, от центра области карты
            var startPositionX = -MapWidth / 2 + MapItemSize.x / 2 + offsetX;
            // начальная координата ячейки по Y, от центра области карты
            var startPositionY = -MapHeight / 2 + MapItemSize.y / 2 + offsetY;

            for (int j = 0; j < matrix[i].Length; j++)
            {
                // расчет координаты ячейки
                var itemPosition = new Vector2(startPositionX + deltaPositionX * j, startPositionY + deltaPositionY * i);
                CreateMapItem(matrix[i][j], itemPosition);
            }
        }
    }

    /// <summary>
    /// Создать объект с указанием типа и расположения
    /// </summary>
    /// <param name="mapItemType"></param>
    /// <param name="position"></param>
    private void CreateMapItem(GridMap.MapItemType mapItemType, Vector2 position)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.SetParent(transform);
        var imageMap = GetImageMap(mapItemType);
        item.transform.localPosition = position;
        item.GetComponent<MapItem>().SetImage(imageMap);

        item.GetComponent<RectTransform>().sizeDelta = MapItemSize;
    }

    /// <summary>
    /// Подбор картинки ячейки в зависимости от типа
    /// </summary>
    /// <param name="mapItemType"></param>
    /// <returns></returns>
    private Sprite GetImageMap(GridMap.MapItemType mapItemType)
    {
        switch (mapItemType)
        {
            case GridMap.MapItemType.Light:
                {
                    int index = Random.Range(0, lightSprites.Length);
                    return lightSprites[index];
                }
            case GridMap.MapItemType.LightAdditional:
                {
                    int index = Random.Range(0, lightAdditionalSprites.Length);
                    return lightAdditionalSprites[index];
                }
            case GridMap.MapItemType.Night:
                {
                    int index = Random.Range(0, nightSprites.Length);
                    return nightSprites[index];
                }
            case GridMap.MapItemType.NightAdditional:
                {
                    int index = Random.Range(0, nightAdditionalSprites.Length);
                    return nightAdditionalSprites[index];
                }
        }
        return null;
    }
}
