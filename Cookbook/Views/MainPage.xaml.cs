using Cookbook.Models;
using System;
using Cookbook.Services;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechSynthesis;

namespace Cookbook.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }


        public async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // The media object for controlling and playing audio.
            MediaElement mediaElement = new MediaElement();

            // The object for controlling the speech synthesis engine (voice).
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();

            // Generate the audio stream from plain text.
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Hello World");

            // Send the stream to the media object.
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
}
