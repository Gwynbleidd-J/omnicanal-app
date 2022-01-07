using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using PortSIP;
using System.Text;
using System.Media;
using LoginForms.Shared;
using System.Net.NetworkInformation;
using System.Drawing;
using LoginForms.Properties;

namespace LoginForms
{
    public partial class CallsView : Form, SIPCallbackEvents
    {
        RestHelper rh = new RestHelper();
        //En el método onInviteRinging se pondría un tono de llamada si así lo quisieran
        private const int MAX_LINES = 2; // Maximum lines
        private const int LINE_BASE = 1;

        private Session[] _CallSessions = new Session[MAX_LINES];

        //Quitar bordes desde la carga para evitar el parpadeo

        #region Declaración de los elementos gráficos de la aplicación
        CheckBox checkBoxNeedRegister = new CheckBox
        {
            Checked = true
        };

        ComboBox ComboBoxSRTP = new ComboBox
        {
        };

        TextBox TextBoxStunServer = new TextBox
        {
        };
        
        TextBox TextBoxStunPort = new TextBox 
        { 
        };

        CheckBox CheckBoxSDP = new CheckBox
        {
            Checked = false
        };

        CheckBox checkBoxMakeVideo = new CheckBox
        {
            Checked = false
        };

        CheckBox CheckBoxAA = new CheckBox
        {
            Checked = true
        };

        CheckBox CheckBoxConf = new CheckBox
        {
            Checked = false
        };

        CheckBox checkBoxAnswerVideo = new CheckBox
        {
            Checked = true
        };

        ComboBox ComboBoxLines = new ComboBox
        {

        };

        ComboBox ComboBoxCameras = new ComboBox
        {

        };

        CheckBox checkBoxPCMU = new CheckBox
        {
            Checked = true
        };

        CheckBox checkBoxPCMA = new CheckBox
        {
            Checked = true
        };

        CheckBox checkBoxG729 = new CheckBox
        {
            Checked = true
        };

        CheckBox checkBoxAEC = new CheckBox
        {
            Checked = true
        };

        #endregion


        static bool networkIsAvailable = false;
        private bool sIPInited = false;
        private bool sIPLogined = false;
        private int currentlyLine = LINE_BASE;

       
        string boton2 = Convert.ToString(Resources._2);
        string boton3 = Convert.ToString(Resources._3);
        string boton4 = Convert.ToString(Resources._4);
        string boton5 = Convert.ToString(Resources._5);
        string boton6 = Convert.ToString(Resources._6);
        string boton7 = Convert.ToString(Resources._7);
        string boton8 = Convert.ToString(Resources._8);
        string boton9 = Convert.ToString(Resources._9);
        string boton0 = Convert.ToString(Resources._0);
        string botonAsterisco = Convert.ToString(Resources.asterisco);
        string botonGato = Convert.ToString(Resources.gato);

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

        private void joinConference(Int32 index)
        {
            if (sIPInited == false)
            {
                return;
            }
            if (CheckBoxConf.Checked == false)
            {
                return;
            }
            portSIPLib.setRemoteVideoWindow(_CallSessions[index].getSessionId(), IntPtr.Zero);
            portSIPLib.joinToConference(_CallSessions[index].getSessionId());

            // We need to un-hold the line
            if (_CallSessions[index].getHoldState())
            {
                portSIPLib.unHold(_CallSessions[index].getSessionId());
                _CallSessions[index].setHoldState(false);
            }
        }

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
            BackColor = Color.FromArgb(226, 224, 224);
            this.FormBorderStyle = FormBorderStyle.None;
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


        #region metodos para iniciar y detener la grabación de una llamada
        private void StartCallRecord()
        {
            if (sIPInited == false)
            {
                MessageBox.Show("Please initialize the SDK first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string filePath = @"d:\softphone-call-records";
            string fileName = "grabacion";

            AUDIO_RECORDING_FILEFORMAT audioRecordFileFormat = AUDIO_RECORDING_FILEFORMAT.FILEFORMAT_WAVE;

            //Se empieza la grabación
            int rt = portSIPLib.startRecord(_CallSessions[currentlyLine].getSessionId(), filePath, fileName, true, audioRecordFileFormat,
                RECORD_MODE.RECORD_BOTH, VIDEOCODEC_TYPE.VIDEO_CODEC_H264, RECORD_MODE.RECORD_RECV);

            if (rt != 0)
            {
                MessageBox.Show("Failed to start record conversation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MessageBox.Show("Started record conversation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void EndCallRecord()
        {
            if (sIPInited == false)
            {
                return;
            }

            portSIPLib.stopRecord(_CallSessions[currentlyLine].getSessionId());
            MessageBox.Show("Stop record conversation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
        #endregion

        #region Métodos de PortSIP para el correcto funcionamiento del softphone

        public Int32 onRegisterSuccess(String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            // use the Invoke method to modify the control.
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add("Registration succeeded");
                //ListBoxSIPLog.Items.Add(statusCode);
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
                //ListBoxSIPLog.Items.Add(statusCode);
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
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                AA = CheckBoxAA.Checked;
                answerVideo = checkBoxAnswerVideo.Checked;
            }));

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
                rh.SendCall();
                //StartCallRecord();

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
            //Text += ": Ringing...";//original
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
            rh.SendCall();
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {

                ListBoxSIPLog.Items.Add(Text);

                joinConference(i);
            }));

            // If this is the refer call then need set it to normal
            if (_CallSessions[i].isReferCall())
            {
                _CallSessions[i].setReferCall(false, 0);
            }
            //Start Call Record
            //StartCallRecord();
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
            //EndCallRecord();
            CallTypification typification = new CallTypification();
            typification.ShowDialog();
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
            try
            {
                string text = "Received an OPTIONS message: ";
                text += optionsMessage.ToString();
                ListBoxSIPLog.Invoke(new MethodInvoker(delegate
                {
                    //Para detener la llamada cuando se logre la llamada
                    //soundPlayer.Stop();
                    //ListBoxSIPLog.Items.Add(Text);
                }));
                //MessageBox.Show(text, "Received an OPTIONS message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
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









            ComboBox ComboBoxTransport = new ComboBox
            {
                
            };



            Connect();
            CheckNetwork();
        }

        private void CallsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            deRegisterFromServer();
        }

        private void TrackBarSpeaker_ValueChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            portSIPLib.setSpeakerVolume(TrackBarSpeaker.Value);
        }

        private void TrackBarMicrophone_ValueChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            portSIPLib.setMicVolume(TrackBarMicrophone.Value);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //if (sIPInited == true)
            //{
            //    MessageBox.Show("You are already logged in.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            ////if (TextBoxUserName.Text.Length <= 0)
            ////{
            ////    MessageBox.Show("The user name does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ////    return;
            ////}

            ////if (TextBoxPassword.Text.Length <= 0)
            ////{
            ////    MessageBox.Show("The password does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ////    return;
            ////}

            ////if (TextBoxServer.Text.Length <= 0)
            ////{
            ////    MessageBox.Show("The SIP server does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ////    return;
            ////}

            //int SIPServerPort = 0;
            //if (TextBoxServerPort.Text.Length > 0)
            //{
            //    SIPServerPort = int.Parse(TextBoxServerPort.Text);
            //    if (SIPServerPort > 65535 || SIPServerPort <= 0)
            //    {
            //        MessageBox.Show("The SIP server port is out of range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        return;
            //    }
            //}

            //int StunServerPort = 0;
            //if (TextBoxStunPort.Text.Length > 0)
            //{
            //    StunServerPort = int.Parse(TextBoxStunPort.Text);
            //    if (StunServerPort > 65535 || StunServerPort <= 0)
            //    {
            //        MessageBox.Show("The Stun server port is out of range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        return;
            //    }
            //}

            //Random rd = new Random();
            //int LocalSIPPort = rd.Next(1000, 5000) + 4000; // Generate the random port for SIP

            //TRANSPORT_TYPE transportType = TRANSPORT_TYPE.TRANSPORT_TLS;
            ////switch (ComboBoxTransport.SelectedIndex)
            ////{
            ////    case 0:
            ////        transportType = TRANSPORT_TYPE.TRANSPORT_UDP;
            ////        break;

            ////    case 1:
            ////        transportType = TRANSPORT_TYPE.TRANSPORT_TLS;

            ////        break;

            ////    case 2:
            ////        transportType = TRANSPORT_TYPE.TRANSPORT_TCP;
            ////        break;

            ////    case 3:
            ////        transportType = TRANSPORT_TYPE.TRANSPORT_PERS;
            ////        break;
            ////    default:
            ////        MessageBox.Show("The transport is wrong.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            ////        return;
            ////}



            ////
            //// Create the class instance of PortSIP VoIP SDK, you can create more than one instances and 
            //// each instance register to a SIP server to support multiples accounts & providers.
            //// for example:
            ///*
            //_sdkLib1 = new PortSIPLib(from1);
            //_sdkLib2 = new PortSIPLib(from2);
            //_sdkLib3 = new PortSIPLib(from3);
            //*/


            //portSIPLib = new PortSIPLib(this);

            ////
            //// Create and set the SIP callback handers, this MUST called before
            //// _sdkLib.initialize();
            ////

            //portSIPLib.createCallbackHandlers();

            //string logFilePath = ""; // The log file path, you can change it - the folder MUST exists
            //string agent = "PortSIP VoIP SDK";
            //string stunServer = TextBoxStunServer.Text;

            //// Initialize the SDK
            //int rt = portSIPLib.initialize(transportType,
            //     // Use 0.0.0.0 for local IP then the SDK will choose an available local IP automatically.
            //     // You also can specify a certain local IP to instead of "0.0.0.0", more details please read the SDK User Manual
            //     "0.0.0.0",
            //     LocalSIPPort,
            //     PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE,
            //     logFilePath,
            //     MAX_LINES,
            //     agent,
            //     0,
            //     0,
            //     "/",
            //     "",
            //      false);


            //if (rt != 0)
            //{
            //    portSIPLib.releaseCallbackHandlers();
            //    MessageBox.Show("Initialize failure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            //ListBoxSIPLog.Items.Add("Initialized.");
            //sIPInited = true;

            //loadDevices();
            ////initScreenSharing();

            //#region La informacion se tiene que introducir manualmente por el usuario
            ////string userName = TextBoxUserName.Text;
            ////string password = TextBoxPassword.Text;
            ////string sipDomain = TextBoxUserDomain.Text;
            ////string displayName = TextBoxDisplayName.Text;
            ////string authName = TextBoxAuthName.Text;
            ////string sipServer = TextBoxServer.Text;
            //#endregion

            //#region La informacion del usuario viene desde el JSON que se genera en el login
            //string userName = GlobalSocket.currentUser.credentials.userName;
            //string password = GlobalSocket.currentUser.credentials.password;
            //string sipDomain = GlobalSocket.currentUser.credentials.domain;
            //string displayName = GlobalSocket.currentUser.credentials.displayName;
            //string authName = GlobalSocket.currentUser.credentials.authName;
            //string sipServer = GlobalSocket.currentUser.credentials.server;
            //string sipServerPort = GlobalSocket.currentUser.credentials.port;

            //TextBoxUserName.Text = userName;
            //TextBoxPassword.Text = password;
            //TextBoxDisplayName.Text = displayName;
            //TextBoxAuthName.Text = authName;
            //TextBoxUserDomain.Text = sipDomain;
            //TextBoxServer.Text = sipServer;
            //TextBoxServerPort.Text = sipServerPort;
            //SIPServerPort = int.Parse(TextBoxServerPort.Text);
            //#endregion

            //int outboundServerPort = 0;
            //string outboundServer = "";

            //// Set the SIP user information
            //rt = portSIPLib.setUser(userName, displayName, authName, password, sipDomain, sipServer, SIPServerPort, stunServer, StunServerPort, outboundServer, outboundServerPort);
            //if (rt != 0)
            //{
            //    portSIPLib.unInitialize();
            //    portSIPLib.releaseCallbackHandlers();
            //    sIPInited = false;

            //    ListBoxSIPLog.Items.Clear();

            //    MessageBox.Show("setUser failure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            //ListBoxSIPLog.Items.Add("Succeeded set user information.");

            //// Example: set the codec parameter for AMR-WB
            ///*
             
            // _sdkLib.setAudioCodecParameter(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB, "mode-set=0; octet-align=0; robust-sorting=0");
             
            //*/

            //SetSRTPType();

            //string licenseKey = "PORTSIP_TEST_LICENSE";
            //rt = portSIPLib.setLicenseKey(licenseKey);
            //if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
            //{
            //    MessageBox.Show("This sample was built base on evaluation PortSIP VoIP SDK, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: sales@portsip.com to purchase the official version.");
            //}
            //else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
            //{
            //    MessageBox.Show("The wrong license key was detected, please check with sales@portsip.com or support@portsip.com");
            //}

            ////SetVideoResolution();
            ////SetVideoQuality();

            ////UpdateAudioCodecs();
            //InitDefaultAudioCodecs();
            ////UpdateVideoCodecs();

            //InitSettings();
            ////updatePrackSetting();

            //if (checkBoxNeedRegister.Checked == true)
            //{
            //    rt = portSIPLib.registerServer(120, 0);
            //    if (rt != 0)
            //    {
            //        portSIPLib.removeUser();
            //        sIPInited = false;
            //        portSIPLib.unInitialize();
            //        portSIPLib.releaseCallbackHandlers();

            //        ListBoxSIPLog.Items.Clear();

            //        MessageBox.Show("register to server failed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    }


            //    ListBoxSIPLog.Items.Add("Registering...");
            //}
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

        #region boton para constestar las llamadas entrantes
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


                joinConference(currentlyLine);
            }
            else
            {
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text += ": failed to answer call !";
                ListBoxSIPLog.Items.Add(Text);
            }
        }
        #endregion



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
                //EndCallRecord();
                CallTypification typification = new CallTypification();
                typification.ShowDialog();
            }
            
            //EndCallRecord();
        }

        #region Boton para rechazar una llamaada
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
        #endregion

        #region Metodos Hold y Unhold se comentan ya que posiblemente causen errores en el softphone

        private void btnHold_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getSessionState() == false || _CallSessions[currentlyLine].getHoldState() == true)
            {
                return;
            }


            string Text;
            int rt = portSIPLib.hold(_CallSessions[currentlyLine].getSessionId());
            if (rt != 0)
            {
                Text = "Line " + currentlyLine.ToString();
                Text = Text + ": hold failure.";
                ListBoxSIPLog.Items.Add(Text);

                return;
            }


            _CallSessions[currentlyLine].setHoldState(true);

            Text = "Line " + currentlyLine.ToString();
            Text = Text + ": hold";
            ListBoxSIPLog.Items.Add(Text);
        }

        private void btnUnhold_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getSessionState() == false || _CallSessions[currentlyLine].getHoldState() == false)
            {
                return;
            }

            string Text;
            int rt = portSIPLib.unHold(_CallSessions[currentlyLine].getSessionId());
            if (rt != 0)
            {
                _CallSessions[currentlyLine].setHoldState(false);

                Text = "Line " + currentlyLine.ToString();
                Text = Text + ": Un-Hold Failure.";
                ListBoxSIPLog.Items.Add(Text);

                return;
            }

            _CallSessions[currentlyLine].setHoldState(false);

            Text = "Line " + currentlyLine.ToString();
            Text = Text + ": Un-Hold";
            ListBoxSIPLog.Items.Add(Text);
        }
        #endregion
       
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

        #region botón para la transferencia asistida
        private void btnForeignTransfer_Click(object sender, EventArgs e)
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


            string referTo = txtTransferTo.Text;
            if (referTo.Length <= 0)
            {
                MessageBox.Show("The transfer numbre is empty", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            int replaceLine = MAX_LINES;
            if (replaceLine <= 0 || replaceLine >= MAX_LINES)
            {
                MessageBox.Show("The replace line out of range", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_CallSessions[replaceLine].getSessionState() == false)
            {
                MessageBox.Show("The replace line does not established yet", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int rt = portSIPLib.attendedRefer(_CallSessions[currentlyLine].getSessionId(), _CallSessions[replaceLine].getSessionId(), referTo);
            if (rt != 0)
            {
                string Text = "Line" + MAX_LINES.ToString();
                Text = Text + ": failed to Transfer";
                ListBoxSIPLog.Items.Add(Text);
            }
            else
            {
                string Text = "Line " + MAX_LINES.ToString();
                Text = Text + ": Transferring";
                ListBoxSIPLog.Items.Add(Text);
            }
        }
        #endregion

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

        private void btn3_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "3";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 3, 160, true);
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "4";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 4, 160, true);
            }
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text += "5";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 5, 160, true);
            }
        }

        private void bnt6_Click(object sender, EventArgs e)
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

        private void btn9_Click(object sender, EventArgs e)
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

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                TextBoxRecordFilePath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnStartRecord_Click(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                MessageBox.Show("Please initialize the SDK first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (TextBoxRecordFilePath.Text.Length <= 0 || TextBoxRecordFileName.Text.Length <= 0)
            {
                MessageBox.Show("The file path or file name is empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            string filePath = TextBoxRecordFilePath.Text;
            string fileName = TextBoxRecordFileName.Text;

            AUDIO_RECORDING_FILEFORMAT audioRecordFileFormat = AUDIO_RECORDING_FILEFORMAT.FILEFORMAT_WAVE;

            //  Start recording
            int rt = portSIPLib.startRecord(_CallSessions[currentlyLine].getSessionId(),
                                        filePath,
                                        fileName,
                                        true,
                                        audioRecordFileFormat,
                                        RECORD_MODE.RECORD_BOTH,
                                        VIDEOCODEC_TYPE.VIDEO_CODEC_H264,
                                        RECORD_MODE.RECORD_RECV);
            if (rt != 0)
            {
                MessageBox.Show("Failed to start record conversation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MessageBox.Show("Started record conversation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnEndRecord_Click(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            portSIPLib.stopRecord(_CallSessions[currentlyLine].getSessionId());

            MessageBox.Show("Stop record conversation.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void CheckBoxConf_CheckedChanged(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                CheckBoxConf.Checked = false;
                return;
            }

            Int32 width = 352;
            Int32 height = 288;

            //switch (ComboBoxVideoResolution.SelectedIndex)
            //{
            //    case 0://qcif
            //        width = 176;
            //        height = 144;
            //        break;
            //    case 1://cif
            //        width = 352;
            //        height = 288;
            //        break;
            //    case 2://VGA
            //        width = 640;
            //        height = 480;
            //        break;
            //    case 3://svga
            //        width = 800;
            //        height = 600;
            //        break;
            //    case 4://xvga
            //        width = 1024;
            //        height = 768;
            //        break;
            //    case 5://q720
            //        width = 1280;
            //        height = 720;
            //        break;
            //    case 6://qvga
            //        width = 320;
            //        height = 240;
            //        break;
            //}


            if (CheckBoxConf.Checked == true)
            {

                //int rt = portSIPLib.createVideoConference(remoteVideoPanel.Handle, width, height, false);
                int rt = portSIPLib.createAudioConference();
                if (rt == 0)
                {
                    ListBoxSIPLog.Items.Add("Make conference succeeded");
                    for (int i = LINE_BASE; i < MAX_LINES; ++i)
                    {
                        if (_CallSessions[i].getSessionState() == true)
                        {
                            joinConference(i);
                        }
                    }
                }
                else
                {
                    ListBoxSIPLog.Items.Add("Failed to create conference");
                    CheckBoxConf.Checked = false;
                }
            }
            else
            {
                // Stop conference
                // Before stop the conference, MUST place all lines to hold state

                for (int i = LINE_BASE; i < MAX_LINES; ++i)
                {
                    if (_CallSessions[i].getSessionState() == true &&
                        _CallSessions[i].getHoldState() == false &&
                        currentlyLine != i)
                    {
                        // place all lines to "Hold" state except current used one
                        portSIPLib.hold(_CallSessions[i].getSessionId());
                        _CallSessions[i].setHoldState(true);
                    }
                }

                portSIPLib.destroyConference();

                if (_CallSessions[currentlyLine].getSessionState() == true &&
                    _CallSessions[currentlyLine].getHoldState() == false)
                {
                    //portSIPLib.setRemoteVideoWindow(_CallSessions[currentlyLine].getSessionId(), remoteVideoPanel.Handle);
                }
                ListBoxSIPLog.Items.Add("Taken off Conference");

            }
        }

        public void Connect()
        {
            if (sIPInited == true)
            {
                MessageBox.Show("You are already logged in.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (TextBoxUserName.Text.Length <= 0)
            //{
            //    MessageBox.Show("The user name does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            //if (TextBoxPassword.Text.Length <= 0)
            //{
            //    MessageBox.Show("The password does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            //if (TextBoxServer.Text.Length <= 0)
            //{
            //    MessageBox.Show("The SIP server does not allows empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

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

            #region La informacion se tiene que introducir manualmente por el usuario
            //string userName = TextBoxUserName.Text;
            //string password = TextBoxPassword.Text;
            //string sipDomain = TextBoxUserDomain.Text;
            //string displayName = TextBoxDisplayName.Text;
            //string authName = TextBoxAuthName.Text;
            //string sipServer = TextBoxServer.Text;
            #endregion

            #region La informacion del usuario viene desde el JSON que se genera en el login
            string userName = GlobalSocket.currentUser.credentials.userName;
            string password = GlobalSocket.currentUser.credentials.password;
            string sipDomain = GlobalSocket.currentUser.credentials.domain;
            string displayName = GlobalSocket.currentUser.credentials.displayName;
            string authName = GlobalSocket.currentUser.credentials.authName;
            string sipServer = GlobalSocket.currentUser.credentials.server;
            string sipServerPort = GlobalSocket.currentUser.credentials.port;

            TextBoxUserName.Text = userName;
            TextBoxPassword.Text = password;
            TextBoxDisplayName.Text = displayName;
            TextBoxAuthName.Text = authName;
            TextBoxUserDomain.Text = sipDomain;
            TextBoxServer.Text = sipServer;
            TextBoxServerPort.Text = sipServerPort;
            SIPServerPort = int.Parse(TextBoxServerPort.Text);
            #endregion

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

            if (checkBoxNeedRegister.Checked == true)
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

        private void Disconnect()
        {
            if (sIPInited == false)
            {
                return;
            }
            deRegisterFromServer();
            ListBoxSIPLog.Items.Add("Softphone Disconect");
        }


        
        public void CheckNetwork()
        {
            networkIsAvailable = false;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface nic in nics)
            {
                if ((nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    && nic.OperationalStatus == OperationalStatus.Up)
                {
                    networkIsAvailable = true;
                }
            }
            if (networkIsAvailable != false)
            {
                pnlRed.BackColor = Color.Green;
                lblNotification.Text = "Softphone Online";
            }
            else
            {
                pnlRed.BackColor = Color.Red;
            }
            Console.WriteLine($"Disponibilidad de la red[Mensaje 1]: {networkIsAvailable}");
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }

        public void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            networkIsAvailable = e.IsAvailable;
            if (!networkIsAvailable)
            {
                pnlRed.BackColor = Color.Red;
                lblNotification.Text = "Softphone desconectado, revisa la conexión a Internet";
            }
            else
            {
                Disconnect();
                Connect();
                pnlRed.BackColor = Color.Green;
                lblNotification.Text = "Softphone Online";

            }
            Console.WriteLine($"Disponibilidad de la red[Mensaje 2]: {networkIsAvailable}");
        }

        #region Metodos para darle animación a los botones del softphone
        private void btn1_MouseDown(object sender, MouseEventArgs e)
        {
            btn1.BackgroundImage = Properties.Resources._1_presionado;
        }

        //esto es lo nuevo de que le agregue
        //ahora hay que ver que se conecte al PBX
        //tienes abierto visual code?
        //SII


        private void btn1_MouseUp(object sender, MouseEventArgs e)
        {
            btn1.BackgroundImage = Properties.Resources._1;
        }

        private void btn2_MouseDown(object sender, MouseEventArgs e)
        {
            btn2.BackgroundImage = Properties.Resources._2_presionado;
        }

        private void btn2_MouseUp(object sender, MouseEventArgs e)
        {
            btn2.BackgroundImage = Properties.Resources._2;
        }

        private void btn3_MouseDown(object sender, MouseEventArgs e)
        {
            btn3.BackgroundImage = Properties.Resources._3_presionado;
        }

        private void btn3_MouseUp(object sender, MouseEventArgs e)
        {
            btn3.BackgroundImage = Properties.Resources._3;
        }

        private void btn4_MouseDown(object sender, MouseEventArgs e)
        {
            btn4.BackgroundImage = Properties.Resources._4_presionado;
        }

        private void btn4_MouseUp(object sender, MouseEventArgs e)
        {
            btn4.BackgroundImage = Properties.Resources._4;
        }

        private void btn5_MouseDown(object sender, MouseEventArgs e)
        {
            btn5.BackgroundImage = Properties.Resources._5_presionado;
        }

        private void btn5_MouseUp(object sender, MouseEventArgs e)
        {
            btn5.BackgroundImage = Properties.Resources._5;
        }

        private void bnt6_MouseDown(object sender, MouseEventArgs e)
        {
            bnt6.BackgroundImage = Properties.Resources._6_presionado;
        }

        private void bnt6_MouseUp(object sender, MouseEventArgs e)
        {
            bnt6.BackgroundImage = Properties.Resources._6;
        }

        private void bnt7_MouseDown(object sender, MouseEventArgs e)
        {
            bnt7.BackgroundImage = Properties.Resources._7_presionado;
        }

        private void bnt7_MouseUp(object sender, MouseEventArgs e)
        {
            bnt7.BackgroundImage = Properties.Resources._7;
        }

        private void bnt8_MouseDown(object sender, MouseEventArgs e)
        {
            bnt8.BackgroundImage = Properties.Resources._8_presionado;
        }

        private void bnt8_MouseUp(object sender, MouseEventArgs e)
        {
            bnt8.BackgroundImage = Properties.Resources._8;
        }

        private void btn9_MouseDown(object sender, MouseEventArgs e)
        {
            btn9.BackgroundImage = Properties.Resources._9_presionado;
        }

        private void btn9_MouseUp(object sender, MouseEventArgs e)
        {
            btn9.BackgroundImage = Properties.Resources._9;
        }

        private void btnAsterisk_MouseDown(object sender, MouseEventArgs e)
        {
            btnAsterisk.BackgroundImage = Properties.Resources.asterisco_presionado;
        }

        private void btnAsterisk_MouseUp(object sender, MouseEventArgs e)
        {
            btnAsterisk.BackgroundImage = Properties.Resources.asterisco;
        }

        private void btn0_MouseDown(object sender, MouseEventArgs e)
        {
            btn0.BackgroundImage = Properties.Resources._0_presionado;
        }

        private void btn0_MouseUp(object sender, MouseEventArgs e)
        {
            btn0.BackgroundImage = Properties.Resources._0;
        }

        private void btnNumeral_MouseDown(object sender, MouseEventArgs e)
        {
            btnNumeral.BackgroundImage = Properties.Resources.gato_presionado;
        }

        private void btnNumeral_MouseUp(object sender, MouseEventArgs e)
        {
            btnNumeral.BackgroundImage = Properties.Resources.gato;
        }

        private void btnUnhold_MouseDown(object sender, MouseEventArgs e)
        {
            btnUnhold.BackgroundImage = Properties.Resources.hold_hover;
        }

        private void btnUnhold_MouseUp(object sender, MouseEventArgs e)
        {
            btnUnhold.BackgroundImage = Properties.Resources.hold;
        }

        private void btnHold_MouseDown(object sender, MouseEventArgs e)
        {
            btnHold.BackgroundImage = Properties.Resources.tiempo_presionado;
        }

        private void btnHold_MouseUp(object sender, MouseEventArgs e)
        {
            btnHold.BackgroundImage = Properties.Resources.tiempo;
        }

        private void btnDial_MouseDown(object sender, MouseEventArgs e)
        {
            btnDial.BackgroundImage = Properties.Resources.llamar_presionado;
        }

        private void btnDial_MouseUp(object sender, MouseEventArgs e)
        {
            btnDial.BackgroundImage = Properties.Resources.llamar;
        }

        private void btnHangUp_MouseDown(object sender, MouseEventArgs e)
        {
            btnHangUp.BackgroundImage = Properties.Resources.colgar_presionado;
        }


        private void btnHangUp_MouseUp(object sender, MouseEventArgs e)
        {
            btnHangUp.BackgroundImage = Properties.Resources.colgar;
        }

        #endregion

        #region Metodo para salir de una conferencia
        //Se comenta para ver si pueded tener una utilidad en un futuro.
        private void btnExitFromConference_Click(object sender, EventArgs e)
        {
            if (_CallSessions[currentlyLine].getSessionState() == true)
            {
                portSIPLib.removeFromConference(_CallSessions[currentlyLine].getSessionId());
                MessageBox.Show("Se desconecto correctamente de la conferencia", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("No se pudo desconectar el softphone de la conferencia", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnClearListBox_MouseDown(object sender, MouseEventArgs e)
        {
            btnClearListBox.BackgroundImage = Properties.Resources.backspace_hover;
        }

        private void btnClearListBox_MouseUp(object sender, MouseEventArgs e)
        {
            btnClearListBox.BackgroundImage = Properties.Resources.backspace;
        }

        private void btnStartRecord_MouseDown(object sender, MouseEventArgs e)
        {
            btnStartRecord.BackgroundImage = Properties.Resources.grabar_hover;
        }

        private void btnStartRecord_MouseUp(object sender, MouseEventArgs e)
        {
            btnStartRecord.BackgroundImage = Properties.Resources.grabar;
        }

        private void btnEndRecord_MouseDown(object sender, MouseEventArgs e)
        {
            btnEndRecord.BackgroundImage = Properties.Resources.stop_hover;
        }

        private void btnEndRecord_MouseUp(object sender, MouseEventArgs e)
        {
            btnEndRecord.BackgroundImage = Properties.Resources.stop;
        }

        private void btnTransfer_MouseDown(object sender, MouseEventArgs e)
        {
            btnTransfer.BackgroundImage = Properties.Resources.interambiar_presionado;
        }

        private void btnTransfer_MouseUp(object sender, MouseEventArgs e)
        {
            btnTransfer.BackgroundImage = Properties.Resources.intercambiar;
        }





        #endregion

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
}
