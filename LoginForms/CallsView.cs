using System;
using System.Windows.Forms;
using Ozeki.VoIP;
using Ozeki.Media;
using Ozeki.Network;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using LoginForms.Shared;
using System.IO;

namespace LoginForms
{
    public partial class CallsView : Form
    {
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


        public CallsView()
        {
            InitializeComponent();
        }

        private void InitializeSoftPhone()
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
            try
            {
                InvokeGUIThread(() => { lb_log.Items.Clear(); });
                if (rdUDP.Checked == true)
                {
                    var userAgent = "Omnicanal-Softphone";
                    softPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 58000, 62000, userAgent);
                    InvokeGUIThread(() => { lb_log.Items.Add("Softphone created!"); });

                    softPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(softPhone_inComingCall);


                    sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, port, proxy);
                    //sIPAccount = new SIPAccount(true, "5003","5003","5003","5003", "192.168.1.140", 5060);
                    InvokeGUIThread(() =>
                    {
                        lb_log.Items.Add($"SIP Account created - {sIPAccount.DisplayName}");
                        lb_log.Items.Add($"User Name - {sIPAccount.UserName}");
                        lb_log.Items.Add($"Ip Address - {sIPAccount.DomainServerHost}");
                        lb_log.Items.Add($"Server Port - {sIPAccount.DomainServerPort}");

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
            }
            catch (Exception ex)
            {
                InvokeGUIThread(() => { lb_log.Items.Add(ex.Message); });
            }
        }

        private void rdTLS_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                InvokeGUIThread(() => { lb_log.Items.Clear(); });
                if (rdTLS.Checked == true)
                {
                    var userAgent = "Omnicanal-Softphone";
                    softPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 58000, 62000, userAgent);
                    InvokeGUIThread(() => { lb_log.Items.Add("Softphone created!"); });

                    softPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(softPhone_inComingCall);


                    //sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, port, proxy);
                    sIPAccount = new SIPAccount(requiredRegister, displayName, userName, registerName, password, domain, 5060, proxy);
                    InvokeGUIThread(() =>
                    {
                        lb_log.Items.Add($"SIP Account created - {sIPAccount.DisplayName}");
                        lb_log.Items.Add($"User Name - {sIPAccount.UserName}");
                        lb_log.Items.Add($"Ip Address - {sIPAccount.DomainServerHost}");
                        lb_log.Items.Add($"Server Port - {sIPAccount.DomainServerPort}");

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
            }
            catch (Exception ex)
            {
                InvokeGUIThread(() => { lb_log.Items.Add(ex.Message); });
            }
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
            var btn = sender as Button;

            if (call != null) { return; }

            if (btn == null) return;

            txtDisplay.Text += btn.Text.Trim();
        }

        private void btnResponse_Click(object sender, EventArgs e)
        {
            try
            {
                if (inComingCall)
                {
                    inComingCall = false;
                    call.Answer();

                    InvokeGUIThread(() => { lb_log.Items.Add("Call accepted."); });
                    return;
                }

                if (call != null)
                {
                    return;
                }

                if (phoneLineInformation != RegState.RegistrationSucceeded)
                {
                    InvokeGUIThread(() => { lb_log.Items.Add("Registratin Failed!"); txtDisplay.Text = "OFFLINE"; });
                    return;
                }
                reDialNumber = txtDisplay.Text;
                call = softPhone.CreateCallObject(phoneLine, txtDisplay.Text);

                WireUpCallEvents();
                call.Start();
            }
            catch (Exception ex)
            {
                InvokeGUIThread(() => { lb_log.Items.Add(ex.Message); });
            }
        }

        private void btnHangUp_Click(object sender, EventArgs e)
        {
            if (call != null)
            {
                if (inComingCall && call.CallState == CallState.Ringing)
                {
                    call.Reject();
                    InvokeGUIThread(() => { lb_log.Items.Add("Call rejected."); });
                }
                else
                {
                    call.HangUp();
                    inComingCall = false;
                    InvokeGUIThread(() => { lb_log.Items.Add("Call hanged up."); });
                }
                call = null;
            }

            txtDisplay.Text = string.Empty;
        }

        private void btnRedial_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(reDialNumber))
                return;

            if (call != null)
                return;

            call = softPhone.CreateCallObject(phoneLine, reDialNumber);
            WireUpCallEvents();
            call.Start();
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            if (call == null)
                return;

            if (!localHeld)
            {
                call.Hold();
                btnHold.Text = "Unhold";
                localHeld = true;
            }
            else
            {
                btnHold.Text = "Hold";
                localHeld = false;
                call.Unhold();
            }
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
            //if(call.CallState == CallState.InCall) 
            //{ 
            string transfer = "15481";
            if (call == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(transfer))
            {
                return;
            }
            call.BlindTransfer(transfer);
            lb_log.Items.Add($"Transfiriendo a: {transfer}");
            //}

            //else
            //{
            //    lb_log.Items.Add($"No se pudo transferir a la extension");
            //}
        }
    }
}
