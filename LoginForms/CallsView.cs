using PortSIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LoginForms.Shared;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using LoginForms.Models;
using Newtonsoft.Json.Linq;

namespace LoginForms
{
    public partial class CallsView : Form, SIPCallbackEvents
    {
        RestHelper rh = new RestHelper();
        private const int MAX_LINES = 2; // Maximum lines
        private const int LINE_BASE = 1;

        private Session[] _CallSessions = new Session[MAX_LINES];

        static bool networkIsAvailable = false;
        private bool sIPInited = false;
        private bool sIPLogined = false;
        private int currentlyLine = LINE_BASE;

        private PortSIPLib sdkLib;

        #region Metodos PortSIP
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
            int nics = sdkLib.getNICNums();
            for (int i = 0; i < nics; ++i)
            {
                sdkLib.getLocalIpAddress(i, localIP, 64);
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


        private void updatePrackSetting()
        {
            if (!sIPInited)
            {
                return;
            }

            sdkLib.enableReliableProvisional(checkBoxPRACK.Checked);
        }

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
            sdkLib.setRemoteVideoWindow(_CallSessions[index].getSessionId(), IntPtr.Zero);
            sdkLib.joinToConference(_CallSessions[index].getSessionId());

            // We need to un-hold the line
            if (_CallSessions[index].getHoldState())
            {
                sdkLib.unHold(_CallSessions[index].getSessionId());
                _CallSessions[index].setHoldState(false);
            }
        }

        private void loadDevices()
        {
            if (sIPInited == false)
            {
                return;
            }

            int num = sdkLib.getNumOfPlayoutDevices();
            for (int i = 0; i < num; ++i)
            {
                StringBuilder deviceName = new StringBuilder();
                deviceName.Length = 256;

                if (sdkLib.getPlayoutDeviceName(i, deviceName, 256) == 0)
                {
                    ComboBoxSpeakers.Items.Add(deviceName.ToString());
                }

                ComboBoxSpeakers.SelectedIndex = 0;
            }


            num = sdkLib.getNumOfRecordingDevices();
            for (int i = 0; i < num; ++i)
            {
                StringBuilder deviceName = new StringBuilder();
                deviceName.Length = 256;

                if (sdkLib.getRecordingDeviceName(i, deviceName, 256) == 0)
                {
                    ComboBoxMicrophones.Items.Add(deviceName.ToString());
                }

                ComboBoxMicrophones.SelectedIndex = 0;
            }

            int volume = sdkLib.getSpeakerVolume();
            TrackBarSpeaker.SetRange(0, 255);
            TrackBarSpeaker.Value = volume;

            volume = sdkLib.getMicVolume();
            TrackBarMicrophone.SetRange(0, 255);
            TrackBarMicrophone.Value = volume;

        }


        private void InitSettings()
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.enableAEC(checkBoxAEC.Checked);
            sdkLib.enableCNG(checkBoxCNG.Checked);
            sdkLib.enableAGC(checkBoxAGC.Checked);
            sdkLib.enableANS(checkBoxANS.Checked);


            sdkLib.setVideoNackStatus(checkBoxNack.Checked);

            sdkLib.setDoNotDisturb(CheckBoxDND.Checked);
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

            sdkLib.setSrtpPolicy(SRTPPolicy, true);
        }


        // Default we just using PCMU, PCMA, and G.279
        private void InitDefaultAudioCodecs()
        {
            if (sIPInited == false)
            {
                return;
            }


            sdkLib.clearAudioCodec();


            // Default we just using PCMU, PCMA, G729
            sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);

            sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF);  // for DTMF as RTP Event - RFC2833
        }

        private void UpdateAudioCodecs()
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.clearAudioCodec();

            if (checkBoxPCMU.Checked == true)
            {
                sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            }


            if (checkBoxPCMA.Checked == true)
            {
                sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            }


            if (checkBoxG729.Checked == true)
            {
                sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);
            }

            #region Codecs que probablemente se puedan usar
            //if (checkBoxILBC.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_ILBC);
            //}

            //if (checkBoxGSM.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_GSM);
            //}

            //if (checkBoxAMR.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMR);
            //}

            //if (CheckBoxG722.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G722);
            //}

            //if (CheckBoxSpeex.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEX);
            //}

            //if (CheckBoxAMRwb.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB);
            //}

            //if (CheckBoxSpeexWB.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEXWB);
            //}

            //if (CheckBoxG7221.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G7221);
            //}

            //if (checkBoxOPUS.Checked == true)
            //{
            //    sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_OPUS);
            //}
            #endregion

            sdkLib.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF);

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
                    sdkLib.rejectCall(_CallSessions[i].getSessionId(), 486);
                }
                else if (_CallSessions[i].getSessionState() == true)
                {
                    sdkLib.hangUp(_CallSessions[i].getSessionId());
                }

                _CallSessions[i].reset();
            }

            if (sIPLogined)
            {
                sdkLib.unRegisterServer();
                sIPLogined = false;
            }

            sdkLib.removeUser();

            if (sIPInited)
            {
                sdkLib.unInitialize();
                sdkLib.releaseCallbackHandlers();

                sIPInited = false;
            }


            ListBoxSIPLog.Items.Clear();

            ComboBoxLines.SelectedIndex = 0;
            currentlyLine = LINE_BASE;


            ComboBoxSpeakers.Items.Clear();
            ComboBoxMicrophones.Items.Clear();
        }
        #endregion

        #region Metodos Propios de la aplicacion
        public CallsView()
        {
            InitializeComponent();

            BackColor = Color.FromArgb(226, 224, 224);
            this.FormBorderStyle = FormBorderStyle.None;
        }


        #region metodos para iniciar y detener la grabación de una llamada
        private async void StartCallRecord()
        {
            if (sIPInited == false)
            {
                MessageBox.Show("Please initialize the SDK first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string callId = await rh.GetIdCall();
            //MsjApi jsonData = JsonConvert.DeserializeObject<MsjApi>(callId);
            //var data = jsonData.data;
            var jobject = JsonConvert.DeserializeObject<JObject>(callId);

            var id = jobject.Value<string>("data").ToString();

            string filePath = "C:/Users/KODE/Documents/llamadas";
            string fileName = $"grabacion-{id}";

            AUDIO_RECORDING_FILEFORMAT audioRecordFileFormat = AUDIO_RECORDING_FILEFORMAT.FILEFORMAT_WAVE;

            //start recording
            int rt = sdkLib.startRecord(_CallSessions[currentlyLine].getSessionId(), filePath, fileName, false, audioRecordFileFormat, RECORD_MODE.RECORD_BOTH, VIDEOCODEC_TYPE.VIDEO_CODEC_H264, RECORD_MODE.RECORD_RECV);

            if (rt != 0)
            {
                lblRecordEstatus.Text = "Fallo la grabacion de la conversación.";
                //MessageBox.Show("Fallo la grabacion de la conversación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            lblRecordEstatus.Text = "Se inicio la grabación de la llamada.";
            //MessageBox.Show("Se inicio la grabación de la llamada.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void EndCallRecord()
        {
            if (sIPInited == false)
            {
                return;
            }
            sdkLib.stopRecord(_CallSessions[currentlyLine].getSessionId());
            lblRecordEstatus.Text = "Se detuvo la grabación de la llamada.";
            btnBeginRecord.Text = "Grabar Llamada";
            btnBeginRecord.Enabled = true;
            //MessageBox.Show("Se detuvo la grabación de la llamada.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            ComboBoxTransport.Items.Add("UDP");
            ComboBoxTransport.Items.Add("TLS");
            ComboBoxTransport.Items.Add("TCP");
            ComboBoxTransport.Items.Add("PERS");

            ComboBoxTransport.SelectedIndex = 0;

            ComboBoxSRTP.Items.Add("None");
            ComboBoxSRTP.Items.Add("Prefer");
            ComboBoxSRTP.Items.Add("Force");

            ComboBoxSRTP.SelectedIndex = 0;

            ComboBoxLines.Items.Add("Line-1");


            ComboBoxLines.SelectedIndex = 0;

            Connect();
            CheckNetwork();
        }

        private void CallsView_FormClosing(object sender, FormClosingEventArgs e)
        {
            deRegisterFromServer();
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

            TRANSPORT_TYPE transportType = TRANSPORT_TYPE.TRANSPORT_UDP;
            switch (ComboBoxTransport.SelectedIndex)
            {
                case 0:
                    transportType = TRANSPORT_TYPE.TRANSPORT_UDP;
                    break;

                case 1:
                    transportType = TRANSPORT_TYPE.TRANSPORT_TLS;
                    break;

                case 2:
                    transportType = TRANSPORT_TYPE.TRANSPORT_TCP;
                    break;

                case 3:
                    transportType = TRANSPORT_TYPE.TRANSPORT_PERS;
                    break;
                default:
                    MessageBox.Show("The transport is wrong.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
            }



            //
            // Create the class instance of PortSIP VoIP SDK, you can create more than one instances and 
            // each instance register to a SIP server to support multiples accounts & providers.
            // for example:
            /*
            _sdkLib1 = new PortSIPLib(from1);
            _sdkLib2 = new PortSIPLib(from2);
            _sdkLib3 = new PortSIPLib(from3);
            */


            sdkLib = new PortSIPLib(this);

            //
            // Create and set the SIP callback handers, this MUST called before
            // _sdkLib.initialize();
            //
            sdkLib.createCallbackHandlers();

            string logFilePath = "d:\\"; // The log file path, you can change it - the folder MUST exists
            string agent = "PortSIP VoIP SDK";
            string stunServer = TextBoxStunServer.Text;

            // Initialize the SDK
            int rt = sdkLib.initialize(transportType,
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
                sdkLib.releaseCallbackHandlers();
                MessageBox.Show("Initialize failure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ListBoxSIPLog.Items.Add("Initialized.");
            sIPInited = true;

            loadDevices();

            string userName = TextBoxUserName.Text;
            string password = TextBoxPassword.Text;
            string sipDomain = TextBoxUserDomain.Text;
            string displayName = TextBoxDisplayName.Text;
            string authName = TextBoxAuthName.Text;
            string sipServer = TextBoxServer.Text;

            int outboundServerPort = 0;
            string outboundServer = "";

            // Set the SIP user information
            rt = sdkLib.setUser(userName, displayName, authName, password, sipDomain, sipServer, SIPServerPort, stunServer, StunServerPort, outboundServer, outboundServerPort);
            if (rt != 0)
            {
                sdkLib.unInitialize();
                sdkLib.releaseCallbackHandlers();
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

            string licenseKey = "1WINqx00QjlERUMwMkI1OUMzNUI2OTdENjI2MDA2RDNFRjU2REBCQkRCNzI1NEE4MDVDRjUwNzQ1NDAxRkIyRkQzMTNFREA4MEE3RkYzMjkwOTJBMkJFM0MzMzBGOTY3MzRGRDBBM0A5NEQwNDFEQzE2OTUyNjhGRjBBRkE2QzE4QTQ1N0I2NA";
            rt = sdkLib.setLicenseKey(licenseKey);
            if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
            {
                MessageBox.Show("This sample was built base on evaluation PortSIP VoIP SDK, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: sales@portsip.com to purchase the official version.");
            }
            else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
            {
                MessageBox.Show("The wrong license key was detected, please check with sales@portsip.com or support@portsip.com");
            }

            UpdateAudioCodecs();

            InitSettings();
            updatePrackSetting();

            if (checkBoxNeedRegister.Checked)
            {
                rt = sdkLib.registerServer(120, 0);
                if (rt != 0)
                {
                    sdkLib.removeUser();
                    sIPInited = false;
                    sdkLib.unInitialize();
                    sdkLib.releaseCallbackHandlers();

                    ListBoxSIPLog.Items.Clear();

                    MessageBox.Show("register to server failed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                ListBoxSIPLog.Items.Add("Registering...");
            }
        }

        private void btnOffline_Click(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            deRegisterFromServer();
        }

        private void ComboBoxLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                ComboBoxLines.SelectedIndex = 0;
                return;
            }

            if (currentlyLine == (ComboBoxLines.SelectedIndex + LINE_BASE))
            {
                return;
            }

            if (CheckBoxConf.Checked == true)
            {
                currentlyLine = ComboBoxLines.SelectedIndex + LINE_BASE;
                return;
            }

            // To switch the line, must hold currently line first
            if (_CallSessions[currentlyLine].getSessionState() == true && _CallSessions[currentlyLine].getHoldState() == false)
            {
                sdkLib.hold(_CallSessions[currentlyLine].getSessionId());
                sdkLib.setRemoteVideoWindow(_CallSessions[currentlyLine].getSessionId(), IntPtr.Zero);
                _CallSessions[currentlyLine].setHoldState(true);

                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": Hold";
                ListBoxSIPLog.Items.Add(Text);
            }



            currentlyLine = ComboBoxLines.SelectedIndex + LINE_BASE;


            // If target line was in hold state, then un-hold it
            if (_CallSessions[currentlyLine].getSessionState() == true && _CallSessions[currentlyLine].getHoldState() == true)
            {
                sdkLib.unHold(_CallSessions[currentlyLine].getSessionId());
                //sdkLib.setRemoteVideoWindow(_CallSessions[currentlyLine].getSessionId(), remoteVideoPanel.Handle);
                _CallSessions[currentlyLine].setHoldState(false);

                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": UnHold - call established";
                ListBoxSIPLog.Items.Add(Text);
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "1";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 1, 160, true);
            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "2";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 2, 160, true);
            }
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "3";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 3, 160, true);
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "4";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 4, 160, true);
            }
        }

        private void bnt5_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "5";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 5, 160, true);
            }
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "6";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 6, 160, true);
            }
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "7";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 7, 160, true);
            }
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "8";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 8, 160, true);
            }
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "9";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 9, 160, true);
            }
        }

        private void btnAsterisco_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "*";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 10, 160, true);
            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "0";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 0, 160, true);
            }
        }

        private void btnGato_Click(object sender, EventArgs e)
        {
            TextBoxPhoneNumber.Text = TextBoxPhoneNumber.Text + "#";
            if (sIPInited == true && _CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.sendDtmf(_CallSessions[currentlyLine].getSessionId(), DTMF_METHOD.DTMF_RFC2833, 11, 160, true);
            }
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
            if (sdkLib.isAudioCodecEmpty() == true)
            {
                InitDefaultAudioCodecs();
            }

            //  Usually for 3PCC need to make call without SDP
            Boolean hasSdp = true;
            if (CheckBoxSDP.Checked == true)
            {
                hasSdp = false;
            }

            sdkLib.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);

            int sessionId = sdkLib.call(callTo, hasSdp, checkBoxMakeVideo.Checked);
            if (sessionId <= 0)
            {
                ListBoxSIPLog.Items.Add("Call failure");
                return;
            }

            //sdkLib.setRemoteVideoWindow(sessionId, remoteVideoPanel.Handle);

            _CallSessions[currentlyLine].setSessionId(sessionId);
            _CallSessions[currentlyLine].setSessionState(true);

            string Text = "Line " + currentlyLine.ToString();
            Text = Text + ": Calling...";
            ListBoxSIPLog.Items.Add(Text);
        }

        private void btnHangUp_Click(object sender, EventArgs e)
        {
            if (sIPInited == false || (checkBoxNeedRegister.Checked && (sIPLogined == false)))
            {
                return;
            }

            if (_CallSessions[currentlyLine].getRecvCallState() == true)
            {
                sdkLib.rejectCall(_CallSessions[currentlyLine].getSessionId(), 486);
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": Rejected call";
                ListBoxSIPLog.Items.Add(Text);

                return;
            }

            if (_CallSessions[currentlyLine].getSessionState() == true)
            {
                sdkLib.hangUp(_CallSessions[currentlyLine].getSessionId());
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": Hang up";
                ListBoxSIPLog.Items.Add(Text);
                EndCallRecord();
                CallTypification typification = new CallTypification();
                typification.ShowDialog();
                typification.StartPosition = FormStartPosition.CenterParent;
                btnBeginRecord.Enabled = true;
                btnBeginRecord.Text = "Grabar Llamada";
            }
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

            //sdkLib.setRemoteVideoWindow(_CallSessions[currentlyLine].getSessionId(), remoteVideoPanel.Handle);

            int rt = sdkLib.answerCall(_CallSessions[currentlyLine].getSessionId(), checkBoxAnswerVideo.Checked);
            if (rt == 0)
            {
                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": Call established";
                ListBoxSIPLog.Items.Add(Text);


                joinConference(currentlyLine);
            }
            else
            {
                _CallSessions[currentlyLine].reset();

                string Text = "Line " + currentlyLine.ToString();
                Text = Text + ": failed to answer call !";
                ListBoxSIPLog.Items.Add(Text);
            }
        }

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
            int rt = sdkLib.hold(_CallSessions[currentlyLine].getSessionId());
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

        private void btnUnHold_Click(object sender, EventArgs e)
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
            int rt = sdkLib.unHold(_CallSessions[currentlyLine].getSessionId());
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

            int rt = sdkLib.refer(_CallSessions[currentlyLine].getSessionId(), referTo);
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
            sdkLib.muteMicrophone(CheckBoxMute.Checked);
        }

        private void TrackBarSpeaker_ValueChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.setSpeakerVolume(TrackBarSpeaker.Value);
        }

        private void TrackBarMicrophone_ValueChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.setMicVolume(TrackBarMicrophone.Value);
        }

        private void ComboBoxMicrophones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
        }

        private void ComboBoxSpeakers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
        }

        private void btnClearListBox_Click(object sender, EventArgs e)
        {
            ListBoxSIPLog.Items.Clear();
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


            sdkLib = new PortSIPLib(this);

            //
            // Create and set the SIP callback handers, this MUST called before
            // _sdkLib.initialize();
            //
            sdkLib.createCallbackHandlers();

            string logFilePath = "d:\\"; // The log file path, you can change it - the folder MUST exists
            string agent = "PortSIP VoIP SDK";
            string stunServer = TextBoxStunServer.Text;

            // Initialize the SDK
            int rt = sdkLib.initialize(transportType,
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
                sdkLib.releaseCallbackHandlers();
                MessageBox.Show("Initialize failure.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ListBoxSIPLog.Items.Add("Initialized.");
            sIPInited = true;

            loadDevices();

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
            rt = sdkLib.setUser(userName, displayName, authName, password, sipDomain, sipServer, SIPServerPort, stunServer, StunServerPort, outboundServer, outboundServerPort);
            if (rt != 0)
            {
                sdkLib.unInitialize();
                sdkLib.releaseCallbackHandlers();
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

            string licenseKey = "1WINqx00QjlERUMwMkI1OUMzNUI2OTdENjI2MDA2RDNFRjU2REBCQkRCNzI1NEE4MDVDRjUwNzQ1NDAxRkIyRkQzMTNFREA4MEE3RkYzMjkwOTJBMkJFM0MzMzBGOTY3MzRGRDBBM0A5NEQwNDFEQzE2OTUyNjhGRjBBRkE2QzE4QTQ1N0I2NA";
            rt = sdkLib.setLicenseKey(licenseKey);
            if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
            {
                MessageBox.Show("This sample was built base on evaluation PortSIP VoIP SDK, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: sales@portsip.com to purchase the official version.");
            }
            else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
            {
                MessageBox.Show("The wrong license key was detected, please check with sales@portsip.com or support@portsip.com");
            }

            UpdateAudioCodecs();

            InitSettings();
            updatePrackSetting();

            if (checkBoxNeedRegister.Checked)
            {
                rt = sdkLib.registerServer(120, 0);
                if (rt != 0)
                {
                    sdkLib.removeUser();
                    sIPInited = false;
                    sdkLib.unInitialize();
                    sdkLib.releaseCallbackHandlers();

                    ListBoxSIPLog.Items.Clear();

                    MessageBox.Show("register to server failed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }


                ListBoxSIPLog.Items.Add("Registering...");
            }
        }

        public void Disconnect()
        {
            if (sIPInited == false)
            {
                return;
            }

            deRegisterFromServer();
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

        private void btnReconnect_Click(object sender, EventArgs e)
        {
            ListBoxSIPLog.Items.Clear();
            ListBoxSIPLog.Items.Add("Reconectando el softphone");
            Disconnect();
            Connect();
        }
        #endregion

        #region Metodos para darle animación a los botones del softphone
        private void btn1_MouseDown(object sender, MouseEventArgs e)
        {
            btn1.BackgroundImage = Properties.Resources._1_presionado;
        }

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

        private void btn6_MouseDown(object sender, MouseEventArgs e)
        {
            btn6.BackgroundImage = Properties.Resources._6_presionado;
        }

        private void btn6_MouseUp(object sender, MouseEventArgs e)
        {
            btn6.BackgroundImage = Properties.Resources._6;
        }

        private void btn7_MouseDown(object sender, MouseEventArgs e)
        {
            btn7.BackgroundImage = Properties.Resources._7_presionado;
        }

        private void btn7_MouseUp(object sender, MouseEventArgs e)
        {
            btn7.BackgroundImage = Properties.Resources._7;
        }

        private void btn8_MouseDown(object sender, MouseEventArgs e)
        {
            btn8.BackgroundImage = Properties.Resources._8_presionado;
        }

        private void btn8_MouseUp(object sender, MouseEventArgs e)
        {
            btn8.BackgroundImage = Properties.Resources._8;
        }

        private void btn9_MouseDown(object sender, MouseEventArgs e)
        {
            btn9.BackgroundImage = Properties.Resources._9_presionado;
        }

        private void btn9_MouseUp(object sender, MouseEventArgs e)
        {
            btn9.BackgroundImage = Properties.Resources._9;
        }

        private void btnAsterisco_MouseDown(object sender, MouseEventArgs e)
        {
            btnAsterisco.BackgroundImage = Properties.Resources.asterisco_presionado;
        }

        private void btnAsterisco_MouseUp(object sender, MouseEventArgs e)
        {
            btnAsterisco.BackgroundImage = Properties.Resources.asterisco;
        }

        private void btn0_MouseDown(object sender, MouseEventArgs e)
        {
            btn0.BackgroundImage = Properties.Resources._0_presionado;
        }

        private void btn0_MouseUp(object sender, MouseEventArgs e)
        {
            btn0.BackgroundImage = Properties.Resources._0;
        }

        private void btnGato_MouseDown(object sender, MouseEventArgs e)
        {
            btnGato.BackgroundImage = Properties.Resources.gato_presionado;
        }

        private void btnGato_MouseUp(object sender, MouseEventArgs e)
        {
            btnGato.BackgroundImage = Properties.Resources.gato;
        }

        private void btnUnHold_MouseDown(object sender, MouseEventArgs e)
        {
            btnUnHold.BackgroundImage = Properties.Resources.hold_hover;
        }

        private void btnUnHold_MouseUp(object sender, MouseEventArgs e)
        {
            btnUnHold.BackgroundImage = Properties.Resources.hold;
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

        private void btnTransfer_MouseDown(object sender, MouseEventArgs e)
        {
            btnTransfer.BackgroundImage = Properties.Resources.interambiar_presionado;
        }

        private void btnTransfer_MouseUp(object sender, MouseEventArgs e)
        {
            btnTransfer.BackgroundImage = Properties.Resources.intercambiar;
        }

        private void btnClearListBox_MouseDown(object sender, MouseEventArgs e)
        {
            btnClearListBox.BackgroundImage = Properties.Resources.backspace_hover;
        }

        private void btnClearListBox_MouseUp(object sender, MouseEventArgs e)
        {
            btnClearListBox.BackgroundImage = Properties.Resources.backspace;
        }
        #endregion

        #region Delegados para que portSip funcione
        /// <summary>
        ///  With below all onXXX functions, you MUST use the Invoke/BeginInvoke method if you want
        ///  modify any control on the Forms.
        ///  More details please visit: http://msdn.microsoft.com/en-us/library/ms171728.aspx
        ///  The Invoke method is recommended.
        ///  
        ///  if you don't like Invoke/BeginInvoke method, then  you can add this line to Form_Load:
        ///  Control.CheckForIllegalCrossThreadCalls = false;
        ///  This requires .NET 2.0 or higher
        /// 
        /// </summary>
        /// 
        public Int32 onRegisterSuccess(String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            // use the Invoke method to modify the control.
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add("Registration succeeded");
            }));

            sIPLogined = true;

            return 0;
        }


        public Int32 onRegisterFailure(String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add("Registration failure");
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
                sdkLib.rejectCall(sessionId, 486);
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
            //        sdkLib.setRemoteVideoWindow(_CallSessions[index].getSessionId(), remoteVideoPanel.Handle);
            //    }));
            //}

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


                sdkLib.answerCall(_CallSessions[index].getSessionId(), answerVideo);

                Text = "Line " + index.ToString();
                Text = Text + ": Answered call by Auto answer";

                ListBoxSIPLog.Invoke(new MethodInvoker(delegate
                {
                    ListBoxSIPLog.Items.Add(Text);
                }));
                rh.SendCall("0");
                //StartCallRecord();
                return 0;
            }

            Text = "Line " + index.ToString();
            Text = Text + ": Call incoming from ";
            Text = Text + callerDisplayName;
            Text = Text + "<";
            Text = Text + caller;
            Text = Text + ">";


            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            //  You should write your own code to play the wav file here for alert the incoming call(incoming tone);

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
            Text = Text + ": Call is trying...";

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
            Text = Text + ": Call session progress.";

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
            Text = Text + ": Ringing...";

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
            Text = Text + ": Call established";
            rh.SendCall("1");
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
                    sdkLib.unHold(_CallSessions[index].getOriginCallSessionId());
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
            Text = Text + ": Call is updated";
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
            Text = Text + ": Call is connected";
            //rh.SendCall();
            //Task task = new Task(StartCallRecord);
            //task.Wait = 100;
            //StartCallRecord();
            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));

            return 0;
        }


        public Int32 onInviteBeginingForward(String forwardTo)
        {
            string Text = "An incoming call was forwarded to: ";
            Text = Text + forwardTo;

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
            Text = Text + ": Call closed";

            ListBoxSIPLog.Invoke(new MethodInvoker(delegate
            {
                ListBoxSIPLog.Items.Add(Text);
            }));
            EndCallRecord();
            CallTypification typification = new CallTypification();
            typification.ShowDialog();
            typification.StartPosition = FormStartPosition.CenterParent;

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
            Text = Text + ": Placed on hold by remote.";

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
            Text = Text + ": Take off hold by remote.";

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
                sdkLib.rejectRefer(referId);
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
            int referSessionId = sdkLib.acceptRefer(referId, referSipMessage.ToString());
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
                sdkLib.hangUp(_CallSessions[index].getSessionId());
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
            Text = Text + ": Transfer Trying";

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
            Text = Text + ": Transfer Ringing";

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
            sdkLib.hangUp(_CallSessions[i].getSessionId());
            _CallSessions[i].reset();

            string Text = "Line " + i.ToString();
            Text = Text + ": Transfer succeeded, call closed.";

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
            Text = Text + ": Transfer failure";

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

        private async void btnBeginRecord_Click(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                MessageBox.Show("Please initialize the SDK first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string callId = await rh.GetIdCall();
            //MsjApi jsonData = JsonConvert.DeserializeObject<MsjApi>(callId);
            //var data = jsonData.data;
            var jobject = JsonConvert.DeserializeObject<JObject>(callId);

            var id = jobject.Value<string>("data").ToString();

            string filePath = "C:/Users/KODE/Documents/llamadas";
            string fileName = $"grabacion-{id}";

            AUDIO_RECORDING_FILEFORMAT audioRecordFileFormat = AUDIO_RECORDING_FILEFORMAT.FILEFORMAT_WAVE;

            //start recording
            int rt = sdkLib.startRecord(_CallSessions[currentlyLine].getSessionId(), filePath, fileName, false, audioRecordFileFormat, RECORD_MODE.RECORD_BOTH, VIDEOCODEC_TYPE.VIDEO_CODEC_H264, RECORD_MODE.RECORD_RECV);

            if(rt != 0)
            {
                MessageBox.Show("Fallo la grabacion de la conversación.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MessageBox.Show("Se inicio la grabación de la llamada.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            btnBeginRecord.Text = "Grabando...";
            btnBeginRecord.Enabled = false;
        }

        private void btnEndRecord_Click(object sender, EventArgs e)
        {
            if(sIPInited == false)
            {
                return;
            }
            sdkLib.stopRecord(_CallSessions[currentlyLine].getSessionId());
            MessageBox.Show("Se detuvo la grabación de la llamada.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void checkBoxAEC_CheckedChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.enableAEC(checkBoxAEC.Checked);
        }

        private void checkBoxCNG_CheckedChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.enableCNG(checkBoxCNG.Checked);
        }

        private void checkBoxAGC_CheckedChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.enableAGC(checkBoxAGC.Checked);
        }

        private void checkBoxANS_CheckedChanged(object sender, EventArgs e)
        {
            if (sIPInited == false)
            {
                return;
            }

            sdkLib.enableANS(checkBoxANS.Checked);
        }
    }
}
