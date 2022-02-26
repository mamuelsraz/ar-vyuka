using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGroup : LayoutGroup
{
    [Space]
    public float height;
    public Vector2 margin;
    public bool onMiddle;
    float currentY = 0;

    public override void CalculateLayoutInputHorizontal()
    {
        PlaceChildren();
    }

    void PlaceChildren()
    {
        currentY = 0;

        base.CalculateLayoutInputHorizontal();

        float maxWidth = rectTransform.rect.width;
        int beginChild = 0;
        int lastChild = 0;


        for (int i = 0; i < rectChildren.Count; i++)
        {
            if (i >= beginChild)
            {

                float elementWidth = 0;
                lastChild = GetLastChildThatCanFit(beginChild, ref elementWidth);

                if (onMiddle) PopulateCenter(beginChild, lastChild, elementWidth);
                else PopulateNormal(beginChild, lastChild);

                beginChild = lastChild + 1;

                currentY -= height + margin.y;
            }
        }
    }

    int GetLastChildThatCanFit(int beginChild, ref float width)
    {
        float overallWidth = 0;
        int lastChild = rectChildren.Count - 1;

        for (int ii = beginChild; ii < rectChildren.Count; ii++)
        {
            overallWidth += rectChildren[ii].rect.width + margin.x;

            if ((overallWidth - margin.x) > rectTransform.rect.width)
            {
                lastChild = ii - 1;
                width = overallWidth - margin.x*2 - rectChildren[ii].rect.width;
                return lastChild;
            }
        }

        width = overallWidth - margin.x;
        return lastChild;
    }

    void PopulateNormal(int beginChild, int lastChild)
    {
        float currentX = 0;
        //populate the row
        for (int ii = beginChild; ii <= lastChild; ii++)
        {
            rectChildren[ii].sizeDelta = new Vector2(rectChildren[ii].sizeDelta.x, height);
            rectChildren[ii].anchoredPosition = new Vector2(currentX, currentY);
            currentX += rectChildren[ii].rect.width + margin.x;
        }
    }

    void PopulateCenter(int beginChild, int lastChild, float elementWidth)
    {
        float currentX = 0;
        float holeWidth = rectTransform.rect.width - elementWidth;

        //populate the row
        for (int ii = beginChild; ii <= lastChild; ii++)
        {
            rectChildren[ii].sizeDelta = new Vector2(rectChildren[ii].sizeDelta.x, height);
            rectChildren[ii].anchoredPosition = new Vector2(currentX + holeWidth / 2, currentY);
            currentX += rectChildren[ii].rect.width + margin.x;
        }
    }



    public override void CalculateLayoutInputVertical()
    {
        PlaceChildren();
    }

    public override void SetLayoutHorizontal()
    {
        PlaceChildren();
    }

    public override void SetLayoutVertical()
    {
        PlaceChildren();
    }
}
