﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace perSONA
{
    public partial class calibrationSettingsA2 : Form
    {
        private readonly IvAInterface vAInterface;
        string speakerBrand;
        string speakerModel;

        public calibrationSettingsA2(IvAInterface vAInterface, string calibrationObjectBrand, string calibrationObjectModel)
        {
            InitializeComponent();
            this.vAInterface = vAInterface;
            speakerBrand = calibrationObjectBrand;
            speakerModel = calibrationObjectModel;

            filliPhoneModelBox();
            fillMicrophoneBrandBox();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IPhoneModelBox.Text) | string.IsNullOrWhiteSpace(IOSVersionBox.Text) | string.IsNullOrWhiteSpace(microphoneBrandBox.Text) | string.IsNullOrWhiteSpace(microphoneModelBox.Text))
            {
                MessageBox.Show("Adicione o modelo do IPhone, vesão do IOS e marca e modelo do microfone externo para continuar", "Erro");
            }
            else
            {
                calibrationData calibration = new calibrationData()
                {
                    CalibrationObjectBrand = speakerBrand,
                    CalibrationObjectModel = speakerModel,
                    IPhoneModel = IPhoneModelBox.Text,
                    IOSVersion = IOSVersionBox.Text,
                    MicrophoneBrand = microphoneBrandBox.Text,
                    MicrophoneModel = microphoneModelBox.Text,
                    MicrophoneSerialNumber = microphoneSerialNumberBox.Text,
                    LastCalibrationDate = lastCalibrationDateBox.Value,
                    NotCalibrate = notCalibrateCheckbox.Checked,
                    ApplicationName = applicationNameBox.Text,
                    ApplicationVersion = applicationVersionBox.Text,
                    CalibrationDateTime = DateTime.Now
                };
                string calibrationJson = Newtonsoft.Json.JsonConvert.SerializeObject(calibration);


                try
                {
                    File.WriteAllText(string.Format("{0}/CalibrationData/{1}.json",
                                                  Properties.Settings.Default.RESULTS_FOLDER,
                                                  "Calibration " + Properties.Settings.Default.CALIBRATION_ID), calibrationJson);

                }
                catch (DirectoryNotFoundException)
                {
                    string dir = string.Format("{0}/CalibrationData", Properties.Settings.Default.RESULTS_FOLDER);
                    Directory.CreateDirectory(dir);
                    File.WriteAllText(string.Format("{0}/CalibrationData/{1}.json",
                                        Properties.Settings.Default.RESULTS_FOLDER,
                                        "Calibration " + Properties.Settings.Default.CALIBRATION_ID), calibrationJson);
                }

                new calibrationHelp(vAInterface).Show();
                Close();
            }
        }

        private void filliPhoneModelBox()
        {
            var wb = new XLWorkbook(Properties.Settings.Default.EQUIPMENTS_TABLE_LOCATION);
            var Table = wb.Worksheet(4);

            var linha = 2;
            string previousCell = "";

            while (true)
            {
                var ModelColumnCell = Table.Cell("A" + linha.ToString()).Value.ToString();
                if (string.IsNullOrEmpty(ModelColumnCell)) break;
                if (previousCell != ModelColumnCell)
                {
                    IPhoneModelBox.Items.Add(ModelColumnCell);
                }
                linha++;
                previousCell = ModelColumnCell;
            }

            wb.Dispose();
        }
        private void fillMicrophoneBrandBox()
        {
            var wb = new XLWorkbook(Properties.Settings.Default.EQUIPMENTS_TABLE_LOCATION);
            var Table = wb.Worksheet(6);

            var linha = 2;
            string previousCell = "";

            while (true)
            {
                var BrandColumnCell = Table.Cell("A" + linha.ToString()).Value.ToString();
                if (string.IsNullOrEmpty(BrandColumnCell)) break;
                if (previousCell != BrandColumnCell)
                {
                    microphoneBrandBox.Items.Add(BrandColumnCell);
                }
                linha++;
                previousCell = BrandColumnCell;
            }

            wb.Dispose();
        }

        private void microphoneBrandBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            microphoneModelBox.Items.Clear();
            //fillModelBox
            var wb = new XLWorkbook(Properties.Settings.Default.EQUIPMENTS_TABLE_LOCATION);
            var Table = wb.Worksheet(6);

            var linha = 2;

            while (true)
            {
                var ModelColumnCell = Table.Cell("B" + linha.ToString()).Value.ToString();
                var BrandColumnCell = Table.Cell("A" + linha.ToString()).Value.ToString();

                if (string.IsNullOrEmpty(BrandColumnCell)) break;

                if (microphoneBrandBox.Text == BrandColumnCell)
                {
                    microphoneModelBox.Items.Add(ModelColumnCell);
                }
                linha++;
            }

            wb.Dispose();
        }

        private void notCalibrateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (notCalibrateCheckbox.Checked) lastCalibrationDateBox.Visible = false;
            else lastCalibrationDateBox.Visible = true;
        }
    }
}
