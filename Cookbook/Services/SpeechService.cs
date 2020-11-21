using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace Cookbook.Services
{
    class SpeechService
    {
        /// <summary>
        /// Reading given expression out loud
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task readOutLoud(string expression)
        {
            // The media object for controlling and playing audio.
            MediaElement mediaElement = new MediaElement();

            // The object for controlling the speech synthesis engine (voice).
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();


            // Choosing the voice of reader. You can only use voices that you have installed on your Windows system.
            // To obtain new voices, go into Region & Country -> Speech -> More Voices
            // This part of the method looks for available English voices on the system, if not found, it uses your language's default voice
            // Default Hungarian voice: Microsoft Szabolcs 
            using (var speaker = new SpeechSynthesizer())
            {
                if ((SpeechSynthesizer.AllVoices.Any(x => x.Language.Contains("EN"))))
                {
                    speaker.Voice = (SpeechSynthesizer.AllVoices.First(x => x.Language.Contains("EN")));
                    synth.Voice = speaker.Voice;
                }
            }

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(expression);

            // Send the stream to the media object.
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
