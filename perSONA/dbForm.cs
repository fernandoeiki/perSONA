﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VA;

namespace perSONA
{
    public partial class dbForm : Form
    {
        private readonly IvAInterface vAInterface;
        string speechFolder;
        VANet vA;


        public dbForm(IvAInterface idbInterface)
        {
                InitializeComponent();
                this.vAInterface = idbInterface;
        }

        private void PlayAudio_Click(object sender, EventArgs e)
        {
            vA = vAInterface.getVa();

            vA.Reset();
            int receiverId = vA.CreateSoundReceiver("Subject");

            double xSides = 0;
            double zFront = 0;
            double yHeight = 1.7;


            VAVec3 receiverPosition = new VAVec3(xSides, yHeight, zFront);
            VAVec3 receiverOrientationV = new VAVec3(0, 0, -1);
            VAVec3 receiverOrientationU = new VAVec3(0, 1, 0);

            vA.SetSoundReceiverPosition(receiverId, receiverPosition);
            vA.SetSoundReceiverOrientationVU(receiverId, receiverOrientationV, receiverOrientationU);
            vAInterface.concatText(string.Format("\r\nCreated Receiver: {3} at position: {0},{1},{2}, looking forward ",
                                     xSides, zFront, yHeight, receiverId));

            int hrirId = vA.CreateDirectivityFromFile("data/ITA_Artificial_Head_5x5_44kHz_128.v17.ir.daff");
            vA.SetSoundReceiverDirectivity(receiverId, hrirId);

            string speechFile = System.IO.Path.Combine(speechFolder, listBox1.GetItemText(listBox1.SelectedItem));
            vAInterface.concatText(speechFile);

            string noiseFile = "data/Sounds/Noise/4talker-babble_ISTS.wav";
            vAInterface.createAcousticScene(speechFile, noiseFile);

            vAInterface.playScene(2, 0, 50);
        }

        private void SelectList_Click(object sender, EventArgs e)
        {
            speechFolder = vAInterface.getDatabaseFolder();
            string[] filePaths = System.IO.Directory.GetFiles(@speechFolder, "*.wav");
            string[] fileNames = filePaths.Select(System.IO.Path.GetFileName).ToArray();
            listBox1.DataSource = fileNames;
            vAInterface.concatText(speechFolder);
            PlayAudio.Enabled = true;
            SaveChanges.Enabled = true;
            Preview.Enabled = true;

        }

        private void SaveChanges_Click(object sender, EventArgs e)
        {
            saveTag();
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text;

            updateWordsFromTag(title);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string speechFile = System.IO.Path.Combine(speechFolder, listBox1.GetItemText(listBox1.SelectedItem));
            vAInterface.concatText(speechFile);

            string noiseFile = "data/Sounds/Noise/4talker-babble_ISTS.wav";
            vAInterface.createAcousticScene(speechFile, noiseFile);
            string title = vAInterface.getTitle(speechFile);
            updateWordsFromTag(title);
        }   

        private void saveTag()
        {
            string speechFile = System.IO.Path.Combine(speechFolder, listBox1.GetItemText(listBox1.SelectedItem));

            TagLib.File tagFile = TagLib.File.Create(speechFile);
            tagFile.Tag.Title = textBox1.Text;
            tagFile.Save();
        }

        private void updateWordsFromTag(string title)
        {            
            if (!string.IsNullOrEmpty(title))
            {
                textBox1.Text = title;
                string[] words = title.Split(null);
                listBox2.DataSource = words;
                listBox2.ClearSelected();
            }
            else
            {
                textBox1.Text = "";
                listBox2.DataSource = "Digite Texto na Caixa ao lado".Split(null);
                listBox2.ClearSelected();
            }
        }
    }
}
