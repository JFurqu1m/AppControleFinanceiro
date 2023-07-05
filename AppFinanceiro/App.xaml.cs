using AppFinanceiro.Views;

namespace AppFinanceiro;

public partial class App : Application
{
    public App(TransactionList listPage)
    {
        InitializeComponent();

        MainPage = new NavigationPage(listPage);
    }
}
