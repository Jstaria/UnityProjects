using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DungeonSections : ScriptableObject
{
    public Section GetSection(string name)
    {
        if (sectionsDict == null)
            CreateDict();

        return sectionsDict[name];
    }

    [SerializeField] private List<Section> sections;
    private Dictionary<string, Section> sectionsDict;

    private void CreateDict()
    {
        sectionsDict = new Dictionary<string, Section>();

        foreach (Section section in sections)
        {
            sectionsDict.Add(section.SectionName, section);    
        }
    }

}

public class Section
{
    public GameObject SectionPrefab;
    public string SectionName;
}
