﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace perSONA
{
    public partial class Sende_mailForm : Form
    {
        public Sende_mailForm()
        {
            InitializeComponent();
        }
                
        private void button1_Click(object sender, EventArgs e)
        {

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            
            mail.To.Add("softwarepersona@gmail.com") ;             // put to address here
            mail.Bcc.Add (From.Text);                              // put the from address here
            
            mail.Subject = Subject.Text;                           // put subject here	
            mail.SubjectEncoding = System.Text.Encoding.UTF8;      // change text to type UTF8

            mail.Body = Body.Text;                                 // put body of email here
            mail.BodyEncoding = System.Text.Encoding.UTF8;         // change text to type UTF8

            // send the mail
            mail.IsBodyHtml = true;
            mail.From = new System.Net.Mail.MailAddress("softwarepersona@gmail.com");

            System.Net.Mail.SmtpClient Client = new System.Net.Mail.SmtpClient();

            Client.Credentials = new System.Net.NetworkCredential("softwarepersona@gmail.com" , "lva2cafe");

            Client.Port = 587;
            Client.EnableSsl = true;

            Client.Host = "smtp.gmail.com";

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Client.Send(mail);
                MessageBox.Show("Mensagem Enviada");
            }
            catch (Exception) 
            {
                Cursor.Current = Cursors.WaitCursor;
                MessageBox.Show("Erro no Envio");
            }
            Cursor.Current = Cursors.Default;
            Close();
            // In case of doubt -> https://www.youtube.com/watch?v=WKyVqmbCVc0   and    https://www.hostinger.com.br/tutoriais/aprenda-a-utilizar-o-smtp-google/
        }

        private void WhatsApp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Você será direcionado a um grupo de WhatsApp destinado a resolver dúvidas de usuários, após ser respondido pedimos que se retire do grupo para facilitar a organização, Obrigado.", "Grupo de Dúvidas", MessageBoxButtons.OK);
            System.Diagnostics.Process.Start("https://chat.whatsapp.com/DsJNiJ4zwYBCcRT0jxCP8h");
        }
    }
}
