using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class FingerTracking : MonoBehaviour
{
    public float[] data = new float[40];
    XRHandSubsystem xrHandSubsystem;

    XRHandJointID[] FingerNameStretched_OpenXR =
    {
        XRHandJointID.ThumbMetacarpal,
        XRHandJointID.ThumbProximal,
        XRHandJointID.ThumbDistal,
        XRHandJointID.IndexProximal,
        XRHandJointID.IndexIntermediate,
        XRHandJointID.IndexDistal,
        XRHandJointID.MiddleProximal,
        XRHandJointID.MiddleIntermediate,
        XRHandJointID.MiddleDistal,
        XRHandJointID.RingProximal,
        XRHandJointID.RingIntermediate,
        XRHandJointID.RingDistal,
        XRHandJointID.LittleProximal,
        XRHandJointID.LittleIntermediate,
        XRHandJointID.LittleDistal,
    };
    XRHandJointID[] FingerNameSpread_OpenXR =
    {
        XRHandJointID.ThumbProximal,
        XRHandJointID.IndexProximal,
        XRHandJointID.MiddleProximal,
        XRHandJointID.RingProximal,
        XRHandJointID.LittleProximal,
    };
    XRHandJointID[] FingerNameStretched_OpenXR_Oya =
    {
        XRHandJointID.Wrist,
        XRHandJointID.ThumbMetacarpal,
        XRHandJointID.ThumbProximal,
        XRHandJointID.IndexMetacarpal,
        XRHandJointID.IndexProximal,
        XRHandJointID.IndexIntermediate,
        XRHandJointID.MiddleMetacarpal,
        XRHandJointID.MiddleProximal,
        XRHandJointID.MiddleIntermediate,
        XRHandJointID.RingMetacarpal,
        XRHandJointID.RingProximal,
        XRHandJointID.RingIntermediate,
        XRHandJointID.LittleMetacarpal,
        XRHandJointID.LittleProximal,
        XRHandJointID.LittleIntermediate,
    };
    XRHandJointID[] FingerNameSpread_OpenXR_Oya =
    {
        XRHandJointID.Wrist,
        XRHandJointID.IndexMetacarpal,
        XRHandJointID.MiddleMetacarpal,
        XRHandJointID.RingMetacarpal,
        XRHandJointID.LittleMetacarpal,
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
    public void StartXR()
    {
        StartCoroutine(StartXRCoroutine());
    }
    void StopXR()
    {
        xrHandSubsystem.Stop();
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }
    public IEnumerator StartXRCoroutine()
    {
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
        var activeLoader = XRGeneralSettings.Instance.Manager.activeLoader;
        if (activeLoader == null)
        {
            Debug.LogError("activeLoader ERROR");
        }
        else
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            var xrDisplay = activeLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
            xrDisplay.Stop();
            xrHandSubsystem = activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
            if (xrHandSubsystem == null)
            {
                Debug.LogError("xrHandSubsystem ERROR");
            }
            else
            {
                xrHandSubsystem.Start();
            }
        }
    }
    private void Start()
    {
        //StartXR();
        xrHandSubsystem = XRGeneralSettings.Instance?.Manager?.activeLoader?.GetLoadedSubsystem<XRHandSubsystem>();

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
	private void OnDestroy()
	{
        //StopXR();
    }
	void FixedUpdate()
    {
        if (xrHandSubsystem == null) return;
        Quaternion[] boneRotations = new Quaternion[40];
        for (int i = 0; i < 15; i++)
        {
            Pose pose;
            Pose poseoya;
            xrHandSubsystem.leftHand.GetJoint(FingerNameStretched_OpenXR[i]).TryGetPose(out pose);
            xrHandSubsystem.leftHand.GetJoint(FingerNameStretched_OpenXR_Oya[i]).TryGetPose(out poseoya);
            boneRotations[i] = Quaternion.Inverse(poseoya.rotation) * pose.rotation;
            xrHandSubsystem.rightHand.GetJoint(FingerNameStretched_OpenXR[i]).TryGetPose(out pose);
            xrHandSubsystem.rightHand.GetJoint(FingerNameStretched_OpenXR_Oya[i]).TryGetPose(out poseoya);
            boneRotations[i + 15] = Quaternion.Inverse(poseoya.rotation) * pose.rotation;
        }
        for (int i = 0; i < 5; i++)
        {
            Pose pose;
            Pose poseoya;
            xrHandSubsystem.leftHand.GetJoint(FingerNameSpread_OpenXR[i]).TryGetPose(out pose);
            xrHandSubsystem.leftHand.GetJoint(FingerNameSpread_OpenXR_Oya[i]).TryGetPose(out poseoya);
            boneRotations[i + 30] = Quaternion.Inverse(poseoya.rotation) * pose.rotation;
            xrHandSubsystem.rightHand.GetJoint(FingerNameSpread_OpenXR[i]).TryGetPose(out pose);
            xrHandSubsystem.rightHand.GetJoint(FingerNameSpread_OpenXR_Oya[i]).TryGetPose(out poseoya);
            boneRotations[i + 35] = Quaternion.Inverse(poseoya.rotation) * pose.rotation;
        }

        float[] eulerAnglesStretched = new float[30];
        for (int i = 0; i < 30; i++)
        {
            eulerAnglesStretched[i] = -boneRotations[i].eulerAngles.x;
            eulerAnglesStretched[i] += UnityMuscleDegreeStretched[i];
            if (eulerAnglesStretched[i] > 180) eulerAnglesStretched[i] -= 360;
            if (eulerAnglesStretched[i] < -180) eulerAnglesStretched[i] += 360;
            eulerAnglesStretched[i] = InverseLerpUnclamped(HumanTrait.GetMuscleDefaultMin(UnityMuscleIndexStretched[i]), HumanTrait.GetMuscleDefaultMax(UnityMuscleIndexStretched[i]), eulerAnglesStretched[i]);
            data[i] = Mathf.InverseLerp(-2, 2, eulerAnglesStretched[i] * 2 - 1);
        }

        float[] eulerAnglesSpread = new float[10];
        eulerAnglesSpread[0] = -boneRotations[30].eulerAngles.y;
        eulerAnglesSpread[1] = boneRotations[31].eulerAngles.y;
        eulerAnglesSpread[2] = boneRotations[32].eulerAngles.y;
        eulerAnglesSpread[3] = -boneRotations[33].eulerAngles.y;
        eulerAnglesSpread[4] = -boneRotations[34].eulerAngles.y;
        eulerAnglesSpread[5] = boneRotations[35].eulerAngles.y;
        eulerAnglesSpread[6] = -boneRotations[36].eulerAngles.y;
        eulerAnglesSpread[7] = -boneRotations[37].eulerAngles.y;
        eulerAnglesSpread[8] = boneRotations[38].eulerAngles.y;
        eulerAnglesSpread[9] = boneRotations[39].eulerAngles.y;
        for (int i = 0; i < 10; i++)
        {
            eulerAnglesSpread[i] += UnityMuscleDegreeSpread[i];
            if (eulerAnglesSpread[i] > 180) eulerAnglesSpread[i] -= 360;
            if (eulerAnglesSpread[i] < -180) eulerAnglesSpread[i] += 360;
            eulerAnglesSpread[i] = InverseLerpUnclamped(HumanTrait.GetMuscleDefaultMin(UnityMuscleIndexSpread[i]), HumanTrait.GetMuscleDefaultMax(UnityMuscleIndexSpread[i]), eulerAnglesSpread[i]);
            data[i + 30] = Mathf.InverseLerp(-2, 2, eulerAnglesSpread[i] * 2 - 1);
        }
        Debug.Log(boneRotations[0].eulerAngles);
    }
}
