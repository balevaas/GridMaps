using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap
{

    /// <summary>
    /// Перечисление типов тайлов
    /// </summary>
    public enum MapItemType
    {
        Light,
        LightAdditional,
        Night,
        NightAdditional,
    }

    // интервал в процентах, сколько ячеек может занять указанный тип
    public float minPercentLightType = 0.1f;
    public float maxPercentLightType = 0.4f;

    // интервал в процентах, сколько ячеек может занять указанный тип
    public float minPercentNightType = 0.1f;
    public float maxPercentNightType = 0.4f;

    /// <summary>
    /// Генерация матрицы
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public MapItemType[][] GenerateMatrix(int width, int height)
    {
        MapItemType[][] matrix = new MapItemType[height][];

        for (int i = 0; i < matrix.Length; i++)
        {
            // для каждой нечетной строки минусуем один элемент
            var offset = i % 2 == 0 ? 0 : 1;
            var calculatedWidth = width - offset;
            matrix[i] = MapItemsForLine(calculatedWidth);
        }

        return matrix;
    }

    /// <summary>
    /// Генерация линии карты с заданым количеством элементов
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
    private MapItemType[] MapItemsForLine(int width)
    {
        // количество элементов карты в расчете с процентов
        int minCountLightType = (int)Mathf.Ceil(width * minPercentLightType);
        int maxCountLightType = (int)Mathf.Ceil(width * maxPercentLightType);

        // количество элементов карты в расчете с процентов
        int minCountNightType = (int)Mathf.Ceil(width * minPercentNightType);
        int maxCountNightType = (int)Mathf.Ceil(width * maxPercentNightType);

        // рандомное значение самых левых элементов, в указанных границах 
        int lightTypeCount = Random.Range(minCountLightType, maxCountLightType);
        // рандомное значение самых правых элементов, в указанных границах 
        int nightTypeCount = Random.Range(minCountNightType, maxCountNightType);

        // рандомное значение дополнительных элементов с левой стороны, от 0 до максимально свободного количества элементов
        int lightAdditionalLightTypeCount = Random.Range(0, width - (lightTypeCount + nightTypeCount));
        // рандомное значение дополнительных элементов с правой стороны, остальное свободное количество элементов
        int lightAdditionalNightTypeCount = width - (lightTypeCount + nightTypeCount + lightAdditionalLightTypeCount);

        List<MapItemType> items = new List<MapItemType>();

        // добавляем к списку количество элементов для нужного типа, именно в нужно порядке
        AddMapItem(items, MapItemType.Light, lightTypeCount);
        AddMapItem(items, MapItemType.LightAdditional, lightAdditionalLightTypeCount);
        AddMapItem(items, MapItemType.NightAdditional, lightAdditionalNightTypeCount);
        AddMapItem(items, MapItemType.Night, nightTypeCount);

        return items.ToArray();
    }

    /// <summary>
    /// Добавление тайлов в список
    /// </summary>
    /// <param name="items"></param>
    /// <param name="itemType"></param>
    /// <param name="count"></param>
    private void AddMapItem(List<MapItemType> items, MapItemType itemType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            items.Add(itemType);
        }
    }
}
