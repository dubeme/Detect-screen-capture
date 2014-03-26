using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TestForScreenCapture.Resources;
using Microsoft.Devices;
using System.Windows.Threading;
using Microsoft.Xna.Framework.Media;

namespace TestForScreenCapture
{
    public partial class MainPage : PhoneApplicationPage
    {
        PictureCollection lastKnownPictureCollectionState = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Save a copy of the current state of the Pictures on the phone
            lastKnownPictureCollectionState = new MediaLibrary().Pictures;

            // This timer is used to check for changes in the collection
            // of photos in the phone.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            // This can be change to your satisfaction
            // But, you have to be careful not to over do it.
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            PictureCollection currentPictureCollection = new MediaLibrary().Pictures;

            if (currentPictureCollection.Count > lastKnownPictureCollectionState.Count)
            {
                for (int i = lastKnownPictureCollectionState.Count; i < currentPictureCollection.Count; i++)
                {
                    // All I have to check for is the name of the picture
                    // Since I am only interested in the screen shot.
                    // The main reason, I am using this method is that 
                    // since only one app can run at a time, and 
                    // the user never leaves the app. 
                    // Any picture that is added to the MedialLibrary with "wp_ss"
                    // has a high probability of coming from my app.
                    if (currentPictureCollection[i].Name.Substring(0,5) == "wp_ss")
                    {
                        MessageBox.Show("Got you.");

                        tbk.Text = "This screenshot [" + currentPictureCollection[i].Name + "] taken on [" + 
                            currentPictureCollection[i].Date + "].";
                        lastKnownPictureCollectionState = currentPictureCollection;
                        // Since I am only interested in finding the first occurance
                        return;
                    }
                }
            }
        }
    }
}