using dnSpy.Contracts.App;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace dnSpy_update_checker {
    /// <summary>
    /// Interaction logic for frmUpdate.xaml
    /// </summary>
    public partial class frmUpdate : UserControl {
        public frmUpdate() {
            InitializeComponent();

            this.Loaded += FrmUpdate_Loaded;
        }

        private void FrmUpdate_Loaded(object sender, RoutedEventArgs e) {
            var list = new List<UpdateItem>();
            list.Add(new UpdateItem() { Value = "Hello world" });
            list.Add(new UpdateItem() { Value = "Hello world2" });
            list.Add(new UpdateItem() { Value = "Hello world3" });

            lbUpdateItems.ItemsSource = list;

        }

        private void btnGo_Click(object sender, RoutedEventArgs e) {
            var selected = string.Join("\n", ((List<UpdateItem>)lbUpdateItems.ItemsSource).Where(i => i.IsChecked));
            MsgBox.Instance.Show(selected);
        }
    }

    class UpdateItem : INotifyPropertyChanged {

        private string value;
        private bool isChecked;

        public string Value {
            get { return value; }
            set {
                this.value = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsChecked {
            get { return isChecked; }
            set {
                this.isChecked = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
