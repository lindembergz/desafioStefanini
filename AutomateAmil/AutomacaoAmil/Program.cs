using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using OpenQA.Selenium.Edge;
using System.Diagnostics;

class Program
{

    static bool ComModal = true;
    static void Main(string[] args)
    {
        // Configura o caminho onde o msedgedriver.exe está localizado
        var driverPath = @"C:\Lindemberg\AutomateAmil\edgedriver_win64";
        //var driverPath = @"C:\Lindemberg\AutomateAmil\AutomacaoAmil\bin\Debug\net7.0";

        // Configuração do Edge com ajustes
        EdgeOptions options = new EdgeOptions();
        //ChromeOptions options = new ChromeOptions();
        options.AddArgument("--start-maximized");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--remote-debugging-port=9222");
        options.AddArgument("--disable-gpu");
        options.AddArgument("--disable-d3d11");
        options.AddArgument("--ignore-certificate-errors");
        options.AddArgument("--disable-blink-features=AutomationControlled");
        options.AddArgument("--guest");
        options.AddArgument("--enable -unsafe-swiftshader");
        options.AddArgument("--disable-features=EdgeUserTopicOnUrlProtobuf");
        options.AddArgument("--disable-extensions");

        // Inicialização do WebDriver dentro de uma estrutura 'using' para garantir o fechamento
        using (IWebDriver driver = new EdgeDriver(driverPath, options))
        //using (IWebDriver driver = new ChromeDriver(driverPath, options))
        {
            // Inicialização do WebDriverWait com timeout de 50 segundos
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3000));

            try
            {
                // Redirecionar para a nova URL
                driver.Navigate().GoToUrl("https://credenciado.amil.com.br/login");
                Console.WriteLine("Navegador iniciou com sucesso.");

                // Espera explícita para o carregamento da página
                wait.Until(d => d.FindElement(By.TagName("body")));

                AceitarCookies(wait);               

                // Tentar fechar a janela modal informativo
                FecharModal(wait);
                // Thread.Sleep(3000);
                login(wait);

                // Esperar o redirecionamento para a página de comunicados
                wait.Until(d => d.Url.Contains("/institucional/comunicados"));
                Console.WriteLine("Redirecionado para a página de comunicados.");

                // Espera adicional para garantir que o menu foi carregado
                wait.Until(d => d.FindElement(By.Id("menu-usuario")));

                // Localizar o item de menu "Faturamento" pelo texto
                NavegarParaFaturamento(wait);

                Task.Delay(1000);

                AplicarFiltros(wait);

                Task.Delay(1000);
      

                wait.Until(d => d.FindElement(By.CssSelector("table.resultado-guia-tabela")));
                Console.WriteLine("Resultados da pesquisa carregados.");

                // Passo 3: Processar a tabela de resultados
                List<string> guiasParaProcessar = ColetarGuiasParaProcessar(driver, wait);
                Console.WriteLine($"Total de guias para processar: {guiasParaProcessar.Count}");

                foreach (var numeroGuia in guiasParaProcessar)
                {
                    Console.WriteLine($"\nProcessando a guia: {numeroGuia}");
                    ProcessarGuia(driver, wait, numeroGuia);
                }

                Console.WriteLine("\nProcessamento concluído para todas as guias.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            finally
            {
                // Esperar antes de fechar o navegador para visualização dos resultados
                Console.WriteLine("Pressione qualquer tecla para encerrar o navegador.");
                Console.ReadKey();

                // Fechar o navegador
                driver.Quit();
            }
        }

        static List<string> ColetarGuiasParaProcessar(IWebDriver driver, WebDriverWait wait)
        {
            List<string> guias = new List<string>();

            try
            {
                IWebElement tabela = wait.Until(d => d.FindElement(By.CssSelector("table.resultado-guia-tabela tbody")));
                IReadOnlyCollection<IWebElement> linhas = tabela.FindElements(By.TagName("tr"));

                foreach (var linha in linhas)
                {
                    try
                    {
                        // Verificar se a linha possui <as-tooltip accessibility="Usuário">
                        IWebElement acoesTd = linha.FindElement(By.CssSelector("td.acoes.tour-acoes"));
                        IReadOnlyCollection<IWebElement> tooltipUsuarios = acoesTd.FindElements(By.XPath(".//as-tooltip[@accessibility='Usuário']"));

                        if (tooltipUsuarios.Count == 0)
                        {
                            // Extrair o "Nº guia prestador" para identificar a linha posteriormente
                            IWebElement numeroGuiaTd = linha.FindElement(By.XPath("td[@aria-label='Nº guia prestador']"));
                            string numeroGuia = numeroGuiaTd.Text.Trim();
                            guias.Add(numeroGuia);
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        // Pulando esta linha se algum elemento não for encontrado
                        continue;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                throw new Exception("Tabela de resultados não encontrada.");
            }

            return guias.Distinct().ToList(); // Evita duplicatas
        }

        static void ProcessarGuia(IWebDriver driver, WebDriverWait wait, string numeroGuia)
        {
            try
            {
                wait.Until(d => d.FindElement(By.TagName("body")));

                // Re-localizar a linha correspondente ao "Nº guia prestador"
                IWebElement tabela = wait.Until(d => d.FindElement(By.CssSelector("table.resultado-guia-tabela tbody")));
                IWebElement linha = tabela.FindElements(By.TagName("tr"))
                    .FirstOrDefault(tr => tr.FindElement(By.XPath("td[@aria-label='Nº guia prestador']")).Text.Trim() == numeroGuia);

                if (linha == null)
                {
                    Console.WriteLine($"Linha com 'Nº guia prestador' {numeroGuia} não encontrada. Pulando para a próxima.");
                    return;
                }

                // Localizar a <td> com classe "acoes tour-acoes"
                IWebElement acoesTdLocalizada = linha.FindElement(By.CssSelector("td.acoes.tour-acoes"));

                // Localizar o botão "Editar" e clicar
                IWebElement editarButton = acoesTdLocalizada.FindElement(By.CssSelector("button.editar"));

                // Scroll até o botão para garantir que está visível
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", editarButton);

                // Clicar no botão "Editar"
                editarButton.Click();
                Console.WriteLine("Botão 'Editar' clicado.");

                FecharModal(wait);


                // Espera pelo carregamento do botão "Visualizar"
                wait.Until(d => d.FindElement(By.XPath("//button[contains(@aria-label, 'Visualizar detalhes do procedimento')]")));
                Console.WriteLine("Botão 'Visualizar' encontrado.");

                // Clicar no botão "Visualizar"
                IWebElement visualizarButton = driver.FindElement(By.XPath("//button[contains(@aria-label, 'Visualizar detalhes do procedimento')]"));
                visualizarButton.Click();
                Console.WriteLine("Botão 'Visualizar' clicado.");


                // Aguarda o carregamento da página ou modal de edição
                // Dependendo da implementação do sistema, pode ser necessário alternar para um iframe ou gerenciar janelas
                // Aqui assumimos que após clicar em "Editar", o sistema navega para uma nova página
                FecharModal(wait);

                // Aguarda a visualização carregar (assumindo que permanece na mesma janela)
                wait.Until(d => d.FindElement(By.XPath("//button[contains(text(),'Profissionais')]")));
                Console.WriteLine("Verificando existência do botão 'Profissionais'.");

                // Verificar a existência do botão "Profissionais"
                bool profissionaisExiste = false;
                try
                {
                    IWebElement profissionaisButton = driver.FindElement(By.XPath("//button[contains(text(),'Profissionais')]"));
                    profissionaisExiste = profissionaisButton.Displayed && profissionaisButton.Enabled;
                    Console.WriteLine("Botão 'Profissionais' encontrado.");
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Botão 'Profissionais' não encontrado.");
                }

                if (profissionaisExiste)
                {
                    Thread.Sleep(2000);
                    //--------------------------------------------------------------------
                    // NOVO CÓDIGO: Verificar e preencher o cbos, se necessário
                    //--------------------------------------------------------------------

                    // 1) Localizar o as-cbos-autocomplete e verificar se está vazio
                    //    Normalmente, há um <input> interno. Verifique se é disabled; se for,
                    //    precisamos clicar no botão "Editar" dentro do <as-cbos-autocomplete>.

                    IWebElement cbosAutocomplete = null;
                    string valueCbos = null;
                    try
                    {
                        // Localiza o container do <as-cbos-autocomplete>
                        cbosAutocomplete = driver.FindElement(By.CssSelector("as-cbos-autocomplete[controlname='cbos']#solicitante-cbos"));
                        // Dentro dele, deve ter um input
                        IWebElement inputCbos = cbosAutocomplete.FindElement(By.CssSelector("div.field input"));
                        valueCbos = inputCbos.GetAttribute("value")?.Trim();
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Elemento <as-cbos-autocomplete> ou seu <input> não encontrado.");
                    }

                    // 2) Se o inputCBOS estiver vazio (valueCbos for null ou String.Empty),
                    //    buscar o número do conselho (id="solicitante-numero") e preencher
                    //    "225133" se for 7587 ou "251510" caso contrário.
                    if (string.IsNullOrEmpty(valueCbos))
                    {
                        try
                        {
                            // Localiza o input do número do conselho
                            IWebElement numeroConselhoField = driver.FindElement(By.Id("solicitante-numero"));
                            string numeroConselho = numeroConselhoField.GetAttribute("value")?.Trim();

                            if (!string.IsNullOrEmpty(numeroConselho))
                            {
                                // Precisamos habilitar o input do CBO, pois ele está "disabled"
                                try
                                {
                                    // Há um <button type="button" value="editar"></button> dentro do <as-cbos-autocomplete>?
                                    IWebElement botaoEditarCbos = cbosAutocomplete.FindElement(By.CssSelector("div.field button[value='editar']"));
                                    botaoEditarCbos.Click();
                                    Console.WriteLine("Botão de edição do CBOs clicado para habilitar o input.");
                                }
                                catch (NoSuchElementException)
                                {
                                    Console.WriteLine("Botão de edição do CBOs não encontrado; não será possível alterar o valor.");
                                }

                                // Reencontrar o input pois agora deve estar habilitado
                                IWebElement inputCbos = cbosAutocomplete.FindElement(By.CssSelector("div.field input"));
                                inputCbos.Clear();
                                // Decide qual valor preencher
                                string valorParaPreencher = (numeroConselho == "7587") ? "225133" : "251510";
                                inputCbos.SendKeys(valorParaPreencher);
           
                                Task.Delay(3000);

                                Console.WriteLine($"Campo CBO preenchido com {valorParaPreencher}, conforme o número do conselho {numeroConselho}.");
                                
                            }
                        }
                        catch (NoSuchElementException)
                        {
                            Console.WriteLine("Número do conselho (solicitante-numero) não encontrado.");
                        }

                       
                    }

                    //--------------------------------------------------------------------
                    // FIM DO NOVO CÓDIGO
                    //--------------------------------------------------------------------


                    try
                    {
                        IWebElement atualizarButton = wait.Until(d => d.FindElement(By.XPath("//button[@type='submit' and @value='incluir' and contains(text(),'Atualizar')]")));
                        atualizarButton.Click();
                        Console.WriteLine("Botão 'Atualizar' clicado.");
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine("Botão 'Atualizar' não encontrado.");
                    }

                    // Esperar pela mensagem de sucesso
                    bool mensagemSucesso = false;
                    try
                    {
                        wait.Until(d => d.FindElement(By.XPath("//as-message//p[contains(text(),'Guia SADT atualizada com sucesso.')]")));
                        mensagemSucesso = true;
                        Console.WriteLine("Mensagem de sucesso encontrada.");
                    }
                    catch (WebDriverTimeoutException)
                    {
                        Console.WriteLine("Mensagem de sucesso não encontrada.");
                    }

                    if (mensagemSucesso &&  !string.IsNullOrEmpty(valueCbos))
                    {
                        // Clicar no botão "Voltar"
                        try
                        {
                            IWebElement voltarButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@class, 'voltar') and contains(text(),'Voltar')]")));
                            voltarButton.Click();
                            Console.WriteLine("Botão 'Voltar' clicado.");

                            //Thread.Sleep(2000);
                            Task.Delay(2000);

                            // Aguarda o retorno à página de resultados
                            wait.Until(d => d.FindElement(By.CssSelector("table.resultado-guia-tabela")));

                            ComModal = false;

                            Console.WriteLine("Retornou à página de resultados.");
                        }
                        catch (WebDriverTimeoutException)
                        {
                            Console.WriteLine("Botão 'Voltar' não encontrado ou não foi possível retornar à página de resultados.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Botão 'Profissionais' não existe. Nenhuma ação de atualização necessária.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Erro ao processar a guia {numeroGuia}: Elementos necessários não foram encontrados a tempo.");
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Erro ao processar a guia {numeroGuia}: Elemento não encontrado - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao processar a guia {numeroGuia}: {ex.Message}");
            }
        }        

    }

    private static void AplicarFiltros(WebDriverWait wait)
    {
        // Seleção da opção "Personalizado"
        try
        {
            // Localizar o <dd> que contém o <input> com id="PERSONALIZADO" e clicar no <label>
            IWebElement personalizadoLabel = wait.Until(d => d.FindElement(By.XPath("//dd[input[@id='PERSONALIZADO']]/label")));
            personalizadoLabel.Click();
            Console.WriteLine("Opção 'Personalizado' selecionada.");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Elemento 'Personalizado' não encontrado.");
        }
        Thread.Sleep(500);


        string[] datas;
        try
        {
            datas = File.ReadAllLines("periodo.txt");
            if (datas.Length < 2)
            {
                throw new Exception("Arquivo periodo.txt precisa ter pelo menos 2 linhas (dataInicial, dataFinal).");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Não foi possível ler as datas do arquivo. Erro: {ex.Message}");
            datas = new string[] { "02/11/2024", "02/11/2024" };
        }

        string data1 = datas[0];
        string data2 = datas[1];

        try
        {
            IWebElement dataInicialField = wait.Until(d => d.FindElement(By.Id("dataInicial")));
            dataInicialField.Clear(); // Limpa o campo antes de inserir o valor
            dataInicialField.SendKeys(data1);
            Console.WriteLine($"Campo 'dataInicial' preenchido com {data1}.");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Campo 'dataInicial' não encontrado.");
        }

        // Preencher o campo "dataFinal"
        try
        {
            IWebElement dataFinalField = wait.Until(d => d.FindElement(By.Id("dataFinal")));
            dataFinalField.Clear();
            dataFinalField.SendKeys(data2);
            Console.WriteLine($"Campo 'dataFinal' preenchido com {data2}.");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Campo 'dataFinal' não encontrado.");
        }

        Task.Delay(1000);

        // Clicar no botão "Pesquisar"
        try
        {
            IWebElement pesquisarButton = wait.Until(d => d.FindElement(By.XPath("//button[@type='submit' and contains(text(),'Pesquisar')]")));
            pesquisarButton.Click();
            Console.WriteLine("Botão 'Pesquisar' clicado com sucesso.");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Botão 'Pesquisar' não encontrado.");
        }

        // Aguarda o carregamento dos resultados da pesquisa

    }

    private static void NavegarParaFaturamento(WebDriverWait wait)
    {
        try
        {
            // XPath para localizar o link <a> que contém um <div> com o texto "Faturamento"
            IWebElement faturamentoMenuItem = wait.Until(d => d.FindElement(By.XPath("//ul[@id='menu-usuario']//div[contains(text(),'Faturamento')]/ancestor::a")));

            // Clicar no item de menu "Faturamento"
            faturamentoMenuItem.Click();
            Console.WriteLine("Menu 'Faturamento' clicado com sucesso.");            

            // Tentar fechar a janela modal informativo
            FecharModal(wait);
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Menu 'Faturamento' não encontrado.");
        }


    }

    private static void AceitarCookies(WebDriverWait wait)
    {
        // Tentar clicar no botão de aceitação de cookies
        try
        {
            wait.Until(d => d.FindElement(By.XPath("//button[contains(text(),'Aceitar') or contains(text(),'Accept')]")));


            IWebElement acceptCookiesButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(text(),'Aceitar') or contains(text(),'Accept')]")));
            acceptCookiesButton.Click();
            Console.WriteLine("Botão de aceitação de cookies clicado.");
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Botão de aceitação de cookies não encontrado.");
        }
    }

    private static void login(WebDriverWait wait)
    {
        // Preencher o campo "username" com o valor fornecido
        IWebElement usernameField = wait.Until(d => d.FindElement(By.Name("username")));
        usernameField.SendKeys("00000000000");
        Console.WriteLine("Campo 'username' preenchido.");

        // Preencher o campo "password" com o valor fornecido
        IWebElement passwordField = wait.Until(d => d.FindElement(By.Name("password")));
        passwordField.SendKeys("00000000000");
        Console.WriteLine("Campo 'password' preenchido.");

        // Clicar no botão "Entrar"
        IWebElement entrarButton = wait.Until(d => d.FindElement(By.XPath("//button[@type='submit' and contains(text(),'Entrar')]")));
        entrarButton.Click();
        Console.WriteLine("Botão 'Entrar' clicado com sucesso.");
    }

    private static void FecharModal(WebDriverWait wait)
    {
        try
        {
            Task.Delay(2000);
            if (ComModal)
            {
                IWebElement fecharModalButton = wait.Until(d => d.FindElement(By.XPath("//button[contains(@aria-label,'Finalizar')]")));
                fecharModalButton.Click();
                Console.WriteLine("Janela modal fechada.");
            }
        }
        catch (NoSuchElementException)
        {
            Console.WriteLine("Janela modal não encontrada ou já fechada.");
        }
    }
}