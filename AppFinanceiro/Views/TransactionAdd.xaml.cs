using AppFinanceiro.Models;
using AppFinanceiro.Properties.Libraries.Utils.FixBugs;
using AppFinanceiro.Repositories;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Platform;
using System.Text;

namespace AppFinanceiro.Views;

public partial class TransactionAdd : ContentPage
{
    private ITransactionRepository _repository;
    public TransactionAdd(ITransactionRepository repository)
    {
        InitializeComponent();
        _repository = repository;
    }

    private void TapGestureRecognizer_Tapped_ToClose(object sender, TappedEventArgs e)
    {
        KeyboardFixBugs.HideKeyboard();
        Navigation.PopModalAsync();

    }

    private void OnButtonClicked_Save(object sender, EventArgs e)
    {
        if (!IsValidData())
            return;

        SaveTransactionInDataBase();

        KeyboardFixBugs.HideKeyboard();
        Navigation.PopModalAsync();

        WeakReferenceMessenger.Default.Send<string>("Teste");

    }

    private void SaveTransactionInDataBase()
    {
        Transaction transaction = new Transaction()
        {
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
            Name = EntryName.Text,
            Date = DatePickerDate.Date,
            Value = Math.Abs(double.Parse(EntryValue.Text))
        };

        _repository.Add(transaction);
    }

    private bool IsValidData()
    {
        bool valid = true;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo 'Nome' deve ser preenchido!");
            valid = false;
        }

        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O campo 'Valor' deve ser preenchido!");
            valid = false;
        }
        double result;

        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            sb.AppendLine("O campo 'Valor' é Inválido!");
            valid = false;
        }

        if (!valid)
        {
            LabelError.Text = sb.ToString();
            LabelError.IsVisible = true;
        }

        return valid;
    }
}