using UnityEngine;
using UnityEngine.Video;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class PlayGifOnEditor : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void OnEnable()
    {
        if (videoPlayer == null) return;

        videoPlayer.isLooping = true;

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.Prepare();
    }

    private void OnPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnPrepared;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (videoPlayer != null && !videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }

            SceneView.RepaintAll();
        }
#endif
    }

    public void Preview()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop();
        videoPlayer.Play();
    }

    public void StopPreview()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(PlayGifOnEditor))]
public class VideoPreviewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayGifOnEditor vp = (PlayGifOnEditor)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Preview"))
        {
            vp.Preview();
        }

        if (GUILayout.Button("Stop"))
        {
            vp.StopPreview();
        }
    }
}
#endif