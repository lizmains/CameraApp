﻿using NativeMedia;

namespace CameraApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnTakeVidBtnClicked(object sender, EventArgs e)
    {
        TakeVideo(); //Take video
    }
    private void OnPicVidBtnClicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new MainPage()); //navigate to main page
        //PickVideo(); //Pick video from camera roll
        PickImage();
    }

    
    // found this code at: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device-media/picker?tabs=macios#take-a-photo
    public async void TakeVideo()
    {
        // if (MediaPicker.Default.IsCaptureSupported)
        // {
        //     FileResult video = await MediaPicker.Default.CaptureVideoAsync();
        //
        //     if (video != null)
        //     {
        //         // save the file into local storage
        //         // string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), video.FileName); //not sure if this is the right directory
        //         string localFilePath = Path.Combine(FileSystem.CacheDirectory, video.FileName); //not sure if this is the right directory
        //
        //         using Stream sourceStream = await video.OpenReadAsync();
        //         using FileStream localFileStream = File.OpenWrite(localFilePath);
        //
        //         await sourceStream.CopyToAsync(localFileStream);
        //         await Shell.Current.DisplayAlert("OOPS", localFileStream.Name, "Ok");
        //     }
        // }

        //THIS WORKS... saves screenshot to actual camera roll, good things good things... lets try with video
        // var thingy = await Screenshot.CaptureAsync();
        // await MediaGallery.SaveAsync(MediaFileType.Image, await thingy.OpenReadAsync(), "myMedia.png");

        //trying to combine two things here, don't think its actually going to work... using mediaPicker AND xamarin.mediaGallery
        // var vid = await CrossMedia.Current.TakePhotoAsync(new StoreVideoOptions
        
        //IN THE WORKS
        // var video = await MediaPicker.CapturePhotoAsync();
        var video = await MediaGallery.CapturePhotoAsync();
        await MediaGallery.SaveAsync(MediaFileType.Image, await video.OpenReadAsync(), "myMedia.png");
        if (video == null)
        {
            await DisplayAlert("Null Video", "Video could not be saved", "OK");
        }
        
        //I dont think this can be saved...
        // if (video != null)
        // {
        //     await MediaGallery.SaveAsync(MediaFileType.Image, await video.OpenReadAsync(), "myShot.mp4");
        //     await DisplayAlert("Video Saved", "Video was sucessfully saved", "OK");
        //     Console.WriteLine($"video filepath: {video.FullPath}");
        // }
        
    }
    
    public async void PickVideo()
    {

        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Pick video",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
        {
            return;
        }

        var stream = await result.OpenReadAsync();
        myVideo.Source = ImageSource.FromStream(() => stream); //I think this has something to do with why it wont come up
    }
    
    public async void PickImage()
    {
        //this one opens the camera roll but doesn't do anything with said file
        // if (MediaPicker.Default.IsCaptureSupported)
        // {
        //     FileResult video = await MediaPicker.Default.PickVideoAsync(); //opens camera roll, nothing happens after select video as of right now
        // }

        //This one only allows you to pick photos and display them, can pick videos but cannot display them
        // var result = await FilePicker.PickAsync(new PickOptions
        // {
        //     PickerTitle = "Pick video",
        //     FileTypes = FilePickerFileType.Images
        // });
        //
        // if (result == null)
        // {
        //     return;
        // }
        //
        // var stream = await result.OpenReadAsync();
        // myVideo.Source = ImageSource.FromStream(() => stream);

        // Not using MediaGallery, but the built-in MediaPicker
        var results = await MediaPicker.PickPhotoAsync();
        //For some reason none of this is being hit at all...
        await DisplayAlert("You are here 1", "here 1", "OK");

        if (results == null)
        {
            await DisplayAlert("Null Photos", "The photos you have selected are null", "OK");
            return;
        }

        var fileName = Path.GetFileNameWithoutExtension(results.FullPath);
        var extension = Path.GetExtension(results.FullPath);
        var contentType = results.ContentType;
        var readIt = await results.OpenReadAsync();

        await DisplayAlert(fileName, $"Extension: {extension}, Content-type: {contentType}", "OK");

        myVideo.Source = ImageSource.FromStream(() => readIt);

        await DisplayAlert("You are here", "Here", "OK");
    }
}
