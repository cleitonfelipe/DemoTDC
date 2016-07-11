using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DemoTDC
{
    [Activity(Label = "DemoTDC", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        string sas = @"https://demotdc.blob.core.windows.net/containertdc?sv=2015-04-05&sr=c&sig=%2FEdazcZj753JjQh%2BCzpD4WxdyPlN2xNY19Ub8zF4SAE%3D&se=2016-07-09T15%3A52%3A29Z&sp=rwdl";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            TextView txt = FindViewById<TextView>(Resource.Id.textBlob);
            Button btn = FindViewById<Button>(Resource.Id.button1);

            string date = DateTime.Now.ToString();
            string blob = "sasblob_tdc.txt";

            button.Click += async delegate {
                await CreateBlob(sas, blob);
                txt.Text = await ReturnBlob(sas, blob);
            };

            btn.Click += async delegate {
                await DeleteBlob(sas, blob);
                txt.Text = String.Empty;
            };
        }

        static async Task CreateBlob(string sas, string nomeBlob)
        {
            //Try performing container operations with the SAS provided.

            //Return a reference to the container using the SAS URI.
            CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));            
            try
            {
                //Write operation: write a new blob to the container.
                CloudBlockBlob blob = container.GetBlockBlobReference(nomeBlob);

                string blobContent = "Este blob foi criado para uma demo da Trilha de Xamarin do TDC!";
                MemoryStream msWrite = new MemoryStream(Encoding.UTF8.GetBytes(blobContent));
                msWrite.Position = 0;
                using (msWrite)
                {
                    await blob.UploadFromStreamAsync(msWrite);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        static async Task<string> ReturnBlob(string sas, string blobNome)
        {
            //Try performing container operations with the SAS provided.

            //Return a reference to the container using the SAS URI.
            CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));
            try
            {
                //Read operation: Get a reference to one of the blobs in the container and read it.
                CloudBlockBlob blob = container.GetBlockBlobReference(blobNome);
                string data = await blob.DownloadTextAsync();

                return data;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        static async Task DeleteBlob(string sas, string blobNome)
        {
            CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));
            try
            {
                //Delete operation: Delete a blob in the container.
                CloudBlockBlob blob = container.GetBlockBlobReference(blobNome);
                await blob.DeleteAsync();                
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

