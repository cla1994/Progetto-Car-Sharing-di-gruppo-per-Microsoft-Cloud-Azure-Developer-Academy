using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Academy2023.Net.Models
{
    public class APIResponses
    {
        public class CalcoloArrivo
        {
            public double Distances { get; set; }

            public int Durations { get; set; }

            public override string ToString()
            {
                return $"Distanza: {Distances}, durata: {Durations}";
            }
        }

        // creato per aggiungere info aggiuntive
        public class CalcoloArrivoV2 : CalcoloArrivo
        {
            public string Origine { get; set; }

            public string Destinazione { get; set; }
        }


        public class CalcoloMatrice
        {
            public int nOrigin;
            public int nDestination;
            public string[] Origins;
            public string[] Destinations;
            public CalcoloArrivo[,] Matrix;

            public CalcoloMatrice(int nOrigin, int nDestination){
                Matrix=new CalcoloArrivo[nOrigin,nDestination];
            }

 
            public CalcoloArrivoV2 FindMin()
            {
                if(Matrix == null)
                {
                    return new CalcoloArrivoV2()
                    {
                        Durations= 0,
                        Distances= 0,
                        Origine = "",
                        Destinazione = ""
                    };
                }

                CalcoloArrivoV2 result = new CalcoloArrivoV2();

                //prende tutte righe e colonne, tante coppie di valori, ordino matrice con durations e con select e firstordefault ottengo indici di riga e colonna e l'elemento
                var QueryResult = Enumerable.Range(0, Origins.Count())
                 .SelectMany(i => Enumerable.Range(0, Destinations.Count())
                 .OrderBy(j => Matrix[i,j].Durations)
                 .Select(j => new {
                     Row = i,
                     Column = j,
                     result = Matrix[i,j]                  
                 }))
                 .FirstOrDefault();

                result.Distances = QueryResult.result.Distances;

                result.Durations = QueryResult.result.Durations;

                result.Origine = Origins[QueryResult.Row];

                result.Destinazione= Destinations[QueryResult.Column];

                return result;

            }
        }
        public class Coordinates
        {
            public double lat { get; set; }
            public double lon {  get; set; }
            public override string ToString()
            {
                return lat+ "," + lon;  
            }

            public string ToJson()
            {
                return $"position = "+"{ lat: "+lat+", "+"lng: "+lon+" }";
            }
        }
    }
}
