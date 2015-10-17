using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System.Collections.Generic;
using System.Diagnostics;

namespace DeviceApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        private INavigationService NavigationService { get; }

        private IDeviceGestureService DeviceGestureService { get; }

        public bool HasCameraButton => this.DeviceGestureService.IsHardwareCameraButtonPresent;

        public bool HasBackButton => this.DeviceGestureService.IsHardwareBackButtonPresent;

        public bool HasKeyboard => this.DeviceGestureService.IsKeyboardPresent;

        public bool HasMouse => this.DeviceGestureService.IsMousePresent;

        public bool HasTouch => this.DeviceGestureService.IsTouchPresent;

        public MainPageViewModel(INavigationService ns, IDeviceGestureService ds)
        {
            this.NavigationService = ns;
            this.DeviceGestureService = ds;
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            this.DeviceGestureService.GoBackRequested += this.DeviceGestureService_GoBackRequested;
            this.DeviceGestureService.CameraButtonPressed += this.DeviceGestureService_CameraButtonPressed;
            this.DeviceGestureService.MouseMoved += this.DeviceGestureService_MouseMoved;
        }

        private void DeviceGestureService_MouseMoved(object sender, Windows.Devices.Input.MouseEventArgs e)
        {
            Debug.WriteLine($"{e.MouseDelta.X}, {e.MouseDelta.Y}");
        }

        public void NavigateNextPage()
        {
            this.NavigationService.Navigate("Next", null);
        }

        private void DeviceGestureService_CameraButtonPressed(object sender, DeviceGestureEventArgs e)
        {
            Debug.WriteLine(nameof(this.DeviceGestureService_CameraButtonPressed));
        }

        private void DeviceGestureService_GoBackRequested(object sender, DeviceGestureEventArgs e)
        {
            Debug.WriteLine(nameof(this.DeviceGestureService_GoBackRequested));
            e.Handled = true;
            e.Cancel = true;
        }

        public override void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatingFrom(e, viewModelState, suspending);
            this.DeviceGestureService.GoBackRequested -= this.DeviceGestureService_GoBackRequested;
            this.DeviceGestureService.CameraButtonPressed -= this.DeviceGestureService_CameraButtonPressed;
            this.DeviceGestureService.MouseMoved -= this.DeviceGestureService_MouseMoved;
        }
    }
}
