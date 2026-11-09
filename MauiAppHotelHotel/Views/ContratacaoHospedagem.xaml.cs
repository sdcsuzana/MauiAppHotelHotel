using MauiAppHotelHotel.Models;

namespace MauiAppHotelHotel.Views;

public partial class ContratacaoHospedagem : ContentPage
{
    // Tornamos o campo 'App?' seguro para evitar avisos de nulidade
    App? PropriedadesApp;

    public ContratacaoHospedagem()
    {
        InitializeComponent();

        // Obtém a instância de App garantindo que não é nula
        PropriedadesApp = Application.Current as App;

        if (PropriedadesApp != null)
        {
            pck_quarto.ItemsSource = PropriedadesApp.lista_quartos;
        }

        // Define os limites do Check-in de forma segura (sem estourar no mês de Dezembro)
        dtpck_checkin.MinimumDate = DateTime.Now;
        dtpck_checkin.MaximumDate = DateTime.Now.AddMonths(1);

        // Trata a data de check-in caso venha como nulo (Safe Navigation)
        DateTime checkinDate = dtpck_checkin.Date ?? DateTime.Now;

        dtpck_checkout.MinimumDate = checkinDate.AddDays(1);
        dtpck_checkout.MaximumDate = checkinDate.AddMonths(6);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Valida se o usuário selecionou um quarto antes de continuar
            if (pck_quarto.SelectedItem == null)
            {
                await DisplayAlertAsync("Atenção", "Por favor, selecione um quarto.", "OK");
                return;
            }

            Hospedagem h = new Hospedagem
            {
                suite = (Quarto)pck_quarto.SelectedItem,
                QntAdultos = Convert.ToInt32(stp_adultos.Value),
                QntCriancas = Convert.ToInt32(stp_criancas.Value),
                DataCheckIn = dtpck_checkin.Date ?? DateTime.Now,
                DataCheckOut = dtpck_checkout.Date ?? DateTime.Now.AddDays(1)
            };

            // Navega para a tela de resumo passando os dados criados
            await Navigation.PushAsync(new HospedagemContratada
            {
                BindingContext = h
            });
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Ops", ex.Message, "OK");
        }
    }

    private void dtpck_checkin_DateSelected(object sender, DateChangedEventArgs e)
    {
        DatePicker elemento = (DatePicker)sender;

        // Trata o valor nulo da data selecionada
        DateTime data_selecionada_checkin = elemento.Date ?? DateTime.Now;

        dtpck_checkout.MinimumDate = data_selecionada_checkin.AddDays(1);
        dtpck_checkout.MaximumDate = data_selecionada_checkin.AddMonths(6);
    }
}