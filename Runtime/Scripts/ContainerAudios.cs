using UnityEngine;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(AudioSource),typeof(Container))]
    public class ContainerAudios : MonoBehaviour
    {
        private Container container;
        [SerializeField] private AudioClip[] getAudioClips;
        [SerializeField] private AudioClip[] dropAudioClips;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            container = GetComponent<Container>();
        }

        private void OnEnable()
        {
            container.OnLocalItemAdd += ItemGet;
            container.OnLocalItemRemove += ItemDrop;
        }

        private void OnDisable()
        {
            container.OnLocalItemAdd -= ItemGet;
            container.OnLocalItemRemove -= ItemGet;
        }

        private void ItemGet(Item item,byte amount)
        {
            if(getAudioClips.Length > 0)
            {
                AudioClip clip = getAudioClips[Random.Range(0,getAudioClips.Length)];
                if(clip) audioSource.PlayOneShot(clip);
            }
            
        }

        private void ItemDrop(Item item, byte amount)
        {
            if(dropAudioClips.Length > 0)
            {
                AudioClip clip = dropAudioClips[Random.Range(0,dropAudioClips.Length)];
                if(clip) audioSource.PlayOneShot(clip);
            }
        }
    }
}

