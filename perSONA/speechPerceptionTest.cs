﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace perSONA
{
    public class speechPerceptionTest
    {
        public double FinalPercentage { get; set; }
        public string Label { get; set; }
        public double SignalToNoise { get; set; }
        public string SpeechFolder { get; set; }
        public string SpeechDirection { get; set; } = null;
        public string NoiseDirection { get; set; } = null;
        public string NoiseFile { get; set; }
        public double AngleSpeech { get; set; }
        public double RadiusSpeech { get; set; }
        public double AngleNoise { get; set; }
        public double RadiusNoise { get; set; }
        public double MeanSRT { get; set; }
        public double[] PresentingLogic { get; set; } = { 2, 1 };
        public double[] IterativeSNR { get; set; } = { };
        public double AcceptanceRule { get; set; } = 0.5;
        public double SignalToNoiseStep { get; set; } = 4;
        public DateTime TestStart { get; set; }
        public string Applicator { get; set; } = null;
        public string PatientName { get; set; } = null;
        public string TotalDuration { get; set; } = "0";
        public string[] IterativeDuration { get; set; } = { };
        public string[] IterativePercentage { get; set; } = { };

        public override string ToString()
        {

            string testString;



            testString = string.Format( "{0} - {1} - Carried by: {11}\r\n Mean SRT: {12}" +
                                        "Speech Folder: {2} Angle:{3}\r\n " +
                                        "Noise Folder:{4} Angle:{5}\r\n " +
                                        "Logic: {6}-down-{7}-up, Criteria:{8}%, SNR:{9}dB SNR step:{10}dB",
                                        Label, TestStart.ToShortDateString(), SpeechFolder, AngleSpeech, NoiseFile, AngleNoise,
                                        PresentingLogic[0], PresentingLogic[1], AcceptanceRule * 100, SignalToNoise, SignalToNoiseStep,
                                        Applicator, MeanSRT);


            return testString;
        }

        public speechPerceptionTest()
        {
        }

        public string testSummary()
        {
            if(AngleNoise == 0)
            {
                 NoiseDirection = "Frente";
            }
            else if (AngleNoise > 0)
            {
                NoiseDirection = "Direita";
            }
            else
            {
                NoiseDirection = "Esquerda";
            }

            if (AngleSpeech == 0)
            {
                NoiseDirection = "Frente";
            }
            else if (AngleSpeech > 0)
            {
                NoiseDirection = "Direita";
            }
            else
            {
                SpeechDirection = "Esquerda";
            }
            
            return string.Format("Ensaio: {0} - Data: {1}\r\n" +
                                 "Direção da fala: {2} \r\n" +
                                 "Direção do ruído: {3} \r\n" +
                                 "Distancia da fonte sonora da fala: {10}m \r\n" +
                                 "Distancia da fonte sonora do ruído: {11}m \r\n" +
                                 "Lista de falas: {4} \r\n" +
                                 "Tipo de teste: {7}-down-{8}-up \r\n" +
                                 "Tipo de Ruído: {5} \r\n" +
                                 "Porcentagem de acerto: {9}% \r\n" +
                                 "Aplicador: {6}", Label, TestStart.ToShortDateString(),
                                 SpeechDirection, NoiseDirection, SpeechFolder, NoiseFile,
                                 Applicator, PresentingLogic[0], PresentingLogic[1], FinalPercentage, RadiusSpeech, RadiusNoise);
        }

        public speechPerceptionTest(
                                    double angleSpeech, double radiusSpeech, 
                                    double angleNoise, double radiusNoise,
                                    string speechFolder, string noiseFile,
                                    string label, double snr)
        {
            AngleSpeech = angleSpeech;
            RadiusSpeech = radiusSpeech;
            AngleNoise = angleNoise;
            RadiusNoise = radiusNoise;
            SignalToNoise = snr;
            SpeechFolder = speechFolder;
            NoiseFile = noiseFile;
            Label = label;
            TestStart = DateTime.Now;


        }

        public speechPerceptionTest(
                            double angleSpeech, double radiusSpeech,
                            double angleNoise, double radiusNoise,
                            string speechFolder, string noiseFile,
                            string label, double snr,
                            double[] presentingLogic,
                            double acceptanceRule, double signalToNoiseStep,
                            string applicator, string patientName)
        {
            Applicator = applicator;
            PatientName = patientName;
            AngleSpeech = angleSpeech;
            RadiusSpeech = radiusSpeech;
            AngleNoise = angleNoise;
            RadiusNoise = radiusNoise;
            SignalToNoise = snr;
            SpeechFolder = speechFolder;
            NoiseFile = noiseFile;
            Label = label;
            PresentingLogic = presentingLogic;
            AcceptanceRule = acceptanceRule;
            SignalToNoiseStep = signalToNoiseStep;
            TestStart = DateTime.Now;
        }

    }
}
