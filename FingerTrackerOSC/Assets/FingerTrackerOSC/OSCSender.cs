using UnityEngine;
using OscCore;
using System.IO;
using System;
using System.Threading;

public class OSCSender : MonoBehaviour
{
    [SerializeField]
    private FingerTracking data;
    OscClient client;
    int c = 0;
    int mode = 0;
    void Start()
    {
        String ip = "127.0.0.1";
        int port = 9000;
        using (StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/ip.txt"))
		{
            ip = sr.ReadLine();
            port = int.Parse(sr.ReadLine());
            mode = int.Parse(sr.ReadLine());
        }
        client = new OscClient(ip, port);
    }
     private void Update()
    {
        Thread.Sleep(16);
    }
    void FixedUpdate()
    {
        //Economy:Kakukaku (1fps from others). Time divide 320bit into 80bit
        if (mode == 1)
        {
            c++;
            client.Send("/avatar/parameters/FingerFlag", 0);
            switch (c % 4)
            {
                case 0:
                    client.Send("/avatar/parameters/Finger0", data.data[0]);
                    client.Send("/avatar/parameters/Finger1", data.data[1]);
                    client.Send("/avatar/parameters/Finger2", data.data[2]);
                    client.Send("/avatar/parameters/Finger3", data.data[30]);
                    client.Send("/avatar/parameters/Finger4", data.data[3]);
                    client.Send("/avatar/parameters/Finger5", data.data[4]);
                    client.Send("/avatar/parameters/Finger6", data.data[5]);
                    client.Send("/avatar/parameters/Finger7", data.data[31]);
                    client.Send("/avatar/parameters/Finger8", data.data[6]);
                    client.Send("/avatar/parameters/Finger9", data.data[7]);
                    break;
                case 1:
                    client.Send("/avatar/parameters/Finger0", data.data[8]);
                    client.Send("/avatar/parameters/Finger1", data.data[32]);
                    client.Send("/avatar/parameters/Finger2", data.data[9]);
                    client.Send("/avatar/parameters/Finger3", data.data[10]);
                    client.Send("/avatar/parameters/Finger4", data.data[11]);
                    client.Send("/avatar/parameters/Finger5", data.data[33]);
                    client.Send("/avatar/parameters/Finger6", data.data[12]);
                    client.Send("/avatar/parameters/Finger7", data.data[13]);
                    client.Send("/avatar/parameters/Finger8", data.data[14]);
                    client.Send("/avatar/parameters/Finger9", data.data[34]);
                    break;
                case 2:
                    client.Send("/avatar/parameters/Finger0", data.data[15]);
                    client.Send("/avatar/parameters/Finger1", data.data[16]);
                    client.Send("/avatar/parameters/Finger2", data.data[17]);
                    client.Send("/avatar/parameters/Finger3", data.data[35]);
                    client.Send("/avatar/parameters/Finger4", data.data[18]);
                    client.Send("/avatar/parameters/Finger5", data.data[19]);
                    client.Send("/avatar/parameters/Finger6", data.data[20]);
                    client.Send("/avatar/parameters/Finger7", data.data[36]);
                    client.Send("/avatar/parameters/Finger8", data.data[21]);
                    client.Send("/avatar/parameters/Finger9", data.data[22]);
                    break;
                case 3:
                    client.Send("/avatar/parameters/Finger0", data.data[23]);
                    client.Send("/avatar/parameters/Finger1", data.data[37]);
                    client.Send("/avatar/parameters/Finger2", data.data[24]);
                    client.Send("/avatar/parameters/Finger3", data.data[25]);
                    client.Send("/avatar/parameters/Finger4", data.data[26]);
                    client.Send("/avatar/parameters/Finger5", data.data[38]);
                    client.Send("/avatar/parameters/Finger6", data.data[27]);
                    client.Send("/avatar/parameters/Finger7", data.data[28]);
                    client.Send("/avatar/parameters/Finger8", data.data[29]);
                    client.Send("/avatar/parameters/Finger9", data.data[39]);
                    break;
            }
            client.Send("/avatar/parameters/FingerFlag", c % 4 + 1);
        }
        //FullData:Moves smoothly. Cut off one finger for 320bit to VRCMax(248bit)
        else
        {
            String[] path ={
               "/avatar/parameters/LHThumb1",
               "/avatar/parameters/LHThumb2",
               "/avatar/parameters/LHThumb3",
               "/avatar/parameters/LHIndex1",
               "/avatar/parameters/LHIndex2",
               "/avatar/parameters/LHIndex3",
               "/avatar/parameters/LHMiddle1",
               "/avatar/parameters/LHMiddle2",
               "/avatar/parameters/LHMiddle3",
               "/avatar/parameters/LHRing1",
               "/avatar/parameters/LHRing2",
               "/avatar/parameters/LHRing3",
               "/avatar/parameters/LHLittle1",
               "/avatar/parameters/LHLittle2",
               "/avatar/parameters/LHLittle3",
               "/avatar/parameters/RHThumb1",
               "/avatar/parameters/RHThumb2",
               "/avatar/parameters/RHThumb3",
               "/avatar/parameters/RHIndex1",
               "/avatar/parameters/RHIndex2",
               "/avatar/parameters/RHIndex3",
               "/avatar/parameters/RHMiddle1",
               "/avatar/parameters/RHMiddle2",
               "/avatar/parameters/RHMiddle3",
               "/avatar/parameters/RHRing1",
               "/avatar/parameters/RHRing2",
               "/avatar/parameters/RHRing3",
               "/avatar/parameters/RHLittle1",
               "/avatar/parameters/RHLittle2",
               "/avatar/parameters/RHLittle3",
               "/avatar/parameters/LHThumbS",
               "/avatar/parameters/LHIndexS",
               "/avatar/parameters/LHMiddleS",
               "/avatar/parameters/LHRingS",
               "/avatar/parameters/LHLittleS",
               "/avatar/parameters/RHThumbS",
               "/avatar/parameters/RHIndexS",
               "/avatar/parameters/RHMiddleS",
               "/avatar/parameters/RHRingS",
               "/avatar/parameters/RHLittleS",
            };
            for (int i = 0; i < 40; i++)
            {
                client.Send(path[i], data.data[i]);

            }
        }

    }
}
