open System
open Suave
open Suave.Operators
open Suave.Filters
open System.Text.Json

type SampleInnerModel = { SomeInnerId: Guid; SomeInnerDescription: string; SomeInnerDate: DateTime }

type SampleOutputModel = { SomeId: Guid; SomeDescription : string; SomeDate : DateTime; SomeInnerModel : SampleInnerModel }

type SampleInputModel = { SomeId: Guid; SomeDescription : string; SomeDate : DateTime; SomeInnerModel : SampleInnerModel }

let toJson (payload: 'a) =
    JsonSerializer.Serialize(payload)

let fromJson (request: HttpRequest) =
    JsonSerializer.Deserialize(request.rawForm |> System.Text.ASCIIEncoding.UTF8.GetString)

// didn't really need the httpContext, but adding it to make the function reevaluate and return different ids and dates (p.s. I don't know what I'm doing in F# 😛)
let getHandler httpContext =
    let output = { 
          SampleOutputModel.SomeId = Guid.NewGuid();
          SomeDescription = "SomeDescription";
          SomeDate = DateTime.UtcNow;
          SomeInnerModel = {
              SomeInnerId = Guid.NewGuid();
              SomeInnerDescription = "SomeInnerDescription";
              SomeInnerDate = DateTime.UtcNow
      }}
    Successful.OK (toJson output) httpContext

let postHandler =
    request(fun request -> 
        let input = request |> fromJson
        { 
            SampleOutputModel.SomeId = input.SomeId;
            SomeDescription = input.SomeDescription;
            SomeDate = input.SomeDate;
            SomeInnerModel = {
                SomeInnerId = input.SomeInnerModel.SomeInnerId;
                SomeInnerDescription = input.SomeInnerModel.SomeInnerDescription;
                SomeInnerDate = input.SomeInnerModel.SomeInnerDate
        }}
        |> toJson
        |> Successful.OK
    )

let app =
  choose [
    GET >=> choose [
      path "/sample" >=> getHandler
    ]
    POST >=> choose [      
     path "/sample" >=> postHandler
    ]
  ]


let serverConfig = 
  { defaultConfig with bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" 5000 ] }

[<EntryPoint>]
let main argv =
  startWebServer serverConfig app
  0