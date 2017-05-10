using UnityEngine;
using System.Collections;

public class PlayAudio : MonoBehaviour {

    private AudioSource audios;
    bool isPlay;
	// Use this for initialization
	void Start () {
        audios = this.GetComponent<AudioSource>();
    }

    //判断是否播放与暂停
    public void Select()
    {
        //isPlay = !isPlay;
        if (audios.isPlaying == false)
            PlayAudios();
        else
            PauseAudios();
    }
	//播放音乐
    public void PlayAudios()
    {
        if(audios.clip != null)
        {
            audios.Play();
            if (audios.isPlaying == false)
            {
                audios.Stop();
                audios.Play();
            }
               
        }
        
    }
    //暂停播放音乐
    public void PauseAudios()
    {
        if (audios.clip != null)
            audios.Pause();
    }

    public void PlayDuan()
    {
        audios.Stop();
        audios.Play();
    }
}
