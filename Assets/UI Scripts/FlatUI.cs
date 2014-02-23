using UnityEngine;
using System.Collections;

public class FlatUI : MonoBehaviour {
	
	public Texture2D backgroundTexture;
	public Texture2D cloud1Texture;
	public Texture2D cloud2Texture;
	public Texture2D cloud3Texture;
	public Texture2D cloud4Texture;
	public Texture2D resumeTexture;
	public Texture2D quitTexture;
	public Texture2D cursorTexture;
	public Texture2D borderTexture;
	
	bool paused;
	enum CursorPosition { resume, quit };
	CursorPosition cursorPosition;
	
	Vector2 resumePosition;
	Vector2 quitPosition;
	
	public GUIStyle backgroundStyle;
	public GUIStyle cloud1Style;
	public GUIStyle cloud2Style;
	public GUIStyle cloud3Style;
	public GUIStyle cloud4Style;
	public GUIStyle resumeStyle;
	public GUIStyle quitStyle;
	public GUIStyle cursorStyle;
	public GUIStyle borderStyle;

    enum Fading { fadeIn, fadeOut, noFade};
    Fading fading;

    float fadeRate;
    float UIOpacity;

    float prevTime;
	
	// Use this for initialization
	void Start ()
	{
		paused = false;
		cursorPosition = CursorPosition.resume;
		
		resumePosition = new Vector2(Screen.width * 3 / 8, Screen.height * 13 / 32);
		quitPosition = new Vector2(Screen.width * 3/ 8, Screen.height * 39 / 64);

        fading = Fading.noFade;
        fadeRate = 2.0f; // alpha per second
        UIOpacity = 0.0f;
        prevTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			paused = !paused;
			if(paused)
			{
                pause();
			} else {
                resume();
			}
		}
		
		if(paused)
		{
			if(Input.GetKeyDown(KeyCode.W))
			{
				//	Move cursor up
				switch(cursorPosition)
				{
				case CursorPosition.resume:
					cursorPosition = CursorPosition.quit;
					break;
				case CursorPosition.quit:
					cursorPosition = CursorPosition.resume;
					break;
				}
			}
			if(Input.GetKeyDown(KeyCode.S))
			{
				//	Move cursor down
				switch(cursorPosition)
				{
				case CursorPosition.resume:
					cursorPosition = CursorPosition.quit;
					break;
				case CursorPosition.quit:
					cursorPosition = CursorPosition.resume;
					break;
				}
			}
            if (Input.GetKeyDown(KeyCode.Return))
            {
                switch (cursorPosition)
                {
                case CursorPosition.resume:
                    resume();
                    break;
                case CursorPosition.quit:
                    //  This doesn't work in the editor
                    Application.Quit();
                    break;
                }

            }
        }

        switch (fading)
        {
            case Fading.fadeIn:
                UIOpacity += fadeRate * (Time.realtimeSinceStartup - prevTime);
                if (UIOpacity >= 1.0f)
                {
                    UIOpacity = 1.0f;
                    fading = Fading.noFade;
                }
                break;
            case Fading.fadeOut:
                UIOpacity -= fadeRate * (Time.realtimeSinceStartup - prevTime);
                if (UIOpacity <= 0.0f)
                {
                    UIOpacity = 0.0f;
                    fading = Fading.noFade;
                }
                break;
        }
        prevTime = Time.realtimeSinceStartup;
	}

    void pause()
    {
        paused = true;
        Time.timeScale = 0;
        fading = Fading.fadeIn;
        if (UIOpacity == 1.0f)
        {
            UIOpacity = 0.0f;
        }
        cursorPosition = CursorPosition.resume;
    }

    void resume()
    {
        paused = false;
        Time.timeScale = 1;
        fading = Fading.fadeOut;
        if (UIOpacity == 0.0f)
        {
            UIOpacity = 1.0f;
        }
    }
	
	void OnGUI()
	{
		if(UIOpacity > 0.0f)
		{
			int sw = Screen.width;
			int sh = Screen.height;

            GUI.color = new Color(1.0f, 1.0f, 1.0f, UIOpacity);
			
            GUI.DrawTexture(new Rect(0, 0, sw, sh), backgroundTexture);
			
			GUI.DrawTexture(new Rect(0, 0, sw/2, sh/2), cloud1Texture, ScaleMode.ScaleToFit, true);
			GUI.DrawTexture(new Rect(sw/2, 0, sw/2, sh/2), cloud2Texture, ScaleMode.ScaleToFit, true);
            GUI.DrawTexture(new Rect(0, sh / 2, sw / 2, sh / 2), cloud3Texture, ScaleMode.ScaleToFit, true);
			GUI.DrawTexture(new Rect(sw/2, sh/2, sw/2, sh/2), cloud4Texture, ScaleMode.ScaleToFit, true);

            GUI.DrawTexture(new Rect(0, sh * 7 / 8, sw, sh / 8), borderTexture);
			
			//GUIUtility.RotateAroundPivot(180, new Vector2(sw / 2, sh / 2));
            GUIUtility.ScaleAroundPivot(new Vector2(-1.0f, -1.0f), new Vector2(sw / 2, sh / 2));

            GUI.DrawTexture(new Rect(0, sh * 7 / 8, sw, sh / 8), borderTexture);

            //GUIUtility.RotateAroundPivot(180, new Vector2(sw / 2, sh / 2));
            GUIUtility.ScaleAroundPivot(new Vector2(-1.0f, -1.0f), new Vector2(sw / 2, sh / 2));

            GUI.DrawTexture(new Rect(sw / 4, sh * 5 / 16, sw / 2, sh / 8), resumeTexture, ScaleMode.ScaleToFit, true);
            GUI.DrawTexture(new Rect(sw / 4, sh / 2, sw / 2, sh / 8), quitTexture, ScaleMode.ScaleToFit, true);
			
			switch(cursorPosition)
			{
			case CursorPosition.resume:
                GUI.DrawTexture(new Rect(resumePosition.x, resumePosition.y, sw / 4, sh / 8), cursorTexture, ScaleMode.ScaleToFit, true);
				break;
			case CursorPosition.quit:
                GUI.DrawTexture(new Rect(quitPosition.x, quitPosition.y, sw / 4, sh / 8), cursorTexture, ScaleMode.ScaleToFit, true);
				break;
			}
		}
	}
}
