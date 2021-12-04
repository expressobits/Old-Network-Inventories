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
            container.OnClientItemAdd += ItemGet;
            container.OnClientItemRemove += ItemDrop;
        }

        private void OnDisable()
        {
            container.OnClientItemAdd -= ItemGet;
            container.OnClientItemRemove -= ItemGet;
        }

        private void ItemGet(Item item,ushort amount)
        {
            if(getAudioClips.Length > 0)
            {
                AudioClip clip = getAudioClips[Random.Range(0,getAudioClips.Length)];
                if(clip) audioSource.PlayOneShot(clip);
            }
            
        }

        private void ItemDrop(Item item, ushort amount)
        {
            if(dropAudioClips.Length > 0)
            {
                AudioClip clip = dropAudioClips[Random.Range(0,dropAudioClips.Length)];
                if(clip) audioSource.PlayOneShot(clip);
            }
        }
    }
}

