using LASI.Algorithm;
using LASI.Algorithm.DocumentConstructs;
using LASI.Algorithm.LexicalLookup;
using LASI.ContentSystem;
using LASI.UserInterface.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using LASI.InteropLayer;
using LASI.Utilities;
using System.IO;


namespace LASI.UserInterface
{
    /// <summary>
    /// Interaction logic for ResultsWindow.xaml
    /// </summary>
    public partial class ResultsScreen : Window
    {
        #region Constructor
        public ResultsScreen() {

            InitializeComponent();
            Visualizer.ChangeChartKind(ChartKind.NounPhrasesOnly);
            this.Closed += (s, e) => Application.Current.Shutdown();
        }
        #endregion

        #region Methods

        #region View Construction

        /// <summary>
        /// This function associates The buttons which allow the user to modify various aspects of the chart views with their respective functionality.
        /// This is done to allow the functionality to be exposed only after the charts have been 
        /// </summary>
        private void SetupChartViewControls() {
            changeToBarChartButton.Click += ChangeToBarChartButton_Click;
            changeToColumnChartButton.Click += ChangeToColumnChartButton_Click;
            changeToPieChartButton.Click += ChangeToPieChartButton_Click;
        }

        public async Task CreateWeightViewsForAllDocumentsAsync() {
            foreach (var doc in documents) {
                await CreateWeightViewAsync(doc);

            }
            SetupChartViewControls();
        }

        private async Task CreateWeightViewAsync(Document document) {
            var page1 = document.Paginate(20).FirstOrDefault();

            var nounPhraseLabels = from s in page1 != null ? page1.Sentences : document.Paragraphs.SelectMany(p => p.Sentences)
                                        .AsParallel().WithDegreeOfParallelism(Concurrency.CurrentMax)
                                   select s.Phrases.GetNounPhrases() into nounPhrases
                                   from nounPhrase in nounPhrases
                                        .AsParallel().WithDegreeOfParallelism(Concurrency.CurrentMax)
                                   group nounPhrase by new
                                   {
                                       nounPhrase.Text,
                                       nounPhrase.Type
                                   } into g
                                   orderby g.First().Weight descending
                                   select CreateLabelForWeightedView(g.First());

            var weightedListPanel = new StackPanel();
            var grid = new Grid();

            foreach (var l in nounPhraseLabels) { weightedListPanel.Children.Add(l); }

            grid.Children.Add(new ScrollViewer { Content = weightedListPanel });
            var tab = new TabItem { Header = document.Name, Content = grid };

            weightedByDocumentTabControl.Items.Add(tab);
            weightedByDocumentTabControl.SelectedItem = tab;

            await Visualizer.InitChartDisplayAsync(document);
            await Visualizer.DisplayKeyRelationships(document);
        }

        private static Label CreateLabelForWeightedView(NounPhrase element) {
            var genderString = !element.Words.GetDeterminers().Any() ?
                element.IsFullMaleName() ? "Male" : element.IsFullFemaleName() ? "Female" :
                (from p in element.Words.GetProperNouns()
                 group p by p.IsFemaleName() ? "Female" : p.IsMaleName() ? "Male" : "Undetermind"
                     into g
                     orderby g.Count() descending
                     select g.Key).FirstOrDefault() : string.Empty;

            genderString = !string.IsNullOrEmpty(genderString) ? "\nprevialing gender: " + genderString : string.Empty;

            var wordLabel = new Label {
                Tag = element,
                Content = String.Format("Weight : {0}  \"{1}\"", element.Weight, element.Text),
                Foreground = Brushes.Black,
                Padding = new Thickness(1, 1, 1, 1),
                ContextMenu = new ContextMenu(),
                ToolTip = string.Format("{0}{1}",
                element.Type.Name, genderString)
            };
            var menuItem1 = new MenuItem {
                Header = "view definition",
            };
            menuItem1.Click += (s, e) => {
                Process.Start(String.Format("http://www.dictionary.reference.com/browse/{0}?s=t", element.Text));
            };
            wordLabel.ContextMenu.Items.Add(menuItem1);
            var menuItem2 = new MenuItem {
                Header = "Copy"
            };
            menuItem2.Click += (se, ee) => Clipboard.SetText((wordLabel.Tag as ILexical).Text);

            wordLabel.ContextMenu.Items.Add(menuItem2);
            return wordLabel;
        }


        public async Task BuildTextViewsForAllDocumentsAsync() {  // This is for the lexial relationships tab
            var tasks = documents.Select(d => BuildTextViewOfDocument(d)).ToList();
            while (tasks.Any()) {
                var finishedTask = await Task.WhenAny(tasks);
                tasks.Remove(finishedTask);
            }

        }

        private async Task BuildTextViewOfDocument(Document document) {
            var panel = new WrapPanel();
            var tab = new TabItem {
                Header = document.Name,
                Content = new ScrollViewer {
                    Content = panel
                }
            };
            var phraseLabels = new List<Label>();
            var dataSource = document.Paginate(10).FirstOrDefault();

            foreach (var phrase in (dataSource != null ? dataSource.Sentences : document.Sentences).SelectMany(p => p.Phrases)) {
                var phraseLabel = new Label {
                    Content = phrase.Text,
                    Tag = phrase,
                    Foreground = Brushes.Black,
                    Padding = new Thickness(1, 1, 1, 1),
                    ContextMenu = new ContextMenu(),
                    ToolTip = phrase.Type.Name,
                };
                var pronounPhrase = phrase as PronounPhrase;
                if (pronounPhrase != null && pronounPhrase.EntityRefererredTo != null) {
                    CreatePronounPhraseLabelMenu(phraseLabels, phraseLabel, pronounPhrase);
                }
                var vP = phrase as VerbPhrase;
                if (vP != null) {
                    CreateVerbPhraseLabelMenu(phraseLabels, phraseLabel, vP);
                }
                phraseLabels.Add(phraseLabel);
            }
            foreach (var l in phraseLabels) {
                panel.Children.Add(l);
            }
            recomposedDocumentsTabControl.Items.Add(tab);
            recomposedDocumentsTabControl.SelectedItem = tab;
            await Task.Yield();
        }

        #endregion

        #region Named Event Handlers

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) {
            this.Close();
            Application.Current.Shutdown();
        }
        private void printButton_Click_1(object sender, RoutedEventArgs e) {
            var printDialog = new PrintDialog();
            printDialog.ShowDialog();
            var focusedChart = (FrequencyCharts.SelectedItem as TabItem).Content as Visual;
            try {
                printDialog.PrintVisual(focusedChart, "Current View");
            } catch (NullReferenceException) { // There is no chart selected by the user.
            }

        }


        private async void ChangeToBarChartButton_Click(object sender, RoutedEventArgs e) {
            await Visualizer.ToBarCharts();
        }

        private async void ChangeToColumnChartButton_Click(object sender, RoutedEventArgs e) {
            await Visualizer.ToColumnCharts();
        }

        private async void ChangeToPieChartButton_Click(object sender, RoutedEventArgs e) {
            await Visualizer.ToPieCharts();
        }

        private async Task ProcessNewDocument(string docPath, ProgressBar progressBar, Label progressLabel) {

            var chosenFile = FileManager.AddFile(docPath, true);

            await FileManager.ConvertAsNeededAsync();
            currentOperationProgressBar.Value += 10;
            currentOperationLabel.Content = string.Format("Tagging {0}...", chosenFile.NameSansExt);
            var textfile = FileManager.TextFiles.Where(f => f.NameSansExt == chosenFile.NameSansExt).First();

            var doc = await Tagger.DocumentFromRawAsync(textfile);
            currentOperationProgressBar.Value += 10;
            currentOperationLabel.Content = string.Format("{0}: Analyzing Syntax...", chosenFile.NameSansExt);
            foreach (var task in LASI.Algorithm.Binding.Binder.GetBindingTasksForDocument(doc)) {
                currentOperationLabel.Content = task.InitializationMessage;
                await task.Task;
                currentOperationProgressBar.Value += task.PercentWorkRepresented;
                currentOperationLabel.Content = task.CompletionMessage;
            }
            currentOperationProgressBar.Value += 15;
            currentOperationLabel.Content = string.Format("{0}: Correlating Relationships...", chosenFile.NameSansExt);
            var tasks = LASI.Algorithm.Weighting.Weighter.GetWeightingProcessingTasks(doc).ToList();
            foreach (var task in tasks) {

                var message = task.InitializationMessage;
                currentOperationLabel.Content = message;
                await task.Task;
                for (int i = 0; i < 9; i++) {
                    currentOperationProgressBar.Value += 1;

                }


            }

            currentOperationProgressBar.Value += 5;
            currentOperationLabel.Content = string.Format("{0}: Visualizing...", chosenFile.NameSansExt);
            await CreateWeightViewAsync(doc);
            await BuildTextViewOfDocument(doc);

            currentOperationLabel.Content = string.Format("{0}: Added...", chosenFile.NameSansExt);

            currentOperationProgressBar.Value = 100;

            documents.Add(doc);
        }

        /// <summary>
        /// Tags, loads, parses, binds weights, and visualizes the document indicated by the provided path.
        /// </summary>
        /// <param name="docPath">The file system path to a document file.</param>
        /// <returns>A System.Threading.Tasks.Task object representing the ongoing work while the document is being processed.</returns>
        private async Task ProcessNewDocument(string docPath) {
            await ProcessNewDocument(docPath, currentOperationProgressBar, currentOperationLabel);
        }

        private void NewProjectMenuItem_Click_1(object sender, RoutedEventArgs e) {

            //Hacky solution to make every option function. This makes new project restart LASI.
            App.Current.Exit += (sndr, evt) => {
                System.Windows.Forms.Application.Restart();

            };
            App.Current.Shutdown();
        }

        private void OpenManualMenuItem_Click_1(object sender, RoutedEventArgs e) {
            try {
                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.BaseDirectory + @"\Manual.pdf");
            } catch (FileNotFoundException) {
                MessageBox.Show(this, "Unable to locate the User Manual, please contact the LASI team for further support.");
            } catch (Exception) {
                MessageBox.Show(this, "Sorry, the manual could not be opened. Please ensure you have a pdf viewer installed.");
            }
        }

        private void openLicensesMenuItem_Click_1(object sender, RoutedEventArgs e) {
            var componentsDisplay = new ComponentInfoDialogWindow {
                Left = this.Left,
                Top = this.Top,
                Owner = this
            };
            componentsDisplay.ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e) {
            Process.Start("http://lasi-product.org");
        }
        private async void exportButton_Click(object sender, RoutedEventArgs e) {
            foreach (var doc in documents) {
                using (
                    var docWriter = new LASI.ContentSystem.Serialization.XML.SimpleLexicalSerializer(
                    FileManager.ResultsDir + System.IO.Path.DirectorySeparatorChar + new string(
                    doc.Name.TakeWhile(c => c != '.').ToArray()) + ".xml")) {
                    await docWriter.WriteAsync(from S in doc.Sentences
                                               from R in S.Phrases
                                               select R, doc.Name, ContentSystem.Serialization.XML.DegreeOfOutput.Comprehensive);
                }
            }
            var exportDialog = new ExportResultsDialog();
            exportDialog.ShowDialog();
        }

        private async void documentJoinButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new CrossJoinSelectDialog(this) {
                Left = this.Left,
                Top = this.Top,
            };

            if (!(dialog.ShowDialog()) ?? false)
                return;

            var r = await new CrossDocumentJoiner().JoinDocumentsAsnyc(dialog.SelectDocuments);

            metaRelationshipsDataGrid.ItemsSource = Visualizer.CreateRelationshipData(r);


        }


        private async void AddMenuItem_Click(object sender, RoutedEventArgs e) {

            var openDialog = new Microsoft.Win32.OpenFileDialog {
                Filter = "LASI File Types|*.docx; *.pdf; *.txt",
            };
            openDialog.ShowDialog(this);
            if (openDialog.FileNames.Any()) {
                if (!DocumentManager.FileNamePresent(openDialog.SafeFileName)) {
                    currentOperationLabel.Content = string.Format("Converting {0}...", openDialog.FileName);
                    currentOperationFeedbackCanvas.Visibility = Visibility.Visible;
                    currentOperationProgressBar.Value = 0;
                    await ProcessNewDocument(openDialog.FileName);
                    //currentOperationFeedbackCanvas.Visibility = Visibility.Hidden;
                } else {
                    MessageBox.Show(this, string.Format("A document named {0} is already part of the project.", openDialog.SafeFileName));
                }
            }

        }

        #endregion

        #region Label Context Menu Construction

        private static void CreateVerbPhraseLabelMenu(List<Label> phraseLabels, Label phraseLabel, VerbPhrase vP) {
            if (vP.Subjects.Any()) {
                CreateVerbPhraseLabelSubjectMenu(phraseLabels, phraseLabel, vP);
            }
            if (vP.DirectObjects.Any()) {
                CreateVerbPhraseLabelDirectObjectMenu(phraseLabels, phraseLabel, vP);
            }
            if (vP.IndirectObjects.Any()) {
                CreateVerbPhraseLabelIndirectObjectMenu(phraseLabels, phraseLabel, vP);
            }
            if (vP != null && vP.ObjectOfThePreoposition != null) {
                CreateVerbPhraseLabelPrepositionalObjectMenu(phraseLabels, phraseLabel, vP);
            }
        }

        private static void CreateVerbPhraseLabelPrepositionalObjectMenu(List<Label> phraseLabels, Label phraseLabel, VerbPhrase vP) {
            var visitSubjectMI = new MenuItem {
                Header = "view prepositional object"
            };
            visitSubjectMI.Click += (sender, e) => {
                var objlabels = from l in phraseLabels
                                where l.Tag.Equals(vP.ObjectOfThePreoposition)
                                select l;
                foreach (var l in objlabels) {
                    l.Foreground = Brushes.Black;
                    l.Background = Brushes.Red;
                }
            };
            phraseLabel.ContextMenu.Items.Add(visitSubjectMI);
        }

        private static void CreateVerbPhraseLabelIndirectObjectMenu(List<Label> phraseLabels, Label phraseLabel, VerbPhrase vP) {
            var visitSubjectMI = new MenuItem {
                Header = "view indirect objects"
            };
            visitSubjectMI.Click += (sender, e) => {
                var objlabels = from r in vP.IndirectObjects
                                join l in phraseLabels on r equals l.Tag
                                select l;
                foreach (var l in objlabels) {
                    l.Foreground = Brushes.Black;
                    l.Background = Brushes.Red;
                }
            };
            phraseLabel.ContextMenu.Items.Add(visitSubjectMI);
        }

        private static void CreateVerbPhraseLabelDirectObjectMenu(List<Label> phraseLabels, Label phraseLabel, VerbPhrase vP) {
            var visitSubjectMI = new MenuItem {
                Header = "view direct objects"
            };
            visitSubjectMI.Click += (sender, e) => {
                var objlabels = from r in vP.DirectObjects
                                join l in phraseLabels on r equals l.Tag
                                select l;
                foreach (var l in objlabels) {
                    l.Foreground = Brushes.Black;
                    l.Background = Brushes.Red;
                }
            };
            phraseLabel.ContextMenu.Items.Add(visitSubjectMI);
        }

        private static void CreateVerbPhraseLabelSubjectMenu(List<Label> phraseLabels, Label phraseLabel, VerbPhrase vP) {
            var visitSubjectMI = new MenuItem {
                Header = "view subjects"
            };
            visitSubjectMI.Click += (sender, e) => {
                var objlabels = from r in vP.Subjects
                                join l in phraseLabels on r equals l.Tag
                                select l;
                foreach (var l in objlabels) {
                    l.Foreground = Brushes.Black;
                    l.Background = Brushes.Red;
                }
            };
            phraseLabel.ContextMenu.Items.Add(visitSubjectMI);
        }

        private static void CreatePronounPhraseLabelMenu(List<Label> phraseLabels, Label phraseLabel, PronounPhrase pronounPhrase) {
            var visitBoundEntity = new MenuItem {
                Header = "view referred to"
            };
            visitBoundEntity.Click += (sender, e) => {
                var objlabels = from l in phraseLabels
                                where l.Tag == pronounPhrase.EntityRefererredTo
                                select l;
                foreach (var l in objlabels) {
                    l.Foreground = Brushes.Black;
                    l.Background = Brushes.Teal;
                }
            };
            phraseLabel.ContextMenu.Items.Add(visitBoundEntity);
        }

        #endregion

        #endregion

        #region Properties and Fields

        private List<Document> documents = new List<Document>();

        public List<Document> Documents {
            get {
                return documents;
            }
            set {
                documents = value;
            }
        }

        #endregion

        private void openPreferencesMenuItem_Click(object sender, RoutedEventArgs e) {
            var preferences = new PreferencesWindow();
            preferences.Left = (this.Left - preferences.Left) / 2;
            preferences.Top = (this.Top - preferences.Top) / 2;
            var saved = preferences.ShowDialog();
        }
    }

}
