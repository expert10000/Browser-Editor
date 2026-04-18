// Copyright (C) Microsoft Corporation. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using SharpBrowser.Controls.BrowserTabStrip.Data;

namespace WebView2WindowsFormsBrowser
{
    public partial class BrowserForm : Form
    {
        private CoreWebView2CreationProperties _creationProperties = null;
        private bool _useChromeLayout;
        private Panel _contentPanel;
        private GradientPanel _chromePanel;
        private TableLayoutPanel _chromeLayout;
        private CenteredFlowLayoutPanel _navPanel;
        private CenteredFlowLayoutPanel _actionPanel;
        private Panel _addressShell;
        private SplitContainer _editorSplit;
        private SplitContainer _designPreviewSplit;
        private Panel _previewHostPanel;
        private Panel _visualEditorHostPanel;
        private RichTextBox _htmlEditor;
        private WebView2 _visualEditorWebView;
        private Button _lightThemeButton;
        private Button _darkThemeButton;
        private Button _openHtmlButton;
        private Button _saveHtmlButton;
        private Button _formatHtmlButton;
        private Button _toggleVisualEditorButton;
        private System.Windows.Forms.Timer _editorPreviewTimer;
        private System.Windows.Forms.Timer _editorSyntaxTimer;
        private bool _suppressEditorTextChanged;
        private bool _isApplyingSyntaxHighlight;
        private bool _isRenderingEditorPreview;
        private bool _isRenderingVisualEditor;
        private bool _ignoreNextVisualEditorMessage;
        private bool _isSyncingSourceFromVisualEditor;
        private bool _isVisualEditorPanelOpen;
        private bool _viewportOnlySyntaxHighlightPending;
        private bool _syncEditorFromNavigation;
        private string _pendingVisualEditorHtml = string.Empty;
        private Uri _editorBaseUri;
        private const int SyntaxHighlightMaxChars = 35000;
        private const int SyntaxHighlightViewportPadding = 3000;
        private const int ScriptFormatMaxChars = 80000;
        private const int CssFormatMaxChars = 80000;
        private static readonly Regex HtmlCommentPattern = new Regex("<!--[\\s\\S]*?-->", RegexOptions.Compiled);
        private static readonly Regex HtmlDoctypePattern = new Regex("<!DOCTYPE[\\s\\S]*?>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex HtmlEntityPattern = new Regex("&(?:#\\d+|#x[0-9a-fA-F]+|[a-zA-Z][a-zA-Z0-9]+);", RegexOptions.Compiled);
        private static readonly Regex HtmlScriptBlockPattern = new Regex("<script\\b[^>]*>(?<code>[\\s\\S]*?)</script\\s*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex JavaScriptBlockCommentPattern = new Regex("/\\*[\\s\\S]*?\\*/", RegexOptions.Compiled);
        private static readonly Regex JavaScriptLineCommentPattern = new Regex("//.*?$", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex JavaScriptSingleQuotedStringPattern = new Regex("'(?:\\\\.|[^'\\\\])*'", RegexOptions.Compiled);
        private static readonly Regex JavaScriptDoubleQuotedStringPattern = new Regex("\"(?:\\\\.|[^\"\\\\])*\"", RegexOptions.Compiled);
        private static readonly Regex JavaScriptTemplateStringPattern = new Regex("`(?:\\\\.|[^`\\\\])*`", RegexOptions.Compiled);
        private static readonly Regex JavaScriptKeywordPattern = new Regex("\\b(?:async|await|break|case|catch|class|const|continue|debugger|default|delete|do|else|export|extends|finally|for|function|if|import|in|instanceof|let|new|of|return|super|switch|this|throw|try|typeof|var|void|while|with|yield)\\b", RegexOptions.Compiled);
        private static readonly Regex JavaScriptLiteralPattern = new Regex("\\b(?:true|false|null|undefined|NaN|Infinity)\\b", RegexOptions.Compiled);
        private static readonly Regex JavaScriptNumberPattern = new Regex("\\b(?:0[xX][0-9a-fA-F_]+|0[bB][01_]+|0[oO][0-7_]+|(?:\\d[\\d_]*)(?:\\.(?:\\d[\\d_]*))?(?:[eE][+\\-]?\\d[\\d_]*)?|\\.\\d[\\d_]*(?:[eE][+\\-]?\\d[\\d_]*)?)\\b", RegexOptions.Compiled);
        private UiTheme _currentTheme = UiTheme.Dark;
        private static readonly Color EditorBaseColor = Color.FromArgb(232, 236, 244);
        private static readonly Color EditorCommentColor = Color.FromArgb(120, 160, 120);
        private static readonly Color EditorTagBracketColor = Color.FromArgb(148, 163, 184);
        private static readonly Color EditorTagNameColor = Color.FromArgb(96, 165, 250);
        private static readonly Color EditorAttributeNameColor = Color.FromArgb(244, 114, 182);
        private static readonly Color EditorAttributeValueColor = Color.FromArgb(253, 186, 116);
        private static readonly Color EditorEntityColor = Color.FromArgb(45, 212, 191);
        private static readonly Color EditorJavaScriptKeywordColor = Color.FromArgb(196, 181, 253);
        private static readonly Color EditorJavaScriptStringColor = Color.FromArgb(134, 239, 172);
        private static readonly Color EditorJavaScriptNumberColor = Color.FromArgb(251, 146, 60);
        private static readonly Color EditorJavaScriptLiteralColor = Color.FromArgb(56, 189, 248);
        private static readonly Color EditorBaseColorLight = Color.FromArgb(31, 41, 55);
        private static readonly Color EditorCommentColorLight = Color.FromArgb(22, 101, 52);
        private static readonly Color EditorTagBracketColorLight = Color.FromArgb(71, 85, 105);
        private static readonly Color EditorTagNameColorLight = Color.FromArgb(29, 78, 216);
        private static readonly Color EditorAttributeNameColorLight = Color.FromArgb(190, 24, 93);
        private static readonly Color EditorAttributeValueColorLight = Color.FromArgb(180, 83, 9);
        private static readonly Color EditorEntityColorLight = Color.FromArgb(13, 148, 136);
        private static readonly Color EditorJavaScriptKeywordColorLight = Color.FromArgb(91, 33, 182);
        private static readonly Color EditorJavaScriptStringColorLight = Color.FromArgb(22, 101, 52);
        private static readonly Color EditorJavaScriptNumberColorLight = Color.FromArgb(194, 65, 12);
        private static readonly Color EditorJavaScriptLiteralColorLight = Color.FromArgb(3, 105, 161);
        private const int WM_SETREDRAW = 0x000B;
        private const int EM_GETSCROLLPOS = 0x0400 + 221;
        private const int EM_SETSCROLLPOS = 0x0400 + 222;
        private enum UiTheme
        {
            Dark,
            Light
        }

        public CoreWebView2CreationProperties CreationProperties
        {
            get
            {
                if (_creationProperties == null)
                {
                    _creationProperties = new Microsoft.Web.WebView2.WinForms.CoreWebView2CreationProperties();
                }
                return _creationProperties;
            }
            set
            {
                _creationProperties = value;
            }
        }

        public BrowserForm()
        {
            InitializeComponent();

            // Designer protection – skip runtime-only code in design mode
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            ApplyChromeLayout();
            AttachControlEventHandlers(this.webView2Control);
            HandleResize();
        }

        public BrowserForm(CoreWebView2CreationProperties creationProperties = null)
        {
            this.CreationProperties = creationProperties;
            InitializeComponent();

            // Designer protection – skip runtime-only code in design mode
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;

            ApplyChromeLayout();
            AttachControlEventHandlers(this.webView2Control);
            HandleResize();
        }

        private void UpdateTitleWithEvent(string message)
        {
            string currentDocumentTitle = this.webView2Control?.CoreWebView2?.DocumentTitle ?? "Uninitialized";
            this.Text = currentDocumentTitle + " (" + message + ")";
        }

        CoreWebView2Environment _webViewEnvironment;
        CoreWebView2Environment WebViewEnvironment
        {
            get
            {
                if (_webViewEnvironment == null && webView2Control?.CoreWebView2 != null)
                {
                    _webViewEnvironment = webView2Control.CoreWebView2.Environment;
                }
                return _webViewEnvironment;
            }
        }

        CoreWebView2Settings _webViewSettings;
        CoreWebView2Settings WebViewSettings
        {
            get
            {
                if (_webViewSettings == null && webView2Control?.CoreWebView2 != null)
                {
                    _webViewSettings = webView2Control.CoreWebView2.Settings;
                }
                return _webViewSettings;
            }
        }

        string _lastInitializeScriptId;

        List<CoreWebView2Frame> _webViewFrames = new List<CoreWebView2Frame>();
        void WebView_HandleIFrames(object sender, CoreWebView2FrameCreatedEventArgs args)
        {
            _webViewFrames.Add(args.Frame);
            args.Frame.Destroyed += WebViewFrames_DestoryedNestedIFrames;
        }

        void WebViewFrames_DestoryedNestedIFrames(object sender, object args)
        {
            try
            {
                var frameToRemove = _webViewFrames.SingleOrDefault(r => r.IsDestroyed() == 1);
                if (frameToRemove != null)
                    _webViewFrames.Remove(frameToRemove);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string WebViewFrames_ToString()
        {
            string result = "";
            for (var i = 0; i < _webViewFrames.Count; i++)
            {
                if (i > 0) result += "; ";
                result += i.ToString() + " " +
                    (String.IsNullOrEmpty(_webViewFrames[i].Name) ? "<empty_name>" : _webViewFrames[i].Name);
            }
            return String.IsNullOrEmpty(result) ? "no iframes available." : result;
        }

        #region Event Handlers
        // Enable (or disable) buttons when webview2 is init (or disposed). Similar to the CanExecute feature of WPF.
        private void UpdateButtons(bool isEnabled)
        {
            this.btnEvents.Enabled = isEnabled;
            this.btnBack.Enabled = isEnabled && webView2Control != null && webView2Control.CanGoBack;
            this.btnForward.Enabled = isEnabled && webView2Control != null && webView2Control.CanGoForward;
            this.btnRefresh.Enabled = isEnabled;
            this.btnGo.Enabled = isEnabled;
            this.closeWebViewToolStripMenuItem.Enabled = isEnabled;
            this.allowExternalDropMenuItem.Enabled = isEnabled;
            this.xToolStripMenuItem.Enabled = isEnabled;
            this.xToolStripMenuItem1.Enabled = isEnabled;
            this.xToolStripMenuItem2.Enabled = isEnabled;
            this.xToolStripMenuItem3.Enabled = isEnabled;
            this.whiteBackgroundColorMenuItem.Enabled = isEnabled;
            this.redBackgroundColorMenuItem.Enabled = isEnabled;
            this.blueBackgroundColorMenuItem.Enabled = isEnabled;
            this.transparentBackgroundColorMenuItem.Enabled = isEnabled;
        }

        private void EnableButtons()
        {
            UpdateButtons(true);
        }

        private void DisableButtons(object sender, EventArgs e)
        {
            UpdateButtons(false);
        }

        private void WebView2Control_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            UpdateTitleWithEvent("NavigationStarting");
        }

        private async void WebView2Control_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            UpdateTitleWithEvent("NavigationCompleted");

            if (_isRenderingEditorPreview)
            {
                _isRenderingEditorPreview = false;
                return;
            }

            if (!_syncEditorFromNavigation || !e.IsSuccess)
                return;

            _syncEditorFromNavigation = false;
            await SyncEditorWithCurrentPageAsync();
        }

        private void WebView2Control_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (_isRenderingEditorPreview)
                return;

            if (webView2Control.Source != null)
                txtUrl.Text = webView2Control.Source.AbsoluteUri;
        }

        private void WebView2Control_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show($"WebView2 creation failed with exception = {e.InitializationException}");
                UpdateTitleWithEvent("CoreWebView2InitializationCompleted failed");
                return;
            }

            // Setup host resource mapping for local files
            this.webView2Control.CoreWebView2.SetVirtualHostNameToFolderMapping("appassets.example", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            _syncEditorFromNavigation = true;
            this.webView2Control.Source = new Uri(GetStartPageUri(this.webView2Control.CoreWebView2));

            this.webView2Control.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
            this.webView2Control.CoreWebView2.HistoryChanged += CoreWebView2_HistoryChanged;
            this.webView2Control.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            this.webView2Control.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.Image, CoreWebView2WebResourceRequestSourceKinds.Document);
            this.webView2Control.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
            this.webView2Control.CoreWebView2.FrameCreated += WebView_HandleIFrames;

            UpdateTitleWithEvent("CoreWebView2InitializationCompleted succeeded");
            EnableButtons();
        }

        void AttachControlEventHandlers(Microsoft.Web.WebView2.WinForms.WebView2 control)
        {
            control.CoreWebView2InitializationCompleted += WebView2Control_CoreWebView2InitializationCompleted;
            control.NavigationStarting += WebView2Control_NavigationStarting;
            control.NavigationCompleted += WebView2Control_NavigationCompleted;
            control.SourceChanged += WebView2Control_SourceChanged;
            control.KeyDown += WebView2Control_KeyDown;
            control.KeyUp += WebView2Control_KeyUp;
            control.Disposed += DisableButtons;
        }

        private void WebView2Control_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateTitleWithEvent($"KeyUp key={e.KeyCode}");
            if (!this.acceleratorKeysEnabledToolStripMenuItem.Checked)
                e.Handled = true;
        }

        private void WebView2Control_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateTitleWithEvent($"KeyDown key={e.KeyCode}");
            if (!this.acceleratorKeysEnabledToolStripMenuItem.Checked)
                e.Handled = true;
        }

        private void CoreWebView2_HistoryChanged(object sender, object e)
        {
            // No explicit check for webView2Control initialization because the events can only start
            // firing after the CoreWebView2 and its events exist for us to subscribe.
            btnBack.Enabled = webView2Control.CoreWebView2.CanGoBack;
            btnForward.Enabled = webView2Control.CoreWebView2.CanGoForward;
            UpdateTitleWithEvent("HistoryChanged");
        }

        private void CoreWebView2_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (_isRenderingEditorPreview)
                return;

            if (this.webView2Control.Source != null)
                this.txtUrl.Text = this.webView2Control.Source.AbsoluteUri;
            UpdateTitleWithEvent("SourceChanged");
        }

        private void CoreWebView2_DocumentTitleChanged(object sender, object e)
        {
            this.Text = this.webView2Control.CoreWebView2.DocumentTitle;
            UpdateTitleWithEvent("DocumentTitleChanged");
        }
        #endregion

        #region UI event handlers
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            webView2Control.Reload();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            var rawUrl = txtUrl.Text?.Trim() ?? string.Empty;
            if (TryLoadLocalHtmlFile(rawUrl))
                return;

            Uri uri = null;

            if (Uri.IsWellFormedUriString(rawUrl, UriKind.Absolute))
            {
                uri = new Uri(rawUrl);
            }
            else if (!rawUrl.Contains(" ") && rawUrl.Contains("."))
            {
                // An invalid URI contains a dot and no spaces, try tacking http:// on the front.
                uri = new Uri("http://" + rawUrl);
            }
            else
            {
                // Otherwise treat it as a web search.
                uri = new Uri("https://bing.com/search?q=" +
                    String.Join("+", Uri.EscapeDataString(rawUrl).Split(new string[] { "%20" }, StringSplitOptions.RemoveEmptyEntries)));
            }

            _editorBaseUri = uri;
            _syncEditorFromNavigation = true;
            webView2Control.Source = uri;
            if (ShouldBlockUri())
            {
                _syncEditorFromNavigation = false;
                webView2Control.CoreWebView2.NavigateToString("You've attempted to navigate to a domain in the blocked sites list. Press back to return to the previous page.");
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            webView2Control.GoBack();
        }

        private void btnEvents_Click(object sender, EventArgs e)
        {
            (new EventMonitor(this.webView2Control)).Show(this);
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            webView2Control.GoForward();
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            BtnGo_Click(sender, EventArgs.Empty);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            HandleResize();
        }

        private void closeWebViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveWebViewFromHost();
            webView2Control.Dispose();
        }

        private void createWebViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            void EnsureProcessIsClose(uint pid)
            {
                try
                {
                    var process = Process.GetProcessById((int)pid);
                    process.Kill();
                }
                catch (ArgumentException)
                {
                    // Process already exited.
                }
            }
            if (this.webView2Control.CoreWebView2 != null)
            {
                var processId = this.webView2Control.CoreWebView2.BrowserProcessId;
                RemoveWebViewFromHost();
                this.webView2Control.Dispose();
                EnsureProcessIsClose(processId);
            }
            this.webView2Control = GetReplacementControl(false);
            PrepareWebViewControl(this.webView2Control);
            AddWebViewToHost();
            HandleResize();
        }

        private void createNewWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BrowserForm().Show();
        }

        private void createNewWindowWithOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new NewWindowOptionsDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                new BrowserForm(dialog.CreationProperties).Show();
            }
        }

        private void ThreadProc(CoreWebView2CreationProperties creationProperties)
        {
            try
            {
                var creationProps = new CoreWebView2CreationProperties();
                // The CoreWebView2CreationProperties object cannot be assigned directly, because its member _task will also be assigned.
                creationProps.BrowserExecutableFolder = creationProperties.BrowserExecutableFolder;
                creationProps.UserDataFolder = creationProperties.UserDataFolder;
                creationProps.Language = creationProperties.Language;
                creationProps.AdditionalBrowserArguments = creationProperties.AdditionalBrowserArguments;
                creationProps.ProfileName = creationProperties.ProfileName;
                creationProps.IsInPrivateModeEnabled = creationProperties.IsInPrivateModeEnabled;
                var tempForm = new BrowserForm(creationProps);
                tempForm.Show();
                // Run the message pump
                Application.Run();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Create New Thread Failed: " + exception.Message, "Create New Thread");
            }
        }

        private void createNewThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread newFormThread = new Thread(() =>
            {
                ThreadProc(webView2Control.CreationProperties);
            });
            newFormThread.SetApartmentState(ApartmentState.STA);
            newFormThread.IsBackground = false;
            newFormThread.Start();
        }

        private void xToolStripMenuItem05_Click(object sender, EventArgs e)
        {
            this.webView2Control.ZoomFactor = 0.5;
        }

        private void xToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.webView2Control.ZoomFactor = 1.0;
        }

        private void xToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.webView2Control.ZoomFactor = 2.0;
        }

        private void xToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Zoom factor: {this.webView2Control.ZoomFactor}", "WebView Zoom factor");
        }

        private void backgroundColorMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            Color backgroundColor = Color.FromName(menuItem.Text);
            this.webView2Control.DefaultBackgroundColor = backgroundColor;
        }

        private void taskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.webView2Control.CoreWebView2.OpenTaskManagerWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Open Task Manager Window failed");
            }
        }

        private async void methodCDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextInputDialog dialog = new TextInputDialog(
              title: "Call CDP Method",
              description: "Enter the CDP method name to call, followed by a space,\r\n" +
                "followed by the parameters in JSON format.",
              defaultInput: "Runtime.evaluate {\"expression\":\"alert(\\\"test\\\")\"}"
            );
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string[] words = dialog.inputBox().Trim().Split(' ');
                if (words.Length == 1 && words[0] == "")
                {
                    MessageBox.Show(this, "Invalid argument:" + dialog.inputBox(), "CDP Method call failed");
                    return;
                }
                string methodName = words[0];
                string methodParams = (words.Length == 2 ? words[1] : "{}");

                try
                {
                    string cdpResult = await this.webView2Control.CoreWebView2.CallDevToolsProtocolMethodAsync(methodName, methodParams);
                    MessageBox.Show(this, cdpResult, "CDP method call successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "CDP method call failed");
                }
            }
        }

        private void allowExternalDropMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.AllowExternalDrop = this.allowExternalDropMenuItem.Checked;
        }

        private void setUsersAgentMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new TextInputDialog(
                title: "SetUserAgent",
                description: "Enter UserAgent");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // <SetUserAgent>
                WebViewSettings.UserAgent = dialog.inputBox();
                // </SetUserAgent>
            }
        }

        private void getDocumentTitleMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(webView2Control.CoreWebView2.DocumentTitle, "Document Title");
        }

        private bool _isPrintToPdfInProgress = false;
        private async void portraitMenuItem_Click(object sender, EventArgs e)
        {
            if (_isPrintToPdfInProgress)
            {
                MessageBox.Show(this, "Print to PDF in progress", "Print To PDF");
                return;
            }
            try
            {
                // <PrintToPdf as Portrait>
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = "C:\\";
                saveFileDialog.Filter = "Pdf Files|*.pdf";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _isPrintToPdfInProgress = true;
                    bool isSuccessful = await webView2Control.CoreWebView2.PrintToPdfAsync(
                        saveFileDialog.FileName);
                    _isPrintToPdfInProgress = false;
                    string message = (isSuccessful) ?
                        "Print to PDF succeeded" : "Print to PDF failed";
                    MessageBox.Show(this, message, "Print To PDF Completed");
                }
                // </PrintToPdf as Portrait>
            }
            catch (NotImplementedException exception)
            {
                MessageBox.Show(this, "Print to PDF Failed: " + exception.Message,
                   "Print to PDF");
            }
        }

        private async void landscapeMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (_isPrintToPdfInProgress)
                {
                    MessageBox.Show(this, "Print to PDF in progress", "Print To PDF");
                    return;
                }
                try
                {
                    // <PrintToPdf as landscape>
                    CoreWebView2PrintSettings printSettings = WebViewEnvironment.CreatePrintSettings();
                    printSettings.Orientation = CoreWebView2PrintOrientation.Landscape;
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.InitialDirectory = "C:\\";
                    saveFileDialog.Filter = "Pdf Files|*.pdf";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        _isPrintToPdfInProgress = true;
                        bool isSuccessful = await webView2Control.CoreWebView2.PrintToPdfAsync(
                            saveFileDialog.FileName, printSettings);
                        _isPrintToPdfInProgress = false;
                        string message = (isSuccessful) ?
                            "Print to PDF succeeded" : "Print to PDF failed";
                        MessageBox.Show(this, message, "Print To PDF Completed");
                    }
                    // </PrintToPdf as landscape>
                }
                catch (NotImplementedException exception)
                {
                    MessageBox.Show(this, "Print to PDF Failed: " + exception.Message,
                       "Print to PDF");
                }
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (_isPrintToPdfInProgress)
            {
                var selection = MessageBox.Show(
                    "Print to PDF in progress. Continue closing?",
                    "Print to PDF", MessageBoxButtons.YesNo);
                if (selection == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void getUserDataFolderMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(WebViewEnvironment.UserDataFolder, "User Data Folder");
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "Get User Data Folder Failed: " + exception.Message, "User Data Folder");
            }
        }

        private void toggleVisibilityMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.Visible = this.toggleVisibilityMenuItem.Checked;
        }

        private void toggleCustomServerCertificateSupportMenuItem_Click(object sender, EventArgs e)
        {
            ToggleCustomServerCertificateSupport();
        }

        private void clearServerCertificateErrorActionsMenuItem_Click(object sender, EventArgs e)
        {
            ClearServerCertificateErrorActions();
        }

        private void toggleDefaultScriptDialogsMenuItem_Click(object sender, EventArgs e)
        {

            WebViewSettings.AreDefaultScriptDialogsEnabled = !WebViewSettings.AreDefaultScriptDialogsEnabled;

            MessageBox.Show("Default script dialogs will be " + (WebViewSettings.AreDefaultScriptDialogsEnabled ? "enabled" : "disabled"), "after the next navigation.");
        }

        private void addRemoteObjectMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.webView2Control.CoreWebView2.AddHostObjectToScript("bridge", new BridgeAddRemoteObject());
            }
            catch (NotSupportedException exception)
            {
                MessageBox.Show("CoreWebView2.AddRemoteObject failed: " + exception.Message);
            }

            this.webView2Control.CoreWebView2.FrameCreated += (s, args) =>
            {
                if (args.Frame.Name.Equals("iframe_name"))
                {
                    try
                    {
                        string[] origins = new string[] { "https://appassets.example" };
                        args.Frame.AddHostObjectToScript("bridge", new BridgeAddRemoteObject(), origins);
                    }
                    catch (NotSupportedException exception)
                    {
                        MessageBox.Show("Frame.AddHostObjectToScript failed: " + exception.Message);
                    }
                }
                args.Frame.NameChanged += (nameChangedSender, nameChangedArgs) =>
                {
                    CoreWebView2Frame frame = (CoreWebView2Frame)nameChangedSender;
                    MessageBox.Show("Frame.NameChanged: " + frame.Name);
                };
                args.Frame.Destroyed += (frameDestroyedSender, frameDestroyedArgs) =>
                {
                    // Handle frame destroyed
                };
            };

            this.webView2Control.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "appassets.example", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            this.webView2Control.Source = new Uri("http://google.com");
        }

        // <DOMContentLoaded>
        private void domContentLoadedMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.CoreWebView2.DOMContentLoaded += WebView_DOMContentLoaded;
            this.webView2Control.CoreWebView2.FrameCreated += WebView_FrameCreatedDOMContentLoaded;
            this.webView2Control.NavigateToString(@"<!DOCTYPE html>" +
                                      "<h1>DOMContentLoaded sample page</h1>" +
                                      "<h2>The content to the iframe and below will be added after DOM content is loaded </h2>" +
                                      "<iframe style='height: 200px; width: 100%;'/>");
            this.webView2Control.CoreWebView2.NavigationCompleted += (s, args) =>
            {
                this.webView2Control.CoreWebView2.DOMContentLoaded -= WebView_DOMContentLoaded;
                this.webView2Control.CoreWebView2.FrameCreated -= WebView_FrameCreatedDOMContentLoaded;
            };
        }
        void WebView_DOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs arg)
        {
            _ = this.webView2Control.ExecuteScriptAsync(
                    "let content = document.createElement(\"h2\");" +
                    "content.style.color = 'blue';" +
                    "content.textContent = \"This text was added by the host app\";" +
                    "document.body.appendChild(content);");
        }
        void WebView_FrameCreatedDOMContentLoaded(object sender, CoreWebView2FrameCreatedEventArgs args)
        {
            args.Frame.DOMContentLoaded += (frameSender, DOMContentLoadedArgs) =>
            {
                args.Frame.ExecuteScriptAsync(
                    "let content = document.createElement(\"h2\");" +
                    "content.style.color = 'blue';" +
                    "content.textContent = \"This text was added to the iframe by the host app\";" +
                    "document.body.appendChild(content);");
            };
        }
        // </DOMContentLoaded>

        private void navigateWithWebResourceRequestMenuItem_Click(object sender, EventArgs e)
        {
            // <NavigateWithWebResourceRequest>
            // Prepare post data as UTF-8 byte array and convert it to stream
            // as required by the application/x-www-form-urlencoded Content-Type
            var dialog = new TextInputDialog(
                title: "NavigateWithWebResourceRequest",
                description: "Specify post data to submit to https://www.w3schools.com/action_page.php.");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string postDataString = "input=" + dialog.inputBox();
                UTF8Encoding utfEncoding = new UTF8Encoding();
                byte[] postData = utfEncoding.GetBytes(postDataString);
                MemoryStream postDataStream = new MemoryStream(postDataString.Length);
                postDataStream.Write(postData, 0, postData.Length);
                postDataStream.Seek(0, SeekOrigin.Begin);
                CoreWebView2WebResourceRequest webResourceRequest =
                  WebViewEnvironment.CreateWebResourceRequest(
                    "https://www.w3schools.com/action_page.php",
                    "POST",
                    postDataStream,
                    "Content-Type: application/x-www-form-urlencoded\r\n");
                this.webView2Control.CoreWebView2.NavigateWithWebResourceRequest(webResourceRequest);
            }
            // </NavigateWithWebResourceRequest>
        }

        // <WebMessage>
        private void webMessageMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
            this.webView2Control.CoreWebView2.FrameCreated += WebView_FrameCreatedWebMessages;
            this.webView2Control.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "appassets.example", "assets", CoreWebView2HostResourceAccessKind.DenyCors);
            this.webView2Control.Source = new Uri("http://google.com");
        }

        void HandleWebMessage(CoreWebView2WebMessageReceivedEventArgs args, CoreWebView2Frame frame = null)
        {
            try
            {
                if (args.Source != "http://google.com")
                {
                    // Throw exception from untrusted sources.
                    throw new Exception();
                }

                string message = args.TryGetWebMessageAsString();

                if (message.Contains("SetTitleText"))
                {
                    int msgLength = "SetTitleText".Length;
                    this.Text = message.Substring(msgLength);
                }
                else if (message == "GetWindowBounds")
                {
                    string reply = "{\"WindowBounds\":\"Left:" + 0 +
                                   "\\nTop:" + 0 +
                                   "\\nRight:" + this.webView2Control.Width +
                                   "\\nBottom:" + this.webView2Control.Height +
                                   "\"}";
                    if (frame != null)
                    {
                        frame.PostWebMessageAsJson(reply);
                    }
                    else
                    {
                        this.webView2Control.CoreWebView2.PostWebMessageAsJson(reply);
                    }
                }
                else
                {
                    // Ignore unrecognized messages, but log them
                    // since it suggests a mismatch between the web content and the host.
                    Debug.WriteLine($"Unexpected message received: {message}");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Unexpected message received: {e.Message}");
            }
        }

        void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            HandleWebMessage(args);
        }

        // <WebMessageReceivedIFrame>
        void WebView_FrameCreatedWebMessages(object sender, CoreWebView2FrameCreatedEventArgs args)
        {
            args.Frame.WebMessageReceived += (WebMessageReceivedSender, WebMessageReceivedArgs) =>
            {
                HandleWebMessage(WebMessageReceivedArgs, args.Frame);
            };
        }
        // </WebMessageReceivedIFrame>
        // </WebMessage>
        private void toggleMuteStateMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.CoreWebView2.IsMuted = !this.webView2Control.CoreWebView2.IsMuted;
            MessageBox.Show("Mute state will be " + (this.webView2Control.CoreWebView2.IsMuted ? "enabled" : "disabled"), "Mute");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "WebView2WindowsFormsBrowser, Version 1.0\nCopyright(C) 2023", "About WebView2WindowsFormsBrowser");
        }

        void AuthenticationMenuItem_Click(object sender, EventArgs e)
        {
            // <BasicAuthenticationRequested>
            this.webView2Control.CoreWebView2.BasicAuthenticationRequested += delegate (object requestSender, CoreWebView2BasicAuthenticationRequestedEventArgs args)
            {
                // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="Demo credentials in https://authenticationtest.com")]
                args.Response.UserName = "user";
                // [SuppressMessage("Microsoft.Security", "CS002:SecretInNextLine", Justification="Demo credentials in https://authenticationtest.com")]
                args.Response.Password = "pass";
            };
            this.webView2Control.CoreWebView2.Navigate("https://authenticationtest.com/HTTPAuth");
            // </BasicAuthenticationRequested>
        }
        private void ClearAllDOMStorage(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.AllDomStorage);
        }
        private void ClearAllProfileMenuItem_Click(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.AllProfile);
        }

        private void ClearAllSiteMenuItem_Click(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.AllSite);
        }

        private void ClearAutofillMenuItem_Click(object sender, EventArgs e)
        {
            var kinds = CoreWebView2BrowsingDataKinds.GeneralAutofill |
                        CoreWebView2BrowsingDataKinds.PasswordAutosave;
            ClearBrowsingData(sender, e, kinds);
        }

        private void ClearBrowsingHistory(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.BrowsingHistory);
        }

        private void ClearCookies(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.Cookies);
        }

        private void ClearDiskCache(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.DiskCache);
        }

        private void ClearDownloadHistory(object sender, EventArgs e)
        {
            ClearBrowsingData(sender, e, CoreWebView2BrowsingDataKinds.DownloadHistory);
        }

        // These three wrap your existing methods that take extra parameters:
        private void GetCookies(object sender, EventArgs e)
        {
            GetCookiesMenuItem_Click(sender, e, txtUrl.Text);
        }

        private void AddOrUpdateCookie(object sender, EventArgs e)
        {
            var host = new Uri(txtUrl.Text).Host;
            AddOrUpdateCookieMenuItem_Click(sender, e, host);
        }

        private void DeleteCookies(object sender, EventArgs e)
        {
            var host = new Uri(txtUrl.Text).Host;
            DeleteCookiesMenuItem_Click(sender, e, host);
        }
        async void ClearBrowsingData(object target, EventArgs e, CoreWebView2BrowsingDataKinds dataKinds)
        {
            // Clear the browsing data from the last hour.
            await this.webView2Control.CoreWebView2.Profile.ClearBrowsingDataAsync(dataKinds);
            MessageBox.Show(this,
                "Completed",
                "Clear Browsing Data");
            // </ClearBrowsingData>
        }

        void WebView_ClientCertificateRequested(object sender, CoreWebView2ClientCertificateRequestedEventArgs e)
        {
            IReadOnlyList<CoreWebView2ClientCertificate> certificateList = e.MutuallyTrustedCertificates;
            if (certificateList.Count() > 0)
            {
                // There is no significance to the order, picking a certificate arbitrarily.
                e.SelectedCertificate = certificateList.LastOrDefault();
            }
            e.Handled = true;
        }

        private bool _isCustomClientCertificateSelection = false;
        void CustomClientCertificateSelectionMenuItem_Click(object sender, EventArgs e)
        {
            // Safeguarding the handler when unsupported runtime is used.
            try
            {
                if (!_isCustomClientCertificateSelection)
                {
                    this.webView2Control.CoreWebView2.ClientCertificateRequested += WebView_ClientCertificateRequested;
                }
                else
                {
                    this.webView2Control.CoreWebView2.ClientCertificateRequested -= WebView_ClientCertificateRequested;
                }
                _isCustomClientCertificateSelection = !_isCustomClientCertificateSelection;

                MessageBox.Show(this,
                    _isCustomClientCertificateSelection ? "Custom client certificate selection has been enabled" : "Custom client certificate selection has been disabled",
                    "Custom client certificate selection");
            }
            catch (NotImplementedException exception)
            {
                MessageBox.Show(this, "Custom client certificate selection Failed: " + exception.Message, "Custom client certificate selection");
            }
        }
        // <ClientCertificateRequested2>
        // This example hides the default client certificate dialog and shows a custom dialog instead.
        // The dialog box displays mutually trusted certificates list and allows the user to select a certificate.
        // Selecting `OK` will continue the request with a certificate.
        // Selecting `CANCEL` will continue the request without a certificate
        private bool _isCustomClientCertificateSelectionDialog = false;
        void DeferredCustomCertificateDialogMenuItem_Click(object sender, EventArgs e)
        {
            // Safeguarding the handler when unsupported runtime is used.
            try
            {
                if (!_isCustomClientCertificateSelectionDialog)
                {
                    this.webView2Control.CoreWebView2.ClientCertificateRequested += delegate (
                        object requestSender, CoreWebView2ClientCertificateRequestedEventArgs args)
                    {
                        // Developer can obtain a deferral for the event so that the WebView2
                        // doesn't examine the properties we set on the event args until
                        // after the deferral completes asynchronously.
                        CoreWebView2Deferral deferral = args.GetDeferral();

                        System.Threading.SynchronizationContext.Current.Post((_) =>
                        {
                            using (deferral)
                            {
                                IReadOnlyList<CoreWebView2ClientCertificate> certificateList = args.MutuallyTrustedCertificates;
                                if (certificateList.Count() > 0)
                                {
                                    // Display custom dialog box for the client certificate selection.
                                    var dialog = new ClientCertificateSelectionDialog(
                                                                title: "Select a Certificate for authentication",
                                                                host: args.Host,
                                                                port: args.Port,
                                                                client_cert_list: certificateList);
                                    if (dialog.ShowDialog() == DialogResult.OK)
                                    {
                                        // Continue with the selected certificate to respond to the server if `OK` is selected.
                                        args.SelectedCertificate = (CoreWebView2ClientCertificate)dialog.CertificateDataBinding.SelectedItems[0].Tag;
                                    }
                                }
                                args.Handled = true;
                            }

                        }, null);
                    };
                    _isCustomClientCertificateSelectionDialog = true;
                    MessageBox.Show("Custom Client Certificate selection dialog will be used next when WebView2 is making a " +
                        "request to an HTTP server that needs a client certificate.", "Client certificate selection");
                }
            }
            catch (NotImplementedException exception)
            {
                MessageBox.Show(this, "Custom client certificate selection dialog Failed: " + exception.Message, "Client certificate selection");
            }
        }

        async void GetCookiesMenuItem_Click(object sender, EventArgs e, string address)
        {
            // <GetCookies>
            List<CoreWebView2Cookie> cookieList = await this.webView2Control.CoreWebView2.CookieManager.GetCookiesAsync(address);
            StringBuilder cookieResult = new StringBuilder(cookieList.Count + " cookie(s) received from " + address);
            for (int i = 0; i < cookieList.Count; ++i)
            {
                CoreWebView2Cookie cookie = this.webView2Control.CoreWebView2.CookieManager.CreateCookieWithSystemNetCookie(cookieList[i].ToSystemNetCookie());
                cookieResult.Append($"\n{cookie.Name} {cookie.Value} {(cookie.IsSession ? "[session cookie]" : cookie.Expires.ToString("G"))}");
            }
            MessageBox.Show(this, cookieResult.ToString(), "GetCookiesAsync");
            // </GetCookies>
        }

        void AddOrUpdateCookieMenuItem_Click(object sender, EventArgs e, string domain)
        {
            // <AddOrUpdateCookie>
            CoreWebView2Cookie cookie = this.webView2Control.CoreWebView2.CookieManager.CreateCookie("CookieName", "CookieValue", domain, "/");
            this.webView2Control.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
            // </AddOrUpdateCookie>
        }

        void DeleteAllCookiesMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.CoreWebView2.CookieManager.DeleteAllCookies();
        }

        void DeleteCookiesMenuItem_Click(object sender, EventArgs e, string domain)
        {
            this.webView2Control.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("CookieName", domain, "/");
        }

        private void showBrowserProcessInfoMenuItem_Click(object sender, EventArgs e)
        {
            var browserInfo = this.webView2Control.CoreWebView2.BrowserProcessId;
            MessageBox.Show(this, "Browser ID: " + browserInfo.ToString(), "Process ID");
        }

        private void showPerformanceInfoMenuItem_Click(object sender, EventArgs e)
        {
            var processInfoList = WebViewEnvironment.GetProcessInfos();
            var processListCount = processInfoList.Count;
            string message = "";
            if (processListCount == 0)
            {
                message = "No process found.";
            }
            else
            {
                message = $"{processListCount} processes found:\n\n";
                for (int i = 0; i < processListCount; ++i)
                {
                    int processId = processInfoList[i].ProcessId;
                    CoreWebView2ProcessKind processKind = processInfoList[i].Kind;
                    var proc = Process.GetProcessById(processId);
                    var memoryInBytes = proc.PrivateMemorySize64;
                    var b2kb = memoryInBytes / 1024;
                    message += $"Process ID: {processId}, Process Kind: {processKind}, Memory Usage: {b2kb} KB\n";
                }
            }
            MessageBox.Show(this, message, "Process Info");
        }
        // <ProcessFailed>
        // Register a handler for the ProcessFailed event.
        // This handler checks the failure kind and tries to:
        //   * Recreate the webview for browser failure and render unresponsive.
        //   * Reload the webview for render failure.
        //   * Reload the webview for frame-only render failure impacting app content.
        //   * Log information about the failure for other failures.
        private void CoreWebView2_ProcessFailed(object sender, CoreWebView2ProcessFailedEventArgs e)
        {
            void ReinitIfSelectedByUser(string caption, string message)
            {
                this.webView2Control.BeginInvoke(new Action(() =>
                {
                    var selection = MessageBox.Show(this, message, caption, MessageBoxButtons.YesNo);
                    if (selection == DialogResult.Yes)
                    {
                        RemoveWebViewFromHost();
                        this.webView2Control.Dispose();
                        this.webView2Control = GetReplacementControl(false);
                        PrepareWebViewControl(this.webView2Control);
                        AddWebViewToHost();
                        HandleResize();
                    }
                }));
            }

            void ReloadIfSelectedByUser(string caption, string message)
            {
                this.webView2Control.BeginInvoke(new Action(() =>
                {
                    var selection = MessageBox.Show(this, message, caption, MessageBoxButtons.YesNo);
                    if (selection == DialogResult.Yes)
                    {
                        this.webView2Control.CoreWebView2.Reload();
                    }
                }));
            }

            this.webView2Control.Invoke(new Action(() =>
            {
                StringBuilder messageBuilder = new StringBuilder();
                messageBuilder.AppendLine($"Process kind: {e.ProcessFailedKind}");
                messageBuilder.AppendLine($"Reason: {e.Reason}");
                messageBuilder.AppendLine($"Exit code: {e.ExitCode}");
                messageBuilder.AppendLine($"Process description: {e.ProcessDescription}");
                MessageBox.Show(messageBuilder.ToString(), "Child process failed", MessageBoxButtons.OK);
            }));

            if (e.ProcessFailedKind == CoreWebView2ProcessFailedKind.BrowserProcessExited)
            {
                ReinitIfSelectedByUser("Browser process exited",
                    "Browser process exited unexpectedly. Recreate webview?");
            }
            else if (e.ProcessFailedKind == CoreWebView2ProcessFailedKind.RenderProcessUnresponsive)
            {
                ReinitIfSelectedByUser("Web page unresponsive",
                    "Browser render process has stopped responding. Recreate webview?");
            }
            else if (e.ProcessFailedKind == CoreWebView2ProcessFailedKind.RenderProcessExited)
            {
                ReloadIfSelectedByUser("Web page unresponsive",
                    "Browser render process exited unexpectedly. Reload page?");
            }
        }
        WebView2 GetReplacementControl(bool useNewEnvironment)
        {
            WebView2 webView = this.webView2Control;
            WebView2 replacementControl = new WebView2();
            ((System.ComponentModel.ISupportInitialize)(replacementControl)).BeginInit();
            // Setup properties.
            if (useNewEnvironment)
            {
                // Create a new CoreWebView2CreationProperties instance so the environment
                // is made anew.
                replacementControl.CreationProperties = new CoreWebView2CreationProperties();
                replacementControl.CreationProperties.BrowserExecutableFolder = webView.CreationProperties.BrowserExecutableFolder;
                replacementControl.CreationProperties.Language = webView.CreationProperties.Language;
                replacementControl.CreationProperties.UserDataFolder = webView.CreationProperties.UserDataFolder;
                replacementControl.CreationProperties.AdditionalBrowserArguments = webView.CreationProperties.AdditionalBrowserArguments;
            }
            else
            {
                replacementControl.CreationProperties = webView.CreationProperties;
            }
            AttachControlEventHandlers(replacementControl);
            replacementControl.Source = webView.Source ?? new Uri("https://www.bing.com");
            ((System.ComponentModel.ISupportInitialize)(replacementControl)).EndInit();

            return replacementControl;
        }
        // </ProcessFailed>

        // Crash the browser's process on command, to test crash handlers.
        private void crashBrowserProcessMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.CoreWebView2.Navigate("edge://inducebrowsercrashforrealz");
        }

        // Crash the browser's render process on command, to test crash handlers.
        private void crashRendererProcessMenuItem_Click(object sender, EventArgs e)
        {
            this.webView2Control.CoreWebView2.Navigate("edge://kill");
        }

        // <ServerCertificateError>
        // When WebView2 doesn't trust a TLS certificate but host app does, this example bypasses
        // the default TLS interstitial page using the ServerCertificateErrorDetected event handler and
        // continues the request to a server. Otherwise, cancel the request.
        private bool _enableServerCertificateError = false;
        private void ToggleCustomServerCertificateSupport()
        {
            // Safeguarding the handler when unsupported runtime is used.
            try
            {
                if (!_enableServerCertificateError)
                {
                    this.webView2Control.CoreWebView2.ServerCertificateErrorDetected += WebView_ServerCertificateErrorDetected;
                }
                else
                {
                    this.webView2Control.CoreWebView2.ServerCertificateErrorDetected -= WebView_ServerCertificateErrorDetected;
                }
                _enableServerCertificateError = !_enableServerCertificateError;

                MessageBox.Show(this, "Custom server certificate support has been " +
                    (_enableServerCertificateError ? "enabled" : "disabled"),
                    "Custom server certificate support");
            }
            catch (NotImplementedException exception)
            {
                MessageBox.Show(this, "Custom server certificate support failed: " + exception.Message, "Custom server certificate support");
            }
        }

        private void WebView_ServerCertificateErrorDetected(object sender, CoreWebView2ServerCertificateErrorDetectedEventArgs e)
        {
            CoreWebView2Certificate certificate = e.ServerCertificate;

            // Continues the request to a server with a TLS certificate if the error status
            // is of type `COREWEBVIEW2_WEB_ERROR_STATUS_CERTIFICATE_IS_INVALID`
            // and trusted by the host app.
            if (e.ErrorStatus == CoreWebView2WebErrorStatus.CertificateIsInvalid &&
                            ValidateServerCertificate(certificate))
            {
                e.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
            }
            else
            {
                // Cancel the request for other TLS certificate error types or if untrusted by the host app.
                e.Action = CoreWebView2ServerCertificateErrorAction.Cancel;
            }
        }

        // Function to validate the server certificate for untrusted root or self-signed certificate.
        // You may also choose to defer server certificate validation.
        bool ValidateServerCertificate(CoreWebView2Certificate certificate)
        {
            // You may want to validate certificates in different ways depending on your app and
            // scenario. One way might be the following:
            // First, get the list of host app trusted certificates and its thumbprint.
            //
            // Then get the last chain element using `ICoreWebView2Certificate::get_PemEncodedIssuerCertificateChain`
            // that contains the raw data of the untrusted root CA/self-signed certificate. Get the untrusted
            // root CA/self signed certificate thumbprint from the raw certificate data and validate the thumbprint
            // against the host app trusted certificate list.
            //
            // Finally, return true if it exists in the host app's certificate trusted list, or otherwise return false.
            return true;
        }

        // This example clears `AlwaysAllow` response that are added for proceeding with TLS certificate errors.
        async void ClearServerCertificateErrorActions()
        {
            await this.webView2Control.CoreWebView2.ClearServerCertificateErrorActionsAsync();
            MessageBox.Show(this, "message", "Clear server certificate error actions are succeeded");
        }
        // </ServerCertificateError>

        // Prompt the user for a list of blocked domains
        private bool _blockedSitesSet = false;
        private HashSet<string> _blockedSitesList = new HashSet<string>();
        private void blockedDomainsMenuItem_Click(object sender, EventArgs e)
        {
            var blockedSitesString = "";
            if (_blockedSitesSet)
            {
                blockedSitesString = String.Join(";", _blockedSitesList);
            }
            else
            {
                blockedSitesString = "foo.com;bar.org";
            }
            var textDialog = new TextInputDialog(
                title: "Blocked Domains",
                description: "Enter hostnames to block, sparately by semicolons",
                defaultInput: blockedSitesString);
            if (textDialog.ShowDialog() == DialogResult.OK)
            {
                _blockedSitesSet = true;
                _blockedSitesList.Clear();
                if (textDialog.inputBox() != null)
                {
                    string[] textcontent = textDialog.inputBox().Split(';');
                    foreach (string site in textcontent)
                    {
                        _blockedSitesList.Add(site);
                    }
                }
            }

        }

        // Check the URI against the blocked sites list
        private bool ShouldBlockUri()
        {
            foreach (string site in _blockedSitesList)
            {
                if (site.Equals(txtUrl.Text))
                {
                    return true;
                }
            }
            return false;
        }

        private async void injectScriptMenuItem_Click(object sender, EventArgs e)
        {
            // <ExecuteScript>
            var dialog = new TextInputDialog(
                title: "Inject Script",
                description: "Enter some JavaScript to be executed in the context of this page.",
                defaultInput: "window.getComputedStyle(document.body).backgroundColor");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string scriptResult = await this.webView2Control.ExecuteScriptAsync(dialog.inputBox());
                    MessageBox.Show(this, scriptResult, "Script Result");
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(this, ex.Message, "Execute Script Fails");
                }
            }
            // </ExecuteScript>
        }

        private async void injectScriptIntoFrameMenuItem_Click(object sender, EventArgs e)
        {
            // <ExecuteScriptFrame>
            string iframesData = WebViewFrames_ToString();
            string iframesInfo = "Enter iframe to run the JavaScript code in.\r\nAvailable iframes: " + iframesData;
            var dialogIFrames = new TextInputDialog(
                title: "Inject Script Into IFrame",
                description: iframesInfo,
                defaultInput: "0");
            if (dialogIFrames.ShowDialog() == DialogResult.OK)
            {
                int iframeNumber = -1;
                try
                {
                    iframeNumber = Int32.Parse(dialogIFrames.inputBox());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Can not convert " + dialogIFrames.inputBox() + " to int");
                }
                if (iframeNumber >= 0 && iframeNumber < _webViewFrames.Count)
                {
                    var dialog = new TextInputDialog(
                        title: "Inject Script",
                        description: "Enter some JavaScript to be executed in the context of iframe " + dialogIFrames.inputBox(),
                        defaultInput: "window.getComputedStyle(document.body).backgroundColor");
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string scriptResult = await _webViewFrames[iframeNumber].ExecuteScriptAsync(dialog.inputBox());
                            MessageBox.Show(this, scriptResult, "Script Result");
                        }
                        catch (InvalidOperationException ex)
                        {
                            MessageBox.Show(this, ex.Message, "Execute Script Frame Fails");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Can not read frame index or it is out of available range");
                }
            }
            // </ExecuteScriptFrame>
        }

        // Prompt the user for some scripts and register it to execute whenever a new page loads.
        private async void addInitializeScriptMenuItem_Click(object sender, EventArgs e)
        {
            TextInputDialog dialog = new TextInputDialog(
              title: "Add Initialize Script",
              description: "Enter the JavaScript code to run as the initialization script that runs before any script in the HTML document.",
              // This example script stops child frames from opening new windows.  Because
              // the initialization script runs before any script in the HTML document, we
              // can trust the results of our checks on window.parent and window.top.
              defaultInput: "if (window.parent !== window.top) {\r\n" +
              "    delete window.open;\r\n" +
              "}");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string scriptId = await this.webView2Control.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(dialog.inputBox());
                    _lastInitializeScriptId = scriptId;
                    MessageBox.Show(this, scriptId, "AddScriptToExecuteOnDocumentCreated Id");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.ToString(), "AddScriptToExecuteOnDocumentCreated failed");
                }
            }
        }

        // Prompt the user for an initialization script ID and deregister that script.
        private void removeInitializeScriptMenuItem_Click(object sender, EventArgs e)
        {
            TextInputDialog dialog = new TextInputDialog(
              title: "Remove Initialize Script",
              description: "Enter the ID created from Add Initialize Script.",
              defaultInput: _lastInitializeScriptId);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string scriptId = dialog.inputBox();
                // check valid
                try
                {
                    Int64 result = Int64.Parse(scriptId);
                    Int64 lastId = Int64.Parse(_lastInitializeScriptId);
                    if (result > lastId)
                    {
                        MessageBox.Show(this, scriptId, "Invalid ScriptId, should be less or equal than " + _lastInitializeScriptId);
                    }
                    else
                    {
                        this.webView2Control.CoreWebView2.RemoveScriptToExecuteOnDocumentCreated(scriptId);
                        if (result == lastId && lastId >= 2)
                        {
                            _lastInitializeScriptId = (lastId - 1).ToString();
                        }
                        MessageBox.Show(this, scriptId, "RemoveScriptToExecuteOnDocumentCreated Id");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show(this, scriptId, "Invalid ScriptId, should be Integer");

                }
            }
        }

        // Prompt the user for a string and then post it as a web message.
        private void postMessageStringMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new TextInputDialog(
                title: "Post Web Message String",
                description: "Web message string:\r\nEnter the web message as a string.");

            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.webView2Control.CoreWebView2.PostWebMessageAsString(dialog.inputBox());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "PostMessageAsString Failed: " + exception.Message,
                   "Post Message As String");
            }
        }

        // Prompt the user for some JSON and then post it as a web message.
        private void postMessageJsonMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new TextInputDialog(
                title: "Post Web Message JSON",
                description: "Web message JSON:\r\nEnter the web message as JSON.",
                 defaultInput: "{\"SetColor\":\"blue\"}");

            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.webView2Control.CoreWebView2.PostWebMessageAsJson(dialog.inputBox());
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "PostMessageAsJSON Failed: " + exception.Message,
                   "Post Message As JSON");
            }
        }

        // Prompt the user for a string and then post it as a web message to the first iframe.
        private void postMessageStringIframeMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new TextInputDialog(
                title: "Post Web Message String Iframe",
                description: "Web message string:\r\nEnter the web message as a string.");

            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (_webViewFrames.Count != 0)
                    {
                        _webViewFrames[0].PostWebMessageAsString(dialog.inputBox());
                    }
                    else
                    {
                        MessageBox.Show("No iframes found");
                    }
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "PostMessageAsStringIframe Failed: " + exception.Message,
                   "Post Message As String");
            }
        }

        // Prompt the user for some JSON and then post it as a web message to the first iframe.
        private void postMessageJsonIframeMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new TextInputDialog(
                title: "Post Web Message JSON Iframe",
                description: "Web message JSON:\r\nEnter the web message as JSON.",
                 defaultInput: "{\"SetColor\":\"blue\"}");

            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (_webViewFrames.Count != 0)
                    {
                        _webViewFrames[0].PostWebMessageAsJson(dialog.inputBox());
                    }
                    else
                    {
                        MessageBox.Show("No iframes found");
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, "PostMessageAsJSONIframe Failed: " + exception.Message,
                   "Post Message As JSON");
            }
        }
        #endregion

        private void openHtmlButton_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*",
                Title = "Open HTML File"
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadHtmlFileIntoEditor(dialog.FileName);
        }

        private void formatHtmlButton_Click(object sender, EventArgs e)
        {
            if (_htmlEditor == null)
                return;

            SetEditorText(_htmlEditor.Text, formatHtml: true, resetCaret: false);
            _ = RenderEditorToPreviewAsync();
        }

        private void toggleVisualEditorButton_Click(object sender, EventArgs e)
        {
            SetVisualEditorPanelVisible(!_isVisualEditorPanelOpen);
        }

        private void SetVisualEditorPanelVisible(bool isVisible)
        {
            _isVisualEditorPanelOpen = isVisible;
            if (_designPreviewSplit != null && !_designPreviewSplit.IsDisposed)
                _designPreviewSplit.Panel1Collapsed = !isVisible;

            if (isVisible)
            {
                string html = _htmlEditor?.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(html))
                    html = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"></head><body></body></html>";
                _ = RenderHtmlToVisualEditorAsync(EnsureBaseTag(html));
            }
            else
            {
                _isRenderingVisualEditor = false;
                _ignoreNextVisualEditorMessage = false;
                _isSyncingSourceFromVisualEditor = false;
            }

            ApplyCommandButtonStyles();
            UpdateEditorSplitLayout();
        }

        private void darkThemeButton_Click(object sender, EventArgs e)
        {
            SetUiTheme(UiTheme.Dark);
        }

        private void lightThemeButton_Click(object sender, EventArgs e)
        {
            SetUiTheme(UiTheme.Light);
        }

        private void saveHtmlButton_Click(object sender, EventArgs e)
        {
            SaveEditorHtmlToFile();
        }

        private void SaveEditorHtmlToFile()
        {
            if (_htmlEditor == null)
                return;

            string suggestedPath = GetSuggestedHtmlSavePath();

            using var dialog = new SaveFileDialog
            {
                Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*",
                Title = "Save HTML File",
                AddExtension = true,
                DefaultExt = "html",
                OverwritePrompt = true
            };

            if (!string.IsNullOrWhiteSpace(suggestedPath))
            {
                string directory = Path.GetDirectoryName(suggestedPath);
                if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
                    dialog.InitialDirectory = directory;

                string fileName = Path.GetFileName(suggestedPath);
                if (!string.IsNullOrWhiteSpace(fileName))
                    dialog.FileName = fileName;
            }

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                File.WriteAllText(dialog.FileName, _htmlEditor.Text ?? string.Empty, Encoding.UTF8);
                _editorBaseUri = new Uri(dialog.FileName);
                _syncEditorFromNavigation = false;
                txtUrl.Text = dialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to save HTML file:\n" + ex.Message, "Save HTML");
            }
        }

        private string GetSuggestedHtmlSavePath()
        {
            if (_editorBaseUri?.IsFile == true)
                return _editorBaseUri.LocalPath;

            if (webView2Control?.Source?.IsFile == true)
                return webView2Control.Source.LocalPath;

            string input = txtUrl?.Text?.Trim() ?? string.Empty;
            if (Path.IsPathRooted(input))
            {
                try
                {
                    return Path.GetFullPath(input);
                }
                catch
                {
                    // Ignore invalid path and fall back to default file name.
                }
            }

            return "document.html";
        }

        private bool TryLoadLocalHtmlFile(string rawInput)
        {
            if (string.IsNullOrWhiteSpace(rawInput))
                return false;

            string candidatePath = rawInput;
            if (Uri.TryCreate(rawInput, UriKind.Absolute, out var uri) && uri.IsFile)
                candidatePath = uri.LocalPath;

            if (!Path.IsPathRooted(candidatePath))
                return false;

            var fullPath = Path.GetFullPath(candidatePath);
            if (!File.Exists(fullPath))
                return false;

            var ext = Path.GetExtension(fullPath);
            bool isHtml = ext.Equals(".html", StringComparison.OrdinalIgnoreCase) ||
                          ext.Equals(".htm", StringComparison.OrdinalIgnoreCase);
            if (!isHtml)
                return false;

            LoadHtmlFileIntoEditor(fullPath);
            return true;
        }

        private void LoadHtmlFileIntoEditor(string filePath)
        {
            try
            {
                string html = File.ReadAllText(filePath);
                _editorBaseUri = new Uri(filePath);
                _syncEditorFromNavigation = false;
                txtUrl.Text = filePath;
                SetEditorText(html, formatHtml: true);
                _ = RenderEditorToPreviewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Unable to open HTML file:\n" + ex.Message, "Open HTML");
            }
        }

        private void htmlEditor_TextChanged(object sender, EventArgs e)
        {
            if (_suppressEditorTextChanged || _isApplyingSyntaxHighlight || _editorPreviewTimer == null)
                return;

            _editorPreviewTimer.Stop();
            _editorPreviewTimer.Start();

            QueueEditorSyntaxHighlight(viewportOnly: false);
        }

        private void htmlEditor_VScroll(object sender, EventArgs e)
        {
            QueueViewportSyntaxHighlight();
        }

        private void htmlEditor_HScroll(object sender, EventArgs e)
        {
            QueueViewportSyntaxHighlight();
        }

        private void QueueEditorSyntaxHighlight(bool viewportOnly = false)
        {
            if (_editorSyntaxTimer == null || _suppressEditorTextChanged || _isApplyingSyntaxHighlight)
                return;

            _viewportOnlySyntaxHighlightPending = viewportOnly;
            _editorSyntaxTimer.Stop();
            _editorSyntaxTimer.Start();
        }

        private void QueueViewportSyntaxHighlight()
        {
            if (_htmlEditor == null || _htmlEditor.TextLength <= SyntaxHighlightMaxChars)
                return;

            QueueEditorSyntaxHighlight(viewportOnly: true);
        }

        private async void editorPreviewTimer_Tick(object sender, EventArgs e)
        {
            _editorPreviewTimer.Stop();
            await RenderEditorToPreviewAsync();
        }

        private void editorSyntaxTimer_Tick(object sender, EventArgs e)
        {
            _editorSyntaxTimer.Stop();
            bool viewportOnly = _viewportOnlySyntaxHighlightPending;
            _viewportOnlySyntaxHighlightPending = false;
            ApplyHtmlSyntaxHighlight(viewportOnly);
        }

        private async Task SyncEditorWithCurrentPageAsync()
        {
            if (webView2Control.CoreWebView2 == null || _htmlEditor == null)
                return;

            try
            {
                string html = await GetPageHtmlWithDocTypeAsync();
                _editorBaseUri = webView2Control.Source;
                SetEditorText(html, formatHtml: true);
                if (_isVisualEditorPanelOpen)
                    await RenderHtmlToVisualEditorAsync(EnsureBaseTag(html));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not read page source:\n" + ex.Message, "Load HTML Source");
            }
        }

        private async Task<string> GetPageHtmlWithDocTypeAsync()
        {
            var core = webView2Control.CoreWebView2;
            if (core == null)
                return string.Empty;

            const string script = @"(() => {
                const dt = document.doctype;
                let docType = '';
                if (dt) {
                    docType = '<!DOCTYPE ' + dt.name;
                    if (dt.publicId) {
                        docType += ' PUBLIC ""' + dt.publicId + '""';
                    } else if (dt.systemId) {
                        docType += ' SYSTEM';
                    }
                    if (dt.systemId) {
                        docType += ' ""' + dt.systemId + '""';
                    }
                    docType += '>';
                }
                return docType + '\n' + document.documentElement.outerHTML;
            })();";

            string raw = await core.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<string>(raw) ?? string.Empty;
        }

        private void SetEditorText(string html, bool formatHtml = false, bool resetCaret = true, bool applySyntaxHighlight = true)
        {
            if (_htmlEditor == null)
                return;

            string editorText = formatHtml ? FormatHtmlForEditor(html) : (html ?? string.Empty);
            _suppressEditorTextChanged = true;
            _htmlEditor.Text = editorText;
            if (resetCaret)
            {
                _htmlEditor.SelectionStart = 0;
                _htmlEditor.SelectionLength = 0;
            }
            _suppressEditorTextChanged = false;

            if (applySyntaxHighlight)
                ApplyHtmlSyntaxHighlight();
        }

        private static string FormatHtmlForEditor(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument
                {
                    OptionFixNestedTags = true
                };
                doc.LoadHtml(html);

                var sb = new StringBuilder(html.Length + (html.Length / 4));
                foreach (var node in doc.DocumentNode.ChildNodes)
                {
                    AppendFormattedNode(node, sb, 0);
                }

                string formatted = sb.ToString().Trim();
                return string.IsNullOrWhiteSpace(formatted) ? html : formatted;
            }
            catch
            {
                return html;
            }
        }

        private static void AppendFormattedNode(HtmlNode node, StringBuilder sb, int depth)
        {
            if (node == null)
                return;

            string indent = new string(' ', depth * 2);

            switch (node.NodeType)
            {
                case HtmlNodeType.Document:
                    foreach (var child in node.ChildNodes)
                        AppendFormattedNode(child, sb, depth);
                    return;

                case HtmlNodeType.Comment:
                    sb.Append(indent).Append("<!--").Append(node.InnerHtml).AppendLine("-->");
                    return;

                case HtmlNodeType.Text:
                    string rawText = node.InnerText;
                    if (string.IsNullOrWhiteSpace(rawText))
                        return;
                    sb.Append(indent).AppendLine(rawText.Trim());
                    return;

                case HtmlNodeType.Element:
                    break;

                default:
                    if (!string.IsNullOrWhiteSpace(node.OuterHtml))
                        sb.Append(indent).AppendLine(node.OuterHtml.Trim());
                    return;
            }

            string name = node.Name?.ToLowerInvariant() ?? string.Empty;
            bool preserveInner = name == "script" || name == "style" || name == "pre" || name == "textarea";
            bool hasChildren = node.ChildNodes.Count > 0;
            bool isVoid = IsVoidElement(name);
            string openTag = BuildOpenTag(node);

            if (!hasChildren || (isVoid && !preserveInner))
            {
                sb.Append(indent).AppendLine(openTag);
                return;
            }

            if (preserveInner)
            {
                sb.Append(indent).AppendLine(openTag);
                string formattedInner = FormatPreservedContent(name, node);
                AppendIndentedMultiline(sb, formattedInner, indent + "  ");
                sb.Append(indent).Append("</").Append(node.Name).AppendLine(">");
                return;
            }

            bool singleTextChild = node.ChildNodes.Count == 1 &&
                                   node.ChildNodes[0].NodeType == HtmlNodeType.Text &&
                                   !string.IsNullOrWhiteSpace(node.ChildNodes[0].InnerText);

            if (singleTextChild)
            {
                string content = node.ChildNodes[0].InnerText.Trim();
                sb.Append(indent).Append(openTag).Append(content).Append("</").Append(node.Name).AppendLine(">");
                return;
            }

            sb.Append(indent).AppendLine(openTag);
            foreach (var child in node.ChildNodes)
                AppendFormattedNode(child, sb, depth + 1);
            sb.Append(indent).Append("</").Append(node.Name).AppendLine(">");
        }

        private static string BuildOpenTag(HtmlNode node)
        {
            var sb = new StringBuilder();
            sb.Append('<').Append(node.Name);
            foreach (var attribute in node.Attributes)
            {
                sb.Append(' ').Append(attribute.Name).Append("=\"")
                    .Append((attribute.Value ?? string.Empty).Replace("\"", "&quot;"))
                    .Append('"');
            }
            sb.Append('>');
            return sb.ToString();
        }

        private static bool IsVoidElement(string name)
        {
            return name == "area" || name == "base" || name == "br" || name == "col" || name == "embed" ||
                   name == "hr" || name == "img" || name == "input" || name == "link" || name == "meta" ||
                   name == "param" || name == "source" || name == "track" || name == "wbr";
        }

        private static string FormatPreservedContent(string tagName, HtmlNode node)
        {
            string inner = node.InnerHtml ?? string.Empty;
            if (string.IsNullOrWhiteSpace(inner))
                return string.Empty;

            if (tagName == "script")
            {
                string type = node.GetAttributeValue("type", string.Empty)?.Trim().ToLowerInvariant() ?? string.Empty;
                if (type.Contains("json"))
                    return FormatJsonContent(inner);
                return FormatJavaScriptContent(inner);
            }

            if (tagName == "style")
                return FormatCssContent(inner);

            return NormalizeMultiline(inner);
        }

        private static string NormalizeMultiline(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            var sb = new StringBuilder(content.Length);
            using var reader = new StringReader(content);
            string line;
            bool first = true;
            while ((line = reader.ReadLine()) != null)
            {
                if (!first)
                    sb.Append('\n');
                sb.Append(line.TrimEnd());
                first = false;
            }
            return sb.ToString().Trim();
        }

        private static void AppendIndentedMultiline(StringBuilder sb, string content, string indent)
        {
            if (string.IsNullOrEmpty(content))
                return;

            using var reader = new StringReader(content);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length == 0)
                {
                    sb.AppendLine();
                }
                else
                {
                    sb.Append(indent).AppendLine(line);
                }
            }
        }

        private static string FormatJsonContent(string content)
        {
            try
            {
                using var doc = JsonDocument.Parse(content);
                return JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });
            }
            catch
            {
                return NormalizeMultiline(content);
            }
        }

        private static string FormatJavaScriptContent(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            if (input.Length > ScriptFormatMaxChars)
                return NormalizeMultiline(input);

            var sb = new StringBuilder(input.Length + (input.Length / 3));
            int indent = 0;
            int parenDepth = 0;
            bool atLineStart = true;
            bool inSingle = false;
            bool inDouble = false;
            bool inTemplate = false;
            bool inLineComment = false;
            bool inBlockComment = false;
            bool escape = false;

            void TrimTrailingSpaces()
            {
                while (sb.Length > 0 && (sb[sb.Length - 1] == ' ' || sb[sb.Length - 1] == '\t'))
                    sb.Length--;
            }

            void EnsureIndent()
            {
                if (!atLineStart)
                    return;
                sb.Append(' ', Math.Max(0, indent) * 2);
                atLineStart = false;
            }

            void AppendChar(char c)
            {
                EnsureIndent();
                sb.Append(c);
                atLineStart = false;
            }

            void NewLine()
            {
                TrimTrailingSpaces();
                if (sb.Length == 0 || sb[sb.Length - 1] != '\n')
                    sb.Append('\n');
                atLineStart = true;
            }

            bool IsWord(char c) => char.IsLetterOrDigit(c) || c == '_' || c == '$';

            char PeekNextNonWhitespace(int index)
            {
                for (int j = index + 1; j < input.Length; j++)
                {
                    char candidate = input[j];
                    if (!char.IsWhiteSpace(candidate))
                        return candidate;
                }
                return '\0';
            }

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char next = i + 1 < input.Length ? input[i + 1] : '\0';

                if (inLineComment)
                {
                    AppendChar(c);
                    if (c == '\n')
                    {
                        inLineComment = false;
                        atLineStart = true;
                    }
                    continue;
                }

                if (inBlockComment)
                {
                    AppendChar(c);
                    if (c == '*' && next == '/')
                    {
                        AppendChar('/');
                        i++;
                        inBlockComment = false;
                    }
                    continue;
                }

                if (inSingle || inDouble || inTemplate)
                {
                    AppendChar(c);
                    if (escape)
                    {
                        escape = false;
                        continue;
                    }

                    if (c == '\\')
                    {
                        escape = true;
                        continue;
                    }

                    if (inSingle && c == '\'')
                        inSingle = false;
                    else if (inDouble && c == '"')
                        inDouble = false;
                    else if (inTemplate && c == '`')
                        inTemplate = false;
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    if (c == '\n' || c == '\r')
                        continue;

                    char prev = sb.Length > 0 ? sb[sb.Length - 1] : '\0';
                    char nextNonWs = PeekNextNonWhitespace(i);
                    if (IsWord(prev) && IsWord(nextNonWs))
                        AppendChar(' ');
                    continue;
                }

                if (c == '/' && next == '/')
                {
                    EnsureIndent();
                    sb.Append("//");
                    atLineStart = false;
                    inLineComment = true;
                    i++;
                    continue;
                }

                if (c == '/' && next == '*')
                {
                    EnsureIndent();
                    sb.Append("/*");
                    atLineStart = false;
                    inBlockComment = true;
                    i++;
                    continue;
                }

                if (c == '\'')
                {
                    inSingle = true;
                    AppendChar(c);
                    continue;
                }

                if (c == '"')
                {
                    inDouble = true;
                    AppendChar(c);
                    continue;
                }

                if (c == '`')
                {
                    inTemplate = true;
                    AppendChar(c);
                    continue;
                }

                if (c == '(')
                {
                    parenDepth++;
                    AppendChar(c);
                    continue;
                }

                if (c == ')')
                {
                    parenDepth = Math.Max(0, parenDepth - 1);
                    AppendChar(c);
                    continue;
                }

                if (c == '{')
                {
                    AppendChar('{');
                    indent++;
                    NewLine();
                    continue;
                }

                if (c == '}')
                {
                    indent = Math.Max(0, indent - 1);
                    NewLine();
                    AppendChar('}');
                    char nextNonWs = PeekNextNonWhitespace(i);
                    if (nextNonWs != ';' && nextNonWs != ',' && nextNonWs != ')' && nextNonWs != '\0')
                        NewLine();
                    continue;
                }

                if (c == ';')
                {
                    AppendChar(';');
                    if (parenDepth == 0)
                        NewLine();
                    else
                        AppendChar(' ');
                    continue;
                }

                if (c == ',')
                {
                    AppendChar(',');
                    if (parenDepth <= 1)
                        AppendChar(' ');
                    continue;
                }

                AppendChar(c);
            }

            string formatted = sb.ToString().Trim();
            return CollapseExcessBlankLines(formatted);
        }

        private static string FormatCssContent(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            if (input.Length > CssFormatMaxChars)
                return NormalizeMultiline(input);

            var sb = new StringBuilder(input.Length + (input.Length / 3));
            int indent = 0;
            int parenDepth = 0;
            bool atLineStart = true;
            bool inSingle = false;
            bool inDouble = false;
            bool inComment = false;
            bool escape = false;

            void TrimTrailingSpaces()
            {
                while (sb.Length > 0 && (sb[sb.Length - 1] == ' ' || sb[sb.Length - 1] == '\t'))
                    sb.Length--;
            }

            void EnsureIndent()
            {
                if (!atLineStart)
                    return;
                sb.Append(' ', Math.Max(0, indent) * 2);
                atLineStart = false;
            }

            void AppendChar(char c)
            {
                EnsureIndent();
                sb.Append(c);
                atLineStart = false;
            }

            void NewLine()
            {
                TrimTrailingSpaces();
                if (sb.Length == 0 || sb[sb.Length - 1] != '\n')
                    sb.Append('\n');
                atLineStart = true;
            }

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char next = i + 1 < input.Length ? input[i + 1] : '\0';

                if (inComment)
                {
                    AppendChar(c);
                    if (c == '*' && next == '/')
                    {
                        AppendChar('/');
                        i++;
                        inComment = false;
                        NewLine();
                    }
                    continue;
                }

                if (inSingle || inDouble)
                {
                    AppendChar(c);
                    if (escape)
                    {
                        escape = false;
                        continue;
                    }

                    if (c == '\\')
                    {
                        escape = true;
                        continue;
                    }

                    if (inSingle && c == '\'')
                        inSingle = false;
                    else if (inDouble && c == '"')
                        inDouble = false;
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    if (c == '\n' || c == '\r')
                        continue;
                    if (sb.Length > 0 && sb[sb.Length - 1] != ' ' && sb[sb.Length - 1] != '\n')
                        AppendChar(' ');
                    continue;
                }

                if (c == '/' && next == '*')
                {
                    EnsureIndent();
                    sb.Append("/*");
                    atLineStart = false;
                    inComment = true;
                    i++;
                    continue;
                }

                if (c == '\'')
                {
                    inSingle = true;
                    AppendChar(c);
                    continue;
                }

                if (c == '"')
                {
                    inDouble = true;
                    AppendChar(c);
                    continue;
                }

                if (c == '(')
                {
                    parenDepth++;
                    AppendChar(c);
                    continue;
                }

                if (c == ')')
                {
                    parenDepth = Math.Max(0, parenDepth - 1);
                    AppendChar(c);
                    continue;
                }

                if (c == '{')
                {
                    AppendChar('{');
                    indent++;
                    NewLine();
                    continue;
                }

                if (c == '}')
                {
                    indent = Math.Max(0, indent - 1);
                    NewLine();
                    AppendChar('}');
                    NewLine();
                    continue;
                }

                if (c == ';')
                {
                    AppendChar(';');
                    if (parenDepth == 0)
                        NewLine();
                    continue;
                }

                if (c == ':')
                {
                    AppendChar(':');
                    if (next != ' ')
                        AppendChar(' ');
                    continue;
                }

                if (c == ',')
                {
                    AppendChar(',');
                    if (next != ' ')
                        AppendChar(' ');
                    continue;
                }

                AppendChar(c);
            }

            string formatted = sb.ToString().Trim();
            return CollapseExcessBlankLines(formatted);
        }

        private static string CollapseExcessBlankLines(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var sb = new StringBuilder(input.Length);
            int blankRun = 0;
            using var reader = new StringReader(input);
            string line;
            bool first = true;
            while ((line = reader.ReadLine()) != null)
            {
                bool blank = string.IsNullOrWhiteSpace(line);
                if (blank)
                {
                    blankRun++;
                    if (blankRun > 1)
                        continue;
                }
                else
                {
                    blankRun = 0;
                }

                if (!first)
                    sb.Append('\n');
                sb.Append(line.TrimEnd());
                first = false;
            }
            return sb.ToString();
        }

        private void ApplyHtmlSyntaxHighlight(bool viewportOnlyMode = false)
        {
            if (_htmlEditor == null || _isApplyingSyntaxHighlight)
                return;

            string text = _htmlEditor.Text ?? string.Empty;
            if (text.Length == 0)
                return;

            _isApplyingSyntaxHighlight = true;
            _suppressEditorTextChanged = true;
            _htmlEditor.SuspendLayout();
            try
            {
                int selectionStart = _htmlEditor.SelectionStart;
                int selectionLength = _htmlEditor.SelectionLength;
                var scrollPos = GetRichTextScrollPosition(_htmlEditor);
                SuspendControlPainting(_htmlEditor);

                int colorStart = 0;
                int colorLength = text.Length;
                if (text.Length > SyntaxHighlightMaxChars)
                {
                    int viewportStart = Math.Max(0, _htmlEditor.GetCharIndexFromPosition(new Point(1, 1)));
                    int viewportEnd = Math.Max(0, _htmlEditor.GetCharIndexFromPosition(new Point(
                        Math.Max(1, _htmlEditor.ClientSize.Width - 4),
                        Math.Max(1, _htmlEditor.ClientSize.Height - 4))));
                    if (viewportEnd < viewportStart)
                        viewportEnd = viewportStart;

                    int focusStart = viewportStart;
                    int focusEnd = viewportEnd;
                    if (!viewportOnlyMode)
                    {
                        int selectionEnd = selectionStart + selectionLength;
                        bool selectionNearViewport =
                            selectionEnd >= viewportStart - SyntaxHighlightViewportPadding &&
                            selectionStart <= viewportEnd + SyntaxHighlightViewportPadding;

                        if (selectionNearViewport)
                        {
                            focusStart = Math.Min(viewportStart, selectionStart);
                            focusEnd = Math.Max(viewportEnd, selectionEnd);
                        }
                    }

                    colorStart = Math.Max(0, focusStart - SyntaxHighlightViewportPadding);
                    int colorEnd = Math.Min(text.Length, Math.Max(focusEnd, colorStart) + SyntaxHighlightViewportPadding);
                    colorLength = Math.Max(0, colorEnd - colorStart);
                }

                if (colorLength > 0)
                {
                    string colorText = colorStart == 0 && colorLength == text.Length
                        ? text
                        : text.Substring(colorStart, colorLength);

                    ColorizeRange(colorStart, colorLength, GetEditorBaseSyntaxColor());
                    ApplyRegexColor(HtmlCommentPattern, colorText, colorStart, GetEditorCommentSyntaxColor());
                    ApplyRegexColor(HtmlDoctypePattern, colorText, colorStart, GetEditorCommentSyntaxColor());
                    ApplyHtmlTagColoring(colorText, colorStart);
                    ApplyRegexColor(HtmlEntityPattern, colorText, colorStart, GetEditorEntitySyntaxColor());
                    ApplyScriptBlockJavaScriptColoring(text, colorStart, colorLength, viewportOnlyMode);
                }

                RestoreEditorSelectionAndScroll(selectionStart, selectionLength, scrollPos);
            }
            finally
            {
                ResumeControlPainting(_htmlEditor);
                _htmlEditor.ResumeLayout();
                _suppressEditorTextChanged = false;
                _isApplyingSyntaxHighlight = false;
            }
        }

        private void RestoreEditorSelectionAndScroll(int selectionStart, int selectionLength, Point scrollPos)
        {
            if (_htmlEditor == null)
                return;

            int textLength = _htmlEditor.TextLength;
            int safeStart = Math.Clamp(selectionStart, 0, textLength);
            int safeLength = Math.Clamp(selectionLength, 0, Math.Max(0, textLength - safeStart));

            SetRichTextScrollPosition(_htmlEditor, scrollPos);
            _htmlEditor.Select(safeStart, safeLength);
        }

        private void ApplyRegexColor(Regex pattern, string text, int offset, Color color)
        {
            foreach (Match match in pattern.Matches(text))
            {
                if (match.Success && match.Length > 0)
                    ColorizeRange(offset + match.Index, match.Length, color);
            }
        }

        private void ApplyHtmlTagColoring(string text, int offset)
        {
            int i = 0;
            while (i < text.Length)
            {
                if (text[i] != '<')
                {
                    i++;
                    continue;
                }

                if (StartsWith(text, i, "<!--"))
                {
                    int commentEnd = text.IndexOf("-->", i + 4, StringComparison.Ordinal);
                    if (commentEnd < 0)
                        break;
                    i = commentEnd + 3;
                    continue;
                }

                int tagEnd = FindTagEnd(text, i + 1);
                if (tagEnd < 0)
                    break;

                ColorizeRange(offset + i, 1, GetEditorTagBracketSyntaxColor());
                ColorizeRange(offset + tagEnd, 1, GetEditorTagBracketSyntaxColor());

                int cursor = i + 1;
                if (cursor < tagEnd && text[cursor] == '/')
                {
                    ColorizeRange(offset + cursor, 1, GetEditorTagBracketSyntaxColor());
                    cursor++;
                }

                while (cursor < tagEnd && char.IsWhiteSpace(text[cursor]))
                    cursor++;

                if (cursor < tagEnd && (text[cursor] == '!' || text[cursor] == '?'))
                {
                    i = tagEnd + 1;
                    continue;
                }

                int tagNameStart = cursor;
                while (cursor < tagEnd && IsTagNameChar(text[cursor]))
                    cursor++;
                if (cursor > tagNameStart)
                    ColorizeRange(offset + tagNameStart, cursor - tagNameStart, GetEditorTagNameSyntaxColor());

                while (cursor < tagEnd)
                {
                    while (cursor < tagEnd && char.IsWhiteSpace(text[cursor]))
                        cursor++;
                    if (cursor >= tagEnd)
                        break;

                    if (text[cursor] == '/')
                    {
                        ColorizeRange(offset + cursor, 1, GetEditorTagBracketSyntaxColor());
                        cursor++;
                        continue;
                    }

                    int attrNameStart = cursor;
                    while (cursor < tagEnd && IsAttributeNameChar(text[cursor]))
                        cursor++;

                    if (cursor == attrNameStart)
                    {
                        cursor++;
                        continue;
                    }

                    ColorizeRange(offset + attrNameStart, cursor - attrNameStart, GetEditorAttributeNameSyntaxColor());

                    while (cursor < tagEnd && char.IsWhiteSpace(text[cursor]))
                        cursor++;

                    if (cursor < tagEnd && text[cursor] == '=')
                    {
                        ColorizeRange(offset + cursor, 1, GetEditorTagBracketSyntaxColor());
                        cursor++;
                        while (cursor < tagEnd && char.IsWhiteSpace(text[cursor]))
                            cursor++;

                        if (cursor >= tagEnd)
                            break;

                        int valueStart = cursor;
                        if (text[cursor] == '"' || text[cursor] == '\'')
                        {
                            char quote = text[cursor];
                            cursor++;
                            while (cursor < tagEnd)
                            {
                                if (text[cursor] == '\\' && cursor + 1 < tagEnd)
                                {
                                    cursor += 2;
                                    continue;
                                }

                                if (text[cursor] == quote)
                                {
                                    cursor++;
                                    break;
                                }
                                cursor++;
                            }
                        }
                        else
                        {
                            while (cursor < tagEnd && !char.IsWhiteSpace(text[cursor]) && text[cursor] != '>')
                                cursor++;
                        }

                        if (cursor > valueStart)
                            ColorizeRange(offset + valueStart, cursor - valueStart, GetEditorAttributeValueSyntaxColor());
                    }
                }

                i = tagEnd + 1;
            }
        }

        private void ApplyScriptBlockJavaScriptColoring(string fullText, int colorStart, int colorLength, bool viewportOnlyMode)
        {
            if (string.IsNullOrEmpty(fullText) || colorLength <= 0)
                return;

            int colorEnd = Math.Min(fullText.Length, colorStart + colorLength);
            int scriptScanPadding = viewportOnlyMode ? 1536 : 4096;
            int scanStart = Math.Max(0, colorStart - scriptScanPadding);
            int scanEnd = Math.Min(fullText.Length, colorEnd + scriptScanPadding);
            if (scanEnd <= scanStart)
                return;

            bool anyApplied = false;
            string scanText = fullText.Substring(scanStart, scanEnd - scanStart);
            foreach (Match match in HtmlScriptBlockPattern.Matches(scanText))
            {
                if (!match.Success)
                    continue;

                Group codeGroup = match.Groups["code"];
                if (!codeGroup.Success || codeGroup.Length <= 0)
                    continue;

                int scriptStart = scanStart + codeGroup.Index;
                int scriptEnd = scriptStart + codeGroup.Length;
                int highlightStart = Math.Max(scriptStart, colorStart);
                int highlightEnd = Math.Min(scriptEnd, colorEnd);
                if (highlightEnd <= highlightStart)
                    continue;

                string scriptText = fullText.Substring(highlightStart, highlightEnd - highlightStart);
                ApplyJavaScriptSyntaxColoring(scriptText, highlightStart);
                anyApplied = true;
            }

            if (anyApplied)
                return;

            // Local fallback for very large script blocks where the closing tag is outside the viewport scan.
            int localBacktrack = viewportOnlyMode ? 16384 : 65536;
            int localForward = viewportOnlyMode ? 4096 : 32768;
            int localStart = Math.Max(0, colorStart - localBacktrack);
            int localEnd = Math.Min(fullText.Length, colorEnd + localForward);
            if (localEnd <= localStart)
                return;

            string localText = fullText.Substring(localStart, localEnd - localStart);
            int relProbe = Math.Clamp(colorStart - localStart, 0, Math.Max(0, localText.Length - 1));

            int relOpenBefore = localText.LastIndexOf("<script", relProbe, StringComparison.OrdinalIgnoreCase);
            int relCloseBefore = localText.LastIndexOf("</script", relProbe, StringComparison.OrdinalIgnoreCase);

            int relOpenCandidate = relOpenBefore > relCloseBefore
                ? relOpenBefore
                : localText.IndexOf("<script", relProbe, StringComparison.OrdinalIgnoreCase);

            if (relOpenCandidate < 0)
                return;

            int relOpenTagEnd = localText.IndexOf('>', relOpenCandidate);
            if (relOpenTagEnd < 0)
                return;

            int relScriptStart = relOpenTagEnd + 1;
            int relScriptClose = localText.IndexOf("</script", relScriptStart, StringComparison.OrdinalIgnoreCase);

            int absScriptStart = localStart + relScriptStart;
            int absScriptEnd = relScriptClose >= 0 ? localStart + relScriptClose : colorEnd;

            int fallbackHighlightStart = Math.Max(absScriptStart, colorStart);
            int fallbackHighlightEnd = Math.Min(absScriptEnd, colorEnd);
            if (fallbackHighlightEnd <= fallbackHighlightStart)
                return;

            string fallbackScriptText = fullText.Substring(fallbackHighlightStart, fallbackHighlightEnd - fallbackHighlightStart);
            ApplyJavaScriptSyntaxColoring(fallbackScriptText, fallbackHighlightStart);
        }

        private void ApplyJavaScriptSyntaxColoring(string text, int offset)
        {
            if (string.IsNullOrEmpty(text))
                return;

            ApplyRegexColor(JavaScriptKeywordPattern, text, offset, GetEditorJavaScriptKeywordSyntaxColor());
            ApplyRegexColor(JavaScriptLiteralPattern, text, offset, GetEditorJavaScriptLiteralSyntaxColor());
            ApplyRegexColor(JavaScriptNumberPattern, text, offset, GetEditorJavaScriptNumberSyntaxColor());
            ApplyRegexColor(JavaScriptSingleQuotedStringPattern, text, offset, GetEditorJavaScriptStringSyntaxColor());
            ApplyRegexColor(JavaScriptDoubleQuotedStringPattern, text, offset, GetEditorJavaScriptStringSyntaxColor());
            ApplyRegexColor(JavaScriptTemplateStringPattern, text, offset, GetEditorJavaScriptStringSyntaxColor());
            ApplyRegexColor(JavaScriptLineCommentPattern, text, offset, GetEditorCommentSyntaxColor());
            ApplyRegexColor(JavaScriptBlockCommentPattern, text, offset, GetEditorCommentSyntaxColor());
        }

        private static int FindTagEnd(string text, int startIndex)
        {
            bool inSingleQuote = false;
            bool inDoubleQuote = false;

            for (int i = startIndex; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    continue;
                }

                if (c == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    continue;
                }

                if (c == '>' && !inSingleQuote && !inDoubleQuote)
                    return i;
            }

            return -1;
        }

        private static bool StartsWith(string text, int index, string value)
        {
            if (index < 0 || index + value.Length > text.Length)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (text[index + i] != value[i])
                    return false;
            }
            return true;
        }

        private static bool IsTagNameChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == ':' || c == '-' || c == '_';
        }

        private static bool IsAttributeNameChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == ':' || c == '-' || c == '_' || c == '.';
        }

        private void ColorizeRange(int start, int length, Color color)
        {
            if (_htmlEditor == null || start < 0 || length <= 0 || start >= _htmlEditor.TextLength)
                return;

            int safeLength = Math.Min(length, _htmlEditor.TextLength - start);
            if (safeLength <= 0)
                return;

            _htmlEditor.Select(start, safeLength);
            _htmlEditor.SelectionColor = color;
        }

        private static void SuspendControlPainting(Control control)
        {
            if (control == null || !control.IsHandleCreated)
                return;

            SendMessage(control.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        private static void ResumeControlPainting(Control control)
        {
            if (control == null || !control.IsHandleCreated)
                return;

            SendMessage(control.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            control.Invalidate();
            control.Update();
        }

        private static Point GetRichTextScrollPosition(RichTextBox editor)
        {
            if (editor == null || !editor.IsHandleCreated)
                return Point.Empty;

            IntPtr mem = Marshal.AllocHGlobal(Marshal.SizeOf<NativePoint>());
            try
            {
                Marshal.StructureToPtr(new NativePoint(), mem, false);
                SendMessage(editor.Handle, EM_GETSCROLLPOS, IntPtr.Zero, mem);
                var p = Marshal.PtrToStructure<NativePoint>(mem);
                return new Point(p.X, p.Y);
            }
            finally
            {
                Marshal.FreeHGlobal(mem);
            }
        }

        private static void SetRichTextScrollPosition(RichTextBox editor, Point position)
        {
            if (editor == null || !editor.IsHandleCreated)
                return;

            IntPtr mem = Marshal.AllocHGlobal(Marshal.SizeOf<NativePoint>());
            try
            {
                Marshal.StructureToPtr(new NativePoint { X = position.X, Y = position.Y }, mem, false);
                SendMessage(editor.Handle, EM_SETSCROLLPOS, IntPtr.Zero, mem);
            }
            finally
            {
                Marshal.FreeHGlobal(mem);
            }
        }

        private async Task RenderEditorToPreviewAsync(bool syncVisualEditor = true)
        {
            if (_htmlEditor == null || webView2Control.CoreWebView2 == null)
                return;

            string html = _htmlEditor.Text;
            if (string.IsNullOrWhiteSpace(html))
            {
                html = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"></head><body></body></html>";
            }

            string htmlForPreview = EnsureBaseTag(html);
            _syncEditorFromNavigation = false;
            _isRenderingEditorPreview = true;
            webView2Control.CoreWebView2.NavigateToString(htmlForPreview);

            if (syncVisualEditor && _isVisualEditorPanelOpen && !_isSyncingSourceFromVisualEditor)
                await RenderHtmlToVisualEditorAsync(htmlForPreview);
        }

        private async Task RenderHtmlToVisualEditorAsync(string htmlForPreview)
        {
            _pendingVisualEditorHtml = htmlForPreview ?? string.Empty;
            if (!_isVisualEditorPanelOpen || _visualEditorWebView?.CoreWebView2 == null)
                return;

            _isRenderingVisualEditor = true;
            _ignoreNextVisualEditorMessage = true;
            _visualEditorWebView.CoreWebView2.NavigateToString(_pendingVisualEditorHtml);
            await Task.CompletedTask;
        }

        private static string GetVisualEditorBridgeScript()
        {
            return @"(() => {
    const buildHtml = () => {
        const dt = document.doctype;
        let docType = '';
        if (dt) {
            docType = '<!DOCTYPE ' + dt.name;
            if (dt.publicId) {
                docType += ' PUBLIC ' + dt.publicId;
            } else if (dt.systemId) {
                docType += ' SYSTEM';
            }
            if (dt.systemId) {
                docType += ' ' + dt.systemId;
            }
            docType += '>';
        }
        return docType + '\n' + document.documentElement.outerHTML;
    };

    const notifyHost = () => {
        clearTimeout(window.__browserAiVisualTimer);
        window.__browserAiVisualTimer = setTimeout(() => {
            window.chrome.webview.postMessage(buildHtml());
        }, 280);
    };

    document.designMode = 'on';
    if (document.body) {
        document.body.setAttribute('contenteditable', 'true');
        document.body.spellcheck = false;
    }

    if (!window.__browserAiVisualHooked) {
        ['input', 'keyup', 'paste', 'cut', 'drop', 'blur'].forEach((eventName) => {
            document.addEventListener(eventName, notifyHost, true);
        });
        window.__browserAiVisualHooked = true;
    }

    notifyHost();
})();";
        }

        private async void VisualEditorWebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess || _visualEditorWebView?.CoreWebView2 == null)
                return;

            _visualEditorWebView.CoreWebView2.WebMessageReceived += VisualEditorWebView_WebMessageReceived;

            if (_isVisualEditorPanelOpen)
            {
                string initialHtml = string.IsNullOrWhiteSpace(_pendingVisualEditorHtml)
                    ? "<!DOCTYPE html><html><head><meta charset='utf-8'></head><body><p>Visual editor ready.</p></body></html>"
                    : _pendingVisualEditorHtml;

                await RenderHtmlToVisualEditorAsync(initialHtml);
            }
        }

        private async void VisualEditorWebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (_visualEditorWebView?.CoreWebView2 == null)
                return;

            if (!_isVisualEditorPanelOpen)
            {
                _isRenderingVisualEditor = false;
                _ignoreNextVisualEditorMessage = false;
                return;
            }

            if (!e.IsSuccess)
            {
                _isRenderingVisualEditor = false;
                _ignoreNextVisualEditorMessage = false;
                return;
            }

            try
            {
                await _visualEditorWebView.CoreWebView2.ExecuteScriptAsync(GetVisualEditorBridgeScript());
            }
            catch
            {
                _ignoreNextVisualEditorMessage = false;
                // Ignore visual editor script failures and keep HTML source editing available.
            }
            finally
            {
                _isRenderingVisualEditor = false;
            }
        }

        private async void VisualEditorWebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (!_isVisualEditorPanelOpen || _isRenderingVisualEditor || _isSyncingSourceFromVisualEditor || _htmlEditor == null)
                return;

            if (_ignoreNextVisualEditorMessage)
            {
                _ignoreNextVisualEditorMessage = false;
                return;
            }

            string html;
            try
            {
                html = JsonSerializer.Deserialize<string>(e.WebMessageAsJson) ?? string.Empty;
            }
            catch
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(html))
                return;

            _isSyncingSourceFromVisualEditor = true;
            try
            {
                SetEditorText(html, formatHtml: false, resetCaret: false, applySyntaxHighlight: false);
                if (_editorSyntaxTimer != null)
                {
                    QueueEditorSyntaxHighlight(viewportOnly: false);
                }

                await RenderEditorToPreviewAsync(syncVisualEditor: false);
            }
            finally
            {
                _isSyncingSourceFromVisualEditor = false;
            }
        }

        private string EnsureBaseTag(string html)
        {
            if (_editorBaseUri == null || string.IsNullOrWhiteSpace(html))
                return html;

            try
            {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var htmlNode = doc.DocumentNode.SelectSingleNode("//html");
                if (htmlNode == null)
                    return html;

                var head = htmlNode.SelectSingleNode("./head");
                if (head == null)
                {
                    head = doc.CreateElement("head");
                    htmlNode.PrependChild(head);
                }

                var baseNode = head.SelectSingleNode("./base[@href]") ?? doc.CreateElement("base");
                baseNode.SetAttributeValue("href", _editorBaseUri.AbsoluteUri);

                if (baseNode.ParentNode == null)
                    head.PrependChild(baseNode);

                return doc.DocumentNode.OuterHtml;
            }
            catch
            {
                return html;
            }
        }

        private void HandleResize()
        {
            if (_useChromeLayout)
            {
                LayoutChromeContentBounds();
                return;
            }

            // Resize the webview
            webView2Control.Size = this.ClientSize - new System.Drawing.Size(webView2Control.Location);

            // Move the Events button
            btnEvents.Left = this.ClientSize.Width - btnEvents.Width;
            // Move the Go button
            btnGo.Left = this.btnEvents.Left - btnGo.Size.Width;

            // Resize the URL textbox
            txtUrl.Width = btnGo.Left - txtUrl.Left;
        }

        private void LayoutChromeContentBounds()
        {
            if (!_useChromeLayout || _contentPanel == null || _chromePanel == null || menuStrip1 == null)
                return;

            int top = menuStrip1.Height + _chromePanel.Height;
            int height = Math.Max(0, ClientSize.Height - top);
            _contentPanel.Location = new Point(0, top);
            _contentPanel.Size = new Size(ClientSize.Width, height);
        }

        private Control GetWebViewHost()
        {
            if (_useChromeLayout && _previewHostPanel != null)
                return _previewHostPanel;

            return _useChromeLayout && _contentPanel != null ? _contentPanel : this;
        }

        private void RemoveWebViewFromHost()
        {
            var host = GetWebViewHost();
            if (webView2Control != null && host.Controls.Contains(webView2Control))
                host.Controls.Remove(webView2Control);
        }

        private void AddWebViewToHost()
        {
            var host = GetWebViewHost();
            if (webView2Control != null && !host.Controls.Contains(webView2Control))
                host.Controls.Add(webView2Control);
        }

        private void PrepareWebViewControl(WebView2 control)
        {
            if (control == null)
                return;

            if (_useChromeLayout)
            {
                control.Dock = DockStyle.Fill;
                control.Margin = new Padding(0);
                control.DefaultBackgroundColor = _currentTheme == UiTheme.Dark
                    ? Color.FromArgb(12, 14, 18)
                    : Color.FromArgb(247, 250, 255);
            }
            else
            {
                control.DefaultBackgroundColor = Color.Transparent;
            }
        }

        private void ApplyChromeLayout()
        {
            if (_useChromeLayout)
                return;

            _useChromeLayout = true;
            SuspendLayout();

            DoubleBuffered = true;
            BackColor = Color.FromArgb(15, 17, 21);
            ForeColor = Color.FromArgb(230, 233, 240);
            Font = new Font("Bahnschrift", 10F, FontStyle.Regular);

            menuStrip1.Dock = DockStyle.Top;
            menuStrip1.BackColor = Color.FromArgb(18, 21, 28);
            menuStrip1.ForeColor = Color.FromArgb(223, 228, 236);
            menuStrip1.Font = new Font("Bahnschrift", 10F, FontStyle.Regular);
            menuStrip1.Padding = new Padding(10, 3, 0, 3);
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new DarkMenuColorTable());
            ApplyMenuItemStyle(menuStrip1.Items);

            _chromePanel = new GradientPanel
            {
                Dock = DockStyle.Top,
                Height = 92,
                Padding = new Padding(12, 18, 12, 18),
                StartColor = Color.FromArgb(27, 32, 43),
                EndColor = Color.FromArgb(20, 24, 33),
                Angle = 90,
                BorderColor = Color.FromArgb(46, 54, 68),
                BorderThickness = 1
            };

            _chromeLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            _chromeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _chromeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _chromeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            _chromeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _navPanel = new CenteredFlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Left
            };

            _actionPanel = new CenteredFlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Right
            };

            _addressShell = new GradientPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(18, 22, 32),
                StartColor = Color.FromArgb(18, 22, 32),
                EndColor = Color.FromArgb(18, 22, 32),
                Angle = 0,
                BorderColor = Color.FromArgb(44, 52, 66),
                BorderThickness = 1,
                Padding = new Padding(14, 10, 14, 10),
                Margin = new Padding(12, 0, 12, 0)
            };

            _contentPanel = new Panel
            {
                Dock = DockStyle.None,
                Padding = new Padding(12, 18, 12, 12),
                BackColor = Color.FromArgb(13, 15, 19)
            };

            _editorSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 6,
                BackColor = Color.FromArgb(20, 24, 33)
            };
            _editorSplit.Panel1.Padding = new Padding(0, 2, 6, 0);
            _editorSplit.Panel2.Padding = new Padding(0, 2, 0, 0);
            _editorSplit.Resize += (_, __) => UpdateEditorSplitLayout();

            _designPreviewSplit = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 6,
                BackColor = Color.FromArgb(20, 24, 33)
            };
            _designPreviewSplit.Panel1.Padding = new Padding(6, 0, 3, 0);
            _designPreviewSplit.Panel2.Padding = new Padding(3, 0, 0, 0);
            _designPreviewSplit.Resize += (_, __) => UpdateEditorSplitLayout();

            _htmlEditor = new RichTextBox
            {
                Dock = DockStyle.Fill,
                AcceptsTab = true,
                WordWrap = false,
                DetectUrls = false,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 11F, FontStyle.Regular),
                BackColor = Color.FromArgb(14, 17, 24),
                ForeColor = EditorBaseColor,
                ScrollBars = RichTextBoxScrollBars.Both
            };
            _htmlEditor.TextChanged += htmlEditor_TextChanged;
            _htmlEditor.VScroll += htmlEditor_VScroll;
            _htmlEditor.HScroll += htmlEditor_HScroll;

            _previewHostPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.FromArgb(12, 14, 18)
            };

            _visualEditorHostPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = Color.FromArgb(12, 14, 18)
            };

            _visualEditorWebView = new WebView2
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                AllowExternalDrop = false,
                CreationProperties = this.CreationProperties,
                DefaultBackgroundColor = Color.FromArgb(12, 14, 18)
            };
            _visualEditorWebView.CoreWebView2InitializationCompleted += VisualEditorWebView_CoreWebView2InitializationCompleted;
            _visualEditorWebView.NavigationCompleted += VisualEditorWebView_NavigationCompleted;

            _openHtmlButton = new Button
            {
                Text = "Open HTML"
            };
            _openHtmlButton.Click += openHtmlButton_Click;

            _saveHtmlButton = new Button
            {
                Text = "Save HTML"
            };
            _saveHtmlButton.Click += saveHtmlButton_Click;

            _darkThemeButton = new Button
            {
                Text = "Dark"
            };
            _darkThemeButton.Click += darkThemeButton_Click;

            _lightThemeButton = new Button
            {
                Text = "Light"
            };
            _lightThemeButton.Click += lightThemeButton_Click;

            _formatHtmlButton = new Button
            {
                Text = "Format"
            };
            _formatHtmlButton.Click += formatHtmlButton_Click;

            _toggleVisualEditorButton = new Button
            {
                Text = "WYSIWYG"
            };
            _toggleVisualEditorButton.Click += toggleVisualEditorButton_Click;

            _editorPreviewTimer = new System.Windows.Forms.Timer
            {
                Interval = 350
            };
            _editorPreviewTimer.Tick += editorPreviewTimer_Tick;

            _editorSyntaxTimer = new System.Windows.Forms.Timer
            {
                Interval = 420
            };
            _editorSyntaxTimer.Tick += editorSyntaxTimer_Tick;

            ApplyCommandButtonStyles();

            btnBack.Margin = new Padding(0, 0, 8, 0);
            btnForward.Margin = new Padding(0, 0, 8, 0);
            btnRefresh.Margin = new Padding(0, 0, 8, 0);
            btnStop.Margin = new Padding(0, 0, 8, 0);
            btnGo.Margin = new Padding(0);
            _darkThemeButton.Margin = new Padding(0, 0, 8, 0);
            _lightThemeButton.Margin = new Padding(0, 0, 8, 0);
            _openHtmlButton.Margin = new Padding(0, 0, 8, 0);
            _saveHtmlButton.Margin = new Padding(0, 0, 8, 0);
            _formatHtmlButton.Margin = new Padding(0, 0, 8, 0);
            _toggleVisualEditorButton.Margin = new Padding(0, 0, 8, 0);
            linksBtn.Margin = new Padding(0, 0, 8, 0);
            ScrapeBtn.Margin = new Padding(0, 0, 8, 0);

            SetButtonWidth(btnBack, 92);
            SetButtonWidth(btnForward, 104);
            SetButtonWidth(btnRefresh, 96);
            SetButtonWidth(btnStop, 96);
            SetButtonWidth(_darkThemeButton, 88);
            SetButtonWidth(_lightThemeButton, 92);
            SetButtonWidth(_openHtmlButton, 126);
            SetButtonWidth(_saveHtmlButton, 126);
            SetButtonWidth(_formatHtmlButton, 108);
            SetButtonWidth(_toggleVisualEditorButton, 132);
            SetButtonWidth(linksBtn, 100);
            SetButtonWidth(ScrapeBtn, 112);

            txtUrl.BorderStyle = BorderStyle.None;
            txtUrl.AutoSize = false;
            txtUrl.BackColor = _addressShell.BackColor;
            txtUrl.ForeColor = Color.FromArgb(233, 237, 244);
            txtUrl.Font = new Font("Bahnschrift", 14F, FontStyle.Regular);
            txtUrl.MinimumSize = new Size(160, 38);
            txtUrl.Height = 38;
            txtUrl.Margin = new Padding(0);
            txtUrl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtUrl.KeyDown += txtUrl_KeyDown;

            btnGo.Dock = DockStyle.Right;
            btnGo.AutoSize = false;
            btnGo.Width = 92;
            btnGo.Height = 40;
            btnGo.TextAlign = ContentAlignment.MiddleCenter;
            btnGo.Text = "Go";
            btnGo.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            linksBtn.Text = "Links";
            ScrapeBtn.Text = "Scrape";

            _navPanel.Controls.Add(btnBack);
            _navPanel.Controls.Add(btnForward);
            _navPanel.Controls.Add(btnRefresh);
            _navPanel.Controls.Add(btnStop);

            _actionPanel.Controls.Add(_darkThemeButton);
            _actionPanel.Controls.Add(_lightThemeButton);
            _actionPanel.Controls.Add(_openHtmlButton);
            _actionPanel.Controls.Add(_saveHtmlButton);
            _actionPanel.Controls.Add(_formatHtmlButton);
            _actionPanel.Controls.Add(_toggleVisualEditorButton);
            _actionPanel.Controls.Add(linksBtn);
            _actionPanel.Controls.Add(ScrapeBtn);
            _actionPanel.Controls.Add(btnEvents);

            _addressShell.Controls.Add(txtUrl);
            _addressShell.Controls.Add(btnGo);
            _addressShell.Resize += (_, __) => LayoutAddressBar();

            _chromeLayout.Controls.Add(_navPanel, 0, 0);
            _chromeLayout.Controls.Add(_addressShell, 1, 0);
            _chromeLayout.Controls.Add(_actionPanel, 2, 0);
            _chromePanel.Controls.Add(_chromeLayout);

            PrepareWebViewControl(webView2Control);

            _editorSplit.Panel1.Controls.Add(_htmlEditor);
            _editorSplit.Panel2.Controls.Add(_designPreviewSplit);
            _designPreviewSplit.Panel1.Controls.Add(_visualEditorHostPanel);
            _designPreviewSplit.Panel2.Controls.Add(_previewHostPanel);
            _visualEditorHostPanel.Controls.Add(_visualEditorWebView);
            _previewHostPanel.Controls.Add(webView2Control);
            _visualEditorWebView.Source = new Uri("about:blank");
            _designPreviewSplit.Panel1Collapsed = true;
            _isVisualEditorPanelOpen = false;
            _contentPanel.Controls.Add(_editorSplit);

            Controls.Add(_contentPanel);
            Controls.Add(_chromePanel);

            Controls.SetChildIndex(menuStrip1, 0);
            Controls.SetChildIndex(_chromePanel, 1);
            Controls.SetChildIndex(_contentPanel, 2);

            ResumeLayout(true);
            LayoutAddressBar();
            LayoutChromeContentBounds();
            UpdateEditorSplitLayout();
            SetEditorText("<!DOCTYPE html>\n<html>\n<head>\n  <meta charset=\"utf-8\" />\n  <title>New Document</title>\n</head>\n<body>\n  <h1>Edit HTML on the left</h1>\n  <p>Click WYSIWYG to open the middle visual editor. Preview stays on the right.</p>\n</body>\n</html>", formatHtml: true);
            ApplyThemeStyling();
        }

        private void SetUiTheme(UiTheme theme)
        {
            if (_currentTheme == theme)
                return;

            _currentTheme = theme;
            ApplyThemeStyling();
        }

        private void ApplyThemeStyling()
        {
            bool isDark = _currentTheme == UiTheme.Dark;

            BackColor = isDark ? Color.FromArgb(15, 17, 21) : Color.FromArgb(244, 247, 252);
            ForeColor = isDark ? Color.FromArgb(230, 233, 240) : Color.FromArgb(28, 34, 48);

            menuStrip1.BackColor = isDark ? Color.FromArgb(18, 21, 28) : Color.FromArgb(232, 238, 248);
            menuStrip1.ForeColor = isDark ? Color.FromArgb(223, 228, 236) : Color.FromArgb(35, 45, 63);
            ProfessionalColorTable menuColorTable = isDark ? new DarkMenuColorTable() : new LightMenuColorTable();
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(menuColorTable);
            ApplyMenuItemStyle(menuStrip1.Items);

            if (_chromePanel != null)
            {
                _chromePanel.StartColor = isDark ? Color.FromArgb(27, 32, 43) : Color.FromArgb(250, 252, 255);
                _chromePanel.EndColor = isDark ? Color.FromArgb(20, 24, 33) : Color.FromArgb(236, 243, 251);
                _chromePanel.BorderColor = isDark ? Color.FromArgb(46, 54, 68) : Color.FromArgb(183, 195, 215);
                _chromePanel.Invalidate();
            }

            if (_addressShell is GradientPanel addressPanel)
            {
                Color addressColor = isDark ? Color.FromArgb(18, 22, 32) : Color.FromArgb(255, 255, 255);
                addressPanel.BackColor = addressColor;
                addressPanel.StartColor = addressColor;
                addressPanel.EndColor = addressColor;
                addressPanel.BorderColor = isDark ? Color.FromArgb(44, 52, 66) : Color.FromArgb(182, 194, 214);
                addressPanel.Invalidate();
            }

            if (_contentPanel != null)
                _contentPanel.BackColor = isDark ? Color.FromArgb(13, 15, 19) : Color.FromArgb(239, 244, 251);

            if (_editorSplit != null)
                _editorSplit.BackColor = isDark ? Color.FromArgb(20, 24, 33) : Color.FromArgb(214, 225, 242);

            if (_designPreviewSplit != null)
                _designPreviewSplit.BackColor = isDark ? Color.FromArgb(18, 22, 32) : Color.FromArgb(214, 225, 242);

            if (_visualEditorHostPanel != null)
                _visualEditorHostPanel.BackColor = isDark ? Color.FromArgb(12, 14, 18) : Color.FromArgb(247, 250, 255);

            if (_previewHostPanel != null)
                _previewHostPanel.BackColor = isDark ? Color.FromArgb(12, 14, 18) : Color.FromArgb(247, 250, 255);

            if (_htmlEditor != null)
            {
                _htmlEditor.BackColor = isDark ? Color.FromArgb(14, 17, 24) : Color.FromArgb(255, 255, 255);
                _htmlEditor.ForeColor = GetEditorBaseSyntaxColor();
            }

            txtUrl.BackColor = isDark ? Color.FromArgb(18, 22, 32) : Color.FromArgb(255, 255, 255);
            txtUrl.ForeColor = isDark ? Color.FromArgb(233, 237, 244) : Color.FromArgb(32, 44, 63);

            if (webView2Control != null)
                webView2Control.DefaultBackgroundColor = isDark ? Color.FromArgb(12, 14, 18) : Color.FromArgb(247, 250, 255);

            if (_visualEditorWebView != null)
                _visualEditorWebView.DefaultBackgroundColor = isDark ? Color.FromArgb(12, 14, 18) : Color.FromArgb(247, 250, 255);

            ApplyCommandButtonStyles();
            ApplyHtmlSyntaxHighlight();
        }

        private void ApplyCommandButtonStyles()
        {
            ApplyButtonStyle(btnBack);
            ApplyButtonStyle(btnForward);
            ApplyButtonStyle(btnRefresh);
            ApplyButtonStyle(btnStop);
            ApplyButtonStyle(btnGo, Color.FromArgb(59, 130, 246));
            ApplyButtonStyle(_openHtmlButton, Color.FromArgb(251, 146, 60));
            ApplyButtonStyle(_saveHtmlButton, Color.FromArgb(34, 197, 94));
            ApplyButtonStyle(_formatHtmlButton, Color.FromArgb(14, 165, 233));
            Color? visualAccent = _isVisualEditorPanelOpen ? Color.FromArgb(245, 158, 11) : null;
            ApplyButtonStyle(_toggleVisualEditorButton, visualAccent);
            ApplyButtonStyle(linksBtn, Color.FromArgb(99, 102, 241));
            ApplyButtonStyle(ScrapeBtn, Color.FromArgb(16, 185, 129));
            ApplyButtonStyle(btnEvents);

            if (_darkThemeButton != null)
            {
                Color? darkAccent = _currentTheme == UiTheme.Dark ? Color.FromArgb(59, 130, 246) : null;
                ApplyButtonStyle(_darkThemeButton, darkAccent);
                _darkThemeButton.Text = _currentTheme == UiTheme.Dark ? "Dark *" : "Dark";
            }

            if (_lightThemeButton != null)
            {
                Color? lightAccent = _currentTheme == UiTheme.Light ? Color.FromArgb(245, 158, 11) : null;
                ApplyButtonStyle(_lightThemeButton, lightAccent);
                _lightThemeButton.Text = _currentTheme == UiTheme.Light ? "Light *" : "Light";
            }

            if (_toggleVisualEditorButton != null)
                _toggleVisualEditorButton.Text = _isVisualEditorPanelOpen ? "WYSIWYG *" : "WYSIWYG";
        }

        private Color GetEditorBaseSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorBaseColor : EditorBaseColorLight;
        }

        private Color GetEditorCommentSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorCommentColor : EditorCommentColorLight;
        }

        private Color GetEditorTagBracketSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorTagBracketColor : EditorTagBracketColorLight;
        }

        private Color GetEditorTagNameSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorTagNameColor : EditorTagNameColorLight;
        }

        private Color GetEditorAttributeNameSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorAttributeNameColor : EditorAttributeNameColorLight;
        }

        private Color GetEditorAttributeValueSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorAttributeValueColor : EditorAttributeValueColorLight;
        }

        private Color GetEditorEntitySyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorEntityColor : EditorEntityColorLight;
        }

        private Color GetEditorJavaScriptKeywordSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorJavaScriptKeywordColor : EditorJavaScriptKeywordColorLight;
        }

        private Color GetEditorJavaScriptStringSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorJavaScriptStringColor : EditorJavaScriptStringColorLight;
        }

        private Color GetEditorJavaScriptNumberSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorJavaScriptNumberColor : EditorJavaScriptNumberColorLight;
        }

        private Color GetEditorJavaScriptLiteralSyntaxColor()
        {
            return _currentTheme == UiTheme.Dark ? EditorJavaScriptLiteralColor : EditorJavaScriptLiteralColorLight;
        }

        private static Color GetReadableTextColor(Color backgroundColor)
        {
            int luminance = (backgroundColor.R * 299 + backgroundColor.G * 587 + backgroundColor.B * 114) / 1000;
            return luminance >= 150 ? Color.FromArgb(24, 30, 41) : Color.FromArgb(244, 247, 252);
        }

        private void ApplyButtonStyle(Button button, Color? accentColor = null)
        {
            if (button == null)
                return;

            bool isDark = _currentTheme == UiTheme.Dark;
            var baseColor = accentColor ?? (isDark ? Color.FromArgb(28, 34, 48) : Color.FromArgb(221, 230, 243));
            var hoverColor = isDark ? ControlPaint.Light(baseColor, 0.15f) : ControlPaint.Dark(baseColor, 0.05f);
            var downColor = isDark ? ControlPaint.Dark(baseColor, 0.1f) : ControlPaint.Dark(baseColor, 0.12f);

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = isDark ? 0 : 1;
            button.FlatAppearance.BorderColor = isDark ? baseColor : ControlPaint.Dark(baseColor, 0.16f);
            button.FlatAppearance.MouseOverBackColor = hoverColor;
            button.FlatAppearance.MouseDownBackColor = downColor;
            button.BackColor = baseColor;
            button.ForeColor = GetReadableTextColor(baseColor);
            button.Font = new Font("Bahnschrift", 12F, FontStyle.Bold);
            button.AutoSize = false;
            button.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button.Padding = new Padding(12, 4, 12, 4);
            button.Height = 40;
            button.MinimumSize = new Size(88, 40);
            button.UseVisualStyleBackColor = false;
        }

        private void SetButtonWidth(Button button, int width)
        {
            if (button == null)
                return;
            button.Width = width;
        }

        private void LayoutAddressBar()
        {
            if (!_useChromeLayout || _addressShell == null)
                return;

            int goWidth = btnGo.Width;
            int goHeight = btnGo.Height;
            int gap = 8;

            var padding = _addressShell.Padding;
            var inner = new Rectangle(
                padding.Left,
                padding.Top,
                Math.Max(0, _addressShell.ClientSize.Width - padding.Left - padding.Right),
                Math.Max(0, _addressShell.ClientSize.Height - padding.Top - padding.Bottom)
            );
            int centerY = inner.Top + (inner.Height - goHeight) / 2;

            btnGo.Location = new Point(inner.Right - goWidth, centerY);

            int textHeight = txtUrl.Height;
            int textY = inner.Top + (inner.Height - textHeight) / 2;
            txtUrl.Location = new Point(inner.Left, textY);
            txtUrl.Width = Math.Max(120, btnGo.Left - gap - txtUrl.Left);
        }

        private void UpdateEditorSplitLayout()
        {
            if (_editorSplit == null || _editorSplit.IsDisposed)
                return;

            int width = _editorSplit.ClientSize.Width;
            if (width <= 0)
                return;

            int available = Math.Max(0, width - _editorSplit.SplitterWidth);
            int desiredDistance = (int)(available * 0.34f);
            int minPane = Math.Min(260, available / 2);
            if (minPane < 80)
                minPane = available / 2;

            int minDistance = minPane;
            int maxDistance = Math.Max(minDistance, available - minPane);
            int clampedDistance = Math.Clamp(desiredDistance, minDistance, maxDistance);

            _editorSplit.Panel1MinSize = 0;
            _editorSplit.Panel2MinSize = 0;
            _editorSplit.SplitterDistance = clampedDistance;
            _editorSplit.Panel1MinSize = minPane;
            _editorSplit.Panel2MinSize = minPane;

            UpdateDesignPreviewSplitLayout();
        }

        private void UpdateDesignPreviewSplitLayout()
        {
            if (_designPreviewSplit == null || _designPreviewSplit.IsDisposed)
                return;

            int width = _designPreviewSplit.ClientSize.Width;
            if (width <= 0)
                return;

            int available = Math.Max(0, width - _designPreviewSplit.SplitterWidth);
            int minPane = Math.Min(220, available / 2);
            if (minPane < 80)
                minPane = available / 2;

            int desiredDistance = available / 2;
            int minDistance = minPane;
            int maxDistance = Math.Max(minDistance, available - minPane);
            int clampedDistance = Math.Clamp(desiredDistance, minDistance, maxDistance);

            _designPreviewSplit.Panel1MinSize = 0;
            _designPreviewSplit.Panel2MinSize = 0;
            _designPreviewSplit.SplitterDistance = clampedDistance;
            _designPreviewSplit.Panel1MinSize = minPane;
            _designPreviewSplit.Panel2MinSize = minPane;
        }

        private void ApplyMenuItemStyle(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                item.ForeColor = menuStrip1.ForeColor;
                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                    ApplyMenuItemStyle(menuItem.DropDownItems);
            }
        }

        private sealed class DarkMenuColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => Color.FromArgb(17, 20, 26);
            public override Color ImageMarginGradientBegin => Color.FromArgb(17, 20, 26);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(17, 20, 26);
            public override Color ImageMarginGradientEnd => Color.FromArgb(17, 20, 26);
            public override Color MenuBorder => Color.FromArgb(46, 54, 68);
            public override Color MenuItemBorder => Color.FromArgb(58, 66, 82);
            public override Color MenuItemSelected => Color.FromArgb(40, 46, 60);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(40, 46, 60);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(40, 46, 60);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(33, 38, 50);
            public override Color MenuItemPressedGradientMiddle => Color.FromArgb(33, 38, 50);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(33, 38, 50);
            public override Color SeparatorDark => Color.FromArgb(34, 39, 50);
            public override Color SeparatorLight => Color.FromArgb(34, 39, 50);
            public override Color ToolStripBorder => Color.FromArgb(34, 39, 50);
            public override Color ToolStripGradientBegin => Color.FromArgb(18, 21, 28);
            public override Color ToolStripGradientMiddle => Color.FromArgb(18, 21, 28);
            public override Color ToolStripGradientEnd => Color.FromArgb(18, 21, 28);
        }

        private sealed class LightMenuColorTable : ProfessionalColorTable
        {
            public override Color ToolStripDropDownBackground => Color.FromArgb(246, 249, 255);
            public override Color ImageMarginGradientBegin => Color.FromArgb(246, 249, 255);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(246, 249, 255);
            public override Color ImageMarginGradientEnd => Color.FromArgb(246, 249, 255);
            public override Color MenuBorder => Color.FromArgb(170, 184, 207);
            public override Color MenuItemBorder => Color.FromArgb(156, 173, 199);
            public override Color MenuItemSelected => Color.FromArgb(220, 232, 250);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(220, 232, 250);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(220, 232, 250);
            public override Color MenuItemPressedGradientBegin => Color.FromArgb(209, 223, 244);
            public override Color MenuItemPressedGradientMiddle => Color.FromArgb(209, 223, 244);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(209, 223, 244);
            public override Color SeparatorDark => Color.FromArgb(190, 204, 226);
            public override Color SeparatorLight => Color.FromArgb(190, 204, 226);
            public override Color ToolStripBorder => Color.FromArgb(190, 204, 226);
            public override Color ToolStripGradientBegin => Color.FromArgb(232, 238, 248);
            public override Color ToolStripGradientMiddle => Color.FromArgb(232, 238, 248);
            public override Color ToolStripGradientEnd => Color.FromArgb(232, 238, 248);
        }

        private sealed class GradientPanel : Panel
        {
            public Color StartColor { get; set; } = Color.FromArgb(27, 32, 43);
            public Color EndColor { get; set; } = Color.FromArgb(20, 24, 33);
            public float Angle { get; set; } = 90F;
            public Color BorderColor { get; set; } = Color.FromArgb(46, 54, 68);
            public int BorderThickness { get; set; } = 1;

            public GradientPanel()
            {
                DoubleBuffered = true;
                ResizeRedraw = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                using var brush = new LinearGradientBrush(ClientRectangle, StartColor, EndColor, Angle);
                e.Graphics.FillRectangle(brush, ClientRectangle);

                if (BorderThickness > 0)
                {
                    using var pen = new Pen(BorderColor, BorderThickness);
                    var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private sealed class CenteredFlowLayoutPanel : FlowLayoutPanel
        {
            public CenteredFlowLayoutPanel()
            {
                DoubleBuffered = true;
            }

            protected override void OnLayout(LayoutEventArgs levent)
            {
                base.OnLayout(levent);

                int available = ClientSize.Height;
                if (available <= 0)
                    return;

                foreach (Control control in Controls)
                {
                    if (!control.Visible)
                        continue;
                    int centeredTop = Math.Max(0, (available - control.Height) / 2);
                    control.Top = centeredTop;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct NativePoint
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private string GetSdkBuildVersion()
        {
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions();

            // The full version string A.B.C.D
            var targetVersionMajorAndRest = options.TargetCompatibleBrowserVersion;
            var versionList = targetVersionMajorAndRest.Split('.');
            if (versionList.Length != 4)
            {
                return "Invalid SDK build version";
            }
            // Keep C.D
            return versionList[2] + "." + versionList[3];
        }

        private string GetRuntimeVersion(CoreWebView2 webView2)
        {
            return webView2.Environment.BrowserVersionString;
        }

        private string GetAppPath()
        {
            return System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        private string GetRuntimePath(CoreWebView2 webView2)
        {
            int processId = (int)webView2.BrowserProcessId;
            try
            {
                Process process = System.Diagnostics.Process.GetProcessById(processId);
                var fileName = process.MainModule.FileName;
                return System.IO.Path.GetDirectoryName(fileName);
            }
            catch (ArgumentException e)
            {
                return e.Message;
            }
            catch (InvalidOperationException e)
            {
                return e.Message;
            }
            // Occurred when a 32-bit process wants to access the modules of a 64-bit process.
            catch (Win32Exception e)
            {
                return e.Message;
            }
        }

        private string GetStartPageUri(CoreWebView2 webView2)
        {
            string uri = "http://google.com";
            if (webView2 == null)
            {
                return uri;
            }
            string sdkBuildVersion = GetSdkBuildVersion(),
                   runtimeVersion = GetRuntimeVersion(webView2),
                   appPath = GetAppPath(),
                   runtimePath = GetRuntimePath(webView2);
            string newUri = $"{uri}";
            return newUri;
        }
        private Task WaitForNavigationAsync()
        {
            var core = webView2Control.CoreWebView2;
            if (core == null)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<bool>();

            void Handler(object? sender, CoreWebView2NavigationCompletedEventArgs e)
            {
                core.NavigationCompleted -= Handler;
                tcs.TrySetResult(true);
            }

            core.NavigationCompleted += Handler;
            return tcs.Task;
        }

        private async Task<string> GetPageHtmlAsync()
        {
            var core = webView2Control.CoreWebView2;
            if (core == null)
                return string.Empty;

            string js = "document.documentElement.outerHTML";
            string raw = await core.ExecuteScriptAsync(js);
            return JsonSerializer.Deserialize<string>(raw) ?? string.Empty;
        }

        private (List<string> links, string? nextUrl) ExtractLinksAndNext(string html, string currentUrl)
        {
            var baseUri = new Uri(currentUrl);

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var links = new List<string>();

            var aNodes = doc.DocumentNode.SelectNodes("//article//a[@href]");
            if (aNodes != null)
            {
                foreach (var a in aNodes)
                {
                    var href = a.GetAttributeValue("href", null);
                    if (string.IsNullOrWhiteSpace(href))
                        continue;

                    if (!href.Contains("/oferta/"))
                        continue;

                    href = WebUtility.HtmlDecode(href);

                    var absolute = href.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                        ? href
                        : new Uri(baseUri, href).ToString();   // << use baseUri here

                    links.Add(absolute);
                }
            }

            // NEXT page
            var nextNode = doc.DocumentNode.SelectSingleNode(
                "//a[@rel='next' and not(contains(@class,'disabled'))]"
            );

            string? nextUrl = null;
            if (nextNode != null)
            {
                var href = nextNode.GetAttributeValue("href", null);
                if (!string.IsNullOrWhiteSpace(href))
                {
                    href = WebUtility.HtmlDecode(href);
                    nextUrl = href.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                        ? href
                        : new Uri(baseUri, href).ToString();   // same pattern
                }
            }

            return (links.Distinct().ToList(), nextUrl);
        }


        private async Task<int> SaveProductLinksAsync(IEnumerable<string> urls)
        {
            using var db = new BrowserDbContext();
            int added = 0;

            foreach (var url in urls.Distinct())
            {
                bool exists = await db.ProductLinks.AnyAsync(p => p.Url == url);
                if (exists) continue;

                db.ProductLinks.Add(new ProductLink
                {
                    Url = url,
                    Status = "pending",
                    LastError = null
                });
                added++;
            }

            await db.SaveChangesAsync();
            return added;
        }
        private async Task CollectCategoryLinksAsync()
        {
            if (webView2Control.CoreWebView2 == null)
            {
                MessageBox.Show("WebView is not initialized yet. Wait for the page to load.");
                return;
            }

            // start from current URL (category page)
            string startUrl = webView2Control.Source?.AbsoluteUri ?? txtUrl.Text;
            if (string.IsNullOrWhiteSpace(startUrl))
            {
                MessageBox.Show("No current URL.");
                return;
            }

            string currentUrl = startUrl;
            int totalAdded = 0;
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                while (!string.IsNullOrEmpty(currentUrl))
                {
                    webView2Control.CoreWebView2.Navigate(currentUrl);
                    await WaitForNavigationAsync();

                    string html = await GetPageHtmlAsync();

                    var (links, nextUrl) = ExtractLinksAndNext(html, currentUrl);

                    int added = await SaveProductLinksAsync(links);
                    totalAdded += added;

                    currentUrl = nextUrl;
                }

                MessageBox.Show($"Finished. Added {totalAdded} new product links.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while collecting links:\n" + ex.Message);
            }
            finally
            {
                this.Cursor = oldCursor;
            }
        }

        async private void linksBtn_Click(object sender, EventArgs e)
        {
            await CollectCategoryLinksAsync();
        }

        async private void ScrapeBtn_Click(object sender, EventArgs e)
        {
            await ScrapePendingProductsAsync();
        }
        private class ProductScrapeResult
        {
            public string Title { get; set; }
            public string PriceText { get; set; }
            public string Description { get; set; }
            public string[] ImageUrls { get; set; }
            public Dictionary<string, string> Specifications { get; set; }
            public string Url { get; set; }
        }

        private async Task<bool> ScrapeOneProductAsync(ProductLink link)
        {
            if (webView2Control.CoreWebView2 == null)
                throw new InvalidOperationException("WebView2 not initialized.");

            // 1. Navigate to product page
            webView2Control.CoreWebView2.Navigate(link.Url);
            await WaitForNavigationAsync();

            const string expandParamsScript = @"
    (function() {
        var candidates = Array.from(document.querySelectorAll('button, a, span'))
            .filter(function(el) {
                var t = (el.innerText || '').toLowerCase();
                return t.includes('pełne parametry') ||
                       t.includes('wszystkie parametry') ||
                       t.includes('więcej parametrów') ||
                       t.includes('pokaż więcej') &&
                       t.includes('parametr');
            });

        candidates.forEach(function(el) {
            try { el.click(); } catch (e) {}
        });
    })();
";

            // click the buttons
            await webView2Control.CoreWebView2.ExecuteScriptAsync(expandParamsScript);

            // give the page a bit of time to load extra rows / update DOM
            await Task.Delay(1000);


            // 2. JavaScript to extract data from Allegro offer page
            const string script = @"
(function() {
    function textOrNull(el) {
        return el ? el.innerText.trim() : null;
    }
    function text(el) {
        return el ? el.innerText.trim() : '';
    }

    // ----- title & price -----
    var titleEl = document.querySelector('h1');
    var priceEl = document.querySelector('[data-role=""price""]') ||
                  document.querySelector('meta[itemprop=""price""]');

    var descEl = document.querySelector('[data-box-name=""Description""]') ||
                 document.querySelector('#description');

    // ----- images -----
    var imgEls = Array.from(document.querySelectorAll('img'));
    var imgSrcs = imgEls
        .map(function(img) { return img.src; })
        .filter(function(src) { return src && src.startsWith('http'); });
    var uniqueImgs = Array.from(new Set(imgSrcs));

    // ----- parameters (page + modal) -----
    var specs = {};

    function collectFromRoot(root) {
        if (!root) return;
        var rows = root.querySelectorAll('tr');
        rows.forEach(function(row) {
            var cells = row.querySelectorAll('th,td');
            if (cells.length !== 2) return;

            var name = text(cells[0]);
            var val  = text(cells[1]);

            if (!name || !val) return;
            if (name.length > 120 || val.length > 800) return;

            specs[name] = val;
        });
    }

    var roots = [];

    // main parameters box
    var mainBox = document.querySelector('[data-box-name=""Parameters""]');
    if (mainBox) roots.push(mainBox);

    // any dialog/modal that looks like the big parameters panel
    document.querySelectorAll('div[role=""dialog""]').forEach(function(d) {
        var header = d.querySelector('h1,h2,h3');
        if (header && /parametr/i.test(header.innerText)) {
            roots.push(d);
        }
    });

    // sections with aria-label mentioning parameters
    document.querySelectorAll('section[aria-label]').forEach(function(s) {
        var label = s.getAttribute('aria-label') || '';
        if (/parametr/i.test(label)) {
            roots.push(s);
        }
    });

    // fallback: some Allegro layouts use data-testid
    if (roots.length === 0) {
        var panel = document.querySelector('[data-testid=""parameters-section""]');
        if (panel) roots.push(panel);
    }

    // ultimate fallback: whole document (won't hurt)
    if (roots.length === 0) {
        roots.push(document.body);
    }

    roots.forEach(collectFromRoot);

    return {
        Title:        textOrNull(titleEl),
        PriceText:    priceEl ? (priceEl.content || priceEl.innerText.trim()) : null,
        Description:  textOrNull(descEl),
        ImageUrls:    uniqueImgs,
        Specifications: specs,
        Url:          location.href
    };
})();
";


            string json = await webView2Control.CoreWebView2.ExecuteScriptAsync(script);

            var result = JsonSerializer.Deserialize<ProductScrapeResult>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (result == null)
                throw new Exception("Could not deserialize product data.");

            // 3. Save to DB
            using var db = new BrowserDbContext();

            // attach the link so we can update its status
            var dbLink = await db.ProductLinks.FirstAsync(p => p.Id == link.Id);

            var product = await db.Products
                .FirstOrDefaultAsync(p => p.Url == link.Url);

            if (product == null)
            {
                product = new ProductInfo
                {
                    Url = link.Url
                };
                db.Products.Add(product);
            }

            product.Title = result.Title ?? product.Title;
            product.Description = result.Description ?? product.Description;

            // put price into Specifications as well
            var specs = result.Specifications ?? new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(result.PriceText))
                specs["Cena"] = result.PriceText;

            product.Specifications = specs;
            product.ImageUrls = result.ImageUrls?.ToList() ?? new List<string>();
            product.ScrapedAt = DateTime.UtcNow;

            dbLink.Status = "scraped";
            dbLink.LastError = null;

            await db.SaveChangesAsync();
            return true;
        }
        private static readonly TimeSpan MinDelay = TimeSpan.FromSeconds(20);
        private static readonly TimeSpan Jitter = TimeSpan.FromSeconds(20);
        private async Task ScrapePendingProductsAsync(int maxCount = 0)
        {
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            int scraped = 0;
            int failed = 0;
            var rnd = new Random();

            try
            {
                while (true)
                {
                    using var db = new BrowserDbContext();

                    var next = await db.ProductLinks
                        .Where(p => p.Status == "pending")
                        .OrderBy(p => p.Id)
                        .FirstOrDefaultAsync();

                    if (next == null)
                        break; // nothing left

                    // detach before passing to another context
                    var linkCopy = new ProductLink
                    {
                        Id = next.Id,
                        Url = next.Url,
                        Status = next.Status,
                        LastError = next.LastError
                    };

                    try
                    {
                        await ScrapeOneProductAsync(linkCopy);
                        scraped++;
                    }
                    catch (Exception ex)
                    {
                        using var db2 = new BrowserDbContext();
                        var dbLink = await db2.ProductLinks.FirstAsync(p => p.Id == linkCopy.Id);
                        dbLink.Status = "failed";
                        dbLink.LastError = ex.Message;
                        await db2.SaveChangesAsync();
                        failed++;
                    }

                    if (maxCount > 0 && scraped + failed >= maxCount)
                        break;

                    var jitterSeconds = rnd.NextDouble() * Jitter.TotalSeconds;
                    var delay = MinDelay + TimeSpan.FromSeconds(jitterSeconds);
                    await Task.Delay(delay);
                }

                MessageBox.Show($"Scraping finished.\nScraped: {scraped}\nFailed: {failed}");
            }
            finally
            {
                this.Cursor = oldCursor;
            }
        }

        private static readonly HttpClient _httpClient = new HttpClient();

        // choose some key for folder name: Allegro numeric id if present, else link.Id
        private static string GetProductKey(ProductLink link)
        {
            var m = Regex.Match(link.Url, @"(\d+)(?:\?|$)");
            if (m.Success) return m.Groups[1].Value;
            return link.Id.ToString();
        }
        private static string GetProductKey(ProductInfo product)
        {
            var url = product.Url ?? "";
            var m = Regex.Match(url, @"(\d+)(?:\?|$)");
            if (m.Success) return m.Groups[1].Value;
            return product.Id.ToString();
        }
        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        private async Task<List<string>> DownloadImagesAsync(IEnumerable<string> urls, string productKey)
        {
            var localPaths = new List<string>();
            if (urls == null) return localPaths;

            var baseDir = Path.Combine(AppContext.BaseDirectory, "images");
            Directory.CreateDirectory(baseDir);

            var productDirName = SanitizeFileName(productKey);
            var productDir = Path.Combine(baseDir, productDirName);
            Directory.CreateDirectory(productDir);

            int index = 0;
            foreach (var url in urls.Distinct())
            {
                if (string.IsNullOrWhiteSpace(url))
                    continue;

                try
                {
                    var uri = new Uri(url);
                    var ext = Path.GetExtension(uri.AbsolutePath);
                    if (string.IsNullOrEmpty(ext) || ext.Length > 5)
                        ext = ".jpg";

                    var fileName = $"img_{index++}{ext}";
                    var filePath = Path.Combine(productDir, fileName);

                    if (!File.Exists(filePath))
                    {
                        using var resp = await _httpClient.GetAsync(uri);
                        resp.EnsureSuccessStatusCode();

                        await using var fs = File.Create(filePath);
                        await resp.Content.CopyToAsync(fs);
                    }

                    localPaths.Add(filePath);
                }
                catch
                {
                    // ignore single-image failures, continue with others
                }
            }

            return localPaths;
        }

        private async Task DownloadImagesForScrapedProductsAsync()
        {
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            int productsProcessed = 0;

            try
            {
                using var db = new BrowserDbContext();

                // Only products that have been scraped but have no local images yet
                var products = await db.Products
                    .Where(p => p.ScrapedAt != null &&
                                (p.ImagePathsJson == null || p.ImagePathsJson == ""))
                    .OrderBy(p => p.Id)
                    .ToListAsync();

                foreach (var product in products)
                {
                    var urls = product.ImageUrls; // comes from Specs scraper
                    if (urls == null || urls.Count == 0)
                        continue;

                    var key = GetProductKey(product);
                    var localPaths = await DownloadImagesAsync(urls, key);
                    product.ImagePaths = localPaths;
                    productsProcessed++;
                }

                await db.SaveChangesAsync();

                MessageBox.Show($"Downloaded images for {productsProcessed} products.");
            }
            finally
            {
                this.Cursor = oldCursor;
            }
        }


    }
}
