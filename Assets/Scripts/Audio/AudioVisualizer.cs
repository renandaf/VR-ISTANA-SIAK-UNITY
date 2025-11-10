using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    private GameObject[] _visualizers;

    [SerializeField] private GameObject imagePrefab;
    public float maxScale;
    public float spectrumLength;
    private bool playAudio;

    private void OnEnable()
    {
        _visualizers = new GameObject[2048];
        for (int i = 0; i < spectrumLength; i++)
        {
            GameObject objToSpawn = (GameObject)Instantiate(imagePrefab);
            objToSpawn.transform.position = this.transform.position;
            objToSpawn.transform.SetParent(this.transform);
            objToSpawn.name = "BandVisualizer " + i;
            objToSpawn.transform.localPosition = new Vector3(i, 0, 0);
            _visualizers[i] = objToSpawn;
        }
        playAudio = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playAudio)
        {
            Color randomColor = Random.ColorHSV(1f, 1f, 1f, 1f);
            Vector4 nextColor = new Vector4(randomColor.r, randomColor.g, randomColor.b, 255);
            for (int i = 0; i < spectrumLength; i++)
            {
                _visualizers[i].transform.localScale =
                Vector3.Lerp(_visualizers[i].transform.localScale,
                new Vector3(1, (AudioMaster.Samples[i] * maxScale) + 2, 1), AudioMaster.LerpTime * Time.deltaTime);
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject obj in _visualizers)
        {
            Destroy(obj);
        }
        _visualizers = new GameObject[2048];
        playAudio = false;
    }
}
