using dnSpy.Contracts.App;
using dnSpy.Contracts.Extension;
using dnSpy_update_checker.Github;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace dnSpy_update_checker
{
    [ExportExtension]
    public class ExtensionLoader : IExtension {

        [Import]
        public IAppWindow dnWindow;

        public ExtensionInfo ExtensionInfo => new ExtensionInfo() {
            ShortDescription = "Ability to check for updates on github.",
            Copyright = "Copyright 2019 DeStilleGast"
        };

        public IEnumerable<string> MergedResourceDictionaries {
            get {
                yield break;
            }
        }

        public void OnEvent(ExtensionEvent @event, object obj) {
            //if(@event == ExtensionEvent.AppLoaded) {
            //    var dialog = new Window();
            //    var updateContent = new frmUpdate();
            //    dialog.Content = updateContent;
            //    dialog.Width = updateContent.MinWidth + 25;
            //    dialog.Height = updateContent.MinHeight + 40;
            //    dialog.ShowDialog();
            //}

            if (@event == ExtensionEvent.AppLoaded) {
                new Thread(() => {
                    var githubInfo = new GithubApi().GetResult("0xd4d", "dnSpy");

                    var currentVersion = new Version(dnWindow.AssemblyInformationalVersion.Replace("v", ""));
                    var githubVersion = new Version(githubInfo.tag_name.Replace("v", ""));

                    if (currentVersion < githubVersion) {
                        dnWindow.MainWindow.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() => {
                            var askToOpenPage = MsgBox.Instance.Show($"There is a update available, you have version ({currentVersion.ToString()}) while version ({githubVersion.ToString()}) is available on Github\n\nRelease note:\n{githubInfo.body.Trim()}\n\nWould you like to open the download page ?", MsgBoxButton.Yes | MsgBoxButton.No);
                            if (askToOpenPage == MsgBoxButton.Yes) {
                                Process.Start(githubInfo.html_url);
                            }
                        }));
                    }
                }).Start();
            }
        }
    }
}
