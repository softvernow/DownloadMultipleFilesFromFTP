using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadMultipleFilesFromFTP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Download_Files();
        }

        private void Download_Files()
        {
            List<String> files = Get_File_List_From_FTP();

            foreach (String file in files)
            {
                String file_data = Get_File_Data_And_Download_File(file);

                txtResults.AppendText(file + "\t\r");
                txtResults.AppendText(file_data + "\t\r");
                txtResults.AppendText("\t\r");
            }

        }


        private List<String> Get_File_List_From_FTP()
        {
            //result data from file
            List<String> file_list = new List<String>();

            //do ftpwebrequest
            //initialize FtpWebRequest with your FTP Url
            //your FTP url should start with ftp://wwww.youftpsite.com//
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpInfo.ftp_url);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            //set up credentials
            request.Credentials = new NetworkCredential(FtpInfo.ftp_login, FtpInfo.ftp_password);

            //initialize ftp response
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            //open readers
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);


            String line = String.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                if(line.Contains(".txt"))
                    file_list.Add(line);
            }


                //closing
            reader.Close();
            response.Close();

            return file_list;

        }


        private String Get_File_Data_And_Download_File(String file_name)
        {
            //result data from file
            String result = String.Empty;

            //do ftpwebrequest
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(FtpInfo.ftp_url + file_name);
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            //set up credentials
            request.Credentials = new NetworkCredential(FtpInfo.ftp_login, FtpInfo.ftp_password);

            //initialize ftp response
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            //open readers
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            //data from file.
            result = reader.ReadToEnd();

            //set to save file locally. 
            using (StreamWriter file = File.CreateText(file_name))
            {
                file.WriteLine(result);
                file.Close();

            }

            //closing
            reader.Close();
            response.Close();

            return result;

        }
    }
}
