using MauiApp1TempoAgora.Models;
using MauiApp1TempoAgora.Services;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace MauiApp1TempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        
        public MainPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!IsConnectedToInternet())
                {
                    await DisplayAlert("Conexão", "Sem conexão com a internet.", "OK");
                    return;
                }

                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                            $"Longitude: {t.lon} \n" +
                            $"Nascer do sol: {t.sunrise} \n" +
                            $"Por do sol: {t.sunset} \n" +
                            $"Temperatura máxima: {t.temp_max} \n" +
                            $"Temperatura mínima: {t.temp_min} \n" +
                            $"Descrição: {t.description} \n" +
                            $"Velocidade: {t.speed} \n" +
                            $"Visibilidade: {t.visibility} \n";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = $"Não foram encontrados dados do" +
                            $"tempo para a cidade {txt_cidade.Text}";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.RequestTimeout ||
                    ex.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                    ex.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                {
                    await DisplayAlert("Erro de Conexão", "Sem conexão com o servidor." +
                        "Verifique sua internet.", "OK");
                }
                else
                {
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private bool IsConnectedToInternet()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("google.com");
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false; 
            }
        }
    }

}
