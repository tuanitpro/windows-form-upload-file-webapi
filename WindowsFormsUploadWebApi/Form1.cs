using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsUploadWebApi
{
   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void AddMessage(string text)
        {
            richTextBox1.BeginInvoke((Action)(() =>
                {
                    richTextBox1.AppendText(Environment.NewLine + text);
                }));
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    HttpClient httpClient = new HttpClient();
                    // Read the files 
                    foreach (String file in openFileDialog1.FileNames)
                    {
                        var fileStream = File.Open(file, FileMode.Open);
                        var fileInfo = new FileInfo(file);
                        FileUploadResult uploadResult = null;
                        bool _fileUploaded = false;

                        var content = new MultipartFormDataContent();
                        content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", fileInfo.Name)
                        );
                        string uploadServiceBaseAddress = "http://minhquandalat.com/api/FileUpload";
                        Task taskUpload = httpClient.PostAsync(uploadServiceBaseAddress, content).ContinueWith(task =>
                        {
                            if (task.Status == TaskStatus.RanToCompletion)
                            {
                                var response = task.Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    uploadResult = response.Content.ReadAsAsync<FileUploadResult>().Result;
                                    if (uploadResult != null)
                                        _fileUploaded = true;

                                // Read other header values if you want..
                                foreach (var header in response.Content.Headers)
                                    {
                                        Debug.WriteLine("{0}: {1}", header.Key, string.Join(",", header.Value));
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine("Status Code: {0} - {1}", response.StatusCode, response.ReasonPhrase);
                                    Debug.WriteLine("Response Body: {0}", response.Content.ReadAsStringAsync().Result);
                                }
                            }

                            fileStream.Dispose();
                        });

                        taskUpload.Wait();
                        if (_fileUploaded)
                        {
                            AddMessage(uploadResult.Name);
                            AddMessage("http://minhquandalat.com/uploads/" + uploadResult.Name);

                            
                        }
                    }

                    httpClient.Dispose();
                }
            }
            catch (Exception ex)
            {
                AddMessage(ex.Message);
            }
        }

        private void btnRestSharp_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string []path = openFileDialog1.FileNames;

                var client = new RestClient("http://minhquandalat.com/api/FileUpload");
                

                var request = new RestRequest(Method.POST);
                foreach (var item in path)
                {
                    request.AddFile("fileName", item);
                }
                

                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as 

                AddMessage(content);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
