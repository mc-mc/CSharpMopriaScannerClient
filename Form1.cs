using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;


namespace MopriaScannerClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
         
            txtStatus.Text = "";
            string url = "http://"+txtIP.Text.Trim()+"/eSCL/ScannerStatus";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Make a synchronous GET request
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        string resp= response.Content.ReadAsStringAsync().Result;
                        txtStatus.Text = resp;
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        throw new Exception($"Failed to make a GET request. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the HTTP request
                txtStatus.Text = $"{url} Error while making the request: {ex.Message}";
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //crashes printer

            string url = "http://" + txtIP.Text.Trim() + "/eSCL/ScanBufferInfo";
            string payload = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<scan:ScanSettings xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03"" xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"">
   <pwg:Version>2.0</pwg:Version>
   <pwg:ScanRegions>
      <pwg:ScanRegion>
         <pwg:Height>3500</pwg:Height>
         <pwg:Width>2550</pwg:Width>
         <pwg:XOffset>0</pwg:XOffset>
         <pwg:YOffset>0</pwg:YOffset>
      </pwg:ScanRegion>
   </pwg:ScanRegions>
   <scan:XResolution>100</scan:XResolution>
   <scan:YResolution>100</scan:YResolution>
   <scan:ColorMode>RGB24</scan:ColorMode>
</scan:ScanSettings>
";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Create a StringContent with the XML payload
                    StringContent content = new StringContent(payload, Encoding.UTF8, "text/xml");

                    // Make a synchronous PUT request
                    HttpResponseMessage response = client.PutAsync(url, content).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        string resp =response.Content.ReadAsStringAsync().Result;
                        txtStatus.Text = resp;
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        throw new Exception($"Failed to make a PUT request. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the HTTP request
                txtStatus.Text = $"{url} Error while making the request: {ex.Message}";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtDocumentId.Text = "";
            txtStatus.Text = "";
            string url = "http://" + txtIP.Text.Trim() + "/eSCL/ScanJobs";
            string payload = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<scan:ScanSettings xmlns:scan=""http://schemas.hp.com/imaging/escl/2011/05/03"" xmlns:pwg=""http://www.pwg.org/schemas/2010/12/sm"">
   <pwg:Version>2.0</pwg:Version>
   <pwg:ScanRegions>
      <pwg:ScanRegion>
         <pwg:Height>3500</pwg:Height>
         <pwg:Width>2550</pwg:Width>
         <pwg:XOffset>0</pwg:XOffset>
         <pwg:YOffset>0</pwg:YOffset>
      </pwg:ScanRegion>
   </pwg:ScanRegions>
   <scan:DocumentFormatExt>image/jpeg</scan:DocumentFormatExt>
   <pwg:ContentType>TextAndPhoto</pwg:ContentType>
   <scan:XResolution>100</scan:XResolution>
   <scan:YResolution>100</scan:YResolution>
   <scan:ColorMode>RGB24</scan:ColorMode>
</scan:ScanSettings>
";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Create a StringContent with the XML payload
                    StringContent content = new StringContent(payload, Encoding.UTF8, "text/xml");

                    // Make a synchronous PUT request
                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        string resp = response.Content.ReadAsStringAsync().Result;
                        txtStatus.Text = resp + "Done\n" + "Headers:" + response.Headers.ToString();

                        // Get header  Location:   to get URL of scan data  
                        Dictionary<string, string> hdrs       = response.Headers.ToDictionary(a => a.Key, a => string.Join(";", a.Value));
                        txtDocumentId.Text = hdrs["Location"];

                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        throw new Exception($"Failed to make a Post request. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the HTTP request
                txtStatus.Text = $"{url} Error while making the request: {ex.Message}";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {

            txtStatus.Text = "";
            string url = txtDocumentId.Text+ "/ScanImageInfo";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Make a synchronous GET request
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        string resp = response.Content.ReadAsStringAsync().Result;
                        txtStatus.Text = resp;
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        throw new Exception($"Failed to make a GET request. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the HTTP request
                txtStatus.Text=$"{url} Error while making the GET request: {ex.Message}";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            txtStatus.Text = "";
            string url = txtDocumentId.Text + "/NextDocument";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Make a synchronous GET request
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {

                        // Read the image data as a byte array
                        byte[] imageData = response.Content.ReadAsByteArrayAsync().Result;

                        // Create a MemoryStream from the byte array
                        using (MemoryStream stream = new MemoryStream(imageData))
                        {
                            // Create an Image from the MemoryStream
                            Image image = Image.FromStream(stream);

                            // Set the PictureBox image
                            picBox.Image = image;

                            txtStatus.Text = $"OK:  Image size retrieved: {image.Size.Width} x {image.Size.Height}\n   Format:{image.RawFormat.ToString()}";
                        }

                        
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        throw new Exception($"Failed to make a GET request. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the HTTP request
                txtStatus.Text = $"{url} Error while making the GET request: {ex.Message}";
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtStatus.Text = "";
            string url = "http://" + txtIP.Text.Trim() + "/eSCL/ScannerCapabilities";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Make a synchronous GET request
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        string resp = response.Content.ReadAsStringAsync().Result;
                        txtStatus.Text = resp;
                    }
                    else
                    {
                        // Handle the case when the request was not successful
                        throw new Exception($"Failed to make a GET request. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the HTTP request
                throw new Exception($"Error while making the GET request: {ex.Message}");
            }
        }
    }
}
