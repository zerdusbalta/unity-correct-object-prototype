using UnityEngine;

public class DropZone : MonoBehaviour
{
    public string acceptedType = "Food";

    public bool IsCorrect(DraggableItem item)
    {
        if (item == null)
            return false;

        return item.HasType(acceptedType);
    }

    public Vector3 GetSnapPosition()
    {
        return transform.position;
    }

    public bool IsMoreThanHalfInside(Collider2D itemCollider)
    {
        Collider2D zoneCollider = GetComponent<Collider2D>();

        Bounds itemBounds = itemCollider.bounds;
        Bounds zoneBounds = zoneCollider.bounds;

        float overlapX = Mathf.Min(itemBounds.max.x, zoneBounds.max.x) - Mathf.Max(itemBounds.min.x, zoneBounds.min.x);
        float overlapY = Mathf.Min(itemBounds.max.y, zoneBounds.max.y) - Mathf.Max(itemBounds.min.y, zoneBounds.min.y);

        if (overlapX <= 0 || overlapY <= 0)
            return false;

        float overlapArea = overlapX * overlapY;
        float itemArea = itemBounds.size.x * itemBounds.size.y;

        return overlapArea >= itemArea * 0.5f;
    }
}