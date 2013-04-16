﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LASI.Utilities;
using LASI.InteropLayer;
using LASI.GuiInterop;
using LASI.UserInterface.Dialogs;
namespace LASI.UserInterface
{
    /// <summary>
    /// Interaction logic for DialogToProcedeToResults.xaml
    /// </summary>
    public partial class InProgressScreen : Window
    {
        public InProgressScreen() {
            InitializeComponent();
            this.IsVisibleChanged += async (s, e) => await InitPawPrintAlternation();
            this.Closing += (s, e) => Application.Current.Shutdown();
            ProgressBar.Value = 0;
            ProgressLabel.Content = ProcessingState.Initializing;
        }


        #region Visual Feedback Functions

        #region Animation

        private async Task InitPawPrintAlternation() {
            var pawPrints = new[] { pawPrintImg1, pawPrintImg3, pawPrintImg2, pawPrintImg4, pawPrintImg5, pawPrintImg6 }.ToList();
            pawPrints.ForEach(pp => pp.Opacity = 0);
            foreach (var pp in pawPrints) {
                FadeImage(pp);
                await Task.Delay(2700);
            }

        }
        private async void FadeImage(Image img) {
            while (img.Opacity > 0.0) {
                img.Opacity -= 0.01;
                await Task.Delay(10);
            }
            await Task.Delay(500);
            while (img.Opacity < 1.0) {
                img.Opacity += 0.01;
                await Task.Delay(10);
            }
            FadeImage(img);

        }

        #endregion

        #region Progress Bar

        StatusProvider status = new StatusProvider();

        public async Task InitProgressBar() {

            var msg = await status.GetStatus();

            ProgressBar.ToolTip = msg.ToString();
            ProgressLabel.Content = msg.ToString();
            for (var i = 0; i < 20; i++) {
                await Task.Delay(20);
                ProgressBar.Value++;
            }
            if (ProgressBar.Value < 100) {
                await InitProgressBar();
            } else {
                await Task.Delay(100);
                DisplayProceedDialog();
            }


        }

        private void DisplayProceedDialog() {

            var procedeDialog = new DialogToProcedeToResults();

            procedeDialog.Topmost = true;

            procedeDialog.PositionAt(this.Left, this.Top);

            procedeDialog.ContinueButton.Click += (sender, e) => ProceedToResultsView();

            //Shows the window as a modal dialog
            procedeDialog.ShowDialog();

        }

        #endregion

        #endregion


        private void ProceedToResultsView() {
            WindowManager.ResultsScreen.SetTitle(WindowManager.CreateProjectScreen.LastLoadedProjectName + " - L.A.S.I.");
            this.SwapWith(WindowManager.ResultsScreen);
            WindowManager.ResultsScreen.BuildAssociationTextView();
            WindowManager.ResultsScreen.BuildFullSortedView();
        }
        private void MenuItem_Click_3(object sender, RoutedEventArgs e) {
            this.Close();

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e) {

        }

    }
}
