using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
namespace 
{
    /// <summary>
    /// Classe estática com métodos de validação de cadastro base (CPF, Email, Celular, etc)
    /// </summary>
    public static class ValidacaoCadastro
    {
        private static readonly string RegistroGeralCep = @"(^[0-9]{5}[0-9]{3})";
        private static readonly string RegistroGeralEmail = @"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$";
        private static readonly string RegistroGeralTelefoneCelular = @"^(?:[0-9]{8})$|^((?:[2-5]|9[0-9])[0-9]{3}\-?[0-9]{4})$";
        /// <summary>
        /// Recebe uma string contendo o CPF e retorna se é válido ou não.
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaCPF(string cpf)
        {
            if (cpf != null)
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                cpf = cpf.Trim().Replace(".", "").Replace("-", "");
                if (cpf.Length != 11)
                    return false;
                for (int j = 0; j < 10; j++)
                    if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                        return false;
                string tempCpf = cpf.Substring(0, 9);
                int soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                int resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                string digito = resto.ToString();
                tempCpf = tempCpf + digito;
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();
                return cpf.EndsWith(digito);
            }
            else
                return true;
        }
        /// <summary>
        /// Recebe uma string contendo o CNPJ e retorna se é válido ou não.
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaCNPJ(string cnpj)
        {
            if (cnpj != null)
            {
                int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
                if (cnpj.Length != 14)
                    return false;
                string tempCnpj = cnpj.Substring(0, 12);
                int soma = 0;
                for (int i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
                int resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                string digito = resto.ToString();
                tempCnpj = tempCnpj + digito;
                soma = 0;
                for (int i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;
                digito = digito + resto.ToString();
                return cnpj.EndsWith(digito);
            }
            else
                return true;
        }
        /// <summary>
        /// Recebe um int? contendo o DDD e retorna se é válido ou não.
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaDDD(int? ddd)
        {
            if (ddd != null)
            {
                Regex rg = new Regex(@"^(?:[14689][1-9]|2[12478]|3[1234578]|5[1345]|7[134579])$");
                if (rg.IsMatch(ddd.ToString()))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Recebe um long? contendo o número do telefone ou do celular (sem DDD) e retorna se é válido ou não.
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaTelefoneECelular(long? telCel)
        {
            var RegistroGeralTelefoneCelularRegex = new Regex(RegistroGeralTelefoneCelular);
            if (telCel != null && RegistroGeralTelefoneCelularRegex.IsMatch(telCel.ToString()))
                return true;
            return false;
        }
        /// <summary>
        /// Recebe uma string contendo o e-mail e retorna se é válido ou não.
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaEmail(string email)
        {
            if (email != null)
            {
                var RegistroGeralEmailRegex = new Regex(RegistroGeralEmail);
                if (RegistroGeralEmailRegex.IsMatch(email))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Recebe uma string contendo o número do CEP e retorna se o valor é válido ou não
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaCep(string cep)
        {
            if (!string.IsNullOrEmpty(cep))
            {
                cep = cep.Contains("-") ? cep.Replace("-", "") : cep;
                if (cep.Length == 8)
                {
                    var RegistroGeralCepRegex = new Regex(RegistroGeralCep);
                    // Não permitir alfanumericos e também strings com caracteres repetidos... exemplo: "00000000"
                    if (cep.All(char.IsDigit) && !cep.All(c => c == cep[0]) && RegistroGeralCepRegex.IsMatch(cep.ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Recebe uma string contendo a unidade federativa (UF) e retorna se é válida ou não.
        /// <br><br>
        /// Retorno: <b>bool</b>
        /// </br></br>
        /// </summary>
        public static bool ValidaUF(string uf)
        {
            List<string> ufs = new List<string>() { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "GO", "ES", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SP", "SC", "SE", "TO" };
            if (ufs.Contains(uf?.ToUpper()))
                return true;
            return false;
        }
    }
}
