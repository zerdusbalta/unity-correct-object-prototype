using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    public string itemType;

    public bool HasType(string type)
    {
        if (string.IsNullOrWhiteSpace(itemType) || string.IsNullOrWhiteSpace(type))
            return false;

        string[] types = itemType.Split(',');

        foreach (string t in types)
        {
            if (t.Trim().ToLower() == type.Trim().ToLower())
            {
                return true;
            }
        }

        return false;
    }
}