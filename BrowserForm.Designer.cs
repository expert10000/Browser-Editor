// Copyright (C) Microsoft Corporation. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using System;
using Microsoft.Web.WebView2.Core;

namespace WebView2WindowsFormsBrowser
{
    partial class BrowserForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnEvents = new System.Windows.Forms.Button();
            btnBack = new System.Windows.Forms.Button();
            btnForward = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnStop = new System.Windows.Forms.Button();
            btnGo = new System.Windows.Forms.Button();
            txtUrl = new System.Windows.Forms.TextBox();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printToPDFMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            portraitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            landscapeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            getDocumentTitleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            getUserDataFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showBrowserProcessInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            crashBrowserProcessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            crashRendererProcessMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showPerformanceInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            closeWebViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createWebViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createNewWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createNewWindowWithOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            createNewThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            controlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            acceleratorKeysEnabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            allowExternalDropMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            serverCertificateErrorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleCustomServerCertificateSupportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearServerCertificateErrorActionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            setUsersAgentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleDefaultScriptDialogsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleVisibilityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            xToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            xToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            backgroundColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            whiteBackgroundColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            redBackgroundColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            blueBackgroundColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            transparentBackgroundColorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            scriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            injectScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            injectScriptIntoFrameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            methodCDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            taskManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            postMessageStringMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            postMessageJsonMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            postMessageStringIframeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            postMessageJsonIframeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addInitializeScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeInitializeScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            scenarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AuthenticationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearBrowsingData = new System.Windows.Forms.ToolStripMenuItem();
            ClearAllDOMStorageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearAllProfileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearAllSiteMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearAutofillMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearBrowsingHistoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearCookiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearDiskCacheMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ClearDownloadHistoryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clientCertificateRequestedStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CustomClientCertificateSelectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeferredCustomCertificateDialogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cookieManagementStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            GetCookiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            AddOrUpdateCookieMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteCookiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            DeleteAllCookiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addRemoteObjectMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            domContentLoadedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            navigateWithWebResourceRequestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            webMessageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toggleMuteStateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            blockedDomainsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            webView2Control = new Microsoft.Web.WebView2.WinForms.WebView2();
            linksBtn = new System.Windows.Forms.Button();
            ScrapeBtn = new System.Windows.Forms.Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView2Control).BeginInit();
            SuspendLayout();
            // 
            // btnEvents
            // 
            btnEvents.Location = new System.Drawing.Point(356, 38);
            btnEvents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnEvents.Name = "btnEvents";
            btnEvents.Size = new System.Drawing.Size(100, 35);
            btnEvents.TabIndex = 10;
            btnEvents.Text = "Events";
            btnEvents.UseVisualStyleBackColor = true;
            btnEvents.Visible = false;
            btnEvents.Click += btnEvents_Click;
            // 
            // btnBack
            // 
            btnBack.Enabled = false;
            btnBack.Location = new System.Drawing.Point(16, 38);
            btnBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(100, 35);
            btnBack.TabIndex = 0;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnForward
            // 
            btnForward.Enabled = false;
            btnForward.Location = new System.Drawing.Point(124, 38);
            btnForward.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnForward.Name = "btnForward";
            btnForward.Size = new System.Drawing.Size(100, 35);
            btnForward.TabIndex = 1;
            btnForward.Text = "Forward";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += btnForward_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new System.Drawing.Point(232, 38);
            btnRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(100, 35);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "Reload";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new System.Drawing.Point(356, 38);
            btnStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnStop.Name = "btnStop";
            btnStop.Size = new System.Drawing.Size(100, 35);
            btnStop.TabIndex = 3;
            btnStop.Text = "Cancel";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // btnGo
            // 
            btnGo.Location = new System.Drawing.Point(1252, 75);
            btnGo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            btnGo.Name = "btnGo";
            btnGo.Size = new System.Drawing.Size(100, 35);
            btnGo.TabIndex = 5;
            btnGo.Text = "Go";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += BtnGo_Click;
            // 
            // txtUrl
            // 
            txtUrl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtUrl.BackColor = System.Drawing.SystemColors.Info;
            txtUrl.Font = new System.Drawing.Font("Segoe UI", 13F);
            txtUrl.ForeColor = System.Drawing.SystemColors.Highlight;
            txtUrl.Location = new System.Drawing.Point(16, 79);
            txtUrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new System.Drawing.Size(1222, 36);
            txtUrl.TabIndex = 4;
            txtUrl.Text = "http://google.com";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, processToolStripMenuItem, windowToolStripMenuItem, controlToolStripMenuItem, viewToolStripMenuItem, scriptToolStripMenuItem, scenarioToolStripMenuItem, audioToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1408, 28);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { printToPDFMenuItem, getDocumentTitleMenuItem, getUserDataFolderMenuItem, exitMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // printToPDFMenuItem
            // 
            printToPDFMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { portraitMenuItem, landscapeMenuItem });
            printToPDFMenuItem.Name = "printToPDFMenuItem";
            printToPDFMenuItem.Size = new System.Drawing.Size(230, 26);
            printToPDFMenuItem.Text = "Print to PDF";
            // 
            // portraitMenuItem
            // 
            portraitMenuItem.Name = "portraitMenuItem";
            portraitMenuItem.Size = new System.Drawing.Size(162, 26);
            portraitMenuItem.Text = "Portrait";
            portraitMenuItem.Click += portraitMenuItem_Click;
            // 
            // landscapeMenuItem
            // 
            landscapeMenuItem.Name = "landscapeMenuItem";
            landscapeMenuItem.Size = new System.Drawing.Size(162, 26);
            landscapeMenuItem.Text = "Landscape";
            landscapeMenuItem.Click += landscapeMenuItem_Click;
            // 
            // getDocumentTitleMenuItem
            // 
            getDocumentTitleMenuItem.Name = "getDocumentTitleMenuItem";
            getDocumentTitleMenuItem.Size = new System.Drawing.Size(230, 26);
            getDocumentTitleMenuItem.Text = "Get Document Title";
            getDocumentTitleMenuItem.Click += getDocumentTitleMenuItem_Click;
            // 
            // getUserDataFolderMenuItem
            // 
            getUserDataFolderMenuItem.Name = "getUserDataFolderMenuItem";
            getUserDataFolderMenuItem.Size = new System.Drawing.Size(230, 26);
            getUserDataFolderMenuItem.Text = "Get User Data Folder";
            getUserDataFolderMenuItem.Click += getUserDataFolderMenuItem_Click;
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new System.Drawing.Size(230, 26);
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += exitMenuItem_Click;
            // 
            // processToolStripMenuItem
            // 
            processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showBrowserProcessInfoMenuItem, crashBrowserProcessMenuItem, crashRendererProcessMenuItem, showPerformanceInfoMenuItem });
            processToolStripMenuItem.Name = "processToolStripMenuItem";
            processToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            processToolStripMenuItem.Text = "Process";
            // 
            // showBrowserProcessInfoMenuItem
            // 
            showBrowserProcessInfoMenuItem.Name = "showBrowserProcessInfoMenuItem";
            showBrowserProcessInfoMenuItem.Size = new System.Drawing.Size(268, 26);
            showBrowserProcessInfoMenuItem.Text = "Show Browser Process Info";
            showBrowserProcessInfoMenuItem.Click += showBrowserProcessInfoMenuItem_Click;
            // 
            // crashBrowserProcessMenuItem
            // 
            crashBrowserProcessMenuItem.Name = "crashBrowserProcessMenuItem";
            crashBrowserProcessMenuItem.Size = new System.Drawing.Size(268, 26);
            crashBrowserProcessMenuItem.Text = "Crash Browser Process";
            crashBrowserProcessMenuItem.Click += crashBrowserProcessMenuItem_Click;
            // 
            // crashRendererProcessMenuItem
            // 
            crashRendererProcessMenuItem.Name = "crashRendererProcessMenuItem";
            crashRendererProcessMenuItem.Size = new System.Drawing.Size(268, 26);
            crashRendererProcessMenuItem.Text = "Crash Renderer Process";
            crashRendererProcessMenuItem.Click += crashRendererProcessMenuItem_Click;
            // 
            // showPerformanceInfoMenuItem
            // 
            showPerformanceInfoMenuItem.Name = "showPerformanceInfoMenuItem";
            showPerformanceInfoMenuItem.Size = new System.Drawing.Size(268, 26);
            showPerformanceInfoMenuItem.Text = "Show Performance Info";
            showPerformanceInfoMenuItem.Click += showPerformanceInfoMenuItem_Click;
            // 
            // windowToolStripMenuItem
            // 
            windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { closeWebViewToolStripMenuItem, createWebViewToolStripMenuItem, createNewWindowToolStripMenuItem, createNewWindowWithOptionsToolStripMenuItem, createNewThreadToolStripMenuItem });
            windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            windowToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            windowToolStripMenuItem.Text = "Window";
            // 
            // closeWebViewToolStripMenuItem
            // 
            closeWebViewToolStripMenuItem.Name = "closeWebViewToolStripMenuItem";
            closeWebViewToolStripMenuItem.Size = new System.Drawing.Size(319, 26);
            closeWebViewToolStripMenuItem.Text = "Close WebView";
            closeWebViewToolStripMenuItem.Click += closeWebViewToolStripMenuItem_Click;
            // 
            // createWebViewToolStripMenuItem
            // 
            createWebViewToolStripMenuItem.Name = "createWebViewToolStripMenuItem";
            createWebViewToolStripMenuItem.Size = new System.Drawing.Size(319, 26);
            createWebViewToolStripMenuItem.Text = "Create WebView";
            createWebViewToolStripMenuItem.Click += createWebViewToolStripMenuItem_Click;
            // 
            // createNewWindowToolStripMenuItem
            // 
            createNewWindowToolStripMenuItem.Name = "createNewWindowToolStripMenuItem";
            createNewWindowToolStripMenuItem.Size = new System.Drawing.Size(319, 26);
            createNewWindowToolStripMenuItem.Text = "Create New Window";
            createNewWindowToolStripMenuItem.Click += createNewWindowToolStripMenuItem_Click;
            // 
            // createNewWindowWithOptionsToolStripMenuItem
            // 
            createNewWindowWithOptionsToolStripMenuItem.Name = "createNewWindowWithOptionsToolStripMenuItem";
            createNewWindowWithOptionsToolStripMenuItem.Size = new System.Drawing.Size(319, 26);
            createNewWindowWithOptionsToolStripMenuItem.Text = "Create New Window With Options";
            createNewWindowWithOptionsToolStripMenuItem.Click += createNewWindowWithOptionsToolStripMenuItem_Click;
            // 
            // createNewThreadToolStripMenuItem
            // 
            createNewThreadToolStripMenuItem.Name = "createNewThreadToolStripMenuItem";
            createNewThreadToolStripMenuItem.Size = new System.Drawing.Size(319, 26);
            createNewThreadToolStripMenuItem.Text = "Create New Thread";
            createNewThreadToolStripMenuItem.Click += createNewThreadToolStripMenuItem_Click;
            // 
            // controlToolStripMenuItem
            // 
            controlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { acceleratorKeysEnabledToolStripMenuItem, allowExternalDropMenuItem, serverCertificateErrorMenuItem, setUsersAgentMenuItem, toggleDefaultScriptDialogsMenuItem });
            controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            controlToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            controlToolStripMenuItem.Text = "Settings";
            // 
            // acceleratorKeysEnabledToolStripMenuItem
            // 
            acceleratorKeysEnabledToolStripMenuItem.Checked = true;
            acceleratorKeysEnabledToolStripMenuItem.CheckOnClick = true;
            acceleratorKeysEnabledToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            acceleratorKeysEnabledToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            acceleratorKeysEnabledToolStripMenuItem.Name = "acceleratorKeysEnabledToolStripMenuItem";
            acceleratorKeysEnabledToolStripMenuItem.Size = new System.Drawing.Size(288, 26);
            acceleratorKeysEnabledToolStripMenuItem.Text = "Toggle AcceleratorKeys";
            // 
            // allowExternalDropMenuItem
            // 
            allowExternalDropMenuItem.Checked = true;
            allowExternalDropMenuItem.CheckOnClick = true;
            allowExternalDropMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            allowExternalDropMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            allowExternalDropMenuItem.Name = "allowExternalDropMenuItem";
            allowExternalDropMenuItem.Size = new System.Drawing.Size(288, 26);
            allowExternalDropMenuItem.Text = "Toggle AllowExternalDrop";
            allowExternalDropMenuItem.Click += allowExternalDropMenuItem_Click;
            // 
            // serverCertificateErrorMenuItem
            // 
            serverCertificateErrorMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleCustomServerCertificateSupportMenuItem, clearServerCertificateErrorActionsMenuItem });
            serverCertificateErrorMenuItem.Name = "serverCertificateErrorMenuItem";
            serverCertificateErrorMenuItem.Size = new System.Drawing.Size(288, 26);
            serverCertificateErrorMenuItem.Text = "Server Certificate Error";
            // 
            // toggleCustomServerCertificateSupportMenuItem
            // 
            toggleCustomServerCertificateSupportMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toggleCustomServerCertificateSupportMenuItem.Name = "toggleCustomServerCertificateSupportMenuItem";
            toggleCustomServerCertificateSupportMenuItem.Size = new System.Drawing.Size(366, 26);
            toggleCustomServerCertificateSupportMenuItem.Text = "Toggle Custom Server Certificate Support";
            toggleCustomServerCertificateSupportMenuItem.Click += toggleCustomServerCertificateSupportMenuItem_Click;
            // 
            // clearServerCertificateErrorActionsMenuItem
            // 
            clearServerCertificateErrorActionsMenuItem.Name = "clearServerCertificateErrorActionsMenuItem";
            clearServerCertificateErrorActionsMenuItem.Size = new System.Drawing.Size(366, 26);
            clearServerCertificateErrorActionsMenuItem.Text = "Clear Server Certificate Error Actions";
            clearServerCertificateErrorActionsMenuItem.Click += clearServerCertificateErrorActionsMenuItem_Click;
            // 
            // setUsersAgentMenuItem
            // 
            setUsersAgentMenuItem.Name = "setUsersAgentMenuItem";
            setUsersAgentMenuItem.Size = new System.Drawing.Size(288, 26);
            setUsersAgentMenuItem.Text = "Set Users Agent";
            setUsersAgentMenuItem.Click += setUsersAgentMenuItem_Click;
            // 
            // toggleDefaultScriptDialogsMenuItem
            // 
            toggleDefaultScriptDialogsMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toggleDefaultScriptDialogsMenuItem.Name = "toggleDefaultScriptDialogsMenuItem";
            toggleDefaultScriptDialogsMenuItem.Size = new System.Drawing.Size(288, 26);
            toggleDefaultScriptDialogsMenuItem.Text = "Toggle Default Script Dialogs";
            toggleDefaultScriptDialogsMenuItem.Click += toggleDefaultScriptDialogsMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleVisibilityMenuItem, zoomToolStripMenuItem, backgroundColorMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            viewToolStripMenuItem.Text = "View";
            // 
            // toggleVisibilityMenuItem
            // 
            toggleVisibilityMenuItem.Checked = true;
            toggleVisibilityMenuItem.CheckOnClick = true;
            toggleVisibilityMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            toggleVisibilityMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toggleVisibilityMenuItem.Name = "toggleVisibilityMenuItem";
            toggleVisibilityMenuItem.Size = new System.Drawing.Size(211, 26);
            toggleVisibilityMenuItem.Text = "Toggle Visibility";
            toggleVisibilityMenuItem.Click += toggleVisibilityMenuItem_Click;
            // 
            // zoomToolStripMenuItem
            // 
            zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { xToolStripMenuItem, xToolStripMenuItem1, xToolStripMenuItem2, xToolStripMenuItem3 });
            zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
            zoomToolStripMenuItem.Size = new System.Drawing.Size(211, 26);
            zoomToolStripMenuItem.Text = "Zoom";
            // 
            // xToolStripMenuItem
            // 
            xToolStripMenuItem.Name = "xToolStripMenuItem";
            xToolStripMenuItem.Size = new System.Drawing.Size(199, 26);
            xToolStripMenuItem.Text = "0.5x";
            xToolStripMenuItem.Click += xToolStripMenuItem05_Click;
            // 
            // xToolStripMenuItem1
            // 
            xToolStripMenuItem1.Name = "xToolStripMenuItem1";
            xToolStripMenuItem1.Size = new System.Drawing.Size(199, 26);
            xToolStripMenuItem1.Text = "1.0x";
            xToolStripMenuItem1.Click += xToolStripMenuItem1_Click;
            // 
            // xToolStripMenuItem2
            // 
            xToolStripMenuItem2.Name = "xToolStripMenuItem2";
            xToolStripMenuItem2.Size = new System.Drawing.Size(199, 26);
            xToolStripMenuItem2.Text = "2.0x";
            xToolStripMenuItem2.Click += xToolStripMenuItem2_Click;
            // 
            // xToolStripMenuItem3
            // 
            xToolStripMenuItem3.Name = "xToolStripMenuItem3";
            xToolStripMenuItem3.Size = new System.Drawing.Size(199, 26);
            xToolStripMenuItem3.Text = "Get ZoomFactor";
            xToolStripMenuItem3.Click += xToolStripMenuItem3_Click;
            // 
            // backgroundColorMenuItem
            // 
            backgroundColorMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { whiteBackgroundColorMenuItem, redBackgroundColorMenuItem, blueBackgroundColorMenuItem, transparentBackgroundColorMenuItem });
            backgroundColorMenuItem.Name = "backgroundColorMenuItem";
            backgroundColorMenuItem.Size = new System.Drawing.Size(211, 26);
            backgroundColorMenuItem.Text = "Background Color";
            // 
            // whiteBackgroundColorMenuItem
            // 
            whiteBackgroundColorMenuItem.Name = "whiteBackgroundColorMenuItem";
            whiteBackgroundColorMenuItem.Size = new System.Drawing.Size(169, 26);
            whiteBackgroundColorMenuItem.Text = "White";
            whiteBackgroundColorMenuItem.Click += backgroundColorMenuItem_Click;
            // 
            // redBackgroundColorMenuItem
            // 
            redBackgroundColorMenuItem.Name = "redBackgroundColorMenuItem";
            redBackgroundColorMenuItem.Size = new System.Drawing.Size(169, 26);
            redBackgroundColorMenuItem.Text = "Red";
            redBackgroundColorMenuItem.Click += backgroundColorMenuItem_Click;
            // 
            // blueBackgroundColorMenuItem
            // 
            blueBackgroundColorMenuItem.Name = "blueBackgroundColorMenuItem";
            blueBackgroundColorMenuItem.Size = new System.Drawing.Size(169, 26);
            blueBackgroundColorMenuItem.Text = "Blue";
            blueBackgroundColorMenuItem.Click += backgroundColorMenuItem_Click;
            // 
            // transparentBackgroundColorMenuItem
            // 
            transparentBackgroundColorMenuItem.Name = "transparentBackgroundColorMenuItem";
            transparentBackgroundColorMenuItem.Size = new System.Drawing.Size(169, 26);
            transparentBackgroundColorMenuItem.Text = "Transparent";
            transparentBackgroundColorMenuItem.Click += backgroundColorMenuItem_Click;
            // 
            // scriptToolStripMenuItem
            // 
            scriptToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { injectScriptMenuItem, injectScriptIntoFrameMenuItem, methodCDPToolStripMenuItem, taskManagerToolStripMenuItem, postMessageStringMenuItem, postMessageJsonMenuItem, postMessageStringIframeMenuItem, postMessageJsonIframeMenuItem, addInitializeScriptMenuItem, removeInitializeScriptMenuItem });
            scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            scriptToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            scriptToolStripMenuItem.Text = "Script";
            // 
            // injectScriptMenuItem
            // 
            injectScriptMenuItem.Name = "injectScriptMenuItem";
            injectScriptMenuItem.Size = new System.Drawing.Size(271, 26);
            injectScriptMenuItem.Text = "Inject Script";
            injectScriptMenuItem.Click += injectScriptMenuItem_Click;
            // 
            // injectScriptIntoFrameMenuItem
            // 
            injectScriptIntoFrameMenuItem.Name = "injectScriptIntoFrameMenuItem";
            injectScriptIntoFrameMenuItem.Size = new System.Drawing.Size(271, 26);
            injectScriptIntoFrameMenuItem.Text = "Inject Script Into Frame";
            injectScriptIntoFrameMenuItem.Click += injectScriptIntoFrameMenuItem_Click;
            // 
            // methodCDPToolStripMenuItem
            // 
            methodCDPToolStripMenuItem.Name = "methodCDPToolStripMenuItem";
            methodCDPToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
            methodCDPToolStripMenuItem.Text = "Call CDP Method";
            methodCDPToolStripMenuItem.Click += methodCDPToolStripMenuItem_Click;
            // 
            // taskManagerToolStripMenuItem
            // 
            taskManagerToolStripMenuItem.Name = "taskManagerToolStripMenuItem";
            taskManagerToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
            taskManagerToolStripMenuItem.Text = "Open Task Manager";
            taskManagerToolStripMenuItem.Click += taskManagerToolStripMenuItem_Click;
            // 
            // postMessageStringMenuItem
            // 
            postMessageStringMenuItem.Name = "postMessageStringMenuItem";
            postMessageStringMenuItem.Size = new System.Drawing.Size(271, 26);
            postMessageStringMenuItem.Text = "Post Message String";
            postMessageStringMenuItem.Click += postMessageStringMenuItem_Click;
            // 
            // postMessageJsonMenuItem
            // 
            postMessageJsonMenuItem.Name = "postMessageJsonMenuItem";
            postMessageJsonMenuItem.Size = new System.Drawing.Size(271, 26);
            postMessageJsonMenuItem.Text = "Post Message JSON";
            postMessageJsonMenuItem.Click += postMessageJsonMenuItem_Click;
            // 
            // postMessageStringIframeMenuItem
            // 
            postMessageStringIframeMenuItem.Name = "postMessageStringIframeMenuItem";
            postMessageStringIframeMenuItem.Size = new System.Drawing.Size(271, 26);
            postMessageStringIframeMenuItem.Text = "Post Message String Iframe";
            postMessageStringIframeMenuItem.Click += postMessageStringIframeMenuItem_Click;
            // 
            // postMessageJsonIframeMenuItem
            // 
            postMessageJsonIframeMenuItem.Name = "postMessageJsonIframeMenuItem";
            postMessageJsonIframeMenuItem.Size = new System.Drawing.Size(271, 26);
            postMessageJsonIframeMenuItem.Text = "Post Message JSON Iframe";
            postMessageJsonIframeMenuItem.Click += postMessageJsonIframeMenuItem_Click;
            // 
            // addInitializeScriptMenuItem
            // 
            addInitializeScriptMenuItem.Name = "addInitializeScriptMenuItem";
            addInitializeScriptMenuItem.Size = new System.Drawing.Size(271, 26);
            addInitializeScriptMenuItem.Text = "Add Initialize Script";
            addInitializeScriptMenuItem.Click += addInitializeScriptMenuItem_Click;
            // 
            // removeInitializeScriptMenuItem
            // 
            removeInitializeScriptMenuItem.Name = "removeInitializeScriptMenuItem";
            removeInitializeScriptMenuItem.Size = new System.Drawing.Size(271, 26);
            removeInitializeScriptMenuItem.Text = "Remove Initialize Script";
            removeInitializeScriptMenuItem.Click += removeInitializeScriptMenuItem_Click;
            // 
            // scenarioToolStripMenuItem
            // 
            scenarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { AuthenticationMenuItem, clearBrowsingData, clientCertificateRequestedStripMenuItem, cookieManagementStripMenuItem, addRemoteObjectMenuItem, domContentLoadedMenuItem, navigateWithWebResourceRequestMenuItem, webMessageMenuItem });
            scenarioToolStripMenuItem.Name = "scenarioToolStripMenuItem";
            scenarioToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            scenarioToolStripMenuItem.Text = "Scenario";
            // 
            // AuthenticationMenuItem
            // 
            AuthenticationMenuItem.Name = "AuthenticationMenuItem";
            AuthenticationMenuItem.Size = new System.Drawing.Size(331, 26);
            AuthenticationMenuItem.Text = "Authentication";
            AuthenticationMenuItem.Click += AuthenticationMenuItem_Click;
            // 
            // clearBrowsingData
            // 
            clearBrowsingData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { ClearAllDOMStorageMenuItem, ClearAllProfileMenuItem, ClearAllSiteMenuItem, ClearAutofillMenuItem, ClearBrowsingHistoryMenuItem, ClearCookiesMenuItem, ClearDiskCacheMenuItem, ClearDownloadHistoryMenuItem });
            clearBrowsingData.Name = "clearBrowsingData";
            clearBrowsingData.Size = new System.Drawing.Size(331, 26);
            clearBrowsingData.Text = "Clear Browsing Data";
            // 
            // ClearAllDOMStorageMenuItem
            // 
            ClearAllDOMStorageMenuItem.Name = "ClearAllDOMStorageMenuItem";
            ClearAllDOMStorageMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearAllDOMStorageMenuItem.Text = "All DOM Storage";
            ClearAllDOMStorageMenuItem.Click += ClearAllDOMStorage;
            // 
            // ClearAllProfileMenuItem
            // 
            ClearAllProfileMenuItem.Name = "ClearAllProfileMenuItem";
            ClearAllProfileMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearAllProfileMenuItem.Text = "All Profile";
            ClearAllProfileMenuItem.Click += ClearAllProfileMenuItem_Click;
            // 
            // ClearAllSiteMenuItem
            // 
            ClearAllSiteMenuItem.Name = "ClearAllSiteMenuItem";
            ClearAllSiteMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearAllSiteMenuItem.Text = "All Site";
            ClearAllSiteMenuItem.Click += ClearAllSiteMenuItem_Click;
            // 
            // ClearAutofillMenuItem
            // 
            ClearAutofillMenuItem.Name = "ClearAutofillMenuItem";
            ClearAutofillMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearAutofillMenuItem.Text = "Autofill";
            ClearAutofillMenuItem.Click += ClearAutofillMenuItem_Click;
            // 
            // ClearBrowsingHistoryMenuItem
            // 
            ClearBrowsingHistoryMenuItem.Name = "ClearBrowsingHistoryMenuItem";
            ClearBrowsingHistoryMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearBrowsingHistoryMenuItem.Text = "Browsing History";
            ClearBrowsingHistoryMenuItem.Click += ClearBrowsingHistory;
            // 
            // ClearCookiesMenuItem
            // 
            ClearCookiesMenuItem.Name = "ClearCookiesMenuItem";
            ClearCookiesMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearCookiesMenuItem.Text = "Cookies";
            ClearCookiesMenuItem.Click += ClearCookies;
            // 
            // ClearDiskCacheMenuItem
            // 
            ClearDiskCacheMenuItem.Name = "ClearDiskCacheMenuItem";
            ClearDiskCacheMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearDiskCacheMenuItem.Text = "Disk Cache";
            ClearDiskCacheMenuItem.Click += ClearDiskCache;
            // 
            // ClearDownloadHistoryMenuItem
            // 
            ClearDownloadHistoryMenuItem.Name = "ClearDownloadHistoryMenuItem";
            ClearDownloadHistoryMenuItem.Size = new System.Drawing.Size(212, 26);
            ClearDownloadHistoryMenuItem.Text = "Download History";
            ClearDownloadHistoryMenuItem.Click += ClearDownloadHistory;
            // 
            // clientCertificateRequestedStripMenuItem
            // 
            clientCertificateRequestedStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { CustomClientCertificateSelectionMenuItem, DeferredCustomCertificateDialogMenuItem });
            clientCertificateRequestedStripMenuItem.Name = "clientCertificateRequestedStripMenuItem";
            clientCertificateRequestedStripMenuItem.Size = new System.Drawing.Size(331, 26);
            clientCertificateRequestedStripMenuItem.Text = "Client Certificate Request";
            // 
            // CustomClientCertificateSelectionMenuItem
            // 
            CustomClientCertificateSelectionMenuItem.Name = "CustomClientCertificateSelectionMenuItem";
            CustomClientCertificateSelectionMenuItem.Size = new System.Drawing.Size(433, 26);
            CustomClientCertificateSelectionMenuItem.Text = "Custom Client Certificate Selection";
            CustomClientCertificateSelectionMenuItem.Click += CustomClientCertificateSelectionMenuItem_Click;
            // 
            // DeferredCustomCertificateDialogMenuItem
            // 
            DeferredCustomCertificateDialogMenuItem.Name = "DeferredCustomCertificateDialogMenuItem";
            DeferredCustomCertificateDialogMenuItem.Size = new System.Drawing.Size(433, 26);
            DeferredCustomCertificateDialogMenuItem.Text = "Deferred Custom Client Certificate Selection Dialog";
            DeferredCustomCertificateDialogMenuItem.Click += DeferredCustomCertificateDialogMenuItem_Click;
            // 
            // cookieManagementStripMenuItem
            // 
            cookieManagementStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { GetCookiesMenuItem, AddOrUpdateCookieMenuItem, DeleteCookiesMenuItem, DeleteAllCookiesMenuItem });
            cookieManagementStripMenuItem.Name = "cookieManagementStripMenuItem";
            cookieManagementStripMenuItem.Size = new System.Drawing.Size(331, 26);
            cookieManagementStripMenuItem.Text = "Cookie Management";
            // 
            // GetCookiesMenuItem
            // 
            GetCookiesMenuItem.Name = "GetCookiesMenuItem";
            GetCookiesMenuItem.Size = new System.Drawing.Size(243, 26);
            GetCookiesMenuItem.Text = "Get Cookies";
            GetCookiesMenuItem.Click += GetCookies;
            // 
            // AddOrUpdateCookieMenuItem
            // 
            AddOrUpdateCookieMenuItem.Name = "AddOrUpdateCookieMenuItem";
            AddOrUpdateCookieMenuItem.Size = new System.Drawing.Size(243, 26);
            AddOrUpdateCookieMenuItem.Text = "Add Or Update Cookie";
            AddOrUpdateCookieMenuItem.Click += AddOrUpdateCookie;
            // 
            // DeleteCookiesMenuItem
            // 
            DeleteCookiesMenuItem.Name = "DeleteCookiesMenuItem";
            DeleteCookiesMenuItem.Size = new System.Drawing.Size(243, 26);
            DeleteCookiesMenuItem.Text = "Delete Cookie";
            DeleteCookiesMenuItem.Click += DeleteCookies;
            // 
            // DeleteAllCookiesMenuItem
            // 
            DeleteAllCookiesMenuItem.Name = "DeleteAllCookiesMenuItem";
            DeleteAllCookiesMenuItem.Size = new System.Drawing.Size(243, 26);
            DeleteAllCookiesMenuItem.Text = "Delete All Cookies";
            DeleteAllCookiesMenuItem.Click += DeleteAllCookiesMenuItem_Click;
            // 
            // addRemoteObjectMenuItem
            // 
            addRemoteObjectMenuItem.Name = "addRemoteObjectMenuItem";
            addRemoteObjectMenuItem.Size = new System.Drawing.Size(331, 26);
            addRemoteObjectMenuItem.Text = "Add Remote Object";
            addRemoteObjectMenuItem.Click += addRemoteObjectMenuItem_Click;
            // 
            // domContentLoadedMenuItem
            // 
            domContentLoadedMenuItem.Name = "domContentLoadedMenuItem";
            domContentLoadedMenuItem.Size = new System.Drawing.Size(331, 26);
            domContentLoadedMenuItem.Text = "DOM Content Loaded";
            domContentLoadedMenuItem.Click += domContentLoadedMenuItem_Click;
            // 
            // navigateWithWebResourceRequestMenuItem
            // 
            navigateWithWebResourceRequestMenuItem.Name = "navigateWithWebResourceRequestMenuItem";
            navigateWithWebResourceRequestMenuItem.Size = new System.Drawing.Size(331, 26);
            navigateWithWebResourceRequestMenuItem.Text = "Navigate with WebResourceRequest";
            navigateWithWebResourceRequestMenuItem.Click += navigateWithWebResourceRequestMenuItem_Click;
            // 
            // webMessageMenuItem
            // 
            webMessageMenuItem.Name = "webMessageMenuItem";
            webMessageMenuItem.Size = new System.Drawing.Size(331, 26);
            webMessageMenuItem.Text = "Web Message";
            webMessageMenuItem.Click += webMessageMenuItem_Click;
            // 
            // audioToolStripMenuItem
            // 
            audioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleMuteStateMenuItem });
            audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            audioToolStripMenuItem.Size = new System.Drawing.Size(63, 24);
            audioToolStripMenuItem.Text = "Audio";
            // 
            // toggleMuteStateMenuItem
            // 
            toggleMuteStateMenuItem.Checked = true;
            toggleMuteStateMenuItem.CheckOnClick = true;
            toggleMuteStateMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            toggleMuteStateMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            toggleMuteStateMenuItem.Name = "toggleMuteStateMenuItem";
            toggleMuteStateMenuItem.Size = new System.Drawing.Size(214, 26);
            toggleMuteStateMenuItem.Text = "Toggle Mute State";
            toggleMuteStateMenuItem.Click += toggleMuteStateMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(133, 26);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // blockedDomainsMenuItem
            // 
            blockedDomainsMenuItem.Name = "blockedDomainsMenuItem";
            blockedDomainsMenuItem.Size = new System.Drawing.Size(359, 44);
            blockedDomainsMenuItem.Text = "Blocked Domains";
            blockedDomainsMenuItem.Click += blockedDomainsMenuItem_Click;
            // 
            // webView2Control
            // 
            webView2Control.AllowExternalDrop = true;
            webView2Control.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            webView2Control.CreationProperties = null;
            webView2Control.DefaultBackgroundColor = System.Drawing.Color.Transparent;
            webView2Control.Location = new System.Drawing.Point(16, 113);
            webView2Control.Margin = new System.Windows.Forms.Padding(2);
            webView2Control.Name = "webView2Control";
            webView2Control.Size = new System.Drawing.Size(1336, 568);
            webView2Control.Source = new Uri("https://www.bing.com/", UriKind.Absolute);
            webView2Control.TabIndex = 7;
            webView2Control.ZoomFactor = 1D;
            // 
            // linksBtn
            // 
            linksBtn.Location = new System.Drawing.Point(463, 42);
            linksBtn.Name = "linksBtn";
            linksBtn.Size = new System.Drawing.Size(94, 29);
            linksBtn.TabIndex = 8;
            linksBtn.Text = "LINKS";
            linksBtn.UseVisualStyleBackColor = true;
            linksBtn.Click += linksBtn_Click;
            // 
            // ScrapeBtn
            // 
            ScrapeBtn.Location = new System.Drawing.Point(563, 44);
            ScrapeBtn.Name = "ScrapeBtn";
            ScrapeBtn.Size = new System.Drawing.Size(94, 29);
            ScrapeBtn.TabIndex = 9;
            ScrapeBtn.Text = "SCRAPE";
            ScrapeBtn.UseVisualStyleBackColor = true;
            ScrapeBtn.Click += ScrapeBtn_Click;
            // 
            // BrowserForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(1408, 692);
            Controls.Add(ScrapeBtn);
            Controls.Add(linksBtn);
            Controls.Add(webView2Control);
            Controls.Add(btnGo);
            Controls.Add(txtUrl);
            Controls.Add(btnStop);
            Controls.Add(btnRefresh);
            Controls.Add(btnForward);
            Controls.Add(btnBack);
            Controls.Add(btnEvents);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "BrowserForm";
            Text = "BrowserForm";
            Resize += Form_Resize;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webView2Control).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private System.Windows.Forms.Button btnEvents;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Drawing.Bitmap webViewLogoBitmap;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2Control;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeWebViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createWebViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewWindowWithOptionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acceleratorKeysEnabledToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearBrowsingData;
        private System.Windows.Forms.ToolStripMenuItem clientCertificateRequestedStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cookieManagementStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem backgroundColorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whiteBackgroundColorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redBackgroundColorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blueBackgroundColorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem taskManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem methodCDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparentBackgroundColorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowExternalDropMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setUsersAgentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getDocumentTitleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getUserDataFolderMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToPDFMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portraitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem landscapeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleVisibilityMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverCertificateErrorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleCustomServerCertificateSupportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearServerCertificateErrorActionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleDefaultScriptDialogsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scenarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRemoteObjectMenuItem;
        private System.Windows.Forms.ToolStripMenuItem domContentLoadedMenuItem;
        private System.Windows.Forms.ToolStripMenuItem navigateWithWebResourceRequestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem webMessageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBrowserProcessInfoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crashBrowserProcessMenuItem;
        private System.Windows.Forms.ToolStripMenuItem crashRendererProcessMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showPerformanceInfoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleMuteStateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blockedDomainsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem injectScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem injectScriptIntoFrameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addInitializeScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeInitializeScriptMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AuthenticationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearAllDOMStorageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearAllProfileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearAllSiteMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearAutofillMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearBrowsingHistoryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearCookiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearDiskCacheMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearDownloadHistoryMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CustomClientCertificateSelectionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeferredCustomCertificateDialogMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GetCookiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AddOrUpdateCookieMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteCookiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteAllCookiesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postMessageStringMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postMessageJsonMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postMessageStringIframeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postMessageJsonIframeMenuItem;
        private System.Windows.Forms.Button linksBtn;
        private System.Windows.Forms.Button ScrapeBtn;
    }
}
