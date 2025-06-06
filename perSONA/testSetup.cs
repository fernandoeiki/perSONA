﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace perSONA
{
    public partial class testSetup : Form
    {
        public speechPerceptionTest test;
        private readonly IvAInterface vAInterface;
        string speechFolder;
        string noiseFolder = "data/Sounds/Noise";
        string[] subjects;

        public testSetup(IvAInterface vAInterface, string testTipe, string[] subjects)
        {

            InitializeComponent();
            resizeScreen();

            this.subjects = subjects;
            applicatorLabel.Text = subjects[0];
            patientLabel.Text = subjects[1];

            this.vAInterface = vAInterface;

            comboBox3.DataSource = Directory.GetFiles(noiseFolder).Select(Path.GetFileName).ToArray();
            comboBox3.SelectedItem = comboBox3.Items.IndexOf("4talker-babble_ISTS.wav");
            string[] procedureList = { "2-down-1-up", "1-down-1-up" };
            comboBox1.DataSource = procedureList;
            comboBox1.SelectedItem = comboBox1.Items.IndexOf("2-down-1-up");
            string[] SpeechList = { "Alcaim1_/F", "Alcaim1_/M", "RASP", "Trainning" };
            speechFiles.DataSource = SpeechList;
            speechFiles.SelectedItem = speechFiles.Items.IndexOf("Trainning");
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());

            switch (testTipe)
            {
                case "Speech Right":
                    speechRight.Checked = true;
                    break;

                case "Speech Left":
                    speechLeft.Checked = true;
                    break;

                case "Speech Front":
                    speechFront.Checked = true;
                    break;

                default:
                    break;
            }

        }

        private void BeginTest_Click(object sender, EventArgs e)
        {
            double[] angles = getSceneAngles();
            double[] radius = getSceneDistances();
            double angleSpeech = checkDirection(speechLeft.Checked, speechFront.Checked, speechRight.Checked); ;
            double radiusSpeech = (double)speechDistance.Value;
            double angleNoise = checkDirection(noiseLeft.Checked, noiseFront.Checked, noiseRight.Checked); ;
            double radiusNoise = (double)noiseDistance.Value;
            double snr = (double)initialSnr.Value;
            string noiseFile = Path.Combine(noiseFolder, comboBox3.SelectedItem.ToString());

            string procedureString = (string)comboBox1.SelectedItem;

            double[] presentingLogic = { double.Parse(procedureString.Split('-')[0]), double.Parse(procedureString.Split('-')[2]) };
            double[] iterativeSNR = { };
            double acceptanceRule = (double)numericRule.Value;
            double signalToNoiseStep = (double)stepSnr.Value;
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string Location = Path.Combine("data", "Sounds", "Speech", speechFiles.GetItemText(speechFiles.SelectedItem), speechList.GetItemText(speechList.SelectedItem));
            speechFolder = vAInterface.getDatabaseFiles(Location);

            speechPerceptionTest speechTest = new speechPerceptionTest(
                                                    angleSpeech, radiusSpeech,
                                                    angleNoise, radiusNoise,
                                                    speechFolder, noiseFile,
                                                    textBox1.Text, snr,
                                                    presentingLogic,
                                                    acceptanceRule / 100, signalToNoiseStep,
                                                    subjects[0], subjects[1]);
            string testString = speechTest.ToString();
            vAInterface.concatText(testString);

            if (Application.OpenForms["speechIterTestForm"] == null)
            {
                new speechIterTestForm(speechTest, vAInterface).Show();
            }
        }

        private double checkDirection(bool left, bool front, bool right)
        {
            if (left)
            {
                return -90;
            }
            else if (front)
            {
                return 0;
            }
            else
            {
                return 90;
            }
        }

        private double[] getSceneAngles()
        {
            double angleSpeech = checkDirection(speechLeft.Checked, speechFront.Checked, speechRight.Checked);
            double angleNoise = checkDirection(noiseLeft.Checked, noiseFront.Checked, noiseRight.Checked);

            double[] angles = { angleSpeech, angleNoise };
            return angles;
        }

        private double[] getSceneDistances()
        {
            double radiusSpeech = (double)speechDistance.Value;
            double radiusNoise = (double)noiseDistance.Value;
            double[] radius = { radiusSpeech, radiusNoise };

            return radius;
        }

        private void speechLeft_CheckedChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void speechFront_CheckedChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void speechRight_CheckedChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void noiseLeft_CheckedChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void noiseFront_CheckedChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void noiseRight_CheckedChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void speechDistance_ValueChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }

        private void noiseDistance_ValueChanged(object sender, EventArgs e)
        {
            vAInterface.plotSceneGraph(zedGraphControl1, getSceneDistances(), getSceneAngles());
        }
        private void resizeScreen()
        {
            double PCResolutionWidth = Screen.PrimaryScreen.Bounds.Width;
            double PCResolutionHeight = Screen.PrimaryScreen.Bounds.Height;

            double formWidth = this.Size.Width;
            double formHeight = this.Size.Height;

            if ((formWidth > PCResolutionWidth) | (formHeight > PCResolutionHeight * 0.925))
            {
                int newWidth = Convert.ToInt32(PCResolutionWidth * 0.54);
                int newHeight = Convert.ToInt32(PCResolutionHeight * 0.875);
                this.Size = new Size(newWidth, newHeight);
            }
        }

        private void speechFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SpeechFile = speechFiles.GetItemText(speechFiles.SelectedItem);
            var filePaths = Path.Combine("data", "Sounds", "Speech", SpeechFile);
            string[] SpeechLists = Directory.GetDirectories(filePaths).Select(Path.GetFileName).ToArray();
            speechList.DataSource = SpeechLists;
        }
    }
}
