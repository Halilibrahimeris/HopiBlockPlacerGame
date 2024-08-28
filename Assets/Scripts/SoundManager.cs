using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance;
   public enum BlockSoundTypes {Click};

   [Header("UI")]
        public AudioSource blockSound;

 public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

 public void PlayMainSound(BlockSoundTypes currentBlockSound)
        {
            switch (currentBlockSound)
            {
                case BlockSoundTypes.Click:
                    blockSound.Play();
                    break;
            }

            //  cagirmak istendiginde = SoundManager.Instance.PlayMainSound(SoundManager.BlockSoundTypes.Click);
        }    

} 
