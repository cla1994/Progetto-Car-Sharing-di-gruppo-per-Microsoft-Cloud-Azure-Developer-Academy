using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Academy2023.Net.Models;
using System.Text.Json;
using System.Text;
using System.Text.Json.Nodes;

namespace Academy2023.Net.Controllers;

// [Authorize]
public class RoutesAPIController : ControllerBase
{
    private ILogger<RoutesAPIController> _logger;
    private readonly HttpClient DirectionClient;
    private readonly HttpClient MatrixDirectionClient;
    private readonly HttpClient geoCodingClient;



    public RoutesAPIController(ILogger<RoutesAPIController> logger, IHttpClientFactory ApiFactory)
    {
        _logger = logger;

        DirectionClient = ApiFactory.CreateClient("routefinder_routes");
        MatrixDirectionClient = ApiFactory.CreateClient("routefinder_distanceMatrix");
        geoCodingClient = ApiFactory.CreateClient("routefinder_geoCoding");

    }

    [HttpGet]
    public async Task<IActionResult> GetDirection(string Origin= "1600 Amphitheatre Parkway, Mountain View, CA", string Destination= "450 Serra Mall, Stanford, CA 94305, USA", string Partenza= "2014-10-02T15:01:23Z")
    {
        //Creiamo il payload(Carico) del messaggio, stile dizionario inserendo i parametri necessari.
        var payload = new
        {
            origin = new
            {
              address= Origin
            },
            destination = new
            {
                address = Destination
            },
         //   departureTime = Partenza,
            travelMode = "DRIVE"
        };

        //Serializziamo il dizionario in una stringa(Json)
        string payloadJson = JsonSerializer.Serialize(payload);
        //Creiamo l'HTTPContent, nello specifico una sua sottoclasse: StringContent. Dichiariamo che il contenuto è un json.
        StringContent stringContent = new StringContent(payloadJson, Encoding.UTF8, "application/json");
        //Inseriesco nella post l'url di riferimento e il contenuto che gli passiamo.
        HttpResponseMessage response = await DirectionClient.PostAsync(DirectionClient.BaseAddress.ToString(), stringContent);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();

        var json = JsonObject.Parse(responseContent);
        return Ok(json);
    }
    public async Task<APIResponses.CalcoloArrivo> CalcoloArrivo(string Origin = "1600 Amphitheatre Parkway, Mountain View, CA", string Destination = "450 Serra Mall, Stanford, CA 94305, USA")
    {
        //creiamo il payload per la richiesta
        var payload = new
        {
            origin = new
            {
                address = Origin
            },
            destination = new
            {
                address = Destination
            },
            travelMode = "DRIVE"
        };

        //Serializziamo il dizionario in una stringa(Json)
        string payloadJson = JsonSerializer.Serialize(payload);
        //Creiamo l'HTTPContent, nello specifico una sua sottoclasse: StringContent. Dichiariamo che il contenuto è un json.
        StringContent stringContent = new StringContent(payloadJson, Encoding.UTF8, "application/json");
        //Inseriesco nella post l'url di riferimento e il contenuto che gli passiamo.
        HttpResponseMessage response = await DirectionClient.PostAsync(DirectionClient.BaseAddress.ToString(), stringContent);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        //creiamo il json della risposta
        JsonNode? json = JsonObject.Parse(responseContent);
        //{"routes":[{"distanceMeters":12004,"duration":"961s"}]}
        // selezioniamo dal json l'array routes/accediamo

        JsonArray routes = json["routes"].AsArray();
        // "distanceMeters": 252727,
        // "duration": "9732s"
        // json["routes"][0]["distanceMeters"] modo alternativo
        // selezioniamo i dati che ci servono dal json
        string DistanceMeters = routes[0]["distanceMeters"].ToString();
        string Duration = routes[0]["duration"].ToString();

        // data la classe apiresponses.calcoloarrivo, assegniamo i risultati
        APIResponses.CalcoloArrivo result = new APIResponses.CalcoloArrivo
        {
            Distances = double.Parse(DistanceMeters)/1000,
            Durations = Int32.Parse(Duration.TrimEnd('s'))
        };

        return result;
    }

    [HttpGet]
    public async Task<IActionResult> GetMatrixDirection(List<String> Origins = null, List<String> Destinations = null)
    {
        if (Origins.Count() == 0 || Destinations.Count() == 0)
        {
            //Mock data test
            Origins = new List<String>();
            Origins.Add("Via Venturi,30 Pieve di Cento, BO");
            Origins.Add("Via Marietti Milano");

            Destinations = new List<String>();
            Destinations.Add("Via San Michele degli Scalzi 50 Pisa, PI");
            Destinations.Add("Via Francesco Rismondo 29 Pisa, PI");
        }

        //Creo il payload in base alla documentazione di https://developers.google.com/maps/documentation/routes/compute_route_matrix?hl=it
        StringBuilder sb= new StringBuilder();
        sb.Append("[ ");

        for(int i=0; i < Origins.Count; i++)
        {
            sb.Append(new Origin(Origins[i]).ToString()); 
            if (i < Origins.Count - 1)
            {
                sb.Append(",");
            }
        }

        sb.Append(" ]");
        string origins = sb.ToString();
        sb.Clear();

        sb.Append("[ ");

        for (int i = 0; i < Destinations.Count; i++)
        {
            sb.Append(new Destination(Destinations[i]).ToString());
            if (i < Destinations.Count - 1)
            {
                sb.Append(",");
            }
        }
        sb.Append(" ]");
        string destinations = sb.ToString();

        string payload = "{ " + $"\"origins\": {origins}, \"destinations\": {destinations}, \"travelMode\": \"DRIVE\", \"routingPreference\": \"TRAFFIC_AWARE\"" + " }";

        //Una volta creato il payload lo trasformo in JSON e lo passo al client insieme all'header

        //Creiamo l'HTTPContent, nello specifico una sua sottoclasse: StringContent. Dichiariamo che il contenuto è un json.
        StringContent stringContent = new StringContent(payload); // , Encoding.UTF8, "application/json");
        HttpResponseMessage response = await MatrixDirectionClient.PostAsync(MatrixDirectionClient.BaseAddress,stringContent);
        Console.WriteLine(response.ReasonPhrase);
        Console.WriteLine(response.Content);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        var json = JsonObject.Parse(responseContent);
        return Ok(json);
    }
    public async Task<APIResponses.CalcoloArrivoV2> CalcoloMatrice(List<String> Origins = null, List<String> Destinations = null) //return string origin destination durata minore
    {
        //Creo liste di default
        if (Origins.Count() == 0 || Destinations.Count() == 0)
        {
            //Mock data test
            Origins = new List<String>();
            Origins.Add("Via Venturi,30 Pieve di Cento, BO");
            Origins.Add("Via Marietti Milano");

            Destinations = new List<String>();
            Destinations.Add("Via San Michele degli Scalzi 50 Pisa, PI");
            Destinations.Add("Via Francesco Rismondo 29 Pisa, PI");
        }

        //Creo il payload
        StringBuilder sb = new StringBuilder();
        sb.Append("[ ");

        for (int i = 0; i < Origins.Count; i++)
        {
            sb.Append(new Origin(Origins[i]).ToString());
            if (i < Origins.Count - 1)
            {
                sb.Append(",");
            }
        }

        sb.Append(" ]");
        string origins = sb.ToString();
        sb.Clear();

        sb.Append("[ ");

        for (int i = 0; i < Destinations.Count; i++)
        {
            sb.Append(new Destination(Destinations[i]).ToString());
            if (i < Destinations.Count - 1)
            {
                sb.Append(",");
            }
        }
        sb.Append(" ]");
        string destinations = sb.ToString();

        string payload = "{ " + $"\"origins\": {origins}, \"destinations\": {destinations}, \"travelMode\": \"DRIVE\", \"routingPreference\": \"TRAFFIC_AWARE\"" + " }";

        //Una volta creato il payload lo trasformo in JSON e lo passo al client insieme all'header

        //Creiamo l'HTTPContent, nello specifico una sua sottoclasse: StringContent. Dichiariamo che il contenuto è un json.
        StringContent stringContent = new StringContent(payload); // , Encoding.UTF8, "application/json");
        HttpResponseMessage response = await MatrixDirectionClient.PostAsync(MatrixDirectionClient.BaseAddress, stringContent);
        Console.WriteLine(response.ReasonPhrase);
        Console.WriteLine(response.Content);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        var json = JsonObject.Parse(responseContent);

        //Creiamo la matrice
        APIResponses.CalcoloMatrice responseMatrix=new APIResponses.CalcoloMatrice(Origins.Count(),Destinations.Count());
        responseMatrix.Origins = Origins.ToArray();
        responseMatrix.Destinations=Destinations.ToArray();
        JsonArray rides = json.AsArray();
        int originIndex, destinationIndex;
        //Popolo la matrice
        for(int i=0;i<rides.Count();i++)
        {
            originIndex=Int32.Parse(rides[i]["originIndex"].ToString());
            destinationIndex = Int32.Parse(rides[i]["destinationIndex"].ToString());
            responseMatrix.Matrix[originIndex, destinationIndex] = new APIResponses.CalcoloArrivo
            {
                Durations= Int32.Parse(rides[i]["duration"].ToString().TrimEnd('s')),
                Distances = Double.Parse(rides[i]["distanceMeters"].ToString())/1000
            };
        }
        //Restituisco il tragitto con distanza minore (In termini di s)
        return responseMatrix.FindMin();
    }

    public async Task<APIResponses.Coordinates> GetCoordinate(string Origin = "Via Venturi 30 Pieve di Cento Bo")
    {
        //Richiesta a google
        HttpResponseMessage response = await geoCodingClient.GetAsync(geoCodingClient.BaseAddress.ToString()+"?address="+Origin+"&key=AIzaSyAyfiXOt2c92VbTeeErK6fsDTSlpMwttIY");
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();
        var json = JsonObject.Parse(responseContent);
        var content=json["results"].AsArray();
        //Metto la risposta ricevuta nell'oggetto Coordinates
        APIResponses.Coordinates result = new APIResponses.Coordinates()
        {
            lat = Double.Parse(content[0]["geometry"]["location"]["lat"].ToString(), System.Globalization.CultureInfo.InvariantCulture),
            lon = Double.Parse(content[0]["geometry"]["location"]["lng"].ToString(), System.Globalization.CultureInfo.InvariantCulture)
        };
        //Restituisco il risultato
        return result;
    } 
}