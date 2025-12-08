/******************************************************************
 	NomeFile : StandOrdini/SpeechSynth.cs
    Data	 : 03.12.2025
 	Autore   : Mauro Artuso

 ******************************************************************/

using System;
using System.Speech.Synthesis;
using System.Windows.Forms;

using static StandCommonFiles.CommonCl;
using static StandCommonFiles.LogServer;

using static StandFacile.Define;

namespace StandFacile
{
    /// <summary>
    /// classe di sintesi vocale
    /// </summary>
    public partial class SpeechSynth : Form
    {
        /// <summary>riferimento alla classe di sintesi vocale/// </summary>
        public static SpeechSynth rSpeechSynth;

        SpeechSynthesizer _synth = new SpeechSynthesizer();

        /// <summary>Ottiene il flag di sintesi vocale abilitata/// </summary>
        public bool GetVoiceSynthEnabled()
        {
            return ckBox_VoiceSynthEnabled.Checked;
        }

        /// <summary>
        /// costruttore della classe di sintesi vocale
        /// </summary>
        public SpeechSynth()
        {
            InitializeComponent();

            rSpeechSynth = this;
            Hide();
        }

        /// <summary>
        /// inizializza la lista delle voci disponibili
        /// </summary>
        public void Init(bool bShow)
        {
            comboBox.Items.Clear();

            foreach (InstalledVoice voice in _synth.GetInstalledVoices())
            {
                String sTmp;
                VoiceInfo info = voice.VoiceInfo;

                sTmp = String.Format("SpeechSynth: {0} {1} {2}", info.Name, info.Age, info.Gender);
                LogToFile(sTmp, true);

                comboBox.Items.Add(info.Name);
            }

            comboBox.Text = _synth.Voice.Name;


            try
            {
                ckBox_VoiceSynthEnabled.Checked = (ReadRegistry(VOICE_SYNTH_KEY, 0) == 1);
                comboBox.SelectedIndex = ReadRegistry(VOICE_SYNTH_NAME, 0);

                volTrackBar.Value = ReadRegistry(VOICE_SYNTH_VOL, 50);

                textBox.Text = ReadRegistry(VOICE_SYNTH_TEXT, "serviamo il numero #");
            }
            catch
            {
                ckBox_VoiceSynthEnabled.Checked = false;
                comboBox.SelectedIndex = 0;
                volTrackBar.Value = 50;
                textBox.Text = "serviamo il numero #";
            }

            if (bShow)
                ShowDialog();
        }

        /// <summary>
        /// funzione per la riproduzione vocale di un testo con sostituzione del token # <br/>
        /// aggiunge anche una pausa iniziale 
        /// </summary>
        public void TextSpeak(String sTextParam)
        {
            String sTextToPlay = textBox.Text.Replace("#", ", " + sTextParam);

            if (!ckBox_VoiceSynthEnabled.Checked)
                return;

            _synth.Volume = volTrackBar.Value;  // 20..100
            _synth.SelectVoice(comboBox.Text);

            // non blocca il thread chiamante
            _synth.SpeakAsync(sTextToPlay);
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            String sTextToPlay = textBox.Text.Replace("#", "10");

            _synth.Volume = volTrackBar.Value;  // 20..100
            _synth.SelectVoice(comboBox.Text);

            _synth.Speak(sTextToPlay);
        }

        /// <summary>
        /// salva le impostazioni di sintesi vocale nel registry
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            WriteRegistry(VOICE_SYNTH_KEY, ckBox_VoiceSynthEnabled.Checked ? 1 : 0);

            WriteRegistry(VOICE_SYNTH_NAME, comboBox.SelectedIndex);

            WriteRegistry(VOICE_SYNTH_VOL, volTrackBar.Value);

            WriteRegistry(VOICE_SYNTH_TEXT, textBox.Text);

            LogToFile("SpeechSynth : OKBtnClick");

            Hide();
        }


        private void SpeechSynth_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
