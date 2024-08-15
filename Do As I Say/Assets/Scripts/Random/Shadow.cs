using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shadow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(-.1f, -.1f, 1);
    [SerializeField] private Material matShadow;
    private List<GameObject> shadows;
    private List<GameObject> children;

    public void SetShadowImage(List<GameObject> children)
    {
        this.children = children;

        if (shadows != null)
        {
            for (int i = 0; i < shadows.Count; i++)
            {
                GameObject.Destroy(shadows[i]);
            }
        }

        shadows = new List<GameObject>();

        for (int i = 0; i < children.Count; i++)
        {
            GameObject obj = new GameObject(children[i].name, typeof(Image));
            obj.GetComponent<Image>().sprite = children[i].GetComponent<Image>().sprite;
            obj.GetComponent<Image>().SetNativeSize();

            shadows.Add(obj);
            
            shadows[i].transform.SetParent(transform);
            shadows[i].transform.SetAsFirstSibling();

            shadows[i].GetComponent<Image>().rectTransform.localPosition = offset;

            shadows[i].GetComponent<Image>().material = matShadow;
            shadows[i].GetComponent<Image>().material.SetTexture("_MainTex", shadows[i].GetComponent<Image>().sprite.texture);
            //shadows[i].GetComponent<Image>().sortingOrder = 2;

            //shadows[i].transform.localScale = children[i].transform.localScale;
        }
    }

    public void SetShadow(List<GameObject> children)
    {
        this.children = children;

        if (shadows != null)
        {
            for (int i = 0; i < shadows.Count; i++)
            {
                GameObject.Destroy(shadows[i]);
            }
        }
        
        shadows = new List<GameObject>();

        for (int i = 0; i < children.Count; i++)
        {
            shadows.Add(Instantiate(children[i]));
            shadows[i].transform.parent = transform;

            shadows[i].transform.localPosition = offset;

            shadows[i].GetComponent<SpriteRenderer>().material = matShadow;
            shadows[i].GetComponent<SpriteRenderer>().material.SetTexture("_MainTex", shadows[i].GetComponent<SpriteRenderer>().sprite.texture);
            shadows[i].GetComponent<SpriteRenderer>().sortingOrder = 2;

            shadows[i].transform.localScale = children[i].transform.localScale;
        }
    }

    private void Update()
    {
        UpdateShadow();
    }

    public void UpdateShadow()
    {
        //if (shadows == null) return;
        for (int i = 0; i < shadows.Count;i++)
        {
            Image image;
            SpriteRenderer sprd;

            if (shadows[i].TryGetComponent<Image>(out image))
            {
                image.rectTransform.localPosition = offset;
                image.rectTransform.localScale = children[i].GetComponent<Image>().rectTransform.localScale;
                image.rectTransform.localRotation = children[i].GetComponent<Image>().rectTransform.localRotation;
            }
            else if (shadows[i].TryGetComponent<SpriteRenderer>(out sprd))
            {
                sprd.transform.localPosition = offset;
                sprd.transform.localScale = children[i].GetComponent<SpriteRenderer>().transform.localScale;
                sprd.transform.localRotation = children[i].GetComponent<SpriteRenderer>().transform.localRotation;
            }
            
        }
    }
}
