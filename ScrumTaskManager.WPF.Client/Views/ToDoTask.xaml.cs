using Microsoft.Extensions.DependencyInjection;
using ScrumTaskManager.WPF.Client.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScrumTaskManager.WPF.Client.Views
{
    /// <summary>
    /// Interaction logic for ToDoTask.xaml
    /// </summary>
    public partial class ToDoTask : UserControl
    {
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ToDoTask), new PropertyMetadata(default(object)));

        private TasksViewModel _viewModel;

        public ToDoTask()
        {
            InitializeComponent();
            _viewModel = App.Current.Services.GetService<TasksViewModel>();
        }

        private void ToDoTask_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _viewModel.OpenTaskCommand.Execute(DataContext);
        }
    }
}
