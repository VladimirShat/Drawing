using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    [Header("Brush")]
    public GameObject brushPrefab;
    public float brushSize = 0.5f;

    [Header("Draw Texture On Object")]
    public GameObject picture;
    public Material pictureMaterial;

    [Space(20)]
    public RenderTexture RTexture;
    //public bool saveTextureAsPNG;
    //public string fileName;
    
    private bool brushIsAvailable;
    private GameObject brush;

    private void Start()
    {
        Texture2D texture = Texture2DManager.GenerateColorTexture2D(RTexture.width, RTexture.height, Color.black);

        pictureMaterial.mainTexture = texture;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (!brushIsAvailable)
                {
                    brush = Instantiate(brushPrefab, hit.point, Quaternion.identity, transform);
                    brush.transform.localScale = Vector3.one * brushSize;
                    brushIsAvailable = true;
                }
                else
                {
                    brush.transform.position = hit.point;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //use lerp for smoothly brush move
                brush.transform.position = Vector3.Lerp(brush.transform.position, hit.point, Time.deltaTime / 0.15f);
            }
        }

        //hide the brush object if the mouse button is not pressed
        if (brushIsAvailable)
        {
            if (Input.GetMouseButtonUp(0))
            {
                brush.SetActive(false);
                brushIsAvailable = false;
            }
        }
    }   

    public void Save()
    {
        StartCoroutine(CoroutineSave()); 
    }

    private IEnumerator CoroutineSave()
    {
        yield return new WaitForEndOfFrame();

        RenderTexture.active = RTexture;

        var texture2D = new Texture2D(RTexture.width, RTexture.height, TextureFormat.ARGB32, false);
        texture2D.ReadPixels(new Rect(0, 0, RTexture.width, RTexture.height), 0, 0);
        texture2D.Apply();

        pictureMaterial.mainTexture = texture2D;

        //сохранение текстуры
        //if(saveTextureAsPNG)
        //{
        //Texture2DManager.SaveTextureAsPNG(texture2D, fileName);
        //}
    }
}
