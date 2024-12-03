using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SpeechServiceApp.Controllers
{
    public class SpeechController : Controller
    {
        private const string SubscriptionKey = "abcd1234efgh5678ijkl9012mnop3456"; 
        private const string Region = "eastus";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecognizeSpeech()
        {
            string recognizedText = string.Empty;

            try
            {
                var config = SpeechConfig.FromSubscription(SubscriptionKey, Region);

                var audioFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "audio", "sample.wav");

                using var audioInput = AudioConfig.FromWavFileInput(audioFilePath);
                using var recognizer = new SpeechRecognizer(config, audioInput);
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    recognizedText = result.Text;
                }
                else
                {
                    recognizedText = "Speech could not be recognized.";
                }
            }
            catch (Exception ex)
            {
                recognizedText = $"Error: {ex.Message}";
            }

            ViewBag.RecognizedText = recognizedText;
            return View("Index");
        }
    }
}
