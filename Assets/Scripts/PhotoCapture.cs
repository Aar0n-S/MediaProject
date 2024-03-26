using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;

    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Camera Remove")]
    [SerializeField] private GameObject cameraRemove;
    [SerializeField] private float cameraRemoveTime;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;
    
    
    private Texture2D screenCapture;
    private bool viewingPhoto;

    private void Start()
    {
        //sets variable to the screen width and height using this format
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void Update()
    {
        //0 defines if its left click
        if (Input.GetMouseButtonDown(0))
        {
            //if you are not viewing a photo you can capture one 
            if(!viewingPhoto)
            {
                StartCoroutine(CapturePhoto());
            }
            //else remove the photo
            else
            {
                RemovePhoto();
            }
        }
    }

    //used to make sure all the postprocessing is done before the screenshot is taken
    //IENumerator looks for a pause created by a yield and returns the value 
    IEnumerator CapturePhoto()
    {
        
        viewingPhoto = true;
        
        //yield makes sure the code stops until the correct value is returned
        yield return new WaitForEndOfFrame();

        //the region which will be read is the screen width and height 
        Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);

        //readPixels allows you to save the capture as a texture from the source (regionToRead). false stops it updating the bitmaps again
        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        //applies all previouse pixel changes 
        screenCapture.Apply();

        ShowPhoto();
    }

    void ShowPhoto()
    {
        //a sprite is needed to add the texture to the UI
        // create a sprite 
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);
        StartCoroutine(cameraFlashEffect());
        StartCoroutine(cameraRemoveEffect());

        fadingAnimation.Play("PhotoFade");

    }

    IEnumerator cameraFlashEffect()
    {
        cameraAudio.Play();
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }

    IEnumerator cameraRemoveEffect()
    {
        cameraRemove.SetActive(false);
        yield return new WaitForSeconds(cameraRemoveTime);
        cameraRemove.SetActive(true);
    }

    void RemovePhoto()
    {
        //set viewing photo and photo frmae to false to remove the photo from the screen
        viewingPhoto = false;
        photoFrame.SetActive(false);
        
    }
    


}
