using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ControlePJS
{
    public static class Executar
    {
        public static List<T> AbrirBanco<T>(string Arquivo)
        {
            string filePath = Environment.CurrentDirectory + "/beta/" + Arquivo + ".proj";
            try
            {
                var text = File.ReadAllText(filePath);
                var retorno = JsonConvert.DeserializeObject<List<T>>(text);
                return retorno;
            }
            catch
            {
                SalvarBanco<T>(new List<T>(), Arquivo);
                return new List<T>();
            }

        }
        public static void SalvarBanco<T>(List<T> Lista, string Arquivo)
        {
        Inicio:
            try
            {
                string filePath = Environment.CurrentDirectory + "/beta/" + Arquivo + ".proj";
                var serializer = JsonConvert.SerializeObject(Lista);
                File.WriteAllText(filePath, serializer);
            }
            catch
            {
                goto Inicio;
            }
        }
    }
}
