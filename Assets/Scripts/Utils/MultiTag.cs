using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTag : MonoBehaviour
{
    public enum Tag
    {
        Floor,
        Player,
        Obstacle,
        Enemy
    }

    [SerializeField]
    private List<Tag> myTags = new List<Tag>();

    public void AddTag(Tag tag)
    {
        if (!CheckTag(tag)) myTags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        if (CheckTag(tag)) myTags.Remove(tag);
    }

    public bool CheckTag(Tag tag)
    {
        return myTags.Contains(tag);
    }

    public bool CheckMultipleTags(List<Tag> tags)
    {
        foreach (Tag t in tags)
        {
            if (myTags.Contains(t)) return true;
        }

        return false;
    }

}
