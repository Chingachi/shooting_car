using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
public class GroundPipeline : MonoBehaviour
{
    private const float CAR_CHECK_PERIOD_IN_SECONDS = 0.5f;

    [SerializeField]
    private Transform[] _groundChunks;
    [SerializeField]
    private Vector3 _chunkSize;

    [Inject]
    private Car _car;

    private CancellationTokenSource _cts;

    private void Start()
    {
        _cts = new CancellationTokenSource();
        TrackCar(_cts.Token).Forget();
    }

    private void OnDestroy()
    {
        _cts.Dispose();
    }

    private async UniTaskVoid TrackCar(CancellationToken token)
    {
        while(!token.IsCancellationRequested)
        {
            await UniTask.WaitForSeconds(CAR_CHECK_PERIOD_IN_SECONDS, cancellationToken: token);
            SetupChunksUpToCar();
        }
    }

    private void SetupChunksUpToCar()
    {
        foreach(Transform chunk in _groundChunks)
        {
            float checkPos = chunk.position.z + _chunkSize.z;

            if(checkPos <= _car.transform.position.z)
            {
                chunk.position = new Vector3(chunk.position.x, chunk.position.y, chunk.position.z + _groundChunks.Length * _chunkSize.z);
            }
        }
    }


    private void OnValidate()
    {
        MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();

        if(children.Length == 0)
        {
            Debug.Log("Ground pipeline has no chunks in children");
            return;
        }

        _groundChunks = new Transform[children.Length];

        _chunkSize = children[0].bounds.size;

        for(int i = 0; i < children.Length; i++)
        {
            _groundChunks[i] = children[i].transform;
            children[i].transform.position = new Vector3(0, 0, i * _chunkSize.z);
        }
    }
}