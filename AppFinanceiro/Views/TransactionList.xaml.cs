
using AppFinanceiro.Models;
using AppFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppFinanceiro.Views;

public partial class TransactionList : ContentPage
{
    private ITransactionRepository _repository;

    public TransactionList(ITransactionRepository repository)
    {
        this._repository = repository;

        InitializeComponent();
        Reload();

        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });
    }

    private void Reload()
    {
        var items = _repository.GetAll();
        CollectionViewTransactions.ItemsSource = _repository.GetAll();

        double income =  items.Where(x => x.Type == Models.TransactionType.Income).Sum(a => a.Value);
        double expense =  items.Where(x => x.Type == Models.TransactionType.Expenses).Sum(a => a.Value);

        double balance = income - expense;

        lblIncome.Text = income.ToString("C");
        lblExpense.Text = expense.ToString("C");
        lblBalance.Text = balance.ToString("C");

    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs e)
    {
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();
        Navigation.PushModalAsync(transactionAdd);
    }

   
    private void TapGestureRecognizer_Tapped_ToTransactionsEdit(object sender, TappedEventArgs e)
    {
        var grid =(Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEdtit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdtit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdtit);
    }

    private async void TapGestureRecognizer_Tapped_ToDelete(object sender, TappedEventArgs e)
    {
        await AnimationBorder((Border)sender, true);
         bool result = await App.Current.MainPage.DisplayAlert("Excluir", "Tem certeza que deseja excluir ? ", "Sim", "Não");

        if (result)
        {
            Transaction transaction = (Transaction)e.Parameter;
            _repository.Delete(transaction);

            Reload();
        }
        else
        {
            await AnimationBorder((Border)sender, false);
        }
    }

    private string _labelOriginalText;

    private async Task AnimationBorder(Border border, bool IsDeleteAnimation)
    {
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
        {
            _labelOriginalText = label.Text;

            await border.RotateYTo(90, 500);

            border.BackgroundColor = Colors.Red;
            
            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 500);
        }
        else
        {
            await border.RotateYTo(90, 500);

            border.BackgroundColor = Colors.White;
            label.TextColor = Colors.Black;
            label.Text = _labelOriginalText;

            await border.RotateYTo(0, 500);
        }
    }
}