using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamadaApi.Database
{
    public static class DBValidate
    {
        public static string SafeSql(this string s)
        {
            if(s == null)
                return "";

            var r = s.Replace("'", "´");
            r = r.Replace("\"", "&quot;");
            r = r.Replace(";", ".,");
            r = r.Replace("--", "");
            r = r.Replace("xp_", "");
            r = r.Replace("/*", "");
            r = r.Replace("*/", "");
            r = r.Replace("%", "");
            r = r.Replace("true", "");
            r = r.Replace("false", "");
            r = r.Replace("script", "");
            r = r.Replace("</", "");
            r = r.Replace("/>", "");
            r = r.Replace("table", "tabela").Replace("TABLE", "TABELA");
            r = r.Replace("delete", "apagar").Replace("DELETE", "APAGAR");
            r = r.Replace("insert", "inserir").Replace("INSERT", "INSERIR");
            r = r.Replace("update", "alterar").Replace("UPDATE", "ALTERAR");


            return r;
        }


        //Formata data para o padrão sql
        public static string DateSql(this DateTime data)
        {
            return data.Year.ToString().PadLeft(4, '0') +
                data.Month.ToString().PadLeft(2, '0') +
                data.Day.ToString().PadLeft(2, '0');
        }

        //Formata data para o padrão sql
        public static string DateSql(this string data)
        {
            if(string.IsNullOrEmpty(data))
                return "19000101";

            try
            {
                var novaData = Convert.ToDateTime(data);
                return novaData.Year.ToString().PadLeft(4, '0')
                    + novaData.Month.ToString().PadLeft(2, '0')
                    + novaData.Day.ToString().PadLeft(2, '0');
            }
            catch
            {
                return "19000101";
            }
        }

        //Formata data para o padrão SQL
        public static string ValorToSql(this decimal? valorObj)
        {
            try
            {
                if(valorObj == null)
                {
                    return "0";
                }

                var valor = Convert.ToDouble(valorObj);
                return valor.ToString().Replace(".", "").Replace(",", ".");
            }
            catch
            {
                return "0";
            }
        }


        //Formata data para o padrão Mysql
        public static string DateMySql(this DateTime data)
        {
            return data.Year.ToString().PadLeft(4, '0') + "-"
                 + data.Month.ToString().PadLeft(2, '0') + "-"
                 + data.Day.ToString().PadLeft(2, '0');
        }

        //Formata data para o padrão Mysql
        public static string DateMySql(this string data)
        {
            if(string.IsNullOrEmpty(data))
                return "0000-00-00";

            try
            {
                var novaData = Convert.ToDateTime(data);
                return novaData.Year.ToString().PadLeft(4, '0') + "-"
                    + novaData.Month.ToString().PadLeft(2, '0') + "-"
                    + novaData.Day.ToString().PadLeft(2, '0');
            }
            catch
            {
                return "0000-00-00";
            }
        }


        //Remove acentos //Extensao de string 
        public static string RemoveAcentos(this string s)
        {
            string normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new();

            for(int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if(CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        //Remove caracteres //Extensao de string 
        public static string RemoveCaracteres(this string s)
        {
            if(s == null)
                return "";

            string[] listaCaracteres = { "_", "/", "\\", "-", ".", "'", ";", "#", ":", ")", "(", " " };

            if(string.IsNullOrEmpty(s))
                return s;

            foreach(string retorno in listaCaracteres)
            {
                s = s.Replace(retorno, "");
            }

            return s;
        }


        //Formata para Zeros à esquerda
        public static string Zeros(string key, int pos)
        {
            return key.PadLeft(pos);
        }

        //Gera uma senha aleatória
        public static string GetRandomPassword(int tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; //.-@()!*
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

    }
}
