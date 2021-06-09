using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Ozeki.VoIP;
using Ozeki.Media;

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

        private bool inComingCall;

        private string reDialNumber;

        private bool localHeld;


        public CallsView()
        {
            InitializeComponent();
        }


        private void InitializeSoftPhone()
        {
            try
            {
                softPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 58000, 62000);
                InvokeGUIThread(() => { lb_log.Items.Add("Softphone created!"); });

                softPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(softPhone_inComingCall);

                //SIPAccount sIPAccount = new SIPAccount(true, "2000", "2000", "2000", "2000", "192.168.1.140", 5060);
                SIPAccount sIPAccount = new SIPAccount(true, "29495", "29495", "29495", "IbZ09uh2WDjJ50lv8WPs", "producto.enlacetpe.mx", 5060, "187.188.103.230");
                //SIPAccount sIPAccount = new SIPAccount(register.isRequired, register.displayName, register.userName, register.registerName, register.password, register.domain, register.port, register.proxy);
                InvokeGUIThread(() =>
                {
                    lb_log.Items.Add($"SIP Account created - {sIPAccount.DisplayName}");
                    lb_log.Items.Add($"User Name - {sIPAccount.UserName}");
                    lb_log.Items.Add($"Ip Address - {sIPAccount.DomainServerHost}");
                    lb_log.Items.Add($"Server Port - {sIPAccount.DomainServerPort}");

                });

                phoneLine = softPhone.CreatePhoneLine(sIPAccount);
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
                InvokeGUIThread(() => { lb_log.Items.Add("Local IP error!"); });
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
        }



        private void phoneLine_PhoneLineInformation(object sender, RegistrationStateChangedArgs e)
        {
            phoneLineInformation = e.State;

            InvokeGUIThread(() =>
            {
                if (phoneLineInformation == RegState.RegistrationSucceeded)
                {
                    lb_log.Items.Add("Registration succeeded - Online");
                }
                else
                {
                    lb_log.Items.Add("Not registered - Offline: " + phoneLineInformation.ToString());
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
    }
}
