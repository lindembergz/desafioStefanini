using System;

public class ContaBancaria
{
    public int Numero { get; }
    public string Titular { get; set; }
    private decimal _saldo;
    private const decimal TaxaSaque = 3.50m;

    public ContaBancaria(int numero, string titular, decimal depositoInicial = 0)
    {
        Numero = numero;
        Titular = titular;
        _saldo = depositoInicial;
    }

    public decimal Saldo
    {
        get { return _saldo; }
    }

    public void Deposito(decimal valor)
    {
        if (valor > 0)
        {
            _saldo += valor;
        }
        else
        {
            Console.WriteLine("O valor do depósito deve ser positivo.");
        }
    }

    public void Saque(decimal valor)
    {
        if (valor > 0)
        {
            _saldo -= (valor + TaxaSaque);
        }
        else
        {
            Console.WriteLine("O valor do saque deve ser positivo.");
        }
    }

    public override string ToString()
    {
        return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {_saldo:F2}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Entre com o número da conta: ");
        int numero = int.Parse(Console.ReadLine());

        Console.Write("Entre com o titular da conta: ");
        string titular = Console.ReadLine();

        Console.Write("Haverá depósito inicial (s/n)? ");
        char opcaoDepositoInicial = char.Parse(Console.ReadLine().ToLower());

        ContaBancaria conta;

        if (opcaoDepositoInicial == 's')
        {
            Console.Write("Entre com o valor de depósito inicial: ");
            decimal depositoInicial = decimal.Parse(Console.ReadLine());
            conta = new ContaBancaria(numero, titular, depositoInicial);
        }
        else
        {
            conta = new ContaBancaria(numero, titular);
        }

        Console.WriteLine("\nDados da conta:");
        Console.WriteLine(conta);

        Console.Write("\nEntre com um valor para depósito: ");
        decimal valorDeposito = decimal.Parse(Console.ReadLine());
        conta.Deposito(valorDeposito);

        Console.WriteLine("\nDados da conta atualizados:");
        Console.WriteLine(conta);

        Console.Write("\nEntre com um valor para saque: ");
        decimal valorSaque = decimal.Parse(Console.ReadLine());
        conta.Saque(valorSaque);

        Console.WriteLine("\nDados da conta atualizados:");
        Console.WriteLine(conta);
    }
}
