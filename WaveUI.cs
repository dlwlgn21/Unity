using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveUI : MonoBehaviour
{
    [SerializeField]
    private WaveSpawner mSpawner;

    [SerializeField]
    private Animator mWaveAnimator;

    [SerializeField]
    private Text mWaveCountdownText;

    [SerializeField]
    private Text mWaveLevelText;

    private WaveSpawner.eSpawnState mPrevSpawnState;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mSpawner != null);
        Debug.Assert(mWaveAnimator != null);
        Debug.Assert(mWaveCountdownText != null);
        Debug.Assert(mWaveLevelText != null);
    }

    // Update is called once per frame
    void Update()
    {
        switch (mSpawner.State)
        {
            case WaveSpawner.eSpawnState.COUNTING:
                updateCountingUI();
                break;
            case WaveSpawner.eSpawnState.SPAWNING:
                updateSpawningUI();
                break;
        }
        mPrevSpawnState = mSpawner.State;
    }

    private void updateCountingUI()
    {
        // For Only Call Once
        if (mPrevSpawnState != WaveSpawner.eSpawnState.COUNTING)
        {
            mWaveAnimator.SetBool("WaveIncoming", false);
            mWaveAnimator.SetBool("WaveCountdown", true);
        }
        mWaveCountdownText.text = ((int)mSpawner.WaveCountdown).ToString();
    }

    private void updateSpawningUI()
    {
        // For Only Call Once
        if (mPrevSpawnState != WaveSpawner.eSpawnState.SPAWNING)
        {
            mWaveAnimator.SetBool("WaveIncoming", true);
            mWaveAnimator.SetBool("WaveCountdown", false);
            mWaveLevelText.text = (mSpawner.NextWaveIndx + 1).ToString();
        }

    }
}
