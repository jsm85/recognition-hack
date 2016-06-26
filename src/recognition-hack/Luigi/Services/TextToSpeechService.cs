using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Luigi.Models;

namespace Luigi.Services
{
    public class TextToSpeechService
    {
        /// <summary>
        /// This method is called once the audio returned from the service.
        /// It will then attempt to play that audio file.
        /// Note that the playback will fail if the output audio format is not pcm encoded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="GenericEventArgs{Stream}"/> instance containing the event data.</param>
        static void PlayAudio(object sender, GenericEventArgs<Stream> args)
        {
            Console.WriteLine(args.EventData);

            // For SoundPlayer to be able to play the wav file, it has to be encoded in PCM.
            // Use output audio format AudioOutputFormat.Riff16Khz16BitMonoPcm to do that.
            SoundPlayer player = new SoundPlayer(args.EventData);
            player.Play();
            args.EventData.Dispose();
        }

        /// <summary>
        /// Handler an error when a TTS request failed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GenericEventArgs{Exception}"/> instance containing the event data.</param>
        static void ErrorHandler(object sender, GenericEventArgs<Exception> e)
        {
            Console.WriteLine("Unable to complete the TTS request: [{0}]", e.ToString());
        }

        public static void Speak()
        {
            Console.WriteLine("Starting Authtentication");
            AccessTokenInfo token;

            // Note: Sign up at http://www.projectoxford.ai to get a subscription key.  Search for Speech APIs from Azure Marketplace.  Use the subscription key as Client secret below.
            Authentication.Authentication auth = new Authentication.Authentication("recog-hack-luigi", "2147d5958bdf498fb41026f202486331");

            try
            {
                token = auth.GetAccessToken();
                Console.WriteLine("Token: {0}\n", token.access_token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed authentication.");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
                return;
            }

            Console.WriteLine("Starting TTSSample request code execution.");

            string requestUri = "https://speech.platform.bing.com/synthesize";

            var cortana = new Synthesize(new Synthesize.InputOptions()
            {
                RequestUri = new Uri(requestUri),
                // Text to be spoken.
                Text = "BUONGIORNO, my name is Luigi, your personal chef. What ingredients do you have in your kitchen?",
                VoiceType = Gender.Male,
                // Refer to the documentation for complete list of supported locales.
                Locale = "it-IT",
                // You can also customize the output voice. Refer to the documentation to view the different
                // voices that the TTS service can output.
                VoiceName = "Microsoft Server Speech Text to Speech Voice (it-IT, Cosimo, Apollo)",
                // Service can return audio in different output format. 
                OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm,
                AuthorizationToken = "Bearer " + token.access_token,
            });

            cortana.OnAudioAvailable += PlayAudio;
            cortana.OnError += ErrorHandler;
            cortana.Speak(CancellationToken.None).Wait();
        }
    }

    /// <summary>
    /// Generic event args
    /// </summary>
    /// <typeparam name="T">Any type T</typeparam>
    public class GenericEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEventArgs{T}" /> class.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public GenericEventArgs(T eventData)
        {
            this.EventData = eventData;
        }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        public T EventData { get; private set; }
    }

    /// <summary>
    /// Gender of the voice.
    /// </summary>
    public enum Gender
    {
        Female,
        Male
    }

    /// <summary>
    /// Voice output formats.
    /// </summary>
    public enum AudioOutputFormat
    {
        /// <summary>
        /// raw-8khz-8bit-mono-mulaw request output audio format type.
        /// </summary>
        Raw8Khz8BitMonoMULaw,
        /// <summary>
        /// raw-16khz-16bit-mono-pcm request output audio format type.
        /// </summary>
        Raw16Khz16BitMonoPcm,
        /// <summary>
        /// riff-8khz-8bit-mono-mulaw request output audio format type.
        /// </summary>
        Riff8Khz8BitMonoMULaw,
        /// <summary>
        /// riff-16khz-16bit-mono-pcm request output audio format type.
        /// </summary>
        Riff16Khz16BitMonoPcm,
        /// <summary>
        /// ssml-16khz-16bit-mono-silk request output audio format type.
        /// It is a SSML with audio segment, with audio compressed by SILK codec
        /// </summary>
        Ssml16Khz16BitMonoSilk,
        /// <summary>
        /// ssml-16khz-16bit-mono-tts request output audio format type.
        /// It is a SSML with audio segment, and it needs tts engine to play out
        /// </summary>
        Ssml16Khz16BitMonoTts
    }

    /// <summary>
    /// Sample synthesize request
    /// </summary>
    public class Synthesize
    {
        /// <summary>
        /// The ssml template
        /// </summary>
        private const string SsmlTemplate = "<speak version='1.0' xml:lang='en-us'><voice xml:lang='{0}' xml:gender='{1}' name='{2}'>{3}</voice></speak>";

        /// <summary>
        /// The input options
        /// </summary>
        private InputOptions inputOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Synthesize"/> class.
        /// </summary>
        /// <param name="input">The input.</param>
        public Synthesize(InputOptions input)
        {
            this.inputOptions = input;
        }

        /// <summary>
        /// Called when a TTS request has been completed and audio is available.
        /// </summary>
        public event EventHandler<GenericEventArgs<Stream>> OnAudioAvailable;

        /// <summary>
        /// Called when an error has occured. e.g this could be an HTTP error.
        /// </summary>
        public event EventHandler<GenericEventArgs<Exception>> OnError;

        /// <summary>
        /// Sends the specified text to be spoken to the TTS service and saves the response audio to a file.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task</returns>
        public Task Speak(CancellationToken cancellationToken)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var client = new HttpClient(handler);

            foreach (var header in this.inputOptions.Headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }

            var genderValue = "";
            switch (this.inputOptions.VoiceType)
            {
                case Gender.Male:
                    genderValue = "Male";
                    break;
                case Gender.Female:
                default:
                    genderValue = "Female";
                    break;

            }

            var request = new HttpRequestMessage(HttpMethod.Post, this.inputOptions.RequestUri)
            {
                Content = new StringContent(String.Format(SsmlTemplate, this.inputOptions.Locale, genderValue, this.inputOptions.VoiceName, this.inputOptions.Text))
            };

            var httpTask = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            Console.WriteLine("Response status code: [{0}]", httpTask.Result.StatusCode);

            var saveTask = httpTask.ContinueWith(
                async (responseMessage, token) =>
                {
                    try
                    {
                        if (responseMessage.IsCompleted && responseMessage.Result != null && responseMessage.Result.IsSuccessStatusCode)
                        {

                            var httpStream = await responseMessage.Result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            this.AudioAvailable(new GenericEventArgs<Stream>(httpStream));
                        }
                        else
                        {
                            this.Error(new GenericEventArgs<Exception>(new Exception(String.Format("Service returned {0}", responseMessage.Result.StatusCode))));
                        }
                    }
                    catch (Exception e)
                    {
                        this.Error(new GenericEventArgs<Exception>(e.GetBaseException()));
                    }
                    finally
                    {
                        responseMessage.Dispose();
                        request.Dispose();
                        client.Dispose();
                        handler.Dispose();
                    }
                },
                TaskContinuationOptions.AttachedToParent,
                cancellationToken);

            return saveTask;
        }

        /// <summary>
        /// Called when a TTS requst has been successfully completed and audio is available.
        /// </summary>
        private void AudioAvailable(GenericEventArgs<Stream> e)
        {
            EventHandler<GenericEventArgs<Stream>> handler = this.OnAudioAvailable;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Error handler function
        /// </summary>
        /// <param name="e">The exception</param>
        private void Error(GenericEventArgs<Exception> e)
        {
            EventHandler<GenericEventArgs<Exception>> handler = this.OnError;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Inputs Options for the TTS Service.
        /// </summary>
        public class InputOptions
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Input"/> class.
            /// </summary>
            public InputOptions()
            {
                this.Locale = "en-us";
                this.VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, Kate)";
                // Default to Riff16Khz16BitMonoPcm output format.
                this.OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm;
            }

            /// <summary>
            /// Gets or sets the request URI.
            /// </summary>
            public Uri RequestUri { get; set; }

            /// <summary>
            /// Gets or sets the audio output format.
            /// </summary>
            public AudioOutputFormat OutputFormat { get; set; }

            /// <summary>
            /// Gets or sets the headers.
            /// </summary>
            public IEnumerable<KeyValuePair<string, string>> Headers
            {
                get
                {
                    List<KeyValuePair<string, string>> toReturn = new List<KeyValuePair<string, string>>();
                    toReturn.Add(new KeyValuePair<string, string>("Content-Type", "application/ssml+xml"));

                    string outputFormat;

                    switch (this.OutputFormat)
                    {
                        case AudioOutputFormat.Raw16Khz16BitMonoPcm:
                            outputFormat = "raw-16khz-16bit-mono-pcm";
                            break;
                        case AudioOutputFormat.Raw8Khz8BitMonoMULaw:
                            outputFormat = "raw-8khz-8bit-mono-mulaw";
                            break;
                        case AudioOutputFormat.Riff16Khz16BitMonoPcm:
                            outputFormat = "riff-16khz-16bit-mono-pcm";
                            break;
                        case AudioOutputFormat.Riff8Khz8BitMonoMULaw:
                            outputFormat = "riff-8khz-8bit-mono-mulaw";
                            break;
                        case AudioOutputFormat.Ssml16Khz16BitMonoSilk:
                            outputFormat = "ssml-16khz-16bit-mono-silk";
                            break;
                        case AudioOutputFormat.Ssml16Khz16BitMonoTts:
                            outputFormat = "ssml-16khz-16bit-mono-tts";
                            break;
                        default:
                            outputFormat = "riff-16khz-16bit-mono-pcm";
                            break;
                    }

                    toReturn.Add(new KeyValuePair<string, string>("X-Microsoft-OutputFormat", outputFormat));
                    // authorization Header
                    toReturn.Add(new KeyValuePair<string, string>("Authorization", this.AuthorizationToken));
                    // Refer to the doc
                    toReturn.Add(new KeyValuePair<string, string>("X-Search-AppId", "07D3234E49CE426DAA29772419F436CA"));
                    // Refer to the doc
                    toReturn.Add(new KeyValuePair<string, string>("X-Search-ClientID", "1ECFAE91408841A480F00935DC390960"));
                    // The software originating the request
                    toReturn.Add(new KeyValuePair<string, string>("User-Agent", "TTSClient"));

                    return toReturn;
                }
                set
                {
                    Headers = value;
                }
            }

            /// <summary>
            /// Gets or sets the locale.
            /// </summary>
            public String Locale { get; set; }

            /// <summary>
            /// Gets or sets the type of the voice; male/female.
            /// </summary>
            public Gender VoiceType { get; set; }

            /// <summary>
            /// Gets or sets the name of the voice.
            /// </summary>
            public string VoiceName { get; set; }

            /// <summary>
            /// Authorization Token.
            /// </summary>
            public string AuthorizationToken { get; set; }

            /// <summary>
            /// Gets or sets the text.
            /// </summary>
            public string Text { get; set; }
        }
    }

}