using System.IO;
using UnityEngine;
using Valve.VR;


public class FingerTracking : MonoBehaviour
{

    [SerializeField]
    private SteamVR_Action_Skeleton SkeletonActionLeft;
    [SerializeField]
    private SteamVR_Action_Skeleton SkeletonActionRight;
    public float[] data = new float[40];
    public bool isTracked = false;

    int[] FingerNameStretched_SteamVR =
    {
        SteamVR_Skeleton_JointIndexes.thumbProximal,
        SteamVR_Skeleton_JointIndexes.thumbMiddle,
        SteamVR_Skeleton_JointIndexes.thumbDistal,
        SteamVR_Skeleton_JointIndexes.indexProximal,
        SteamVR_Skeleton_JointIndexes.indexMiddle,
        SteamVR_Skeleton_JointIndexes.indexDistal,
        SteamVR_Skeleton_JointIndexes.middleProximal,
        SteamVR_Skeleton_JointIndexes.middleMiddle,
        SteamVR_Skeleton_JointIndexes.middleDistal,
        SteamVR_Skeleton_JointIndexes.ringProximal,
        SteamVR_Skeleton_JointIndexes.ringMiddle,
        SteamVR_Skeleton_JointIndexes.ringDistal,
        SteamVR_Skeleton_JointIndexes.pinkyProximal,
        SteamVR_Skeleton_JointIndexes.pinkyMiddle,
        SteamVR_Skeleton_JointIndexes.pinkyDistal,
    };
    int[] FingerNameSpread_SteamVR =
    {
        SteamVR_Skeleton_JointIndexes.thumbMiddle,
        SteamVR_Skeleton_JointIndexes.indexProximal,
        SteamVR_Skeleton_JointIndexes.middleProximal,
        SteamVR_Skeleton_JointIndexes.ringProximal,
        SteamVR_Skeleton_JointIndexes.pinkyProximal,
    };
    int[] UnityMuscleIndexStretched =
        {
        55, //Left Thumb 1 Stretched
        57, //Left Thumb 2 Stretched
        58, //Left Thumb 3 Stretched
        59, //Left Index 1 Stretched
        61, //Left Index 2 Stretched
        62, //Left Index 3 Stretched
        63, //Left Middle 1 Stretched
        65, //Left Middle 2 Stretched
        66, //Left Middle 3 Stretched
        67, //Left Ring 1 Stretched
        69, //Left Ring 2 Stretched
        70, //Left Ring 3 Stretched
        71, //Left Little 1 Stretched
        73, //Left Little 2 Stretched
        74, //Left Little 3 Stretched
        75, //Right Thumb 1 Stretched
        77, //Right Thumb 2 Stretched
        78, //Right Thumb 3 Stretched
        79, //Right Index 1 Stretched
        81, //Right Index 2 Stretched
        82, //Right Index 3 Stretched
        83, //Right Middle 1 Stretched
        85, //Right Middle 2 Stretched
        86, //Right Middle 3 Stretched
        87, //Right Ring 1 Stretched
        89, //Right Ring 2 Stretched
        90, //Right Ring 3 Stretched
        91, //Right Little 1 Stretched
        93, //Right Little 2 Stretched
        94, //Right Little 3 Stretched
    };
    int[] UnityMuscleIndexSpread =
        {
        56, //Left Thumb Spread
        60, //Left Index Spread
        64, //Left Middle Spread
        68, //Left Ring Spread
        72, //Left Little Spread
        76, //Right Thumb Spread
        80, //Right Index Spread
        84, //Right Middle Spread
        88, //Right Ring Spread
        92, //Right Little Spread
    };
    int[] UnityMuscleDegreeStretched =
        {
        110, //Left Thumb 1 Stretched
        20, //Left Thumb 2 Stretched
        20, //Left Thumb 3 Stretched
        40, //Left Index 1 Stretched
        40, //Left Index 2 Stretched
        40, //Left Index 3 Stretched
        40, //Left Middle 1 Stretched
        40, //Left Middle 2 Stretched
        40, //Left Middle 3 Stretched
        40, //Left Ring 1 Stretched
        40, //Left Ring 2 Stretched
        40, //Left Ring 3 Stretched
        40, //Left Little 1 Stretched
        40, //Left Little 2 Stretched
        40, //Left Little 3 Stretched
        -70, //Right Thumb 1 Stretched
        20, //Right Thumb 2 Stretched
        20, //Right Thumb 3 Stretched
        40, //Right Index 1 Stretched
        40, //Right Index 2 Stretched
        40, //Right Index 3 Stretched
        40, //Right Middle 1 Stretched
        40, //Right Middle 2 Stretched
        40, //Right Middle 3 Stretched
        40, //Right Ring 1 Stretched
        40, //Right Ring 2 Stretched
        40, //Right Ring 3 Stretched
        40, //Right Little 1 Stretched
        40, //Right Little 2 Stretched
        40, //Right Little 3 Stretched
    };
    int[] UnityMuscleDegreeSpread =
        {
        0, //Left Thumb Spread
        0, //Left Index Spread
        0, //Left Middle Spread
        -10, //Left Ring Spread
        0, //Left Little Spread
        0, //Right Thumb Spread
        0, //Right Index Spread
        0, //Right Middle Spread
        -10, //Right Ring Spread
        0, //Right Little Spread
    };
    float InverseLerpUnclamped(float a, float b, float value)
    {
        return (value - a) / (b - a);

    }
    private void Start()
    {
        using (StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/num.txt"))
        {
            for (int i = 0; i < UnityMuscleDegreeStretched.Length; i++)
            {
                UnityMuscleDegreeStretched[i] = int.Parse(sr.ReadLine());
            }
            for (int i = 0; i < UnityMuscleDegreeSpread.Length; i++)
            {
                UnityMuscleDegreeSpread[i] = int.Parse(sr.ReadLine());
            }
        }
    }
    private void FixedUpdate()
    {
        Quaternion[] boneRotationsLeft = SkeletonActionLeft.GetBoneRotations(true);
        Quaternion[] boneRotationsRight = SkeletonActionRight.GetBoneRotations(true);

        float[] eulerAnglesStretched = new float[30];
        for (int i = 0; i < 15; i++)
        {
            eulerAnglesStretched[i] = boneRotationsLeft[FingerNameStretched_SteamVR[i]].eulerAngles.z;
            eulerAnglesStretched[i + 15] = boneRotationsRight[FingerNameStretched_SteamVR[i]].eulerAngles.z;
        }
        for (int i = 0; i < 30; i++)
        {
            eulerAnglesStretched[i] += UnityMuscleDegreeStretched[i];
            if (eulerAnglesStretched[i] > 180) eulerAnglesStretched[i] -= 360;
            if (eulerAnglesStretched[i] < -180) eulerAnglesStretched[i] += 360;
            eulerAnglesStretched[i] = InverseLerpUnclamped(HumanTrait.GetMuscleDefaultMin(UnityMuscleIndexStretched[i]), HumanTrait.GetMuscleDefaultMax(UnityMuscleIndexStretched[i]), eulerAnglesStretched[i]);
            data[i] = Mathf.InverseLerp(-2, 2, eulerAnglesStretched[i] * 2 - 1);
        }

        float[] eulerAnglesSpread = new float[10];
        eulerAnglesSpread[0] = boneRotationsLeft[FingerNameSpread_SteamVR[0]].eulerAngles.y;
        eulerAnglesSpread[5] = boneRotationsRight[FingerNameSpread_SteamVR[0]].eulerAngles.y;
        for (int i = 1; i < 3; i++)
        {
            eulerAnglesSpread[i] = -boneRotationsLeft[FingerNameSpread_SteamVR[i]].eulerAngles.y;
            eulerAnglesSpread[i + 5] = -boneRotationsRight[FingerNameSpread_SteamVR[i]].eulerAngles.y;
        }
        for (int i = 3; i < 5; i++)
        {
            eulerAnglesSpread[i] = boneRotationsLeft[FingerNameSpread_SteamVR[i]].eulerAngles.y;
            eulerAnglesSpread[i + 5] = boneRotationsRight[FingerNameSpread_SteamVR[i]].eulerAngles.y;
        }
        for (int i = 0; i < 10; i++)
        {
            eulerAnglesSpread[i] += UnityMuscleDegreeSpread[i];
            if (eulerAnglesSpread[i] > 180) eulerAnglesSpread[i] -= 360;
            if (eulerAnglesSpread[i] < -180) eulerAnglesSpread[i] += 360;
            eulerAnglesSpread[i] = InverseLerpUnclamped(HumanTrait.GetMuscleDefaultMin(UnityMuscleIndexSpread[i]), HumanTrait.GetMuscleDefaultMax(UnityMuscleIndexSpread[i]), eulerAnglesSpread[i]);
            data[i + 30] = Mathf.InverseLerp(-2, 2, eulerAnglesSpread[i] * 2 - 1);
        }
        Debug.Log(boneRotationsLeft[FingerNameStretched_SteamVR[0]].eulerAngles);
    }

}
