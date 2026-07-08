using System;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows;

namespace LimpaSpooler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LogMessage("Pronto para iniciar a limpeza do spooler.");
        }

        private async void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            BtnClean.IsEnabled = false;
            TxtLog.Clear();
            LogMessage("Iniciando operação de limpeza...");

            await Task.Run(() => ExecutarLimpeza());

            BtnClean.IsEnabled = true;
        }

        private void LogMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TxtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                TxtLog.ScrollToEnd();
            });
        }

        private void ExecutarLimpeza()
        {
            string serviceName = "Spooler";
            ServiceController spoolerService = new ServiceController(serviceName);

            try
            {
                // Parar o serviço
                LogMessage("Verificando status do serviço do Spooler...");
                if (spoolerService.Status != ServiceControllerStatus.Stopped && 
                    spoolerService.Status != ServiceControllerStatus.StopPending)
                {
                    LogMessage("Parando serviço do Spooler...");
                    spoolerService.Stop();
                    spoolerService.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    LogMessage("Serviço do Spooler parado com sucesso.");
                }
                else
                {
                    LogMessage("O serviço do Spooler já estava parado.");
                }

                // Limpar arquivos
                string spoolPath = @"C:\Windows\System32\spool\PRINTERS";
                LogMessage($"Localizando a pasta da fila: {spoolPath}");
                
                if (Directory.Exists(spoolPath))
                {
                    string[] files = Directory.GetFiles(spoolPath);
                    if (files.Length == 0)
                    {
                        LogMessage("A fila de impressão já está vazia. Nenhum arquivo para excluir.");
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                            try
                            {
                                string fileName = Path.GetFileName(file);
                                LogMessage($"Excluindo {fileName}...");
                                File.Delete(file);
                            }
                            catch (Exception ex)
                            {
                                LogMessage($"ERRO ao excluir arquivo: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    LogMessage($"Aviso: Diretório não encontrado ({spoolPath}).");
                }

                // Iniciar o serviço
                LogMessage("Iniciando serviço do Spooler...");
                spoolerService.Start();
                spoolerService.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                LogMessage("Serviço do Spooler iniciado com sucesso.");

                LogMessage("OPERAÇÃO CONCLUÍDA COM SUCESSO!");
            }
            catch (System.TimeoutException)
            {
                LogMessage("ERRO: Tempo limite esgotado ao aguardar o serviço.");
            }
            catch (InvalidOperationException ex)
            {
                LogMessage($"ERRO: Problema de permissão ou serviço não encontrado. Execute como Administrador! Detalhes: {ex.Message}");
            }
            catch (Exception ex)
            {
                LogMessage($"ERRO INESPERADO: {ex.Message}");
            }
            finally
            {
                spoolerService.Dispose();
            }
        }
    }
}
