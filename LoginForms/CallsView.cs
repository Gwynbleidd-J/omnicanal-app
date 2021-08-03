﻿using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using PortSIP;
using System.Text;
using System.Media;

#region Librerias Ozeki
/*
using Ozeki.VoIP;
using Ozeki.Media;
using Ozeki.Network;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using LoginForms.Shared;
using System.IO;
*/
#endregion


namespace LoginForms
{
    public partial class CallsView : Form, SIPCallbackEvents
    {

        //En el método onInviteRinging se pondría un tono de llamada si así lo quisieran
        private const int MAX_LINES = 9; // Maximum lines
        private const int LINE_BASE = 1;

        private Session[] _CallSessions = new Session[MAX_LINES];

        private bool sIPInited = false;
        private bool sIPLogined = false;
        private int currentlyLine = LINE_BASE;

        private PortSIPLib portSIPLib;
        //Para poner un tono de llamada cuando llamen al softphone
        //SoundPlayer soundPlayer = new SoundPlayer(@"C:\Users\Gwynbleidd J\Downloads\trip-innocent.wav");

        //private videoScreen _fmVideoScreen;

        private int findSession(int sessionId)
        {
            int index = -1;
            for (int i = LINE_BASE; i < MAX_LINES; ++i)
            {
                if (_CallSessions[i].getSessionId() == sessionId)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private byte[] GetBytes(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }

        private string GetString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        private string getLocalIP()
        {
            StringBuilder localIP = new StringBuilder();
            localIP.Length = 64;
            int nics = portSIPLib.getNICNums();
            for (int i = 0; i < nics; ++i)
            {
                portSIPLib.getLocalIpAddress(i, localIP, 64);
                if (localIP.ToString().IndexOf(":") == -1)
                {
                    // No ":" in the IP then it's the IPv4 address, we use it in our sample
                    break;
                }
                else
                {
                    // the ":" is occurs in the IP then this is the IPv6 address.
                    // In our sample we don't use the IPv6.
                }

            }

            return localIP.ToString();
        }

        #region Metodo PRACK comentado
        //private void updatePrackSetting()
        //{
        //    if (!sIPInited)
        //    {
        //        return;
        //    }

        //    portSIPLib.enableReliableProvisional(checkBoxPRACK.Checked);
        //}

        //private void joinConference(Int32 index)
        //{
        //    if (sIPInited == false)
        //    {
        //        return;
        //    }
        //    if (CheckBoxConf.Checked == false)
        //    {
        //        return;
        //    }
        //    portSIPLib.setRemoteVideoWindow(_CallSessions[index].getSessionId(), IntPtr.Zero);
        //    portSIPLib.joinToConference(_CallSessions[index].getSessionId());

        //    // We need to un-hold the line
        //    if (_CallSessions[index].getHoldState())
        //    {
        //        portSIPLib.unHold(_CallSessions[index].getSessionId());
        //        _CallSessions[index].setHoldState(false);
        //    }
        //}

        //private void initScreenSharing()
        //{
        //    StringBuilder deviceName = new StringBuilder();
        //    deviceName.Length = 1024;

        //    int nums = portSIPLib.getScreenSourceCount();
        //    for (int i = 0; i < nums; ++i)
        //    {
        //        portSIPLib.getScreenSourceTitle(i, deviceName, 1024);
        //        ComboboxScreenLst.Items.Add(deviceName.ToString());
        //    }

        //    ComboboxScreenLst.SelectedIndex = 0;

        //}
        #endregion

        private void loadDevices()
        {
            if (sIPInited == false)
            {
                return;
            }

            int num = portSIPLib.getNumOfPlayoutDevices();
            for (int i = 0; i < num; ++i)
            {
                StringBuilder deviceName = new StringBuilder();
                deviceName.Length = 256;

                if (portSIPLib.getPlayoutDeviceName(i, deviceName, 256) == 0)
                {
                    ComboBoxSpeakers.Items.Add(deviceName.ToString());
                }

                ComboBoxSpeakers.SelectedIndex = 0;
            }


            num = portSIPLib.getNumOfRecordingDevices();
            for (int i = 0; i < num; ++i)
            {
                StringBuilder deviceName = new StringBuilder();
                deviceName.Length = 256;

                if (portSIPLib.getRecordingDeviceName(i, deviceName, 256) == 0)
                {
                    ComboBoxMicrophones.Items.Add(deviceName.ToString());
                }

                ComboBoxMicrophones.SelectedIndex = 0;
            }


            num = portSIPLib.getNumOfVideoCaptureDevices();
            for (int i = 0; i < num; ++i)
            {
                StringBuilder uniqueId = new StringBuilder();
                uniqueId.Length = 256;
                StringBuilder deviceName = new StringBuilder();
                deviceName.Length = 256;

                if (portSIPLib.getVideoCaptureDeviceName(i, uniqueId, 256, deviceName, 256) == 0)
                {
                    ComboBoxCameras.Items.Add(deviceName.ToString());
                }

                ComboBoxCameras.SelectedIndex = 0;
            }


            int volume = portSIPLib.getSpeakerVolume();
            TrackBarSpeaker.SetRange(0, 255);
            TrackBarSpeaker.Value = volume;

            volume = portSIPLib.getMicVolume();
            TrackBarMicrophone.SetRange(0, 255);
            TrackBarMicrophone.Value = volume;

        }

        private void InitSettings()
        {
            if (sIPInited == false)
            {
                return;
            }

            portSIPLib.enableAEC(true);
            //portSIPLib.enableAEC(checkBoxAEC.Checked);
            //portSIPLib.enableVAD(checkBoxVAD.Checked);
            //portSIPLib.enableCNG(checkBoxCNG.Checked);
            //portSIPLib.enableAGC(checkBoxAGC.Checked);
            //portSIPLib.enableANS(checkBoxANS.Checked);



            //portSIPLib.setVideoNackStatus(checkBoxNack.Checked);
            //portSIPLib.setDoNotDisturb(CheckBoxDND.Checked);
        }

        private void SetSRTPType()
        {
            if (sIPInited == false)
            {
                return;
            }

            SRTP_POLICY SRTPPolicy = SRTP_POLICY.SRTP_POLICY_NONE;

            switch (ComboBoxSRTP.SelectedIndex)
            {
                case 0:
                    SRTPPolicy = SRTP_POLICY.SRTP_POLICY_NONE;
                    break;

                case 1:
                    SRTPPolicy = SRTP_POLICY.SRTP_POLICY_PREFER;
                    break;

                case 2:
                    SRTPPolicy = SRTP_POLICY.SRTP_POLICY_FORCE;
                    break;
            }

            portSIPLib.setSrtpPolicy(SRTPPolicy, true);
        }

        #region Codecs de Video que posiblemente se ocupen
        //private void SetVideoResolution()
        //{
        //    if (sIPInited == false)
        //    {
        //        return;
        //    }

        //    Int32 width = 352;
        //    Int32 height = 288;

        //    switch (ComboBoxVideoResolution.SelectedIndex)
        //    {
        //        case 0://qcif
        //            width = 176;
        //            height = 144;
        //            break;
        //        case 1://cif
        //            width = 352;
        //            height = 288;
        //            break;
        //        case 2://VGA
        //            width = 640;
        //            height = 480;
        //            break;
        //        case 3://svga
        //            width = 800;
        //            height = 600;
        //            break;
        //        case 4://xvga
        //            width = 1024;
        //            height = 768;
        //            break;
        //        case 5://q720
        //            width = 1280;
        //            height = 720;
        //            break;
        //        case 6://qvga
        //            width = 320;
        //            height = 240;
        //            break;
        //    }

        //    portSIPLib.setVideoResolution(width, height);
        //}

        //private void SetVideoQuality()
        //{
        //    if (sIPInited == false)
        //    {
        //        return;
        //    }

        //    portSIPLib.setVideoBitrate(_CallSessions[currentlyLine].getSessionId(), TrackBarVideoQuality.Value);
        //}

        // Default we just using PCMU, PCMA, and G.279
        #endregion

        private void InitDefaultAudioCodecs()
        {
            if (sIPInited == false)
            {
                return;
            }


            portSIPLib.clearAudioCodec();

            // Default we just using PCMU, PCMA, G729
            portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);

            portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF);  // for DTMF as RTP Event - RFC2833
        }

        private void UpdateAudioCodecs()
        {
            if (sIPInited == false)
            {
                return;
            }

            portSIPLib.clearAudioCodec();

            if (checkBoxPCMU.Checked == true)
            {
                portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            }


            if (checkBoxPCMA.Checked == true)
            {
                portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            }


            if (checkBoxG729.Checked == true)
            {
                portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);
            }

            #region Codecs de Audio que quisas se usen por eso se comentan
            //if (checkBoxILBC.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_ILBC);
            //}


            //if (checkBoxGSM.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_GSM);
            //}


            //if (checkBoxAMR.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMR);
            //}

            //if (CheckBoxG722.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G722);
            //}

            //if (CheckBoxSpeex.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEX);
            //}

            //if (CheckBoxAMRwb.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB);
            //}

            //if (CheckBoxSpeexWB.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEXWB);
            //}

            //if (CheckBoxG7221.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G7221);
            //}

            //if (checkBoxOPUS.Checked == true)
            //{
            //    portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_OPUS);
            //}
            #endregion

            portSIPLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF);

        }

        #region Codecs de video que posiblemente se ocupen
        //private void UpdateVideoCodecs()
        //{
        //    if (sIPInited == false)
        //    {
        //        return;
        //    }

        //    portSIPLib.clearVideoCodec();

        //    if (checkBoxH263.Checked == true)
        //    {
        //        portSIPLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H263);
        //    }

        //    if (checkBoxH2631998.Checked == true)
        //    {
        //        portSIPLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H263_1998);
        //    }

        //    if (checkBoxH264.Checked == true)
        //    {
        //        portSIPLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_H264);
        //    }

        //    if (checkBoxVP8.Checked == true)
        //    {
        //        portSIPLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_VP8);
        //    }

        //    if (checkBoxVP9.Checked == true)
        //    {
        //        portSIPLib.addVideoCodec(VIDEOCODEC_TYPE.VIDEO_CODEC_VP9);
        //    }

        //}
        #endregion

        public CallsView()
        {
            InitializeComponent();
        }

        private void deRegisterFromServer()
        {
            if (sIPInited == false)
            {
                return;
            }

            for (int i = LINE_BASE; i < MAX_LINES; ++i)
            {
                if (_CallSessions[i].getRecvCallState() == true)
                {
                    portSIPLib.rejectCall(_CallSessions[i].getSessionId(), 486);
                }
                else if (_CallSessions[i].getSessionState() == true)
                {
                    portSIPLib.hangUp(_CallSessions[i].getSessionId());
                }

                _CallSessions[i].reset();
            }

            if (sIPLogined)
            {
                portSIPLib.unRegisterServer();
                sIPLogined = false;
            }

            portSIPLib.removeUser();

            if (sIPInited)
            {
                portSIPLib.unInitialize();
                portSIPLib.releaseCallbackHandlers();

                sIPInited = false;
            }


            ListBoxSIPLog.Items.Clear();

            ComboBoxLines.SelectedIndex = 0;
            currentlyLine = LINE_BASE;


            ComboBoxSpeakers.Items.Clear();
            ComboBoxMicrophones.Items.Clear();
            ComboBoxCameras.Items.Clear();
        }

        #region Métodos de PortSIP para el correcto funcionamiento del softphone

        public Int32 onRegisterSuccess(String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            // use the Invoke method to modify the control.
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add("Registration succeeded");
                ListBoxSIPLog.Items.Add(statusCode);
                //ListBoxSIPLog.Items.Add(sipMessage);
            }));

            sIPLogined = true;
            return 0;
        }

        public Int32 onRegisterFailure(String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add("Registration failure");
                ListBoxSIPLog.Items.Add(statusCode);
                //ListBoxSIPLog.Items.Add(sipMessage);
            }));


            sIPLogined = false;

            return 0;
        }

        public Int32 onInviteIncoming(Int32 sessionId, String callerDisplayName, String caller, String calleeDisplayName, String callee, String audioCodecNames, String videoCodecNames, Boolean existsAudio, Boolean existsVideo, StringBuilder sipMessage)
        {
            int index = -1;
            for (int i = LINE_BASE; i < MAX_LINES; ++i)
            {
                if (_CallSessions[i].getSessionState() == false && _CallSessions[i].getRecvCallState() == false)
                {
                    index = i;
                    _CallSessions[i].setRecvCallState(true);
                    break;
                }
            }

            if (index == -1)
            {
                portSIPLib.rejectCall(sessionId, 486);
                return 0;
            }

            if (existsVideo)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }
            if (existsAudio)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }

            _CallSessions[index].setSessionId(sessionId);
            string Text = string.Empty;

            bool needIgnoreAutoAnswer = false;
            int j = 0;

            for (j = LINE_BASE; j < MAX_LINES; ++j)
            {
                if (_CallSessions[j].getSessionState() == true)
                {
                    needIgnoreAutoAnswer = true;
                    break;
                }
            }

            //if (existsVideo)
            //{
            //    ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            //    {
            //        portSIPLib.setRemoteVideoWindow(_CallSessions[index].getSessionId(), remoteVideoPanel.Handle);
            //    }));
            ////}

            Boolean AA = false;
            bool answerVideo = false;
            //ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            //{
            //    AA = CheckBoxAA.Checked;
            //    answerVideo = checkBoxAnswerVideo.Checked;
            //}));

            if (needIgnoreAutoAnswer == false && AA == true)
            {
                _CallSessions[index].setRecvCallState(false);
                _CallSessions[index].setSessionState(true);


                portSIPLib.answerCall(_CallSessions[index].getSessionId(), answerVideo);

                Text = "Line " + index.ToString();
                Text += ": Answered call by Auto answer";

                ListBoxSIPLog.Invoke(new MethodInvoker(delegate
                {
                    ListBoxSIPLog.Items.Add(Text);
                }));

                return 0;
            }

            Text = "Line " + index.ToString();
            Text += ": Call incoming from ";
            Text += callerDisplayName;
            Text += "<";
            Text += caller;
            Text += ">";


            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                //cuando se reciba una llamada que suene un tono
                //soundPlayer.Play();
                ListBoxSIPLog.Items.Add(Text);
            }));

            //  You should write your own code to play the wav file here for alert the incoming call(incoming tone);

            //Para reproducir un archivo .wav como tono de llamada.
            // en los parentesis se pondria en tono de llamada cuando el softphone reciba una llamada. 

            return 0;
        }

        public Int32 onInviteTrying(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Call is trying...";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onInviteSessionProgress(Int32 sessionId, String audioCodecNames, String videoCodecNames, Boolean existsEarlyMedia, Boolean existsAudio, Boolean existsVideo, StringBuilder sipMessage)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            if (existsVideo)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }
            if (existsAudio)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }

            _CallSessions[i].setSessionState(true);

            string Text = "Line " + i.ToString();
            Text += ": Call session progress.";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            _CallSessions[i].setEarlyMeida(existsEarlyMedia);

            return 0;
        }

        public Int32 onInviteRinging(Int32 sessionId, String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            if (_CallSessions[i].hasEarlyMedia() == false)
            {
                // No early media, you must play the local WAVE  file for ringing tone
            }

            string Text = "Line " + i.ToString();
            Text += ": Ringing...";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));


            return 0;
        }

        public Int32 onInviteAnswered(Int32 sessionId, String callerDisplayName, String caller, String calleeDisplayName, String callee, String audioCodecNames, String videoCodecNames, Boolean existsAudio, Boolean existsVideo, StringBuilder sipMessage)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            if (existsVideo)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }
            if (existsAudio)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }


            _CallSessions[i].setSessionState(true);

            string Text = "Line " + i.ToString();
            Text += ": Call established";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {

                ListBoxSIPLog.Items.Add(Text);

                //joinConference(i);
            }));

            // If this is the refer call then need set it to normal
            if (_CallSessions[i].isReferCall())
            {
                _CallSessions[i].setReferCall(false, 0);
            }

            return 0;
        }

        public Int32 onInviteFailure(Int32 sessionId, String reason, Int32 code, StringBuilder sipMessage)
        {
            int index = findSession(sessionId);
            if (index == -1)
            {
                return 0;
            }

            string Text = "Line " + index.ToString();
            Text += ": call failure, ";
            Text += reason;
            Text += ", ";
            Text += code.ToString();

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));


            if (_CallSessions[index].isReferCall())
            {
                // Take off the origin call from HOLD if the refer call is failure
                int originIndex = -1;
                for (int i = LINE_BASE; i < MAX_LINES; ++i)
                {
                    // Looking for the origin call
                    if (_CallSessions[i].getSessionId() == _CallSessions[index].getOriginCallSessionId())
                    {
                        originIndex = i;
                        break;
                    }
                }

                if (originIndex != -1)
                {
                    portSIPLib.unHold(_CallSessions[index].getOriginCallSessionId());
                    _CallSessions[originIndex].setHoldState(false);

                    // Switch the currently line to origin call line
                    currentlyLine = originIndex;
                    ComboBoxLines.SelectedIndex = currentlyLine - 1;

                    Text = "Current line is set to: ";
                    Text += currentlyLine.ToString();

                    ListBoxSIPLog.Invoke(new MethodInvoker(delegate
                    {
                        ListBoxSIPLog.Items.Add(Text);
                    }));
                }
            }

            _CallSessions[index].reset();

            return 0;
        }

        public Int32 onInviteUpdated(Int32 sessionId, String audioCodecNames, String videoCodecNames, Boolean existsAudio, Boolean existsVideo, Boolean existsScreen, StringBuilder sipMessage)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            if (existsVideo)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }
            if (existsAudio)
            {
                // If more than one codecs using, then they are separated with "#",
                // for example: "g.729#GSM#AMR", "H264#H263", you have to parse them by yourself.
            }

            string Text = "Line " + i.ToString();
            Text += ": Call is updated";
            Text += existsScreen.ToString();
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            if (existsScreen && _CallSessions[currentlyLine].getExistsScreen() == false)
            {
                //demo don't process  multi screen 
                for (int nIndex = LINE_BASE; nIndex < MAX_LINES; ++nIndex)
                {
                    if (_CallSessions[nIndex].getExistsScreen() == true)
                    {
                        return 0;
                    }
                }
                // This call has Screen
                //processScreenShareStarted();
                _CallSessions[currentlyLine].setExistsScreen(true);
            }
            else if (existsScreen == false && _CallSessions[currentlyLine].getExistsScreen() == true)
            {
                _CallSessions[currentlyLine].setExistsScreen(false);
                //processScreenShareStoped();
            }
            return 0;
        }

        public Int32 onInviteConnected(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Call is connected";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                //Para detener la llamada cuando se logre la llamada
                //soundPlayer.Stop();
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onInviteBeginingForward(String forwardTo)
        {
            string Text = "An incoming call was forwarded to: ";
            Text += forwardTo;

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onInviteClosed(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            _CallSessions[i].reset();

            string Text = "Line " + i.ToString();
            Text += ": Call closed";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onDialogStateUpdated(String BLFMonitoredUri, String BLFDialogState, String BLFDialogId, String BLFDialogDirection)
        {
            string text = "The user ";
            text += BLFMonitoredUri;
            text += " dialog state is updated: ";
            text += BLFDialogState;
            text += ", dialog id: ";
            text += BLFDialogId;
            text += ", direction: ";
            text += BLFDialogDirection;

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onRemoteHold(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Placed on hold by remote.";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onRemoteUnHold(Int32 sessionId, String audioCodecNames, String videoCodecNames, Boolean existsAudio, Boolean existsVideo)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Take off hold by remote.";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onReceivedRefer(Int32 sessionId, Int32 referId, String to, String from, StringBuilder referSipMessage)
        {


            int index = findSession(sessionId);
            if (index == -1)
            {
                portSIPLib.rejectRefer(referId);
                return 0;
            }


            string Text = "Received REFER on line ";
            Text += index.ToString();
            Text += ", refer to ";
            Text += to;

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            // Accept the REFER automatically
            int referSessionId = portSIPLib.acceptRefer(referId, referSipMessage.ToString());
            if (referSessionId <= 0)
            {
                Text = "Failed to accept REFER on line ";
                Text += index.ToString();

                ListBoxSIPLog.Invoke(new MethodInvoker(delegate
                {
                    ListBoxSIPLog.Items.Add(Text);
                }));
            }
            else
            {
                portSIPLib.hangUp(_CallSessions[index].getSessionId());
                _CallSessions[index].reset();


                _CallSessions[index].setSessionId(referSessionId);
                _CallSessions[index].setSessionState(true);

                Text = "Accepted the REFER";
                ListBoxSIPLog.Invoke(new MethodInvoker(delegate
                {
                    ListBoxSIPLog.Items.Add(Text);
                }));
            }

            return 0;
        }

        public Int32 onReferAccepted(Int32 sessionId)
        {
            int index = findSession(sessionId);
            if (index == -1)
            {
                return 0;
            }

            string Text = "Line ";
            Text += index.ToString();
            Text += ", the REFER was accepted";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onReferRejected(Int32 sessionId, String reason, Int32 code)
        {
            int index = findSession(sessionId);
            if (index == -1)
            {
                return 0;
            }

            string Text = "Line ";
            Text += index.ToString();
            Text += ", the REFER was rejected";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onTransferTrying(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Transfer Trying";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));


            return 0;
        }

        public Int32 onTransferRinging(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Transfer Ringing";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onACTVTransferSuccess(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            // Close the call after succeeded transfer the call
            portSIPLib.hangUp(_CallSessions[i].getSessionId());
            _CallSessions[i].reset();

            string Text = "Line " + i.ToString();
            Text += ": Transfer succeeded, call closed.";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onACTVTransferFailure(Int32 sessionId, String reason, Int32 code)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Line " + i.ToString();
            Text += ": Transfer failure";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            //  reason is error reason
            //  code is error code

            return 0;
        }

        public Int32 onReceivedSignaling(Int32 sessionId, StringBuilder signaling)
        {
            // This event will be fired when the SDK received a SIP message
            // you can use signaling to access the SIP message.

            return 0;
        }

        public Int32 onSendingSignaling(Int32 sessionId, StringBuilder signaling)
        {
            // This event will be fired when the SDK sent a SIP message
            // you can use signaling to access the SIP message.

            return 0;
        }

        public Int32 onWaitingVoiceMessage(String messageAccount, Int32 urgentNewMessageCount, Int32 urgentOldMessageCount, Int32 newMessageCount, Int32 oldMessageCount)
        {

            string Text = messageAccount;
            Text += " has voice message.";


            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            // You can use these parameters to check the voice message count

            //  urgentNewMessageCount;
            //  urgentOldMessageCount;
            //  newMessageCount;
            //  oldMessageCount;

            return 0;
        }

        public Int32 onWaitingFaxMessage(String messageAccount, Int32 urgentNewMessageCount, Int32 urgentOldMessageCount, Int32 newMessageCount, Int32 oldMessageCount)
        {
            string Text = messageAccount;
            Text += " has FAX message.";


            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));


            // You can use these parameters to check the FAX message count

            //  urgentNewMessageCount;
            //  urgentOldMessageCount;
            //  newMessageCount;
            //  oldMessageCount;

            return 0;
        }

        public Int32 onRecvDtmfTone(Int32 sessionId, Int32 tone)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string DTMFTone = tone.ToString();
            switch (tone)
            {
                case 10:
                    DTMFTone = "*";
                    break;

                case 11:
                    DTMFTone = "#";
                    break;

                case 12:
                    DTMFTone = "A";
                    break;

                case 13:
                    DTMFTone = "B";
                    break;

                case 14:
                    DTMFTone = "C";
                    break;

                case 15:
                    DTMFTone = "D";
                    break;

                case 16:
                    DTMFTone = "FLASH";
                    break;
            }

            string Text = "Received DTMF Tone: ";
            Text += DTMFTone;
            Text += " on line ";
            Text += i.ToString();

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));


            return 0;
        }

        public Int32 onPresenceRecvSubscribe(Int32 subscribeId, String fromDisplayName, String from, String subject)
        {


            return 0;
        }

        public Int32 onPresenceOnline(String fromDisplayName, String from, String stateText)
        {

            return 0;
        }

        public Int32 onPresenceOffline(String fromDisplayName, String from)
        {


            return 0;
        }

        public Int32 onRecvOptions(StringBuilder optionsMessage)
        {
            //         string text = "Received an OPTIONS message: ";
            //       text += optionsMessage.ToString();
            //     MessageBox.Show(text, "Received an OPTIONS message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return 0;
        }

        public Int32 onRecvInfo(StringBuilder infoMessage)
        {
            string text = "Received a INFO message: ";
            text += infoMessage.ToString();

            MessageBox.Show(text, "Received a INFO message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return 0;
        }

        public Int32 onRecvNotifyOfSubscription(Int32 subscribeId, StringBuilder notifyMsg, byte[] contentData, Int32 contentLenght)
        {

            return 0;
        }

        public Int32 onSubscriptionFailure(Int32 subscribeId, Int32 statusCode)
        {
            return 0;
        }

        public Int32 onSubscriptionTerminated(Int32 subscribeId)
        {
            return 0;
        }

        public Int32 onRecvMessage(Int32 sessionId, String mimeType, String subMimeType, byte[] messageData, Int32 messageDataLength)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string text = "Received a MESSAGE message on line ";
            text += i.ToString();

            if (mimeType == "text" && subMimeType == "plain")
            {
                string mesageText = GetString(messageData);
            }
            else if (mimeType == "application" && subMimeType == "vnd.3gpp.sms")
            {
                // The messageData is binary data
            }
            else if (mimeType == "application" && subMimeType == "vnd.3gpp2.sms")
            {
                // The messageData is binary data
            }

            MessageBox.Show(text, "Received a MESSAGE message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return 0;
        }

        public Int32 onRecvOutOfDialogMessage(String fromDisplayName, String from, String toDisplayName, String to, String mimeType, String subMimeType, byte[] messageData, Int32 messageDataLength)
        {
            string text = "Received a message(out of dialog) from ";
            text += from;

            if (mimeType == "text" && subMimeType == "plain")
            {
                string mesageText = GetString(messageData);
            }
            else if (mimeType == "application" && subMimeType == "vnd.3gpp.sms")
            {
                // The messageData is binary data
            }
            else if (mimeType == "application" && subMimeType == "vnd.3gpp2.sms")
            {
                // The messageData is binary data
            }

            MessageBox.Show(text, "Received a out of dialog MESSAGE message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return 0;
        }

        public Int32 onSendMessageSuccess(Int32 sessionId, Int32 messageId)
        {
            return 0;
        }

        public Int32 onSendMessageFailure(Int32 sessionId, Int32 messageId, String reason, Int32 code)
        {

            return 0;
        }

        public Int32 onSendOutOfDialogMessageSuccess(Int32 messageId, String fromDisplayName, String from, String toDisplayName, String to)
        {


            return 0;
        }

        public Int32 onSendOutOfDialogMessageFailure(Int32 messageId, String fromDisplayName, String from, String toDisplayName, String to, String reason, Int32 code)
        {
            return 0;
        }

        public Int32 onPlayAudioFileFinished(Int32 sessionId, String fileName)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Play audio file - ";
            Text += fileName;
            Text += " end on line: ";
            Text += i.ToString();

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onPlayVideoFileFinished(Int32 sessionId)
        {
            int i = findSession(sessionId);
            if (i == -1)
            {
                return 0;
            }

            string Text = "Play video file end on line: ";
            Text += i.ToString();

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }

        public Int32 onRTPPacketCallback(IntPtr callbackObject, Int32 sessionId, Int32 mediaType, Int32 direction, byte[] RTPPacket, Int32 packetSize)
        {
            /*
                !!! IMPORTANT !!!

                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
                other code which will spend long time, you should post a message to main thread(main window) or other thread,
                let the thread to call SDK API functions or other code.

            */
            return 0;
        }

        public Int32 onAudioRawCallback(IntPtr callbackObject, Int32 sessionId, Int32 callbackType, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] data, Int32 dataLength, Int32 samplingFreqHz)
        {

            /*
                !!! IMPORTANT !!!

                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
                other code which will spend long time, you should post a message to main thread(main window) or other thread,
                let the thread to call SDK API functions or other code.

            */

            // The data parameter is audio stream as PCM format, 16bit, Mono.
            // the dataLength parameter is audio steam data length.



            //
            // IMPORTANT: the data length is stored in dataLength parameter!!!
            //

            DIRECTION_MODE type = (DIRECTION_MODE)callbackType;

            if (type == DIRECTION_MODE.DIRECTION_SEND)
            {
                // The callback data is from local record device of each session, use the sessionId to identifying the session.
            }
            else if (type == DIRECTION_MODE.DIRECTION_RECV)
            {
                // The callback data is received from remote side of each session, use the sessionId to identifying the session.
            }




            return 0;
        }

        public Int32 onVideoRawCallback(IntPtr callbackObject, Int32 sessionId, Int32 callbackType, Int32 width, Int32 height, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] byte[] data, Int32 dataLength)
        {
            /*
                !!! IMPORTANT !!!

                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
                other code which will spend long time, you should post a message to main thread(main window) or other thread,
                let the thread to call SDK API functions or other code.

                The video data format is YUV420, YV12.
            */

            //
            // IMPORTANT: the data length is stored in dataLength parameter!!!
            //

            DIRECTION_MODE type = (DIRECTION_MODE)callbackType;

            if (type == DIRECTION_MODE.DIRECTION_SEND)
            {

            }
            else if (type == DIRECTION_MODE.DIRECTION_RECV)
            {

            }


            return 0;

        }

        public Int32 onScreenRawCallback(IntPtr callbackObject, Int32 sessionId, Int32 callbackType, Int32 width, Int32 height, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] byte[] data, Int32 dataLength)
        {

            /*
                !!! IMPORTANT !!!

                Don't call any PortSIP SDK API functions in here directly. If you want to call the PortSIP API functions or 
                other code which will spend long time, you should post a message to main thread(main window) or other thread,
                let the thread to call SDK API functions or other code.

            */

            // The data parameter is audio stream as PCM format, 16bit, Mono.
            // the dataLength parameter is audio steam data length.



            //
            // IMPORTANT: the data length is stored in dataLength parameter!!!
            //
            return 0;
        }
        #endregion

        private void CallsView_Load(object sender, EventArgs e)
        {
            // Create the call sessions array, allows maximum 500 lines,
            // but we just use 8 lines with this sample, we need a class to save the call sessions information

            int i = 0;
            for (i = 0; i < MAX_LINES; ++i)
            {
                _CallSessions[i] = new Session();
                _CallSessions[i].reset();
            }

            sIPInited = false;
            sIPLogined = false;
            currentlyLine = LINE_BASE;


            TrackBarSpeaker.SetRange(0, 255);
            TrackBarSpeaker.Value = 0;

            TrackBarMicrophone.SetRange(0, 255);
            TrackBarMicrophone.Value = 0;

            //ComboBoxTransport.Items.Add("UDP");
            //ComboBoxTransport.Items.Add("TLS");
            //ComboBoxTransport.Items.Add("TCP");
            //ComboBoxTransport.Items.Add("PERS");

            //ComboBoxTransport.SelectedIndex = 0;

            ComboBoxSRTP.Items.Add("None");
            ComboBoxSRTP.Items.Add("Prefer");
            ComboBoxSRTP.Items.Add("Force");

            ComboBoxSRTP.SelectedIndex = 0;


            //ComboBoxVideoResolution.Items.Add("QCIF");
            //ComboBoxVideoResolution.Items.Add("CIF");
            //ComboBoxVideoResolution.Items.Add("VGA");
            //ComboBoxVideoResolution.Items.Add("SVGA");
            //ComboBoxVideoResolution.Items.Add("XVGA");
            //ComboBoxVideoResolution.Items.Add("720P");
            //ComboBoxVideoResolution.Items.Add("QVGA");

            //ComboBoxVideoResolution.SelectedIndex = 1;

            ComboBoxLines.Items.Add("Line-1");
            ComboBoxLines.Items.Add("Line-2");
            ComboBoxLines.Items.Add("Line-3");
            ComboBoxLines.Items.Add("Line-4");
            ComboBoxLines.Items.Add("Line-5");
            ComboBoxLines.Items.Add("Line-6");
            ComboBoxLines.Items.Add("Line-7");
            ComboBoxLines.Items.Add("Line-8");


            ComboBoxLines.SelectedIndex = 0;

            CheckBox CheckBoxSDP = new CheckBox
            {
                Checked = false
            };

            CheckBox checkBoxMakeVideo = new CheckBox
            {
                Checked = true
            };

            CheckBox checkBoxAnswerVideo = new CheckBox
            {
                Checked = true
            };

            CheckBox checkBoxNeedRegister = new CheckBox
            {
                Checked = true
            };
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (sIPInited == true)
            {
                MessageBox.Show("You are already logged in.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TextBoxUserName.Text.Length <= 0)
            {
                MessageBox.Show("The user name does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TextBoxPassword.Text.Length <= 0)
            {
                MessageBox.Show("The password does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TextBoxServer.Text.Length <= 0)
            {
                MessageBox.Show("The SIP server does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int SIPServerPort = 0;
            if (TextBoxServerPort.Text.Length > 0)
            {
                SIPServerPort = int.Parse(TextBoxServerPort.Text);
                if (SIPServerPort > 65535 || SIPServerPort <= 0)
                {
                    MessageBox.Show("The SIP server port is out of range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }
            }

            int StunServerPort = 0;
            if (TextBoxStunPort.Text.Length > 0)
            {
                StunServerPort = int.Parse(TextBoxStunPort.Text);
                if (StunServerPort > 65535 || StunServerPort <= 0)
                {
                    MessageBox.Show("The Stun server port is out of range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }
            }

            Random rd = new Random();
            int LocalSIPPort = rd.Next(1000, 5000) + 4000; // Generate the random port for SIP

            TRANSPORT_TYPE transportType = TRANSPORT_TYPE.TRANSPORT_TLS;
            //switch (ComboBoxTransport.SelectedIndex)
            //{
            //    case 0:
            //        transportType = TRANSPORT_TYPE.TRANSPORT_UDP;
            //        break;

            //    case 1:
            //        transportType = TRANSPORT_TYPE.TRANSPORT_TLS;

            //        break;

            //    case 2:
            //        transportType = TRANSPORT_TYPE.TRANSPORT_TCP;
            //        break;

            //    case 3:
            //        transportType = TRANSPORT_TYPE.TRANSPORT_PERS;
            //        break;
            //    default:
            //        MessageBox.Show("The transport is wrong.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        return;
            //}



            //
            // Create the class instance of PortSIP VoIP SDK, you can create more than one instances and 
            // each instance register to a SIP server to support multiples accounts & providers.
            // for example:
            /*
            _sdkLib1 = new PortSIPLib(from1);
            _sdkLib2 = new PortSIPLib(from2);
            _sdkLib3 = new PortSIPLib(from3);
            */


            portSIPLib = new PortSIPLib(this);

            //
            // Create and set the SIP callback handers, this MUST called before
            // _sdkLib.initialize();
            //
            portSIPLib.createCallbackHandlers();

            string logFilePath = ""; // The log file path, you can change it - the folder MUST exists
            string agent = "PortSIP VoIP SDK";
            string stunServer = TextBoxStunServer.Text;

            // Initialize the SDK
            int rt = portSIPLib.initialize(transportType,
                 // Use 0.0.0.0 for local IP then the SDK will choose an available local IP automatically.
                 // You also can specify a certain local IP to instead of "0.0.0.0", more details please read the SDK User Manual
                 "0.0.0.0",
                 LocalSIPPort,
                 PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE,
                 logFilePath,
                 MAX_LINES,
                 agent,
                 0,
                 0,
                 "/",
                 "",
                  false);


            if (rt != 0)
            {
                portSIPLib.releaseCallbackHandlers();
                MessageBox.Show("Initialize failure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ListBoxSIPLog.Items.Add("Initialized.");
            sIPInited = true;

            loadDevices();
            //initScreenSharing();

            string userName = TextBoxUserName.Text;
            string password = TextBoxPassword.Text;
            string sipDomain = TextBoxUserDomain.Text;
            string displayName = TextBoxDisplayName.Text;
            string authName = TextBoxAuthName.Text;
            string sipServer = TextBoxServer.Text;

            int outboundServerPort = 0;
            string outboundServer = "";

            // Set the SIP user information
            rt = portSIPLib.setUser(userName, displayName, authName, password, sipDomain, sipServer, SIPServerPort, stunServer, StunServerPort, outboundServer, outboundServerPort);
            if (rt != 0)
            {
                portSIPLib.unInitialize();
                portSIPLib.releaseCallbackHandlers();
                sIPInited = false;

                ListBoxSIPLog.Items.Clear();

                MessageBox.Show("setUser failure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ListBoxSIPLog.Items.Add("Succeeded set user information.");

            // Example: set the codec parameter for AMR-WB
            /*
             
             _sdkLib.setAudioCodecParameter(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB, "mode-set=0; octet-align=0; robust-sorting=0");
             
            */

            SetSRTPType();

            string licenseKey = "PORTSIP_TEST_LICENSE";
            rt = portSIPLib.setLicenseKey(licenseKey);
            if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
            {
                MessageBox.Show("This sample was built base on evaluation PortSIP VoIP SDK, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: sales@portsip.com to purchase the official version.");
            }
            else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
            {
                MessageBox.Show("The wrong license key was detected, please check with sales@portsip.com or support@portsip.com");
            }

            //SetVideoResolution();
            //SetVideoQuality();

            //UpdateAudioCodecs();
            InitDefaultAudioCodecs();
            //UpdateVideoCodecs();

            InitSettings();
            //updatePrackSetting();

            if (checkBoxNeedRegister.Checked)
            {
                rt = portSIPLib.registerServer(120, 0);
                if (rt != 0)
                {
                    portSIPLib.removeUser();
                    sIPInited = false;
                    portSIPLib.unInitialize();
                    portSIPLib.releaseCallbackHandlers();

                    ListBoxSIPLog.Items.Clear();

                    MessageBox.Show("register to server failed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                ListBoxSIPLog.Items.Add("Registering...");
            }
        }
        
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }
            deRegisterFromServer();
            ListBoxSIPLog.Items.Add("Softphone Disconect");
        }
        
        private void btnClearListBox_Click(object sender, EventArgs e)
        {
            ListBoxSIPLog.Items.Clear();
        }
        
        private void btnDial_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }
            if (TextBoxPhoneNumber.Text.Length <= 0)
            {
                MessageBox.Show("The phone number is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_CallSessions[currentlyLine].getSessionState() == true || _CallSessions[currentlyLine].getRecvCallState() == true)
            {
                MessageBox.Show("Current line is busy now, please switch a line.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            string callTo = TextBoxPhoneNumber.Text;

            UpdateAudioCodecs();
            //UpdateVideoCodecs();

            // Ensure the we have been added one audio codec at least
            if (portSIPLib.isAudioCodecEmpty() == true)
            {
                InitDefaultAudioCodecs();
            }

            //  Usually for 3PCC need to make call without SDP
            Boolean hasSdp = true;
            if (CheckBoxSDP.Checked == true)
            {
                hasSdp = false;
            }

            portSIPLib.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);

            int sessionId = portSIPLib.call(callTo, hasSdp, checkBoxMakeVideo.Checked);
            if (sessionId <= 0)
            {
                ListBoxSIPLog.Items.Add("Call failure");
                return;
            }

            //portSIPLib.setRemoteVideoWindow(sessionId, remoteVideoPanel.Handle);

            _CallSessions[currentlyLine].setSessionId(sessionId);
            _CallSessions[currentlyLine].setSessionState(true);

            string Text = "Line " + currentlyLine.ToString();
            Text += ": Calling...";
            ListBoxSIPLog.Items.Add(Text);
        }

        private void btnAnswer_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getRecvCallState() == false)
            {
                MessageBox.Show("No incoming call on current line, please switch a line.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _CallSessions[currentlyLine].setRecvCallState(false);
            _CallSessions[currentlyLine].setSessionState(true);

            //portSIPLib.setRemoteVideoWindow(_CallSessions[currentlyLine].getSessionId(), remoteVideoPanel.Handle);

            int rt = portSIPLib.answerCall(_CallSessions[currentlyLine].getSessionId(), checkBoxAnswerVideo.Checked);
            if (rt == 0)
            {
                string Text = "Line " + currentlyLine.ToString();
                Text += ": Call established";
                ListBoxSIPLog.Items.Add(Text);


                //joinConference(_CurrentlyLine);
            }
            else
            {
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text += ": failed to answer call !";
                ListBoxSIPLog.Items.Add(Text);
            }
        }

        private void btnHangUp_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getRecvCallState() == true)
            {
                portSIPLib.rejectCall(_CallSessions[currentlyLine].getSessionId(), 486);
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text += ": Rejected call";
                ListBoxSIPLog.Items.Add(Text);

                return;
            }

            if (_CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.hangUp(_CallSessions[currentlyLine].getSessionId());
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text += ": Hang up";
                ListBoxSIPLog.Items.Add(Text);
            }
        }

        #region Metodos Hold y Unhold se comentan ya que posiblemente causen errores en el softphone
        private void btnHold_Click(object sender, EventArgs e)
        {
            //if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            //{
            //    return;
            //}

            //if (_CallSessions[currentlyLine].getSessionState() == false || _CallSessions[currentlyLine].getHoldState() == true)
            //{
            //    return;
            //}


            //string Text;
            //int rt = portSIPLib.hold(_CallSessions[currentlyLine].getSessionId());
            //if (rt != 0)
            //{
            //    Text = "Line " + currentlyLine.ToString();
            //    Text = Text + ": hold failure.";
            //    ListBoxSIPLog.Items.Add(Text);

            //    return;
            //}


            //_CallSessions[currentlyLine].setHoldState(true);

            //Text = "Line " + currentlyLine.ToString();
            //Text = Text + ": hold";
            //ListBoxSIPLog.Items.Add(Text);
        }

        private void btnUnHold_Click(object sender, EventArgs e)
        {
            //if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            //{
            //    return;
            //}

            //if (_CallSessions[currentlyLine].getSessionState() == false || _CallSessions[currentlyLine].getHoldState() == false)
            //{
            //    return;
            //}

            //string Text;
            //int rt = portSIPLib.unHold(_CallSessions[currentlyLine].getSessionId());
            //if (rt != 0)
            //{
            //    _CallSessions[currentlyLine].setHoldState(false);

            //    Text = "Line " + currentlyLine.ToString();
            //    Text = Text + ": Un-Hold Failure.";
            //    ListBoxSIPLog.Items.Add(Text);

            //    return;
            //}

            //_CallSessions[currentlyLine].setHoldState(false);

            //Text = "Line " + currentlyLine.ToString();
            //Text = Text + ": Un-Hold";
            //ListBoxSIPLog.Items.Add(Text);
        }
        #endregion 

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getRecvCallState() == true)
            {
                portSIPLib.rejectCall(_CallSessions[currentlyLine].getSessionId(), 486);
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text += ": Rejected call";
                ListBoxSIPLog.Items.Add(Text);

                return;
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getSessionState() == false)
            {
                MessageBox.Show("Need to make the call established first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //TransferCallForm TransferDlg = new TransferCallForm();
            //if (TransferDlg.ShowDialog() != DialogResult.OK)
            //{
            //    return;
            //}

            string referTo = txtTransferTo.Text;
            if (referTo.Length <= 0)
            {
                MessageBox.Show("The transfer number is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int rt = portSIPLib.refer(_CallSessions[currentlyLine].getSessionId(), referTo);
            if (rt != 0)
            {
                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": failed to Transfer";
                ListBoxSIPLog.Items.Add(Text);
            }
            else
            {
                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": Transferring";
                ListBoxSIPLog.Items.Add(Text);
            }
        }

        private void CheckBoxMute_CheckedChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }
            if (CheckBoxMute.Checked)
            {
                portSIPLib.muteMicrophone(CheckBoxMute.Checked);
                ListBoxSIPLog.Items.Add("Microphone muted");
            }
            else
            {
                ListBoxSIPLog.Items.Add("Microphone Unmuted");
            }
            
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "1";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 1, 160, true);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "2";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 2, 160, true);
            }
        }

        private void bnt3_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "3";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 3, 160, true);
            }
        }

        private void bnt4_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "4";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 4, 160, true);
            }
        }

        private void bnt5_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "5";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 5, 160, true);
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "6";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 6, 160, true);
            }
        }

        private void bnt7_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "7";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 7, 160, true);
            }
        }

        private void bnt8_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "8";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 8, 160, true);
            }
        }

        private void bnt9_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "9";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 9, 160, true);
            }
        }

        private void btnAsterisk_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "*";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 10, 160, true);
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "0";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 0, 160, true);
            }
        }

        private void btnNumeral_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "#";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 11, 160, true);
            }
        }


        //private void btnAttendTransfer_Click(object sender, EventArgs e)
        //{
        //    if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
        //    {
        //        return;
        //    }

        //    if (_CallSessions[currentlyLine].getSessionState() == false)
        //    {
        //        MessageBox.Show("Need to make the call established first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }

        //    string referTo = txtTransferTo.Text;

        //    if (referTo.Length <= 0)
        //    {
        //        MessageBox.Show("The transfer number is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }

        //    int replaceLine = int.Parse(txtLineNum.Text);

        //    if (replaceLine <= 0 || replaceLine >= MAX_LINES)
        //    {
        //        MessageBox.Show("The replace line out of range", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }

        //    if (_CallSessions[replaceLine].getSessionState() == false)
        //    {
        //        MessageBox.Show("The replace line does not established yet", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }

        //    int rt = portSIPLib.attendedRefer(_CallSessions[currentlyLine].getSessionId(), _CallSessions[replaceLine].getSessionId(), referTo);

        //    if (rt != 0)
        //    {
        //        string Text = "Line " + currentlyLine.ToString();
        //        Text = Text + ": failed to Attend transfer";
        //        ListBoxSIPLog.Items.Add(Text);
        //    }
        //    else
        //    {
        //        string Text = "Line " + currentlyLine.ToString();
        //        Text = Text + ": Transferring";
        //        ListBoxSIPLog.Items.Add(Text);
        //    }
        //}
    }

    #region softphone de Ozeki
    /*
    private ISoftPhone softPhone;
    private IPhoneLine phoneLine;
    private RegState phoneLineInformation;
    private IPhoneCall call;
    private Microphone microphone = Microphone.GetDefaultDevice();
    private Speaker speaker = Speaker.GetDefaultDevice();
    MediaConnector connector = new MediaConnector();
    PhoneCallAudioSender mediaSender = new PhoneCallAudioSender();
    PhoneCallAudioReceiver mediaReceiver = new PhoneCallAudioReceiver();
    SIPAccount sIPAccount;


    private bool inComingCall;

    private string reDialNumber;

    private bool localHeld;
    //Credenciales para el registro del softphone que se
    //traen desde el json de cada agente al momento de cuando inicia sesión
    private static readonly bool requiredRegister = GlobalSocket.currentUser.credentials.requiredRegister; 
    private static readonly string displayName = GlobalSocket.currentUser.credentials.displayName;
    private static readonly string userName = GlobalSocket.currentUser.credentials.userName;
    private static readonly string registerName = GlobalSocket.currentUser.credentials.registerName;
    private static readonly string password = GlobalSocket.currentUser.credentials.password;
    private static readonly string domain = GlobalSocket.currentUser.credentials.domain;
    private static readonly int port = GlobalSocket.currentUser.credentials.port;
    private static readonly string proxy = GlobalSocket.currentUser.credentials.proxy;
 *         private void InitializeSoftPhone()
    {
        try
        {
            InvokeGUIThread(() => { lb_log.Items.Clear(); });
            var userAgent = "Totalsec-Softphone";
            //Aqui nada mas se crea el objeto softphone con los codecs de audio y video
            softPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 58000, 62000, userAgent);
            InvokeGUIThread(() => { lb_log.Items.Add("Softphone created!"); });
            softPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(softPhone_inComingCall);

            //sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, port, proxy);
            sIPAccount = new SIPAccount(true, "29335", "29335", "29335", "IbZ09uh2WDjJ50lv8WPs", "totalsec.com.mx", 5060, "200.38.97.144");
            //sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, port, proxy);
            InvokeGUIThread(() =>
            {
                lb_log.Items.Add($"SIP Account created - {sIPAccount.DisplayName}");
                lb_log.Items.Add($"Extensión - {sIPAccount.UserName}");
                lb_log.Items.Add($"Dominio - {sIPAccount.DomainServerHost}");
                lb_log.Items.Add($"Puerto - {sIPAccount.DomainServerPort}");
            });
            var phoneLineConfig = new PhoneLineConfiguration(sIPAccount);
            phoneLineConfig.TransportType = TransportType.Udp;
            phoneLine = softPhone.CreatePhoneLine(phoneLineConfig);
            phoneLine.RegistrationStateChanged += phoneLine_PhoneLineInformation;
            InvokeGUIThread(() => { lb_log.Items.Add("Phoneline created."); });
            softPhone.RegisterPhoneLine(phoneLine);
            txtDisplay.Text = string.Empty;
            lbl_NumberToDial.Text = sIPAccount.RegisterName;

            inComingCall = false;
            reDialNumber = string.Empty;
            localHeld = false;
            ConnectMedia();

        }
        catch (Exception ex)
        {
            InvokeGUIThread(() => { lb_log.Items.Add(ex.Message); });
        }
    }

    private void rdUDP_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void rdTLS_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void StartDevices()
    {
        if (microphone != null)
        {
            microphone.Start();
            InvokeGUIThread(() => { lb_log.Items.Add("Microphone Started."); });
        }

        if (speaker != null)
        {
            speaker.Start();
            InvokeGUIThread(() => { lb_log.Items.Add("Speaker Started."); });
        }
    }

    private void StopDevices()
    {
        if (microphone != null)
        {
            microphone.Stop();
            InvokeGUIThread(() => { lb_log.Items.Add("Microphone Stopped."); });
        }

        if (speaker != null)
        {
            speaker.Stop();
            InvokeGUIThread(() => { lb_log.Items.Add("Speaker Stopped."); });
        }
    }

    private void ConnectMedia()
    {
        if (microphone != null)
        {
            connector.Connect(microphone, mediaSender);
        }

        if (speaker != null)
        {
            connector.Connect(mediaReceiver, speaker);
        }
    }

    //private void DisconnectMedia()
    //{
    //    if (microphone != null)
    //    {
    //        connector.Disconnect(microphone, mediaSender);
    //    }

    //    if (speaker != null)
    //    {
    //        connector.Disconnect(mediaReceiver, speaker);
    //    }
    //}

    private void InvokeGUIThread(Action action)
    {
        Invoke(action);
    }

    private void softPhone_inComingCall(object sender, VoIPEventArgs<IPhoneCall> e)
    {
        InvokeGUIThread(() => { lb_log.Items.Add("Incoming call from: " + e.Item.DialInfo.ToString()); txtDisplay.Text = "Ringing (" + e.Item.DialInfo.Dialed + ")"; });

        reDialNumber = e.Item.DialInfo.Dialed;
        call = e.Item; 
        WireUpCallEvents();
        inComingCall = true;
        //call.Forward("10531");
    }

    private void phoneLine_PhoneLineInformation(object sender, RegistrationStateChangedArgs e)
    {
        phoneLineInformation = e.State;

        InvokeGUIThread(() =>
        {
        if (phoneLineInformation == RegState.RegistrationSucceeded)
        {
            lb_log.Items.Add("Registro exitoso - Online");
        }
        else
        {
            lb_log.Items.Add("No registrado - Offline: " + phoneLineInformation.ToString());
        }
        });

    }

    private void call_CallStateChanged(object sender, CallStateChangedArgs e)
    {
        InvokeGUIThread(() => { lb_log.Items.Add("Callstate changed." + e.State.ToString()); txtDisplay.Text = e.State.ToString(); });

        if (e.State == CallState.Answered)
        {
            btnHold.Enabled = true;
            //btnHold.Text = "Hold";

            StartDevices();

            mediaReceiver.AttachToCall(call);
            mediaSender.AttachToCall(call);


            InvokeGUIThread(() => { lb_log.Items.Add("Call started."); });
        }

        if (e.State == CallState.InCall)
        {
            btnHold.Enabled = true;
            //  btnHold.Text = "Hold";
            StartDevices();
        }

        if (e.State.IsCallEnded() == true)
        {
            localHeld = false;

            StopDevices();

            mediaReceiver.Detach();
            mediaSender.Detach();

            WireDownCallEvents();

            call = null;

            InvokeGUIThread(() => { lb_log.Items.Add("Call ended."); txtDisplay.Text = string.Empty; });

        }

        if (e.State == CallState.LocalHeld)
        {
            StopDevices();
        }

    }

    private void WireUpCallEvents()
    {
        call.CallStateChanged += (call_CallStateChanged);
    }

    private void WireDownCallEvents()
    {
        call.CallStateChanged -= (call_CallStateChanged);
    }

    private void CallsView_Load(object sender, EventArgs e)
    {
        InitializeSoftPhone();
    }

    private void buttonKeyPadButton_Click(object sender, EventArgs e)
    {

    }

    private void btnResponse_Click(object sender, EventArgs e)
    {

    }

    private void btnHangUp_Click(object sender, EventArgs e)
    {

    }

    private void btnRedial_Click(object sender, EventArgs e)
    {

    }

    private void btnHold_Click(object sender, EventArgs e)
    {

    }
    
    private void btnDisconect_Click(object sender, EventArgs e)
    {
        try
        {
            InvokeGUIThread(() =>
            {
                softPhone.UnregisterPhoneLine(phoneLine);
                phoneLine.Dispose();
            });
        }
        catch (Exception ex)
        {
            lb_log.Items.Add($"Error Al desconectar {ex.Message}");
        }
    }
    private void btnTransfer_Click(object sender, EventArgs e)
    {

    }
 */
    #endregion


}
