using CommunityToolkit.Maui.Views;
using NativeMedia;

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
        PickImage();
    }

    
    public async void TakeVideo()
    {
        //IN THE WORKS
        var video = await MediaPicker.CapturePhotoAsync();
        // var video = await MediaGallery.CapturePhotoAsync();
        await MediaGallery.SaveAsync(MediaFileType.Image, await video.OpenReadAsync(), "myMedia.png");
        if (video == null)
        {
            await DisplayAlert("Null Video", "Video could not be saved", "OK");
        }

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
        
        // Not using MediaGallery, but the built-in MediaPicker
        var results = await MediaPicker.PickVideoAsync();
        // var results = await MediaGallery.PickAsync(1, MediaFileType.Image)
        
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
        
        // myVideo.Source = ImageSource.FromStream(() => readIt);

        mediaElement.Source = MediaSource.FromResource(results.FullPath);
        
        await DisplayAlert("You are here", "Here", "OK");
    }
}
