open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive

type SampleInnerModel = { SomeInnerId: Guid; SomeInnerDescription: string; SomeInnerDate: DateTime }

type SampleOutputModel = { SomeId: Guid; SomeDescription : string; SomeDate : DateTime; SomeInnerModel : SampleInnerModel }

type SampleInputModel = { SomeId: Guid; SomeDescription : string; SomeDate : DateTime; SomeInnerModel : SampleInnerModel }

let getHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
       let output =
           { 
                SampleOutputModel.SomeId = Guid.NewGuid();
                SomeDescription = "SomeDescription";
                SomeDate = DateTime.UtcNow;
                SomeInnerModel = {
                    SomeInnerId = Guid.NewGuid();
                    SomeInnerDescription = "SomeInnerDescription";
                    SomeInnerDate = DateTime.UtcNow
           }}
       return! negotiate output next ctx
    }
let postHandler (next: HttpFunc) (ctx: HttpContext) =
    task {
        let! input = ctx.BindModelAsync<SampleInputModel>()
        let output =
            { 
                SampleOutputModel.SomeId = input.SomeId;
                SomeDescription = input.SomeDescription;
                SomeDate = input.SomeDate;
                SomeInnerModel = {
                    SomeInnerId = input.SomeInnerModel.SomeInnerId;
                    SomeInnerDescription = input.SomeInnerModel.SomeInnerDescription;
                    SomeInnerDate = input.SomeInnerModel.SomeInnerDate
            }}
        return! negotiate output next ctx
    }

let webApp =
    choose [
        subRouteCi "/sample" (
            choose [
                GET >=>
                    choose [
                        routeCi "" >=> getHandler
                    ]
                POST >=>
                    choose [
                        routeCi "" >=> postHandler
                    ]
            ]
        )
    ]

let configureApp (app : IApplicationBuilder) =
    // Add Giraffe to the ASP.NET Core pipeline
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    |> ignore)
        .Build()
        .Run()
    0