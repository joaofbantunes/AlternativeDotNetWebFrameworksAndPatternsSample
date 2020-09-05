using System;
using System.Net;
using System.Text.Json;
using AltApi.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AltApi.Feather
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = WebApplication.Create(args);

            app.MapGet("/sample", async ctx =>
            {
                var output = new SampleOutputModel
                {
                    SomeId = Guid.NewGuid(),
                    SomeDescription = nameof(SampleOutputModel.SomeDescription),
                    SomeDate = DateTime.UtcNow,
                    SomeInnerModel = new SampleInnerModel
                    {
                        SomeInnerId = Guid.NewGuid(),
                        SomeInnerDate = DateTime.UtcNow,
                        SomeInnerDescription = nameof(SampleInnerModel.SomeInnerDescription)
                    }
                };

                await ctx.Response.WriteAsync(JsonSerializer.Serialize(output));
                ctx.Response.StatusCode = (int) HttpStatusCode.OK;
            });

            app.MapPost("/sample", async ctx =>
            {
                var input = await ctx.Request.ReadJsonAsync<SampleInputModel>();
                var output = new SampleOutputModel
                {
                    SomeId = input.SomeId,
                    SomeDescription = input.SomeDescription,
                    SomeDate = input.SomeDate,
                    SomeInnerModel = new SampleInnerModel
                    {
                        SomeInnerId = input.SomeInnerModel.SomeInnerId,
                        SomeInnerDate = input.SomeInnerModel.SomeInnerDate,
                        SomeInnerDescription = input.SomeInnerModel.SomeInnerDescription
                    }
                };

                await ctx.Response.WriteJsonAsync(output);
                ctx.Response.StatusCode = (int) HttpStatusCode.OK;
            });

            app.Run();
        }
    }
}